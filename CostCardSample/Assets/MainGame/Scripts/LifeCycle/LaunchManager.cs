using System;
using System.Collections;
using System.Collections.Generic;
using QAssetBundle;
using QFramework;
using QFramework.Scripts;
using UnityEngine;

namespace MainGame.Scripts
{
    public class LaunchManager : MonoBehaviour
    {
        private LaunchManager()
        {
            
        }
        private void Awake()
        {
            ResKit.Init();
            
            UIRoot.Instance.OnSingletonInit();

            UIKit.OpenPanel<PanelHome>();
            
            GameModel.Instance.Init();
        }
    }
}
