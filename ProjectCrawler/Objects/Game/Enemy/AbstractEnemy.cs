using ProjectCrawler.Objects.Generic.GameBase;
using ProjectCrawler.Objects.Generic.Utility;

namespace ProjectCrawler.Objects.Game.Enemy
{
    /// <summary>
    /// Represents an abstract enemy.
    /// </summary>
    public abstract class AbstractEnemy : GameObject
    {
        /// <summary>
        /// Constructor taking the polygon shape of the enemy.
        /// </summary>
        /// <param name="Poly"></param>
        public AbstractEnemy(Polygon Poly) : base(Poly)
        {
        }

        /// <summary>
        /// Health of the enemy.
        /// </summary>
        protected int health;
        public int Health
        {
            get
            {
                return health;
            }
        }
    }
}
