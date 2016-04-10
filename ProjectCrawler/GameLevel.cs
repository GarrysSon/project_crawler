using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ProjectCrawler
{
    public abstract class GameLevel
    {
        /// <summary>
        /// A list of all game objects contained by the level.
        /// </summary>
        protected List<GameObject> gameObjects;

        /// <summary>
        /// A collection of all game objects contained by the level, grouped by type.
        /// </summary>
        protected Dictionary<Type, List<GameObject>> typedGameObjects;

        /// <summary>
        /// Base constructor.
        /// </summary>
        public GameLevel()
        {
            this.gameObjects = new List<GameObject>();
            this.typedGameObjects = new Dictionary<Type, List<GameObject>>();
        }

        /// <summary>
        /// Registers a game object with the level.
        /// </summary>
        /// <param name="GO">The game object to register.</param>
        public void RegisterGameObject(GameObject GO)
        {
            this.gameObjects.Add(GO);
            Type t = GO.GetType();
            while (t != typeof(GameObject))
            {
                if (!this.typedGameObjects.ContainsKey(t))
                {
                    this.typedGameObjects[t] = new List<GameObject>();
                }
                this.typedGameObjects[t].Add(GO);
                t = t.BaseType;
            }
        }

        /// <summary>
        /// Retrieves all game objects of type T.
        /// </summary>
        /// <typeparam name="T">The type of game objects to retrieve.</typeparam>
        /// <returns>A list of the desired game objects.</returns>
        public List<T> GetObjectsOfType<T>()
        {
            return this.typedGameObjects[typeof(T)] as List<T>;
        }
    }
}
