using System;
using System.Collections;
using UnityEngine;

namespace MainGame.Scripts
{
    public class TaskQueue : MonoBehaviour
    {
        private TaskQueue(){}

        private static TaskQueue _instance;
        
        public static TaskQueue Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TaskQueue();
                }

                return _instance;
            }
        }

        public void Delay(float time,System.Action call)
        {
            StartCoroutine(DoDelay(time,call));
        }

        private IEnumerator DoDelay(float time,System.Action call)
        {
            var wait = new WaitForSeconds(time);
            yield return wait;
            call.Invoke();
        }
    }
}