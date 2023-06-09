using System;
using System.Collections.Generic;

namespace MainGame.Scripts
{
    public class CardGroup<T>:IGroup
    {
        public List<T> _list;

        public CardGroup()
        {
            _list = new List<T>();
        }


        public void Disturb()
        {
        }

        public void GetCardBtType()
        {
        }
    }
}