using System;
using System.Threading.Tasks;

namespace Ouranos.RedArmory.Models.Services
{

    public interface IDispatcherService
    {

        Task SafeAction(Action action);

    }

}