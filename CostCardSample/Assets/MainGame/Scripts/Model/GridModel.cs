using System.Collections;
using System.Collections.Generic;
using MainGame.Events;
using QFramework;
using QFramework.Scripts;
using UnityEngine;

namespace  MainGame.Scripts
{
    public enum BlockType
    {
        Land,
        Forest,
        Grass,
        Water,
        Moutain,
        
        Tower = 100
    }

    public enum ElementType
    {
        Metal,
        Wood,
        Water,
        Fire,
        Earth
    }
    
    public class Block
    {
        public BlockType TypeInfo = BlockType.Land;
    }
    
    public class GridModel : AbstractModel
    {
        public Block[][] BlockBoard;
        public Vector2Int BlockSize = Vector2Int.one * 10;
        public int RandomCount = 5;
        public int BlockInter = 5;
        
        private ResLoader _loader = ResLoader.Allocate();
        private bool _isFull = false;
        protected override void OnInit()
        {
            Debug.Log("GridModel Init");
            BlockBoard = new Block[BlockSize.x][];
            InitController();
        }

        #region 公有方法

        

        #endregion

        #region 私有方法
        private void StartGame(Game_Event_Start g)
        {
            InitBoard();
            RandomBoardInfo();
            GameBoardInit();
        }
        
        private void InitController()
        {
            TypeEventSystem.Global.Register<Game_Event_Start>(StartGame);
        }
        
        private void InitBoard()
        {
            if(_isFull) return;
            for (int i = 0; i < BlockSize.x; i++)
            {
                BlockBoard[i] = new Block[BlockSize.y];
                for (int j = 0; j < BlockSize.y; j++)
                {
                    Block block = new Block();
                    block.TypeInfo = BlockType.Land;
                    BlockBoard[i][j] = block;
                }
            }

            _isFull = true;
        }

        private void RandomBoardInfo()
        {
            List<int> blockList = new List<int>() { 0,1,2,3 };

            var list = blockList.GetRandomElementsIndex(RandomCount);
            for (int i = 0; i < list.Count; i++)
            {
                int index = list[i];
                int row = index / BlockSize.x;
                int line = index % BlockSize.x;
                Block block = BlockBoard[i][i];
                block.TypeInfo = (BlockType)blockList.GetRandomElement();
            }
        }

        private void GameBoardInit()
        {
            Debug.Log("GameBoard Init Success");
            GameObject obj = _loader.LoadSync<GameObject>(QAssetBundle.Block_prefab.BLOCK);
			
            Vector2 pos = Vector2.down;
            RectTransform objRTS = obj.GetRectTransform();
            Vector2 boardOffset = new Vector2((objRTS.rect.width + BlockInter) * (BlockSize.x - 1) / 2,
                (objRTS.rect.width + BlockInter) * (BlockSize.y - 1) / 2);
            Transform GameContent = UIKit.GetPanel<PanelHome>().PanelContent;
            for (int x = 0; x < BlockSize.x; x++)
            {
                for (int y = 0; y < BlockSize.y; y++)
                {
                    Block blockData = BlockBoard[x][y];
                    GameObject block = Object.Instantiate(obj,GameContent);
                    RectTransform rts = block.GetRectTransform();
                    pos = new Vector2((rts.rect.width + BlockInter)* x, (rts.rect.width + BlockInter) * y);
                    rts.anchoredPosition = pos - boardOffset;
                }
            }
        }
        
        #endregion

    }
}
