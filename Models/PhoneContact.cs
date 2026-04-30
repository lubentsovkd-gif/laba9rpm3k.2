using laba9rpm3k._2.ViewModels;
using System.Text.RegularExpressions;

namespace laba9rpm3k._2.Models
{
    public class PhoneContact : ObservableObject
    {
        private string _name = string.Empty;
        private string _phone = string.Empty;

        // Конструктор, который вызывает Validate для проверки значений
        public PhoneContact(string name, string phone)
        {
            Name = name;
            Phone = phone;
        }

        public string Name
        {
            get => _name;
            set
            {
                Set(ref _name, value);
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                Set(ref _phone, value);
            }
        }

        // Метод Validate проверяет, что Name не пуст и Phone соответствует формату
        public bool Validate()
        {
            // Формат номера -> "+7-ХХХ-ХХХ-ХХ-ХХ"
            Regex phoneFormat = new Regex(@"^\+7-\d{3}-\d{3}-\d{2}-\d{2}$");
            return !string.IsNullOrEmpty(this.Name) && phoneFormat.IsMatch(this.Phone);
        }
    }
}