using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PolygonPlayTime
{
    public enum CubeForward
    {
        Forward,
        Back,
        Up,
        Down,
        Left,
        Right
    }
    
    public class TapPolyGon : MonoBehaviour
    {
        #region 公共变量
    
        public Vector3Int CapacityVector = Vector3Int.one;
    
        public Transform CubeCenter;

        public CubeBind[,,] CubeArrray;

        public int[,,] CubeDataArray;

        public GameObject CubePrefab;
        #endregion

        #region 私有变量

        private Color[] ColorList = new[]
        {
            Color.black, Color.blue, Color.cyan, Color.gray, Color.green,
            Color.magenta, Color.red, Color.yellow
        };

        private Vector3[] _cubeForwardsArray = new[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.up,
            Vector3.down, 
            Vector3.left, 
            Vector3.right, 
        };
        
        private Camera _camera;

        private int SpawnCount;
        
        private WaitForEndOfFrame _wait;

        private WaitUntil _waitUntil;
        #endregion
    
        private void Start()
        {
            _camera = GetComponent<Camera>();

            _wait = new WaitForEndOfFrame();
            SpawnCount = CapacityVector.x * CapacityVector.y * CapacityVector.z;
            
            InitCubeData();
            
            StartCoroutine(SpawnCube());

            _waitUntil = new WaitUntil(() => SpawnCount <= 0);
            
            StartCoroutine(CheckSpawnSpecialCube());
        }

        private void InitCubeData()
        {
            CubeDataArray = new int[CapacityVector.x,CapacityVector.y,CapacityVector.z];
            CubeArrray = new CubeBind[CapacityVector.x,CapacityVector.y,CapacityVector.z];
            for (int x = 0;x < CapacityVector.x; x++)
            {
                for (int y = 0;y < CapacityVector.y; y++)
                {
                    for (int z = 0;z < CapacityVector.z; z++)
                    {
                        CubeDataArray[x, y, z] = 0;
                    }
                }
            }
        }
        
        private IEnumerator SpawnCube()
        {
            for (int x = 0;x < CapacityVector.x; x++)
            {
                for (int y = 0;y < CapacityVector.y; y++)
                {
                    for (int z = 0;z < CapacityVector.z; z++)
                    {
                        var cube = InitCube(new Vector3(x, y, z));
                        var bind = new CubeBind();
                        bind.childBind.Add(cube);
                        CubeArrray[x, y, z] = bind;
                        SpawnCount--;
                        yield return _wait;
                    }
                }
            }
        }

        private IEnumerator CheckSpawnSpecialCube()
        {
            yield return _waitUntil;
            
            int colorIndex = 0;
            for (int x = 0;x < CapacityVector.x; x++)
            {
                for (int y = 0;y < CapacityVector.y; y++)
                {
                    for (int z = 0;z < CapacityVector.z; z++)
                    {
                        var item = Polygon.PolygonList.GetRandomItem();
                        var pos = new Vector3Int(x, y, z);
                        // Debug.Log(pos.ToString());
                        bool canSpawn = CheckSpawnPosition(CubeDataArray, pos, item);

                        if (canSpawn)
                        {
                            // Debug.Log("生成！");
                            ChangeCubeData(CubeDataArray, new Vector3Int(x, y, z), item, colorIndex);
                            colorIndex++;
                        }
                        else
                        {
                            // Debug.Log("生成失败！");
                        }
                        
                        yield return _wait;
                    }
                }
            }
        }

        private bool CheckSpawnPosition(int[,,] datas,Vector3Int pos,List<Vector3Int> rect)
        {
            for (int i = 0;i < rect.Count;i++)
            {
                Vector3Int info = rect[i];
                int x = pos.x+info.x;
                int y = pos.y+info.y;
                int z = pos.z+info.z;
                
                if (x > CapacityVector.x - 1 || x < 0 ||
                    y > CapacityVector.y - 1 || y < 0 ||
                    z > CapacityVector.z - 1 || z < 0)
                {
                    return false;
                }
                
                int cubeData = datas[x, y, z];
                if (cubeData > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void ChangeCubeData(int[,,] datas,Vector3Int pos,List<Vector3Int> rect,int colorIndex)
        {
            CubeBind bind = new CubeBind();
            for (int i = 0;i < rect.Count;i++)
            {
                Vector3Int info = rect[i];
                int x = pos.x+info.x;
                int y = pos.y+info.y;
                int z = pos.z+info.z;
                                
                int cubeData = datas[x, y, z];
                datas[x, y, z] = cubeData + 1;
                CubeBind cube = CubeArrray[x, y, z];
                cube.SetColor(ColorList[colorIndex % ColorList.Length]);

                bind.childBind.AddRange(cube.childBind);
                CubeArrray[x, y, z] = bind;
            }
        }
        
        private Transform InitCube(Vector3 pos)
        {
            GameObject cube = Instantiate(CubePrefab,CubeCenter.transform);
            cube.transform.localPosition = pos;
            return cube.transform;
        }
    }

    public class CubeBind
    {
        public List<Transform> childBind = new List<Transform>();
        public CubeForward Forward = CubeForward.Back;

        public void SetColor(Color color)
        {
            for (int i = 0; i < childBind.Count; i++)
            {
                childBind[i].SetRenderColor(color);
            }
        }
    }
    
    public static class MonoExtension
    {
        public static void SetRenderColor(this Transform self,Color color)
        {
            var render = self.GetComponent<MeshRenderer>();
            if (render == null)
                return;
            
            render.material.SetColor("_Color",color);
        }

        public static T GetRandomItem<T>(this List<T> list)
        {
            T item = list[Random.Range(0,list.Count)];
            return item;
        }
    }
        
}