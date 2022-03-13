﻿using NodeNetwork.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFtest1.src.Nodes.Models;

namespace WPFtest1.src.Nodes.Views
{
    [TemplatePart(Name = nameof(Code), Type = typeof(TextBlock))]
    [TemplatePart(Name = nameof(FullName), Type = typeof(TextBlock))]

    public class ForesterNodeView : NodeView
    {
        public static readonly DependencyProperty ViewModelForesterProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(ForesterNodeModel), typeof(NodeView), new PropertyMetadata(null));

        public ForesterNodeModel ForesterViewModel
        {
            get => (ForesterNodeModel)GetValue(ViewModelForesterProperty);
            set => SetValue(ViewModelForesterProperty, value);
        }

        private TextBlock Code { get; set; }

        private TextBlock FullName { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            FullName = GetTemplateChild(nameof(FullName)) as TextBlock;

        }

        public ForesterNodeView(string type) : base()
        {
            


            this.WhenActivated(d =>
            {
                this.OneWayBind(ForesterViewModel, vm => vm.FullName, v => v.FullName.Text).DisposeWith(d);
                this.OneWayBind(ForesterViewModel, vm => vm.Code, v => v.Code.Text).DisposeWith(d);
            }
            );

            this.Style = Application.Current.FindResource(type) as Style;
        }
    }
}
