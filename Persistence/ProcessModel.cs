using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class ProcessModel
    {
        public string Title { get; private set; }
        public string Name { get; private set; }
        public int Id { get; private set; }

        public ProcessModel(Process p)
        {
            Title = p.MainWindowTitle;
            Name = p.ProcessName;
            Id = p.Id;
        }

        public override string ToString()
        {
            string result = String.Format("({0}, {1}) {2}", Id, Name, Title);
            return result;
        }

        public override bool Equals(object o)
        {
            if (o == null) return false;

            return this.Id == ((ProcessModel)o).Id &&
                this.Name.Equals(((ProcessModel)o).Name) &&
                this.Title.Equals(((ProcessModel)o).Title);
        }

    }
}
