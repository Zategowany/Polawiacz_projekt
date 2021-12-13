using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polawiacz_gra.Controls
{
    public class Button : Component
    {

        #region Fields

        private MouseState _currentMouse; //po to by wykrywac lewy przycisk myszy
        private SpriteFont _font; //font tekstu
        private bool _isHoverring; //czy mysz jest na przycisku
        private MouseState _previousMouse; //poprzedni stan myszy
        private Texture2D _texture; //obrazek przycisku

        #endregion

        #region Properties

        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public string Text { get; set; }

        #endregion

        #region Methods

        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.Black;
        }

        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            // jesli najade myszka zmienia kolor
            if (_isHoverring)
            {
                colour = Color.Gray;
            }
            spriteBatch.Draw(_texture, Rectangle, colour);

            //jesli tekst przycisku nie jest pusty ustawiamy go po srodku
            if(!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            //sprawdzanei warunku czy mysz jest na przycisku
            _isHoverring = false;
            if(mouseRectangle.Intersects(Rectangle))
            {
                _isHoverring = true;
                //sprawdzenie czy po najechaniu wcisnelismy przycisk
                if(_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    // jesli "Click" nie jest pusty to chcemy go uzyc
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
        #endregion
    }
}
