using ForresterModeller.src.Nodes.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.Nodes.Viters
{
    public interface INodeViseters<T>
    {
        public T VisitLevel(LevelNodeModel node);
        public T VisitConstant(ConstantNodeViewModel node);
        public T VisitFunc(FunkNodeModel node);
        public T VisitChoose(ChouseNodeModel node);
        public T VisitCross(CrossNodeModel node);
        public T VisitDelay(DelayNodeModel node);
        T VisitLink(LinkNodeModel linkNodeModel);
    }

}
