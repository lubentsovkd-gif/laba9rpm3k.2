using laba9rpm3k._2.Services;
using System.Windows.Input;

namespace laba9rpm3k._2.ViewModels
{
    /*
     * ViewModel для главного окна-оболочки (Shell)
     * Отвечает только за навигацию между экранами приложения
    */
    public class MainWindowViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        public INavigationService NavigationService => _navigationService;

        // Команда для перехода к списку контактов
        public ICommand ShowContactsCommand { get; }

        // Команда для перехода на экран "О программе"
        public ICommand ShowAboutCommand { get; }

        // Конструктор получает сервис навигации через DI
        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            // Инициализация команд
            ShowContactsCommand = new RelayCommand(ExecuteShowContacts);
            ShowAboutCommand = new RelayCommand(ExecuteShowAbout);

            // При запуске показываем список контактов (экран по умолчанию)
            ExecuteShowContacts();
        }

        private void ExecuteShowContacts()
        {
            // Навигация к ViewModel списка контактов
            // Без параметра, так как это стартовый экран
            _navigationService.NavigateTo<ContactsListViewModel>(null);
        }

        private void ExecuteShowAbout()
        {
            // Навигация к ViewModel "О программе"
            _navigationService.NavigateTo<AboutViewModel>(null);
        }
    }
}