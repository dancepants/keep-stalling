using Microsoft.Xna.Framework;
using Relatus;
using Relatus.Graphics;
using Relatus.Graphics.Palettes;
using Relatus.Maths;
using Relatus.Utilities;

namespace KeepStalling
{
    class Gas
    {
        public bool Lingering { get; private set; }

        private Circle circle;
        private float rateOfDecay;

        private Vector2 velocity;
        private float initialRadius;
        private Color initialColor;

        private static Color[] colors;
        static Gas()
        {
            colors = new Color[] { new Color(14, 56, 15), new Color(48, 98, 48), new Color(139, 172, 15), new Color(155, 188, 15) };
        }

        public Gas(float x, float y)
        {
            initialRadius = MoreRandom.Next(8, 17);
            initialColor = colors[MoreRandom.Next(0, colors.Length)];

            circle = new Circle(x, y, initialRadius)
            {
                Color = initialColor
            };
            circle.ApplyChanges();

            velocity = Vector2Ext.Random() * MoreRandom.Next(25, 120);

            rateOfDecay = (float)MoreRandom.NextDouble(8, 40);
            Lingering = true;
        }

        public void Update()
        {
            if (!Lingering)
            {
                return;
            }

            circle.X += velocity.X * Engine.DeltaTime;
            circle.Y += velocity.Y * Engine.DeltaTime;
            circle.Radius -= rateOfDecay * Engine.DeltaTime;
            circle.Color = new Color(initialColor, circle.Radius / initialRadius);
            circle.ApplyChanges();

            if (circle.Radius <= 1)
            {
                Lingering = false;
            }

        }

        public void Draw(Camera camera)
        {
            circle.Draw(camera);
        }
    }
}