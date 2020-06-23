using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1_SQL
{
    
    class Scanner_201602880
    {
        private int state;
        private String AuxLex;
        public List<Token> TokensArray;
        public List<Token> Errors;
        public Token tk;
        public Scanner_201602880()
        {

            state = 0;
            TokensArray = new List<Token>();
            Errors = new List<Token>();
            tk = new Token();
        }

        public void Analyze(String Data_Text)
        {
             
            int row = 0;
            int column = 0;
            char chain;
            int AscciCode;
            AuxLex = "";
            Data_Text = Data_Text + " ";
            MessageBox.Show(Data_Text, "asdfa");
            for (int i = 0; i < Data_Text.Length; i++)
            {
                chain = Data_Text.ElementAt(i);
                
                AscciCode = chain;
                switch (state)
                {
                    case 0:
                        if (AscciCode == 39)//if is this '
                        {
                            state = 15;
                            AuxLex += chain;
                        }
                        else if (AscciCode >= 65 && AscciCode <= 122)// If is Letter
                        {
                            state = 2;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 45)// if is -
                        {
                            state = 3;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 47)// if is /
                        {
                           // MessageBox.Show("entro", "asdfa");
                            state = 4;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 60)// if is <
                        {
                            state = 5;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 62)// if is >
                        {
                            state = 6;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 61)// if is =
                        {
                            state = 7;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 40)//if is (
                        {
                            state = 8;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 41)// if is )
                        {
                            state = 9;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 33)// if is !
                        {
                            state = 10;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 59)// if is ;
                        {
                            state = 11;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 44)// if is ,
                        {
                            state = 12;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 42)// if is *
                        {
                            state = 13;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 34)// if is "
                        {
                            state = 14;
                            AuxLex += chain;
                        }else if (AscciCode==46)
                        {
                            state = 36;
                            AuxLex += chain;

                        } else if (AscciCode >= 48 && AscciCode <= 57)// if is number
                        {
                            state = 34;
                            AuxLex += chain;
                        }
                        else
                        {
                            if (AscciCode >= 00 && AscciCode <= 32 && AscciCode != 10)
                            {
                                column++;
                            } else if (AscciCode == 10)
                            {
                                column = 0;
                                row++;
                            }
                            else
                            {
                                AuxLex += chain;
                                Errors.Add(new Token("Lexico", AuxLex+" "+" no pertenece al alfabeto", -1, row, column, i - AuxLex.Length));
                                AuxLex = "";
                                state = 0;
                            }
                        }
                        break;
                    case 1:
                        if (AscciCode >= 48 && AscciCode <= 57)// if is number after  '
                        {
                            state = 15;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + ",No pertenece al alfabeto de fechas", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 2:
                        if (AscciCode >= 65 && AscciCode <= 122)// if is Letter after Letter
                        {
                            state = 2;
                            AuxLex += chain;

                        } else if (AscciCode >= 48 && AscciCode <= 57){//if is Number after Letter
                            state = 2;
                            AuxLex += chain;

                        }
                        else if (AscciCode==95){// if is _ after Letter 
                            state = 2;
                            AuxLex += chain;

                        }
                        else
                        {
                            String Valor_Tipo = tk.Verify_R(AuxLex);
                            TokensArray.Add(new Token(AuxLex, Valor_Tipo, tk.Verify_Correlative(AuxLex),row,column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                            i -= 1;
                            row++;
                        }
                        break;
                    case 3:
                        if (AscciCode == 45)//if is - after -
                        {
                            state = 26;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + "No pertenece a la exp,Se esperaba -", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 4:
                        if (AscciCode == 42)//if is * after /
                        {
                           // MessageBox.Show("entro2", "asdfa");
                            state = 27;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + "No pertenece a la exp,Se esperaba -", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 5:
                        if (AscciCode == 61)// if is = after <
                        {
                            state = 30;
                            AuxLex += chain;
                        }
                        else
                        {
                            
                            TokensArray.Add(new Token(AuxLex, "Menor", tk.Verify_Correlative("Menor"), row, column, i - AuxLex.Length));
                            AuxLex = "";
                            i -= 1;
                            state = 0;
                            row++;
                        }
                        break;
                    case 6:
                        if (AscciCode == 61)// if is = after <
                        {
                            state = 31;
                            AuxLex += chain;
                        }
                        else
                        {
                            TokensArray.Add(new Token(AuxLex, "Mayor", tk.Verify_Correlative("Mayor"), row, column, i - AuxLex.Length));
                            AuxLex = "";
                            i -= 1;
                            state = 0;
                            row++;
                        }
                        break;
                    case 7:
                        TokensArray.Add(new Token(AuxLex, "igual", tk.Verify_Correlative("igual"), row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    case 8:
                        TokensArray.Add(new Token(AuxLex, "Parentesis Izquierdo", tk.Verify_Correlative("Parentesis Izq"), row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    case 9:
                        TokensArray.Add(new Token(AuxLex, "Parentesis Derecho", tk.Verify_Correlative("Parentesis Der"), row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    case 10:
                        if (AscciCode == 61)//if is = after !
                        {
                            state = 32;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + "No pertenece a la exp,Se esperaba -", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 11:
                        TokensArray.Add(new Token(AuxLex, "Punto y Coma", tk.Verify_Correlative("PuntoComma"), row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    case 12:
                        TokensArray.Add(new Token(AuxLex, "Comma", tk.Verify_Correlative("Comma"), row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    case 13:
                        TokensArray.Add(new Token(AuxLex, "Asterisco", tk.Verify_Correlative("Asterisco"), row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    case 14:
                        if(AscciCode == 34){//if comes " after everything
                            state = 33;
                            AuxLex += chain;
                        }
                        else //still conc
                        {
                            state = 14;
                            AuxLex += chain;
                        }
                        break;
                    case 15:
                        if (AscciCode >= 48 && AscciCode <= 57)//if is number after number
                        {
                            state = 16;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + " no pertenece al alfabeto de fechas", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 16:
                        if (AscciCode == 47)//if is / after number 
                        {
                            state = 17;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + " no pertenece al alfabeto de fechas", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 17:
                        if (AscciCode >= 48 && AscciCode <= 57)//if is number after /
                        {
                            state = 18;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + " no pertenece al alfabeto de fechas", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 18:
                        if (AscciCode >= 48 && AscciCode <= 57)//if is number after number 
                        {
                            state = 19;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + " no pertenece al alfabeto de las fechas", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 19:
                        if (AscciCode == 47)//if is / after number 
                        {
                            state = 20;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + " no pertenece al alfabeto de las fechas", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 20:
                        if (AscciCode >= 48 && AscciCode <= 57)//if is number after number 
                        {
                            state = 21;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + " no pertenece al alfabeto de las fechas", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 21:
                        if (AscciCode >= 48 && AscciCode <= 57)//if is number after number 
                        {
                            state = 22;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + " no pertenece al alfabeto de las fechs", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 22:
                        if (AscciCode >= 48 && AscciCode <= 57)//if is number after number 
                        {
                            state = 23;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + " no pertenece al alfabeto de las fechs", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 23:
                        if (AscciCode >= 48 && AscciCode <= 57)//if is number after number 
                        {
                            state = 24;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + " no pertenece al alfabeto de las fechs", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 24:
                        if(AscciCode == 39)//if is ' after number
                        {
                            state = 25;
                            AuxLex += chain;
                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + " no pertenece al alfabeto de las fechs", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 25:
                        TokensArray.Add(new Token(AuxLex, "Tipo Fecha", tk.Verify_Correlative("Tipo Fecha"), row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    case 26:
                        if (AscciCode == 10)// if is /n ends the comment
                        {
                            TokensArray.Add(new Token(AuxLex, "Comentario", tk.Verify_Correlative("Comentario"), row, column, i - AuxLex.Length));
                            AuxLex = "";
                            i -= 1;
                            state = 0;
                            row++;
                        }
                        else
                        {
                            state = 26;
                            AuxLex += chain;
                        }
                        break;
                    case 27:
                        if (AscciCode == 42)//if is * after body coment

                        {
                          
                            state = 28;
                            AuxLex += chain;
                        }
                        else
                        {
                           // MessageBox.Show("entro3", "asdfa");
                            state = 27;
                            AuxLex += chain;
                        }
                        break;
                    case 28:
                        if (AscciCode == 47)// if is / after *
                        {
                            //MessageBox.Show("entro5", "asdfa");
                            state = 29;
                            AuxLex += chain;

                        }
                        else
                        {
                            AuxLex += chain;
                            Errors.Add(new Token("Lexico", AuxLex + " " + " no pertenece al alfabeto de las de Comentario Multiple", -1, row, column, i - AuxLex.Length));
                            AuxLex = "";
                            state = 0;
                        }
                        break;
                    case 29:
                       // MessageBox.Show("ACEPTO", "asdfa");
                        TokensArray.Add(new Token(AuxLex, "Comentario Multiple", tk.Verify_Correlative("Comentario Multiple"), row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    case 30:
                        TokensArray.Add(new Token(AuxLex, "Menor Igual", tk.Verify_Correlative("Menor Igual"), row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    case 31:
                        TokensArray.Add(new Token(AuxLex, "Mayor Igual", tk.Verify_Correlative("Mayor Igual"), row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    case 32:
                        TokensArray.Add(new Token(AuxLex, "Distinto", tk.Verify_Correlative("Distinto"), row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    case 33:
                        TokensArray.Add(new Token(AuxLex, "Tipo Cadena", tk.Verify_Correlative("Tipo Cadena") ,row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    case 34:
                        if (AscciCode >= 48 && AscciCode <= 57)//if is number after number
                        {
                            state = 34;
                            AuxLex += chain;
                        }
                        else if (AscciCode == 46)
                        {
                            state = 35;
                            AuxLex += chain;
                        }
                        else {
                           // MessageBox.Show("aqui entro", "ZHY");
                            TokensArray.Add(new Token(AuxLex, "Tipo Entero", tk.Verify_Correlative("Tipo Entero"), row, column, i - AuxLex.Length));
                            AuxLex = "";
                            i -= 1;
                            state = 0;
                            row++;
                        }
                        break;
                    case 35:
                        if (AscciCode >= 48 && AscciCode <= 57)//if is number after number
                        {
                            state = 35;
                            AuxLex += chain;
                        }
                        else
                        {
                            TokensArray.Add(new Token(AuxLex, "Tipo Flotante", tk.Verify_Correlative("Tipo Flotante"), row, column, i - AuxLex.Length));
                            AuxLex = "";
                            i -= 1;
                            state = 0;
                            row++;
                        }
                        break;
                    case 36:
                        TokensArray.Add(new Token(AuxLex, "Punto", tk.Verify_Correlative("Punto"), row, column, i - AuxLex.Length));
                        AuxLex = "";
                        i -= 1;
                        state = 0;
                        row++;
                        break;
                    default:
                        break;
                
}
            }

           TokensArray.Add(new Token("", "Ultimo", tk.Verify_Correlative("Ultimo"), 0, 0,0));
        }


        public void Html_Tokens()
        {
            String html_Content;
            html_Content = "<html>" +
            "<link rel=\"stylesheet\" type=\"text/css\" href=\"bootstrap.css\">" +
            "<body>" +
            "<h1 align='center'>LISTADO DE TOKENS</h1></br>" +
           
            "<div class=\"table\">" +
        "<table cellpadding='10' border = '1' align='center'>" +
            "<tr>" +
            "<td><strong>No." +
            "</strong></td>" +

             "<td><strong>Correlativo." +
            "</strong></td>" +

            "<td><strong>Lexema" +
            "</strong></td>" +

           "<td><strong>Tipo" +
            "</strong></td>" +

            "<td><strong>Fila" +
            "</strong></td>" +

             "<td><strong>Columna" +
            "</strong></td>" +

            "</tr>";

            String Cad_tokens = "";
            String tempo_tokens;

            for (int i = 0; i < TokensArray.Count-1; i++)
            {
                Token sen_pos = TokensArray.ElementAt(i);
                
                tempo_tokens = "";
                tempo_tokens = "<tr>" +
                "<td><strong>" + (i + 1).ToString() +
                "</strong></td>" +

                 "<td>" + sen_pos.GetCorr() +
                "</td>" +

                "<td>" + sen_pos.getLexl() +
                "</td>" +

                "<td>" + sen_pos.getType() +
                "</td>" +

                "<td>" + sen_pos.getRow() +
                "</td>" +

                "<td>" + sen_pos.getColumn() +
                "</td>" +

                "</tr>";
                Cad_tokens = Cad_tokens + tempo_tokens;

            }

            html_Content = html_Content + Cad_tokens +
            "</div>" + "</table>"+
            "</table>" +
            "</body>" +
            "</html>";

            //MessageBox.Show(html_Content, "html_Content");

            /*creando archivo html*/
            File.WriteAllText("Reporte de Tokens.html", html_Content);
            System.Diagnostics.Process.Start("Reporte de Tokens.html");


        }


        public void Html_Errores(List<Token> Errors_List)
        {
            String html_Content;
            html_Content = "<html>" +
            "<link rel=\"stylesheet\" type=\"text/css\" href=\"Resources\bootstrap.css\">" +
            "<body>" +
            "<h1 align='center'>LISTADO DE Errores</h1></br>" +
            "<table cellpadding='10' border = '1' align='center'>" +
            "<tr>" +
            "<td><strong>No." +
            "</strong></td>" +

            "<td><strong>Tipo" +
            "</strong></td>" +

           "<td><strong>Descripcion" +
            "</strong></td>" +

            "<td><strong>Fila" +
            "</strong></td>" +

             "<td><strong>Columna" +
            "</strong></td>" +

            "</tr>";

            String Cad_tokens = "";
            String tempo_tokens;

            for (int i = 0; i < Errors_List.Count; i++)
            {
                Token sen_pos = Errors_List.ElementAt(i);

                tempo_tokens = "";
                tempo_tokens = "<tr>" +
                "<td><strong>" + (i + 1).ToString() +
                "</strong></td>" +

                "<td>" + sen_pos.getLexl() +
                "</td>" +

                "<td>" + sen_pos.getType() +
                "</td>" +

                "<td>" + sen_pos.getRow() +
                "</td>" +

                "<td>" + sen_pos.getColumn() +
                "</td>" +

                "</tr>";
                Cad_tokens = Cad_tokens + tempo_tokens;

            }

            html_Content = html_Content + Cad_tokens +
            "</table>" +
            "</body>" +
            "</html>";
            File.WriteAllText("Reporte de Errores.html", html_Content);
            System.Diagnostics.Process.Start("Reporte de Errores.html");
        

        


    }

       
}
}
