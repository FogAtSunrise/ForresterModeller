using ForresterModeller.src.Nodes.Models;
using NodeNetwork.ViewModels;
using System;
using System.Linq;

namespace ForresterModeller.src.Nodes.Viters
{
    public class TransltateViseter : INodeViseters<string>
    {
        public virtual string VisitChoose(ChouseNodeModel node)
        {
            var preEquals = node.Funk;

            foreach (var inputs in node.Inputs.Items)
            {
                if (inputs.Connections.Items.Any())
                {
                    preEquals = preEquals.Replace(inputs.Name, ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue);
                }
            }

            preEquals = preEquals.Replace(" ", "");

            string translatedNode = String.Format("f {0} {1}|", node.GetCoreCode(), preEquals);
            return translatedNode;
        }

        public virtual string VisitConstant(ConstantNodeViewModel node)
        {
            var save_localization = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string translatedNode = String.Format("c {0} {1}|", node.GetCoreCode(), node.Value.ToString());
            System.Threading.Thread.CurrentThread.CurrentCulture = save_localization;
            return translatedNode;
        }

        public virtual string VisitCross(CrossNodeModel node)
        {
            return "";
        }

        public virtual string VisitDelay(DelayNodeModel node)
        {
            var preInEquals = node.InputRate;
            var preStart = node.StartValue;


            preInEquals = preInEquals.Replace(" ", "");
            preStart = preStart.Replace(" ", "");


            foreach (var inputs in node.Inputs.Items)
            {
                if (inputs.Connections.Items.Count() == 0)
                {
                    inputs.Port.IsInErrorMode = true;
                }
                else
                {
                    preInEquals = ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue;
                }
            }



            string translatedNode = String.Format("d {0} {1} {2} {3} {4} {5}|",
                node.GetCoreCode(),
                node.GetCoreCode() + '_' + node.OutputRateName,
                node.DeepDelay.ToString(),
                node.GetCoreCode() + '_' + node.DelayValueName,
                preInEquals,
                preStart
                );


            var save_localization = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            translatedNode += String.Format("c {0} {1}|", node.GetCoreCode() + '_' + node.DelayValueName, node.DelayValue.ToString());
            System.Threading.Thread.CurrentThread.CurrentCulture = save_localization;
            return translatedNode;
        }

        public virtual string VisitFunc(FunkNodeModel node)
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
                    preEquals = preEquals.Replace(inputs.Name, ((ForesterNodeOutputViewModel)inputs.Connections.Items.First().Output).OutputValue);
                }
            }

            string translatedNode = String.Format("h {0} {1}|", node.GetCoreCode(), preEquals);
            return translatedNode;
        }

        public virtual string VisitLevel(LevelNodeModel node)
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
                    preInEquals = ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue;
                }
            }

            string translatedNode = String.Format("l {0} {1} {2} {3}|", node.GetCoreCode(), preStart, preInEquals, preOutEquals);
            return translatedNode;
        }

        public virtual string VisitLink(LinkNodeModel linkNodeModel)
        {
            var resut = "";
            TransltateViseter translater = new LinkTranslator() { Salt = linkNodeModel.Salt};
            var targets = linkNodeModel.Inputs.Items.Select(a => (LincNodeModelInputRate)a);


            foreach (var node in linkNodeModel.Modegel.Nodes.Items)
            {
                var fNode = (ForesterNodeModel)node;


                string resultNode = fNode.AcceptViseter<string>(translater);

                var tryTarget = targets.FirstOrDefault(a => a.Target.Id == fNode.Id);

                if (tryTarget is not null)
                {
                    if (tryTarget.Connections.Items.Any()) {
                        resultNode = resultNode.Replace(tryTarget.Name, ((ForesterNodeOutputViewModel)tryTarget.Connections.Items.First().Output).OutputValue);
                    }
                }
                resut += resultNode;
            }
            return resut;
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

