using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1_SQL
{
    class Parser_201602880
    {
        public int Numb_post_analisis;
        public Token post_analisis;
        public List<Token> Token_List;
        public List<Token> Error_List;
        public List<Tabla> TableList;
        public List<Conditions> condit;
        int InsertTable;
        int position_D;
        int value_column_insert;
        int contador;
        int contador2;
        int contador3;
        int contadorVerify;
        bool Flag_Success;
        String name_delete;
        Tabla BackUp;
        public Parser_201602880()
        {
            Flag_Success = true;
            Numb_post_analisis = 0;
            post_analisis = null;
            Token_List = new List<Token>();
            Error_List = new List<Token>();
            TableList = new List<Tabla>();
            contador = 0;
            contador2 = 0;
           
        }
        

        public void Parser(List<Token> listT,List<Token> Err)
        {
            Error_List = Err;
            Token_List = listT;
            post_analisis = Token_List.ElementAt(0);
            Numb_post_analisis = 0;
            START();
        }

        public void START()
        {
            
            if (post_analisis.GetCorr() == 25)//CREAR
            {
                contador++;
                match(25);//crear
                S0();
                START();
               

            }
            else if (post_analisis.GetCorr()==27)
            {
                contador2++;
                match(27);//INSERTAR
                S1();
                START();
            }
            else if (post_analisis.GetCorr()==30)
            {


                contadorVerify = 0;
                match(30);//Eliminar
                S2();
               
                START();
            }
            else if (post_analisis.GetCorr()==35)
            {
                match(35);//ACTUALIZAR
                S3();
               
                START();
               
            }else if (post_analisis.GetCorr()==37)
            {
                match(37);//SELECCIONAR
                S4();
                
                START();
            }
            else
            {
                Console.WriteLine("___________");
                Console.WriteLine("analisis SINTACTICO FINALIZADO CORRECTAMENTE");

                /**EPSILON*/
            }
                
        }

        public void S0()
        {
            
            match(26);// tabla
            CreateTable();
            match(2);//Identificador
            match(17);//parentesis izq
            SENTENCIA();
            SENTENCIA_R();
            match(18);//parentesis der
            match(9);// punto y comma

        }
        public void SENTENCIA()
        {

            Console.Write("BRRRRRRRRRRR");
            match(2);//Identificador
            TIPO();

            CreateColumns();
            
            
        }
        public void SENTENCIA_R()
        {
            if(post_analisis.GetCorr() == 7)//comma
            {
                match(7);
                SENTENCIA();
                SENTENCIA_R();
            }
        }
        public void TIPO()
        {
            if (post_analisis.GetCorr()==21)//entero
            {
                match(21);
            }
            else if (post_analisis.GetCorr() == 22)//fecha
            {
                match(22);
            }
            else if (post_analisis.GetCorr() == 23)//cadena
            {
                match(23);
            }
            else 
            {
                match(24);//flotante
            }
                
        }

        /*insert methods*/
        public void S1()
        {
            match(28);//en

            value_column_insert = 0;
            GetTableInsert(post_analisis.getLexl());

            match(2);//Identificador
            match(31);//Valores
            match(17);//parentesis izq

            insert_In_Table(InsertTable, value_column_insert, post_analisis.getLexl()); 

            VALOR();
            INGRESO();
            match(18);//parentesis der
            match(9);//punto y coma
        }

        public void INGRESO()
        {
            if (post_analisis.GetCorr() == 7)//comma
            {
                value_column_insert = value_column_insert+1;

                match(7);

                insert_In_Table(InsertTable, value_column_insert, post_analisis.getLexl());

                VALOR();
                INGRESO();
            }
        }



        /*ELIMINAR METHODS*/
        public void S2()
        {
            match(29);//DE
            name_delete = post_analisis.getLexl();
            match(2);//Identificador
            
            CUERPO();
            match(9);//punto y coma

            //delete with conditions
            Multi_Elimination(condit, contador3);

        }

        public void CUERPO()
        {
            if(post_analisis.GetCorr() == 32)//DONDE
            {
                condit = new List<Conditions>();
                contador3 = 0;
                match(32);
                SUB_CUERPO();
                

            }
            else
            {
                
                Single_Elimination(Token_List.ElementAt(Numb_post_analisis - 1).getLexl());
                //EPSILON
            }
        }

        public void SUB_CUERPO()
        {
           
                match(2);//Identificador 
                //SUB_CUERPO2();
                CONDICION();
                VALOR();


                condit.Add(new Conditions(Token_List.ElementAt(Numb_post_analisis-3).getLexl(), 
                Token_List.ElementAt(Numb_post_analisis-2).GetCorr(), 
                Token_List.ElementAt(Numb_post_analisis-1).getLexl(),Token_List.ElementAt(Numb_post_analisis-1).GetCorr(), contador3,false));

           
                SUB_OPERADOR();
                
        }
        
        public void SUB_OPERADOR()
        {
            if (post_analisis.GetCorr() == 33)// y
            {
                match(33);
                SUB_CUERPO();
                SUB_OPERADOR();
            }
            else if (post_analisis.GetCorr() == 34)//O
            {
                contador3++;
                match(34);
                SUB_CUERPO();
                SUB_OPERADOR();
            }
            else
            {
                //EPSILON
            }
        }
        public void SUB_CUERPO2()
        {
            if (post_analisis.GetCorr() == 8)
            {
                match(8);//punto
                match(2);//Identificador
            }
            else
            {
                //EPSILON
            }
        }


        /*ACTUALIZAR METHODS*/
        public void S3()
        {
            match(2);//Identificador
            match(36);//establecer
            match(17);//parentesis izq
            match(2);//Identificador
            match(12);//Igual
            VALOR();
            VALOR_U();
            CLAUSULA_U();
            match(9);//PUNTO Y COMA

        }
        public void VALOR_U()
        {
            if (post_analisis.GetCorr() == 7)//comma
            {
                match(7);
                match(2);//Identificador
                match(12);//Igual
                VALOR();
                VALOR_U();

            }
            else
            {
                //EPSILON
            }
        }

        public void CLAUSULA_U()
        {
            if (post_analisis.GetCorr()==32)//donde
            {
                match(32);
                match(2);//Identificador
                CONDICION();
                VALOR();
                CLAUSULA_AUX();
            }
            else
            {
                //epsilon
            }
        }

        public void CLAUSULA_AUX()
        {
            if (post_analisis.GetCorr() == 33)
            {
                match(33);//y
                match(2);
                CONDICION();
                VALOR();
                CLAUSULA_AUX();
            }
            else if (post_analisis.GetCorr() == 34
)
            {
                match(34);//o
                match(2);
                CONDICION();
                VALOR();
                CLAUSULA_AUX();
            }
            else
            {
                //EPSILON
            }
        }


        /*SELECCIONAR METHODS*/
        public void S4()
        {
            ORIGEN();
            match(29);//DE
            match(2);//Identificador
            SUB_TABLA();
            LOCALIDAD();
            match(9);//[PUNTO Y COMA
        }

        public void ORIGEN()
        {
            if (post_analisis.GetCorr()==2)
            {
                match(2);//Identificador
                match(8);//punto
                match(2);//Identificador
                ALIAS();
                SUB_ORIGEN();
            }
            else
            {
                match(16);//ASTERISCO
            }
        }

        public void ALIAS()
        {
            if (post_analisis.GetCorr() == 38)
            {
                match(38);//como
                match(2);//identificador
            }
            else
            {
                //EPSILON
            }
        }

        public void SUB_ORIGEN()
        {
            if (post_analisis.GetCorr()==7)
            {
                match(7);//comma
                match(2);//Identificador
                match(8);//punto
                match(2);//Ideificador
                ALIAS();
                SUB_ORIGEN();
            }
            else
            {
                //EPSILON
            }
        }

        public void SUB_TABLA()
        {
            if (post_analisis.GetCorr() == 7)
            {
                match(7);//comma
                match(2);
                SUB_TABLA();
            }
            else
            {
                //EPSILON
            }
        }

        public void LOCALIDAD()
        {
            if (post_analisis.GetCorr()==32)
            {
                match(32);//donde
                REGLA();
            }
        }

        public void REGLA()
        {
            match(2);
            CONDICION();
            VALOR();
            REGLA_AUX();
        }

        public void REGLA_AUX()
        {
            if (post_analisis.GetCorr() == 33)
            {
                match(33);//y
                match(2);
                CONDICION();
                VALOR();
                REGLA_AUX();
            }else if (post_analisis.GetCorr() == 34)
            {
                match(34);//o
                match(2);
                CONDICION();
                VALOR();
                REGLA_AUX();
            }
            else
            {
                //EPSILON
            }
        }

        public void VALOR()
        {
            if(post_analisis.GetCorr() == 3)//Tipo Entero
            {
                match(3);
            }else if (post_analisis.GetCorr() == 4)//Tipo Fecha
            {
                match(4);
            }else if(post_analisis.GetCorr() == 5)//tipo Cadena
            {
                match(5);
            }
            else
            {
                match(6);//tipo Flotante
            }
        }

        public void CONDICION()
        {
            if (post_analisis.GetCorr() == 12)//IGUAL
            {
                match(12);
            }else if (post_analisis.GetCorr()==15)//dIFERENTE
            {
                match(15);
            }else if (post_analisis.GetCorr()==11)//Mayor
            {
                match(11);
            }else if (post_analisis.GetCorr()==10)//Menor
            {
                match(10);
            }else if (post_analisis.GetCorr()==13)//Mayor igual
            {
                match(13);
            }else //Menor Igual
            {
                match(14);
            }
        }


        public void match(int tipo)
        {
            
            if (tipo != post_analisis.GetCorr())
            {
                Flag_Success = false;
               
                if (Numb_post_analisis < Token_List.Count())
                {
                   
                    Error_List.Add(new Token("Sintactico", getErrorType(tipo) + " " + "Y se encontro" + " " + getErrorType(post_analisis.GetCorr()), -1, post_analisis.getRow(),post_analisis.getColumn(),0));
                    PanicMode(Numb_post_analisis);
                }

                 
               
                
            }
            if (post_analisis.GetCorr()!=0)
            {

                Numb_post_analisis=Numb_post_analisis+1;
                if (Numb_post_analisis < Token_List.Count()-1)
                {
                    post_analisis = Token_List.ElementAt(Numb_post_analisis);
                }
               
            }
        }

        public void PanicMode(int counter)
        {
            if (counter < Token_List.Count() - 1)
            {
                Token temp = post_analisis;

                temp = Token_List.ElementAt(counter);
                while (counter < Token_List.Count)//ultimo
                {


                    if (temp.GetCorr() == 9)
                    {
                        post_analisis = temp;
                        Numb_post_analisis = counter;
                        match(9);
                        START();
                        break;
                    }
                    counter += 1;
                    temp = Token_List.ElementAt(counter);

                }
            }
            
                
               
        }

        /*CREATE TABLES METHODS*/
        public void CreateTable()
        {
            if (post_analisis.GetCorr()==2 && Flag_Success==true)
            {
                TableList.Add(new Tabla(post_analisis.getLexl(),null));
            }
        }

        public void CreateColumns() { 
            if (Flag_Success == true)
            {
                String Column = Token_List.ElementAt(Numb_post_analisis-2).getLexl();
                int Type = Token_List.ElementAt(Numb_post_analisis-1).GetCorr();
                Tabla t = TableList.ElementAt(contador-1);
                if (t.getList()==null)
                {
                    List<Entity_Table> Columns = new List<Entity_Table>();
                    Columns.Add(new Entity_Table(Column, Type,null));
                    t.SetList(Columns);
                   
                }
                else
                {
                    List<Entity_Table> Columns = t.getList();
                    Columns.Add(new Entity_Table(Column, Type,null));
                    t.SetList(Columns);
                }
                //Entity_Table tb = TableList.ElementAt(contador).getList();
            }
        }

        /*INSERT DATA IN TABLES METHODS*/

        public void GetTableInsert(String tableName)
        {
            
            if (Flag_Success == true)
            {
                for (int i = 0; i < TableList.Count; i++)
                {
                    Tabla t = TableList.ElementAt(i);
                    if (t.getName().ToUpper()==tableName.ToUpper())
                    {
                        InsertTable = i;
                        break;
                    }
                }

            }
        }

        public void insert_In_Table(int value, int column_value,String valueD)
        {
            if (Flag_Success != false)
            {
                try
                {
                    Tabla t_insert = TableList.ElementAt(value);
                    Entity_Table tb = t_insert.getList().ElementAt(column_value);
                    if (tb.getDataArray() == null)
                    {
                        ArrayList ColumnData = new ArrayList();
                        ColumnData.Add(valueD);
                        tb.setDataArray(ColumnData);
                    }
                    else
                    {
                        ArrayList Column_Data = tb.getDataArray();
                        Column_Data.Add(valueD);
                        tb.setDataArray(Column_Data);
                    }
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("No se encontro la tabla a insertar");
                }
            }
            
            
                
        }


        public void ImprimirTablas()
        {
            String html_Content;
            String Cabezera_Html;
            String Title_Table="";
            String Content_Celda="";
            String Content_Table = "";
            Cabezera_Html = "<html>" +
            "<link rel=\"stylesheet\" type=\"text/css\" href=\"bootstrap.css\">" +
            "<body>";
            for (int i = 0; i < TableList.Count; i++)
            {
                String Columns_Table ="";
                Tabla t = TableList.ElementAt(i);
                Title_Table ="</br>"+ "<h2 align='center'>" +"Tabla"+" "+t.getName() +"</h2></br>" + 
                "<table cellpadding='10' border = '1' align='center'>";
                for (int j = 0; j < t.getList().Count; j++)
                {
                   
                    Entity_Table ts = t.getList().ElementAt(j);
                    Columns_Table += 
                    "<td><strong>"+ ts.getEntity()+
                    "</strong></td>";
                    
                }

                if (t.getList()!= null)
                {
                    
                    Content_Celda = tx(t);
                }
                
                Content_Table += Title_Table  + Columns_Table + "</tr>"+ Content_Celda
                 + "</table>";
           
            }
            html_Content= Cabezera_Html+Content_Table+ "</body>" +
            "</html>";
            File.WriteAllText("Tables.html", html_Content);
            System.Diagnostics.Process.Start("Tables.html");

        }


        public String tx(Tabla t)
        {
            String Content = "";
            try
            {
                Tabla temp = t;
                Entity_Table columnas = temp.getList().ElementAt(0);
                ArrayList lis = columnas.getDataArray();
               
                String Values_list;
                for (int lis_index = 0; lis_index < lis.Count; lis_index++)
                {
                    Values_list = "";
                    for (int col_index = 0; col_index < temp.getList().Count; col_index++)
                    {
                        Entity_Table Entity_temp = temp.getList().ElementAt(col_index);
                        ArrayList ls = Entity_temp.getDataArray();
                        Values_list += "<td>" + ls[lis_index] + "</td>";


                        // Console.WriteLine(ls[lis_index]);
                    }
                    Content += "<tr>" + Values_list + "</tr>";
                }
                
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("entro al null");
                
            }
            return Content;
        }
        
        /*ELIMINATION METHDOS*/
        public void Single_Elimination(String name)
        {
            if (Flag_Success == true)
            {
                GetTableInsert(name);
                Tabla t = TableList.ElementAt(InsertTable);
                for (int i = 0; i < t.getList().Count; i++)
                {
                    Entity_Table entity_temp = t.getList().ElementAt(i);
                    entity_temp.setDataArray(null);
                }
            }
        }

        public Tabla GetTableName(String name)
        {
            GetTableInsert(name);
            Tabla t = TableList.ElementAt(InsertTable);
            return t;
        }
       
        public void Multi_Elimination(List<Conditions> l,int contador)
        {

            Tabla temp_table = GetTableName(name_delete);

            if (contador > 0)
            {

            }
            else//NO HAY OR'S
            {
                if (l.Count == 1){
                    int temp = Verify_Conditions(l.Count(), l, temp_table);
                    if (temp > 0)
                    {
                        Delete_Row(position_D, temp_table);
                        position_D = 0;
                    }
                }
                else
                { 
                   int temp = Verify_Conditions(l.Count(), l, temp_table);
                        
                    if (temp == l.Count())
                        {
                        Delete_Row(position_D, temp_table);//call's a delete Row method
                            position_D = 0;
                        }
                        else
                        {
                            Console.WriteLine("No cumplio todas las Y");
                           
                        }

                }
                

            }    
               
                 

            
        }


        public int Verify_Conditions(int counter,List<Conditions> c, Tabla temp_table)
        {
            
            int temp_counter=0;
            bool Succes = false;
            Tabla temp = temp_table;

            BackUp = temp;

            if (temp != null)
            {
                for (int i = 0; i <counter ; i++)
                {
                    
                    Conditions c_temp = c.ElementAt(i);
                    Console.WriteLine("se envia" + c_temp.getValue());
                    String identificador = c_temp.getId();
                    int Condicion_valor = c_temp.getCondition();
                    String temp_value = c_temp.getValue();
                    if (Condicion_valor == 12)
                    {/*case "MENOR":
                    return 10;
                case "MAYOR":
                    return 11;
                case "IGUAL":
                    return 12;
                case "MAYOR IGUAL":
                    return 13;
                case "MENOR IGUAL":
                    return 14;
                case "DISTINTO":
                    return 15;*/

                        Succes = Verify_Sub_Conditions_Equals(identificador, Condicion_valor, temp_value,temp);
                        if (Succes == true)
                        {
                            temp_counter++;
                        }
                    }else if (Condicion_valor == 13)
                    {

                    }
                    
                }
               
            }
            return temp_counter;
        }



        public bool Verify_Sub_Conditions_Equals(String id, int c, String t,Tabla tmp_t) {
          
            bool temp = false;
           
            List<Entity_Table> ls = tmp_t.getList();
            for (int i = 0; i < ls.Count; i++)
            {
                Entity_Table entity_t = ls.ElementAt(i);
                if (entity_t.getEntity().ToUpper() == id.ToUpper())
                {
                    ArrayList ar = entity_t.getDataArray();
                    for (int j = 0; j < ar.Count; j++)
                    {
                        if (contadorVerify != 0)
                        {
                            if (t == ar[position_D].ToString())
                            {

                                return temp = true;
                            }
                            else
                            {
                                return temp = false;
                            }
                        }
                        else
                        {
                            if (t == ar[j].ToString())
                            {
                                position_D = j;
                                contadorVerify++;
                                return temp = true;

                            }
                        }
                       
                        
                    }
                    break;
                }
               
            }
            return temp;
        }
        

        public void Delete_Row(int D_positions,Tabla tmp_t)
        {
            List<Entity_Table> tb = tmp_t.getList();
            for (int i = 0; i < tb.Count; i++)
            {
                Entity_Table entity = tb.ElementAt(i);
                ArrayList a_temp = entity.getDataArray();
                a_temp[D_positions] = null;
            }

        }


        public String getErrorType(int p)
        {
            switch (p)
            {
                case 2:
                    return "IDENTIFICADOR";
                case 3:
                    return "TIPO ENTERO";
                case 4:
                    return "TIPO FECHA";
                case 5:
                    return "TIPO CADENA";
                case 6:
                    return "TIPO FLOTANTE";
                case 7:
                    return "COMMA";
                case 8:
                    return "PUNTO";
                case 9:
                    return "PUNTOCOMMA";
                case 10:
                    return "MENOR";
                case 11:
                    return "MAYOR";
                case 12:
                    return "IGUAL";
                case 13:
                    return "MAYOR IGUAL";
                case 14:
                    return "MENOR IGUAL";
                case 15:
                    return "DISTINTO";
                case 16:
                    return "ASTERISCO";
                case 17:
                    return "PARENTESIS IZQ";
                case 18:
                    return "PARENTESIS DER";
                case 19:
                    return "COMENTARIO";
                case 20:
                    return "COMENTARIO MULTIPLE";
                case 21:
                    return "ENTERO";
                case 22:
                    return "FECHA";
                case 23:
                    return "CADENA";
                case 24:
                    return "FLOTANTE";
                case 25:
                    return "CREAR";
                case 26:
                    return "TABLA";
                case 27:
                    return "INSERTAR";
                case 28:
                    return "EN";
                case 29:
                    return "DE";
                case 30:
                    return "ELIMINAR";
                case 31:
                    return "VALORES";
                case 32:
                    return "DONDE";
                case 33:
                    return "Y";
                case 34:
                    return "O";
                case 35:
                    return "ACTUALIZAR";
                case 36:
                    return "ESTABLECER";
                case 37:
                    return "SELECCIONAR";
                case 38:
                    return "COMO";
                default:
                    //ULTIMO
                    return "ULTIMO";

            }
        }
    }
}
