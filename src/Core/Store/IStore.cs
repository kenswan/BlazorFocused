using System;
namespace BlazoRx.Core.Store
{
    public interface IStore<TState> where TState : class
    {
        TState GetCurrentState();

        void AddReducer<TType>() where TType : Type;

        void GetReducer<TType>() where TType : Type;

        void Build();
    }
}
