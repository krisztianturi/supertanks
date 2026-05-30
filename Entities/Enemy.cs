using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperTanks.Core;
using SuperTanks.Entities;
using SuperTanks.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SuperTanks.Entities
{

    internal enum SpeedType
    {
        Normal,
        Fast,
        VeryFast,
        Various
    }

    internal class Speed
    {
        private SpeedType _speedType;
        internal float Value { get; set; }

        internal Speed(SpeedType speedType)
        {
            this._speedType = speedType;
            this.SetDefults();
        }

        private void SetDefults()
        {
            switch (_speedType)
            {
                case SpeedType.Normal: Value = 100F; break;
                case SpeedType.Fast: Value = 200F; break;
                case SpeedType.VeryFast: Value = 300F; break;
                case SpeedType.Various: Value = 350F; break;

            }
        }

        internal SpeedType GetSpeedType() { return _speedType; }
        internal void SetType(SpeedType type) { _speedType = type; SetDefults(); }
    }


    internal enum MoveType
    {
        UntilBlock,
        Follower
    }

    internal class Enemy : LivingObject
    {

        private static double _LimitTime = 100;
        private double _moveTime,_shootingTime;
        private MoveType _moveType;

        private Vector2? _previousVector, _prevVector;
        private List<Func<Vector2>> _getTargetVectors;
        private int _setDirectionCounter;
        private readonly static double _finish = 1000 / _LimitTime;
        private float _distanceValue;
        private bool _isBlockingNow;
        private double _untilBlockStartTime;

        private Speed _speed;

        internal MoveType GetMoveType() { return _moveType; }
        internal SpeedType GetSpeedType() { return _speed.GetSpeedType(); }

        


        internal Enemy(Texture2D up, Texture2D down, Texture2D left, Texture2D right, Vector2 vector, int sizeX, int sizeY, bool blocking, bool shootable, int power, int vitality, Animation animation, SpeedType speedType) : base(up, down, left, right, vector, sizeX, sizeY, blocking, shootable, power, vitality, animation)
        {
            Invulnerable = true;
            _moveType = MoveType.UntilBlock;
            this.SetDirection(EnumRandomizer.GetRandom<Direction>());
            this._speed = new Speed(speedType);
        }

        internal Enemy(Texture2D up, Texture2D down, Texture2D left, Texture2D right, Vector2 vector, int sizeX, int sizeY, bool blocking, bool shootable, int power, int vitality, Animation animation, List<Func<Vector2>> targets, SpeedType speedType) : base(up, down, left, right, vector, sizeX, sizeY, blocking, shootable, power, vitality, animation)
        {
            Invulnerable = true;
            _moveType = MoveType.Follower;
            _getTargetVectors = targets;
            this._speed = new Speed(speedType);
        }

        internal override void Update(GameTime gameTime, GameManager gm)
        {

            if (_speed.GetSpeedType() == SpeedType.Various)
            {
                if (_speed.Value > 400) _speed.Value = 50F;
                else _speed.Value += 0.3F;
            }


            double currentTime = gameTime.TotalGameTime.TotalMilliseconds;

            if (this.Invulnerable)
            {
                if (this.BecameInvulnerableTime == 0)
                {
                    BecameInvulnerableTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
                else if (currentTime > BecameInvulnerableTime+ this.GetInvulnerableTime())
                {
                    Invulnerable = false;
                    BecameInvulnerableTime = 0;
                }
            }            


            if (currentTime > _moveTime + _LimitTime)
            {
                if (_moveType==MoveType.UntilBlock || _isBlockingNow)
                {
                    Random r = new();
                    if(_previousVector!=null && (_previousVector == this.GetVector() || r.NextDouble()>0.95))
                    {
                        Direction temp;
                        do
                        {
                            temp = EnumRandomizer.GetRandom<Direction>();
                        }
                        while (temp==this.GetDirection());
                        this.SetDirection(temp);

                    }
                    _previousVector = this.GetVector();

                    if (_isBlockingNow)
                    {
                        if (currentTime > _untilBlockStartTime+3000)
                        {
                            _isBlockingNow = false;
                            _untilBlockStartTime = 0;
                        }
                        
                    }

                }
                else
                {
                    var diffs = GetClosestTarget();
                    float diffX = diffs.diffX;
                    float diffY = diffs.diffY;
                    if (_setDirectionCounter==0)
                    {
                        _distanceValue = (Math.Abs(diffX) > Math.Abs(diffY)) ? diffX : diffY;
                        _setDirectionCounter++;
                    }else
                    {
                        _setDirectionCounter++;
                        if (_setDirectionCounter == _finish)
                        {
                            _setDirectionCounter = 0;
                        }
                    }

                    if (Math.Abs(diffX) < 0.01f && Math.Abs(diffY) < 0.01f)
                    {
                        _isBlockingNow = true;
                        _untilBlockStartTime = gameTime.TotalGameTime.TotalMilliseconds;
                    }
                    else if (diffX > 0 && _distanceValue == diffX)
                    {
                        this.SetDirection(Direction.Left);
                    }
                    else if (diffX < 0 && _distanceValue == diffX)
                    {
                        this.SetDirection(Direction.Right);
                    }
                    else if (diffY > 0 && _distanceValue == diffY)
                    {
                        this.SetDirection(Direction.Up);
                    }
                    else if (diffY < 0 && _distanceValue == diffY)
                    {
                        this.SetDirection(Direction.Down);
                    }

                    if (_prevVector != null && _prevVector == this.GetVector())
                    {
                        _isBlockingNow = true;
                        _untilBlockStartTime = gameTime.TotalGameTime.TotalMilliseconds;
                    }

                    _prevVector = this.GetVector();
                }

                _moveTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

            Move(gameTime, gm);
            gm.CheckBoost(this);

            if (currentTime > _shootingTime + 1000)
            {
                gm.Shooting(this);
                _shootingTime = gameTime.TotalGameTime.TotalMilliseconds;
            }

        }

        private (float diffX, float diffY) GetClosestTarget()
        {
            float closestDistance = float.MaxValue;

            float diffX = 0;
            float diffY = 0;

            foreach (var item in _getTargetVectors)
            {
                Vector2 target = item();

                float dx = GetVector().X - target.X;
                float dy = GetVector().Y - target.Y;

                float distance = Math.Abs(dx) + Math.Abs(dy);

                if (distance < closestDistance)
                {
                    closestDistance = distance;

                    diffX = dx;
                    diffY = dy;
                }
            }
            return (diffX, diffY);
        }

        private void Move(GameTime gameTime, GameManager gm)
        {
            Vector2 velocity = Vector2.Zero;
            switch (this.GetDirection())
            {
                case Direction.Left:
                    {
                        velocity.X -= 1;
                        base.SetCurrentImg(base.GetLeftImg());
                    }
                    break;
                case Direction.Right:
                    {
                        velocity.X += 1;
                        base.SetCurrentImg(base.GetRightImg());
                    }
                    break;
                case Direction.Up:
                    {
                        velocity.Y -= 1;
                        base.SetCurrentImg(base.GetUpImg());
                    }
                    break;
                case Direction.Down:
                    {
                        velocity.Y += 1;
                        base.SetCurrentImg(base.GetDownImg());
                    }
                    break;
            }
            float finalSpeed = OnIce ? _speed.Value / 2 : _speed.Value;
            Vector2 movement = velocity * finalSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            gm.MoveWithClamp(velocity, movement, this);
        }

        internal void HitHandle(GameManager gm)
        {
            if (!this.Invulnerable)
            {
                if (this.HasShip)
                {
                    HasShip = false;
                    Invulnerable = true;
                    return;
                }
                else if (this.GetPower() >= 3)
                {
                    this.SetPower(this.GetPower() - 3);
                    return;
                }
                else if (this.GetVitality() == 1)
                {
                    gm.RemoveEnemy(this);
                    return;
                }
                this.SetVitality(this.GetVitality() - 1);
                gm.MakeBoost();
            }

        }

        internal override void Draw(Renderer renderer)
        {
            if (GetVitality()>1)
            {
                renderer.DrawWithDepth(base.GetCurrentImg(), base.GetVector(), Color.Pink,0.1f);
            }
            else
            {
                renderer.DrawWithDepth(base.GetCurrentImg(), base.GetVector(), Color.White, 0.1f);
            }

            if (this.Invulnerable)
            {
                renderer.DrawWithDepth(this.GetAnimation().getImage(), base.GetVector(), Color.White, 0.1f);
            }
        }


    }
}
