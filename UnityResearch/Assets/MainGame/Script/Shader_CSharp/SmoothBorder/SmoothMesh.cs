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
    
    /// <summary>
    /// 生成光滑法线
    /// 推荐将生成的法线放到mesh的切线数组中
    /// </summary>
    /// <param name="_srcMesh"></param>
    /// <returns>返回法线数组</returns>
    public Vector4[] GenerateSmoothNormals(Mesh _srcMesh)
    {
        Vector3[] verticies = _srcMesh.vertices;
        Vector3[] normals = _srcMesh.normals;
        Vector4[] smoothNormals = new Vector4[verticies.Length];
        int i = 0;
        // 把vector3[] 重新装成vector4[]
        foreach (var smo in verticies)
        {
            smoothNormals[i] = smo;
            i++;
        }
        
        // 这里的分组实际上是把使用同一个顶点的面分组，一个顶点会被多个面使用，在不同的面下这个顶点的法线是不一样的
        // 这里通过顶点的vector分组，把相同顶点的分到一个组
        // 然后通过组来获取该顶点的在其他面被使用时的index
        // 通过index获取到关联的顶点，然后拿到法线
        var groups = verticies.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);
        
        foreach (var group in groups)
        {
            if (group.Count() == 1)
                continue;
            Vector3 smoothNormal = Vector3.zero;
            foreach (var index in group)
                // 根据index获取不同面同一顶点的法线
                smoothNormal += normals[index.Value];
            // 法线累加然后归一化
            smoothNormal = smoothNormal.normalized;
            foreach (var index in group)
                // 以相同的index放到光滑法线的数组中
                smoothNormals[index.Value] = smoothNormal;
        }
        
        return smoothNormals;
    }
}
