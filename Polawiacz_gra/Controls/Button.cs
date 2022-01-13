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

        

        private MouseState _currentMouse; //po to by wykrywac lewy przycisk myszy
        private SpriteFont _font; //font tekstu
        private bool _isHoverring; //czy mysz jest na przycisku
        private MouseState _previousMouse; //poprzedni stan myszy
        private Texture2D _texture; //obrazek przycisku

       
        // event ktory sie wykonuje po klikniecu na przycisk
        public event EventHandler Click;
        // czy dany przycisk byl przycisniety
        public bool Clicked { get; private set; }
        //kolor tekstu w przyciskach
        public Color KolorTekstu { get; set; }
        //pozycja
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public string Tekst { get; set; }

     
        //obraz przycisku jego czcionka i kolor tekstu
        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            KolorTekstu = Color.Black;
        }

        /// <summary>
        /// wypisywanie tekstu w przycisku
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var kolor = Color.White;

            // jesli najade myszka zmienia kolor
            if (_isHoverring)
            {
                kolor = Color.Gray;
            }
            spriteBatch.Draw(_texture, Rectangle, kolor);

            //jesli tekst przycisku nie jest pusty ustawiamy go po srodku
            if(!string.IsNullOrEmpty(Tekst))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Tekst).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Tekst).Y / 2);

                spriteBatch.DrawString(_font, Tekst, new Vector2(x, y), KolorTekstu);
            }
        }
        /// <summary>
        /// sprawdzanie czy dany przycisk zostal najaechany myszka i klikniety
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //stany myszy
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
        
    }
}
