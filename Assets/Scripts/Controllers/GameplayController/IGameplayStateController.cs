using Services;
using System.Threading.Tasks;

namespace Controllers
{
    public interface IGameplayStateController
    {
        Task Init(ChangeStateData changeStateData);
    }
}