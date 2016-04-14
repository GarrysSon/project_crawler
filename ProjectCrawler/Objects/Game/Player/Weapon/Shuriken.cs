using Microsoft.Xna.Framework;
using ProjectCrawler.Management;
using ProjectCrawler.Objects.Game.Level.Component;
using ProjectCrawler.Objects.Generic.GameBase;
using ProjectCrawler.Objects.Generic.Utility;

namespace ProjectCrawler.Objects.Game.Player.Weapon
{
    public class Shuriken : GameObject
    {
        /// <summary>
        /// Size and polygon related constants.
        /// </summary>
        private const int SIDE_LENGTH = 16;
        private static readonly Vector2 SIZE = new Vector2(SIDE_LENGTH);
        private static readonly Vector2[] POLY_POINTS =
            new Vector2[]
            {
                new Vector2(-(SIDE_LENGTH / 2)),
                new Vector2(-(SIDE_LENGTH / 2), SIDE_LENGTH / 2),
                new Vector2(SIDE_LENGTH / 2, SIDE_LENGTH / 2),
                new Vector2(SIDE_LENGTH / 2, -(SIDE_LENGTH / 2))
            };

        /// <summary>
        /// Shuriken speed related constants
        /// </summary>
        private const float SPEED = 10f;
        private const float ROTATION_SPEED = 0.2f;
        private const float BOUNCE_SPEED = 4f;
        private const float BOUNCE_Y_VELOCITY_ADJUST = -6f;
        private const float GRAVITY = 0.5f;

        /// <summary>
        /// Post-live fade speed;
        /// </summary>
        private const float FADE_SPEED = -0.04f;

        /// <summary>
        /// Velocity of the shuriken.
        /// </summary>
        private Vector2 velocity;

        /// <summary>
        /// Current angle of the shuriken.
        /// </summary>
        private float angle;

        /// <summary>
        /// The live state of the shuriken. True if it is live.
        /// </summary>
        private bool isLive;

        /// <summary>
        /// Post-live fade timer.
        /// </summary>
        private float fadeTimer;

        /// <summary>
        /// Constructor for the Shuriken.
        /// </summary>
        /// <param name="StartPosition">The start position of the shuriken.</param>
        /// <param name="Direction">The direction of motion of the shuriken.</param>
        public Shuriken(Vector2 StartPosition, Vector2 Direction) : base(new Polygon(POLY_POINTS, StartPosition))
        {
            this.velocity = Direction;
            this.velocity.Normalize();
            this.isLive = true;
            this.fadeTimer = 1f;
        }

        /// <summary>
        /// Updates the shuriken.
        /// </summary>
        public override void Update()
        {
            if (this.isLive)
            {
                // Check if the shuriken will collide with the wall.
                PolyWall wall = LevelManager.CurrentLevel.RetrieveValue<PolyWall>(GlobalConstants.TEST_WALL_TAG);
                IntersectionResult result = this.IsMotionIntersectingPolygon(this.velocity * SPEED, wall);
                if (result != null)
                {
                    this.position += this.velocity * result.Distance;
                    this.velocity = Vector2.Reflect(this.velocity, result.SurfaceNormal);
                    this.velocity *= BOUNCE_SPEED;
                    this.velocity.Y += BOUNCE_Y_VELOCITY_ADJUST;
                    this.isLive = false;
                }
                else
                {
                    this.position += this.velocity * SPEED;
                }
            }
            else
            {
                this.position += this.velocity;
                this.velocity.Y += GRAVITY;
                this.fadeTimer += FADE_SPEED;
                if (fadeTimer <= 0)
                {
                    LevelManager.CurrentLevel.DeregisterGameObject(this);
                }
            }

            // Rotate.
            this.angle += ROTATION_SPEED;
        }

        /// <summary>
        /// Renders the shuriken.
        /// </summary>
        public override void Render()
        {
            Renderer.DrawSprite("shuriken", this.position, SIZE, Angle: this.angle, Depth: 0f, ColorFilter: Color.White * this.fadeTimer);
        }
    }
}
