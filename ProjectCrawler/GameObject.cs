using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrawler
{
    public abstract class GameObject
    {
        /// <summary>
        /// Object position.
        /// </summary>
        protected Vector2 position;
        public Vector2 Position
        {
            get;
        }

        /// <summary>
        /// Rectangle defining bounding box of object.
        /// </summary>
        protected Rectangle boundingBox;
        public Rectangle BoundingBox
        {
            get;
        }

        /// <summary>
        /// Updates the object.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Renders the object.
        /// </summary>
        public abstract void Render();
    }
}
