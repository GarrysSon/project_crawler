﻿using System;
using Microsoft.Xna.Framework;
using ProjectCrawler.Management;
using ProjectCrawler.Objects.Generic.Utility;
using ProjectCrawler.Objects.Game.Level.Component;
using Microsoft.Xna.Framework.Graphics;
using ProjectCrawler.Objects.Generic.GameBase;
using ProjectCrawler.Objects.Game.Effect;

namespace ProjectCrawler.Objects.Game.Enemy
{
    public class FunnyEnemy : AbstractEnemy
    {
        /// <summary>
        /// Health and damage related constants.
        /// </summary>
        private const int MAX_HEALTH = 3;
        private const int INVINCIBLE_TIME = 18;
        private const float KNOCKBACK_FORCE = 6;

        /// <summary>
        /// Movement constants.
        /// </summary>
        //private readonly int[] PATH_DURATIONS = { 300, 140, 300, 140 };
        //private readonly Vector2[] PATH_MOTION = { new Vector2(2, 0), new Vector2(0, 2), new Vector2(-2, 0), new Vector2(0, -2) };
        private const float SPEED = 2f;

        /// <summary>
        /// Size and shadow positioning related constants.
        /// </summary>
        private const int WIDTH = 48;
        private const int HEIGHT = 48;
        private readonly Vector2 SIZE = new Vector2(WIDTH, HEIGHT);
        private readonly Vector2 SHADOW_OFFSET = new Vector2(0, HEIGHT / 2);
        private readonly Vector2 SHADOW_SIZE = new Vector2(60, 30);

        /// <summary>
        /// Path state information.
        /// </summary>
        //private int pathFrameNumber;
        //private int pathFrameTimer;

        /// <summary>
        /// Animation state information.
        /// </summary>
        private int animFrameNumber;
        private int animFrameTimer;

        /// <summary>
        /// Damage state information.
        /// </summary>
        private Vector2 damageImpulse;
        private int invincibleTimer;

        /// <summary>
        /// Velocity of the FunnyEnemy.
        /// </summary>
        private Vector2 velocity;

        /// <summary>
        /// Constructor for the FunnyEnemy.
        /// </summary>
        /// <param name="StartPosition">Start position of the enemy.</param>
        public FunnyEnemy(Vector2 StartPosition, Vector2 StartDirection) : base(Polygon.CreateRectangle(WIDTH, HEIGHT, StartPosition))
        {
            this.health = MAX_HEALTH;
            //pathFrameNumber = 0;
            //pathFrameTimer = 0;
            this.animFrameNumber = 0;
            this.animFrameTimer = 0;
            StartDirection.Normalize();
            this.velocity = StartDirection * SPEED;

        }

        /// <summary>
        /// Renders the FunnyEnemy.
        /// </summary>
        public override void Render()
        {
            Renderer.DrawSprite(
                GlobalConstants.FUNNY_ENEMY_IMAGE_TAG, 
                position + GlobalConstants.BOUNCE_FRAME_POS_OFFSETS[animFrameNumber], 
                SIZE, 
                GlobalConstants.BOUNCE_FRAME_ANGLE_OFFSETS[animFrameNumber], 
                Depth: Renderer.GenerateDepthFromScreenPosition(position),
                ColorFilter: Color.White * ((this.invincibleTimer / 3) % 2 == 0 ? 1f : 0f));
            Renderer.DrawSprite(
                GlobalConstants.DROP_SHADOW_IMAGE_TAG, 
                position + SHADOW_OFFSET, 
                SHADOW_SIZE, 
                ColorFilter: Color.White * 0.6f, 
                Depth: GlobalConstants.SHADOW_DEPTH);
        }

        /// <summary>
        /// Updates the FunnyEnemy.
        /// </summary>
        public override void Update()
        {
            // Update the path
            /*if (++pathFrameTimer == PATH_DURATIONS[pathFrameNumber])
            {
                pathFrameTimer = 0;
                pathFrameNumber = (pathFrameNumber + 1) % PATH_DURATIONS.Length;
            }

            // Move along the path
            position += PATH_MOTION[pathFrameNumber];*/

            // Check for intersections with the wall.
            Vector2 allMotion = this.velocity + (this.invincibleTimer > 0 ? this.damageImpulse : Vector2.Zero);
            PolyWall wall = LevelManager.CurrentLevel.RetrieveValue<PolyWall>(GlobalConstants.TEST_WALL_TAG);
            IntersectionResult result = this.IsMotionIntersectingPolygon(allMotion, wall);
            if (result != null)
            {
                Vector2 reflection = Vector2.Reflect(allMotion, result.SurfaceNormal);
                float preContactLength = result.Distance / allMotion.Length();
                this.position += allMotion * (preContactLength - 0.01f) + reflection * (1 - preContactLength);
                this.velocity = reflection;
                this.damageImpulse = Vector2.Zero;
            }
            else
            {
                this.position += allMotion;
            }

            // Update the animation
            if (++animFrameTimer == GlobalConstants.BOUNCE_FRAME_DURATIONS[animFrameNumber])
            {
                animFrameTimer = 0;
                animFrameNumber = (animFrameNumber + 1) % GlobalConstants.BOUNCE_FRAME_DURATIONS.Length;
            }

            // Update invincibility
            if (this.invincibleTimer > 0)
            {
                this.damageImpulse = Vector2.Lerp(Vector2.Zero, this.damageImpulse, (float)this.invincibleTimer / INVINCIBLE_TIME);
                this.invincibleTimer--;

                // Normalize the speed if the invincible timer runs out
                if (this.invincibleTimer == 0)
                {
                    if (this.velocity.LengthSquared() > 0)
                    {
                        this.velocity.Normalize();
                        this.velocity *= SPEED;
                    }
                }
            }
        }

        /// <summary>
        /// Applies damage to the FunnyEnemy from a given direction vector.
        /// </summary>
        /// <param name="Damage"></param>
        /// <param name="From"></param>
        public override void ApplyDamage(int Damage, Vector2 From)
        {
            // Damage only if not invincible.
            if (this.invincibleTimer == 0)
            {
                this.health -= Damage;
                this.invincibleTimer = INVINCIBLE_TIME;
                From.Normalize();
                this.damageImpulse = From * KNOCKBACK_FORCE;
            }

            if (this.health <= 0)
            {
                // Deregister the enemy if it has died.
                LevelManager.CurrentLevel.DeregisterGameObject(this);
                // Create an exploded enemy.
                Texture2D tex = Renderer.GetImage(GlobalConstants.FUNNY_ENEMY_IMAGE_TAG);
                BreakableObject breakable = new BreakableObject(this.position, tex, 30, 6, 10, new Vector2(24, 64), this.position.Y + HEIGHT / 2, SIZE);
                LevelManager.CurrentLevel.RegisterGameObject(breakable);
                // Generate some swank ass explosion.
                GenerateExplosion();
            }
        }

        private void GenerateExplosion()
        {
            Random rand = new Random();
            for (int i = 0; i < 15; i++)
            {
                Vector2 pos = new Vector2(rand.Next(64) - 32, rand.Next(64) - 32);
                BasicFlash flash = new BasicFlash(
                    new Color(rand.Next(255), rand.Next(255), rand.Next(255)),
                    position + pos,
                    new Vector2(32 + rand.Next(128)),
                    new Vector2(8 + rand.Next(24)),
                    rand.Next(40) + 5);
                LevelManager.CurrentLevel.RegisterGameObject(flash);
            }
        }

        public override int KnockBack
        {
            get { return 6; }
        }

        public override int ContactDamage
        {
            get { return 1; }
        }
    }
}
