using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using Tool.ViewModels.Area;
using Tool.Views;
using Tool.Views.Area;

namespace Tool.ViewModels.Article
{
    public class ArticleModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public ArticleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            try
            {
                _regionManager.RequestNavigate("ArticleListRegion", "ArticleList");
                _regionManager.RequestNavigate("ArticleEditRegion", "ArticleEdit");
                _regionManager.RequestNavigate("ContentRegion", "AreaUserA");
            }
            catch (Exception ex)
            {
                // 处理异常，例如输出异常信息或者记录日志
                Console.WriteLine($"An error occurred while navigating to views: {ex.Message}");
            }
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AreaUserA>("AreaUserA");
            containerRegistry.RegisterForNavigation<AreaUserB>("AreaUserB");
            //containerRegistry.RegisterForNavigation<AreaUserA, AreaUserAViewModel>();
            //containerRegistry.RegisterForNavigation<AreaUserB, AreaUserBViewModel>();
            containerRegistry.RegisterForNavigation<ArticleList, ArticleListViewModel>();
            containerRegistry.RegisterForNavigation<ArticleEdit, ArticleEditViewModel>();
        }
    }
}
