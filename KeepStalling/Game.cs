using Relatus;
namespace KeepStalling {
    class Game : Engine {
        protected override void Initialize() {
            base.Initialize();
            WindowManager.SetTitle("Keep Stalling");
            SceneManager.RegisterScene(new Test("TestScene"));
            SceneManager.QueueScene("TestScene");
        }
    }
}