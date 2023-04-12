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
        protected override void Init()
        {
            ResKit.Init();

            _gridModel = new GridModel();
            
            RegisterModel(_gridModel);

            UIKit.OpenPanel<PanelHome>();

            CanvasSize = new Vector2();

            var rect = UIRoot.Instance.GetRectTransform().rect;
            CanvasSize = new Vector2(rect.width, rect.height);
            
            Debug.Log("MainArchitecture Start Complete");
        }
    }
}
