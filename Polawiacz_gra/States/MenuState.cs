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

            //tworzenie przyciskow
            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(1280/2 -424/2, 200),
                Tekst = "Nowa Gra",
            };

            //odwloanie do eventu ktory ma sie wykonac po wcisniecu przycisku
            newGameButton.Click += NewGameButton_Click;

            var levelsGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(1280/2 - 424/2, 300),
                Tekst = "Poziomy",
            };

            levelsGameButton.Click += LevelsGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(1280/2 - 424/2, 500),
                Tekst = "Wyjdz z Gry",
            };

            quitGameButton.Click += QuitGameButton_Click;

            var rulesGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(1280/2 - 424/2, 400),
                Tekst = "Zasady",
            };

            rulesGameButton.Click += RulesGameButton_Click;

            //lista przyciskow
            _components = new List<Component>()
            {
                newGameButton,
                levelsGameButton,
                quitGameButton,
                rulesGameButton,
            };
        }
        //funkcje ktore wykonuja przyciski
        private void RulesGameButton_Click(object sender, EventArgs e)
        {
            _game.ChanegeState(new Rules(_game, _graphicsDevice, _content));
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void LevelsGameButton_Click(object sender, EventArgs e)
        {
             _game.ChanegeState(new MenuLevels(_game, _graphicsDevice, _content));
            
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            // laduje nowy stan gry
            _game.ChanegeState(new GameState(_game, _graphicsDevice, _content));

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            //rysowanie kazdego obiektu z listy
           foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

           
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // usuwa sprity jesli nie sa potrzebne
        }

        public override void Update(GameTime gameTime)
        {
            //uaktualnienie kazdego przycisku
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
