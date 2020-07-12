using Relatus;
using Relatus.Graphics;

namespace KeepStalling
{
    class Table : RelatusObject
    {
        private Sprite sprite;
        private Quad area;
        public Table(float x, float y) : base(x, y, 139, 41)
        {
            sprite = new Sprite(x, y, "table");
            area = new Quad(x, y, 139, 41);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void SetBounds(float x, float y, int width, int height)
        {
            base.SetBounds(x, y, width, height);
        }

        public override void SetPosition(float x, float y)
        {
            base.SetPosition(x, y);
            sprite.SetPosition(x, y);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public void Update()
        {
            
        }

        public void Draw(Camera cam)
        {
            Sketch.Begin();
            {
                sprite.Draw(cam);
            }
            Sketch.End();
        }
    }
}