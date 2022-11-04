using UnityEngine;
using System.Collections;

/// <summary>
/// 不继承Monobehaviour(不带生命周期，不需要挂)的脚本
/// </summary>
public class MolaSingleton<T> where T : new()
{
    #region Instance

    private static T _instance = default;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }

        #endregion
    }
}
