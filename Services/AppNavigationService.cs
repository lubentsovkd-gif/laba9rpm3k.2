using laba9rpm3k._2.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace laba9rpm3k._2.Services
{
     public class AppNavigationService : ObservableObject, INavigationService
     {
         private readonly IServiceProvider _serviceProvider;
         private object? _currentViewModel;

         // Текущая ViewModel. При изменении автоматически обновляет UI через PropertyChanged
         public object? CurrentViewModel
         {
             get => _currentViewModel;
             private set
             {
                 if (_currentViewModel != value)
                 {
                     _currentViewModel = value;
                     OnPropertyChanged();
                 }
             }
         }
         public AppNavigationService(IServiceProvider serviceProvider)
         {
             _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
         }

         public void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : class
         {
             // 1. Создаём новый экземпляр ViewModel через DI-контейнер
             //    Используем Transient lifetime — новый экземпляр при каждом переходе
             var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

             // 2. Если ViewModel поддерживает INavigationAware, передаём параметр
             if (viewModel is INavigationAware navigationAware)
             {
                 navigationAware.OnNavigatedTo(parameter);
             }

             // 3. Обновляем текущую ViewModel (триггерит обновление UI)
             CurrentViewModel = viewModel;
         }
     }
}
