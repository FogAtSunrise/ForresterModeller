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

        public  virtual string VisitDelay(DelayNodeModel node)
        {
            var preInEquals = node.InputRate;
            var preStart = node.StartValue;


            preInEquals = preInEquals.Replace(" ", "");
            preStart = preStart.Replace(" ", "");

            //TODO prestat find

            string translatedNode = String.Format("d {0} {1} {2} {3} {4} {5}|",
                node.GetCoreCode(),
                node.OutputRateName,
                node.DeepDelay.ToString(),
                node.DelayValueName,
                preInEquals,
                preStart
                );


            var save_localization = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            translatedNode += String.Format("c {0} {1}|", node.DelayValueName, node.DelayValue.ToString());
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
                    preEquals = preEquals.Replace(inputs.Name, ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue);
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

            string translatedNode = String.Format("l {0} {1} {2} {3}|", node.GetCoreCode(), preInEquals, preOutEquals, preStart);
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

    public class LinkTranslator: TransltateViseter
    {
        public string Salt { get; set; }

        public override string VisitDelay(DelayNodeModel node)
        {
            var preInEquals = node.InputRate;
            var preStart = node.StartValue;

            preInEquals = preInEquals.Replace(" ", "");
            preStart = preStart.Replace(" ", "");

            //TODO prestat find

            string translatedNode = String.Format("d {0} {1} {2} {3} {4} {5}|",
                node.GetCoreCode() + Salt,
                node.OutputRateName + Salt,
                node.DeepDelay.ToString(),
                node.DelayValueName + Salt,
                preInEquals,
                preStart
                );


            var save_localization = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            translatedNode += String.Format("c {0} {1}|", node.DelayValueName + Salt, node.DelayValue.ToString());
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
                    preEquals = preEquals.Replace(inputs.Name, ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue + Salt);
                }
            }

            preEquals = preEquals.Replace(" ", "");

            string translatedNode = String.Format("f {0} {1}|", node.GetCoreCode() + Salt, preEquals);
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
                    preEquals = preEquals.Replace(inputs.Name, ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue + Salt);
                }
            }

            string translatedNode = String.Format("h {0} {1}|", node.GetCoreCode() + Salt, preEquals);
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
                    preInEquals = ((ForesterNodeOutputViewModel)inputs.Connections.Items.ToList()[0].Output).OutputValue + Salt;
                }
            }

            string translatedNode = String.Format("l {0} {1} {2} {3}|", node.GetCoreCode() + Salt, preInEquals + Salt, preOutEquals + Salt, preStart);
            return translatedNode;
        }

        public override string VisitLink(LinkNodeModel linkNodeModel)
        {
            var resut = "";

            var saveId= linkNodeModel.Id;
            linkNodeModel.Id += Salt;

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
                        resultNode = resultNode.Replace(tryTarget.Name + linkNodeModel.Salt, ((ForesterNodeOutputViewModel)tryTarget.Connections.Items.First().Output).OutputValue + Salt);
                    }
                }
                resut += resultNode;
            }

            linkNodeModel.Id = saveId;
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

