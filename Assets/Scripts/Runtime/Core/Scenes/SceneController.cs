using UnityEngine.SceneManagement;

namespace HappyCard
{
    public static class SceneController
    {
        private static IScene _current;        //当前正在运行的场景

        public static void Initialize()
        {
            _current = new LoginScene();
            _current.Initialize();
        }

        public static void LoadScene(GameScenes scene)
        { 
            //1. 卸载上个场景的资源
            _current.Dispose();

            //2. 加载场景
            SceneManager.LoadSceneAsync(scene.ToString()).completed += (op) =>
            {
                //3. 构建场景
                _current = BuildScene(scene);

                //4. 初始话当前场景
                _current.Initialize();
            };
        }

        public static void LoadGameScene(Gameplay gameplay)
        {
            switch (gameplay)
            {
                case Gameplay.FightLandlord:
                    LoadScene(GameScenes.Game_FightLandlord);
                    break;
                case Gameplay.BanZiPao:
                    LoadScene(GameScenes.Game_BanZiPao);
                    break;
                case Gameplay.FriedGoldenFlower:
                    LoadScene(GameScenes.Game_FriedGoldenFlower);
                    break;
            }
        }


        private static IScene BuildScene(GameScenes scene)
        {
            switch (scene)
            {
                case GameScenes.Login:
                    return new LoginScene();
                case GameScenes.Main:
                    return new MainScene();
                case GameScenes.Room:
                    return new RoomScene();
                case GameScenes.Game_FightLandlord:
                    return new FightLandlord_GameScene();
                case GameScenes.Game_BanZiPao:
                    break;
                case GameScenes.Game_FriedGoldenFlower:
                    break;
                case GameScenes.Playback:
                    break;
            }
            return null;
        }
    }
}
