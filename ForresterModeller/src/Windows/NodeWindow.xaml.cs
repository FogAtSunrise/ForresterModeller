﻿using DynamicData;
using NodeNetwork.ViewModels;
using System.Windows;
using WPFtest1.src.Nodes.Models;

namespace WPFtest1
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class NodeWindow : Window
    {
        public NodeWindow()
        {

            InitializeComponent();
            var network = new NetworkViewModel();


            var node1 = new ConstantNodeViewModel();
            network.Nodes.Add(node1);

            var node2 = new LevelNodeModel();
            network.Nodes.Add(node2);


            networkView.ViewModel = network;
        }

        private void networkView_Loaded()
        {

        }
    }
}