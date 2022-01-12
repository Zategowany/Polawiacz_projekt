using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Polawiacz_gra.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polawiacz_gra.States
{
    public class Rules : State
    {

        private List<Component> _components;

        public Rules(Game1 game, GraphicsDevice graphicDevice, ContentManager content) : base(game, graphicDevice, content)
        {
            var buttonTexture = content.Load<Texture2D>("Controls/Button");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");

            var menuGameButton = new Button(buttonTexture, buttonFont)

            {
                Position = new Vector2(1280 / 2 - 424 / 2, 800),
                Text = "Menu",
            };

            menuGameButton.Click += MenuGameButton_Click;

            _components = new List<Component>()
            {
               menuGameButton,
            };

        }
        private void MenuGameButton_Click(object sender, EventArgs e)
        {
            _game.ChanegeState(new MenuState(_game, _graphicsDevice, _content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //tlo
            spriteBatch.Draw(_content.Load<Texture2D>("tlozasady"), new Vector2(0, 0), Color.White);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);
      
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
