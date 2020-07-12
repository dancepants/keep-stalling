using System;
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

        public Circle Circle {get; private set;}
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

            Circle = new Circle(x, y, initialRadius)
            {
                Color = initialColor
            };
            Circle.ApplyChanges();

            velocity = Vector2Ext.Random() * MoreRandom.Next(25, 120);

            rateOfDecay = (float)MoreRandom.NextDouble(8, 40);
            Lingering = true;
        }

        public Gas AddToVelocity(float x, float y){
            velocity += new Vector2(x,y);

            return this;
        }

        public Gas MultiplyToVelocity(float x, float y) {
            velocity = new Vector2(velocity.X * x, velocity.Y * y);
            
            return this;
        }


        public Gas Bounce() {
            MultiplyToVelocity(-0.5f, -0.5f);

            return this;
        }

        public bool Collides(RectangleF rectangle) {
            float rectX = Circle.X, rectY = Circle.Y;    

            if (Circle.X < rectangle.X) rectX = rectangle.X;
            else if (Circle.X > rectangle.X + rectangle.Width) rectX = rectangle.X + rectangle.Width;

            if (Circle.Y < rectangle.Y) rectY = rectangle.Y;
            else if (Circle.Y > rectangle.Y + rectangle.Height) rectY = rectangle.Y + rectangle.Height;

            float dx = Circle.X - rectX;
            float dy = Circle.Y - rectY;
            
            if (Math.Sqrt(dx * dx + dy * dy) <= Circle.Radius) {
                return true;
            }
            return false;
        }

        public Gas Crazy()
        {
            velocity *= (float)MoreRandom.NextDouble(0.5, 0.7);
            rateOfDecay *= (float)MoreRandom.NextDouble(0.1, 0.5);

            return this;
        }

        public void Update()
        {
            if (!Lingering)
            {
                return;
            }

            Circle.X += velocity.X * Engine.DeltaTime;
            Circle.Y += velocity.Y * Engine.DeltaTime;
            Circle.Radius -= rateOfDecay * Engine.DeltaTime;
            Circle.Color = new Color(initialColor, Circle.Radius / initialRadius);
            Circle.ApplyChanges();

            if (Circle.Radius <= 1)
            {
                Lingering = false;
            }

        }

        public void Draw(Camera camera)
        {
            Circle.Draw(camera);
        }
    }
}