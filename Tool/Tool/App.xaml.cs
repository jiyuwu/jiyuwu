using IService;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
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
            return new MainWindow();
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
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }
    }
}
