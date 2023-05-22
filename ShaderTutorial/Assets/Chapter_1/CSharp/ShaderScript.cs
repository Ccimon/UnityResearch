using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderScript : MonoBehaviour
{
    private Image _image;
    void OnEnable()
    {
        _image = GetComponent<Image>();
        StartCoroutine(ResetMaterialProperty());
    }

    private IEnumerator ResetMaterialProperty()
    {
        _image.material.SetFloat("_Blend",0f);
        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i <= 60 ;i ++)
        {
            yield return new WaitForSeconds(0.1f);
            _image.material.SetFloat("_Blend",i * 0.0167f);
        }
    }
}
