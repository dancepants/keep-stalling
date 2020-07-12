using Relatus;
using Relatus.ECS;
using Relatus.Graphics;
using Relatus.Input;
using Microsoft.Xna.Framework;
using System;
using Relatus.Graphics.Effects;
using System.Collections.Generic;
using Relatus.Maths;
using Microsoft.Xna.Framework.Input;
using Relatus.Utilities;

namespace KeepStalling
{
    class Test : Scene
    {
        private InputHandler input;
        private Sprite player;
        private Sprite background;
        private float dt;
        private float dtSpeed;

        private float amplitude;
        private float targetAmplitude;

        private List<Gas> farts;

        private Timer stepSoundTracker;
        private bool canStep;

        private Timer fartSoundTracker;
        private bool canFart;

        public Test(string name) : base(name)
        {
            player = new Sprite(0, 0, "player");
            player.RotationOffset = new Vector2(player.Width / 2, player.Height / 2);

            background = new Sprite(0, 0, "background");

            InputProfile profile = new InputProfile("player")
                .RegisterMapping(new InputMapping("Up") { Keys = new Keys[] { Keys.W, Keys.Up }, GamepadButtons = new Buttons[] { Buttons.DPadDown, Buttons.LeftThumbstickDown, Buttons.RightThumbstickDown } })
                .RegisterMapping(new InputMapping("Down") { Keys = new Keys[] { Keys.S, Keys.Down }, GamepadButtons = new Buttons[] { Buttons.DPadUp, Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp } })
                .RegisterMapping(new InputMapping("Left") { Keys = new Keys[] { Keys.A, Keys.Left }, GamepadButtons = new Buttons[] { Buttons.DPadLeft, Buttons.LeftThumbstickLeft, Buttons.RightThumbstickLeft } })
                .RegisterMapping(new InputMapping("Right") { Keys = new Keys[] { Keys.D, Keys.Right }, GamepadButtons = new Buttons[] { Buttons.DPadRight, Buttons.LeftThumbstickRight, Buttons.RightThumbstickRight } })
                .RegisterMapping(new InputMapping("Fart") { Keys = new Keys[] { Keys.Space, Keys.LeftShift }, GamepadButtons = new Buttons[] { Buttons.LeftShoulder, Buttons.RightShoulder } });

            input = new InputHandler(PlayerIndex.One);

            input.LoadProfile(profile);

            farts = new List<Gas>();

            stepSoundTracker = new Timer(400);
            canStep = true;

            fartSoundTracker = new Timer(200);
            canFart = true;
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

            float speed = 60 * Engine.DeltaTime;
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
                targetAmplitude = 0.2f;
                dt += dtSpeed * 0.75f;
            }
            else
            {
                dtSpeed = 200 * Engine.DeltaTime;
                targetAmplitude = 0.1f;
                dt += dtSpeed * 5f;


                if (canStep)
                {
                    SoundManager.PlaySoundEffect("step", 0.4f);
                    canStep = false;
                    stepSoundTracker.Reset();
                    stepSoundTracker.Start();
                }
            }

            if (!canStep)
            {
                stepSoundTracker.Update();

                if (stepSoundTracker.Done)
                {
                    canStep = true;
                }
            }

            if (!(targetAmplitude - 0.01 < amplitude && amplitude < targetAmplitude + 0.01))
            {
                float desired = targetAmplitude - amplitude;
                amplitude += desired * Engine.DeltaTime;
            }

            player.Rotation = (float)Math.Sin(dt * 0.01f) * amplitude;
            player.Scale = new Vector2( (float)Math.Sin(dt * 0.01f) * 0.1f + 1, 1);

            // FArts
            if (input.Pressed("fart"))
            {
                int total = MoreRandom.Next(4, 32);
                for (int i = 0; i < total; i++)
                {
                    Vector2 offset = Vector2Ext.Random() * MoreRandom.Next(16, 48 + 1);
                    farts.Add(new Gas(player.X + offset.X, player.Y + offset.Y));
                }

                Camera.Shake(50, 4, 250);

                if (canFart)
                {

                    SoundManager.PlaySoundEffect($"fart_{MoreRandom.Next(0, 3)}", 0.8f);
                    canFart = false;
                    fartSoundTracker.Reset();
                    fartSoundTracker.Start();

                }
            }

            if (!canFart)
            {
                fartSoundTracker.Update();

                player.Scale = new Vector2((float)Math.Cos(Engine.TotalGameTime.Milliseconds) * 0.25f + 1, (float)Math.Sin(Engine.TotalGameTime.Milliseconds) * 0.25f + 1);

                if (fartSoundTracker.Done)
                {
                    canFart = true;
                }
            }

            for (int i = farts.Count - 1; i >= 0; i--)
            {
                farts[i].Update();

                if (!farts[i].Lingering)
                {
                    farts.RemoveAt(i);
                }
            }

        }
        public override void Draw()
        {
            if (!canFart)
            {
                Sketch.AttachEffect(new Invert());
            }
            Sketch.Begin();
            {
                background.Draw(Camera);
            }
            Sketch.End();


            Sketch.AttachEffect(new Outline(Engine.RenderTarget, 1, new Color(Color.Black, 0.25f)));
            Sketch.AttachEffect(new ChromaticAberration(Engine.RenderTarget, 4, new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1)));
            //Sketch.DisableRelay();
            Sketch.Begin();
            {
                foreach (Gas g in farts)
                {
                    g.Draw(Camera);
                }
            }
            Sketch.End();
            //SketchHelper.ApplyGaussianBlur(Sketch.InterceptRelay(), 1);


            Sketch.AttachEffect(new DropShadow(Engine.RenderTarget, new Vector2(1, 1), 4, new Color(Color.Black, 100)));
            Sketch.Begin();
            {
                player.Draw(Camera);
            }
            Sketch.End();
        }
    }
}