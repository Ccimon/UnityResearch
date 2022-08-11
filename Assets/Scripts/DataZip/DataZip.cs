﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataZip : MonoBehaviour
{
    // Start is called before the first frame update
    public int MaxCount = 32;

    private bool[] Databuffer;
    void Start()
    {
        Databuffer = new bool[MaxCount];

        for (int i = 0; i < MaxCount; i++)
        {
            Databuffer[i] = Random.Range(0.0f,1.0f) > 0.5f ? true : false;
        }

        var list = DataRelease(DataCompress(Databuffer));

        for (int i = 0; i < Databuffer.Length; i ++)
        {
            Debug.Log(Databuffer[i] == list[i]);
        }
        Debug.Log("check end");
    }

    /// <summary>
    /// 对Bool数据列表进行压缩
    /// </summary>
    /// <param name="datas">要执行压缩的Bool列表</param>
    /// <returns>返回无符号Int数组</returns>
    public UInt32[] DataCompress(bool[] datas)
    {
        int i = 0;
        int index = 0;
        UInt32[] intbuffer = new UInt32[datas.Length/32 + 1];
        while (i < datas.Length)
        {
            UInt32 info = 0;
            int max = i + 32;
            for (int j = i; j < max; j ++)
            {
                UInt32 extra = datas[j] ? (UInt32)Math.Pow(2, j) : 0;
                info += extra;
                i++;
            }

            intbuffer[index] = info;
            index++;
            // Debug.Log("bool convert to int. Value:" + info);
        }
        
        return intbuffer;
    }

    /// <summary>
    /// 对Bool数据列表进行压缩
    /// </summary>
    /// <param name="datas">要执行压缩的Bool列表</param>
    /// <returns>返回无符号Int数组</returns>
    public UInt32[] DataCompress(List<bool> datas)
    {
        int i = 0;
        int index = 0;
        UInt32[] intbuffer = new UInt32[datas.Count/32 + 1];
        while (i < datas.Count)
        {
            UInt32 info = 0;
            for (int j = i; j < 32; j ++)
            {
                UInt32 extra = datas[j] ? (UInt32)Math.Pow(2, j) : 0;
                info += extra;
                i++;
            }

            intbuffer[index] = info;
            index++;
            // Debug.Log("bool convert to int. Value:" + info);
        }
        
        return intbuffer;
    }
    
    /// <summary>
    /// 解压缩列表
    /// </summary>
    /// <param name="datas">需要进行解压缩的UInt列表</param>
    /// <returns>接压缩之后的Bool列表</returns>
    public List<bool> DataRelease(UInt32[] datas)
    {
        List<bool> releasebuffer = new List<bool>();
        UInt32 standard = (UInt32)Math.Pow(2,31);
        // Debug.Log("standard value:" + standard);
        for (int i = 0; i < datas.Length; i ++)
        {
            UInt32 info = datas[i];
            for (int j = 0; j < 32; j ++)
            {
                bool flag = info % 2 > 0 ? true : false;
                info =  info >> 1;
                releasebuffer.Add(flag);
            }
        }

        return releasebuffer;
    }
}
