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
    class Orb
    {
        private const int RADIUS = 50;
        private const int MAX_LASERS = 36;
        private const int LASER_SPEED = 1;
        private const int LASER_SEPARATION_ANGLE = 90;
        private const int ROTATION_ANGLE = 15;

        private static Texture2D spriteGreen;
        private static Texture2D spriteRed;
        private Vector2 position;

        private BoundingSphere collision;
        private BoundingBox outOfBounds;

        private Laser[] lasers;
        private int laserCount;

        private static bool inField;
        private bool spawned;

        public Orb(int x = 0, int y = 0, int boundsX = 0, int boundsY = 0)
        {
            position = new Vector2(x, y);
            collision = new BoundingSphere(new Vector3(position.X, position.Y, 0), RADIUS);
            outOfBounds = new BoundingBox(new Vector3(0, 0, 0), new Vector3(boundsX, boundsY, 0));

            lasers = new Laser[MAX_LASERS];
            laserCount = 0;

            inField = false;
            spawned = false;
        }

        public void Load(ContentManager content, String fileGreen, String fileRed)
        {
            spriteGreen = content.Load<Texture2D>(fileGreen);
            spriteRed = content.Load<Texture2D>(fileRed);
        }

        public void Update(Random rand, bool inField, Ship ship, Orb[] orbs)
        {
            for (int i = 0; i < laserCount; i++)
            {
                if (lasers[i] != null)
                {
                    if (!lasers[i].IsAlive())
                    {
                        for (int j = i; j <= laserCount; j++)
                        {
                            lasers[j] = lasers[j + 1];
                        }
                        laserCount--;
                    }
                    else
                    {
                        lasers[i].Update(ship, orbs);
                    }
                }
            }
            if (inField)
            {
                if (!spawned)
                {
                    spawned = true;

                    int rotation = rand.Next(LASER_SEPARATION_ANGLE / ROTATION_ANGLE) * ROTATION_ANGLE;

                    for (int i = 0; i < 360 / LASER_SEPARATION_ANGLE; i++)
                    {
                        if (laserCount < MAX_LASERS - 2)
                        {
                            double radians = 2 * Math.PI * (i * LASER_SEPARATION_ANGLE + rotation) / 360;
                            float xVelocity = (float)Math.Cos(radians) * LASER_SPEED;
                            float yVelocity = (float)Math.Sin(radians) * LASER_SPEED;

                            lasers[laserCount] = new Laser((int)position.X, (int)position.Y, xVelocity, yVelocity);
                            laserCount++;
                        }
                    }
                }
            }
            else
            {
                spawned = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < laserCount; i++)
            {
                if (lasers[i] != null)
                {
                    lasers[i].Draw(spriteBatch);
                }
            }

            if (inField)
            {
                spriteBatch.Draw(spriteRed, new Vector2(position.X - RADIUS, position.Y - RADIUS), Color.White);
            }
            else
            {
                spriteBatch.Draw(spriteGreen, new Vector2(position.X - RADIUS, position.Y - RADIUS), Color.White);
            }
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public BoundingSphere GetOrbCollision()
        {
            return collision;
        }

        public BoundingBox GetOutOfBounds()
        {
            return outOfBounds;
        }

        public void SetInField(bool newInField)
        {
            inField = newInField;
        }

        public bool IsInField()
        {
            return inField;
        }

        public Laser[] GetLasers()
        {
            return lasers;
        }

        public int GetLaserCount()
        {
            return laserCount;
        }

    }
}
