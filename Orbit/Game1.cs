using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Orbit
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int BACK_BUFFER_HEIGHT = 768;
        const int BACK_BUFFER_WIDTH = 1366;

        const int ORB_1_X_POSITION = 455;
        const int ORB_1_Y_POSITION = 384;

        const int ORB_2_X_POSITION = 910;
        const int ORB_2_Y_POSITION = 384;

        const int SHIP_X_POSITION = 455;
        const int SHIP_Y_POSITION = 200;
        const float SHIP_X_VELOCITY = 4.17f;
        const float SHIP_Y_VELOCITY = 0f;


        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private KeyboardState keyPress;
        private Random rand;

        private Ship ship;
        private Field field;
        private Boundary boundary;
        private Missile missile;

        private Orb[] orbs;
        private const int ORB_COUNT = 2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();

            keyPress = Keyboard.GetState();
            rand = new Random(Environment.TickCount);

            graphics.PreferredBackBufferHeight = BACK_BUFFER_HEIGHT;
            graphics.PreferredBackBufferWidth = BACK_BUFFER_WIDTH;

            graphics.ApplyChanges();

            ship = new Ship(SHIP_X_POSITION, SHIP_Y_POSITION,
                                      SHIP_X_VELOCITY, SHIP_Y_VELOCITY);

            field = new Field();
            boundary = new Boundary();

            orbs = new Orb[2];
            orbs[0] = new Orb(ORB_1_X_POSITION, ORB_1_Y_POSITION,
                              BACK_BUFFER_WIDTH, BACK_BUFFER_HEIGHT);

            orbs[1] = new Orb(ORB_2_X_POSITION, ORB_2_Y_POSITION,
                              BACK_BUFFER_WIDTH, BACK_BUFFER_HEIGHT);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // temp objects for static sprite loading
            Orb tempOrb = new Orb();
            Ship tempShip = new Ship();
            Boundary tempBoundary = new Boundary();
            Field tempField = new Field();
            Laser tempLaser = new Laser();

            tempOrb.Load(Content, "OrbGreen (100x100)", "OrbRed (100x100)");
            tempShip.Load(Content, "Ship5 (Small)");
            tempBoundary.Load(Content, "BoundaryGreen (1366x768)", "BoundaryRed (1366x768)");
            tempField.Load(Content, "Field2 (1366x768)", "InnerField (200x200)");
            tempLaser.Load(Content, "Laser (10x10)");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (ship.IsAlive())
            {
                keyPress = Keyboard.GetState();

                ship.Update(keyPress, boundary, field,
                                 orbs, ORB_COUNT);
            }

            bool inField = (!ship.GetCollision().Intersects(field.GetCollisionMiddle()) &&
               !ship.GetCollision().Intersects(field.GetCollisionRight()) &&
               !ship.GetCollision().Intersects(field.GetCollisionLeft())) ||
               ship.GetCollision().Intersects(field.GetCollisionInnerLeft()) ||
               ship.GetCollision().Intersects(field.GetCollisionInnerRight());

            boundary.SetInField(inField);

            for (int i = 0; i < ORB_COUNT; i++)
            {
                orbs[i].Update(rand, inField, ship, orbs);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            field.Draw(spriteBatch);
            boundary.Draw(spriteBatch);

            ship.Draw(spriteBatch);

            for (int i = 0; i < ORB_COUNT; i++)
            {
                orbs[i].Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
