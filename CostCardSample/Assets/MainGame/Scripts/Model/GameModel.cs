using MainGame.Events;
using QFramework;
using QFramework.Scripts;
using UnityEngine;

namespace MainGame.Scripts
{
    public class GameModel : GameSingleton<GameModel>,IModel
    {
        private Transform _content;

        private GameData _curGameData;
        #region 公有方法

        public void Init()
        {
            _content = UIKit.GetPanel<PanelHome>().GameContent;
            _curGameData = new GameData();
        }

        public void Start()
        {
            
        }

        public void Quit()
        {
        }

        public void Recycle()
        {
        }

        #endregion

        #region 私有方法

        private void InitBoard()
        {
            
        }
        
        #endregion
    }
}