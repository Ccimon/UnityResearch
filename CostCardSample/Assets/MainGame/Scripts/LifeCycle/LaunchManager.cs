using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Basic;
using QFramework;
using UnityEngine;

namespace MainGame.Scripts
{
    public class LaunchManager : MonoBehaviour,GameSingletion<LaunchManager>
    {
        private TimeModel _timeModel;
        private void Awake()
        {
            UIRoot.Instance.OnSingletonInit();
            _timeModel = new TimeModel();
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

