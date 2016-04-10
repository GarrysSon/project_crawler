using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrawler
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Renderer.Init(this.Content, this.GraphicsDevice);
            GameObjectManager.Init();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load our content
            Renderer.LoadImage("ninja", "Images/ninjaAttempt2");
            Renderer.LoadImage("funnyEnemy", "Images/weirdEnemy2");
            Renderer.LoadImage("dropShadow", "Images/dropShadow");
            GameObjectManager.AddObject("ninja", new Player(new Vector2(400, 240)));
            GameObjectManager.AddObject("enemy", new FunnyEnemy(new Vector2(100.0f)));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ((Player)GameObjectManager.GetObject("ninja")).Update();
            ((FunnyEnemy)GameObjectManager.GetObject("enemy")).Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BurlyWood);

            // Begin rendering all the shiz.
            Renderer.BeginRender();
            ((Player)GameObjectManager.GetObject("ninja")).Render();
            ((FunnyEnemy)GameObjectManager.GetObject("enemy")).Render();
            Renderer.EndRender();

            base.Draw(gameTime);
        }
    }
}
