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
        private List<Table> tables;
        private Sprite background;
        private Player player;
        public Test(string name) : base(name)
        {
            tables = new List<Table>();
            background = new Sprite(0, 0, "background");
            player = new Player(0, 0);

            tables.Add(new Table(50, 50));
        }

        public override void LoadScene()
        {
        }

        public override void UnloadScene()
        {
        }

        public override void Update()
        {
            foreach (Table t in tables) t.Update(player.farts);
            player.Update(Camera);
        }
        public override void Draw()
        {
            Sketch.Begin();
            {
                // background.Draw(Camera);
            }
            Sketch.End();
            // The following objects begins and ends Sketch internally
            player.Draw(Camera);
            foreach (Table t in tables) t.Draw(Camera);
        }
    }
}