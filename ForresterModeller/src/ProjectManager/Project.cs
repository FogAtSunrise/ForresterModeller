using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForresterModeller.src.ProjectManager
{
    public class Project
    {
        string Name;
        public string getName() { return Name; }
        string PathToFile;

        DateTime CreationDate;
        public DateTime getCreationDate() { return CreationDate; }
        DateTime ChangeDate; 
        public DateTime getChangeDate() { return ChangeDate; }
        public  Project(string name)
        {
            Name = name;
            CreationDate = DateTime.Now;
            ChangeDate= DateTime.Now;
        }
        public Project() { }


}
}
