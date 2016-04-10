using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            set
            {
                currentLevel.OnDeactivation();
                currentLevel = value;
                currentLevel.OnActivation();
            }
        }

        /// <summary>
        /// A collection of all levels registered with the manager.
        /// </summary>
        private static Dictionary<String, GameLevel> gameLevels;

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
        public static void RegisterGameLevel(String Tag, GameLevel Level)
        {
            gameLevels[Tag] = Level;
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
            currentLevel.Render();
        }
    }
}
