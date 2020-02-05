using System;
using GalaSoft.MvvmLight;
using Microsoft.Win32.TaskScheduler;

namespace Ouranos.RedArmory.Models
{

    public sealed class TaskSchedulerItem : ViewModelBase, IDisposable
    {

        #region Fields

        /// <summary>
        /// Indicate value whether <see cref="M:System.IDisposable.Dispose"/> method was called.
        /// </summary>
        private bool _Disposed = false;

        private readonly Task _Task;

        #endregion

        #region Constructors

        internal TaskSchedulerItem(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            this._Task = task;
            this._Enabled = task.Enabled;
            this._LastRunTime = task.LastRunTime;
            this._Name = task.Name;
            this._NextRunTime = task.NextRunTime;
            this._TaskState = task.State;
        }

        #endregion

        #region Properties

        private bool _Enabled;

        public bool Enabled
        {
            get
            {
                return this._Enabled;
            }
            set
            {
                this._Enabled = value;
                this.RaisePropertyChanged();

                if (!this._Disposed && this._Task.Enabled != value)
                {
                    this._Task.Enabled = value;

                    // Vista 以降、Enabled は即座に変更されるため、
                    // TaskState も更新する
                    this.TaskState = this._Task.State;
                }
            }
        }

        private DateTime _LastRunTime;

        public DateTime LastRunTime
        {
            get
            {
                return this._LastRunTime;
            }
            private set
            {
                this._LastRunTime = value;
                this.RaisePropertyChanged();
            }
        }

        private string _Name;

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
                this.RaisePropertyChanged();
            }
        }

        private DateTime _NextRunTime;

        public DateTime NextRunTime
        {
            get
            {
                return this._NextRunTime;
            }
            private set
            {
                this._NextRunTime = value;
                this.RaisePropertyChanged();
            }
        }

        public Task Task
        {
            get
            {
                return this._Task;
            }
        }

        private TaskState _TaskState;

        public TaskState TaskState
        {
            get
            {
                return this._TaskState;
            }
            private set
            {
                this._TaskState = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        public void Run()
        {
            if (!this._Disposed)
            {
                this._Task.Run();

                this.TaskState = this._Task.State;
                this.LastRunTime = this._Task.LastRunTime;
            }
        }

        public void Stop()
        {
            if (!this._Disposed)
            {
                this._Task.Stop();

                this.TaskState = this._Task.State;
                this.LastRunTime = this._Task.LastRunTime;
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Releases all resources used by this <see cref="TaskSchedulerItem"/>.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        /// <summary>
        /// Releases all resources used by this <see cref="TaskSchedulerItem"/>.
        /// </summary>
        /// <param name="disposing">Indicate value whether <see cref="M:System.IDisposable.Dispose"/> method was called.</param>
        private void Dispose(bool disposing)
        {
            if (this._Disposed)
            {
                return;
            }

            this._Disposed = true;

            if (disposing)
            {
                // マネージリソースの解放処理
                this._Task?.Dispose();
            }
        }

        #endregion

    }

}