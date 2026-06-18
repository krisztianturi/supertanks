

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperTanks.Core;
using SuperTanks.Entities;
using SuperTanks.Systems;
using System.Collections.Generic;
using System.Diagnostics;

namespace SuperTanks.Overlays
{
    internal class StatisticRow
    {
        public int Total { get; }
        public MoveType MoveType { get; }
        public SpeedType SpeedType { get; }

        internal StatisticRow(int Total, MoveType MoveType, SpeedType SpeedType)
        {
            this.Total = Total;
            this.MoveType = MoveType;
            this.SpeedType = SpeedType;
        }
    }

    internal class GameOverOverlay : IOverlay
    {

        private bool _won;
        private List<StatisticRow> _rows = new();
        private readonly int _totalEnemies;
        private bool init;

        internal GameOverOverlay(bool won)
        {
            _won = won;
        }

        internal GameOverOverlay(bool won, Dictionary<(MoveType, SpeedType), int> kills)
        {
            _won= won;

            foreach (var item in kills)
            {
                _rows.Add(new StatisticRow(item.Value, item.Key.Item1, item.Key.Item2));
                _totalEnemies += item.Value;
            }
            _rows.Sort((a, b) => b.Total.CompareTo(a.Total));
        }

        public void Update(GameTime gameTime)
        {
            if (!init)
            {
                if (!_won)
                {
                    Status.Reset();
                }
                else
                {
                    if (OverlayManager.GetPrevious() is SinglePlayerOverlay)
                    {
                        Player p = GameCreator.Instance.GetPlayer1();
                        Status.SetPlayer1(p.GetPower(), p.GetVitality(), p.HasShip);
                    }
                    else
                    {
                        Player p = GameCreator.Instance.GetPlayer1();
                        Status.SetPlayer1(p.GetPower(), p.GetVitality(), p.HasShip);
                        Player p2 = GameCreator.Instance.GetPlayer2();
                        Status.SetPlayer1(p2.GetPower(), p2.GetVitality(), p2.HasShip);
                    }
                }
                init = true;
            }


            if (InputManager.ExitPressed())
            {
                OverlayManager.RequestChange(new MenuOverlay());
            }
            if (InputManager.ConfirmPressed())
            {
                if (OverlayManager.GetPrevious() is SinglePlayerOverlay)
                {
                    OverlayManager.RequestChange(new SinglePlayerOverlay());
                }
                else
                {
                    OverlayManager.RequestChange(new MultiPlayerOverlay());
                }


            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float screenWidth = Engine.GetScreenWidth();
            float screenHeight = Engine.GetScreenHeight();
            Renderer renderer = new Renderer(spriteBatch);
            if (_won)
            {
                int distance = 100;
                int columnWidth = 150;


                int tableWidth = columnWidth * 3 + distance * 2;
                float startX = (screenWidth - tableWidth) / 2f;

                int columnHeight = 100;

                int tableHeight = (_rows.Count + 1) * columnHeight;

                float startY = (screenHeight - tableHeight) / 2f;

                renderer.DrawString("Total", new Vector2(startX, startY), Color.Black, 1.5f);
                startX += distance + columnWidth;
                renderer.DrawString("Move Type", new Vector2(startX, startY), Color.Black, 1.5f);
                startX += distance + columnWidth;
                renderer.DrawString("Speed Type", new Vector2(startX, startY), Color.Black, 1.5f);

                foreach (var item in _rows)
                {
                    startX = (screenWidth - tableWidth) / 2f;
                    startY += columnHeight;
                    renderer.DrawString(item.Total.ToString(), new Vector2(startX, startY), Color.Black, 1f);
                    startX += distance + columnWidth;
                    renderer.DrawString(item.MoveType.ToString(), new Vector2(startX, startY), Color.Black, 1f);
                    startX += distance + columnWidth;
                    renderer.DrawString(item.SpeedType.ToString(), new Vector2(startX, startY), Color.Black, 1f);
                }
                startX = (screenWidth - tableWidth) / 2f;
                startY += columnHeight / 2;
                renderer.DrawPixelRectWithoutOffset(new Rectangle((int)startX,(int)startY,columnWidth,2), Color.Black);
                startY += columnHeight / 2;
                renderer.DrawString(_totalEnemies.ToString(), new Vector2(startX, startY), Color.Black, 1f);



            }
            else
            {
                Texture2D gameOverImg = Assets._gameOver;
                renderer.Draw(gameOverImg, new Vector2(screenWidth / 2 - gameOverImg.Width / 2, screenHeight / 2 - gameOverImg.Height / 2), Color.White);
            }

        }


    }
}
