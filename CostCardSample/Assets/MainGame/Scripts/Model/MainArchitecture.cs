using System.Collections;
using System.Collections.Generic;
using QFramework;
using QFramework.Scripts;
using UnityEngine;

namespace MainGame.Scripts
{
    public class MainArchitecture : Architecture<MainArchitecture>,ISingleton
    {
        public Vector2 CanvasSize;
        protected override void Init()
        {
            ResKit.Init();
            
            this.RegisterModel(new GridModel());
            this.RegisterModel(new TimeModel());

            UIKit.OpenPanel<PanelHome>();

            CanvasSize = new Vector2();

            var rect = UIRoot.Instance.GetRectTransform().rect;
            CanvasSize = new Vector2(rect.width, rect.height);
            Debug.Log("MainArchitecture Start Complete");
        }

        public void OnSingletonInit()
        {
            
        }
    }
}
