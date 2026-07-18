using System;
using System.Threading.Tasks;

namespace Services
{
    public interface IState<T> where T : Enum 
    {
        T State { get; }
        Task Enter(ChangeStateData changeStateData = null);
        void Exit();
    }
}
