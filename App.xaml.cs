using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using laba9rpm3k._2.Services;
using laba9rpm3k._2.ViewModels;

namespace laba9rpm3k._2
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();

            services.AddSingleton<IDialogService, DialogService>();
            services.AddTransient<ViewModel>();
            services.AddSingleton<MainWindow>(serviceProvider =>
            {
                var mainWindow = new MainWindow();
                mainWindow.DataContext = serviceProvider.GetRequiredService<ViewModel>();
                return mainWindow;
            });
            var serviceProvider = services.BuildServiceProvider();

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
