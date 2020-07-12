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
        public List<Table> Tables { get; private set; }
        private List<RelatusObject> entities;
        private Sprite background;
        public Player Player {get; private set;}

        public Quadtree<Table> TableQuadtree {get; private set;}

        public Test(string name) : base(name)
        {
            Tables = new List<Table>();
            entities = new List<RelatusObject>();
            background = new Sprite(0, 0, "background");
            Player = new Player(0, 0);
            entities.Add(Player);

            Tables.Add(new Table(200, 50, Tables.Count));

            TableQuadtree = new Quadtree<Table>(new RectangleF(0, 0, WindowManager.PixelWidth * 2, WindowManager.PixelHeight * 2), 256);

            foreach (Table t in Tables)
            {
                TableQuadtree.Insert(t);
                entities.Add(t);
            }


        }

        public override void LoadScene()
        {
        }

        public override void UnloadScene()
        {
        }

        public override void Update()
        {
            Camera.SmoothTrack(Player.Center);

            foreach (Entity e in entities) e.Update();
        }
        public override void Draw()
        {
            entities.Sort();
            Sketch.Begin();
            {
                // background.Draw(Camera);
            }
            Sketch.End();
            // The following objects begins and ends Sketch internally
            foreach (Entity e in entities) e.Draw(Camera);
        }
    }
}