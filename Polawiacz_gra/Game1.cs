using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Polawiacz_gra
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 pozycjaKursora;

        Texture2D targetSprite;
        Texture2D crosshairsSprite;
        Texture2D backgroundSprite;

        SpriteFont gameFont;

        //pozycja celu
        Vector2[] pozycja = new Vector2[10];


        //odleglosc kursora od danego celu
        float[] odleglosc = new float[10];

        const int targetRadius = 45;
        const int promienKursora = 25;
        

        MouseState mState;
        bool mReleased = true;
        int trash = 10;
        double timer = 0;
        int[] numercelu = new int[10];

        float wynik;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //wielkość okna
            _graphics.PreferredBackBufferWidth = 1280;  // szerokość
            _graphics.PreferredBackBufferHeight = 900;   // wysokość
            _graphics.ApplyChanges();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //ladowanie obrazkow
            targetSprite = Content.Load<Texture2D>("target2");
            crosshairsSprite = Content.Load<Texture2D>("crosshairs");
            backgroundSprite = Content.Load<Texture2D>("tlo");
            gameFont = Content.Load<SpriteFont>("galleryFont");
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //nadanie obrazkowi wartosci 1 by mozna bylo pozniej ja zamienic na 0 i wylaczyc obrazek
            if (timer == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    numercelu[i] = 1;
                }
            }
            

            //czas zliczany do 10 pkt
            if(trash > 0)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (trash < 0)
            {
                trash = 0;
            }

            


            mState = Mouse.GetState();

            pozycjaKursora = new Vector2(mState.X, mState.Y);

            if (timer == 0)
            {
                Random rand = new Random();

                //losowanie pozycji w okienku gry
                for (int i=0;i<10;i++)
                {
                    pozycja[i].X= rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                    pozycja[i].Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                }

               
            }

            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {

                //sprawdzanie odleglosci od celu
                for (int i = 0; i < 10; i++)
                {
                    odleglosc[i] = Vector2.Distance(pozycja[i], mState.Position.ToVector2());
                    if (odleglosc[i] < targetRadius && trash > 0)
                    {
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
                
                wynik = (float)(1000/timer);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);

            //jesli kliknalem na obrazek znika
            for (int i = 0; i < 10; i++)
            {
                if (numercelu[i] == 1)
                {
                    _spriteBatch.Draw(targetSprite, new Vector2(pozycja[i].X - targetRadius, pozycja[i].Y - targetRadius), Color.White);
                }
            }
           

            //gafika kursora
            _spriteBatch.Draw(crosshairsSprite, new Vector2(pozycjaKursora.X - promienKursora, pozycjaKursora.Y - promienKursora), Color.White);
            //czas
            _spriteBatch.DrawString(gameFont, "Czas: "+  Math.Ceiling(timer).ToString(), new Vector2(0, 30), Color.Red);
            //wynik
            _spriteBatch.DrawString(gameFont,"Pozostale smieci: " +trash.ToString(), new Vector2(0, 0), Color.White);
            //Zebranie wszystkiego
            if (trash == 0)
            {
                _spriteBatch.DrawString(gameFont, "Zebrales wszystko!!!! Twoj wynik to: " + wynik.ToString(), new Vector2(400, 450), Color.White);
            }
            
            _spriteBatch.End();



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
