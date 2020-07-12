using Relatus;

namespace KeepStalling {
    class Game : Engine {
        protected override void Initialize() {
            base.Initialize();

            WindowManager.SetTitle("Keep Stalling");
            WindowManager.SetGameResolution(384, 216);

            AssetManager.LoadImage("sheet", "Assets/Sprites/Sprites");
            SpriteManager.RegisterSpriteData("player", 386, 0, 96, 128, "sheet");
            SpriteManager.RegisterSpriteData("background", 0, 0, 384, 216, "sheet");
            SpriteManager.RegisterSpriteData("table", 0, 216, 139, 80, "sheet");

            AssetManager.LoadSoundEffect("fart_0", "Assets/Sound Effects/fart_0");
            AssetManager.LoadSoundEffect("fart_1", "Assets/Sound Effects/fart_1");
            AssetManager.LoadSoundEffect("fart_2", "Assets/Sound Effects/fart_5");
            AssetManager.LoadSoundEffect("step", "Assets/Sound Effects/walk");

            SceneManager.RegisterScene(new Test("TestScene"));
            SceneManager.QueueScene("TestScene");
        }
    }
}