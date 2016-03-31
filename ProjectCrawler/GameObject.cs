using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrawler
{
    public interface GameObject
    {
        /// <summary>
        /// Object position.
        /// </summary>
        Vector2 Position
        {
            get;
        }

        /// <summary>
        /// Rectangle defining boudning box of object.
        /// </summary>
        Rectangle BoundingBox
        {
            get;
        }

        /// <summary>
        /// Updates the object.
        /// </summary>
        void Update();

        /// <summary>
        /// Renders the object.
        /// </summary>
        void Render();
    }
}
