using System;
using System.Threading;
using System.Windows.Controls;
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
        public PropertyViewModel(String name, String value, Action<String> updateAction,
            Func<String, Result> validateFunc)
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
        private string _msg;
        public string Message { get => _msg; set => this.RaiseAndSetIfChanged(ref _msg, value); }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                if (_validateFunc != null)
                {
                    var res = _validateFunc(value);
                    IsCorrect = res.result;
                    Message = res.str;
                }
                if (IsCorrect)
                {
                    _updateValue(value);
                    Message = "";
                }
            }
        }
        public bool IsReadOnly { get; set; }
        private bool _isCorrect = true;
        public bool IsCorrect { get => _isCorrect; set => this.RaiseAndSetIfChanged(ref _isCorrect, value); }
    }
}
