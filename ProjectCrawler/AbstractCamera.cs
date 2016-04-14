using Microsoft.Xna.Framework;

namespace ProjectCrawler
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
        public AbstractCamera(Vector2 StartPosition, GameObject FollowedObject) : base()
        {
            this.position = StartPosition;
            this.followedObject = FollowedObject;
        }
    }
}
