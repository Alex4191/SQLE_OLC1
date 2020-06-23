using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_SQL
{
    class Paths
    {
        private string path;
        private String Name;

        public Paths(string path, String Name)
        {

            this.path = path;
            this.Name = Name;
        }
        public string getPath()
        {
            return this.path;
        }
        public String getName()
        {
            return this.Name;
        }
    }
}
