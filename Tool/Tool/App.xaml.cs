using IService;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using Repository.DBHelper;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using Tool.ViewModels.Article;
using Tool.Views;

namespace Tool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        protected override Window CreateShell()
        {
            SQLiteHelper.CreateDB();
            SQLiteHelper.CreateTable();
            // 创建主窗体的实例
            MainWindow mainWindow = new MainWindow(Container.Resolve<IRegionManager>());
            return mainWindow;
        }
        protected override void RegisterTypes(IContainerRegistry registry)
        {
            var services = new ServiceCollection();
            var serviceAsm = Assembly.Load(new AssemblyName("Repository"));

            foreach (Type serviceType in serviceAsm.GetTypes().Where(t => typeof(IServiceSupport).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract))
            {
                var interfaceTypes = serviceType.GetInterfaces();

                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddSingleton(interfaceType, serviceType);
                }
            }

            // 构建 DI 容器并保存实例到 App.ServiceProvider
            ServiceProvider = services.BuildServiceProvider();

            //向容器中注入一个导航
            registry.RegisterForNavigation<Views.Area.AreaUserA>("AreaUserA");
            registry.RegisterForNavigation<Views.Area.AreaUserB>("AreaUserB");
        }

        //protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        //{
        //    moduleCatalog.AddModule<ArticleModule>();
        //    base.ConfigureModuleCatalog(moduleCatalog);
        //}
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}
