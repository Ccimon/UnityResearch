using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStencilTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // var img = GetComponent<Image>();
        // img.material.SetFloat("_StencilComp", 3);
        var render = GetComponent<MeshRenderer>();
        render.material.SetFloat("_StencilValue",1);
        render.material.SetFloat("_StencilComp",5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
