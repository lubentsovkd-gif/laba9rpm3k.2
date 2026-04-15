using laba9rpm3k._2.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace laba9rpm3k._2.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        // Коллекция контактов
        public ObservableCollection<PhoneContact> Contacts { get; }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        // Свойство Phone
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

        // Команды
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel()
        {
            Contacts = new ObservableCollection<PhoneContact>();

            AddCommand = new RelayCommand(
                AddContact,
                () => CanAddContact());

            // Инициализация DeleteCommand
            DeleteCommand = new RelayCommand(
                DeleteContact,
                () => CanDeleteContact());
        }

        private void AddContact()
        {
            try
            {
                // Создаем новый Contact с Name и Phone
                var newContact = new PhoneContact(Name, Phone);
                // Добавляем его в Contacts
                Contacts.Add(newContact);
                // Очищаем поля ввода
                Name = string.Empty;
                Phone = string.Empty;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool CanAddContact()
        {
            // Возвращает true, если Name не пуст и Phone соответствует формату
            var tempContact = new PhoneContact(Name, Phone);
            return tempContact.Validate();
        }

        private void DeleteContact()
        {
            // Удаляем SelectedContact из коллекции Contacts, если он не null
            if (SelectedContact != null)
            {
                Contacts.Remove(SelectedContact);
                SelectedContact = null;
            }
        }

        private bool CanDeleteContact()
        {
            // Возвращает true, если SelectedContact не равен null
            return SelectedContact != null;
        }
    }
}