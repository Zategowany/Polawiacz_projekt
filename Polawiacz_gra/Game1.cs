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
        /* stara pozycja celu
        public Vector2 targetPosition = new Vector2(0, 0);
        public Vector2 targetPosition2 = new Vector2(0, 0);
        public Vector2 targetPosition3 = new Vector2(0, 0);
        public Vector2 targetPosition4 = new Vector2(0, 0);
        public Vector2 targetPosition5 = new Vector2(0, 0);
        public Vector2 targetPosition6 = new Vector2(0, 0);
        public Vector2 targetPosition7 = new Vector2(0, 0);
        public Vector2 targetPosition8 = new Vector2(0, 0);
        public Vector2 targetPosition9 = new Vector2(0, 0);
        public Vector2 targetPosition10 = new Vector2(0, 0);
        */


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

                //losowanie pozycji w okienku gry (trzeba zrobic z tego liste lub klase)
                /*
                targetPosition.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                targetPosition.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                targetPosition2.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                targetPosition2.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                targetPosition3.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                targetPosition3.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                targetPosition4.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                targetPosition4.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                targetPosition5.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                targetPosition5.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                targetPosition6.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                targetPosition6.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                targetPosition7.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                targetPosition7.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                targetPosition8.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                targetPosition8.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                targetPosition9.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                targetPosition9.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                targetPosition10.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                targetPosition10.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                */
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
                    


                /*
                float mouseTargetDist = Vector2.Distance(pozycja[0], mState.Position.ToVector2()); //pozycja myszki czy jerst mniejsza od promienia celu
                if(mouseTargetDist < targetRadius && trash > 0)
                {
                    trash--;
                    numercelu[0] = 0;

                    // Random rand = new Random();
                    // targetPosition.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius );
                    // targetPosition.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                }
                
                float mouseTargetDist2 = Vector2.Distance(pozycja[1], mState.Position.ToVector2()); 
                if (mouseTargetDist2 < targetRadius && trash > 0)
                {
                    trash--;
                    numercelu[1] = 0;

                    // Random rand = new Random();
                    // targetPosition2.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                    // targetPosition2.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                }
                float mouseTargetDist3 = Vector2.Distance(pozycja[2], mState.Position.ToVector2()); 
                if (mouseTargetDist3 < targetRadius && trash > 0)
                {
                    trash--;
                    numercelu[2] = 0;
                }
                float mouseTargetDist4 = Vector2.Distance(pozycja[3], mState.Position.ToVector2()); 
                if (mouseTargetDist4 < targetRadius && trash > 0)
                {
                    trash--;
                    numercelu[3] = 0;
                }
                float mouseTargetDist5 = Vector2.Distance(pozycja[4], mState.Position.ToVector2()); 
                if (mouseTargetDist5 < targetRadius && trash > 0)
                {
                    trash--;
                    numercelu[4] = 0;
                }
                
                float mouseTargetDist6 = Vector2.Distance(pozycja[5], mState.Position.ToVector2()); 
                if (mouseTargetDist6 < targetRadius && trash > 0)
                {
                    trash--;
                    numercelu[5] = 0;
                }
                float mouseTargetDist7 = Vector2.Distance(pozycja[6], mState.Position.ToVector2()); 
                if (mouseTargetDist7 < targetRadius && trash > 0)
                {
                    trash--;
                    numercelu[6] = 0;
                }
                float mouseTargetDist8 = Vector2.Distance(pozycja[7], mState.Position.ToVector2()); 
                if (mouseTargetDist8 < targetRadius && trash > 0)
                {
                    trash--;
                    numercelu[7] = 0;
                }
                float mouseTargetDist9 = Vector2.Distance(pozycja[8], mState.Position.ToVector2()); 
                if (mouseTargetDist9 < targetRadius && trash > 0)
                {
                    trash--;
                    numercelu[8] = 0;
                }
                float mouseTargetDist10 = Vector2.Distance(pozycja[9], mState.Position.ToVector2()); 
                if (mouseTargetDist10 < targetRadius && trash > 0)
                {
                    trash--;
                    numercelu[9] = 0;
                }
                */
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
            /*
            if(numercelu[0] == 1)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(pozycja[0].X - targetRadius, pozycja[0].Y - targetRadius), Color.White);
            }
            
            if (numercelu[1] == 1)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(pozycja[1].X - targetRadius, pozycja[1].Y - targetRadius), Color.White);
            }
            if (numercelu[2] == 1)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(pozycja[2].X - targetRadius, pozycja[2].Y - targetRadius), Color.White);
            }
            if (numercelu[3] == 1)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(pozycja[3].X - targetRadius, pozycja[3].Y - targetRadius), Color.White);
            }
            if (numercelu[4] == 1)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(pozycja[4].X - targetRadius, pozycja[4].Y - targetRadius), Color.White);
            }
            
            if (numercelu[5] == 1)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(pozycja[5].X - targetRadius, pozycja[6].Y - targetRadius), Color.White);
            }
            if (numercelu[6] == 1)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(pozycja[6].X - targetRadius, pozycja[6].Y - targetRadius), Color.White);
            }
            if (numercelu[7] == 1)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(pozycja[7].X - targetRadius, pozycja[7].Y - targetRadius), Color.White);
            }
            if (numercelu[8] == 1)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(pozycja[8].X - targetRadius, pozycja[8].Y - targetRadius), Color.White);
            }
            if (numercelu[9] == 1)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(pozycja[9].X - targetRadius, pozycja[9].Y - targetRadius), Color.White);
            }
            */

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
