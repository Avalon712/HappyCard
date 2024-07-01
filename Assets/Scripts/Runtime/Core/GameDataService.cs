using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HappyCard
{
    /// <summary>
    /// 加载本地存档数据
    /// </summary>
    public static class GameDataService
    {
        public static string UserFilePath => $"{Application.dataPath}/Game Data/user.json";

        public static string PlayerFilePath => $"{Application.dataPath}/Game Data/player.json";

        public static string GameFilesFilePath => $"{Application.dataPath}/Game Data/games.json";


        public static void InitGameData()
        {
            GameDataContainer container = GameDataContainer.Instance;
            container.Self = GetLocalPlayerData();
            container.GameSetting = new GameSetting();

        }


        public static bool HadRegistered()
        {
            return File.Exists(UserFilePath);
        }


        public static Player RegisterNewUser(User user)
        {
            user.remember = true;
            SaveUser(user);
            Player player = GetDefaultPlayer(user.name);
            SavePlayer(player);
            return player;
        }


        public static User GetLocalUserData()
        {
            User user = null;
            string filePath = UserFilePath;
            try
            {
                if (File.Exists(filePath))
                    user = JsonConvert.DeserializeObject<User>(File.ReadAllText(filePath));
            }
            catch(Exception e)
            {
                LogHelper.Error($"本地User信息获取失败，原因：{e.Message}");
                user = null;
            }
            return user;
        }


        public static Player GetLocalPlayerData()
        {
            Player player = null;
            string filePath = PlayerFilePath;
            try
            {
                if (File.Exists(filePath))
                    player = JsonConvert.DeserializeObject<Player>(File.ReadAllText(filePath));
            }
            catch(Exception e)
            {
                LogHelper.Error($"本地Player信息获取失败，原因：{e.Message}");
                player = null;
            }
            return player;
        }

        public static List<GameFile> GetLocalGameFilesData()
        {
            List<GameFile> files = null;
            string filePath = GameFilesFilePath;
            try
            {
                if (File.Exists(filePath))
                    files = JsonConvert.DeserializeObject<List<GameFile>>(File.ReadAllText(filePath));
            }
            catch(Exception e)
            {
                LogHelper.Error($"本地游戏存档List<GameFile>信息获取失败，原因：{e.Message}");
                files = null;
            }
            return files;
        }


        public static void SavePlayer(Player player)
        {
            Player copy = CopyPlayer(player);
            Task.Run(() =>
            {
                try
                {
                    string json = JsonConvert.SerializeObject(copy);
                    File.WriteAllText(PlayerFilePath, json);
                }
                catch (Exception e)
                {
                    LogHelper.Error($"Player信息保存失败，原因：{e.Message}");
                }
            });
        }

        public static void SaveUser(User user)
        {
            Task.Run(() =>
            {
                try
                {
                    string json = JsonConvert.SerializeObject(user);
                    File.WriteAllText(UserFilePath, json);
                }
                catch (Exception e)
                {
                    LogHelper.Error($"User信息保存失败，原因：{e.Message}");
                }
            });
        }


        public static void SaveGameFile(List<GameFile> files)
        {
            Task.Run(() =>
            {
                try
                {
                    string json = JsonConvert.SerializeObject(files);
                    File.WriteAllText(GameFilesFilePath, json);
                }
                catch(Exception e)
                {
                    LogHelper.Error($"游戏存档保存失败，原因：{e.Message}");
                }
            });
        }

        /// <summary>
        /// 将本地存档同步到远端服务器
        /// </summary>
        public static void PushArchiveToRemoteServer()
        {

        }

        /// <summary>
        /// 将远端服务器上的存档拉取到本地
        /// </summary>
        public static void PullArchiveFromRemoteServer()
        {

        }


        public static Player CopyPlayer(Player player)
        {
            return new Player()
            {
                ID = player.ID,
                Name = player.Name,
                Level = player.Level,
                Diamond = player.Diamond,
                Coin = player.Coin,
                HP = player.HP,
                Exp = player.Exp,
                Likes = player.Likes,
                HeadIconID = player.HeadIconID,
            };
        }


        public static Player GetDefaultPlayer(string name)
        {
            Random.InitState((int)DateTime.Now.Ticks);

            return new Player()
            {
                ID = Random.Range(Random.Range(1, 100_000_000), Random.Range(100_000_000, int.MaxValue)),
                Name = name,
                Level = 1,
                HeadIconID = "default" + Random.Range(0, 23),
                Coin = 1000,
                Diamond = 50,
                HP = 20,
                Exp = 0,
            };
        }
    }
}
