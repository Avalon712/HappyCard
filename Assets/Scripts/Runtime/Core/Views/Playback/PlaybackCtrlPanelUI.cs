using System.Collections.Generic;
using UniVue;
using UniVue.Evt;
using UniVue.Evt.Attr;
using UniVue.Model;
using UniVue.Tween;
using UniVue.Tween.Tweens;
using UniVue.Utils;

namespace HappyCard
{
    /// <summary>
    /// 对局回放控制面板UI
    /// </summary>
    public sealed class PlaybackCtrlPanelUI : EventRegister
    {
        private ControlInfo _controlInfo;                        //当前控制信息
        private GameFile _file;                                 //游戏回放档案
        private int _currentBoutPtr;                            //当前演示的回合的指针
        private TweenTask _playBoutTask;                        //定时播放下一个回合
        private Dictionary<int, PlayerUI> _playerUIs;

        public PlaybackCtrlPanelUI(GameFile file)
        {
            _file = file;
            _playerUIs = new Dictionary<int, PlayerUI>(file.Sequence.Count);
            _controlInfo = new ControlInfo(GetBaseInfo());

            Vue.Router.GetView(nameof(GameUIs.Playback_ControlPanelView)).BindModel(_controlInfo);
            InitPlayback();
        }

        #region 初始化流程

        private void InitPlayback()
        {
            SortCards();
            BuildPlayerUIs();
            AddPlayerHandCards();
        }

        private void SortCards()
        {
            for (int i = 0; i < _file.OriginalCards.Length; i++)
                _file.OriginalCards[i].Sort((c1, c2) => c2 - c1);
        }

        private void AddPlayerHandCards()
        {
            foreach (var playerID in _playerUIs.Keys)
            {
                _playerUIs[playerID].AddHandCards(_file.GetOriginalCards(playerID));
            }
        }

        private string GetBaseInfo()
        {
            return _file.Date + "  " + ReflectionUtil.GetEnumAlias(typeof(Gameplay), _file.Gameplay.ToString());
        }

        #endregion


        #region 构建玩家的UI界面


        private void BuildPlayerUIs()
        {
            BuildSelfPlayerUI();

            switch (_file.Gameplay)
            {
                case Gameplay.FightLandlord:
                    BuildOtherPlayerUIs_FightLandlord();
                    break;
                case Gameplay.BanZiPao:
                    break;
                case Gameplay.FriedGoldenFlower:
                    break;
            }
        }


        private void BuildSelfPlayerUI()
        {
            int selfID = GameDataContainer.Instance.Self.ID;
            BuildSelfPlayerUICmd buildSelfCmd = new BuildSelfPlayerUICmd();
            buildSelfCmd.Execute();
            _playerUIs.Add(selfID, buildSelfCmd.PlayerUI);
        }


        private void BuildOtherPlayerUIs_FightLandlord()
        {
            int selfID = GameDataContainer.Instance.Self.ID;
            int leftID = _file.GetNext(selfID);
            int rightID = _file.GetNext(leftID);
            BuildFightLandlordPlayerUICmd buildCmd = new BuildFightLandlordPlayerUICmd();
            buildCmd.Execute();
            _playerUIs.Add(leftID, buildCmd.LeftPlayerUI);
            _playerUIs.Add(rightID, buildCmd.RightPlayerUI);
        }

        #endregion


        private void PlayNext()
        {
            if(_currentBoutPtr == _file.Bouts.Count)
            {
                _playBoutTask?.Kill();
                _playBoutTask = null;
                return;
            }

            Bout bout = _file.Bouts[_currentBoutPtr++];
            PlayerUI playerUI = _playerUIs[bout.PlayerID];
            playerUI.ShowBout(bout);

            if (bout.State == BoutState.OutCard)
            {
                playerUI.RemoveHandCards(bout.OutCards);
            }

            //Debug.Log($"PlayerID={bout.PlayerID} BoutState={bout.State} OutCards=[{(bout.State == BoutState.OutCard ? string.Join(", ", bout.OutCards) : "")}]");
        }

        private void PlayLast()
        {
            if (_currentBoutPtr <= 0 || _currentBoutPtr>=_file.Bouts.Count) return;

            Bout bout = _file.Bouts[--_currentBoutPtr];
            PlayerUI playerUI = _playerUIs[bout.PlayerID];

            if (bout.State == BoutState.OutCard)
                playerUI.AddHandCards(bout.OutCards);
            else
                playerUI.ShowBout(bout);
        }


        #region 回放演示逻辑控制命令


        [EventCall(nameof(PlaySpeed))]
        private void PlaySpeed(float speed)
        {
            if(_playBoutTask != null)
            {
                _playBoutTask.Kill();
                _playBoutTask = TweenBehavior.Timer(PlayNext).Interval(5f / speed).ExecuteNum(int.MaxValue);
            }
        }


        [EventCall(nameof(LastBout))]
        private void LastBout()
        {
            if (_playBoutTask == null || _playBoutTask.State == TweenState.Paused)
                PlayLast();
        }


        [EventCall(nameof(Play))]
        private void Play(bool isPlaying)
        {
            //只有勾选了自动播放才允许进行自动控制
            if (_controlInfo.AutoPlay)
            {
                if (isPlaying)
                {
                    if (_playBoutTask == null)
                    {
                        _playBoutTask = TweenBehavior.Timer(PlayNext).Interval(5f / _controlInfo.PlaySpeed).ExecuteNum(int.MaxValue);
                    }
                    _playBoutTask.Play();
                }
                else
                    _playBoutTask?.Pause();
            }
        }


        [EventCall(nameof(NextBout))]
        private void NextBout()
        {
            if (_playBoutTask == null || _playBoutTask.State == TweenState.Paused)
                PlayNext();
        }


        [EventCall(nameof(Restart))]
        private void Restart()
        {
            _currentBoutPtr = 0;
            foreach (var playerID in _playerUIs.Keys)
            {
                PlayerUI playerUI = _playerUIs[playerID];
                playerUI.AddHandCards(_file.GetOriginalCards(playerID));
                playerUI.Reset();
                playerUI.SortHandCards();
            }
            _controlInfo.IsPlaying = false;
        }


        [EventCall(nameof(Quit))]
        private void Quit()
        {

        }

        #endregion


    }

    public sealed partial class ControlInfo
    {
        [AutoNotify] private string _baseInfo;
        [AutoNotify] private bool _isPlaying;
        [AutoNotify] private bool _autoPlay = true;
        [AutoNotify] private int _playSpeed = 1;

        public ControlInfo(string baseInfo)
        {
            _baseInfo = baseInfo;
        }
    }
}
