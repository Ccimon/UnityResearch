using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Effects/TestOutline", 15)]
    public class TestOutline : Shadow
    {
        /// <summary>
        /// 渐变色的另一个颜色
        /// </summary>
        [SerializeField]
        private Color32 _gradualColor;
        
        /// <summary>
        /// 重复渲染次数
        /// </summary>
        [SerializeField] 
        private int _repeatRender = 4;

        protected TestOutline()
        {}
        
        /// <summary>
        /// 调整Mesh的位置，以及颜色，是其呈现描边效果，以及颜色渐变
        /// </summary>
        /// <param name="verts">顶点列表</param>
        /// <param name="color">默认颜色</param>
        /// <param name="start">顶点列表的索引起始点</param>
        /// <param name="end">顶点列表的索引终点</param>
        /// <param name="x">x轴方向偏移量</param>
        /// <param name="y">y轴方向偏移量</param>
        protected void ApplyShadowZeroAllocGradually(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
        {
            UIVertex vt;

            var neededCapacity = verts.Count + end - start;
            if (verts.Capacity < neededCapacity)
                verts.Capacity = neededCapacity;

            for (int i = start; i < end; ++i)
            {
                vt = verts[i];
                verts.Add(vt);

                Vector3 v = vt.position;
                v.x += x;
                v.y += y;
                vt.position = v;

                byte r;
                byte g;
                byte b;
                
                // 字体渲染本身自带过渡效果，这里直接添加颜色
                if (v.y < 0)
                {
                    r = _gradualColor.r;
                    g = _gradualColor.g;
                    b = _gradualColor.b;
                }
                else
                {
                    r = color.r;
                    g = color.g;
                    b = color.b;
                }

                var newColor = new Color32(r, g, b, color.a);
                
                if (useGraphicAlpha)
                    newColor.a = (byte)((newColor.a * verts[i].color.a) / 255);
                vt.color = newColor;
                verts[i] = vt;
            }
        }
        
        /// <summary>
        /// 重写父类Shadow的Mesh生成
        /// </summary>
        /// <param name="vh"></param>
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;
            var verts = new List<UIVertex>();
            vh.GetUIVertexStream(verts);
            var neededCpacity = verts.Count * 5;
            if (verts.Capacity < neededCpacity)
                verts.Capacity = neededCpacity;

            
            for (int i = 0; i < verts.Count; i++)
            {
                var vt = verts[i];
                verts[i] = new UIVertex()
                {
                    uv0 = vt.uv0,
                    uv1 = vt.uv1,
                    uv2 = vt.uv2,
                    uv3 = vt.uv3,
                    normal = vt.normal,
                    color = vt.color,
                    tangent = vt.tangent,
                    position =  vt.position
                };
            }
            
            // 这里执行重复渲染
            // 重复渲染的主要目的是增加描边精细度
            // Outline组件实现描边效果的核心逻辑是重复渲染Mesh，并将多渲染的Mesh添加新的颜色，并将其顶点在原来的位置上进行偏移
            // 这样就会导致每个Mesh都相较于原文字本身露出来一点点，最终形成描边的效果
            // 但是这个描边的效果，会随着偏移量的增大效果逐渐变差
            // 补足这个描边效果最直接的方式就是增加渲染次数，缩减每个新增Mesh的渲染间隔
            // 于是就有了以下这段逻辑
            var start = 0;
            var end = verts.Count;
            var deg = 360 / _repeatRender;
            for (int i = 0; i < _repeatRender; i++)
            {
                if (i > 0)
                {
                    start = end;
                    end = verts.Count;
                }

                float x = effectDistance.x * Mathf.Cos(Mathf.Deg2Rad * i *deg);
                float y = effectDistance.x * Mathf.Sin(Mathf.Deg2Rad * i *deg);
                ApplyShadowZeroAllocGradually(verts, effectColor, start, verts.Count, x, y);
            }

            vh.Clear();
            vh.AddUIVertexTriangleStream(verts);
            verts.Clear();
        }
    }   
}