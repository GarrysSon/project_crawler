using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectCrawler.Management;

namespace ProjectCrawler.Objects.Generic.GameBase
{
    public class BreakableObject : GameObject
    {
        // A static collection of textures and their corresponding raw data - raw data is cached here so that objects that are broken often don't have to continuously retrieve it
        private static Dictionary<Texture2D, Color[]> cachedImageData = new Dictionary<Texture2D, Color[]>();

        // The texture that will be broken into chunks
        protected Texture2D texBreakable;

        // The raw image data from the texture as stored in a 1-dimensional array of colors
        private Color[] imageData;

        // A dictionary containing chunks and their corresponding control keys.
        private Dictionary<Vector2, Chunk> chunks;

        // Constructor
        public BreakableObject(Vector2 Position, Texture2D TexBreakable, int ChunkCount, int MinSpeed, int MaxSpeed, Vector2 VectorOrigin, float BaseY, Vector2 DisplaySize) : base()
        {
            // Set the position of the breakable object
            position = Position - DisplaySize / 2f;

            // Set the texture of the breakable object
            texBreakable = TexBreakable;

            // Check if the image data has been loaded and cached
            if (!cachedImageData.ContainsKey(texBreakable))
            {
                // Create a temporary array to store the raw data
                Color[] tempData = new Color[texBreakable.Width * texBreakable.Height];

                // Retrieve the raw image data and add it to the cached data
                texBreakable.GetData(tempData);
                cachedImageData.Add(texBreakable, tempData);
            }

            // Retrieve the image data of the breakable object
            imageData = cachedImageData[texBreakable];

            // Create a new dictionary of chunks
            chunks = new Dictionary<Vector2, Chunk>();

            // Break this object into chunks
            BreakIntoChunks(ChunkCount, MinSpeed, MaxSpeed, VectorOrigin, BaseY);
        }

        private void BreakIntoChunks(int ChunkCount, int MinSpeed, int MaxSpeed, Vector2 VectorOrigin, float BaseY)
        {
            // Random object.
            Random rand = new Random();

            // A list of control points
            List<Vector2> points = new List<Vector2>();

            // Create points equivalent to the number of chunks requested.
            for (int i = 0; i < ChunkCount; i++)
            {
                points.Add(new Vector2(rand.Next(texBreakable.Width), rand.Next(texBreakable.Height)));
            }

            // Default vector which will be compared against
            Vector2 defaultVector = new Vector2(texBreakable.Width * 3, texBreakable.Height * 3);
            Vector2 closestVector;

            // Current point
            Vector2 currentPoint = new Vector2(0);

            // Previous distance
            float previousDistance;
            float currentDistance;

            // Loop through the texture by width and height to split it up
            for (int x = 0; x < texBreakable.Width; x++)
            {
                for (int y = 0; y < texBreakable.Height; y++)
                {
                    // Set the starting closest vector to the default vector
                    closestVector = defaultVector;

                    // Set the current point
                    currentPoint.X = x;
                    currentPoint.Y = y;

                    // Set the previous distance to the maximum floating point number
                    previousDistance = float.MaxValue;

                    // Iterate through each of the points chosen to represent chunks and see if the iterated chunk origin is closer to the point x, y than closestVector
                    foreach (Vector2 v in points)
                    {
                        // Set the current distance to the distance between the current point and the currently iterated control point
                        currentDistance = (currentPoint - v).LengthSquared();

                        if (previousDistance > currentDistance)
                        {
                            previousDistance = currentDistance;
                            closestVector = v;
                        }
                    }

                    // See if a chunk has been created for this vector
                    if (!chunks.ContainsKey(closestVector))
                    {
                        float speed = (rand.Next(MaxSpeed - MinSpeed) + MinSpeed);
                        Vector2 vDiff = closestVector - VectorOrigin;
                        double angle = Math.Atan2(vDiff.Y, vDiff.X);
                        chunks.Add(
                            closestVector, 
                            new Chunk(
                                position.X, 
                                position.Y, 
                                texBreakable.Width, 
                                texBreakable.Height, 
                                (float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed,
                                BaseY));
                    }

                    // Get the closest chunk
                    Chunk tempChunk = chunks[closestVector];

                    // Set the color at point x, y for the closest chunk
                    tempChunk.points[x, y] = imageData[x + y * texBreakable.Width];

                    // Update the bounds for this chunk
                    tempChunk.UpdateBounds(x, y);
                }
            }
        }

        public override void Update()
        {
            // Number of dead chunks
            int deadChunks = 0;

            foreach (Chunk c in chunks.Values)
            {
                if (!c.UpdateChunk())
                {
                    deadChunks++;
                }
            }

            // Deregister this object.
            if (deadChunks == chunks.Values.Count())
            {
                LevelManager.CurrentLevel.DeregisterGameObject(this);
            }
        }

        public override void Render()
        {
            Vector2 offset = Vector2.Zero;

            foreach (Chunk c in chunks.Values)
            {
                c.DrawChunk();
            }
        }

        private class Chunk
        {
            public Color[,] points;

            int leftBound, rightBound, topBound, bottomBound;

            float posX, posY, spdX, spdY, alpha, baseY;

            bool isMoving;

            private Texture2D texBlank;

            public Chunk(float PosX, float PosY, int Width, int Height, float SpdX, float SpdY, float BaseY)
            {
                leftBound = int.MaxValue;
                rightBound = int.MinValue;
                topBound = int.MaxValue;
                bottomBound = int.MinValue;

                points = new Color[Width, Height];
                texBlank = Renderer.GetImage("blank");

                posX = PosX;
                posY = PosY;
                spdX = SpdX;
                spdY = SpdY;
                alpha = 1.0f;
                baseY = BaseY;

                isMoving = true;
            }

            public bool UpdateChunk()
            {
                if (isMoving)
                {
                    spdY += 1f;

                    posX += spdX;
                    posY += spdY;

                    bool changeSpeed = false;

                    if (posY + bottomBound > baseY)
                    {
                        posY = baseY - bottomBound;
                        changeSpeed = true;
                    }

                    if (changeSpeed)
                    {
                        if (spdY > 2f)
                        {
                            spdX *= 0.35f;
                            spdY *= -0.35f;
                        }
                        else
                        {
                            isMoving = false;
                        }
                    }
                }
                else
                {
                    alpha -= 0.0333f;

                    if (alpha <= 0)
                    {
                        return false;
                    }
                }

                return true;
            }

            public void DrawChunk()
            {
                // Only draw if the alpha is greater than 0
                if (alpha > 0)
                {
                    for (int x = leftBound; x <= rightBound; x++)
                    {
                        for (int y = topBound; y <= bottomBound; y++)
                        {
                            if (points[x, y].A > 0)
                            {
                                Renderer.DrawSprite(
                                    "blank", 
                                    new Vector2(x + posX, y + posY), 
                                    Vector2.One, 
                                    ColorFilter: points[x, y] * alpha, 
                                    Depth: 0f);
                            }
                        }
                    }
                }

                Renderer.DrawSprite(
                    "dropShadow", 
                    new Vector2(posX + (rightBound - leftBound) / 2 + leftBound, baseY), 
                    new Vector2(30, 15), 
                    ColorFilter: Color.White * 0.3f * alpha, 
                    Depth: GlobalConstants.SHADOW_DEPTH);
            }

            public void UpdateBounds(int X, int Y)
            {
                leftBound = Math.Min(leftBound, X);
                rightBound = Math.Max(rightBound, X);
                topBound = Math.Min(topBound, Y);
                bottomBound = Math.Max(bottomBound, Y);
            }
        }
    }
}
