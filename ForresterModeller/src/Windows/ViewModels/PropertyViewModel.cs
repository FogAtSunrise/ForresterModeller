using System;
using ForresterModeller.src.ProjectManager.miniParser;
using ReactiveUI;

namespace ForresterModeller.src.Windows.ViewModels
{
    /// <summary>
    /// Класс, определяющий поле в окне свойств объекта.
    /// Для каждой строки (как редактируемой, так и нет) необходимо создать
    /// свой экземпляр Property и передать их массив в view
    /// </summary>
    public class PropertyViewModel : ReactiveObject
    {
        /// <summary>
        /// Конструктор для редактируемых полей объекта
        /// </summary>
        /// <param name="name">Имя поля</param>
        /// <param name="value">Значение поля</param>
        /// <param name="updateAction">Метод, обрабатывающий обновление поля на форме</param>
        public PropertyViewModel(String name, String value, Action<String> updateAction, Func<String, Result> validateFunc)
        {
            Name = name;
            _value = value;
            _updateValue = updateAction;
            _validateFunc = validateFunc;
            IsReadOnly = false;
        }
        //public PropertyViewModel(String name, String value, Action<String> updateAction)
        //{
        //    Name = name;
        //    _value = value;
        //    _updateValue = updateAction;
        //    IsReadOnly = false;
        //}
        /// <summary>
        /// Конструктор для нередактируемых полей объекта
        /// </summary>
        /// <param name="name">Имя поля</param>
        /// <param name="value">Значение поля</param>
        public PropertyViewModel(String name, String value)
        {
            Name = name;
            _value = value;
            IsReadOnly = true;
        }
        public string Name { get; set; }
        private Action<String> _updateValue;
        private Func<String, Result> _validateFunc;
        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                if (_validateFunc != null)
                    IsCorrect = _validateFunc(value).result;
                if (IsCorrect)
                    _updateValue(value);
            }
        }
        public bool IsReadOnly { get; set; }
        private bool _isCorrect = true;
        public bool IsCorrect { get => _isCorrect; set => this.RaiseAndSetIfChanged(ref _isCorrect, value); }
    }
}
