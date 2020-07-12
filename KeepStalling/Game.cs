using Relatus;

namespace KeepStalling
{
    class Game : Engine
    {
        protected override void Initialize()
        {
            base.Initialize();

            WindowManager.SetTitle("Keep Stalling");
            WindowManager.SetGameResolution(384, 216);

            AssetManager.LoadImage("sheet", "Assets/Sprites/Sprites");
            SpriteManager.RegisterSpriteData("chair", 1, 256 - 134, 72, 104, "sheet");
            SpriteManager.RegisterSpriteData("coworker", 125, 256 - 255, 60, 113, "sheet");
            SpriteManager.RegisterSpriteData("coworker2", 1, 256 - 255, 50, 119, "sheet");
            SpriteManager.RegisterSpriteData("table", 75, 256 - 134, 139, 80, "sheet");
            SpriteManager.RegisterSpriteData("player", 53, 256 - 255, 70, 113, "sheet");

            AssetManager.LoadSoundEffect("fart_0", "Assets/Sound Effects/fart_0");
            AssetManager.LoadSoundEffect("fart_1", "Assets/Sound Effects/fart_1");
            AssetManager.LoadSoundEffect("fart_2", "Assets/Sound Effects/fart_5");
            AssetManager.LoadSoundEffect("step", "Assets/Sound Effects/walk");

            SceneManager.RegisterScene(new Test("TestScene"));
            SceneManager.QueueScene("TestScene");
        }
    }
}