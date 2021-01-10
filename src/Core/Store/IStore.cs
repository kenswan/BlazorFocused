using System;
using System.Threading.Tasks;

namespace BlazoRx.Core.Store
{
    public interface IStore<TState> where TState : class
    {
        TState GetCurrentState();

        void SetState(Func<TState, TState> updateFunction);

        void Build();
    }
}
