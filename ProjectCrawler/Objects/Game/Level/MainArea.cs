using Microsoft.Xna.Framework;
using ProjectCrawler.Management;
using ProjectCrawler.Objects.Generic.Camera;
using ProjectCrawler.Objects.Generic.GameBase;
using ProjectCrawler.Objects.Generic.Utility;
using ProjectCrawler.Objects.Game.Player;
using ProjectCrawler.Objects.Game.Enemy;
using ProjectCrawler.Objects.Game.Level.Component;
using Microsoft.Xna.Framework.Input;
using System;

namespace ProjectCrawler.Objects.Game.Level
{
    /// <summary>
    /// The main area of the game where the actual gameplay takes place.
    /// </summary>
    public class MainArea : GameLevel
    {
        // Stuff for enemy spawning.
        bool isSpawnKeyPressed;

        /// <summary>
        /// Base constructor.
        /// </summary>
        public MainArea() : base()
        {
            // Add a player character.
            PlayerNinja player = new PlayerNinja(Vector2.Zero);
            this.RegisterGameObject(player);
            this.StoreValue(GlobalConstants.PLAYER_TAG, player);
            // Add a camera to follow the player
            SmoothCamera smoothCamera = new SmoothCamera(player.Position, player, new Rectangle(-512, -512, 1024, 1024), 0.95f);
            this.camera = smoothCamera;
            this.RegisterGameObject(Camera);
            // Add some enemies.
            this.RegisterGameObject(new FunnyEnemy(new Vector2(0, -300f), Vector2.UnitX));
            this.RegisterGameObject(new FunnyEnemy(new Vector2(300f, 0), Vector2.UnitY));
            this.RegisterGameObject(new FunnyEnemy(new Vector2(0, 300f), Vector2.UnitX * -1f));
            this.RegisterGameObject(new FunnyEnemy(new Vector2(-300f, 0), Vector2.UnitY * -1f));
            this.RegisterGameObject(new LazerEnemy(new Vector2(200)));
            // Add a PolyWall as a test.
            Polygon wallShape = new Polygon(
                new Vector2[]
                {
                    // Adjust the top four points upward to allow for some overlap from objects.
                    new Vector2(-448, -192 - 24),
                    new Vector2(-192, -448 - 24),
                    new Vector2(192, -448 - 24),
                    new Vector2(448, -192 - 24),
                    new Vector2(448, 192),
                    new Vector2(192, 448),
                    new Vector2(-192, 448),
                    new Vector2(-448, 192)
                },
                Vector2.Zero);
            PolyWall wallObject = new PolyWall(wallShape, GlobalConstants.TOWER_ROOM_IMAGE_TAG);
            this.RegisterGameObject(wallObject);
            this.StoreValue(GlobalConstants.TEST_WALL_TAG, wallObject);
            isSpawnKeyPressed = false;
        }

        /// <summary>
        /// Renders the level.
        /// </summary>
        public override void Render()
        {
            // Clear the screen to a brownish color
            Renderer.ClearScreen(Color.Black);

            // Finish rendering the level as normal
            base.Render();
        }

        /// <summary>
        /// Updates the level.
        /// </summary>
        public override void Update()
        {
            // Grab the player
            PlayerNinja player = RetrieveValue<PlayerNinja>(GlobalConstants.PLAYER_TAG);
            // Create new enemies if corresponding keys are pressed.
            Random rand = new Random();
            KeyboardState currentState = Keyboard.GetState();
            if (currentState.IsKeyDown(Keys.D1))
            {
                if (!isSpawnKeyPressed)
                {
                    isSpawnKeyPressed = true;
                    // Create a funny enemy
                    while (true)
                    {
                        double angle = rand.NextDouble() * Math.PI * 2;
                        Vector2 pos = new Vector2((float)Math.Cos(angle) * 400, (float)Math.Sin(angle) * 400);
                        if ((pos - player.Position).Length() < 200)
                        {
                            continue;
                        }
                        FunnyEnemy fe = new FunnyEnemy(
                            pos,
                            new Vector2((float)rand.NextDouble() - 0.5f, (float)rand.NextDouble() - 0.5f));
                        RegisterGameObject(fe);
                        break;
                    }
                }
            }
            else if (currentState.IsKeyDown(Keys.D2))
            {
                if (!isSpawnKeyPressed)
                {
                    isSpawnKeyPressed = true;
                    // Create a laser enemy
                    while (true)
                    {
                        double angle = rand.NextDouble() * Math.PI * 2;
                        Vector2 pos = new Vector2((float)Math.Cos(angle) * 400, (float)Math.Sin(angle) * 400);
                        if ((pos - player.Position).Length() < 200)
                        {
                            continue;
                        }
                        LazerEnemy le = new LazerEnemy(pos);
                        RegisterGameObject(le);
                        break;
                    }
                }
            }
            else
            {
                isSpawnKeyPressed = false;
            }
            base.Update();
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
