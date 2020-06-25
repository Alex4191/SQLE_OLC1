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
        public ArrayList positions;
        public List<Conditions> condit_Actualizar;
        public List<Conditions> paramet_Update;
        
        int InsertTable;
        int position_D;
        int value_column_insert;
        int contador;
        int contador2;
        int contador3;
        int contador4;
        int contadorVerify;
        bool Flag_Success;
        String name_delete;
        String update_name;
        Tabla BackUp;
       
        public Parser_201602880()
        {
            update_name = null;
            Flag_Success = true;
            Numb_post_analisis = 0;
            post_analisis = null;
            Token_List = new List<Token>();
            Error_List = new List<Token>();
            TableList = new List<Tabla>();
            condit_Actualizar = new List<Conditions>();
            paramet_Update = new List<Conditions>();
            condit = new List<Conditions>();
            positions = new ArrayList();
            contador = 0;
            contador2 = 0;
            contador4 = 0;
           
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
            condit.Clear();   
            match(29);//DE
            name_delete = post_analisis.getLexl();
            match(2);//Identificador
            
            CUERPO();
            match(9);//punto y coma

            //delete with conditions

            Console.WriteLine("SE VAN A INGRESAR CONDICIONES");
            Console.WriteLine(condit.Count);


            Multi_Elimination(condit, contador3);

           
        }

        public void CUERPO()
        {
            if(post_analisis.GetCorr() == 32)//DONDE
            {
                
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
            positions.Clear();
            // condit_Actualizar.Clear();
            // paramet_Update.Clear();
            update_name = Token_List.ElementAt(Numb_post_analisis).getLexl();
            match(2);//Identificador
            match(36);//establecer
            match(17);//parentesis izq

            String id = Token_List.ElementAt(Numb_post_analisis-3).getLexl();
            match(2);//Identificador
            int corr = Token_List.ElementAt(Numb_post_analisis-2).GetCorr();
            match(12);//Igual
            string value_up = Token_List.ElementAt(Numb_post_analisis-1).getLexl();
            VALOR();

            Update_In_Table(id,corr,value_up, value_up);

            VALOR_U();
            CLAUSULA_U();
            match(9);//PUNTO Y COMA

        }
        public void VALOR_U()
        {
            if (post_analisis.GetCorr() == 7)//comma
            {
                match(7);
                String id = Token_List.ElementAt(Numb_post_analisis - 3).getLexl();
                match(2);//Identificador
                int corr = Token_List.ElementAt(Numb_post_analisis - 2).GetCorr();
                match(12);//Igual
                string value_up = Token_List.ElementAt(Numb_post_analisis - 1).getLexl();
                VALOR();

                Update_In_Table(id, corr, value_up, value_up);
                VALOR_U();

            }
            else
            {
                //EPSILON
            }
        }

        public void CLAUSULA_U()
        {
           
            
            match(32);//DONDE
            String id = Token_List.ElementAt(Numb_post_analisis - 3).getLexl();
            match(2);//Identificador
            int corr = Token_List.ElementAt(Numb_post_analisis - 2).GetCorr();
            CONDICION();
            string value_up = Token_List.ElementAt(Numb_post_analisis - 1).getLexl();
            VALOR();
            
                CLAUSULA_AUX();
           
                //epsilon
            
        }

        public void CLAUSULA_AUX()
        {
            if (post_analisis.GetCorr() == 33)
            {
                contador4++;
                match(33);//y
                match(2);
                CONDICION();
                VALOR();
                CLAUSULA_AUX();
            }
            else if (post_analisis.GetCorr() == 34
)
            {
                contador4++;
                match(34);//o
                match(2);
                CONDICION();
                VALOR();
                CLAUSULA_AUX();
            }
            else
            {
                if (contador4 == 0)
                {
                    //simple actualizacion
                }
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
                Console.WriteLine("entro al null, en proceso de tablas html");
                
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
            if (condit.Count != 0)
            {
                if (contador > 0){
                    int contador_grupo=0;
                    Console.WriteLine("ENTROOOOOO");
                    int v = 0;
                    while (v < contador)
                    {
                        for (int j = 0; j < l.Count; j++)
                        {
                          
                            Conditions c_temp = l.ElementAt(v);
                            Conditions c_temp2 = l.ElementAt(j);
                            int group = c_temp.getGroup();
                            int grupo_actual = c_temp2.getGroup();
                            if (grupo_actual >= group)
                            {
                                //positions.Clear();
                                int temp = Verify_Conditions(v, j, l, temp_table);
                                contador_grupo += temp;
                                Console.WriteLine("EL GRUPO VALUADO ES" + temp);
                                
                                List<Entity_Table> lists = temp_table.getList();
                                if (temp!=0)
                                {
                                    Delete_rows(lists);
                                    positions.Clear();
                                    
                                }

                                v++;
                            }
                          

                        }

                    }

                }
                else//NO HAY OR'S
                {

                    
                    int temp = Verify_Conditions(0,l.Count(), l, temp_table);
                        
                        if (temp ==condit.Count)
                        {
                            
                            List<Entity_Table> lists = temp_table.getList();
                            Delete_rows(lists);
                            positions.Clear();
                    }
                        else
                        {
                            Console.WriteLine("No cumplio todas las Y");
                            positions.Clear();
                    }

                    


                }
            }
            else
            {
                //NADA
            }
            
               
                 

            
        }


        /*CONDICIONCNOES*/

        public int Verify_Conditions(int ind,int counter,List<Conditions> c, Tabla temp_table)
        {
            
            int temp_counter=0;
            bool Succes = false;
            Tabla temp = temp_table;

            BackUp = temp;

            if (temp != null)
            {
                for (int index=ind; index <counter ; index++)
                {
                    
                    Conditions c_temp = c.ElementAt(index);
                    Console.WriteLine("se envia" + c_temp.getValue());
                    String identificador = c_temp.getId();
                    int Condicion_valor = c_temp.getCondition();
                    String temp_value = c_temp.getValue();
                    if (Condicion_valor == 12)
                    {
                        temp_counter=Verify_Sub_Conditions_mMi(identificador, Condicion_valor, temp_value, temp, temp_counter);
                    }
                    else if (Condicion_valor == 10)//MENOR
                    {
                        temp_counter = Verify_Sub_Conditions_mMi(identificador, Condicion_valor, temp_value, temp, temp_counter);
                    }
                    else if (Condicion_valor == 11)//MAYOR
                    {
                        temp_counter = Verify_Sub_Conditions_mMi(identificador, Condicion_valor, temp_value, temp, temp_counter);

                    }else if (Condicion_valor == 13)
                    {
                        temp_counter = Verify_Sub_Conditions_mMi(identificador, Condicion_valor, temp_value, temp, temp_counter);
                    }
                    else if(Condicion_valor == 14)
                    {
                        temp_counter = Verify_Sub_Conditions_mMi(identificador, Condicion_valor, temp_value, temp, temp_counter);

                    }
                    else if (Condicion_valor == 15)
                    {
                        temp_counter = Verify_Sub_Conditions_mMi(identificador, Condicion_valor, temp_value, temp, temp_counter);
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
                            if (c==9) {

                                if (t == ar[j].ToString())
                                {
                                    position_D = j;
                                    contadorVerify++;
                                    return temp = true;

                                }
                            }
                            
                        }
                       
                        
                    }
                    break;
                }
               
            }
            return temp;
        }



        public int Verify_Sub_Conditions_mMi(String id, int c, String t, Tabla tmp_t,int con)
        {
            int contador=con;
            if (positions.Count != 0)
            {
                Console.WriteLine("paso por aca prro");
                List<Entity_Table> ls = tmp_t.getList();
                for (int i = 0; i < ls.Count; i++)
                {
                    Entity_Table entity_t = ls.ElementAt(i);
                    if (entity_t.getEntity().ToUpper() == id.ToUpper())
                    {
                        ArrayList ar = entity_t.getDataArray();
                        int postion =int.Parse(positions[0].ToString());
                        if (ar[postion].Equals(t)){

                            Console.WriteLine("EL sdflkasdklajsdkl;R");
                            contador++;
                            break;
                            
                        }
                    }
                    
                }
               
            }
            else
            {
                try
                {
                    ArrayList ar=null;
                    List<Entity_Table> ls = tmp_t.getList();
                    for (int i = 0; i < ls.Count; i++)
                    {
                        Entity_Table entity_t = ls.ElementAt(i);
                        if (entity_t.getEntity().ToUpper() == id.ToUpper())
                        {
                            ar = entity_t.getDataArray();
                          
                            break;
                        }

                    }
                    if (ar != null)
                    {
                        for (int j = 0; j < ar.Count; j++)
                        {

                            if (ar[j].ToString() != "")
                            {
                                if (c == 10)//MENOR
                                {

                                    if (int.Parse(ar[j].ToString()) < int.Parse(t))
                                    {
                                        positions.Add(j);
                                        contador++;
                                        break;
                                    }
                                }
                                else if (c == 11)
                                {//MAYOR
                                    if (int.Parse(ar[j].ToString()) > int.Parse(t))
                                    {
                                        positions.Add(j);
                                        contador++;
                                        break;
                                    }
                                }
                                else if (c == 12)//IGUAL
                                {
                                    if (ar[j].ToString().ToUpper() == t.ToUpper())
                                    {
                                        Console.WriteLine("su cumplioi" + t);
                                        positions.Add(j);
                                        contador++;

                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("su cumplioi" + t);
                                    }
                                }
                                else if (c == 13)//MAYOR IGUAL
                                {
                                    if (int.Parse(ar[j].ToString()) >= int.Parse(t))
                                    {
                                        positions.Add(j);
                                        contador++;
                                        break;
                                    }
                                }
                                else if (c == 14)//MENOR IGUAL
                                {
                                    if (int.Parse(ar[j].ToString()) <= int.Parse(t))
                                    {
                                        positions.Add(j);
                                        contador++;
                                        break;

                                    }
                                }
                                else if (c == 15)//DISTINTO
                                {
                                    if (int.Parse(ar[j].ToString()) != int.Parse(t))
                                    {
                                        positions.Add(j);
                                        contador++;
                                        break;

                                    }
                                }
                                else
                                {
                                   
                                    //NADA
                                }

                            }


                        }

                    }



                }
                catch (System.FormatException)
                {

                }


                
            }

            return contador;

        }



        public void Delete_rows(List<Entity_Table> ls)
        {
            for (int z = 0; z <positions.Count; z++)
            {

                for (int i = 0; i < ls.Count; i++)
                {
                    Entity_Table entity_t = ls.ElementAt(i);
                    ArrayList ar = entity_t.getDataArray();
                    for (int j = 0; j < ar.Count; j++)
                    {
                        if (positions[z].Equals(j))
                        {
                            ar[j] = "";
                        }
                    }
                }

            } 
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


        /*ACTUALIZAR*/


        public void Update_In_Table(String Identificador, int correlativo, String valor, String name)
        {
           paramet_Update.Add(new Conditions(Identificador,correlativo,valor,0,0,false));
        }

        public void getCondition_Update(String Identificador, int correlativo, String valor, String name)
        {
            condit_Actualizar.Add(new Conditions(Identificador, correlativo, valor, 0, 0, false));
        }

        public void  verifyUpdate()
        {
            
                Tabla temp_table = GetTableName(name_delete);
            
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
