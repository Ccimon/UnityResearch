using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class ArabicAdapterElement : MonoBehaviour
{
    public ElementsType LocationType;

    public void RefreshLocation(ArabicAdapterGroup group)
    {
        RefreshLocationRecursive(transform,group.transform);
    }

    private void RefreshLocationRecursive(Transform local,Transform group)
    {
        ReverseAnchorPreset(local.GetRectTransform(),group.GetRectTransform());
        if (local.childCount > 0)
        {
            for (int i = 0; i < local.childCount; i++)
            {
                var child = local.GetChild(i);
                RefreshLocationRecursive(child,local);
                // Debug.Log(child.gameObject.name + "" + child.GetRectTransform().offsetMax.ToString() + child.GetRectTransform().offsetMin.ToString() );
            }
        }
    }

    private void ReverseAnchorPreset(RectTransform transform,RectTransform group)
    {
        var position = transform.GetRelativePosition(group);
        var location = group.position + position;
        transform.position = location;
    }
}