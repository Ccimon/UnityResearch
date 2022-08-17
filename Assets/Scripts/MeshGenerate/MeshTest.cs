using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

public class MeshTest : MonoBehaviour
{
    static Color color = new Color(255, 0, 0, 255);

    public Texture2D MeshTexture;
    private Vector3[] vertices;
    Color[] colors = { color, color, color, color };
    Vector2[] uvs = { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    int[] triangles;

    void Start()
    {
        StartCoroutine(GenerateMesh());
        GameObject gameObject = new GameObject("Quad");
        gameObject.transform.SetParent(transform, false);
        var mf = gameObject.AddComponent<MeshFilter>();
        var mr = gameObject.AddComponent<MeshRenderer>();
        
        var shader = Shader.Find("Custom/NewSurfaceShader");
        Material mat = new Material(shader);
        
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        // mesh.colors = colors;
        // mesh.uv = uvs;
        mesh.triangles = triangles;
        // mf.mesh = mesh;
        mesh.RecalculateNormals();
        
        mr.material = mat;
    }

    IEnumerator GenerateMesh()
    {
        int xSize = 10;
        int ySize = 5;
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        
        var wait = new WaitForSeconds(0.05f);
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++,i++)
            {
                // Color color = MeshTexture.GetPixel(x, y);
                vertices[i] = new Vector3(x, y);
                // yield return wait;
            }
        }

        triangles = new int[xSize * ySize * 6];
        for (int ti = 0,vi = 0,x = 0; x < xSize; ti+=6,x++,vi++)
        {
            triangles[ti] = vi;
            triangles[ti + 3] = triangles[ti + 2] = vi + 1;
            triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
            triangles[ti + 5] = vi + xSize + 2;
            yield return wait;
        }
    }

    void OnDrawGizmos()
    {
        // if (vertices == null)
        // {
        //     return;
        // }
        
        // Gizmos.color = Color.black;
        // for (int i = 0; i < vertices.Length; i++)
        // {
        //     Gizmos.DrawSphere(vertices[i] * 100,20);
        // }

        // if (triangles == null)
        // {
        //     return;
        // }
        //
        // Gizmos.color = Color.red;;
        // for (int i = 0; i < triangles.Length; i += 6)
        // {
        //     Gizmos.DrawLine(vertices[triangles[i]],vertices[triangles[i+1]]);
        //     Gizmos.DrawLine(vertices[triangles[i+1]],vertices[triangles[i+2]]);
        //     Gizmos.DrawLine(vertices[triangles[i+2]],vertices[triangles[i]]);
        //     Gizmos.DrawLine(vertices[triangles[i+3]],vertices[triangles[i+4]]);
        //     Gizmos.DrawLine(vertices[triangles[i+4]],vertices[triangles[i+5]]);
        //     Gizmos.DrawLine(vertices[triangles[i+5]],vertices[triangles[i+3]]);
        // }
    }
}