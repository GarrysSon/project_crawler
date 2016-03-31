using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCrawler
{
    /// <summary>
    /// Contains all methods for accessing game objects.
    /// </summary>
    public class GameObjectManager
    {
        /// <summary>
        /// The dictionary of all game objects.
        /// </summary>
        private static Dictionary<string, Object> gameObjects;

        /// <summary>
        /// Initializes the needed variables.
        /// </summary>
        public static void Init()
        {
            gameObjects = new Dictionary<string, Object>();
        }

        /// <summary>
        /// Adds a game object to the dictionary of game object.
        /// </summary>
        /// <param name="tag">The tag to identify the added game object.</param>
        /// <param name="gameObject">The game object to be added.</param>
        public static void AddObject(string tag, Object gameObject)
        {
            gameObjects.Add(tag, gameObject);
        }

        /// <summary>
        /// Gets the game object associated with the given tag.
        /// </summary>
        /// <param name="tag">The tag associated with a game object in the dictionary of objects.</param>
        /// <returns>The game object associated with the given tag.</returns>
        public static Object GetObject(string tag)
        {
            return gameObjects[tag];
        }

        /// <summary>
        /// Removes the game object associated with the given tag.
        /// </summary>
        /// <param name="tag">The tag associated with the given object in the dictionary of objects.</param>
        public static void RemoveObject(string tag)
        {
            gameObjects.Remove(tag);
        }
    }
}
