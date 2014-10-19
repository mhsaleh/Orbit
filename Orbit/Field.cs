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
    class Field
    {
        private static Texture2D spriteOuter;
        private static Texture2D spriteInner;

        private Vector2 positionOuter;
        private Vector2 positionInnerLeft;
        private Vector2 positionInnerRight;
        private int radius;

        private BoundingBox collisionMiddle;
        private BoundingSphere collisionRight;
        private BoundingSphere collisionLeft;

        private BoundingSphere collisionInnerRight;
        private BoundingSphere collisionInnerLeft;

        public Field(int x = 0, int y = 0)
        {
            positionOuter = new Vector2(x, y);
            collisionMiddle = new BoundingBox(new Vector3(403, 100, 0), new Vector3(981, 675, 0));
            collisionRight = new BoundingSphere(new Vector3(990, 384, 0), 290);
            collisionLeft = new BoundingSphere(new Vector3(387, 384, 0), 290);

            positionInnerRight = new Vector2(910, 384);
            positionInnerLeft = new Vector2(455, 384);
            radius = 100;

            collisionInnerRight = new BoundingSphere(new Vector3(910, 384, 0), radius);
            collisionInnerLeft = new BoundingSphere(new Vector3(455, 384, 0), radius);
        }

        public void Load(ContentManager content, String fileOuter, String fileInner)
        {
            spriteOuter = content.Load<Texture2D>(fileOuter);
            spriteInner = content.Load<Texture2D>(fileInner);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(spriteOuter, new Vector2(positionOuter.X, positionOuter.Y), Color.White);
            spriteBatch.Draw(spriteInner, new Vector2(positionInnerLeft.X - radius, positionInnerLeft.Y - radius), Color.White);
            spriteBatch.Draw(spriteInner, new Vector2(positionInnerRight.X - radius, positionInnerRight.Y - radius), Color.White);
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

        public BoundingSphere GetCollisionInnerLeft()
        {
            return collisionInnerLeft;
        }

        public BoundingSphere GetCollisionInnerRight()
        {
            return collisionInnerRight;
        }
    }
}
