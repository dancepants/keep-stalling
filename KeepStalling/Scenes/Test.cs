using Relatus;
using Relatus.ECS;
using Relatus.Graphics;
using Relatus.Input;
using Microsoft.Xna.Framework;
using System;
namespace KeepStalling
{
    class Test : Scene
    {
        private InputHandler input;
        private Sprite player;


        private float dt;
        private float dtSpeed;

        private float amplitude;
        private float targetAmplitude;

        public Test(string name) : base(name)
        {
            player = new Sprite(0, 0, "player");
            player.RotationOffset = new Vector2(player.Width / 2, player.Height / 2);
            InputProfile profile = new InputProfile("test");
            input = new InputHandler(PlayerIndex.One);
            input.LoadProfile("basic");
        }
        public override void LoadScene()
        {
        }
        public override void UnloadScene()
        {
        }
        public override void Update()
        {
            input.Update();

            Vector2 initialPosition = player.Position;

            float speed = 30 * Engine.DeltaTime;
            if (input.Pressing("up"))
            {
                player.Y -= speed;
            }
            if (input.Pressing("down"))
            {
                player.Y += speed;
            }
            if (input.Pressing("left"))
            {
                player.X -= speed;
            }
            if (input.Pressing("right"))
            {
                player.X += speed;
            }

            if (player.Position == initialPosition)
            {
                dtSpeed = 1000 * Engine.DeltaTime;
                targetAmplitude = 0.3f;
                dt += dtSpeed * 0.75f;
            }
            else
            {
                dtSpeed = 200 * Engine.DeltaTime;
                targetAmplitude = 0.1f;
                dt += dtSpeed * 5f;
            }
            
            if (!(targetAmplitude - 0.01 < amplitude && amplitude < targetAmplitude + 0.01))
            {
                float desired = targetAmplitude - amplitude;
                amplitude += desired * Engine.DeltaTime;
            }

            player.Rotation = (float)Math.Sin(dt * 0.01f) * amplitude;

        }
        public override void Draw()
        {
            Sketch.Begin();
            {
                player.Draw(Camera);
            }
            Sketch.End();
        }
    }
}