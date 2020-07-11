using Relatus;
using Relatus.Graphics;
namespace KeepStalling {
    class Game : Engine {
        protected override void Initialize() {
            base.Initialize();
            WindowManager.SetTitle("Keep Stalling");

            AssetManager.LoadImage("sheet", "Assets/Sprites/Sprites");
            SpriteManager.RegisterSpriteData("player", 0, 0, 96, 128, "sheet");

            SceneManager.RegisterScene(new Test("TestScene"));
            SceneManager.QueueScene("TestScene");
        }
    }
}