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
        // vektor dla pozycji myszy
        private Vector2 pozycjaKursora;

        //pozycja celu
        Vector2[] pozycja = new Vector2[iloscsmieci];
        //odleglosc kursora od danego celu
        float[] odleglosc = new float[iloscsmieci];
        //promien celu
        const int promienCelu = 45;
        //promien kursora
        const int promienKursora = 25;
        //zmienna ktora decyduje o tym ile smieci sie pojawi na ekranie
        const int iloscsmieci = 15;
        //lista przyciskow
        private List<Component> _components;
        //zmieranie danych z myszy
        MouseState mState;
        bool mReleased = true;
        //dana do liczenia aktualnej ilosci smieci w grze
        int smieci = 0;
        //czas
        double timer = 0;
        //zmienna do zliczania klikniec w zle obiekty
        int zlewybory =0;
        //tablica do pozakywania celow
        int[] numercelu = new int[iloscsmieci];
        //tablica do prtzechowywania jaki rodzaj celu ma sie wyswietlac
        int[] wybortargetu = new int[iloscsmieci];
        // pozwala wylosowac pozycje
        int losowanie = 1;
        //zapisywanie wyniku
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

            //tworzenie przyciskow
            var menuGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(1280 / 2 - 424 / 2, 700),
                Tekst = "Menu",
            };

            //odwloanie do eventu ktory ma sie wykonac po wcisniecu przycisku
            menuGameButton.Click += MenuGameButton_Click;

            var menuingameGameButton = new Button(buttoningameTexture, buttonFont)
            {
                Position = new Vector2(1080, 0),
                
            };

            menuingameGameButton.Click += MenuGameButton_Click;

            var nextlevelGameButton = new Button(buttonTexture, buttonFont)

            {
                Position = new Vector2(1280 / 2 - 424 / 2, 600),
                Tekst = "Nastepny Poziom",
            };

            nextlevelGameButton.Click += NextlevelGameButton_Click;

            var restartGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(1280 / 2 - 424 / 2, 500),
                Tekst = "Restart",
            };

            restartGameButton.Click += RestartGameButton_Click;

           //lista przyciskow
            _components = new List<Component>()
            {
                nextlevelGameButton,
                menuGameButton,
                restartGameButton,
                menuingameGameButton,
            };
           



        }
        //funkcje ktore wykonuja przyciski
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

            // na poczatku gry pokazanie wszystkich smieci pozniej numercelu jest zmieniany na 0 przez co nie jest generowany
            if (timer == 0)
            {
                for (int i = 0; i < iloscsmieci; i++)
                {
                    numercelu[i] = 1;
                    
                }
            }


            //czas zliczany do zebranmia wszystkich smieci
            if (smieci > 0)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (smieci < 0)
            {
                smieci = 0;
            }


            //aktualna pozycja kursora

            mState = Mouse.GetState();

            pozycjaKursora = new Vector2(mState.X, mState.Y);

            //petla sie wykonuje tylko raz na poczatku gry

            if (losowanie == 1)
            {
                Random rand = new Random();

                //dane potrzebne do zliczania ilosci elementow ktorych nie klikamy
                bool warunek = false;
                int zliczanie = 0;

                //losowanie pozycji w okienku gry
                for (int i = 0; i < iloscsmieci; i++)
                {
                    pozycja[i].X = rand.Next(100, 1220 - promienCelu);
                    pozycja[i].Y = rand.Next(100, 850 - promienCelu);

                    //minimalizacja generacji obiektow na sobie, nie moga byc w odleglosci 3*promiencelu
                    int minimalnaodleglosc = promienCelu * 3;
                    for ( int k = 0; k <4; k++)
                    {
                        for (int j = 0; j < iloscsmieci; j++)
                        {
                            if (i != j)
                            {
                                while (Vector2.Distance(pozycja[i], pozycja[j]) < minimalnaodleglosc)
                                {
                                    pozycja[i].X = rand.Next(100, 1220 - promienCelu);
                                    pozycja[i].Y = rand.Next(100, 850 - promienCelu);
                                }
                            }

                        }
                    }
                    //ograniczenie generacji celow w ktore nie mozna kliknac warunek odpowiedniej generacji jesli bedzie wiecej niz 4 obiekty ktorych nie mozna kliknac zmienia na tylko generacje smieci
                    if (warunek == true)
                    {
                        wybortargetu[i] = rand.Next(0, 3);
                    }
                    if (warunek == false)
                    {
                        wybortargetu[i] = rand.Next(0, 5);
                        if (wybortargetu[i] == 3 || wybortargetu[i] == 4)
                        {
                            zliczanie++;
                            if (zliczanie == 4)
                            {
                                warunek = true;
                            }
                        }
                    }
                    //zliczanie ile smieci bedzie na ekranie
                    if (wybortargetu[i] == 0 || wybortargetu[i] == 1 || wybortargetu[i] == 2)
                        smieci++;
                }
                losowanie = 0;
            }
            
            //akcje ktore sie dzieja po kliknieciu w dany cel
            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {

                //sprawdzanie odleglosci od celu
                for (int i = 0; i < iloscsmieci; i++)
                {
                    odleglosc[i] = Vector2.Distance(pozycja[i], mState.Position.ToVector2());
                    if (odleglosc[i] < promienCelu && smieci > 0)
                    {
                        //wybieranie czegos innego niz smieci nie odejmuje ilosci smieci
                        if (wybortargetu[i] == 0 || wybortargetu[i] == 1 || wybortargetu[i] == 2)
                        {
                            smieci--;
                            pozycja[i].X = 3000;
                            pozycja[i].Y = 3000;
                        }
                        //klikniecie w target ktory powinien zostac nietkniety
                        if (wybortargetu[i] == 3 || wybortargetu[i] == 4)
                        {
                            zlewybory++; 
                        }
                        //przestanie rysowania danego celu
                        numercelu[i] = 0;
                    }
                }



                mReleased = false;
            }
            if (mState.LeftButton == ButtonState.Released)
            {
                mReleased = true;
            }
           
            //rysowanie przyciskow po zakonczeniu i liczenie wyniku
            if (smieci == 0)
            {

                wynik = (float)(1000 - timer*10 - 100*zlewybory);
                if(wynik < 0)
                {
                    wynik = 0;
                }
                for (int i=0; i<3 ;i++)
                {
                    _components[i].Update(gameTime);
                }
            }
            //rysowanie przyciskow podczas gry
            if (smieci != 0)
            {
                _components[3].Update(gameTime);

            }



        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //tlo
            spriteBatch.Draw(_content.Load<Texture2D>("tlo2v2"), new Vector2(0, 0), Color.White);

            //rysowanie odpowiednich celow, ktore maja numercelu=1 

            for (int i = 0; i < iloscsmieci; i++)
            {
                if (numercelu[i] == 1)
                {
                   
                   switch (wybortargetu[i])
                    {
                          case 0:
                            spriteBatch.Draw(_content.Load<Texture2D>("target"), new Vector2(pozycja[i].X - promienCelu, pozycja[i].Y - promienCelu), Color.White);
                           break;
                          case 1:
                            spriteBatch.Draw(_content.Load<Texture2D>("target2"), new Vector2(pozycja[i].X - promienCelu, pozycja[i].Y - promienCelu), Color.White);
                           break;
                          case 2:
                            spriteBatch.Draw(_content.Load<Texture2D>("target4"), new Vector2(pozycja[i].X - promienCelu, pozycja[i].Y - promienCelu), Color.White);
                           break;
                          case 3:
                            spriteBatch.Draw(_content.Load<Texture2D>("target3"), new Vector2(pozycja[i].X - promienCelu, pozycja[i].Y - promienCelu), Color.White);
                           break;
                          case 4:
                            spriteBatch.Draw(_content.Load<Texture2D>("target5"), new Vector2(pozycja[i].X - promienCelu, pozycja[i].Y - promienCelu), Color.White);
                           break;

                    }
                        
                }
            }
            //wyswietlanie przycisku menu podczas gry
            if (smieci != 0)
            {
                _components[3].Draw(gameTime, spriteBatch);

            }


            //gafika kursora
            spriteBatch.Draw(_content.Load<Texture2D>("crosshairs"), new Vector2(pozycjaKursora.X - promienKursora, pozycjaKursora.Y - promienKursora), Color.White);
            //czas

            spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Czas: " + Math.Ceiling(timer).ToString(), new Vector2(0, 30), Color.Red);
            //wynik
            spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Pozostale smieci: " + smieci.ToString(), new Vector2(0, 0), Color.White);
            //Zebranie wszystkiego
            if (smieci == 0)
            {
                spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Zebrales wszystko!!!! Twoj wynik to: " + Math.Ceiling(wynik).ToString() + "\n         Dokonales " + zlewybory.ToString()+ " zly(ch) wybor(ow).", new Vector2(400, 0), Color.Black);
               
                //wyswietlenie pozostalych przyciskow
                    for (int i = 0; i < 3; i++)
                    {
                        _components[i].Draw(gameTime, spriteBatch);
                    }

            }

        }

    }
}
