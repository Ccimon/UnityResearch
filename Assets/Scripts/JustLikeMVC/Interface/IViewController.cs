
using GlobalModel.Data;

/// <summary>
/// 所有的ViewController都需要实现这个接口
/// </summary>
/// <typeparam name="T">Init之后会返回的对象类型,一般都是自己的类</typeparam>
/// <typeparam name="TData">Init所需要传入的Data</typeparam>
public interface IViewController<T,TData>  where TData : Data
{
    
    /// <summary>
    /// 初始化页面
    /// </summary>
    /// <param name="data">初始化页面所需的数据</param>
    /// <typeparam name="TData">初始化页面所需的数据类型</typeparam>
    /// <returns>返回自己</returns>
    T Init<TData>(TData data);
    
    /// <summary>
    /// 刷新视图
    /// </summary>
    /// <returns>返回自己</returns>
    T RefreshView();
    
}