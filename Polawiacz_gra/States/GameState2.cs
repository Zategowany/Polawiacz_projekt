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
    public class GameState2 : State
    {

        private Vector2 pozycjaKursora;

        //pozycja celu
        Vector2[] pozycja = new Vector2[iloscsmieci];
        //odleglosc kursora od danego celu
        float[] odleglosc = new float[iloscsmieci];

        const int targetRadius = 45;
        const int promienKursora = 25;
        const int iloscsmieci = 15;
        private List<Component> _components;

        MouseState mState;
        bool mReleased = true;
        int trash = 0;
        double timer = 0;
        int zlewybory =0;
        int[] numercelu = new int[iloscsmieci];
        int[] wybortargetu = new int[iloscsmieci];

        int losowanie = 1;
        float wynik;


        public GameState2(Game1 game, GraphicsDevice graphicDevice, ContentManager content) : base(game, graphicDevice, content)
        {

            var buttonTexture = content.Load<Texture2D>("Controls/Button");
            var buttoningameTexture = content.Load<Texture2D>("Controls/Button2");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");
            var target2Sprite = content.Load<Texture2D>("target2");
            var targetSprite = content.Load<Texture2D>("target");
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
                nextlevelGameButton,
                menuGameButton,
                restartGameButton,
                menuingameGameButton,
            };
           



        }
        private void MenuGameButton_Click(object sender, EventArgs e)
        {
            _game.ChanegeState(new MenuState(_game, _graphicsDevice, _content));
        }
        private void NextlevelGameButton_Click(object sender, EventArgs e)
        {
            _game.ChanegeState(new GameState3(_game, _graphicsDevice, _content));
        }
        private void RestartGameButton_Click(object sender, EventArgs e)
        {
            _game.ChanegeState(new GameState2(_game, _graphicsDevice, _content));
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {


            if (timer == 0)
            {
                for (int i = 0; i < iloscsmieci; i++)
                {
                    numercelu[i] = 1;
                    
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

            //petla sie wykonuje tylko raz na poczatku gry

            if (losowanie == 1)
            {
                Random rand = new Random();

                //losowanie pozycji w okienku gry
                for (int i = 0; i < iloscsmieci; i++)
                {
                    pozycja[i].X = rand.Next(100, 1220 - targetRadius);
                    pozycja[i].Y = rand.Next(100, 850 - targetRadius);

                    //minimalizacja generacji obiektow na sobie
                    int minimalnaodleglosc = targetRadius * 3;
                    for ( int k = 0; k <4; k++)
                    {
                        for (int j = 0; j < iloscsmieci; j++)
                        {
                            if (i != j)
                            {
                                while (Vector2.Distance(pozycja[i], pozycja[j]) < minimalnaodleglosc)
                                {
                                    pozycja[i].X = rand.Next(100, 1220 - targetRadius);
                                    pozycja[i].Y = rand.Next(100, 850 - targetRadius);
                                }
                            }

                        }
                    }
                    
                    wybortargetu[i] = rand.Next(0, 5);
                    if (wybortargetu[i] == 0 || wybortargetu[i] == 1 || wybortargetu[i] == 2)
                        trash++;
                }
                losowanie = 0;
            }
            

            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {

                //sprawdzanie odleglosci od celu
                for (int i = 0; i < iloscsmieci; i++)
                {
                    odleglosc[i] = Vector2.Distance(pozycja[i], mState.Position.ToVector2());
                    if (odleglosc[i] < targetRadius && trash > 0)
                    {
                        //wybieranie czegos innego niz smieci nie odejmuje ilosci smieci
                        if (wybortargetu[i] == 0 || wybortargetu[i] == 1 || wybortargetu[i] == 2)
                        { 
                            trash--;
                            pozycja[i].X = 3000;
                            pozycja[i].Y = 3000;
                        }
                        //klikniecie w target ktory powinien zostac nietkniety
                        if (wybortargetu[i] == 3 || wybortargetu[i] == 4)
                        {
                            zlewybory++; 
                        }

                        numercelu[i] = 0;
                    }
                }



                mReleased = false;
            }
            if (mState.LeftButton == ButtonState.Released)
            {
                mReleased = true;
            }
           
            //rysowanie przyciskow podczas gry jak i po zakonczeniu
            if (trash == 0)
            {

                wynik = (float)(1000 / timer);
                for (int i=0; i<3 ;i++)
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
            spriteBatch.Draw(_content.Load<Texture2D>("tlo2v2"), new Vector2(0, 0), Color.White);

            //jesli kliknalem na obrazek znika

            for (int i = 0; i < iloscsmieci; i++)
            {
                if (numercelu[i] == 1)
                {
                   
                   switch (wybortargetu[i])
                    {
                          case 0:
                            spriteBatch.Draw(_content.Load<Texture2D>("target"), new Vector2(pozycja[i].X - targetRadius, pozycja[i].Y - targetRadius), Color.White);
                           break;
                          case 1:
                            spriteBatch.Draw(_content.Load<Texture2D>("target2"), new Vector2(pozycja[i].X - targetRadius, pozycja[i].Y - targetRadius), Color.White);
                           break;
                          case 2:
                            spriteBatch.Draw(_content.Load<Texture2D>("target4"), new Vector2(pozycja[i].X - targetRadius, pozycja[i].Y - targetRadius), Color.White);
                           break;
                          case 3:
                            spriteBatch.Draw(_content.Load<Texture2D>("target3"), new Vector2(pozycja[i].X - targetRadius, pozycja[i].Y - targetRadius), Color.White);
                           break;
                          case 4:
                            spriteBatch.Draw(_content.Load<Texture2D>("target5"), new Vector2(pozycja[i].X - targetRadius, pozycja[i].Y - targetRadius), Color.White);
                           break;

                    }
                        
                }
            }
            //wyswietrlanie przycisku menu podczas gry
            if (trash != 0)
            {
                _components[3].Draw(gameTime, spriteBatch);

            }


            //gafika kursora
            spriteBatch.Draw(_content.Load<Texture2D>("crosshairs"), new Vector2(pozycjaKursora.X - promienKursora, pozycjaKursora.Y - promienKursora), Color.White);
            //czas

            spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Czas: " + Math.Ceiling(timer).ToString(), new Vector2(0, 30), Color.Red);
            //wynik
            spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Pozostale smieci: " + trash.ToString(), new Vector2(0, 0), Color.White);
            //Zebranie wszystkiego
            if (trash == 0)
            {
                spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Zebrales wszystko!!!! Twoj wynik to: " + Math.Ceiling(wynik).ToString() + "\n Dokonales " + zlewybory.ToString()+" zlych wyborow.", new Vector2(400, 200), Color.White);
               // foreach (var component in _components)
                //    component.Draw(gameTime, spriteBatch);

                    for (int i = 0; i < 3; i++)
                    {
                        _components[i].Draw(gameTime, spriteBatch);
                    }

            }

        }

    }
}
