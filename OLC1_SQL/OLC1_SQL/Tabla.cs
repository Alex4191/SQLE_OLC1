using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_SQL
{
    class Tabla
    {
        String Name;
        List <Entity_Table> Columns;
        public Tabla()
        {
            Name = null;
           
        }

        public Tabla(String Name, List<Entity_Table> Columns)
        {
            this.Name=Name;
            this.Columns = Columns;
        }
        public String getName()
        {
            return Name;
        }
        public List<Entity_Table> getList()
        {
            return Columns;
        }
        public void SetList(List<Entity_Table> Columns)
        {
            this.Columns = Columns;
        }
    }
}
