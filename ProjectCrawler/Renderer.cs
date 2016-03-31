﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        /// <summary>
        /// Initializes the Renderer to do its job.
        /// </summary>
        /// <param name="CM">ContentManager to use.</param>
        /// <param name="GD">GraphicsDevice to use.</param>
        public static void Init(ContentManager CM, GraphicsDevice GD)
        {
            cm = CM;
            gd = GD;
            spriteBatch = new SpriteBatch(gd);
            textures = new Dictionary<string, Texture2D>();
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
        /// Begins a rendering cycle, typically for a frame.
        /// </summary>
        public static void BeginRender()
        {
            spriteBatch.Begin();
        }

        /// <summary>
        /// Ends the current rendering cycle.
        /// </summary>
        public static void EndRender()
        {
            spriteBatch.End();
        }

        /// <summary>
        /// Draws a sprite at a given location with a given size.
        /// </summary>
        /// <param name="Tag">Tag of a previously loaded image.</param>
        /// <param name="Position">Position to draw it.</param>
        /// <param name="Size">Size to draw it.</param>
        public static void DrawSprite(string Tag, Vector2 Position, Vector2 Size)
        {
            Texture2D tex = textures[Tag];
            spriteBatch.Draw(
                tex, 
                new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), 
                null, 
                Color.White, 
                0.0f, 
                new Vector2(tex.Width / 2, tex.Height / 2), 
                SpriteEffects.None, 
                1.0f);
        }
    }
}