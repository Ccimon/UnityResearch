using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using QFra

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
            _flowShader = Shader.Find("Flow");
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
