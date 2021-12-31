using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Polawiacz_gra.Controls;
using Polawiacz_gra.States;
using System;
using System.Collections.Generic;

namespace Polawiacz_gra
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 pozycjaKursora;

        
        private State _currentState;
        private State _nextState;

        public void ChanegeState(State state)
        {
            _nextState = state;
        }
       

        Texture2D targetSprite;
        Texture2D crosshairsSprite;
        Texture2D backgroundSprite;

        SpriteFont gameFont;
        const int promienKursora = 25;
        MouseState mState;
 

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
            _currentState = new MenuState(this, _graphics.GraphicsDevice, Content);
            
            

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

            
            if(_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }

            _currentState.Update(gameTime);
            _currentState.PostUpdate(gameTime);
   

            mState = Mouse.GetState();
            pozycjaKursora = new Vector2(mState.X, mState.Y);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            

            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);

           

            _currentState.Draw(gameTime, _spriteBatch);

            
            //gafika kursora
            _spriteBatch.Draw(crosshairsSprite, new Vector2(pozycjaKursora.X - promienKursora, pozycjaKursora.Y - promienKursora), Color.White);
            _spriteBatch.End();
            


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
