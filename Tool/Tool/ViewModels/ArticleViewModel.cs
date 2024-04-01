using Business;
using IService;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tool.ViewModels
{
    public class ArticleViewModel : BindableBase
    {
        //public string Name { get; set; } = "YCH";
        private string name = "YCH";
        private readonly IArticleService _service;
        ArticleBll articleBll;
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty<string>(ref name, value);
            }
        }
        public ArticleViewModel()
        {
            _service=App.ServiceProvider.GetService<IArticleService>();
            articleBll = new ArticleBll(_service);
            Task.Run(async () => {
                await Task.Delay(2000);
                Name = "这是个测试";
            });
        }
        public ICommand ClickCommand
        {
            get => new DelegateCommand<object>(ButtonClick);
        }
        private void ButtonClick(object obj)
        {
            this.Name = "哈哈哈哈！" + (string)obj;
        }
    }
}
