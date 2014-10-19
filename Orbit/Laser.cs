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
    class Laser
    {
        private const int RADIUS = 5;

        private static Texture2D sprite;
        private Vector2 position;
        private Vector2 velocity;
        private BoundingSphere collision;
        private bool alive;

        public Laser(int x = 0, int y = 0, float xVelocity = 0, float yVelocity = 0)
        {
            position = new Vector2(x, y);
            velocity = new Vector2(xVelocity, yVelocity);
            collision = new BoundingSphere(new Vector3(position.X, position.Y, 0), RADIUS);
            alive = true;
        }

        public void Load(ContentManager content, String file)
        {
            sprite = content.Load<Texture2D>(file);
        }

        public void Update(Ship ship, Orb[] orbs)
        {
            position += velocity;
            collision.Center = new Vector3(position.X, position.Y, 0);


            if (!collision.Intersects(orbs[0].GetOutOfBounds()))
                alive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                float angle = (float)Math.Atan2(velocity.Y, velocity.X);
                spriteBatch.Draw(sprite, position, null, Color.White, angle, new Vector2(RADIUS, RADIUS), 1.0f, SpriteEffects.None, 0f);
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

        public void SetAlive(bool newAlive)
        {
            alive = newAlive;
        }

        public bool IsAlive()
        {
            return alive;
        }
    }
}

