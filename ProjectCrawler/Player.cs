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
            throw new NotImplementedException();
        }

        /// <summary>
        /// The render method for the Player.
        /// </summary>
        public void Render()
        {
            throw new NotImplementedException();
        }
    }
}
