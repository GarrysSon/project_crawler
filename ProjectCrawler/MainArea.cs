using Microsoft.Xna.Framework;

namespace ProjectCrawler
{
    /// <summary>
    /// The main area of the game where the actual gameplay takes place.
    /// </summary>
    public class MainArea : GameLevel
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        public MainArea() : base()
        {
            // Add a player character.
            Player player = new Player(new Vector2(400, 240));
            this.RegisterGameObject(player);
            // Add a camera to follow the player
            SmoothCamera smoothCamera = new SmoothCamera(player.Position, player, 0.95f);
            this.camera = smoothCamera;
            this.RegisterGameObject(Camera);
            // Add an enemy.
            this.RegisterGameObject(new FunnyEnemy(new Vector2(100.0f)));
            // Add a PolyWall as a test.
            Polygon wallShape = new Polygon(
                new Vector2[]
                {
                    new Vector2(-350, -100),
                    new Vector2(-250, -190),
                    new Vector2(250, -190),
                    new Vector2(350, -100),
                    new Vector2(350, 100),
                    new Vector2(250, 190),
                    new Vector2(-250, 190),
                    new Vector2(-350, 100)
                },
                new Vector2(400, 240));
            PolyWall wallObject = new PolyWall(wallShape);
            this.RegisterGameObject(wallObject);
            this.StoreValue(GlobalConstants.TEST_WALL_TAG, wallObject);
        }

        /// <summary>
        /// Renders the level.
        /// </summary>
        public override void Render()
        {
            // Clear the screen to a brownish color
            Renderer.ClearScreen(Color.BurlyWood);

            // Finish rendering the level as normal
            base.Render();
        }

        /// <summary>
        /// Called when this level is activated.
        /// </summary>
        public override void OnActivation()
        {
        }

        /// <summary>
        /// Called when this level is deactivated.
        /// </summary>
        public override void OnDeactivation()
        {
        }
    }
}
