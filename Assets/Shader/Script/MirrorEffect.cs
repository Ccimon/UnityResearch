using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorEffect : MonoBehaviour
{
    // Start is called before the first frame update
    new Camera camera;
    public bool Horizontal;
    void Awake()
    {
        camera = GetComponent<Camera>();
    }
    void OnPreCull()
    {
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        Vector3 scale = new Vector3(Horizontal ? -1 : 1, 1, 1);
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
    }
    void OnPreRender()
    {
        GL.invertCulling = Horizontal;
    }

    void OnPostRender()
    {
        GL.invertCulling = false;
    }
}
