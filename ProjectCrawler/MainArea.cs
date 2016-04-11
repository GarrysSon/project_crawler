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
            this.RegisterGameObject(new Player(new Vector2(400, 240)));
            // Add an enemy.
            this.RegisterGameObject(new FunnyEnemy(new Vector2(100.0f)));
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
