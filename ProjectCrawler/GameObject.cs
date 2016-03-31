using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrawler
{
    public interface GameObject
    {
        Vector2 Position
        {
            get;
        }

        Rectangle BoundingBox
        {
            get;
        }

        void Update();

        void Render();
    }
}
