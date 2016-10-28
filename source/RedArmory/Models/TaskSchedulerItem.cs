using System;
using GalaSoft.MvvmLight;
using Microsoft.Win32.TaskScheduler;

namespace Ouranos.RedArmory.Models
{

    public sealed class TaskSchedulerItem : ViewModelBase, IDisposable
    {

        #region �t�B�[���h

        /// <summary>
        /// <see cref="System.IDisposable.Dispose"/> ���\�b�h���Ă΂ꂽ���ǂ����������l��\���܂��B
        /// </summary>
        private bool _Disposed = false;

        private readonly Task _Task;

        #endregion

        #region �R���X�g���N�^

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

        #region �v���p�e�B

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

                    // Vista �ȍ~�AEnabled �͑����ɕύX����邽�߁A
                    // TaskState ���X�V����
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

        #region ���\�b�h

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

        #region IDisposable �����o

        /// <summary>
        /// <see cref="TaskSchedulerItem"/> �ɂ���Ďg�p����Ă��邷�ׂẴ��\�[�X��������܂��B
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        /// <summary>
        /// <see cref="TaskSchedulerItem"/> �ɂ���Ďg�p����Ă��邷�ׂẴ��\�[�X��������܂��B
        /// </summary>
        /// <param name="disposing"><see cref="System.IDisposable.Dispose"/> ���\�b�h���Ă΂ꂽ���ǂ����������l�B</param>
        private void Dispose(bool disposing)
        {
            if (this._Disposed)
            {
                return;
            }

            this._Disposed = true;

            if (disposing)
            {
                // �}�l�[�W���\�[�X�̉������
                this._Task?.Dispose();
            }
        }

        #endregion

    }

}