using Microsoft.Xna.Framework;
using ProjectCrawler.Management;
using ProjectCrawler.Objects.Generic.GameBase;

namespace ProjectCrawler.Objects.Game.Effect
{
    public class BasicFlash : GameObject
    {
        protected Vector2 startSize;
        protected Vector2 endSize;
        protected int duration;
        protected int timer;
        protected Color color;

        public BasicFlash(Color C, Vector2 Position, Vector2 StartSize, Vector2 EndSize, int Duration)
        {
            color = C;
            position = Position;
            startSize = StartSize;
            endSize = EndSize;
            duration = Duration;
            timer = duration;
        }

        public override void Render()
        {
            Renderer.DrawSprite(
                GlobalConstants.GLOW_IMAGE_TAG,
                position,
                Vector2.Lerp(endSize, startSize, timer / (float)duration),
                ColorFilter: color * (timer / (float)duration),
                Depth: 0.0f,
                DrawAdditive: true);
        }

        public override void Update()
        {
            timer--;
            if (timer < 0)
            {
                LevelManager.CurrentLevel.DeregisterGameObject(this);
            }
        }
    }
}
