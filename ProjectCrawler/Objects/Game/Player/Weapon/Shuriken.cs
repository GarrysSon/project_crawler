﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProjectCrawler.Management;
using ProjectCrawler.Objects.Game.Enemy;
using ProjectCrawler.Objects.Game.Level.Component;
using ProjectCrawler.Objects.Generic.GameBase;
using ProjectCrawler.Objects.Generic.Utility;
using Microsoft.Xna.Framework.Graphics;
using ProjectCrawler.Objects.Game.Effect;

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

        private readonly Vector2 SHADOW_OFFSET = new Vector2(0, 24);
        private readonly Vector2 SHADOW_SIZE = new Vector2(32, 16);

        /// <summary>
        /// Shuriken speed related constants
        /// </summary>
        private const float SPEED = 10f;
        private const float ROTATION_SPEED = 0.4f;
        private const float BOUNCE_SPEED = 4f;
        private const float BOUNCE_Y_VELOCITY_ADJUST = -6f;
        private const float GRAVITY = 0.5f;

        /// <summary>
        /// Damage values.
        /// </summary>
        private const int BASE_DAMAGE = 1;

        /// <summary>
        /// Post-live fade speed;
        /// </summary>
        private const float FADE_SPEED = -0.04f;

        /// <summary>
        /// Contact flash properties.
        /// </summary>
        private readonly Color CONTACT_FLASH_COLOR = new Color(255, 200, 0);
        private readonly Vector2 CONTACT_FLASH_START_SIZE = new Vector2(64);
        private readonly Vector2 CONTACT_FLASH_END_SIZE = new Vector2(256);
        private const int CONTACT_FLASH_DURATION = 6;

        /// <summary>
        /// Velocity of the shuriken.
        /// </summary>
        private Vector2 velocity;
        /// <summary>
        /// Velocity adjustment for gravity.
        /// </summary>
        private float gravityVelocityAdjust;
        /// <summary>
        /// Position adjustment for gravity.
        /// </summary>
        private float gravityPositionAdjust;

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
                IntersectionResult wallResult = this.IsMotionIntersectingPolygon(this.velocity * SPEED, wall);
                if (wallResult != null)
                {
                    this.position += this.velocity * wallResult.Distance;
                    this.velocity = Vector2.Reflect(this.velocity, wallResult.SurfaceNormal);
                    this.velocity *= BOUNCE_SPEED;
                    this.gravityVelocityAdjust = BOUNCE_Y_VELOCITY_ADJUST;
                    //this.velocity.Y += BOUNCE_Y_VELOCITY_ADJUST;
                    this.isLive = false;
                    BasicFlash flash = new BasicFlash(
                        CONTACT_FLASH_COLOR,
                        this.position,
                        CONTACT_FLASH_START_SIZE,
                        CONTACT_FLASH_END_SIZE,
                        CONTACT_FLASH_DURATION);
                    LevelManager.CurrentLevel.RegisterGameObject(flash);
                }
                else
                {
                    // Check for collisions with enemies.
                    List<AbstractEnemy> enemies = LevelManager.CurrentLevel.GetObjectsOfType<AbstractEnemy>();
                    foreach (AbstractEnemy e in enemies)
                    {
                        IntersectionResult enemyResult = this.IsMotionIntersectingPolygon(this.velocity * SPEED, e);
                        if (enemyResult != null)
                        {
                            e.ApplyDamage(BASE_DAMAGE, this.velocity);
                            LevelManager.CurrentLevel.DeregisterGameObject(this);
                            Texture2D tex = Renderer.GetImage(GlobalConstants.SHURIKEN_IMAGE_TAG);
                            BreakableObject breakable = new BreakableObject(this.position, tex, 30, 6, 10, new Vector2(8, 48), this.position.Y + 24, SIZE);
                            LevelManager.CurrentLevel.RegisterGameObject(breakable);
                            BasicFlash flash = new BasicFlash(
                                CONTACT_FLASH_COLOR,
                                this.position,
                                CONTACT_FLASH_START_SIZE,
                                CONTACT_FLASH_END_SIZE,
                                CONTACT_FLASH_DURATION);
                            LevelManager.CurrentLevel.RegisterGameObject(flash);
                            return;
                        }
                    }
                    // Move if no collisions.
                    this.position += this.velocity * SPEED;
                }
            }
            else
            {
                this.gravityPositionAdjust += this.gravityVelocityAdjust;
                this.position += this.velocity;
                this.gravityVelocityAdjust += GRAVITY;
                //this.velocity.Y += GRAVITY;
                this.fadeTimer += FADE_SPEED;
                if (fadeTimer <= 0)
                {
                    LevelManager.CurrentLevel.DeregisterGameObject(this);
                }
            }

            // Rotate.
            this.angle += ROTATION_SPEED * (this.isLive ? 1 : -0.5f);
        }

        /// <summary>
        /// Renders the shuriken.
        /// </summary>
        public override void Render()
        {
            Renderer.DrawSprite(
                GlobalConstants.SHURIKEN_IMAGE_TAG, 
                this.position + new Vector2(0, this.gravityPositionAdjust), 
                SIZE, 
                Angle: this.angle, 
                Depth: 0f, 
                ColorFilter: Color.White * this.fadeTimer);
            Renderer.DrawSprite(
                GlobalConstants.DROP_SHADOW_IMAGE_TAG, 
                this.position + SHADOW_OFFSET, 
                SHADOW_SIZE * (2 - this.fadeTimer), 
                Depth: GlobalConstants.SHADOW_DEPTH, 
                ColorFilter: Color.White * 0.3f * this.fadeTimer);
        }
    }
}
