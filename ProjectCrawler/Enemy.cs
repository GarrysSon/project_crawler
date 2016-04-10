using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectCrawler
{
    /// <summary>
    /// Represents an abstract enemy.
    /// </summary>
    public abstract class Enemy : GameObject
    {
        /// <summary>
        /// Constructor taking the polygon shape of the enemy.
        /// </summary>
        /// <param name="Poly"></param>
        public Enemy(Polygon Poly) : base(Poly)
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
