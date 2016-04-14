using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ProjectCrawler.Management;
using ProjectCrawler.Objects.Game.Level;

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
            Renderer.LoadImage("blank", "Images/blank");
            Renderer.LoadImage("shuriken", "Images/shuriken");

            // Setup the levels
            LevelManager.RegisterGameLevel(GlobalConstants.MAIN_AREA_TAG, new MainArea());
            LevelManager.ActivateLevel(GlobalConstants.MAIN_AREA_TAG);
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

            // Update the current level
            LevelManager.UpdateCurrentLevel();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Render the current level
            LevelManager.RenderCurrentLevel();

            base.Draw(gameTime);
        }
    }
}
