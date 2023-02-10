using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace  MainGame.Scripts
{
    public enum BlockType
    {
        Space,
        Tower,
    }
    
    public class Block
    {
        public BlockType TypeInfo = BlockType.Space;
        public int Owner;
    }
    
    public class GridModel : AbstractModel
    {
        public Block[,] BlockBoard;
        public Vector2Int BlockSize;
        
        protected override void OnInit()
        {
            Debug.Log("GridModel Init");
            BlockBoard = new Block[BlockSize.x,BlockSize.y];
        }
        
        private void RandomBoard()
        {
            for (int i = 0; i < BlockSize.x; i++)
            {
                for (int j = 0; j < BlockSize.y; j++)
                {
                    Block block = new Block();
                    block.TypeInfo = BlockType.Space;
                    BlockBoard[i, j] = new Block();
                    
                }                
            }
        }
    }
}
