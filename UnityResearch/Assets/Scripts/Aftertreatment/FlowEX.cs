using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowEX : MonoBehaviour
{
    private Shader _flowShader;

    private Material _flowMat;

    private Image _image;

    private void Start()
    {
        Debug.Log(this.name);


    }

    public void SetColor(Color color)
    {
        if(_flowShader == null)
        {
#if UNITY_EDITOR
            _flowShader = Shader.Find("Flow");

#else
        _flowShader = QFramework.ResLoader.Allocate().LoadSync<Shader>(QAssetBundle.Shader_flow.FLOW);
#endif
        }

        if (_image == null)
        {
            _image = GetComponent<Image>();
        }

        _image.material = new Material(_flowShader);


        if (color != null)
        {
            _image.material.SetColor("_Color", color);
        }
    }
}
