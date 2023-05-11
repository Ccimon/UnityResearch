using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResetVertexUV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        Mesh mesh = filter.mesh;
        Debug.Log(mesh.vertices.Length);
        mesh.uv = ResetUV(mesh);
        // GenerateTriangles(mesh);

        var uv = Conbine(mesh.uv,Revert(mesh.uv));
        Debug.Log("UV Generate");

        mesh.vertices = Conbine(mesh.vertices, mesh.vertices);
        Debug.Log("Vertices Generate");
        mesh.uv = uv;
        
        Debug.Log(mesh.vertices.Length);
        Debug.Log(mesh.uv.Length);
        Debug.Log(mesh.triangles.Length);
    }

    private Vector2[] ResetUV(Mesh mesh)
    {
        var uv = mesh.uv;
        float count = uv.Length;
        for (int i = 0; i < count; i++)
        {
            var vec = uv[i];
            float y = 0;
            if (i < count - 1)
            {
                y = 1.0f;
            }
            uv[i] = new Vector2(i/count,y);
        }

        return uv;
    }

    private void GenerateTriangles(Mesh mesh)
    {
        int count = mesh.triangles.Length;
        var triangle = new int[count];
        
        for (int i = 0; i < count / 3; i++)
        {
            int index1 = 1 + i;
            int index2 = (2 + i) % count;
            int index3 = (3 + i) % count;
            

            int start = i > 0 ? i * 3 - 1 : 0;
            triangle[start] = index1;
            triangle[start + 1] = index2;
            triangle[start + 2] = index3;
        }

        mesh.triangles = Conbine(mesh.triangles, triangle);
    }
    
    private T[] Revert<T>(T[] vertices)
    {
        int count = vertices.Length;
        T[] result = new T[count];
        // Debug.Log($"Revert,ArrayCount:{count}");
        count -= 1;
        for (int i = 0; i <= count; i ++)
        {
            var value = vertices[count - i];
            Debug.Log($"Revert,Index:{i},Value:{value}");
            result[i] = value;
        }

        return result;
    }

    private T[] Conbine<T>(T[] left,T[] right)
    {
        int count = left.Length + right.Length;
        T[] result = new T[count];
        for (int i = 0; i < count;i ++)
        {
            bool which = i < left.Length;
            int index = which ? i : i - left.Length;
            T[] lst = which ? left : right;
            result[i] = lst[index];
            Debug.Log(result[i]);
        }

        return result;
    }
}
