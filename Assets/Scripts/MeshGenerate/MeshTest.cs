using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTest : MonoBehaviour
{
    static Color color = new Color(255, 0, 0, 255);

    Vector3[] vertices = new Vector3[4];
    Color[] colors = { color, color, color, color };
    Vector2[] uvs = { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    int[] tris = { 0, 1, 2, 0, 2, 3 };

    void Start()
    {
        GameObject gameObject = new GameObject("Quad");
        gameObject.transform.SetParent(transform, false);
        var mf = gameObject.AddComponent<MeshFilter>();
        var mr = gameObject.AddComponent<MeshRenderer>();

        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 1, 0);
        vertices[2] = new Vector3(0.7f, 0.7f    , 0);
        vertices[3] = new Vector3(1, 0, 0);

        var shader = Shader.Find("Custom/NewSurfaceShader");
        Material mat = new Material(shader);

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.uv = uvs;
        mesh.triangles = tris;
        mf.mesh = mesh;

        mr.material = mat;

    }
}