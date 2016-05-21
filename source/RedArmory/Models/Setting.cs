using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using RedArmory.Models.Services;

namespace RedArmory.Models
{

    public class Setting : ViewModelBase
    {

        #region �C�x���g
        #endregion

        #region �t�B�[���h

        private readonly BitnamiRedmineStack _Stack;

        #endregion

        #region �R���X�g���N�^

        public Setting(IBitnamiRedmineService bitnamiRedmineService, BitnamiRedmineStack stack)
        {
            if (bitnamiRedmineService == null)
                throw new ArgumentNullException(nameof(bitnamiRedmineService));

            if (stack == null)
                throw new ArgumentNullException(nameof(stack));

            this._Stack = stack;

            var configuration = new ServiceConfiguration
            {
                Apache = true,
                MySql = true,
                Redmine = true,
                Subversion = true
            };

            var serviceStatuses = bitnamiRedmineService.GetServiceDisplayNames(stack, configuration);
            this.ServiceStatuses = new ObservableCollection<ServiceStatus>(serviceStatuses);
        }

        #endregion

        #region �v���p�e�B

        private ObservableCollection<ServiceStatus> _ServiceStatuses;

        public ObservableCollection<ServiceStatus> ServiceStatuses
        {
            get
            {
                return this._ServiceStatuses;
            }
            set
            {
                this._ServiceStatuses = value;
                this.RaisePropertyChanged();
            }
        }

        public BitnamiRedmineStack Stack
        {
            get
            {
                return this._Stack;
            }
        }

        #endregion

        #region ���\�b�h

        #region �I�[�o�[���C�h

        #endregion

        #region �C�x���g�n���h��

        #endregion

        #region �w���p�[���\�b�h

        #endregion

        #endregion
    }
}