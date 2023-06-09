using MainGame.Events;
using QFramework;
using QFramework.Scripts;
using UnityEngine;

namespace MainGame.Scripts
{
    public class GameManager : GameSingleton<GameManager>
    {

        private IModel _curGameModel;
        #region 公有方法

        public void Init()
        {
            TypeEventSystem.Global.Register<Game_Event_Start>(StartGame);
            
            _curGameModel.Init();
            _curGameModel = GameModel.Instance;
        }

        #endregion

        #region 私有方法

        private void StartGame(Game_Event_Start gevent)
        {
            
        }

        #endregion
    }
}