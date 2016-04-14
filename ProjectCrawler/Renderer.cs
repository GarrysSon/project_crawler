using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ProjectCrawler
{
    /// <summary>
    /// Facilitates handling and drawing of images.
    /// </summary>
    public  class Renderer
    {
        private static ContentManager cm;
        private static GraphicsDevice gd;
        private static SpriteBatch spriteBatch;
        private static Dictionary<string, Texture2D> textures;
        private static Dictionary<string, RenderTarget2D> renderTargets;

        /// <summary>
        /// Initializes the Renderer to do its job.
        /// </summary>
        /// <param name="CM">ContentManager to use.</param>
        /// <param name="GD">GraphicsDevice to use.</param>
        public static void Init(ContentManager CM, GraphicsDevice GD)
        {
            cm = CM;
            gd = GD;
            gd.PresentationParameters.BackBufferWidth = GlobalConstants.WINDOW_WIDTH;
            gd.PresentationParameters.BackBufferHeight = GlobalConstants.WINDOW_HEIGHT;
            spriteBatch = new SpriteBatch(gd);
            textures = new Dictionary<string, Texture2D>();
            renderTargets = new Dictionary<string, RenderTarget2D>();
        }

        /// <summary>
        /// Loads an image.
        /// </summary>
        /// <param name="Tag">Tag to associate with the image.</param>
        /// <param name="Path">Path of the image.</param>
        public static void LoadImage(string Tag, string Path)
        {
            textures[Tag] = cm.Load<Texture2D>(Path);
        }

        /// <summary>
        /// Adds a new render target.
        /// </summary>
        /// <param name="Tag">Tag to associate with the render target.</param>
        /// <param name="Width">Width of the render target.</param>
        /// <param name="Height">Height of the render target.</param>
        public static void AddRenderTarget(string Tag, int Width, int Height)
        {
            renderTargets[Tag] = new RenderTarget2D(gd, Width, Height);
        }

        /// <summary>
        /// Sets the currently active render target.
        /// </summary>
        /// <param name="Tag">Tag of the render target to set.</param>
        public static void SetRenderTarget(string Tag = null)
        {
            gd.SetRenderTarget(Tag == null ? null : renderTargets[Tag]);
        }

        /// <summary>
        /// Begins a rendering cycle, typically for a frame.
        /// </summary>
        public static void BeginRender(Matrix? Transformation = null)
        {
            spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront, transformMatrix: Transformation);
        }

        /// <summary>
        /// Ends the current rendering cycle.
        /// </summary>
        public static void EndRender()
        {
            spriteBatch.End();
        }

        /// <summary>
        /// Clear the screen to a given color.
        /// </summary>
        /// <param name="C">The color to clear the screen to.</param>
        public static void ClearScreen(Color C)
        {
            gd.Clear(C);
        }

        /// <summary>
        /// Draws a sprite at a given location with a given size.
        /// </summary>
        /// <param name="Tag">Tag of a previously loaded image.</param>
        /// <param name="Position">Position to draw it.</param>
        /// <param name="Size">Size to draw it.</param>
        public static void DrawSprite(string Tag, Vector2 Position, Vector2 Size, float Angle = 0f, Color? ColorFilter = null, float Depth = 0.0f)
        {
            Texture2D tex = textures[Tag];
            spriteBatch.Draw(
                tex, 
                new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), 
                null, 
                ColorFilter == null ? Color.White : ColorFilter.Value, 
                Angle, 
                new Vector2(tex.Width / 2, tex.Height / 2), 
                SpriteEffects.None, 
                Depth);
        }

        /// <summary>
        /// Generates a depth value based on a position, with depth decreasing as Y increases.
        /// </summary>
        /// <param name="Position">Position to generate depth from.</param>
        /// <returns></returns>
        public static float GenerateDepthFromScreenPosition(Vector2 Position)
        {
            return 1.0f - Position.Y / (float)gd.Viewport.Height;
        }
    }
}
