using Relatus;

namespace KeepStalling {
    abstract class Entity : RelatusObject
    {
        protected Entity(float x, float y, int width, int height) : base(x, y, width, height)
        {
        }

        public abstract void Update();

        public abstract void Draw(Camera camera);
    }
}