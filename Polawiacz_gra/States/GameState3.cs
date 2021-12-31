using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Polawiacz_gra.Controls;

namespace Polawiacz_gra.States
{
    public class GameState3 : State
    {

        private Vector2 pozycjaKursora;

        //pozycja celu
        Vector2[] pozycja = new Vector2[10];
        //odleglosc kursora od danego celu
        float[] odleglosc = new float[10];

        const int targetRadius = 45;
        const int promienKursora = 25;
        private List<Component> _components;

        MouseState mState;
        bool mReleased = true;
        int trash = 0;
        double timer = 0;
        int zlewybory = 0;
        int[] numercelu = new int[10];
        int[] wybortargetu = new int[10];

        int losowanie = 1;
        float wynik;


        public GameState3(Game1 game, GraphicsDevice graphicDevice, ContentManager content) : base(game, graphicDevice, content)
        {

            var buttonTexture = content.Load<Texture2D>("Controls/Button");
            var buttonFont = content.Load<SpriteFont>("Fonts/Font");
            var target2Sprite = content.Load<Texture2D>("target2");
            var targetSprite = content.Load<Texture2D>("target");
            var crosshairsSprite = content.Load<Texture2D>("crosshairs");
            var backgroundSprite = content.Load<Texture2D>("tlo");
            var gameFont = content.Load<SpriteFont>("galleryFont");

            var menuGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 700),
                Text = "Menu",
            };

            menuGameButton.Click += QuitGameButton_Click;
            _components = new List<Component>()
            {

                menuGameButton,
            };


        }
        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.ChanegeState(new MenuState(_game, _graphicsDevice, _content));
        }


        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {


            if (timer == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    numercelu[i] = 1;

                }
            }


            //czas zliczany do 10 pkt
            if (trash > 0)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (trash < 0)
            {
                trash = 0;
            }




            mState = Mouse.GetState();

            pozycjaKursora = new Vector2(mState.X, mState.Y);

            //petla sie wykonuje tylko raz na poczatku gry

            if (losowanie == 1)
            {
                Random rand = new Random();

                //losowanie pozycji w okienku gry
                for (int i = 0; i < 10; i++)
                {
                    pozycja[i].X = rand.Next(45, 1280 - targetRadius);
                    pozycja[i].Y = rand.Next(45, 900 - targetRadius);
                    wybortargetu[i] = rand.Next(0, 3);
                    if (wybortargetu[i] == 0 || wybortargetu[i] == 1)
                        trash++;
                }
                losowanie = 0;
            }


            if (mState.LeftButton == ButtonState.Pressed && mReleased == true)
            {

                //sprawdzanie odleglosci od celu
                for (int i = 0; i < 10; i++)
                {
                    odleglosc[i] = Vector2.Distance(pozycja[i], mState.Position.ToVector2());
                    if (odleglosc[i] < targetRadius && trash > 0)
                    {
                        //wybieranie czegos innego niz smieci nie odejmuje ilosci smieci
                        if (wybortargetu[i] == 0 || wybortargetu[i] == 1)
                        {
                            trash--;
                        }
                        if (wybortargetu[i] == 2)
                        {
                            zlewybory++;
                        }

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

                wynik = (float)(1000 / timer);
                foreach (var component in _components)
                    component.Update(gameTime);
            }



        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            //jesli kliknalem na obrazek znika

            for (int i = 0; i < 10; i++)
            {
                if (numercelu[i] == 1)
                {

                    switch (wybortargetu[i])
                    {
                        case 0:
                            spriteBatch.Draw(_content.Load<Texture2D>("target"), new Vector2(pozycja[i].X - targetRadius, pozycja[i].Y - targetRadius), Color.White);
                            break;
                        case 1:
                            spriteBatch.Draw(_content.Load<Texture2D>("target2"), new Vector2(pozycja[i].X - targetRadius, pozycja[i].Y - targetRadius), Color.White);
                            break;
                        case 2:
                            spriteBatch.Draw(_content.Load<Texture2D>("target3"), new Vector2(pozycja[i].X - targetRadius, pozycja[i].Y - targetRadius), Color.White);
                            break;

                    }

                }
            }


            //gafika kursora
            spriteBatch.Draw(_content.Load<Texture2D>("crosshairs"), new Vector2(pozycjaKursora.X - promienKursora, pozycjaKursora.Y - promienKursora), Color.White);
            //czas

            spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Czas: " + Math.Ceiling(timer).ToString(), new Vector2(0, 30), Color.Red);
            //wynik
            spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Pozostale smieci: " + trash.ToString(), new Vector2(0, 0), Color.White);
            //Zebranie wszystkiego
            if (trash == 0)
            {
                spriteBatch.DrawString(_content.Load<SpriteFont>("galleryFont"), "Zebrales wszystko!!!! Twoj wynik to: " + wynik.ToString() + "\n Dokonales " + zlewybory.ToString() + " zlych wyborow.", new Vector2(400, 450), Color.White);
                foreach (var component in _components)
                    component.Draw(gameTime, spriteBatch);
            }

        }

    }
}
