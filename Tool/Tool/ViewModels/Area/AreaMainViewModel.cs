using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Tool.Views.Area;

namespace Tool.ViewModels.Area
{
    public class AreaMainViewModel : BindableBase
    {
        public DelegateCommand<string> BtnCommand { get; private set; }


        private  IRegionManager _regionManager;
        public AreaMainViewModel(IRegionManager regionManager)
        {
            //Body = new AreaUserA();
            #region prism set default region
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(AreaUserA));
            #endregion
            BtnCommand = new DelegateCommand<string>(GoUrl);
        }

        //private object body;
        //public object Body
        //{
        //    get { return this.body; }
        //    set
        //    {
        //        this.body = value;
        //        RaisePropertyChanged();
        //    }
        //}

        private void GoUrl(object obj)
        {
            _regionManager.Regions["ContentRegion"].RequestNavigate(obj.ToString());
            //regionManager.RequestNavigate("ContentRegion", param);
            //switch (param)
            //{
            //    case "AreaUserA": Body = new AreaUserA(); break;
            //    case "AreaUserB": Body = new AreaUserB(); break;
            //    default: Body = new AreaUserA(); break;
            //}
        }
    }
}
