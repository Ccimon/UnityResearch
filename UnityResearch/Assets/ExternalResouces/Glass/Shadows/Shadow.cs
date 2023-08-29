using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Shadow : MonoBehaviour {
	public Camera m_ShadowCamera;
	private RenderTexture m_RT;
	public Material m_Mat;

	void Start()
	{
        GetComponent<Projector>().enabled = true;
		//创建一个纹理，用来捕获照相机的看到的（该纹理既为我们需要的影子的源纹理）
        m_RT = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
		m_RT.anisoLevel = 8;
		m_ShadowCamera.targetTexture = m_RT;
		//将上面的纹理传入材质球中，用来将它处理为影子
		m_Mat.SetTexture("_MainTex", m_RT);
	}

	void LateUpdate()
	{
		//实时将生成影子需要的纹理的照相机的矩阵传入材质球，这两个矩阵在下面的着色器代码中使用
        m_Mat.SetTexture("_MainTex", m_RT);
		m_Mat.SetMatrix("_WorldToCameraMatrix", m_ShadowCamera.worldToCameraMatrix);
		m_Mat.SetMatrix("_ProjectionMatrix", m_ShadowCamera.projectionMatrix);

	}


    void OnDrawGizmos()
    {
        if (!Application.isEditor)
            return;
        m_Mat.SetTexture("_MainTex", m_RT);
        //实时将生成影子需要的纹理的照相机的矩阵传入材质球，这两个矩阵在下面的着色器代码中使用
        m_Mat.SetMatrix("_WorldToCameraMatrix", m_ShadowCamera.worldToCameraMatrix);
        m_Mat.SetMatrix("_ProjectionMatrix", m_ShadowCamera.projectionMatrix);
    }


    public void OpenShadow()
    {
        if (m_ShadowCamera.enabled)
        {
            m_ShadowCamera.enabled = false;
            GetComponent<Projector>().enabled = false;
        }
        else
        {
            m_ShadowCamera.enabled = true;
            GetComponent<Projector>().enabled = true;
        }
    }
}