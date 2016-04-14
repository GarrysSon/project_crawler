using ProjectCrawler.Objects.Generic.Utility;

namespace ProjectCrawler.Objects.Generic.GameBase
{
    public abstract class GameObject : Polygon
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GameObject() : base()
        {
        }

        /// <summary>
        /// Constructor taking a polygon.
        /// </summary>
        /// <param name="Poly">Polygon for this object to use.</param>
        public GameObject(Polygon Poly) : base(Poly.Points, Poly.Position)
        {
        }

        /// <summary>
        /// Updates the object.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Renders the object.
        /// </summary>
        public abstract void Render();
    }
}
