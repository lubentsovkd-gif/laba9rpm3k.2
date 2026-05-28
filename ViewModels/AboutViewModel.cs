using laba9rpm3k._2.Services;

namespace laba9rpm3k._2.ViewModels
{
    public class AboutViewModel : ObservableObject, INavigationAware
    {
        private string _appName = "Телефонная книга MVVM";
        private string _authors = "by me (kirxda) & Deepseek";

        public string AppName
        {
            get => _appName;
            set => Set(ref _appName, value);
        }

        public string Authors
        {
            get => _authors;
            set => Set(ref _authors, value);
        }

        public AboutViewModel()
        {
        }

        public void OnNavigatedTo(object? parameter)
        {

        }
    }
}
