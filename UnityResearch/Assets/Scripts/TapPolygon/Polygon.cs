using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Polygon
{
    
    public static List<Vector3Int> LineRect = new List<Vector3Int>()
    {
        new Vector3Int(0,0,0),
        new Vector3Int(0,-1,0)
    };

    public static List<Vector3Int> CubeRect = new List<Vector3Int>()
    {
        new Vector3Int(0,0,0),
        new Vector3Int(1,0,0),
        new Vector3Int(1,0,1),
        new Vector3Int(0,0,1)
    };
    public static List<Vector3Int> SitRect = new List<Vector3Int>()
    {
        new Vector3Int(0,0,0),
        new Vector3Int(1,0,0),
        new Vector3Int(0,1,0),
        new Vector3Int(0,0,1)
    };

    private static List<List<Vector3Int>> _polygonList = null;

    public static List<List<Vector3Int>> PolygonList
    {
        get
        {
            if (_polygonList == null)
            {
                _polygonList = new List<List<Vector3Int>>();
                _polygonList.Add(CubeRect);
                _polygonList.Add(LineRect);
                _polygonList.Add(SitRect);
                return _polygonList;
            }
            else
            {
                return _polygonList;
            }
        }
    }
}
