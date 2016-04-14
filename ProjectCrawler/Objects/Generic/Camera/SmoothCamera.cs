using System;
using Microsoft.Xna.Framework;
using ProjectCrawler.Objects.Generic.GameBase;

namespace ProjectCrawler.Objects.Generic.Camera
{
    /// <summary>
    /// A camera that smoothly follows a specified object.
    /// </summary>
    public class SmoothCamera : AbstractCamera
    {
        /// <summary>
        /// Controls the smoothness of the camera following. Should be a value
        /// between 0 and 1.
        /// </summary>
        protected float smoothness;
        public float Smoothness
        {
            get
            {
                return this.smoothness;
            }

            set
            {
                this.smoothness = 1f - value;
            }
        }

        /// <summary>
        /// Base constructor.
        /// </summary>
        public SmoothCamera() : base()
        {
        }

        /// <summary>
        /// Constructor placing the camera at a given position following an object.
        /// </summary>
        /// <param name="StartPosition">The camera's starting position.</param>
        /// <param name="FollowedObject">The object to follow.</param>
        /// <param name="Smoothing">The smoothing applied to the camera's movement. Should be a value between 0 and 1.</param>
        public SmoothCamera(Vector2 StartPosition, GameObject FollowedObject, float Smoothing) : base(StartPosition, FollowedObject)
        {
            this.Smoothness = Smoothing;
        }

        /// <summary>
        /// Updates the camera.
        /// </summary>
        public override void Update()
        {
            if (this.followedObject != null)
            {
                this.position = Vector2.Lerp(this.position, this.followedObject.Position, this.smoothness);
            }
        }

        /// <summary>
        /// Renders the camera. This doesn't need to be called.
        /// </summary>
        public override void Render()
        {
        }
    }
}
