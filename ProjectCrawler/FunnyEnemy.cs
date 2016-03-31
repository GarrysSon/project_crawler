using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrawler
{
    public class FunnyEnemy : Enemy
    {
        private const int MAX_HEALTH = 10;

        private readonly int[] PATH_DURATIONS = { 300, 140, 300, 140 };
        private readonly Vector2[] PATH_MOTION = { new Vector2(2, 0), new Vector2(0, 2), new Vector2(-2, 0), new Vector2(0, -2) };

        private readonly int[] FRAME_DURATIONS = { 10, 10, 10, 10 };
        //private readonly float[] FRAME_ANGLE_OFFSETS = { 0.0f, 0.0}

        private int frameNumber;
        private int frameTimer;

        /// <summary>
        /// Constructor for the FunnyEnemy.
        /// </summary>
        /// <param name="StartPosition">Start position of the enemy.</param>
        public FunnyEnemy(Vector2 StartPosition)
        {
            position = StartPosition;
            health = MAX_HEALTH;
            frameNumber = 0;
            frameTimer = 0;
        }

        /// <summary>
        /// Renders the FunnyEnemy.
        /// </summary>
        public override void Render()
        {
            base.Render();

            Renderer.DrawSprite("funnyEnemy", position, new Vector2(64));
        }

        /// <summary>
        /// Updates the FunnyEnemy.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Update the path
            if (++frameTimer == PATH_DURATIONS[frameNumber])
            {
                frameTimer = 0;
                frameNumber = (frameNumber + 1) % PATH_DURATIONS.Length;
            }

            // Move along the path
            position += PATH_MOTION[frameNumber];
        }
    }
}
