using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Scripts;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MainGame.Scripts{
    public abstract class IBlock:ViewController,IPointerClickHandler
    {
        protected Vector2 _location;
        protected BlockType _blockType;
        
        
        protected virtual void OnGetHurt(){}

        protected virtual void OnBreak(){}
        
        protected virtual void OnClick(){}
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }

        private void ClickAnim()
        {
        }
    }
}

