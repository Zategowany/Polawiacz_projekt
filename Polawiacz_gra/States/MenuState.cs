using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Polawiacz_gra.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polawiacz_gra.States
{
    public class MenuState : State
    {

        private List<Component> _components;

        public MenuState(Game1 game, GraphicsDevice graphicDevice, ContentManager content) : base(game, graphicDevice, content)
        {
            var buttonTexture = content.Load<Texture2D>("Controls/Button");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");
            var targetSprite = content.Load<Texture2D>("target2");
            var crosshairsSprite = content.Load<Texture2D>("crosshairs");
            var backgroundSprite = content.Load<Texture2D>("tlo");
            var gameFont = content.Load<SpriteFont>("galleryFont");
            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            var levelsGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 300),
                Text = "Levels",
            };

            levelsGameButton.Click += LevelsGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 400),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;


            _components = new List<Component>()
            {
                newGameButton,
                levelsGameButton,
                quitGameButton,
            };
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void LevelsGameButton_Click(object sender, EventArgs e)
        {
             _game.ChanegeState(new MenuLevels(_game, _graphicsDevice, _content));
            //_game.Exit();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            // laduje nowy stan gry
            _game.ChanegeState(new GameState(_game, _graphicsDevice, _content));

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();

           foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

           // spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // usuwa sprity jesli nie sa potrzebne
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
