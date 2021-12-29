using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Polawiacz_gra.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polawiacz_gra.States
{
    public class MenuLevels : State
    {

        private List<Component> _components;

        public MenuLevels(Game1 game, GraphicsDevice graphicDevice, ContentManager content) : base(game, graphicDevice, content)
        {
            var buttonTexture = content.Load<Texture2D>("Controls/Button");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");
            var targetSprite = content.Load<Texture2D>("target2");
            var crosshairsSprite = content.Load<Texture2D>("crosshairs");
            var backgroundSprite = content.Load<Texture2D>("tlo");
            var gameFont = content.Load<SpriteFont>("galleryFont");
            var level1GameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "1",
            };
            level1GameButton.Click += NewGameButton_Click;

            var level2GameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 300),
                Text = "2",
            };
            level2GameButton.Click += LoadGameButton_Click;

            var level3GameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 400),
                Text = "3",
            };
            level3GameButton.Click += QuitGameButton_Click;


            _components = new List<Component>()
            {
                level1GameButton,
                level2GameButton,
                level3GameButton,
            };
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.ChanegeState(new GameState3(_game, _graphicsDevice, _content));
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            _game.ChanegeState(new GameState2(_game, _graphicsDevice, _content));
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
