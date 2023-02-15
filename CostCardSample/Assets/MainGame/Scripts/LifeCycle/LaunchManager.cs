using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace MainGame.Scripts
{
    public class LaunchManager : MonoBehaviour,GameSingleton<LaunchManager>
    {
        private TimeModel _timeModel;
        private void Awake()
        {
            UIRoot.Instance.OnSingletonInit();
             var game = MainArchitecture.Interface;
        }
    }

    public class TimeModel : AbstractModel
    {
        public DateTime LoginTime;
        protected override void OnInit()
        {
            LoginTime = DateTime.Now;
        }
    }
}

