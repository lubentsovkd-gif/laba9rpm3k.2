namespace laba9rpm3k._2.Services
{
    // Интерфейс для ViewModel, которые должны получать уведомления о навигации
    // Реализуется экранами, которым нужно передать параметры или выполнить логику при переходе
    public interface INavigationAware
    {
        void OnNavigatedTo(object? parameter);
    }
}
