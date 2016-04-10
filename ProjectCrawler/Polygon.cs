﻿using Microsoft.Xna.Framework;

namespace ProjectCrawler
{
    public class Polygon
    {
        /// <summary>
        /// The points making up the polygon.
        /// </summary>
        protected Vector2[] points;
        public Vector2[] Points
        {
            get
            {
                return points;
            }
        }

        /// <summary>
        /// Position of the polygon in 2D space.
        /// </summary>
        protected Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public Polygon()
        {
            this.points = new Vector2[0];
            this.position = Vector2.Zero;
        }

        /// <summary>
        /// Constructor building a polygon from an array of points.
        /// </summary>
        /// <param name="Points"></param>
        public Polygon(Vector2[] Points, Vector2 Position)
        {
            this.points = Points;
            this.position = Position;
        }

        /// <summary>
        /// Checks to see if there is intersection between this and another polygon.
        /// </summary>
        /// <param name="P">Another polygon to check for an intersection with.</param>
        /// <param name="PositionDiff">The difference in position between this and the other polygon.</param>
        /// <returns></returns>
        public bool IsIntersectingPolygon(Polygon P)
        {
            Vector2[] aPoints = this.GetPositionAdjustedPoints();
            for (int i = 0; i < aPoints.Length; i++)
            {
                Vector2 A = aPoints[i];
                Vector2 B = aPoints[(i + 1) % aPoints.Length];
                Vector2 b = B - A;
                Vector2 bPerp = new Vector2(b.Y * -1, b.X);

                Vector2[] bPoints = P.GetPositionAdjustedPoints();
                for (int j = 0; j < bPoints.Length; j++)
                {
                    Vector2 C = bPoints[j];
                    Vector2 D = bPoints[(j + 1) % bPoints.Length];
                    Vector2 d = D - C;
                    Vector2 c = C - A;
                    Vector2 dPerp = new Vector2(d.Y * -1, d.X);

                    float t = Vector2.Dot(dPerp, c) / Vector2.Dot(dPerp, b);
                    float u = Vector2.Dot(bPerp, c) / Vector2.Dot(dPerp, b);

                    if (u >= 0 && u <= 1 && t >= 0 && t <= 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the points composing the polygons adjusted for the polygon position.
        /// </summary>
        /// <returns>Adjusted polygon vertices.</returns>
        public Vector2[] GetPositionAdjustedPoints()
        {
            Vector2[] adjustedPoints = new Vector2[this.points.Length];
            for (int i = 0; i < this.points.Length; i++)
            {
                adjustedPoints[i] = this.points[i] + this.position;
            }
            return adjustedPoints;
        }

        /// <summary>
        /// Creates a polygon with a rectangle shape.
        /// </summary>
        /// <param name="Width">Width of the rectangle.</param>
        /// <param name="Height">Height of the rectangle.</param>
        /// <param name="Position">Position of the polygon.</param>
        /// <returns></returns>
        public static Polygon CreateRectangle(float Width, float Height, Vector2 Position)
        {
            float halfWidth = Width / 2f;
            float halfHeight = Height / 2f;
            return new Polygon(
                new Vector2[] 
                {
                    new Vector2(-halfWidth, -halfHeight),
                    new Vector2(halfWidth, -halfHeight),
                    new Vector2(halfWidth, halfHeight),
                    new Vector2(-halfWidth, halfHeight)
                },
                Position);
        }
    }
}
