using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementsType
{
    LeftLocation,
    RightLocation,
    MiddleLocation
}

public class ArabicAdapterGroup : MonoBehaviour
{
    private List<ArabicAdapterElement> Elements;

    void Start()
    {
        Elements = FindElement();
        StartCoroutine(StartAdapt());
    }


    private List<ArabicAdapterElement> FindElement()
    {
        List<ArabicAdapterElement> list = new List<ArabicAdapterElement>();
        list.AddRange(GetComponentsInChildren<ArabicAdapterElement>());

        return list;
    }

    private void ArabicAdapt()
    {
        for (int i = 0; i < Elements.Count; i++)
        {
            var element = Elements[i];
            if (element.LocationType != ElementsType.MiddleLocation)
            {
                element.RefreshLocation(this);
            }
        }
    }

    private IEnumerator StartAdapt()
    {
        yield return new WaitForSeconds(0.5f);
        
        ArabicAdapt();
    }
}
