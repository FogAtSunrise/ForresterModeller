using ForresterModeller.src.Nodes.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.Windows.ViewModels
{
    class MathViewModels : ReactiveObject
    {

        public ObservableCollection<MathViewModel> Models { get; set; } = new();

        //Модель, свойства которой отображаются 
        private IPropertyOwner _activeItem;
        public IPropertyOwner ActiveItem
        {
            get => _activeItem;
            set
            {
                this.RaiseAndSetIfChanged(ref _activeItem, value);
                ActiveItem.OnPropertySelected(_activeItem);
            }
        }
        /*  public ObservableCollection<MathViewModel> _models;
      public ObservableCollection<MathViewModel> Models
          {
          get
          {
              return _models;
          }
          set => this.RaiseAndSetIfChanged(ref _models, value);
      }
        */
        /*
         * 
          <TabControl Grid.Column="0" Name="TabControl" ItemsSource="{Binding TabControlVM.Tabs}"
                           SelectedItem="{Binding TabControlVM.ActiveTab, Mode=TwoWay,  UpdateSourceTrigger=PropertyChanged }" Background="White" Grid.ColumnSpan="2" >
                            <!-- Tab Content Template -->
                            <TabControl.ContentTemplate>
         */
    }
}
