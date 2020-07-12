using Relatus;
using Relatus.ECS;
using Relatus.Graphics;
using Relatus.Input;
using Relatus.Debug;
using Microsoft.Xna.Framework;
using System;
using Relatus.Graphics.Effects;
using System.Collections.Generic;
using Relatus.Maths;
using Microsoft.Xna.Framework.Input;
using Relatus.Utilities;

namespace KeepStalling
{
    class Player : Entity 
    {
        public List<Gas> Farts { get; }
        private InputHandler input;
        private Sprite sprite;

        private Timer stepSoundTracker;
        private bool canStep;

        private Timer fartSoundTracker;
        public bool CanFart;

        private Timer airCooldown;
        private Timer airTimer;

        private Timer fartTimer;

        private float dt, dtSpeed, amplitude, targetAmplitude;


        private Quad debugBounds;

        public Player(float x, float y) : base(x, y, 64, 48)
        {
            sprite = new Sprite(x, y, "player");
            sprite.RotationOffset = new Vector2(sprite.Width / 2, sprite.Height / 2);

            InputProfile iprofile = new InputProfile("player")
                .RegisterMapping(new InputMapping("Up") { Keys = new Keys[] { Keys.W, Keys.Up }, GamepadButtons = new Buttons[] { Buttons.DPadUp, Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp } })
                .RegisterMapping(new InputMapping("Down") { Keys = new Keys[] { Keys.S, Keys.Down }, GamepadButtons = new Buttons[] { Buttons.DPadDown, Buttons.LeftThumbstickDown, Buttons.RightThumbstickDown } })
                .RegisterMapping(new InputMapping("Left") { Keys = new Keys[] { Keys.A, Keys.Left }, GamepadButtons = new Buttons[] { Buttons.DPadLeft, Buttons.LeftThumbstickLeft, Buttons.RightThumbstickLeft } })
                .RegisterMapping(new InputMapping("Right") { Keys = new Keys[] { Keys.D, Keys.Right }, GamepadButtons = new Buttons[] { Buttons.DPadRight, Buttons.LeftThumbstickRight, Buttons.RightThumbstickRight } })
                .RegisterMapping(new InputMapping("Air") { Keys = new Keys[] { Keys.LeftShift }, GamepadButtons = new Buttons[] { Buttons.LeftTrigger, Buttons.RightTrigger } });

            input = new InputHandler(PlayerIndex.One);
            input.LoadProfile(iprofile);

            Farts = new List<Gas>();

            stepSoundTracker = new Timer(400);
            canStep = true;

            fartSoundTracker = new Timer(200);
            CanFart = true;

            airCooldown = new Timer(1000);
            airTimer = new Timer(500);
            airCooldown.Start();
            airTimer.Start();

            fartTimer = new Timer(2500);
            fartTimer.Start();

            debugBounds = new Quad(X, Y, Width, Height)
            {
                LineWidth = 2
            };

            debugBounds.ApplyChanges();

            DebugManager.RegisterDebugEntry(new DebugEntry("playerPos", "X:{0}, Y:{1}"));

            SetPosition(X, Y);
        }

        public override void SetBounds(float x, float y, int width, int height)
        {
            base.SetBounds(x, y, width, height);
        }

        public override void SetPosition(float x, float y)
        {
            base.SetPosition(x, y);
            Depth = (int) (Y + Height);

            sprite.SetPosition(X + Width / 2, Y + Height / 2);
            debugBounds.SetPosition(X, Y);
            debugBounds.ApplyChanges();
        }

