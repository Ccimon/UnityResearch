using System.Collections;
using System.Collections.Generic;
using QFramework;
using QFramework.Scripts;
using UnityEngine;

namespace MainGame.Scripts
{
    public class MainArchitecture : Architecture<MainArchitecture>
    {
        public Vector2 CanvasSize;
        private GridModel _gridModel;
        private TimeModel _timeModel;
        protected override void Init()
        {
            ResKit.Init();

            _gridModel = new GridModel();
            _timeModel = new TimeModel();
            
            RegisterModel(_gridModel);
            RegisterModel(_timeModel);

            UIKit.OpenPanel<PanelHome>();

            CanvasSize = new Vector2();

            var rect = UIRoot.Instance.GetRectTransform().rect;
            CanvasSize = new Vector2(rect.width, rect.height);
            
            _gridModel.InitBoard();
            _gridModel.RandomBoardInfo();
            _gridModel.GridBoardInit();
            Debug.Log("MainArchitecture Start Complete");
        }
        
    }
}
