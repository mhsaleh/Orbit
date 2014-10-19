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
    class Ship
    {
        private const float SCALE = 1f;
        private const int GRAVITY = 3000;
        private const float THRUST = .1f;
        private const float AMPLIFIER = 1.1f;
        private const float BOOST = 1.1f;
        private const float FRICTION = .5f;

        private static Texture2D sprite;
        private static int spriteRadius;
        private Vector2 position;
        private Vector2 velocity;
        private BoundingSphere collision;
        private bool alive;

        public Ship(int x = 0, int y = 0, float xVelocity = 0, float yVelocity = 0)
        {
            position = new Vector2(x, y);
            velocity = new Vector2(xVelocity, yVelocity);
            alive = true;
        }

        public void Load(ContentManager content, String file)
        {
            sprite = content.Load<Texture2D>(file);
            spriteRadius = sprite.Width / 2;

            collision = new BoundingSphere(new Vector3(position.X, position.Y, 0), spriteRadius * SCALE);
        }

        public void Update(KeyboardState keyPress, Boundary boundary, Field field,
                           Orb[] orbs, int orbCount)
        {
            
            Vector2 tempPosition = position + velocity;
            BoundingSphere tempCollision = new BoundingSphere(new Vector3(tempPosition.X, tempPosition.Y, 0), spriteRadius * SCALE);
            Vector2 gravitySum = new Vector2(0, 0);

            for (int i = 0; i < orbCount; i++)
            {
                Laser[] lasers = orbs[i].GetLasers();
                int laserCount = orbs[i].GetLaserCount();

                for (int j = 0; j < laserCount; j++)
                {
                    if (collision.Intersects(lasers[j].GetCollision()))
                    {
                        alive = false;
                        lasers[j].SetAlive(false);
                    }
                }


                Vector2 gravityVector = (orbs[i].GetPosition() - position);
                gravitySum += gravityVector;
                gravityVector.Normalize();
                float distance = Vector2.Distance(orbs[i].GetPosition(), position);

                int rotation = 1;
                double radians = 2 * Math.PI * rotation / 360;

                if (keyPress.IsKeyDown(Keys.A)) {
                    velocity.X = velocity.X * (float)Math.Cos(-radians) - velocity.Y * (float)Math.Sin(-radians);
                    velocity.Y = velocity.X * (float)Math.Sin(-radians) + velocity.Y * (float)Math.Cos(-radians);
                }
                else if (keyPress.IsKeyDown(Keys.D))
                {
                    velocity.X = velocity.X * (float)Math.Cos(radians) - velocity.Y * (float)Math.Sin(radians);
                    velocity.Y = velocity.X * (float)Math.Sin(radians) + velocity.Y * (float)Math.Cos(radians);
                }

                velocity += GRAVITY * gravityVector / (distance * distance);

                if (tempCollision.Intersects(orbs[i].GetOrbCollision())) {
                    alive = false;
                } 
            }

            gravitySum.Normalize();
            if (!tempCollision.Intersects(boundary.GetCollisionMiddle()) &&
                !tempCollision.Intersects(boundary.GetCollisionRight()) &&
                !tempCollision.Intersects(boundary.GetCollisionLeft())) 
            {
                alive = false;
            }
            
            position += velocity;
            collision.Center = new Vector3(position.X, position.Y, 0);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            if (alive)
            {
                float angle = (float)Math.Atan2(velocity.Y, velocity.X) + (float)Math.PI / 2f;
                spriteBatch.Draw(sprite, position, null, Color.White, angle, new Vector2(spriteRadius * SCALE, spriteRadius * SCALE), SCALE, SpriteEffects.None, 0f);
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

        public bool IsAlive() {
            return alive;
        }
    }
}
