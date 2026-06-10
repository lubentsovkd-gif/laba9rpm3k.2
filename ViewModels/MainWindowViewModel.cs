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

        public ICommand ShowContactsCommand { get; }

        public ICommand ShowAboutCommand { get; }

        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            ShowContactsCommand = new RelayCommand(ExecuteShowContacts);
            ShowAboutCommand = new RelayCommand(ExecuteShowAbout);

            ExecuteShowContacts();
        }

        private void ExecuteShowContacts()
        {
            _navigationService.NavigateTo<ContactsListViewModel>(null);
        }

        private void ExecuteShowAbout()
        {
            _navigationService.NavigateTo<AboutViewModel>(null);
        }
    }
}