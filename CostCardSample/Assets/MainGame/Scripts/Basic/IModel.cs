using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MainGame.Scripts
{
    public interface IModel
    {

        public void Init();

        public void Start();

        public void Quit();

        public void Recycle();
    }
}