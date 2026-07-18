using System;
using Services;
using System.Threading.Tasks;

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