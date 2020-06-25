using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_SQL
{
    class Conditions
    {
        String id;
        int condition;
        int group;
        bool cumple;
        String value;
        int value_Type;

        public Conditions()
        {
            id = null;
            condition = 0;
            group = 0;
            cumple = false;
            value = null;
            value_Type = 0;
        }
        public Conditions(String id,int conditions,String value,int value_Type,int group,bool cumple)
        {
            this.id = id;
            this.condition = conditions;
            this.value = value;
            this.value_Type = value_Type;
            this.group = group;
            this.cumple = cumple;
        }

        public string getId()
        {
            return id;
        }
        public int getCondition()
        {
            return condition;
        }
        public int getGroup()
        {
            return group;
        }

        public String getValue()
        {
            return value;
        }

        public int getValue_Type()
        {
            return value_Type;
        }



        public void setId(string id)
        {
            this.id = id;
        }

        public void setConditions(int condition)
        {
            this.condition = condition;
        }

        public void setGroup(int group)
        {
            this.group = group;
        }

        public bool getCumple()
        {
            return cumple;
        }

        public void setCumple(bool cumple)
        {
            this.cumple = cumple;
        }
    }

}
