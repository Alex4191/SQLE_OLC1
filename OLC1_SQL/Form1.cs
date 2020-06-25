using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLC1_SQL
{
    public partial class Form1 : Form
    {
        private List<Paths> List_Paths;
        private String actual_path;
        private String actual_name;
        Scanner_201602880 s;
        Parser_201602880 p;
        public Form1()
        {
            InitializeComponent();
            List_Paths = new List<Paths>();
            s= new Scanner_201602880();
            p = new Parser_201602880();
        }

       



        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "[OLC1]BAPA|*.bapa";
            string row = "";
            string text = "";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string path1 = openFile.FileName;
                StreamReader strReader = new StreamReader(path1, System.Text.Encoding.UTF8);
                string nameC = Path.GetFileNameWithoutExtension(openFile.FileName);
                while ((row = strReader.ReadLine()) != null)
                {
                    text += row + System.Environment.NewLine;
                }
                richTextBox1.Text = text;
                strReader.Close();
                List_Paths.Clear();
                List_Paths.Add(new Paths(path1, nameC));

                actual_path = path1;
                actual_name = nameC;
                this.Text = actual_name;


            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Boolean exists = false;
            string path = "";
            for (int i = 0; i < List_Paths.Count; i++)
            {
                Paths pt = List_Paths.ElementAt(i);
                if (actual_path == pt.getPath())
                {
                    path = actual_path;
                    exists = true;
                }
            }
            if (exists == false)
            {
                save_As();
            }
            else
            {
                save(path);
            }
        }
        private void save(string path)
        {
            try
            {

                string text = richTextBox1.Text;
                StreamWriter writer = new StreamWriter(path);
                writer.Write(text);
                writer.Flush();
                writer.Close();

                string name = Path.GetFileNameWithoutExtension(path);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    
        private void save_As()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
           // saveFile.Filter = "[OLC1]BAPA|*.bapa";
            saveFile.Title = "Save File As";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = (FileStream)saveFile.OpenFile();
                fs.Close();
                string path = saveFile.FileName;
                save(path);
                string nombre = Path.GetFileNameWithoutExtension(path);
                List_Paths.Add(new Paths(path, nombre));
                actual_path = path;
                actual_name = nombre;
                this.Text = actual_name;

            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save_As();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Develop By: Bryan Alvarado"+"\nID: 201602880","Information");
        }

        private void analyzeToolStripMenuItem_Click(object sender, EventArgs e)
        {


           
            String text2 = richTextBox1.SelectedText;
            if (text2.Length==0)
            {
                String text = richTextBox1.Text;
                s.Analyze(text);
                Colors_Lex();
                p.Parser(s.TokensArray, s.Errors);
            }
            else
            {
                s.Analyze(text2);
                Colors_Lex();
                p.Parser(s.TokensArray, s.Errors);
            }
            
           
            
           
        }

        private void loadTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "[OLC1]SQLE|*.sqle";
            string row = "";
            string text = "";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string path1 = openFile.FileName;
                StreamReader strReader = new StreamReader(path1, System.Text.Encoding.UTF8);
                string nameC = Path.GetFileNameWithoutExtension(openFile.FileName);
                while ((row = strReader.ReadLine()) != null)
                {
                    text += row + System.Environment.NewLine;
                }
                richTextBox1.Text = text;
                strReader.Close();
                List_Paths.Clear();
                List_Paths.Add(new Paths(path1, nameC));

                actual_path = path1;
                actual_name = nameC;
                this.Text = actual_name;


            }

        }

        public void Colors_Lex()
        {
            List<Token> TokensArray = s.TokensArray;
            string word;
            int index;
            int sz;
            for (int i = 0; i < TokensArray.Count; i++)
            {
                Token sen_pos = TokensArray.ElementAt(i);
                word = sen_pos.getLexl();
                switch (sen_pos.GetCorr())
                {
                    case 2://IDENTIFICADOR
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Brown;
                        break;
                    case 3://TIPO ENTERO
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.LightBlue;
                        break;
                    case 6:// TIPO FLOTANTE
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.LightBlue;
                        break;
                    case 5://TIPO CADENAS
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Green;
                        break;
                    case 4://TIPO FECHA
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Orange;
                        break;
                    case 10://MENOR
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Red;
                        break;
                    case 11://MAYOR
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Red;
                        break;
                    case 12://IGUAL
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Red;
                        break;
                    case 13://MAYOR IGUAL
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Red;
                        break;
                    case 14://MENOR IGUAL
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Red;
                        break;
                    case 15://DISTINTO
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Red;
                        break;
                    case 19://COMENTARIO
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.LightGray;
                        break;
                    case 20://COMENTARIO MULTIPLE
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.LightGray;
                        break;
                    case 26://TABLA
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Purple;
                        break;
                    case 27://INSERTAR
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Purple;
                        break;
                    case 30://ELIMINAR
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Purple;
                        break;
                    case 35://ACTUALIZAR
                        index = sen_pos.getIndex_lex();
                        sz = word.Length;
                        richTextBox1.Select(index, sz);
                        richTextBox1.SelectionColor = Color.Purple;
                        break;
                    default:
                        break;
                }
            }
        }

        private void showTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (s.TokensArray.Count!= 0)
            {
                s.Html_Tokens();
            }
            else
            {
                MessageBox.Show("You must press the Anayze option first","Tokens Option");
            }
           

           


        }

        private void showErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (p.Error_List.Count != 0)
            {
                s.Html_Errores(p.Error_List);
            }
            else
            {
                MessageBox.Show("Errors not founded","Error Option");
            }
        }

        private void viewTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            p.ImprimirTablas();
        }

        private void userGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("ManualUsuario.pdf");
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("NO SE ENCONTRO EL MANUAL EN LA RUTA ESPECIFICADA");
            }
           
        }

        private void technicalGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("ManualTecnico.pdf");
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("NO SE ENCONTRO EL MANUAL EN LA RUTA ESPECIFICADA");
            }
        }
    }
}
