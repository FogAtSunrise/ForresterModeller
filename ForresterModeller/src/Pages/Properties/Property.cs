

using System;
using System.Windows.Navigation;

namespace ForresterModeller.src.Pages.Properties
{
    /// <summary>
    /// Класс, определяющий поле в окне свойств объекта.
    /// Для каждой строки (как редактируемой, так и нет) необходимо создать
    /// свой экземпляр Property и передать их массив в view
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Конструктор для редактируемых полей объекта
        /// </summary>
        /// <param name="name">Имя поля</param>
        /// <param name="value">Значение поля</param>
        /// <param name="updateAction">Метод, обрабатывающий обновление поля на форме</param>
        public Property(String name, String value, Action<String> updateAction)
        {
            Name = name;
            _value = value;
            _updateValue = updateAction;
            IsReadOnly = false;
        }
        /// <summary>
        /// Конструктор для нередактируемых полей объекта
        /// </summary>
        /// <param name="name">Имя поля</param>
        /// <param name="value">Значение поля</param>
        public Property(String name, String value)
        {
            Name = name;
            _value = value;
            IsReadOnly = true;
        }
        public string Name { get; set; }
        private Action<String> _updateValue;
        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                _updateValue(value);
            }

        }
        public bool IsReadOnly { get; set; }
    }
}
