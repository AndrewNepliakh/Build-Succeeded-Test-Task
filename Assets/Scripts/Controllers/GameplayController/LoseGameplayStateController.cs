using System;
using System.Threading.Tasks;
using Services;

namespace Controllers
{
    public class LoseGameplayStateController : IGameplayStateController
    {
        public event Action OnStateComplete;
        
        public Task Init(ChangeStateData changeStateData)
        {
            OnStateComplete?.Invoke();
            
            return Task.CompletedTask;
        }
    }
}