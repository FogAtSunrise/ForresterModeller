using System.Collections.Specialized;
using System.Windows.Controls;
using ForresterModeller.src.Nodes.Models;
using ForresterModeller.src.Nodes.Views;
using ForresterModeller.src.ProjectManager;
using ForresterModeller.src.ProjectManager.WorkArea;
using NodeNetwork.Toolkit.NodeList;

namespace ForresterModeller.Pages.Tools
{
    /// <summary>
    /// Логика взаимодействия для GraphElements.xaml
    /// </summary>
    public partial class DiagramTools : UserControl
    {
        private NodeListViewModel _nodelistModel;
        private Project _project;

        public DiagramTools(Project project)
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

            _nodelistModel = new();
            _project = project;

            project.Diagrams.CollectionChanged += SetDiagrammNodes;
            foreach (var d in project.Diagrams)
            {
                _nodelistModel.AddNodeType<LinkNodeModel>(() => new LinkNodeModel(project, d.Name));
            }
            DiagranmNodeList.ViewModel = _nodelistModel;
        }

        private void SetDiagrammNodes(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var line in e.NewItems)
                {
                    _nodelistModel.AddNodeType<LinkNodeModel>(() => new LinkNodeModel(_project, ((DiagramManager)line).Name));
                }
            }
        }
    }
}
