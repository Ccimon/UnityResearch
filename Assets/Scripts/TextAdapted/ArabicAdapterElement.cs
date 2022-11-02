using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ArabicAdapterElement : MonoBehaviour
{
    public ElementsType ELtype;

    public void RefreshLocation(ArabicAdapterGroup group)
    {
        RefreshLocationRecursive(transform,group.transform);
    }

    private void RefreshLocationRecursive(Transform local,Transform group)
    {
        ReverseAnchorPreset(local,group);
        if (ELtype == ElementsType.Recursive && local.childCount > 0)
        {
            for (int i = 0; i < local.childCount; i++)
            {
                var child = local.GetChild(i);
                RefreshLocationRecursive(child,local);
            }
        }
    }

    private void ReverseAnchorPreset(Transform trans,Transform group)
    {
        // string logStr =  "{0},AnchorMax:{1},AnchorMin:{2}";
        // Debug.Log(string.Format(logStr,trans.gameObject.name
        //     ,trans.GetRectTransform().anchorMax
        //     ,trans.GetRectTransform().anchorMin));
        // Debug.Log(string.Format(logStr,group.gameObject.name
        //     ,group.GetRectTransform().anchorMax
        //     ,group.GetRectTransform().anchorMin));
        // Debug.Log("________________________________");
        
        var relative = trans.GetRelativePosition(group);
        relative.x *= -1;
        relative.x -= trans.GetRectTransform().rect.width * Math.Sign(relative.x);

        trans.position = group.transform.position + relative;
    }
}