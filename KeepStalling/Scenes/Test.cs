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
        private Sprite background;
        private Player player;
        public Test(string name) : base(name)
        {
            background = new Sprite(0, 0, "background");
            player = new Player(0, 0);
        }

        public override void LoadScene()
        {
        }

        public override void UnloadScene()
        {
        }

        public override void Update()
        {
            player.Update(Camera);
        }
        public override void Draw()
        {
            Sketch.Begin();
            {
                background.Draw(Camera);
            }
            Sketch.End();
            // Player.Draw begins and ends Sketch internally
            player.Draw(Camera);
        }
    }
}