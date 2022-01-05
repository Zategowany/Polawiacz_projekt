using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Polawiacz_gra.Controls;

namespace Polawiacz_gra.States
{
    public class GameState : State
    {
        
        private Vector2 pozycjaKursora;
                             
        //pozycja celu
        Vector2[] pozycja = new Vector2[10];
        //odleglosc kursora od danego celu
        float[] odleglosc = new float[10];

        const int targetRadius = 45;
        const int promienKursora = 25;
        private List<Component> _components;

        MouseState mState;
        bool mReleased = true;
        int trash = 10;
        double timer = 0;
        int[] numercelu = new int[10];

        int losowanie = 0;
        float wynik;
        
        
        public GameState(Game1 game, GraphicsDevice graphicDevice, ContentManager content) : base(game, graphicDevice, content)
        {
            
            var buttonTexture = content.Load<Texture2D>("Controls/Button");
            var buttoningameTexture = content.Load<Texture2D>("Controls/Button2");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");
            var targetSprite = content.Load<Texture2D>("target2");
            var crosshairsSprite = content.Load<Texture2D>("crosshairs");
            var backgroundSprite = content.Load<Texture2D>("tlo");
            var gameFont = content.Load<SpriteFont>("galleryFont");

            var menuGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(400, 700),
                Text = "Menu",
            };
            menuGameButton.Click += MenuGameButton_Click;

            var menuingameGameButton = new Button(buttoningameTexture, buttonFont)
            {
                Position = new Vector2(1080, 0),

            };

            menuingameGameButton.Click += MenuGameButton_Click;

            var nextlevelGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(400, 600),
                Text = "Nastepny Poziom",
            };

            nextlevelGameButton.Click += NextlevelGameButton_Click;

            var restartGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(400, 500),
                Text = "Restart",
            };

            restartGameButton.Click += RestartGameButton_Click;

            _components = new List<Component>()
            {
                menuGameButton,
                nextlevelGameButton,
                restartGameButton,
                menuingameGameButton,
            };


        }

        private void NextlevelGameButton_Click(object sender, EventArgs e)
        {
            _game.ChanegeState(new GameState2(_game, _graphicsDevice, _content));
        }

        private void MenuGameButton_Click(object sender, EventArgs e)
        {
            _game.ChanegeState(new MenuState(_game, _graphicsDevice, _content));
        }

        private void RestartGameButton_Click(object sender, EventArgs e)
        {
            _game.ChanegeState(new GameState(_game, _graphicsDevice, _content));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
           

            if (timer == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    numercelu[i] = 1;
                    losowanie = 1;
                }
            }


            //czas zliczany do 10 pkt
            if (trash > 0)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (trash < 0)
            {
                trash = 0;
            }




            mState = Mouse.GetState();

            pozycjaKursora = new Vector2(mState.X, mState.Y);

            if (losowanie == 1)
            {
                Random rand = new Random();

                //losowanie pozycji w okienku gry
                for (int i = 0; i < 10; i++)
                {
                    pozycja[i].X = rand.Next(70, 1250 - targetRadius);
                    pozycja[i].Y = rand.Next(70, 850 - targetRadius);
                }
                losowanie = 0;

            }

            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {

                //sprawdzanie odleglosci od celu
                for (int i = 0; i < 10; i++)
                {
                    odleglosc[i] = Vector2.Distance(pozycja[i], mState.Position.ToVector2());
                    if (odleglosc[i] < targetRadius && trash > 0)
                    {
                        pozycja[i].X = 3000;
                        pozycja[i].Y = 3000;
                        trash--;
                        numercelu[i] = 0;
                    }
                }



                mReleased = false;
            }
            if (mState.LeftButton == ButtonState.Released)
            {
                mReleased = true;
            }
            // TODO: Add your update logic here
            if (trash == 0)
            {

                wynik = (float)(1000 / timer);
                for (int i = 0; i < 3; i++)
                {
                    _components[i].Update(gameTime);
                }
            }
            if (trash != 0)
            {
                _components[3].Update(gameTime);

            }



        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //tlo
            spriteBatch.Draw(_content.Load<Texture2D>("tlo"), new Vector2(0, 0), Color.White);
            
            for (int i = 0; i < 10; i++)
            {
                if (numercelu[i] == 1)
                {
                    spriteBatch.Draw(_content.Load<Texture2D>("target2"), new Vector2(pozycja[i].X - targetRadius, pozycja[i].Y - targetRadius), Color.White);
                }
            }
            
            //gafika kursora
            spriteBatch.Draw(_content.Load<Texture2D>("crosshairs"), new Vector2(pozycjaKursora.X - promienKursora, pozycjaKursora.Y - promienKursora), Color.White);
            //guzik menu w grze
            if (trash != 0)
            {
                _components[3].Draw(gameTime, spriteBatch);

            }
            //czas
            spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Czas: " + Math.Ceiling(timer).ToString(), new Vector2(0, 30), Color.Red);
            //wynik
            spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Pozostale smieci: " + trash.ToString(), new Vector2(0, 0), Color.White);
            //Zebranie wszystkiego

            if (trash == 0)
            {
                spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Zebrales wszystko!!!! Twoj wynik to: " + Math.Ceiling(wynik).ToString(), new Vector2(400, 200), Color.White);
                for (int i = 0; i < 3; i++)
                {
                    _components[i].Draw(gameTime, spriteBatch);
                }
            }

           


            //spriteBatch.End();

        }
        
    }
}
