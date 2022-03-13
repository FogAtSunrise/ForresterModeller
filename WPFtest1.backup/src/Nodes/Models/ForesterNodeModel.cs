using NodeNetwork.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFtest1.src.Nodes.Models
{
    /// <summary>
    /// Базовая модель узла в схеме форестера
    /// </summary>
    public class ForesterNodeModel : NodeViewModel
    {
        /// <summary>
        /// Полное, осмысленное имя показателя
        /// </summary>
        public string FullName { get; set; }
        

        /// <summary>
        /// Код формулы для вычисления показателя
        /// Код констант дублирует имя
        /// </summary>
        public string Code { get; set; }
        
    }


}
