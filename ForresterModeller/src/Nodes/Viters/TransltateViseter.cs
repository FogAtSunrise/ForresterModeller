using ForresterModeller.src.Nodes.Models;
using System;
using System.Linq;

namespace ForresterModeller.src.Nodes.Viters
{
    public class TransltateViseter : INodeViseters<string>
    {
        public string VisitChoose(ChouseNodeModel node)
        {
            var preEquals = node.Funk;

            foreach (var inputs in node.Inputs.Items)
            {
                preEquals.Replace(inputs.Name, ((ForesterNodeModel)inputs.Connections.Items.ToList()[0].Input.Parent).Id);
            }

            string translatedNode = String.Format("|f {0} {1}|", node.Id, preEquals);
            return translatedNode;
        }

        public string VisitConstant(ConstantNodeViewModel node)
        {
            var save_localization = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string translatedNode = String.Format("|c {0} {1}|", node.Id, node.Value.ToString());
            System.Threading.Thread.CurrentThread.CurrentCulture = save_localization;
            return translatedNode;
        }

        public string VisitCross(CrossNodeModel node)
        {
            return "";
        }

        public string VisitDelay(DelayNodeModel node)
        {
            var preInEquals = node.InputRate;
            var preStart = node.StartValue;

            foreach (var inputs in node.Inputs.Items)
            {
                preInEquals.Replace(inputs.Name, ((ForesterNodeModel)inputs.Connections.Items.ToList()[0].Input.Parent).Id);
                preStart.Replace(inputs.Name, ((ForesterNodeModel)inputs.Connections.Items.ToList()[0].Input.Parent).Id);
            }

            string translatedNode = String.Format("|d {0} {1} {2} {3}|",
                node.Id,
                node.OutputRateName,
                node.DeepDelay.ToString(),
                node.DelayValueName,
                preInEquals
                );


            var save_localization = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            translatedNode += String.Format("|c {0} {1}|", node.Id, node.DelayValue.ToString());
            System.Threading.Thread.CurrentThread.CurrentCulture = save_localization;
            return translatedNode;
        }

        public string VisitFunc(FunkNodeModel node)
        {
            var preEquals = node.Funk;

            foreach (var inputs in node.Inputs.Items)
            {
                preEquals.Replace(inputs.Name, ((ForesterNodeModel)inputs.Connections.Items.ToList()[0].Input.Parent).Id);
            }

            string translatedNode = String.Format("|h {0} {1}|", node.Id, preEquals);
            return translatedNode;
        }

        public string VisitLevel(LevelNodeModel node)
        {
            var preInEquals = node.InputRate;
            var preOutEquals = node.OutputRate;
            var preStart = node.StartValue;

            foreach (var inputs in node.Inputs.Items)
            {
                preInEquals.Replace(inputs.Name, ((ForesterNodeModel)inputs.Connections.Items.ToList()[0].Input.Parent).Id);
                preOutEquals.Replace(inputs.Name, ((ForesterNodeModel)inputs.Connections.Items.ToList()[0].Input.Parent).Id);
                preStart.Replace(inputs.Name, ((ForesterNodeModel)inputs.Connections.Items.ToList()[0].Input.Parent).Id);
            }

            string translatedNode = String.Format("|l {0} {1} {2} {3}|", node.Id, preInEquals, preOutEquals, preStart);
            return translatedNode;
        }
    }

}
