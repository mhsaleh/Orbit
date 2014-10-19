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
    class Missile
    {
        private const int RADIUS = 14;
        private const int GRAVITY = 4;


        private static Texture2D sprite;
        private Vector2 position;
        private Vector2 velocity;
        private BoundingSphere collision;
        private bool alive;

        public Missile(int x, int y, float xVelocity, float yVelocity)
        {
            position = new Vector2(x, y);
            velocity = new Vector2(xVelocity, yVelocity);
            collision = new BoundingSphere(new Vector3(position.X, position.Y, 0), RADIUS);
            alive = false;
        }

        public void Load(ContentManager content, String file)
        {
            sprite = content.Load<Texture2D>(file);
        }

        public void Update(Ship ship, Orb orb, Boundary boundary)
        {

            Vector2 gravityVector = (ship.GetPosition() - position);
            gravityVector.Normalize();

            velocity = gravityVector * GRAVITY;
            position += velocity;
            collision.Center = new Vector3(position.X, position.Y, 0);


            if (collision.Intersects(orb.GetOrbCollision()) || !collision.Intersects(orb.GetOutOfBounds()))
                alive = false;
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            if (alive)
            {
                spriteBatch.Draw(sprite, new Vector2(position.X - RADIUS, position.Y - RADIUS), Color.White);
            }
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public BoundingSphere GetCollision()
        {
            return collision;
        }

        public bool IsAlive()
        {
            return alive;
        }
    }
}
