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
        private readonly float[] FRAME_ANGLE_OFFSETS = { 0.0f, 0.08f, 0.0f, -0.08f };
        private readonly Vector2[] FRAME_POS_OFFSETS = { new Vector2(0), new Vector2(5, -10), new Vector2(0), new Vector2(-5, -10) };

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
        /// The Player's current position.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The Player's bounding box.
        /// </summary>
        private Rectangle boundingBox;

        /// <summary>
        /// The default constructor for the Player object.
        /// </summary>
        public Player()
        {
            this.position = new Vector2();
            this.boundingBox = new Rectangle();
            this.animFrameNumber = 0;
            this.animFrameTimer = 0;
        }

        /// <summary>
        /// The getter for the position of the Player.
        /// </summary>
        public Vector2 Position
        {
            get { return this.position; }
        }

        /// <summary>
        /// The getter for the bounding box of the Player.
        /// </summary>
        public Rectangle BoundingBox
        {
            get { return this.boundingBox; }
        }

        /// <summary>
        /// The update method for the Player.
        /// </summary>
        public void Update()
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

            // Move up if the W key is pressed.
            if(currentState.IsKeyDown(Keys.W))
            {
                this.position.Y += -PLAYER_SPEED;
                this.animate = true;
            }

            // Move left if the A key is pressed.
            if(currentState.IsKeyDown(Keys.A))
            {
                this.position.X += -PLAYER_SPEED;
                this.animate = true;
            }

            // Move down if the S key is pressed.
            if(currentState.IsKeyDown(Keys.S))
            {
                this.position.Y += PLAYER_SPEED;
                this.animate = true;
            }

            // Move right if the D key is pressed.
            if(currentState.IsKeyDown(Keys.D))
            {
                this.position.X += PLAYER_SPEED;
                this.animate = true;
            }

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
        }

        /// <summary>
        /// The render method for the Player.
        /// </summary>
        public void Render()
        {
            // This is the renderer you know...no more jokes.
            // Draw the ninja!
            Renderer.DrawSprite("ninja", this.position + FRAME_POS_OFFSETS[animFrameNumber], new Vector2(64, 64), FRAME_ANGLE_OFFSETS[animFrameNumber]);
        }
    }
}
