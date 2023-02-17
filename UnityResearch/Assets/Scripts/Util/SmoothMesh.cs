using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmoothMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        MeshFilter filter = GetComponent<MeshFilter>();
        var normalBuffer = GenerateSmoothNormals(filter.mesh);
        filter.mesh.tangents = normalBuffer;
    }

    public Vector4[] GenerateSmoothNormals(Mesh _srcMesh)
    {
        Vector3[] verticies = _srcMesh.vertices;
        Vector3[] normals = _srcMesh.normals;
        Vector4[] smoothNormals = new Vector4[verticies.Length];
        int i = 0;
        foreach (var smo in verticies)
        {
            smoothNormals[i] = smo;
            i++;
        }
        var groups = verticies.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);
        foreach (var group in groups)
        {
            if (group.Count() == 1)
                continue;
            Vector3 smoothNormal = Vector3.zero;
            foreach (var index in group)
                smoothNormal += normals[index.Value];
            smoothNormal = smoothNormal.normalized;
            foreach (var index in group)
                smoothNormals[index.Value] = smoothNormal;
        }
        return smoothNormals;
    }
}
