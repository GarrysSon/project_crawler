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
        /// A list of all objects to be deregistered from the level.
        /// </summary>
        protected List<GameObject> deregisteredObjects;

        /// <summary>
        /// Base constructor.
        /// </summary>
        public GameLevel()
        {
            this.InitializeBase();
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
        /// Deregisters the given game object at the end of the current frame.
        /// </summary>
        /// <param name="GO">The game object to deregister.</param>
        public void DeregisterGameObject(GameObject GO)
        {
            this.deregisteredObjects.Add(GO);
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

        /// <summary>
        /// Updates the level and registered objects.
        /// </summary>
        public virtual void Update()
        {
            // Update each of the game objects
            foreach (GameObject g in this.gameObjects)
            {
                if (!this.deregisteredObjects.Contains(g))
                {
                    g.Update();
                }
            }

            // Deregister any pending objects
            foreach (GameObject g in this.deregisteredObjects)
            {
                this.gameObjects.Remove(g);
            }
        }

        /// <summary>
        /// Renders the objects in the level.
        /// </summary>
        public virtual void Render()
        {
            foreach (GameObject g in this.gameObjects)
            {
                g.Render();
            }
        }

        /// <summary>
        /// Resets the level to an intial state.
        /// </summary>
        public virtual void Reset()
        {
            this.InitializeBase();
        }

        private void InitializeBase()
        {
            this.gameObjects = new List<GameObject>();
            this.typedGameObjects = new Dictionary<Type, List<GameObject>>();
            this.deregisteredObjects = new List<GameObject>();
        }
    }
}
