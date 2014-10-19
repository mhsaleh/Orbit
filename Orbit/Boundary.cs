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
    class Boundary
    {
        private static int RADIUS = 384;
        private static int RADIUS_COLLISION_INNER = 325;
        private static int RADIUS_COLLISION_OUTER = 365;

        private static Texture2D spriteGreen;
        private static Texture2D spriteRed;
        private Vector2 position;

        private BoundingBox collisionMiddle;
        private BoundingSphere collisionRight;
        private BoundingSphere collisionLeft;
        private static bool inField;

        public Boundary(int x = 0, int y = 0)
        {
            position = new Vector2(x, y);
            collisionMiddle = new BoundingBox(new Vector3(353, 50, 0), new Vector3(1031, 725, 0));
            collisionRight = new BoundingSphere(new Vector3(990, 384, 0), 340);
            collisionLeft = new BoundingSphere(new Vector3(387, 384, 0), 340);
            inField = false;
        }

        public void Load(ContentManager content, String fileGreen, String fileRed)
        {
            spriteGreen = content.Load<Texture2D>(fileGreen);
            spriteRed = content.Load<Texture2D>(fileRed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (inField)
            {
                spriteBatch.Draw(spriteRed, new Vector2(position.X, position.Y), Color.White);
            }
            else
            {
                spriteBatch.Draw(spriteGreen, new Vector2(position.X, position.Y), Color.White);
            }
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public BoundingBox GetCollisionMiddle()
        {
            return collisionMiddle;
        }

        public BoundingSphere GetCollisionRight()
        {
            return collisionRight;
        }

        public BoundingSphere GetCollisionLeft()
        {
            return collisionLeft;
        }

        public void SetInField(bool newInField)
        {
            inField = newInField;
        }

        public bool IsInField()
        {
            return inField;
        }

    }
}

