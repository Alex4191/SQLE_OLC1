using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC1_SQL
{
    class Token
    {
        
        private String Lexl;
        private String Type;
        private int correlative;
        private int row;
        private int column;
        private int index_lex;

        public Token()
        {
            correlative = 0;
            index_lex= 0;
            Lexl = null;
            Type = null;
            row = 0;
            column = 0;
        }
        public Token(String Lexl, String Type, int correlative, int row, int column, int index_lex)
        {
            this.Lexl = Lexl;
            this.Type = Type;
            this.correlative = correlative;
            this.row = row;
            this.column = column;
            this.index_lex = index_lex;
        }

        public int getRow()
        {
            return row;
        }

        public int getColumn()
        {
            return column;
        }
        public String getLexl()
        {
            return Lexl;
        }
        public String getType()
        {
            return Type;

        }
       
        public int GetCorr()
        {
            return correlative;
        }
        public int getIndex_lex()
        {
            return index_lex;
        }

        public void SetRow(int row)
        {
            this.row = row;
        }
        public void setColumn(int column)
        {
            this.column = column;
        }
        
        public void setType(String type)
        {
            this.Type = type;
        }
        public void setLexl(string Lexl)
        {
            this.Lexl = Lexl;
        }

        



        public String Verify_R(String value)

        {
            String val = value;
            val = val.ToUpper();
            switch (val)
            {
                case "CREAR":
                    return "Palabra Reservada";
                case "TABLA":
                    return "Palabra Reservada";
                case "INSERTAR"
:
                    return "Palabra Reservada";
                case "EN":
                    return "Palabra Reservada";
                case "VALORES":
                    return "Palabra Reservada";
                case "SELECCIONAR":
                    return "Palabra Reservada";
                case "DE":
                    return "Palabra Reservada";
                case "COMO":
                    return "Palabra Reservada";
                case "DONDE":
                    return "Palabra Reservada";
                case "Y":
                    return "Palabra Reservada";
                case "O":
                    return "Palabra Reservada";
                case "ELIMINAR":
                    return "Palabra Reservada";
                case "ACTUALIZAR":
                    return "Palabra Reservada";
                case "ESTABLECER":
                    return "Palabra Reservada";
                case "ENTERO":
                    return "Palabra Reservada";
                case "FECHA":
                    return "Palabra Reservada";
                case "CADENA":
                    return "Palabra Reservada";
                case "FLOTANTE":
                    return "Palabra Reservada";
                default:
                    return "Identificador";

            }


        }

        

        public int Verify_Correlative(String value)

        {
            String val = value;
            val = val.ToUpper();
            switch (val)
            {
               
                case "TIPO ENTERO":
                    return 3;
                case "TIPO FECHA":
                    return 4;
                case "TIPO CADENA":
                    return 5;
                case "TIPO FLOTANTE":
                    return 6;
                case "COMMA":
                    return 7;
                case "PUNTO":
                    return 8;
                case "PUNTOCOMMA":
                    return 9;
                case "MENOR":
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
                    return 15;
                case "ASTERISCO":
                    return 16;
                case "PARENTESIS IZQ":
                    return 17;
                case "PARENTESIS DER":
                    return 18;
                case "COMENTARIO":
                    return 19;
                case "COMENTARIO MULTIPLE":
                    return 20;
                case "ENTERO":
                    return 21;
                case "FECHA":
                    return 22;
                case "CADENA":
                    return 23;
                case "FLOTANTE":
                    return 24;
                case "CREAR":
                    return 25;
                case "TABLA":
                    return 26;
                case "INSERTAR":
                    return 27;
                case "EN":
                    return 28;
                case "DE":
                    return 29;
                case "ELIMINAR":
                    return 30;
                case "VALORES":
                    return 31;
                case "DONDE":
                    return 32;
                case "Y":
                    return 33;
                case "O":
                    return 34;
                case "ACTUALIZAR":
                    return 35;
                case "ESTABLECER":
                    return 36;
                case "SELECCIONAR":
                    return 37;
                case "COMO":
                    return 38;
                default:
                    //ULTIMO
                    return 2;

            }


        }



    }

}
