using Microsoft.Xna.Framework;

namespace ProjectCrawler
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
        /// <param name="SurfaceNormal">Surface normal of intersected surface.</param>
        /// <param name="Distance">Penetration distance past intersected surface.</param>
        public IntersectionResult(Vector2 SurfaceNormal, float Distance)
        {
            this.surfaceNormal = SurfaceNormal;
            this.distance = Distance;
        }
    }
}
