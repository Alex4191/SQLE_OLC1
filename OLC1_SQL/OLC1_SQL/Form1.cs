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

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "[OLC1]SQLe|*.sqle";
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
            saveFile.Filter = "[OLC1]SQLe|*.sqle";
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
            String data = richTextBox1.Text;
            
            s.Analyze(data);
            p.Parser(s.TokensArray);
            if (s.Errors.Count != 0)

            {
                s.Html_Errores();
                s.Html_Tokens();
            }
            else
            {
                s.Html_Tokens();
            }
           
        }

        private void loadTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