        public override string ToString()
        {
            return base.ToString();
        }
        public override void Update()
        {
            input.Update();

            float speed = 60 * Engine.DeltaTime;
            Vector2 playerVelocity = Vector2.Zero;
            if (input.Pressing("up"))
            {
                playerVelocity = new Vector2(playerVelocity.X, -1);
            }
            if (input.Pressing("down"))
            {
                playerVelocity = new Vector2(playerVelocity.X, 1);
            }
            if (input.Pressing("left"))
            {
                playerVelocity = new Vector2(-1, playerVelocity.Y);
            }
            if (input.Pressing("right"))
            {
                playerVelocity = new Vector2(1, playerVelocity.Y);
            }


            if (playerVelocity == Vector2.Zero)
            {
                // Not Moving
                dtSpeed = 1000 * Engine.DeltaTime;
                targetAmplitude = 0.2f;
                dt += dtSpeed * 0.75f;
            }
            else
            {
                playerVelocity.Normalize();
                playerVelocity *= speed;

                // sprite.X += playerVelocity.X;
                // sprite.Y += playerVelocity.Y;
                SetPosition(X + playerVelocity.X, Y + playerVelocity.Y);


                // MOving
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

            sprite.Rotation = (float)Math.Sin(dt * 0.01f) * amplitude;
            sprite.Scale = new Vector2((float)Math.Sin(dt * 0.01f) * 0.1f + 1, 1);

            // FArts
            fartTimer.Update();
            if (fartTimer.Done)
            {
                if (MoreRandom.NextDouble(0, 1) < 0.25) {
                    int total = MoreRandom.Next(16, 64);
                    for (int i = 0; i < total; i++)
                    {
                        Vector2 offset = Vector2Ext.Random() * MoreRandom.Next(16, 48 + 1);
                        Farts.Add(new Gas(sprite.X + offset.X, sprite.Y + offset.Y));
                    }


                    if (CanFart)
                    {

                        SoundManager.PlaySoundEffect($"fart_{MoreRandom.Next(0, 3)}", 0.8f);
                        CanFart = false;
                        fartSoundTracker.Reset();
                        fartSoundTracker.Start();

                    }
                }
                fartTimer.Reset();
                fartTimer.Start();
            }

            if (input.Pressing("air"))
            {
                if (airCooldown.Done)
                {
                    airTimer.Update();
                    int total = MoreRandom.Next(4, 8);
                    for (int i = 0; i < total; i++)
                    {
                        Vector2 offset = Vector2Ext.Random() * MoreRandom.Next(8, 16 + 1);
                        Gas g = new Gas(sprite.X + offset.X, sprite.Y + offset.Y).Crazy();

                        if (playerVelocity != Vector2.Zero)
                        {
                            g.AddToVelocity(-playerVelocity.X * 100, -playerVelocity.Y * 100);
                        }
                        g.MoreT();
                        Farts.Add(g);
                    }
                    if (airTimer.Done)
                    {
                        airCooldown.Reset();
                        airTimer.Reset();
                        airCooldown.Start();
                        airTimer.Start();
                    }
                }
            }
            else
            {
                airCooldown.Update();
            }

            if (!CanFart)
            {
                fartSoundTracker.Update();

                sprite.Scale = new Vector2((float)Math.Cos(Engine.TotalGameTime.Milliseconds) * 0.25f + 1, (float)Math.Sin(Engine.TotalGameTime.Milliseconds) * 0.25f + 1);

                if (fartSoundTracker.Done)
                {
                    CanFart = true;
                }
            }

            for (int i = Farts.Count - 1; i >= 0; i--)
            {
                Farts[i].Update();

                if (!Farts[i].Lingering)
                {
                    Farts.RemoveAt(i);
                }
            }



            TableCollision();

            DebugManager.GetDebugEntry("playerPos").SetInformation(X, Y);


            if (Bounds.Right < 0) {
                SetPosition(WindowManager.PixelWidth * 2 - Width, Y);
            }

            if (Bounds.Left > WindowManager.PixelWidth * 2) {
                SetPosition(4, Y);
            }

            
            if (Bounds.Bottom < 0) {
                SetPosition(X, WindowManager.PixelHeight * 2 - Width);
            }

            if (Bounds.Top > WindowManager.PixelHeight * 2) {
                SetPosition(X, 4);
            }


        }

        private void TableCollision()
        {
            Quadtree<Table> bin = ((Test)SceneManager.CurrentScene).TableQuadtree;

            int buffer = 16;
            List<int> queryResult = bin.Query(new RectangleF(X - buffer, Y - buffer, Width + buffer * 2, Height + buffer * 2));


            List<Table> tables = ((Test)SceneManager.CurrentScene).Tables;
            foreach (int i in queryResult)
            {
                Vector2 resolution = Bounds.GetResolution(tables[i].Bounds);                
                if (resolution != Vector2.Zero)
                {
                    debugBounds.Color = Color.Red;
                    SetPosition(X - resolution.X, Y - resolution.Y);
                }                
                else{

                    debugBounds.Color = Color.White;
                }
                debugBounds.ApplyChanges();
            }


        }

        public override void Draw(Camera cam)
        {


                sprite.Draw(cam);


                if (DebugManager.Debugging)
                {
                    debugBounds.Draw(cam);
                }

        }
    }
}