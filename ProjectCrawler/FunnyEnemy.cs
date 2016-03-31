using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrawler
{
    public class FunnyEnemy : Enemy
    {
        private const int MAX_HEALTH = 10;

        public FunnyEnemy(Vector2 StartPosition)
        {
            position = StartPosition;
            health = MAX_HEALTH;
        }

        public override void Render()
        {
            base.Render();

            Renderer.DrawSprite("funnyEnemy", position, new Vector2(64));
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
