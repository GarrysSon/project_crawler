using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrawler
{
    /// <summary>
    /// Represents an abstract enemy.
    /// </summary>
    public abstract class Enemy : GameObject
    {
        protected int health;
        public int Health
        {
            get
            {
                return health;
            }
        }

        protected Rectangle boundingBox;
        public Rectangle BoundingBox
        {
            get
            {
                return boundingBox;
            }
        }

        protected Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public virtual void Render()
        {
        }

        public virtual void Update()
        {
        }
    }
}
