using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_SQL
{
    class Entity_Table
    {
        String Entity;
        int Type;
        ArrayList DataArray;

        public Entity_Table()
        {
            Entity = null;
            Type = 0;
        }
        public Entity_Table(string Entity,int Type, ArrayList DataArray)
        {
            this.Entity = Entity;
            this.Type = Type;
            this.DataArray = DataArray;
        }

        public String getEntity()
        {
            return Entity;
        }

        public int getType()
        {
            return Type;
        }

        public ArrayList getDataArray()
        {
            return DataArray;
        }
        public void setDataArray(ArrayList DataArray)
        {
            this.DataArray = DataArray;
        }
    }
}
