using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Effects/TestOutline", 15)]
    public class TestOutline : Shadow
    {
        [SerializeField]
        private Color32 _gradualColor;

        [SerializeField] 
        private int _repeatRender = 4;

        protected TestOutline()
        {}

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
                    position =  vt.position * 1.2f
                };
            }

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