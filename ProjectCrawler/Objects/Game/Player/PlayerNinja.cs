using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using ProjectCrawler.Management;
using ProjectCrawler.Objects.Generic.GameBase;
using ProjectCrawler.Objects.Generic.Utility;
using ProjectCrawler.Objects.Game.Level.Component;
using ProjectCrawler.Objects.Game.Player.Weapon;
using System.Collections.Generic;
using ProjectCrawler.Objects.Game.Enemy;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCrawler.Objects.Game.Player
{
    /// <summary>
    /// Class representing a player object.
    /// </summary>
    public class PlayerNinja : GameObject
    {
        /// <summary>
        /// The max health alotted to the player character.
        /// </summary>
        private const int MAX_HEALTH = 5;

        /// <summary>
        /// The speed of the character.
        /// </summary>
        private const int PLAYER_SPEED = 4;

        /// <summary>
        /// The invincibility frame time for the player.
        /// </summary>
        private const int INVINCIBLE_TIME = 30;

        /// <summary>
        /// Variables used for animations.
        /// </summary>
        private readonly int[] FRAME_DURATIONS = { 5, 5, 5, 5 };
        private readonly float[] FRAME_ANGLE_OFFSETS = { 0.0f, 0.05f, 0.0f, -0.05f };
        private readonly Vector2[] FRAME_POS_OFFSETS = { new Vector2(0), new Vector2(3, -6), new Vector2(0), new Vector2(-3, -6) };

        /// <summary>
        /// Variables used for sizing and shadow drawing
        /// </summary>
        private const int WIDTH = 48;
        private const int HEIGHT = 48;
        private readonly Vector2 SIZE = new Vector2(WIDTH, HEIGHT);
        private readonly Vector2 SHADOW_OFFSET = new Vector2(0, HEIGHT / 2);
        private readonly Vector2 SHADOW_SIZE = new Vector2(60, 30);

        /// <summary>
        /// Variables used for sizing the health hearts.
        /// </summary>
        private const int HEART_WIDTH = 20;
        private const int HEART_HEIGHT = 23;
        private readonly Vector2 HEART_SIZE = new Vector2(HEART_WIDTH, HEART_HEIGHT);

        /// <summary>
        /// Weapon constants.
        /// </summary>
        private const int SHURIKEN_RECHARGE_TIME = 12;

        /// <summary>
        /// The frame number for the animation.
        /// </summary>
        private int animFrameNumber;

        /// <summary>
        /// The frame timer for the animation.
        /// </summary>
        private int animFrameTimer;

        /// <summary>
        /// The boolean to tell if the player should be animated.
        /// </summary>
        private bool animate = false;

        /// <summary>
        /// Frames until another shuriken can be fired.
        /// </summary>
        private int fireTimer;

        /// <summary>
        /// The damage state information.
        /// </summary>
        private int invincibleTimer;
        private Vector2 damageImpulse;

        /// <summary>
        /// The Player's health.
        /// </summary>
        [DefaultValue(MAX_HEALTH)]
        public int Health { get; set; }

        /// <summary>
        /// The Player's level.
        /// </summary>
        [DefaultValue(1)]
        public int Level { get; set; }

        /// <summary>
        /// The default constructor for the Player object.
        /// </summary>
        public PlayerNinja(Vector2 StartPosition) : base(Polygon.CreateRectangle(WIDTH, HEIGHT, StartPosition))
        {
            this.Health = MAX_HEALTH;

            // Default animation values.
            this.animFrameNumber = 0;
            this.animFrameTimer = 0;
            this.fireTimer = 0;
        }

        /// <summary>
        /// Applies damage to the player from a given direction.
        /// </summary>
        /// <param name="damage">The damage applied to the ninja.</param>
        /// <param name="knockBack">The knockback applied to the ninja.</param>
        /// <param name="from">The direction in which the damage is coming.</param>
        public void ApplyDamage(int damage, int knockBack, Vector2 from)
        {
            // Damage only if not invincible.
            if (this.invincibleTimer == 0)
            {
                this.Health -= damage;
                this.invincibleTimer = INVINCIBLE_TIME;
                from.Normalize();
                this.damageImpulse = from * knockBack;
            }

            if (this.Health <= 0)
            {
                // Deregister the enemy if it has died.
                LevelManager.CurrentLevel.DeregisterGameObject(this);
                // Create an exploded enemy.
                Texture2D tex = Renderer.GetImage(GlobalConstants.NINJA_IMAGE_TAG);
                BreakableObject breakable = new BreakableObject(this.position, tex, 120, 6, 10, new Vector2(24, 64), this.position.Y + HEIGHT / 2, SIZE);
                LevelManager.CurrentLevel.RegisterGameObject(breakable);
            }
        }

        /// <summary>
        /// The update method for the Player.
        /// </summary>
        public override void Update()
        {
            // This is where all the fun happens for the player and the 
            // player related things...like when the player walks into a bar,
            // holds up two fingers and says, "I'll have five beers." HAHA!
            // That's a great joke. Here's another:
            /* 
             * 	
             *  A man flying in a hot air balloon suddenly realizes he’s lost. He reduces height 
             *  and spots a man down below. He lowers the balloon further and shouts to get 
             *  directions, 
             *  
             * "Excuse me, can you tell me where I am?"
             *  
             *  The man below says: "Yes. You're in a hot air balloon, hovering 30 feet above this field."
             *  
             *  "You must work in Information Technology," says the balloonist.
             *  
             *  "I do" replies the man. "How did you know?"
             *  
             *  "Well," says the balloonist, "everything you have told me is technically correct, 
             *  but It's of no use to anyone."
             *  
             *  The man below replies, "You must work in management."
             *  
             *  "I do," replies the balloonist, "But how'd you know?"
             *  
             *  "Well", says the man, "you don’t know where you are or where you’re going, but you 
             *  expect me to be able to help. You’re in the same position you were before we met, 
             *  but now it’s my fault."
             */

            // Grabbing the current state of the keyboard.
            KeyboardState currentState = Keyboard.GetState();

            // Reset the animation boolean.
            this.animate = false;

            // Motion vector
            Vector2 motion = Vector2.Zero;

            // Move up if the W key is pressed.
            if(currentState.IsKeyDown(Keys.W))
            {
                this.animate = true;
                motion.Y = -1f;
            }

            // Move left if the A key is pressed.
            if(currentState.IsKeyDown(Keys.A))
            {
                this.animate = true;
                motion.X = -1f;
            }

            // Move down if the S key is pressed.
            if(currentState.IsKeyDown(Keys.S))
            {
                this.animate = true;
                motion.Y = 1f;
            }

            // Move right if the D key is pressed.
            if(currentState.IsKeyDown(Keys.D))
            {
                this.animate = true;
                motion.X = 1f;
            }

            // Normalize the motion vector if the Player moved.
            if (this.animate)
            {
                motion.Normalize();
            }
            // this.position += motion * PLAYER_SPEED;

            // Checking if we should animate the movement.
            if (this.animate)
            {
                // Update the animation
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

            // See if the Player's motion will intersect the wall.
            PolyWall wall = LevelManager.CurrentLevel.RetrieveValue<PolyWall>(GlobalConstants.TEST_WALL_TAG);
            IntersectionResult result = this.IsMotionIntersectingPolygon(motion * PLAYER_SPEED, wall);
            if (result != null)
            {
                // Find the amount the Player should slide relative to the wall.
                Vector2 slide = (PLAYER_SPEED - result.Distance) * result.Surface * Vector2.Dot(motion, result.Surface);
                // Set the motion to move to the wall then slide.
                motion = motion * (result.Distance - 2f) + slide;
                // Move the player object.
                this.position += motion;
            }
            else
            {
                this.position += motion * PLAYER_SPEED;
            }

            // Shuriken can be fired if the fire timer equals 0.
            if (fireTimer == 0)
            {
                // Fire left if left is pressed
                if (currentState.IsKeyDown(Keys.Left))
                {
                    fireTimer = SHURIKEN_RECHARGE_TIME;
                    LevelManager.CurrentLevel.RegisterGameObject(
                        new Shuriken(this.position, new Vector2(-1, 0)));
                }
                else if (currentState.IsKeyDown(Keys.Up))
                {
                    fireTimer = SHURIKEN_RECHARGE_TIME;
                    LevelManager.CurrentLevel.RegisterGameObject(
                        new Shuriken(this.position, new Vector2(0, -1)));
                }
                else if (currentState.IsKeyDown(Keys.Right))
                {
                    fireTimer = SHURIKEN_RECHARGE_TIME;
                    LevelManager.CurrentLevel.RegisterGameObject(
                        new Shuriken(this.position, new Vector2(1, 0)));
                }
                else if (currentState.IsKeyDown(Keys.Down))
                {
                    fireTimer = SHURIKEN_RECHARGE_TIME;
                    LevelManager.CurrentLevel.RegisterGameObject(
                        new Shuriken(this.position, new Vector2(0, 1)));
                }
            }
            else
            {
                fireTimer--;
            }

            // Decrement the invincibility timer if needed.
            if(invincibleTimer > 0)
            {
                invincibleTimer--;
            }

            // Check for collisions with enemies.
            List<AbstractEnemy> enemies = LevelManager.CurrentLevel.GetObjectsOfType<AbstractEnemy>();
            foreach (AbstractEnemy e in enemies)
            {
                // Check to see if the player is touching an enemy.
                bool enemyResult = this.IsIntersectingPolygon(e);
                if(enemyResult)
                {
                    this.ApplyDamage(e.ContactDamage, e.KnockBack, this.position - e.Position);
                    return;
                }

            }
        }

        /// <summary>
        /// The render method for the Player.
        /// </summary>
        public override void Render()
        {
            // This is the renderer you know...no more jokes.
            // Draw the ninja!
            Renderer.DrawSprite(
                GlobalConstants.NINJA_IMAGE_TAG, 
                this.position + GlobalConstants.BOUNCE_FRAME_POS_OFFSETS[animFrameNumber], 
                SIZE, 
                GlobalConstants.BOUNCE_FRAME_ANGLE_OFFSETS[animFrameNumber], 
                Depth: Renderer.GenerateDepthFromScreenPosition(position));

            // Draw the dropshadow.
            Renderer.DrawSprite(
                GlobalConstants.DROP_SHADOW_IMAGE_TAG, 
                position + SHADOW_OFFSET, 
                SHADOW_SIZE, 
                ColorFilter: Color.White * 0.6f, 
                Depth: GlobalConstants.SHADOW_DEPTH);

            // The x and y offsets for the hearts.
            float xOffset = HEART_SIZE.X;
            float yOffset = HEART_SIZE.Y;
            float maxXOffset = MAX_HEALTH * xOffset;

            // Draw the hearts reversed so they fall off in the correct order.
            for (int i = 0; i < Health; i++ )
            {
                Renderer.DrawSprite(
                    GlobalConstants.FART_HEART_IMAGE_TAG,
                    new Vector2(
                        LevelManager.CurrentLevel.ScrollPoint.X + (float)GlobalConstants.WINDOW_WIDTH / 2 - (maxXOffset - i * xOffset),
                        LevelManager.CurrentLevel.ScrollPoint.Y - (float)GlobalConstants.WINDOW_HEIGHT / 2 + yOffset),
                    HEART_SIZE);
            }
        }
    }
}
