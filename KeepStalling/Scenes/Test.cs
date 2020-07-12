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
        public Player Player { get; private set; }

        public Quadtree<Table> TableQuadtree { get; private set; }


        private Quad quad;

        public Test(string name) : base(name)
        {
            Tables = new List<Table>();
            entities = new List<RelatusObject>();
            Player = new Player(200, 0);
            entities.Add(Player);

            quad = new Quad(0, 0, WindowManager.PixelWidth * 2, WindowManager.PixelHeight * 2) { Color = Color.Gray };
            quad.ApplyChanges();    

            for (int y = 0; y < WindowManager.PixelHeight * 2 / 41; y++)
            {
                for (int x = 0; x < WindowManager.PixelWidth * 2 / 139; x++)
                {
                    if (x % 2 == 0 && y % 3 == 0)
                    {
                        Tables.Add(new Table(139 * x, 41 * y, Tables.Count));
                    }
                }
            }


            // Tables.Add(new Table(150, 50, Tables.Count));
            // Tables.Add(new Table(300, 50, Tables.Count));
            // Tables.Add(new Table(150, 150, Tables.Count));
            // Tables.Add(new Table(300, 150, Tables.Count));
            // Tables.Add(new Table(150, 250, Tables.Count));
            // Tables.Add(new Table(300, 250, Tables.Count));
            // Tables.Add(new Table(150, 350, Tables.Count));
            // Tables.Add(new Table(300, 350, Tables.Count));

            TableQuadtree = new Quadtree<Table>(new RectangleF(0, 0, WindowManager.PixelWidth * 2, WindowManager.PixelHeight * 2), 256);

            foreach (Table t in Tables)
            {
                TableQuadtree.Insert(t);
                entities.Add(t);
            }

            entities.Add(new Worker(0, 0, true, true));

            entities.Sort();

        }

        public override void LoadScene()
        {
        }

        public override void UnloadScene()
        {
        }

        public override void Update()
        {

            foreach (Entity e in entities) e.Update();

            Camera.SmoothTrack(Player.Center);
        }
        public override void Draw()
        {
            Sketch.CreateBackgroundLayer(Color.White);


            Sketch.AttachEffect(new Outline(Engine.RenderTarget, 1, new Color(Color.Black, 0.25f)));
            Sketch.AttachEffect(new ChromaticAberration(Engine.RenderTarget, 4, new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1)));
            Sketch.Begin();
            {

                quad.Draw(Camera);

                Polygon[] polygons = new Polygon[Player.Farts.Count];
                for (int i = 0; i < polygons.Length; i++)
                {
                    polygons[i] = Player.Farts[i].Circle;

                }
                Batcher.DrawPolygons(polygons, Camera);
            }
            Sketch.End();

            Sketch.AttachEffect(new DropShadow(Engine.RenderTarget, new Vector2(1, 1), 4, new Color(Color.Black, 100)));
            Sketch.Begin();
            {
                foreach (Entity e in entities) e.Draw(Camera);
            }
            Sketch.End();



        }
    }
}