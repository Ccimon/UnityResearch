using MainGame.Events;
using QFramework;
using UnityEngine;

namespace MainGame.Scripts
{
    public class GameModel : AbstractModel
    {
        protected override void OnInit()
        {
            
        }


        #region 私有方法

        private void InitController()
        {
            TypeEventSystem.Global.Register<Game_Event_Start_Complete>(InitGameBoard);
        }

        private void InitGameBoard(Game_Event_Start_Complete g)
        {
            
        }

        #endregion
    }
}