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

        /// <summary>
        /// vektor dla pozycji myszy
        /// </summary>
        // vektor dla pozycji myszy
        private Vector2 pozycjaKursora;
        /// <summary>
        /// pozycja celu
        /// </summary>
        //pozycja celu
        Vector2[] pozycja = new Vector2[iloscsmieci];
        /// <summary>
        /// odleglosc kursora od danego celu
        /// </summary>
        //odleglosc kursora od danego celu
        float[] odleglosc = new float[iloscsmieci];
        /// <summary>
        /// promien celu wykorzystywany do wysrodkowania klikniecia
        /// </summary>
        //promien celu wykorzystywany do wysrodkowania klikniecia
        const int promienCelu = 45;
        /// <summary>
        /// promien kursora, do dokladnego klikanai srodkiem
        /// </summary>
        //promien kursora 
        const int promienKursora = 25;
        /// <summary>
        /// zmienna ktora decyduje o tym ile smieci sie pojawi na ekranie
        /// </summary>
        //zmienna ktora decyduje o tym ile smieci sie pojawi na ekranie
        const int iloscsmieci = 12;
        /// <summary>
        /// lista przyciskow
        /// </summary>
        //lista przyciskow
        private List<Component> _components;
        /// <summary>
        /// zbieranie danych z myszy
        /// </summary>
        //zbieranie danych z myszy
        MouseState mState;
        bool mReleased = true;
        /// <summary>
        /// dana do liczenia aktualnej ilosci smieci w grze
        /// </summary>
        //dana do liczenia aktualnej ilosci smieci w grze
        int smieci = 0;
        /// <summary>
        /// czas
        /// </summary>
        //czas
        double timer = 0;
        
        /// <summary>
        /// tablica do pozakywania celow
        /// </summary>
        //tablica do pozakywania celow
        int[] numercelu = new int[iloscsmieci];
        
        /// <summary>
        /// pozwala wylosowac pozycje
        /// </summary>
        // pozwala wylosowac pozycje
        int losowanie = 1;
        /// <summary>
        /// wypisanie wyniku
        /// </summary>
        //wypisywanie wyniku
        float wynik;
        
        /// wgrywanie wszystkich tekstur i przyciskow
        /// </summary>
        /// <param name="game"></param>
        /// <param name="graphicDevice"></param>
        /// <param name="content"></param>
        public GameState(Game1 game, GraphicsDevice graphicDevice, ContentManager content) : base(game, graphicDevice, content)
        {
            
            var buttonTexture = content.Load<Texture2D>("Controls/Button");
            var buttoningameTexture = content.Load<Texture2D>("Controls/Button2");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");
            var targetSprite = content.Load<Texture2D>("target2");
            var crosshairsSprite = content.Load<Texture2D>("crosshairs");
            var backgroundSprite = content.Load<Texture2D>("tlo");
            var gameFont = content.Load<SpriteFont>("galleryFont");

            //tworzenie przyciskow dostepnych podczas gry jak i po pokazaniu wyniku

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

            // tworzenie listy dostepnych przyciskow

            _components = new List<Component>()
            {
                menuGameButton,
                nextlevelGameButton,
                restartGameButton,
                menuingameGameButton,
            };


        }
        /// <summary>
        /// funkcje przyciskow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //akcje ktore sie dzieja po przycisnieciu przyciskow
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

        /// <summary>
        /// rozmieszca wszystkie obiekty po planszy gry oraz odpowiada za to czy dany elemnt zostal klikniety, po czym je ukrywa jest tu zliczony wynik jak i czas oraz kontroluje to czy dany element nie generuje sie na innym elemencie
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
           
            // ustawienie na poczatku gry widocznosci celu jak i przejscia do losowania
            if (timer == 0)
            {
                for (int i = 0; i < iloscsmieci; i++)
                {
                    numercelu[i] = 1;
                    losowanie = 1;
                    smieci++;
                }
            }


            //czas zliczany do usuniecia wszystkich smieci
            if (smieci > 0)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (smieci < 0)
            {
                smieci = 0;
            }



            //pobieranie aktualnej pozycji kursora
            mState = Mouse.GetState();

            pozycjaKursora = new Vector2(mState.X, mState.Y);

            // losowanie pozycji elementu na ekranie gry
            if (losowanie == 1)
            {
                Random rand = new Random();

                //losowanie pozycji w okienku gry
                for (int i = 0; i < iloscsmieci; i++)
                {
                    
                    pozycja[i].X = rand.Next(100, 1220 - promienCelu);
                    pozycja[i].Y = rand.Next(100, 850 - promienCelu);
                    
                    //minimalizacja generacji obiektow na sobie sprawdza czy jest cos wygenerowane w odleglosci 3*wielkosc celu
                    int minimalnaodleglosc = promienCelu * 3;
                    for (int k = 0; k < 3; k++)
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

                }
                //po wylosowaniu odpowiedniego miejsca nie chcemy wracac do ponownego losowania miejsca
                losowanie = 0;

            }


            // jesli kliknelismy lewy przycisk myszy sprawdza odleglosc jej od celu i jesli jest mniejsza niz srednica celu usuwa go z ekranu
            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {

                //sprawdzanie odleglosci od celu
                for (int i = 0; i < iloscsmieci; i++)
                {
                    odleglosc[i] = Vector2.Distance(pozycja[i], mState.Position.ToVector2());
                    if (odleglosc[i] < promienCelu && smieci > 0)
                    {
                        pozycja[i].X = 3000;
                        pozycja[i].Y = 3000;
                        smieci--;
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
            
            // po zebraniu wszystkich smieci zliczny jest wynik jak i wyswietlane guziki
            if (smieci == 0)
            {

                wynik = (float)(1000 / timer);
                for (int i = 0; i < 3; i++)
                {
                    _components[i].Update(gameTime);
                }
            }
            // jesli gra nadal trwa w rohu jest guzik menu
            if (smieci != 0)
            {
                _components[3].Update(gameTime);

            }



        }
        /// <summary>
        /// rysuje odpowiednie obrazki jako cele i wyswietla wynik z czasem itp.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //tlo
            spriteBatch.Draw(_content.Load<Texture2D>("tlo"), new Vector2(0, 0), Color.White);
            
            // jesli numer celu jest 1 to rysuje teksture jesli 0 to jej nie wyswietla
            for (int i = 0; i < iloscsmieci; i++)
            {
                if (numercelu[i] == 1)
                {
                    spriteBatch.Draw(_content.Load<Texture2D>("target2"), new Vector2(pozycja[i].X - promienCelu, pozycja[i].Y - promienCelu), Color.White);
                }
            }
            
            //gafika kursora
            spriteBatch.Draw(_content.Load<Texture2D>("crosshairs"), new Vector2(pozycjaKursora.X - promienKursora, pozycjaKursora.Y - promienKursora), Color.White);
            //guzik menu w grze
            if (smieci != 0)
            {
                _components[3].Draw(gameTime, spriteBatch);

            }
            //czas
            spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Czas: " + Math.Ceiling(timer).ToString(), new Vector2(0, 30), Color.Red);
            //wyswietla ile smieci zostalo
            spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Pozostale smieci: " + smieci.ToString(), new Vector2(0, 0), Color.White);
            //Zebranie wszystkiego wypisanie wyniku

            if (smieci == 0)
            {
                spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Zebrales wszystko!!!! Twoj wynik to: " + Math.Ceiling(wynik).ToString(), new Vector2(400, 0), Color.Black);
                for (int i = 0; i < 3; i++)
                {
                    _components[i].Draw(gameTime, spriteBatch);
                }
            }


        }
        
    }
}
