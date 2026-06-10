using laba9rpm3k._2.Models;
using laba9rpm3k._2.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using laba9rpm3k._2.Data;
using laba9rpm3k._2.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace laba9rpm3k._2.ViewModels
{
    public class ContactsListViewModel : ObservableObject, INavigationAware
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly ApplicationContext _context;

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
        public ICommand EditCommand { get; }

        public ContactsListViewModel(
            IDialogService dialogService,
            INavigationService navigationService,
            ApplicationContext context)  // ← НОВЫЙ ПАРАМЕТР
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _context = context ?? throw new ArgumentNullException(nameof(context));  // ← НОВОЕ
            Contacts = new ObservableCollection<PhoneContact>();

            AddCommand = new RelayCommand(AddContact, () => CanAddContact());
            DeleteCommand = new RelayCommand(DeleteContact, () => CanDeleteContact());
            EditCommand = new RelayCommand(EditContact, () => CanEditContact());

            LoadContactsFromDatabase();  // ← НОВЫЙ ВЫЗОВ
        }

        private void LoadContactsFromDatabase()
        {
            try
            {
                var dbContacts = _context.Contacts.ToList();
                Contacts.Clear();
                foreach (var dbContact in dbContacts)
                {
                    Contacts.Add(new PhoneContact(dbContact.Name, dbContact.Phone));
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка загрузки данных: {ex.Message}");
            }
        }
        public void OnNavigatedTo(object? parameter)
        {
            LoadContactsFromDatabase();
        }

        private async void AddContact()
        {
            if (_context.Contacts.Any(c => c.Phone == this.Phone))
            {
                _dialogService.ShowWarning("Контакт с таким номером телефона уже существует!");
                return;
            }

            try
            {
                var newContact = new Contact
                {
                    Name = this.Name,
                    Phone = this.Phone
                };

                await _context.Contacts.AddAsync(newContact);
                await _context.SaveChangesAsync();

                Contacts.Add(new PhoneContact(Name, Phone));
                _dialogService.ShowInfo($"Контакт \"{Name}\" успешно добавлен!");

                Name = string.Empty;
                Phone = string.Empty;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка при добавлении: {ex.Message}");
            }
        }

        private bool CanAddContact()
        {
            var tempContact = new PhoneContact(Name, Phone);
            return tempContact.Validate();
        }

        private async void DeleteContact()
        {
            if (SelectedContact == null) return;

            bool isConfirmed = _dialogService.ShowConfirmation(
                $"Вы уверены, что хотите удалить контакт \"{SelectedContact.Name}\"?",
                "Удаление контакта");

            if (!isConfirmed) return;

            try
            {
                var contactToDelete = await _context.Contacts
                    .FirstOrDefaultAsync(c => c.Phone == SelectedContact.Phone);

                if (contactToDelete != null)
                {
                    _context.Contacts.Remove(contactToDelete);
                    await _context.SaveChangesAsync();

                    Contacts.Remove(SelectedContact);
                    SelectedContact = null;
                    _dialogService.ShowInfo("Контакт успешно удалён!");
                }
                else
                {
                    _dialogService.ShowWarning("Контакт не найден в базе данных");
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Ошибка при удалении: {ex.Message}");
            }
        }
        private void EditContact()
        {
            if (SelectedContact != null)
            {
                _navigationService.NavigateTo<ContactsEditViewModel>(SelectedContact);
            }
        }

        private bool CanDeleteContact()
        {
            return SelectedContact != null;
        }
        private bool CanEditContact()
        {
            return SelectedContact != null;
        }
    }
}