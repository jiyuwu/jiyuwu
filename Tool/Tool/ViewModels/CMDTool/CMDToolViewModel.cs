using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Tool.ViewModels.CMDTool
{
    public class CMDToolViewModel : BindableBase
    {
        private bool isTopChecked = false;//默认不置顶
        public bool IsTopChecked //更新
        {
            get { return isTopChecked; }
            set{SetProperty(ref isTopChecked, value);}
        }
        private string _path="";//默认空
        public string Path
        {
            get { return _path; }
            set { SetProperty(ref _path, value); }
        }

        public ICommand ClickOpen
        {
            get => new DelegateCommand<string>(ClickOpenFun);
        }
        private void ClickOpenFun(string path)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/K cd {path}", // /K 参数用于保持 cmd 窗口打开
                UseShellExecute = true,
                Verb = "runas" // 启动进程时请求管理员权限
            };
            Process.Start(psi);
        }
        public ICommand ToggleTop
        {
            get => new DelegateCommand<Window>(ToggleTopFun);
        }
        private void ToggleTopFun(Window window)
        {
            window.Topmost = IsTopChecked;
        }

    }
}
