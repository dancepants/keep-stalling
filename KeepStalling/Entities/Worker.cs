using Microsoft.Xna.Framework;
using Relatus;
using Relatus.Graphics;
using Relatus.Utilities;

namespace KeepStalling
{
    class Worker : Entity
    {
        private Sprite sprite;
        private Timer walkTimer;
        private Timer stopTimer;
        private Vector2 velocity;
        private bool right;
        public Worker(float x, float y, bool startRight, bool boy) : base(x, y, 10, 10)
        {
            sprite = new Sprite(x, y, "coworker");
            walkTimer = new Timer(5000);
            stopTimer = new Timer(2500);
            walkTimer.Start();
            right = startRight;
            if (right) velocity = new Vector2(20, 0);
            else velocity = new Vector2(-20, 0);
        }
        public override void SetPosition(float x, float y)
        {
            base.SetPosition(x, y);
            sprite.SetPosition(x, y);
        }
        public override void Update()
        {
            if (walkTimer.Done)
            {
                velocity.X = -velocity.X;
                stopTimer.Start();
                walkTimer.Reset();
            }
            else
            {
                SetPosition(X + velocity.X, Y + velocity.Y);
            }
            if (stopTimer.Done)
            {
                walkTimer.Start();
                stopTimer.Reset();
            }
        }
        public override void Draw(Camera camera)
        {
            sprite.Draw(camera);
        }
    }
}