using Microsoft.Xna.Framework;
using ProjectCrawler.Management;
using ProjectCrawler.Objects.Generic.Utility;

namespace ProjectCrawler.Objects.Game.Enemy
{
    public class FunnyEnemy : AbstractEnemy
    {
        private const int MAX_HEALTH = 10;

        private readonly int[] PATH_DURATIONS = { 300, 140, 300, 140 };
        private readonly Vector2[] PATH_MOTION = { new Vector2(2, 0), new Vector2(0, 2), new Vector2(-2, 0), new Vector2(0, -2) };

        private readonly int[] FRAME_DURATIONS = { 5, 5, 5, 5 };
        private readonly float[] FRAME_ANGLE_OFFSETS = { 0.0f, 0.05f, 0.0f, -0.05f };
        private readonly Vector2[] FRAME_POS_OFFSETS = { new Vector2(0), new Vector2(3, -6), new Vector2(0), new Vector2(-3, -6) };

        private const int WIDTH = 48;
        private const int HEIGHT = 48;
        private readonly Vector2 SIZE = new Vector2(WIDTH, HEIGHT);
        private readonly Vector2 SHADOW_OFFSET = new Vector2(0, HEIGHT / 2);
        private readonly Vector2 SHADOW_SIZE = new Vector2(60, 30);

        private int pathFrameNumber;
        private int pathFrameTimer;

        private int animFrameNumber;
        private int animFrameTimer;

        /// <summary>
        /// Constructor for the FunnyEnemy.
        /// </summary>
        /// <param name="StartPosition">Start position of the enemy.</param>
        public FunnyEnemy(Vector2 StartPosition) : base(Polygon.CreateRectangle(WIDTH, HEIGHT, StartPosition))
        {
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
            Renderer.DrawSprite(
                "funnyEnemy", 
                position + FRAME_POS_OFFSETS[animFrameNumber], 
                SIZE, 
                FRAME_ANGLE_OFFSETS[animFrameNumber], 
                Depth: Renderer.GenerateDepthFromScreenPosition(position));
            Renderer.DrawSprite(
                "dropShadow", 
                position + SHADOW_OFFSET, 
                SHADOW_SIZE, 
                ColorFilter: Color.White * 0.4f, 
                Depth: 1.0f);
        }

        /// <summary>
        /// Updates the FunnyEnemy.
        /// </summary>
        public override void Update()
        {
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
