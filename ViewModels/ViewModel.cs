using laba9rpm3k._2.Models;
using laba9rpm3k._2.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace laba9rpm3k._2.ViewModels
{
    public class ViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;

        public ObservableCollection<PhoneContact> Contacts { get; }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _phone = string.Empty;
        public string Phone
        {
            get => _phone;
            set => Set(ref _phone, value);
        }

        private PhoneContact? _selectedContact;
        public PhoneContact? SelectedContact
        {
            get => _selectedContact;
            set => Set(ref _selectedContact, value);
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public ViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            Contacts = new ObservableCollection<PhoneContact>();

            AddCommand = new RelayCommand(
                AddContact,
                () => CanAddContact());

            DeleteCommand = new RelayCommand(
                DeleteContact,
                () => CanDeleteContact());
        }

        private void AddContact()
        {
            if (Contacts.Any(c => c.Phone == this.Phone))
            {
                _dialogService.ShowWarning(
                    "Контакт с таким номером телефона уже существует!");
                return;
            }

            try
            {
                var newContact = new PhoneContact(Name, Phone);
                Contacts.Add(newContact);
                _dialogService.ShowInfo($"Контакт \"{newContact.Name}\" успешно добавлен!");
                Name = string.Empty;
                Phone = string.Empty;
            }
            catch (ArgumentException ex)
            {
                _dialogService.ShowError(ex.Message);
            }
        }

        private bool CanAddContact()
        {
            var tempContact = new PhoneContact(Name, Phone);
            return tempContact.Validate();
        }

        private void DeleteContact()
        {
            if (SelectedContact != null)
            {
                bool isConfirmed = _dialogService.ShowConfirmation(
                    $"Вы уверены, что хотите удалить контакт \"{SelectedContact.Name}\"?",
                    "Удаление контакта");

                if (isConfirmed)
                {
                    Contacts.Remove(SelectedContact);
                    SelectedContact = null;
                }
            }
        }

        private bool CanDeleteContact()
        {
            return SelectedContact != null;
        }
    }
}