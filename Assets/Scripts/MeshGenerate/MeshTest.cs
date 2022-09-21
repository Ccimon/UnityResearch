using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

public class MeshTest : MonoBehaviour
{
    static Color color = new Color(255, 0, 0, 255);

    public bool GizmosDrawable = true;
    public Texture2D MeshTexture;
    public float threshold;
    
    private Color[] _colors;
    private Vector2[] _uvs;
    private Vector3[] _vertices;
    private int[] _triangles;

    void Start()
    {

        GameObject gameObject = new GameObject();
        gameObject.transform.SetParent(transform, false);
        var mf = gameObject.AddComponent<MeshFilter>();
        var mr = gameObject.AddComponent<MeshRenderer>();
        StartCoroutine(GenerateMesh());
        var shader = Shader.Find("shader3d/borderline");
        Material mat = new Material(shader);
        
        mf.mesh.Clear();
        Mesh mesh = new Mesh();
        mesh.vertices = _vertices;
        mesh.colors = _colors;
        mesh.uv = _uvs;
        mesh.triangles = _triangles;
        mesh.RecalculateNormals();
        
        mf.mesh = CreateArcSurface(threshold);
        mr.material = mat;
    }

    IEnumerator GenerateMesh()
    {
        int xSize = 10;
        int ySize = 5;
        _vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        _colors = new Color[_vertices.Length];
        // var wait = new WaitForSeconds(0.05f);
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++,i++)
            {
                Color color = MeshTexture.GetPixel(x, y);
                _vertices[i] = new Vector3(x, y,color.a * 20f);
                _colors[i] = color;
                // yield return wait;
            }
        }

        _triangles = new int[xSize * ySize * 6];
        for (int y = 0,vi = 0,ti = 0; y < ySize; y++,vi++)
        {
            for (int x = 0; x < xSize; ti += 6,x++,vi++)
            {
                _triangles[ti] = vi * 1;
                _triangles[ti + 3] = _triangles[ti + 2] = (vi + 1) * 1;
                _triangles[ti + 4] = _triangles[ti + 1] = (vi + xSize + 1) * 1;
                _triangles[ti + 5] = (vi + xSize + 2) * 1;
                // yield return wait;
            }
        }

        yield break;
    }

    private Mesh CreateArcSurface(float threshold)
    {
        float planeWidth = 1;
        float planeHeight = 1;
        int planesSeg = 64;

        Mesh mesh = new Mesh();

        int segments_count = planesSeg;
        int vertex_count = (segments_count + 1) * 2;

        Vector3[] vertices = new Vector3[vertex_count];

        int vi = 0;

        // 普通平面步
        float widthSetup = planeWidth * 1.0f / segments_count;

        // 半径
        float r = planeWidth * 1.0f / (Mathf.Sin(threshold / 2) * 2);

        // 弧度步
        float angleSetup = threshold / planesSeg;

        // 余角
        float coangle = (Mathf.PI - threshold) / 2;

        // 弓形的高度
        // https://zh.wikipedia.org/wiki/%E5%BC%93%E5%BD%A2
        float h = r - (r * Mathf.Cos(threshold / 2));

        // 弓形高度差值（半径-高度）
        float diff = r - h;

        for (int si = 0; si <= segments_count; si++)
        {
            float x = 0;

            float z = 0;

            if (threshold == 0)
            {
                // 阈值为0时,按照普通平面设置顶点
                x = widthSetup * si;

                vertices[vi++] = new Vector3(-planeWidth / 2 + x, planeHeight / 2, z);

                vertices[vi++] = new Vector3(-planeWidth / 2 + x, -planeHeight / 2, z);
            }
            else
            {
                // 阈值不为0时,根据圆的几何性质计算弧上一点
                // https://zh.wikipedia.org/wiki/%E5%9C%86
                x = r * Mathf.Cos(coangle + angleSetup * si);
                z = r * Mathf.Sin(coangle + angleSetup * si);

                vertices[vi++] = new Vector3(-x, planeHeight / 2, z - diff);
                vertices[vi++] = new Vector3(-x, -planeHeight / 2, z - diff);
            }
        }

        int indices_count = segments_count * 3 * 2;
        int[] indices = new int[indices_count];

        int vert = 0;
        int idx = 0;
        for (int si = 0; si < segments_count; si++)
        {
            indices[idx++] = vert + 1;
            indices[idx++] = vert;
            indices[idx++] = vert + 3;

            indices[idx++] = vert;
            indices[idx++] = vert + 2;
            indices[idx++] = vert + 3;

            vert += 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = indices;

        // // https://answers.unity.com/questions/154324/how-do-uvs-work.html
        Vector2[] uv = new Vector2[vertices.Length];

        float uvSetup = 1.0f / segments_count;

        int iduv = 0;
        for (int i = 0; i < uv.Length; i = i + 2)
        {
            uv[i] = new Vector2(uvSetup * iduv, 1);
            uv[i + 1] = new Vector2(uvSetup * iduv, 0);
            iduv++;
        }

        mesh.uv = uv;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        return mesh;
    }
// ————————————————
// 版权声明：本文为CSDN博主「别志华」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
// 原文链接：https://blog.csdn.net/biezhihua/article/details/78789794
    
    void OnDrawGizmos()
    {
        if (_vertices == null || !GizmosDrawable)
        {
            return;
        }
        
        Gizmos.color = Color.black;
        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i] * 100,20);
        }

        if (_triangles == null)
        {
            return;
        }

        float scale = 100f;
        Gizmos.color = Color.red;;
        for (int i = 0; i < _triangles.Length; i += 6)
        {
            Gizmos.DrawLine(_vertices[_triangles[i]].Multi(scale),_vertices[_triangles[i+1]].Multi(scale));
            Gizmos.DrawLine(_vertices[_triangles[i+1]].Multi(scale),_vertices[_triangles[i+2]].Multi(scale));
            Gizmos.DrawLine(_vertices[_triangles[i+2]].Multi(scale),_vertices[_triangles[i]].Multi(scale));
            
            Gizmos.DrawLine(_vertices[_triangles[i+3]].Multi(scale),_vertices[_triangles[i+4]].Multi(scale));
            Gizmos.DrawLine(_vertices[_triangles[i+4]].Multi(scale),_vertices[_triangles[i+5]].Multi(scale));
            Gizmos.DrawLine(_vertices[_triangles[i+5]].Multi(scale),_vertices[_triangles[i+3]].Multi(scale));
        }
    }
}