using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel;

namespace ProjectCrawler
{
    /// <summary>
    /// Class representing a player object.
    /// </summary>
    public class Player : GameObject
    {
        /// <summary>
        /// The max health alotted to the player character.
        /// </summary>
        private const int MAX_HEALTH = 100;

        /// <summary>
        /// The speed of the character.
        /// </summary>
        private const int PLAYER_SPEED = 3;

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
        public Player(Vector2 StartPosition) : base(Polygon.CreateRectangle(WIDTH, HEIGHT, StartPosition))
        {
            // Default animation values.
            this.animFrameNumber = 0;
            this.animFrameTimer = 0;
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
                if (++animFrameTimer == FRAME_DURATIONS[animFrameNumber])
                {
                    animFrameTimer = 0;
                    animFrameNumber = (animFrameNumber + 1) % FRAME_DURATIONS.Length;
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
        }

        /// <summary>
        /// The render method for the Player.
        /// </summary>
        public override void Render()
        {
            // This is the renderer you know...no more jokes.
            // Draw the ninja!
            Renderer.DrawSprite(
                "ninja", 
                this.position + FRAME_POS_OFFSETS[animFrameNumber], 
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
    }
}
