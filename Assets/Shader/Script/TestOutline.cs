using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Effects/TestOutline", 15)]
    public class TestOutline : Shadow
    {
        [SerializeField]
        private Color32 _gradualColor;

        [SerializeField] private float _lessScale = 1;
        [SerializeField] private float _largeScale = 1;
        [SerializeField] private Vector2 _offset;

        private VertexHelper _helper;
        protected TestOutline()
        {}

        protected void ApplyShadowZeroAlloc(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
        {
            
        }
        
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

                if (v.y<0)
                {
                    r = _gradualColor.a;
                    g = _gradualColor.g;
                    b = _gradualColor.b;
                }
                else
                {
                    r = color.a;
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
        
        protected void ApplyShadowZeroAllocByScale(List<UIVertex> verts, Color32 color, int start, int end, float scale)
        {
            UIVertex vt;

            var neededCapacity = verts.Count + end - start;
            if (verts.Capacity < neededCapacity)
                verts.Capacity = neededCapacity;

            for (int i = start; i < end; ++i)
            {
                vt = verts[i];
                verts.Add(vt);

                vt.position.x += scale * Mathf.Sign(vt.position.x);
                vt.position.y += scale * Mathf.Sign(vt.position.y);
                
                Vector3 v = vt.position;

                v.x += _largeScale;
                v.y += _largeScale;
                
                vt.position = v;

                byte r;
                byte g;
                byte b;
                
                r = _gradualColor.a;
                g = _gradualColor.g;
                b = _gradualColor.b;

                var newColor = new Color32(r, g, b, color.a);
                
                if (useGraphicAlpha)
                    newColor.a = (byte)((newColor.a * verts[i].color.a) / 255);
                vt.color = newColor;
                verts[i] = vt;
            }
        }

        // private void OnDrawGizmos()
        // {
        //     var vts = new List<UIVertex>();
        //     _helper.GetUIVertexStream(vts);
        //     
        //     for (int i = 0; i < vts.Count; i++)
        //     {
        //         Gizmos.color = Color.blue;
        //         Gizmos.DrawSphere(vts[i].position,100);
        //     }
        // }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;
            _helper = vh;
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
            ApplyShadowZeroAllocGradually(verts, effectColor, start, verts.Count, effectDistance.x, effectDistance.y);
            
            start = end;
            end = verts.Count;
            ApplyShadowZeroAllocGradually(verts, effectColor, start, verts.Count, effectDistance.x, -effectDistance.y);
            
            start = end;
            end = verts.Count;
            ApplyShadowZeroAllocGradually(verts, effectColor, start, verts.Count, -effectDistance.x, effectDistance.y);
            
            start = end;
            end = verts.Count;
            ApplyShadowZeroAllocGradually(verts, effectColor, start, verts.Count, -effectDistance.x, -effectDistance.y);
            
            // ApplyShadowZeroAllocByScale(verts,effectColor,start,verts.Count,_largeScale);
            // ApplyShadowZeroAllocByScale(verts,effectColor,start,verts.Count,1/_largeScale);
            
            vh.Clear();
            vh.AddUIVertexTriangleStream(verts);
            verts.Clear();
        }
    }   
}