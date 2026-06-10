using laba9rpm3k._2.Data;
using laba9rpm3k._2.Data.Entities;
using laba9rpm3k._2.Models;
using laba9rpm3k._2.Services;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace laba9rpm3k._2.ViewModels
{
    public class ContactsEditViewModel : ObservableObject, INavigationAware
    {
        private readonly ApplicationContext _context;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        private Contact _editingContact;
        private string _editName;
        private string _editPhone;

        public string EditName
        {
            get => _editName;
            set => Set(ref _editName, value);
        }

        public string EditPhone
        {
            get => _editPhone;
            set => Set(ref _editPhone, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ContactsEditViewModel(
            ApplicationContext context,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            SaveCommand = new RelayCommand(SaveContact, CanSaveContact);
            CancelCommand = new RelayCommand(CancelEdit);
        }

        public void OnNavigatedTo(object? parameter)
        {
            if (parameter is PhoneContact phoneContact)
            {
                _editingContact = new Contact
                {
                    Id = 0,
                    Name = phoneContact.Name,
                    Phone = phoneContact.Phone
                };
                EditName = phoneContact.Name;
                EditPhone = phoneContact.Phone;
            }
        }

        private async void SaveContact()
        {
            try
            {
                var contactFromDb = await _context.Contacts
                    .FirstOrDefaultAsync(c => c.Phone == _editingContact.Phone);

                if (contactFromDb != null)
                {
                    contactFromDb.Name = EditName;
                    contactFromDb.Phone = EditPhone;

                    await _context.SaveChangesAsync();
                    _dialogService.ShowInfo("Контакт успешно обновлён!");

                    _navigationService.NavigateTo<ContactsListViewModel>();
                }
                else
                {
                    _dialogService.ShowError("Контакт не найден в базе данных");
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private bool CanSaveContact()
        {
            var tempContact = new PhoneContact(EditName, EditPhone);
            return tempContact.Validate();
        }

        private void CancelEdit()
        {
            _navigationService.NavigateTo<ContactsListViewModel>();
        }
    }
}
