using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relatus.Graphics.Effects;
using Relatus;
using Relatus.ECS;
using Relatus.Graphics;
using Relatus.Maths;

namespace KeepStalling
{
    class Table : Entity, IPartitionable
    {
        private Sprite sprite;

        public int Identifier { get => id; }
        RectangleF IPartitionable.Bounds { get => Bounds; }

        private int id;

        public Table(float x, float y, int index) : base(x, y, 139, 41)
        {
            Depth = (int) (Y + Height);
            sprite = new Sprite(x, y, "table");
            id = index;
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

        public override void Update()
        {
            List<Gas> gasses = ((Test) SceneManager.CurrentScene).Player.Farts;
            foreach (Gas g in gasses)
            {
                if (g.Collides(Bounds))
                {
                    g.Bounce();
                }
            }
        }

        public override void Draw(Camera camera)
        {



            sprite.Draw(camera);
        }
    }
}