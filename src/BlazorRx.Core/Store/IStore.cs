using System;
namespace BlazorRx.Core.Store
{
    public interface IStore<T> where T : class
    {
        T GetCurrentState();
    }
}
