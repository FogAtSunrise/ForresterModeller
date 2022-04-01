using ForresterModeller.src.Nodes.Models;
using NodeNetwork.ViewModels;
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
                preEquals = preEquals.Replace(inputs.Name, ((ForesterNodeModel)inputs.Connections.Items.ToList()[0].Output.Parent).GetCoreCode());
            }

            preEquals = preEquals.Replace(" ","");

            string translatedNode = String.Format("f {0} {1}|", node.GetCoreCode(), preEquals);
            return translatedNode;
        }

        public string VisitConstant(ConstantNodeViewModel node)
        {
            var save_localization = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string translatedNode = String.Format("c {0} {1}|", node.GetCoreCode(), node.Value.ToString());
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


            preInEquals = preInEquals.Replace(" ", "");
            preStart = preStart.Replace(" ", "");

            //TODO prestat find

            string translatedNode = String.Format("d {0} {1} {2} {3}|",
                node.GetCoreCode(),
                node.OutputRateName,
                node.DeepDelay.ToString(),
                node.DelayValueName,
                preInEquals
                );


            var save_localization = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            translatedNode += String.Format("c {0} {1}|", node.GetCoreCode(), node.DelayValue.ToString());
            System.Threading.Thread.CurrentThread.CurrentCulture = save_localization;
            return translatedNode;
        }

        public string VisitFunc(FunkNodeModel node)
        {
            var preEquals = node.Funk;
            preEquals = preEquals.Replace(" ", "");

            foreach (var inputs in node.Inputs.Items)
            {
                if (inputs.Connections.Items.Count() == 0)
                {
                    inputs.Port.IsInErrorMode = true;
                }
                else
                {
                    preEquals = preEquals.Replace(inputs.Name, ((ForesterNodeModel)inputs.Connections.Items.ToList()[0].Output.Parent).GetCoreCode());
                }
            }

            string translatedNode = String.Format("h {0} {1}|", node.GetCoreCode(), preEquals);
            return translatedNode;
        }

        public string VisitLevel(LevelNodeModel node)
        {
            var preInEquals = node.InputRate;
            var preOutEquals = node.OutputRate;
            var preStart = node.StartValue;


            preInEquals = preInEquals.Replace(" ", "");
            preOutEquals = preOutEquals.Replace(" ", "");
            preStart = preStart.Replace(" ", "");


            foreach (var inputs in node.Inputs.Items)
            {
                if (inputs.Connections.Items.Count() == 0)
                {
                    inputs.Port.IsInErrorMode = true;
                }
                else
                {
                    preInEquals =  ((ForesterNodeModel)inputs.Connections.Items.ToList()[0].Output.Parent).GetCoreCode();
                }
            }

            string translatedNode = String.Format("l {0} {1} {2} {3}|", node.GetCoreCode(), preInEquals, preOutEquals, preStart);
            return translatedNode;
        }
    }


    public class NodeTranslator{
        public static string Translate(NetworkViewModel model)
        {
            var result = "";
            var viseter = new TransltateViseter();
            foreach (ForesterNodeModel node in model.Nodes.Items)
            {
                result += node.AcceptViseter<string>(viseter);
            }
            return result.Remove(result.Length - 1);
        }
    }

}

