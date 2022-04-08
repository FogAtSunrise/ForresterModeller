using System.Windows.Controls;
using ForresterModeller.src.Nodes.Models;
using NodeNetwork.Toolkit.NodeList;

namespace ForresterModeller.Pages.Tools
{
    /// <summary>
    /// Логика взаимодействия для GraphElements.xaml
    /// </summary>
    public partial class DiagramTools : UserControl
    {
        public DiagramTools()
        {
            InitializeComponent();

            var nodelistModel = new NodeListViewModel();
            nodelistModel.AddNodeType<ConstantNodeViewModel>(() => new ConstantNodeViewModel());
            nodelistModel.AddNodeType<LevelNodeModel>(() => new LevelNodeModel());
            nodelistModel.AddNodeType<DelayNodeModel>(() => new DelayNodeModel());
            nodelistModel.AddNodeType<CrossNodeModel>(() => new CrossNodeModel());
            nodelistModel.AddNodeType<ChouseNodeModel>(() => new ChouseNodeModel());
            nodelistModel.AddNodeType<FunkNodeModel>(() => new FunkNodeModel());
            equasionNodeList.ViewModel = nodelistModel;


        }
    }
}
