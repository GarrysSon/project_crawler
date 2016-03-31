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

        private readonly int[] FRAME_DURATIONS = { 5, 5, 5, 5 };
        private readonly float[] FRAME_ANGLE_OFFSETS = { 0.0f, 0.08f, 0.0f, -0.08f };
        private readonly Vector2[] FRAME_POS_OFFSETS = { new Vector2(0), new Vector2(5, -10), new Vector2(0), new Vector2(-5, -10) };

        private int pathFrameNumber;
        private int pathFrameTimer;

        private int animFrameNumber;
        private int animFrameTimer;

        /// <summary>
        /// Constructor for the FunnyEnemy.
        /// </summary>
        /// <param name="StartPosition">Start position of the enemy.</param>
        public FunnyEnemy(Vector2 StartPosition)
        {
            position = StartPosition;
            health = MAX_HEALTH;
            pathFrameNumber = 0;
            pathFrameTimer = 0;
            animFrameNumber = 0;
            animFrameTimer = 0;
        }

        /// <summary>
        /// Renders the FunnyEnemy.
        /// </summary>
        public override void Render()
        {
            base.Render();

            Renderer.DrawSprite("funnyEnemy", position + FRAME_POS_OFFSETS[animFrameNumber], new Vector2(64), FRAME_ANGLE_OFFSETS[animFrameNumber]);
        }

        /// <summary>
        /// Updates the FunnyEnemy.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Update the path
            if (++pathFrameTimer == PATH_DURATIONS[pathFrameNumber])
            {
                pathFrameTimer = 0;
                pathFrameNumber = (pathFrameNumber + 1) % PATH_DURATIONS.Length;
            }

            // Move along the path
            position += PATH_MOTION[pathFrameNumber];

            // Update the animation
            if (++animFrameTimer == FRAME_DURATIONS[animFrameNumber])
            {
                animFrameTimer = 0;
                animFrameNumber = (animFrameNumber + 1) % FRAME_DURATIONS.Length;
            }
        }
    }
}
