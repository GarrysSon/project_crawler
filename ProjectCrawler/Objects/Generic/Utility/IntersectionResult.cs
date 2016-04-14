using Microsoft.Xna.Framework;

namespace ProjectCrawler.Objects.Generic.Utility
{
    /// <summary>
    /// Represents teh result of an intersection between two vectors.
    /// </summary>
    public class IntersectionResult
    {
        /// <summary>
        /// Surface normal of the intersected surface.
        /// </summary>
        private Vector2 surfaceNormal;
        public Vector2 SurfaceNormal
        {
            get
            {
                return surfaceNormal;
            }
        }

        private Vector2 surface;
        public Vector2 Surface
        {
            get
            {
                return surface;
            }
        }

        /// <summary>
        /// Penetration depth past the intersected surface.
        /// </summary>
        private float distance;
        public float Distance
        {
            get
            {
                return distance;
            }
        }

        /// <summary>
        /// Constructor/
        /// </summary>
        /// <param name="Surface">Surface vector that was intersected</param>
        /// <param name="Distance">Penetration distance past intersected surface.</param>
        public IntersectionResult(Vector2 Surface, float Distance)
        {
            this.surface = Surface;
            this.surface.Normalize();
            this.surfaceNormal = new Vector2(this.surface.Y * -1, this.surface.X);
            this.distance = Distance;
        }
    }
}
