using laba9rpm3k._2.Data;
using laba9rpm3k._2.Services;
using laba9rpm3k._2.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace laba9rpm3k._2
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=PhoneBookDB_Лубенцов_2307д1;User Id=kirxda;Password=1234;TrustServerCertificate=True"));

            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, AppNavigationService>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddTransient<ContactsEditViewModel>();
            services.AddTransient<ContactsListViewModel>();
            services.AddTransient<AboutViewModel>();

            services.AddSingleton<MainWindow>(serviceProvider =>
            {
                var mainWindow = new MainWindow();
                mainWindow.DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();
                return mainWindow;
            });

            var serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
