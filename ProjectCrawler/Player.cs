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
        }

        /// <summary>
        /// The render method for the Player.
        /// </summary>
        public void Render()
        {
            // This is the renderer you know...no more jokes.
        }
    }
}
