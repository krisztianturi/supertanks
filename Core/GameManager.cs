using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperTanks.Entities;
using SuperTanks.Overlays;
using SuperTanks.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SuperTanks.Core
{
    internal class GameManager
    {
        private SpatialGrid _spatialGrid;

        private static int _edgeLeft, _edgeRight, _edgeTop, _edgeBottom;

        private List<GameObject> _objects = new List<GameObject>();
        private List<GameObject> _toRemove = new List<GameObject>();
        private List<GameObject> _toAdd = new List<GameObject>();

        private bool _isGameOver = false;
        private readonly int _tileSize = 100;


        internal static void SetEdges(int left, int right, int top, int bottom)
        {
            _edgeLeft = left;
            _edgeRight = right;
            _edgeTop = top;
            _edgeBottom = bottom;
        }

        internal GameManager(SpatialGrid _spatialGrid)
        {
            this._spatialGrid = _spatialGrid;
        }

        internal static int GetEdgeLeft() {  return _edgeLeft; }
        internal static int GetEdgeRight(int size) { return _edgeRight-size; }
        internal static int GetEdgeTop() { return _edgeTop; }
        internal static int GetEdgeBottom(int size) { return _edgeBottom-size; }

        internal void UpdateGame(GameTime gameTime)
        {
            
            foreach (GameObject o in _objects)
            {
                o.Update(gameTime, this);
            }


            foreach (var o in _toRemove)
            {
                _objects.Remove(o);
                _spatialGrid.Remove(o);
            }


            _toRemove.Clear();

            foreach (var obj in _toAdd)
            {
                _objects.Add(obj);
                _spatialGrid.Add(obj);
            }
            _toAdd.Clear();

            GameStateChange();
        }

        private void GameStateChange()
        {
            if (_isGameOver)
            {
                TheGameIsLost();
            }
            else if (InputManager.ExitPressed())
            {
                OverlayManager.RequestChange(new MenuOverlay());
                ClosingGame();
            }
        }

        internal void SetGameOver()
        {
            _isGameOver = true;
        }

        private void TheGameIsLost()
        {
            OverlayManager.RequestChange(new GameOverOverlay());
            ClosingGame();
        }

        private void ClosingGame()
        {
            _objects.Clear();
            _toAdd.Clear();
            _toRemove.Clear();
            _spatialGrid = null;
        }

        internal void Draw(Renderer renderer)
        {
            foreach (GameObject item in _objects)
            {
                item.Draw(renderer);
            }
        }

        internal void AddObjects(List<GameObject> gameObjects)
        {
            _objects.AddRange(gameObjects);
        }

        internal void AddObject(GameObject go)
        {
            _objects.Add(go);
        }

        internal void RemoveObject(GameObject o) { _toRemove.Add(o); }
        internal void UpdateObject(GameObject o) { _spatialGrid.Update(o); }

        internal void MoveProjectile(Vector2 newPosition, Projectile projectile)
        {
            if (IsCollided(newPosition, projectile))
            {
                HandleCollision(newPosition, projectile);
                if (projectile.IsRemovableOrSizeReduction(newPosition))
                {
                    _toRemove.Add(projectile);
                }
                else
                {
                    _spatialGrid.Update(projectile);
                }
            }
            else if(!IsProjectileOnEdge(newPosition, projectile))
            {
                projectile.SetVector(newPosition);
                _spatialGrid.Update(projectile);
            }

        }

        internal void HandleProjectileEdge(Vector2 newPosition, int reducedValue, Projectile projectile)
        {
            if (projectile.IsRemovableOrSizeReduction(newPosition, reducedValue))
            {
                _toRemove.Add(projectile);
            }
            else
            {
                _spatialGrid.Update(projectile);
            }
        }

        internal bool IsCollided(Vector2 vector, Projectile projectile)
        {
            Rectangle projRect = projectile.Bounds(vector);
            List<GameObject> nearby = _spatialGrid.GetNearby(projRect);

            var hitAreas = new List<Rectangle>();

            foreach (var other in nearby)
            {
                if (other is Projectile && ((Projectile)other).GetTeam() == projectile.GetTeam()) continue;
                if (other is Player && projectile.GetTeam() == Team.Player) continue;
                if (other is Enemy && projectile.GetTeam() == Team.Enemy) continue;

                Rectangle otherRect = other.Bounds(other.GetVector());

                if (otherRect.Intersects(projRect))
                {
                    return true;
                }
            }           

            return false;
        }

        private void HandleCollision(Vector2 newPosition, Projectile projectile)
        {
            Rectangle damageBox = CreateDamageBox(newPosition, projectile);

            var affected = _spatialGrid.GetNearby(damageBox);
            foreach (var target in affected)
            {
                if (target.Equals(projectile))
                {
                    continue;
                }
                Rectangle targetRect = target.Bounds(target.GetVector());
                if (targetRect.Intersects(damageBox))
                {
                    if (target.IsShootable())
                    {
                        
                        if (target is Projectile tproj)
                        {
                            if (tproj.IsRemovableOrSizeReduction(tproj.GetVector()))
                            {
                                _toRemove.Add(tproj);
                            }
                            else
                            {
                                _spatialGrid.Update(tproj);
                            }
                        }

                        if (target is Player player)
                        {
                            if (projectile.GetTeam() == Team.Player) continue;
                            else
                            {
                                player.HitHandle(this);
                            }
                        }

                        if (target is Enemy enemy)
                        {
                            if (projectile.GetTeam() == Team.Enemy) continue;
                            else
                            {
                                enemy.HitHandle(this);
                            }
                        }

                        if (target is ObjectArea area)
                        {
                            if (area.GetAreaType() == AreaType.EAGLE)
                            {
                                _isGameOver = true;
                                _toRemove.Add(target);
                            }
                            else
                            {
                                if (area.IsRemovable(targetRect, damageBox, projectile.GetDirection()))
                                {
                                    _toRemove.Add(target);
                                }
                                else
                                {
                                    _spatialGrid.Update(target);
                                }

                            }

                        }
                    }


                }

            }
        }

        private bool IsProjectileOnEdge(Vector2 newPosition, Projectile projectile)
        {
            switch (projectile.GetDirection())
            {
                case Direction.Up:
                    {
                        if (newPosition.Y < GetEdgeTop()) { HandleProjectileEdge(newPosition, GetEdgeTop() - (int)newPosition.Y, projectile); return true; }
                    }
                    break;
                case Direction.Down:
                    {
                        if (newPosition.Y > GetEdgeBottom(projectile.GetSizeY())) { HandleProjectileEdge(newPosition, (int)newPosition.Y - GetEdgeBottom(projectile.GetSizeY()), projectile); return true; }
                    }
                    break;
                case Direction.Left:
                    {
                        if (newPosition.X < GetEdgeLeft()) { HandleProjectileEdge(newPosition, GetEdgeLeft() - (int)newPosition.X, projectile); return true; }
                    }
                    break;
                case Direction.Right:
                    {
                        if (newPosition.X > GetEdgeRight(projectile.GetSizeX())) { HandleProjectileEdge(newPosition, (int)newPosition.X - GetEdgeRight(projectile.GetSizeX()), projectile); return true; }
                    }
                    break;
            }
            return false;
        }

        internal Rectangle CreateDamageBox(Vector2 newPosition, Projectile projectile)
        {
            Rectangle damageBox = new Rectangle();
            Rectangle projRect = projectile.Bounds(projectile.GetVector());

            int newPosX = (int)(newPosition.X);
            int newPosY = (int)(newPosition.Y);


            switch (projectile.GetDirection())
            {
                case Direction.Left:
                    {
                        damageBox = new Rectangle(newPosX, projRect.Center.Y -_tileSize/2 ,projRect.Left- newPosX , _tileSize);
                    }
                    break;
                case Direction.Right:
                    {
                        damageBox = new Rectangle(projRect.Right, projRect.Center.Y - _tileSize / 2, newPosX+ projectile.GetSizeX() - projRect.Right, _tileSize);
                    }
                    break;
                case Direction.Up:
                    {
                        damageBox = new Rectangle(projRect.Center.X - _tileSize / 2, newPosY, _tileSize, projRect.Top - newPosY);
                    }
                    break;
                case Direction.Down:
                    {
                        damageBox = new Rectangle(projRect.Center.X - _tileSize / 2, projRect.Bottom, _tileSize, newPosY + projectile.GetSizeY() - projRect.Bottom);
                    }
                    break;
            }
            return damageBox;
        }


        internal void Shooting(LivingObject livingObject)
        {
            int projectileSize = 26;
            int projectileLength = 25 + livingObject.GetPower()* Projectile.GetDamage();

            float basePixelX = livingObject.GetVector().X;
            float basePixelY = livingObject.GetVector().Y;

            float endPixelX = basePixelX, endPixelY = basePixelY;
            bool canCreate = true;

            int sizeX = 0, sizeY=0;
            float coordinate = 0;

            switch (livingObject.GetDirection())
            {
                case Direction.Up:
                    {
                        endPixelX += livingObject.GetSizeX() / 2 - projectileSize / 2;
                        if (endPixelY < GetEdgeTop()) canCreate = false;
                        sizeX = projectileSize;
                        sizeY = projectileLength;
                        coordinate = basePixelY;
                    }
                    break;
                case Direction.Down:
                    {
                        endPixelX += livingObject.GetSizeX() / 2 - projectileSize / 2;
                        endPixelY += livingObject.GetSizeY()-projectileLength;
                        if (endPixelY > GetEdgeBottom(projectileLength)) canCreate = false;
                        sizeX = projectileSize;
                        sizeY = projectileLength;
                        coordinate = livingObject.Bounds(livingObject.GetVector()).Bottom;
                    }
                    break;
                case Direction.Left:
                    {
                        endPixelY += livingObject.GetSizeY() / 2 - projectileSize / 2;
                        if (endPixelX < GetEdgeLeft()) canCreate = false;
                        sizeX = projectileLength;
                        sizeY = projectileSize;
                        coordinate= basePixelX;
                    }
                    break;
                case Direction.Right:
                    {
                        endPixelX += livingObject.GetSizeX() - projectileLength;
                        endPixelY += livingObject.GetSizeY() / 2 - projectileSize / 2;
                        if (endPixelX > GetEdgeRight(projectileLength)) canCreate = false;
                        sizeX = projectileLength;
                        sizeY = projectileSize;
                        coordinate = livingObject.Bounds(livingObject.GetVector()).Right;
                    }
                    break;
            }

            if (!canCreate) return;

            Team owner;
            if (livingObject is Player) owner = Team.Player;
            else owner = Team.Enemy;
            Projectile projectile = EntityFactory.CreateProjectile(new Vector2(endPixelX, endPixelY), sizeX, sizeY, livingObject.GetDirection(), owner, coordinate);
            _toAdd.Add(projectile);

        }

        internal void MoveWithClamp(Vector2 velocity, Vector2 movement, GameObject obj)
        {
            Vector2 nextPosition = obj.GetVector() + movement;

            if (velocity.X < 0)
            {
                Rectangle nextBounds = obj.Bounds(nextPosition);
                List<GameObject> nearbyObjects = _spatialGrid.GetNearby(obj.Bounds(nextPosition));
                float blockingX = float.MinValue;
                bool hasBlock = false;

                foreach (GameObject other in nearbyObjects)
                {
                    if (other == obj) continue;
                    if (!other.IsBlocking()) continue;

                    Rectangle otherBounds = other.Bounds(other.GetVector());
                    if (otherBounds.Intersects(nextBounds))
                    {
                        hasBlock = true;

                        float candidate = other.Bounds(other.GetVector()).Right;

                        if (candidate > blockingX) blockingX = candidate;
                    }
                }

                if (hasBlock)
                {
                    obj.SetVector(new Vector2(blockingX, obj.GetVector().Y));
                }
                else
                {
                    int edgeLeft = GetEdgeLeft();
                    float difference = edgeLeft - nextPosition.X;
                    if (nextPosition.X < edgeLeft || Math.Abs(difference) < Math.Abs(movement.X))
                    {
                        obj.SetVector(new Vector2(edgeLeft, obj.GetVector().Y));
                    }
                    else
                    {
                        obj.SetVector(nextPosition);
                    }

                    
                }

            }
            else if (velocity.X > 0)
            {
                Rectangle nextBounds = obj.Bounds(nextPosition);
                List<GameObject> nearbyObjects = _spatialGrid.GetNearby(obj.Bounds(nextPosition));
                float blockingX = float.MaxValue;
                bool hasBlock = false;

                foreach (GameObject other in nearbyObjects)
                {
                    if (other == obj) continue;
                    if (!other.IsBlocking()) continue;

                    Rectangle otherBounds = other.Bounds(other.GetVector());
                    if (otherBounds.Intersects(nextBounds))
                    {
                        hasBlock = true;

                        float candidate = other.Bounds(other.GetVector()).Left;

                        if (candidate < blockingX)
                            blockingX = candidate;
                    }
                }
                if (hasBlock)
                {
                    obj.SetVector(new Vector2(blockingX - obj.GetSizeX(), obj.GetVector().Y));
                }
                else
                {
                    int edgeRight = GetEdgeRight(obj.GetSizeX());
                    float difference = edgeRight - nextPosition.X;
                    if (nextPosition.X > edgeRight || difference < movement.X)
                    {
                        obj.SetVector(new Vector2(edgeRight, obj.GetVector().Y));
                    }
                    else
                    {
                        obj.SetVector(nextPosition);
                    }
                }
                _spatialGrid.Update(obj);

            }

            if (velocity.Y < 0)
            {
                Rectangle nextBounds = obj.Bounds(nextPosition);
                List<GameObject> nearbyObjects = _spatialGrid.GetNearby(obj.Bounds(nextPosition));
                float blockingY = float.MinValue;
                bool hasBlock = false;

                foreach (GameObject other in nearbyObjects)
                {
                    if (other == obj) continue;
                    if (!other.IsBlocking()) continue;

                    Rectangle otherBounds = other.Bounds(other.GetVector());
                    if (otherBounds.Intersects(nextBounds))
                    {
                        hasBlock = true;

                        float candidate = other.Bounds(other.GetVector()).Bottom;

                        if (candidate > blockingY)
                            blockingY = candidate;
                    }
                }
                if (hasBlock)
                {
                    obj.SetVector(new Vector2(obj.GetVector().X, blockingY));
                }
                else
                {
                    int edgeTop = GetEdgeTop();
                    float difference = edgeTop - nextPosition.Y;
                    if (nextPosition.Y < edgeTop || Math.Abs(difference) < Math.Abs(movement.Y))
                    {
                        obj.SetVector(new Vector2(obj.GetVector().X, edgeTop));
                        _spatialGrid.Update(obj);
                    }
                    else
                    {
                        obj.SetVector(nextPosition);
                    }

                }
                _spatialGrid.Update(obj);
            }
            else if (velocity.Y > 0)
            {
                Rectangle nextBounds = obj.Bounds(nextPosition);
                List<GameObject> nearbyObjects = _spatialGrid.GetNearby(obj.Bounds(nextPosition));
                float blockingY = float.MaxValue;
                bool hasBlock = false;

                foreach (GameObject other in nearbyObjects)
                {
                    if (other == obj) continue;
                    if (!other.IsBlocking()) continue;

                    Rectangle otherBounds = other.Bounds(other.GetVector());
                    if (otherBounds.Intersects(nextBounds))
                    {
                        hasBlock = true;

                        float candidate = other.Bounds(other.GetVector()).Top;

                        if (candidate < blockingY)
                            blockingY = candidate;
                    }
                }
                if (hasBlock)
                {
                    obj.SetVector(new Vector2(obj.GetVector().X, blockingY - obj.GetSizeY()));
                }
                else
                {
                    int edgeBottom = GetEdgeBottom(obj.GetSizeY());
                    float difference = edgeBottom - nextPosition.Y;
                    if (nextPosition.Y > edgeBottom || difference < movement.Y)
                    {
                        obj.SetVector(new Vector2(obj.GetVector().X, edgeBottom));
                        _spatialGrid.Update(obj);
                    }
                    else
                    {
                        obj.SetVector(nextPosition);
                    }

                }
                _spatialGrid.Update(obj);
            }              
                
            

        }

    }
}
