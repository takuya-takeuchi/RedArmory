using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Ouranos.RedArmory.Models.Services
{

    public sealed class DispatcherService : IDispatcherService
    {

        private readonly Dispatcher _Dispatcher;

        public DispatcherService(Dispatcher dispatcher)
        {
            this._Dispatcher = dispatcher;
        }

        public async Task SafeAction(Action action)
        {
            if (!this._Dispatcher.CheckAccess())
                await this._Dispatcher.InvokeAsync(action);
            else
                action.Invoke();
        }

    }

}
