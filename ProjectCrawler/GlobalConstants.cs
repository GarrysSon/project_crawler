using Microsoft.Xna.Framework;

namespace ProjectCrawler
{
    public class GlobalConstants
    {
        /// <summary>
        /// Rendering constants.
        /// </summary>
        public const int WINDOW_WIDTH = 800;
        public const int WINDOW_HEIGHT = 480;
        public static readonly Vector2 WINDOW_SIZE = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);

        public static readonly string MAIN_AREA_TAG = "MAIN_AREA";

        public static readonly string TEST_WALL_TAG = "TEST_WALL";

        public static float SHADOW_DEPTH = 0.9999f;

        /// <summary>
        /// All needed image tag constants go here.
        /// </summary>
        public static readonly string NINJA_IMAGE_TAG = "ninja";
        public static readonly string FUNNY_ENEMY_IMAGE_TAG = "funnyEnemy";
        public static readonly string DROP_SHADOW_IMAGE_TAG = "dropShadow";
        public static readonly string TOWER_ROOM_IMAGE_TAG = "TOWER_ROOM_IMAGE";
        public static readonly string BLANK_IMAGE_TAG = "blank";
        public static readonly string SHURIKEN_IMAGE_TAG = "shuriken";
        public static readonly string FART_HEART_IMAGE_TAG = "fartHeart";

    }
}
