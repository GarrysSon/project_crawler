using System;
using Microsoft.Xna.Framework;
using ProjectCrawler.Management;
using ProjectCrawler.Objects.Generic.Utility;
using ProjectCrawler.Objects.Game.Level.Component;
using Microsoft.Xna.Framework.Graphics;
using ProjectCrawler.Objects.Generic.GameBase;
using ProjectCrawler.Objects.Game.Player;
using ProjectCrawler.Objects.Game.Enemy.Weapon;

namespace ProjectCrawler.Objects.Game.Enemy
{
    public class LazerEnemy : AbstractEnemy
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
        private const float SPEED = 2f;
        private readonly Vector2[] PATROL_MOVEMENTS = {
            new Vector2(SPEED, 0),
            new Vector2(-SPEED, 0),
            new Vector2(0, SPEED),
            new Vector2(0, -SPEED),
            Vector2.Zero
        };
        private const int PATROL_PERIOD = 60;
        private const int SIGHT_DISTANCE_SQUARED = 250 * 250;

        /// <summary>
        /// Laser shit.
        /// </summary>
        private const int LASER_RECHARGE_PERIOD = 60;

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
        /// Patrol and chase information.
        /// </summary>
        private int patrolTimer;
        private bool isInPursuit;

        /// <summary>
        /// Velocity of the LazerEnemy.
        /// </summary>
        private Vector2 velocity;

        /// <summary>
        /// Laser state shit.
        /// </summary>
        private int laserTimer;

        public LazerEnemy(Vector2 StartPosition) : base(Polygon.CreateRectangle(WIDTH, HEIGHT, StartPosition))
        {
            this.health = MAX_HEALTH;
            this.animFrameNumber = 0;
            this.animFrameTimer = 0;
            this.patrolTimer = 0;
            this.isInPursuit = false;
            this.velocity = Vector2.Zero;
        }

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
                Texture2D tex = Renderer.GetImage("lazerEnemy");
                BreakableObject breakable = new BreakableObject(this.position, tex, 30, 6, 10, new Vector2(24, 64), this.position.Y + HEIGHT / 2, SIZE);
                LevelManager.CurrentLevel.RegisterGameObject(breakable);
            }
        }

        public override void Render()
        {
            Renderer.DrawSprite(
                "lazerEnemy",
                position + GlobalConstants.BOUNCE_FRAME_POS_OFFSETS[animFrameNumber],
                SIZE,
                GlobalConstants.BOUNCE_FRAME_ANGLE_OFFSETS[animFrameNumber],
                Depth: Renderer.GenerateDepthFromScreenPosition(position),
                ColorFilter: Color.White * ((this.invincibleTimer / 3) % 2 == 0 ? 1f : 0f));
            Renderer.DrawSprite(
                "dropShadow",
                position + SHADOW_OFFSET,
                SHADOW_SIZE,
                ColorFilter: Color.White * 0.6f,
                Depth: GlobalConstants.SHADOW_DEPTH);
        }

        public override void Update()
        {
            // Have the enemy patrol when not in range of the player
            PlayerNinja player = LevelManager.CurrentLevel.RetrieveValue<PlayerNinja>(GlobalConstants.PLAYER_TAG);
            Vector2 diffVector = player.Position - this.position;
            if (diffVector.LengthSquared() < SIGHT_DISTANCE_SQUARED)
            {
                isInPursuit = true;
                diffVector.Normalize();
                this.velocity = Vector2.Lerp(this.velocity, diffVector * SPEED, 0.1f);

                // Check if a laser should be fired.
                if (this.laserTimer > 0)
                {
                    this.laserTimer--;
                }
                else
                {
                    this.laserTimer = LASER_RECHARGE_PERIOD;
                    LevelManager.CurrentLevel.RegisterGameObject(new Laser(this.Position, diffVector));
                }
            }
            else if (isInPursuit)
            {
                isInPursuit = false;
                this.patrolTimer = PATROL_PERIOD;
                this.velocity = PATROL_MOVEMENTS[4];
            }

            if (!isInPursuit)
            {
                if (this.patrolTimer > 0)
                {
                    this.patrolTimer--;
                }
                else
                {
                    Random rand = new Random();
                    this.patrolTimer = PATROL_PERIOD;
                    this.velocity = PATROL_MOVEMENTS[rand.Next(PATROL_MOVEMENTS.Length)];
                }
            }

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
            if (this.velocity.LengthSquared() > 0)
            {
                if (++animFrameTimer == GlobalConstants.BOUNCE_FRAME_DURATIONS[animFrameNumber])
                {
                    animFrameTimer = 0;
                    animFrameNumber = (animFrameNumber + 1) % GlobalConstants.BOUNCE_FRAME_DURATIONS.Length;
                }
            }
            else
            {
                animFrameNumber = 0;
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

        public override int KnockBack
        {
            get { return 6; }
        }

        public override int ContactDamage
        {
            get { return 11; }
        }
    }
}
