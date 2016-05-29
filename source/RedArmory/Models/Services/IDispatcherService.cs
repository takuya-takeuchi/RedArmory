using System;
using System.Threading.Tasks;

namespace RedArmory.Models.Services
{

    public interface IDispatcherService
    {

        Task SafeAction(Action action);

    }

}