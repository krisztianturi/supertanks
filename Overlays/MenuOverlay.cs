using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperTanks.Systems;
using System;
using System.Xml.Linq;


namespace SuperTanks.Overlays
{
    internal class MenuOverlay : IOverlay
    {        

        private double _lastNavigateTime;
        private readonly static int _optionsNumber = 2;
        private int _currentOption = 0;

        internal MenuOverlay()
        {
        }

        public void Update(GameTime gameTime)
        {
            
            if (InputManager.ExitPressed())
            {
                OverlayManager.GetExitAction().Invoke();
            }
            else if (InputManager.ConfirmPressed())
            {
                if (_currentOption == 0)
                {
                    OverlayManager.RequestChange(new SinglePlayerOverlay());
                }
                else if (_currentOption == 1)
                {
                    OverlayManager.RequestChange(new MultiPlayerOverlay());
                }

            }


            double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
            if (currentTime>500+_lastNavigateTime)
            {
                if (InputManager.IsDown(Keys.Up))
                {
                    _lastNavigateTime = gameTime.TotalGameTime.TotalMilliseconds;
                    _currentOption--;
                    if (_currentOption<0)
                    {
                        _currentOption = _optionsNumber-1;
                    }
                }
                else if (InputManager.IsDown(Keys.Down))
                {
                    _lastNavigateTime = gameTime.TotalGameTime.TotalMilliseconds;
                    _currentOption++;
                    if (_currentOption>_optionsNumber-1)
                    {
                        _currentOption = 0;
                    }
                }
            }

            

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int screenX = Engine.GetScreenWidth();
            int screenY = Engine.GetScreenHeight();
            Renderer renderer = new Renderer(spriteBatch);
            renderer.DrawPixelRectWithoutOffset(new Rectangle(0, 0, screenX, screenY), Color.Black);
            Rectangle titleRect = new Rectangle(screenX/2-screenX/4,200,screenX/2,Assets._title.Height);
            renderer.DrawRectWithoutOffset(Assets._title,titleRect);


            float menuVectorX = titleRect.X + titleRect.Width / 2 - Assets._menuOptions.Width / 2;
            renderer.DrawVectorWithoutOffset(Assets._menuOptions, new Vector2(menuVectorX, titleRect.Bottom+100));
            renderer.DrawVectorWithoutOffset(Assets._arrow, new Vector2(menuVectorX-Assets._arrow.Width-50, titleRect.Bottom+100 + _currentOption*100));


        }


    }
}
