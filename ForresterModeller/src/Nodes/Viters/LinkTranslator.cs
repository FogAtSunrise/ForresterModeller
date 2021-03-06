using ForresterModeller.src.Nodes.Models;
using System;
using System.Linq;

namespace ForresterModeller.src.Nodes.Viters
{
    public class LinkTranslator: TransltateViseter
    {
        public string Salt { get; set; }

        public override string VisitDelay(DelayNodeModel node)
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
                    preInEquals = ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue + "_" + Salt;
                }
            }

            string translatedNode = String.Format("d {0} {1} {2} {3} {4} {5}|",
                node.GetCoreCode() + "_" + Salt,
                node.GetCoreCode() + '_' + node.OutputRateName + "_" + Salt,
                node.DeepDelay.ToString(),
                node.GetCoreCode() + '_' + node.DelayValueName + "_" + Salt,
                preInEquals,
                preStart
                );

            var save_localization = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            translatedNode += String.Format("c {0} {1}|", node.GetCoreCode() + '_' + node.DelayValueName + "_" + Salt, node.DelayValue.ToString());
            System.Threading.Thread.CurrentThread.CurrentCulture = save_localization;
            return translatedNode;
        }

        public override string VisitChoose(ChouseNodeModel node)
        {
            var preEquals = node.Funk;

            foreach (var inputs in node.Inputs.Items)
            {
                if (inputs.Connections.Items.Any())
                {
                    preEquals = preEquals.Replace(inputs.Name, ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue + '_' + Salt);
                }
            }

            preEquals = preEquals.Replace(" ", "");

            string translatedNode = String.Format("f {0} {1}|", node.GetCoreCode() + "_" + Salt,  preEquals);
            return translatedNode;
        }

        public override string VisitFunc(FunkNodeModel node)
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
                    preEquals = preEquals.Replace(inputs.Name, ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue + '_' + Salt);
                }
            }

            string translatedNode = String.Format("h {0} {1}|", node.GetCoreCode() + "_" + Salt, preEquals);
            return translatedNode;
        }

        public override string VisitLevel(LevelNodeModel node)
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
                    preInEquals = ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue + "_" + Salt;
                }
            }

            string translatedNode = String.Format("l {0} {1} {2} {3}|", node.GetCoreCode() + "_" + Salt, preStart, preInEquals, preOutEquals + "_" + Salt);
            return translatedNode;
        }

        public override string VisitLink(LinkNodeModel linkNodeModel)
        {
            var resut = "";

            var saveId= linkNodeModel.Id;
            linkNodeModel.Id += "_" + Salt;

            TransltateViseter translater = new LinkTranslator() { Salt = linkNodeModel.Salt };
            var targets = linkNodeModel.Inputs.Items.Select(a => (LincNodeModelInputRate)a);


            foreach (var node in linkNodeModel.Modegel.Nodes.Items)
            {
                var fNode = (ForesterNodeModel)node;

                string resultNode = fNode.AcceptViseter<string>(translater);

                var tryTarget = targets.FirstOrDefault(a => a.Target.Id == fNode.Id);

                if (tryTarget is not null)
                {
                    if (tryTarget.Connections.Items.Any())
                    {
                        resultNode = resultNode.Replace(tryTarget.Name + "_" + linkNodeModel.Salt, ((ForesterNodeOutputViewModel)tryTarget.Connections.Items.First().Output).OutputValue + "_" + Salt);
                    }
                }
                resut += resultNode;
            }

            linkNodeModel.Id = saveId;
            return resut;
        }


        public override string VisitConstant(ConstantNodeViewModel node)
        {

            var save_localization = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string translatedNode = String.Format("c {0} {1}|", node.GetCoreCode()  +"_" + Salt, node.Value.ToString());
            System.Threading.Thread.CurrentThread.CurrentCulture = save_localization;
            return translatedNode;
        }

    }

}

