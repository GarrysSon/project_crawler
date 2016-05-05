using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProjectCrawler.Management;
using ProjectCrawler.Objects.Game.Enemy;
using ProjectCrawler.Objects.Game.Level.Component;
using ProjectCrawler.Objects.Generic.GameBase;
using ProjectCrawler.Objects.Generic.Utility;
using Microsoft.Xna.Framework.Graphics;
using ProjectCrawler.Objects.Game.Player;

namespace ProjectCrawler.Objects.Game.Enemy.Weapon
{
    public class Laser : GameObject
    {
        /// <summary>
        /// Size and polygon related constants.
        /// </summary>
        private const int POLY_SIDE_LENGTH = 8;
        private const int DRAW_SIDE_LENGTH = 32;
        private static readonly Vector2 DRAW_SIZE = new Vector2(DRAW_SIDE_LENGTH);
        private static readonly Vector2[] POLY_POINTS =
            new Vector2[]
            {
                new Vector2(-(POLY_SIDE_LENGTH / 2)),
                new Vector2(-(POLY_SIDE_LENGTH / 2), POLY_SIDE_LENGTH / 2),
                new Vector2(POLY_SIDE_LENGTH / 2, POLY_SIDE_LENGTH / 2),
                new Vector2(POLY_SIDE_LENGTH / 2, -(POLY_SIDE_LENGTH / 2))
            };

        private readonly Vector2 GLOW_OFFSET = new Vector2(0, 24);
        private readonly Vector2 GLOW_SIZE = new Vector2(128, 64);
        private readonly Color GLOW_COLOR = new Color(192, 0, 255);

        /// <summary>
        /// laser speed related constants
        /// </summary>
        private const float SPEED = 10f;

        /// <summary>
        /// Damage values.
        /// </summary>
        private const int BASE_DAMAGE = 1;

        /// <summary>
        /// Knockback amount.
        /// </summary>
        private const int KNOCKBACK = 6;

        /// <summary>
        /// Velocity of the laser.
        /// </summary>
        private Vector2 velocity;

        /// <summary>
        /// Constructor for the laser.
        /// </summary>
        /// <param name="StartPosition">The start position of the laser.</param>
        /// <param name="Direction">The direction of motion of the laser.</param>
        public Laser(Vector2 StartPosition, Vector2 Direction) : base(new Polygon(POLY_POINTS, StartPosition))
        {
            this.velocity = Direction;
            this.velocity.Normalize();
            this.velocity *= SPEED;
        }

        /// <summary>
        /// Updates the laser.
        /// </summary>
        public override void Update()
        {
            // Check if the laser will collide with the wall.
            PolyWall wall = LevelManager.CurrentLevel.RetrieveValue<PolyWall>(GlobalConstants.TEST_WALL_TAG);
            IntersectionResult wallResult = this.IsMotionIntersectingPolygon(this.velocity, wall);
            if (wallResult != null)
            {
                LevelManager.CurrentLevel.DeregisterGameObject(this);
                return;
            }
            else
            {
                // Check for collision with the player.
                PlayerNinja player = LevelManager.CurrentLevel.RetrieveValue<PlayerNinja>(GlobalConstants.PLAYER_TAG);
                IntersectionResult intersectResult = this.IsMotionIntersectingPolygon(this.velocity, player);
                if (intersectResult != null)
                {
                    player.ApplyDamage(BASE_DAMAGE, KNOCKBACK, this.velocity);
                    LevelManager.CurrentLevel.DeregisterGameObject(this);
                    return;
                }
                // Move if no collisions.
                this.position += this.velocity;
            }
        }

        /// <summary>
        /// Renders the laser.
        /// </summary>
        public override void Render()
        {
            Renderer.DrawSprite(
                GlobalConstants.LASER_IMAGE_TAG,
                this.position,
                DRAW_SIZE,
                Depth: 0f,
                DrawAdditive: true);
            Renderer.DrawSprite(
                GlobalConstants.GLOW_IMAGE_TAG,
                this.position + GLOW_OFFSET,
                GLOW_SIZE,
                Depth: 0f,
                DrawAdditive: true,
                ColorFilter: GLOW_COLOR * 0.5f);
        }
    }
}
