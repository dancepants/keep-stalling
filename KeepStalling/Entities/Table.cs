using System.Collections.Generic;
using Relatus;
using Relatus.Graphics;

namespace KeepStalling
{
    class Table : RelatusObject
    {
        private Sprite sprite;
        public Table(float x, float y) : base(x, y, 139, 80)
        {
            sprite = new Sprite(x, y, "table");
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

        public void Update(List<Gas> gasses)
        {
           foreach (Gas g in gasses) {
               if (g.Collides(Bounds)) {
                   g.Bounce();
               }
           }
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