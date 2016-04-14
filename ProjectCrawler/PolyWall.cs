﻿using System;
using Microsoft.Xna.Framework;

namespace ProjectCrawler
{
    /// <summary>
    /// Represents a polygonal wall, used for testing.
    /// </summary>
    public class PolyWall : GameObject
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="WallShape">Polygon to use for the wall shape.</param>
        public PolyWall(Polygon WallShape) : base(WallShape)
        {
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
            for (int i = 0; i < this.points.Length; i++)
            {
                Vector2 A = this.points[i] + this.position;
                Vector2 B = this.points[(i + 1) % this.points.Length] + this.position;
                Vector2 mid = (A + B) / 2f;
                float length = (B - A).Length();
                Renderer.DrawSprite("blank", mid, new Vector2(length, 4), this.Angle(B - A), Color.White, 1f);
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