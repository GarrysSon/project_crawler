using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProjectCrawler.Objects.Generic.Camera;

namespace ProjectCrawler.Objects.Generic.GameBase
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
        /// Stores arbitrary objects associated with string keys(tags).
        /// </summary>
        private Dictionary<string, object> keyValueStore;

        /// <summary>
        /// A GameObject to which the camera should scroll.
        /// </summary>
        protected AbstractCamera camera;
        public AbstractCamera Camera
        {
            get
            {
                return this.camera;
            }
            set
            {
                this.camera = value;
            }
        }

        public Vector2 ScrollPoint
        {
            get
            {
                if (this.camera != null)
                {
                    return this.camera.Position;
                }
                return GlobalConstants.WINDOW_SIZE / 2f;
            }
        }

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
            // Update each of the game objects.
            // Objects can be registered here, so we can't use foreach.
            for (int i = 0; i < this.gameObjects.Count; i++)
            {
                GameObject g = this.gameObjects[i];
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

        /// <summary>
        /// Stores a value associated with a tag.
        /// </summary>
        /// <param name="Tag">Tag to associate with the value.</param>
        /// <param name="Value">Value to store.</param>
        public void StoreValue(string Tag, object Value)
        {
            this.keyValueStore[Tag] = Value;
        }

        /// <summary>
        /// Retrieves the value associated with the given tag.
        /// </summary>
        /// <typeparam name="T">Type to retrieve the value as.</typeparam>
        /// <param name="Tag">Tag associated with the value to retrieve.</param>
        /// <returns></returns>
        public T RetrieveValue<T>(string Tag)
        {
            return (T)this.keyValueStore[Tag];
        }

        /// <summary>
        /// Called by LevelManager when this level is set as the current level.
        /// </summary>
        public abstract void OnActivation();

        /// <summary>
        /// Called by LevelManager when this level is removed as the current level.
        /// </summary>
        public abstract void OnDeactivation();

        private void InitializeBase()
        {
            this.gameObjects = new List<GameObject>();
            this.typedGameObjects = new Dictionary<Type, List<GameObject>>();
            this.deregisteredObjects = new List<GameObject>();
            this.keyValueStore = new Dictionary<string, object>();
        }
    }
}
