using System;
using Microsoft.Xna.Framework;
using ProjectCrawler.Management;
using ProjectCrawler.Objects.Generic.GameBase;

namespace ProjectCrawler.Objects.Generic.Camera
{
    /// <summary>
    /// Abstract camera class.
    /// </summary>
    public abstract class AbstractCamera : GameObject
    {
        /// <summary>
        /// Object that the camera will follow.
        /// </summary>
        protected GameObject followedObject;
        public GameObject FollowedObject
        {
            get
            {
                return this.followedObject;
            }

            set
            {
                this.followedObject = value;
            }
        }

        /// <summary>
        /// Zoom level of the camera.
        /// </summary>
        protected float zoom;
        public float Zoom
        {
            get
            {
                return this.zoom;
            }

            set
            {
                this.zoom = value;
            }
        }

        /// <summary>
        /// The bounds of the camera.
        /// </summary>
        protected Rectangle bounds;
        public Rectangle Bounds
        {
            get
            {
                return this.bounds;
            }
        }

        /// <summary>
        /// Base constructor.
        /// </summary>
        public AbstractCamera() : base()
        {
        }

        /// <summary>
        /// Constructor placing the camera at a given position following an object.
        /// </summary>
        /// <param name="StartPosition">The camera's starting position.</param>
        /// <param name="FollowedObject">The object to follow.</param>
        public AbstractCamera(Vector2 StartPosition, GameObject FollowedObject, Rectangle CameraBounds) : base()
        {
            this.position = StartPosition;
            this.followedObject = FollowedObject;
            this.bounds = CameraBounds;
        }

        /// <summary>
        /// Updates the camera.
        /// </summary>
        public override void Update()
        {
            // Keep the camera in bounds.
            int halfWidth = GlobalConstants.WINDOW_WIDTH / 2;
            int halfHeight = GlobalConstants.WINDOW_HEIGHT / 2;
            if ((this.position.X - halfWidth) < this.bounds.Left)
            {
                this.position.X = this.bounds.Left + halfWidth;
            }
            else if ((this.position.X + halfWidth) > this.bounds.Right)
            {
                this.position.X = this.bounds.Right - halfWidth;
            }
            if ((this.position.Y - halfHeight) < this.bounds.Top)
            {
                this.position.Y = this.bounds.Top + halfHeight;
            }
            else if ((this.position.Y + halfHeight) > this.bounds.Bottom)
            {
                this.position.Y = this.bounds.Bottom - halfHeight;
            }
        }
    }
}
