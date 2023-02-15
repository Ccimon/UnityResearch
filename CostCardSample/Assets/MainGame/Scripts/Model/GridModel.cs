using System.Collections;
using System.Collections.Generic;
using MainGame.Events;
using QFramework;
using UnityEngine;

namespace  MainGame.Scripts
{
    public enum BlockType
    {
        Space,
        Grass,
        Water,
        Moutain,
        
        Tower = 100
    }
    
    public class Block
    {
        public BlockType TypeInfo = BlockType.Space;
        public int Owner;
    }
    
    public class GridModel : AbstractModel
    {
        public Block[][] BlockBoard;
        public Vector2Int BlockSize = Vector2Int.one;
        public int RandomCount = 5;
        
        private bool _isFull = false;
        protected override void OnInit()
        {
            Debug.Log("GridModel Init");
            BlockBoard = new Block[BlockSize.x][];
        }
        
        public void InitBoard()
        {
            if(_isFull) return;
            for (int i = 0; i < BlockSize.x; i++)
            {
                BlockBoard[i] = new Block[BlockSize.y];
                for (int j = 0; j < BlockSize.y; j++)
                {
                    Block block = new Block();
                    block.TypeInfo = BlockType.Space;
                    BlockBoard[i][j] = block;
                }                
            }

            _isFull = true;
        }

        public void RandomBoardInfo()
        {
            List<int> blockList = new List<int>() { 0,1,2,3 };

            var list = blockList.GetRandomElementsIndex(RandomCount);
            for (int i = 0; i < list.Count; i++)
            {
                int index = list[i];
                int row = index / BlockSize.x;
                int line = index % BlockSize.x;
                Block block = BlockBoard[row][line];
                block.TypeInfo = (BlockType)blockList.GetRandomElement();

            }
        }

        public void GridBoardInit()
        {
            this.SendEvent<Game_Event_Board_Init>();
        }
    }
}
