using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ProjectCrawler
{
    public class LevelManager
    {
        /// <summary>
        /// The current game level.
        /// </summary>
        private static GameLevel currentLevel;
        public static GameLevel CurrentLevel
        {
            get
            {
                return currentLevel;
            }
        }

        /// <summary>
        /// A collection of all levels registered with the manager.
        /// </summary>
        private static Dictionary<string, GameLevel> gameLevels;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static LevelManager()
        {
            gameLevels = new Dictionary<string, GameLevel>();
        }

        /// <summary>
        /// Registers a game level with the manager. Note that this doesn't change the current level.
        /// </summary>
        /// <param name="Tag">Tag to associate with the level.</param>
        /// <param name="Level">The level to register.</param>
        public static void RegisterGameLevel(string Tag, GameLevel Level)
        {
            gameLevels[Tag] = Level;
        }

        /// <summary>
        /// Activates and sets as current the level associated with the given tag.
        /// </summary>
        /// <param name="Tag">The tag of the level to activate.</param>
        public static void ActivateLevel(string Tag)
        {
            if (currentLevel != null)
            {
                currentLevel.OnDeactivation();
            }
            currentLevel = gameLevels[Tag];
            currentLevel.OnActivation();
        }

        /// <summary>
        /// Updates the current level.
        /// </summary>
        public static void UpdateCurrentLevel()
        {
            currentLevel.Update();
        }

        /// <summary>
        ///  Renders the current level.
        /// </summary>
        public static void RenderCurrentLevel()
        {
            Renderer.BeginRender(
                Matrix.CreateTranslation(
                    new Vector3(
                        -(currentLevel.ScrollPoint.X - GlobalConstants.WINDOW_WIDTH / 2), 
                        -(currentLevel.ScrollPoint.Y - GlobalConstants.WINDOW_HEIGHT / 2), 
                        0)));
            currentLevel.Render();
            Renderer.EndRender();
        }
    }
}
