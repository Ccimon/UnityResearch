
using GlobalModel.Data;

public interface IViewController<T,TData>  where TData : Data
{

    T Init<TData>(TData data);
    
    T RefreshView<TData>(TData data);
    
}