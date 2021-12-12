using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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

        

        Vector2 targetPosition = new Vector2(0,0);
        Vector2 targetPosition2 = new Vector2(0, 0);

        const int targetRadius = 45;
        const int promienKursora = 25;
        

        MouseState mState;
        bool mReleased = true;
        int trash = 10;
        double timer = 0;

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
            targetSprite = Content.Load<Texture2D>("target");
            crosshairsSprite = Content.Load<Texture2D>("crosshairs");
            backgroundSprite = Content.Load<Texture2D>("sky");
            gameFont = Content.Load<SpriteFont>("galleryFont");
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
                targetPosition.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                targetPosition.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                targetPosition2.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                targetPosition2.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
            }

            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {
                float mouseTargetDist = Vector2.Distance(targetPosition, mState.Position.ToVector2()); //pozycja myszki czy jerst mniejsza od promienia celu
                if(mouseTargetDist < targetRadius && trash > 0)
                {
                    trash--;

                    Random rand = new Random();
                    targetPosition.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius );
                    targetPosition.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                }
                float mouseTargetDist2 = Vector2.Distance(targetPosition2, mState.Position.ToVector2()); //pozycja myszki czy jerst mniejsza od promienia celu
                if (mouseTargetDist2 < targetRadius && trash > 0)
                {
                    trash--;

                    Random rand = new Random();
                    targetPosition2.X = rand.Next(45, _graphics.PreferredBackBufferWidth - targetRadius);
                    targetPosition2.Y = rand.Next(45, _graphics.PreferredBackBufferHeight - targetRadius);
                }
                mReleased = false;
            }
            if (mState.LeftButton == ButtonState.Released)
            {
                mReleased = true;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
            //jesli score jest 10 nie generuje
            if(trash > 0)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(targetPosition.X - targetRadius, targetPosition.Y - targetRadius), Color.White);
            }
            if (trash > 0)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(targetPosition2.X - targetRadius, targetPosition2.Y - targetRadius), Color.White);
            }

            //gafika kursora
            _spriteBatch.Draw(crosshairsSprite, new Vector2(pozycjaKursora.X - promienKursora, pozycjaKursora.Y - promienKursora), Color.White);
            //czas
            _spriteBatch.DrawString(gameFont, "Czas: "+  Math.Ceiling(timer).ToString(), new Vector2(0, 30), Color.Red);
            //wynik
            _spriteBatch.DrawString(gameFont,"Pozostale smieci: " +trash.ToString(), new Vector2(0, 0), Color.White);
            _spriteBatch.End();



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
