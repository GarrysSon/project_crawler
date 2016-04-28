using System;
using Microsoft.Xna.Framework;
using ProjectCrawler.Management;
using ProjectCrawler.Objects.Generic.GameBase;
using ProjectCrawler.Objects.Generic.Utility;

namespace ProjectCrawler.Objects.Game.Level.Component
{
    /// <summary>
    /// Represents a polygonal wall, used for testing.
    /// </summary>
    public class PolyWall : GameObject
    {
        /// <summary>
        /// The tag of an image to draw to represent the PolyWall.
        /// </summary>
        protected string imageTag;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="WallShape">Polygon to use for the wall shape.</param>
        public PolyWall(Polygon WallShape, string ImageTag = null) : base(WallShape)
        {
            this.imageTag = ImageTag;
        }

        /// <summary>
        /// Updates the PolyWall.
        /// </summary>
        public override void Update()
        {
        }

        /// <summary>
        /// Renders the PolyWall as a series of lines following the underlying polygon.
        /// </summary>
        public override void Render()
        {
            // If no image tag is specified, draw the wall as lines.
            // Draw the specified image otherwise.
            if (this.imageTag == null)
            {
                for (int i = 0; i < this.points.Length; i++)
                {
                    Vector2 A = this.points[i] + this.position;
                    Vector2 B = this.points[(i + 1) % this.points.Length] + this.position;
                    Vector2 mid = (A + B) / 2f;
                    float length = (B - A).Length();
                    Renderer.DrawSprite(GlobalConstants.BLANK_IMAGE_TAG, mid, new Vector2(length, 4), this.Angle(B - A), Color.White, 1f);
                }
            }
            else
            {
                Vector2 imageSize = Renderer.GetImageSize(this.imageTag);
                Renderer.DrawSprite(this.imageTag, this.position, imageSize, Depth: 1f);
            }
        }

        /// <summary>
        /// Returns the angle between the given vector and the X-axis.
        /// </summary>
        /// <param name="V">A vector.</param>
        /// <returns></returns>
        private float Angle(Vector2 V)
        {
            return (float)Math.Atan2(V.Y, V.X);
        }
    }
}
