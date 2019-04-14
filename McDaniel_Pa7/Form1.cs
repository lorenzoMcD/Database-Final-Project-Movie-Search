using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;



namespace McDaniel_Pa7
{
    public partial class Form1 : Form
    {
        private string filepath;
        public int cnt = 0;
       
        bool matchFound = false;
        public string name = "McDaniel";

        public string castSearch;
        public string directorSearch;
        public string keywordSearch;
        public StreamWriter sw2 = new StreamWriter("McDaniel" +"_"+ "Summary.csv");
        public bool isclosed = false;
        public Form1()

        
        {
            InitializeComponent();
            filepath = "";

             
            
            sw2.Write("filename,castSearch,directorSearch,keywordSearch\n"); 
            


        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {


            DialogResult dr = openFileDialog1.ShowDialog();


            if (dr == DialogResult.OK)
            {
                filepath = openFileDialog1.FileName;
            }
            else
            {
                filepath = "";
            }


        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            bool keepgoing = true;
            // if user input a string that can be converted into a number program will not search
            int number;
            if (int.TryParse(textBox1.Text.Trim(), out number))
            {
                MessageBox.Show("Please dont insert numbers into the textbox, enter non numerical strings only ");
                keepgoing = false;
            }


            if (int.TryParse(textBox2.Text.Trim(), out number))
            {
                MessageBox.Show("Please dont insert numbers into the textbox, enter non numerical strings only ");
                keepgoing = false;
            }

            if (int.TryParse(textBox3.Text.Trim(), out number))
            {
                MessageBox.Show("Please dont insert numbers into the textbox, enter non numerical strings only ");
                keepgoing = false;

            }
            // make sure we don't open a connection to an empty filepath
            if (filepath == "")
            {
                richTextBox1.Text = "Select a file first!";

                return;
            }

            // First thing to do!  open a connection to the database
            // We need a specific connection string to create our connection
            // opject for the database
            // Hint: go to www.connectionstrings.com !
            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};", filepath);



            // Creating a connection to the database we selected.
            OleDbConnection dbConn = new OleDbConnection(connString);
            // Opening the connection to the database
            dbConn.Open();



            // Next step, we need to send a command (query) to the database
            // and retrieve some data
            OleDbCommand dbCommand = new OleDbCommand();


            // For our first example, we will hand-create the SQL command to send to the database.

            dbCommand.CommandText = "SELECT releaseyear ,title,director,[cast],plot,genre FROM MoviePlots"; // put brackets around cast , its connected to reserved word in other table
            dbCommand.Connection = dbConn;           // ... to this database connection.



            // next, send the CommandText query to the database!
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            // NOTE: the returned table is stored in the dbReader object

            richTextBox1.Clear(); // clear out old junk data
                                 

            string castSearch = textBox1.Text;

            string swv = string.Format("{0}.csv", textBox1.Text);
            string swv2 = string.Format("McDaniel" + "_" + "{0}" + "{1}" + "{2}",textBox1.Text,textBox2.Text,textBox3.Text);
            string directorSearch = textBox2.Text;
            directorSearch.ToLower();
            string plotSearch = textBox3.Text;






                StreamWriter sw = new StreamWriter(name.Replace(" ", "_") + swv.Replace(" ", string.Empty));
                sw.Write("ReleaseYear,Title,Director,Cast\n");
            


           


            castSearch = textBox1.Text;
            directorSearch = textBox2.Text;
            keywordSearch = textBox3.Text;

            if (keepgoing == true)
            {
                if (isclosed == true)
                {
                    using (StreamWriter sw2 = File.AppendText("McDaniel" + "_" + "Summary.csv"))
                    {
                        sw2.Write("{0},{1},{2},{3}\n", swv2, castSearch, directorSearch, keywordSearch);
                    }

                }
                else
                {

                    sw2.Write("{0},{1},{2},{3}\n", swv2, castSearch, directorSearch, keywordSearch);
                }
            }
            string[] c = castSearch.Split(' '); // gonna split on the space

            string[] d = directorSearch.Split(' '); // gonna split on the space

            string[] p = plotSearch.Split(' '); // gonna split on the space









            // Now we read each row from the returned table
            if (keepgoing == true)  // if user input a string that can be converted into a number program will not search
            {
                while (dbReader.Read() == true)
                {


                    string releaseyear = dbReader[0].ToString();

                    string title = dbReader[1].ToString(); 
                                                          

                    string director = dbReader[2].ToString();
                


                    string cast = dbReader[3].ToString(); 
                    string trimCast = cast.Replace(" ", string.Empty);

                    


                    string plot = dbReader[4].ToString(); 



                    string genre = dbReader[5].ToString();


                  



                    string ryearmatch;
                    string titlematch;
                    string directmatch;
                    string castmatch;
                    string genrematch;



                    // info needed in order to match or grab my matched data on line it appears as database
                    // cycles line by line

                    ryearmatch = dbReader[0].ToString();
                    titlematch = dbReader[1].ToString();
                    directmatch = dbReader[2].ToString();
                    castmatch = dbReader[3].ToString();
                    genrematch = dbReader[5].ToString();

                 
                    
                    
                    
                    
                    
                    ////////////////_______  directorSearch textbox logic ______////////////




                    bool bbreak = false; // the bbreak logic was created in attempt to remove duplicates, worked a little but doesnt remove all duplicates

                    if (directorSearch != "")
                    {

                        foreach (string dss in d)

                        {


                            if (textBox1.Text == "" && textBox3.Text == "")
                            {
                                if (bbreak)

                                    break;



                                if (director.ToLower().Contains(dss.ToLower()))
                                {



                                    matchFound = true;

                                    if (matchFound == true)
                                    {

                                        richTextBox1.AppendText(String.Format("year: {0}, director: {1}, title: {2} , cast: {3} \n ", ryearmatch, directmatch, titlematch, castmatch));

                                        sw.Write("{0},{1},{2},{3}\n", ryearmatch, titlematch, "\"" + directmatch + "\"", "\"" + castmatch + "\"");

                                        bbreak = true;
                                        break;
                                    }

                                }



                            }
                        }

                        foreach (string dS in d)
                        {

                            foreach (string cs in c)
                            {

                                foreach (string ps in p)
                                {



                                    if (textBox1.Text != "" && textBox3.Text == "")
                                    {

                                        if (bbreak)

                                            break;


                                        if (director.ToLower().Contains(dS.ToLower()) && trimCast.ToLower().Contains(cs.ToLower()))
                                        {
                                            matchFound = true;

                                            if (matchFound == true)
                                            {

                                                richTextBox1.AppendText(String.Format("year: {0}, director: {1}, title: {2} , cast: {3} \n ", ryearmatch, directmatch, titlematch, castmatch));

                                                sw.Write("{0},{1},{2},{3}\n", ryearmatch, titlematch, "\"" + directmatch + "\"", "\"" + castmatch + "\"");

                                                bbreak = true;
                                                break;
                                            }
                                        }



                                    }
                                    else if (textBox3.Text != "" && textBox1.Text == "")
                                    {
                                        if (bbreak)

                                            break;

                                        if (director.ToLower().Contains(dS.ToLower()) && plot.ToLower().Contains(ps.ToLower()))
                                        {

                                            matchFound = true;

                                            if (matchFound == true)
                                            {

                                                richTextBox1.AppendText(String.Format("year: {0}, director: {1}, title: {2} , cast: {3} \n ", ryearmatch, directmatch, titlematch, castmatch));

                                                sw.Write("{0},{1},{2},{3}\n", ryearmatch, titlematch, "\"" + directmatch + "\"", "\"" + castmatch + "\"");

                                                bbreak = true;
                                                break;
                                            }
                                        }

                                    }

                                    else if (textBox1.Text != "" && textBox3.Text != "" && textBox2.Text != "")
                                    {
                                        if (bbreak)

                                            break;

                                        if (director.ToLower().Contains(dS.ToLower()) && plot.ToLower().Contains(ps.ToLower()) && trimCast.ToLower().Contains(cs.ToLower()))
                                        {
                                            matchFound = true;

                                            if (matchFound == true)
                                            {

                                                richTextBox1.AppendText(String.Format("year: {0}, director: {1}, title: {2} , cast: {3} \n ", ryearmatch, directmatch, titlematch, castmatch));

                                                sw.Write("{0},{1},{2},{3}\n", ryearmatch, titlematch, "\"" + directmatch + "\"", "\"" + castmatch + "\"");

                                                bbreak = true;
                                                break;
                                            }
                                        }


                                    }



                                }

                            }
                        }
                    }
                    
                    ////////////////_______  plotsearch textbox logic ______////////////


                    else if (plotSearch != "")
                    {

                        foreach (string pS in p)
                        {


                            if (textBox1.Text == "" && textBox2.Text == "")
                            {
                                if (bbreak)

                                    break;

                                if (plot.ToLower().Contains(pS.ToLower()))
                                {
                                    matchFound = true;

                                    if (matchFound == true)
                                    {
                                        cnt = 1;
                                        richTextBox1.AppendText(String.Format("year: {0}, director: {1}, title: {2} , cast: {3} \n ", ryearmatch, directmatch, titlematch, castmatch));

                                        sw.Write("{0},{1},{2},{3}\n", ryearmatch, titlematch, "\"" + directmatch + "\"", "\"" + castmatch + "\"");

                                        break;
                                    }

                                }


                            }

                        }


                        foreach (string dS in d)
                        {

                            foreach (string cs in c)
                            {

                                foreach (string ps in p)
                                {

                                    if (textBox1.Text != "" && textBox2.Text == "")
                                    {
                                        if (bbreak)

                                            break;

                                        if (plot.ToLower().Contains(ps.ToLower()) && trimCast.ToLower().Contains(cs.ToLower()))
                                        {
                                            matchFound = true;

                                            if (matchFound == true)
                                            {
                                                cnt = 1;
                                                richTextBox1.AppendText(String.Format("year: {0}, director: {1}, title: {2} , cast: {3} \n ", ryearmatch, directmatch, titlematch, castmatch));

                                                sw.Write("{0},{1},{2},{3}\n", ryearmatch, titlematch, "\"" + directmatch + "\"", "\"" + castmatch + "\"");

                                                break;
                                            }
                                        }

                                    }
                                    else if (textBox2.Text != "" && textBox1.Text == "")
                                    {
                                        if (bbreak)

                                            break;

                                        if (director.ToLower().Contains(dS.ToLower()) && plot.ToLower().Contains(ps.ToLower()))
                                        {
                                            matchFound = true;

                                            if (matchFound == true)
                                            {
                                                cnt = 1;
                                                richTextBox1.AppendText(String.Format("year: {0}, director: {1}, title: {2} , cast: {3} \n ", ryearmatch, directmatch, titlematch, castmatch));

                                                sw.Write("{0},{1},{2},{3}\n", ryearmatch, titlematch, "\"" + directmatch + "\"", "\"" + castmatch + "\"");

                                                break;
                                            }
                                        }

                                    }


                                    else if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
                                    {
                                        if (bbreak)

                                            break;

                                        if (director.ToLower().Contains(dS.ToLower()) && plot.ToLower().Contains(ps.ToLower()) && trimCast.ToLower().Contains(cs.ToLower()))
                                        {

                                            matchFound = true;

                                            if (matchFound == true)
                                            {
                                                cnt = 1;
                                                richTextBox1.AppendText(String.Format("year: {0}, director: {1}, title: {2} , cast: {3} \n ", ryearmatch, directmatch, titlematch, castmatch));

                                                sw.Write("{0},{1},{2},{3}\n", ryearmatch, titlematch, "\"" + directmatch + "\"", "\"" + castmatch + "\"");

                                                break;
                                            }


                                        }



                                    }


                                }

                            }
                        }
                    }


                    ////////////////_______  castsearch textbox logic ______////////////



                    else if (castSearch != "")
                    {


                        foreach (string cS in c)
                        {


                            if (castSearch != "")
                            {
                                if (bbreak)

                                    break;
                                if (trimCast.ToLower().Contains(cS.ToLower()))
                                {

                                    matchFound = true;

                                    if (matchFound == true)
                                    {
                                        cnt = 1;
                                        richTextBox1.AppendText(String.Format("year: {0}, director: {1}, title: {2} , cast: {3} \n ", ryearmatch, directmatch, titlematch, castmatch));

                                        sw.Write("{0},{1},{2},{3}\n", ryearmatch, titlematch, "\"" + directmatch + "\"", "\"" + castmatch + "\"");

                                        break;
                                    }
                                }


                            }


                        }




                        foreach (string dS in d)
                        {
                            if (bbreak)

                                break;
                            foreach (string cs in c)
                            {
                                if (bbreak)

                                    break;
                                foreach (string ps in p)
                                {
                                    if (bbreak)

                                        break;


                                    if (textBox2.Text != "" && textBox3.Text == "")
                                    {
                                        if (bbreak)

                                            break;

                                        if (trimCast.ToLower().Contains(cs.ToLower()) && director.ToLower().Contains(dS.ToLower()))
                                        {
                                            matchFound = true;

                                            if (matchFound == true)
                                            {
                                                cnt = 1;
                                                richTextBox1.AppendText(String.Format("year: {0}, director: {1}, title: {2} , cast: {3} \n ", ryearmatch, directmatch, titlematch, castmatch));

                                                sw.Write("{0},{1},{2},{3}\n", ryearmatch, titlematch, "\"" + directmatch + "\"", "\"" + castmatch + "\"");

                                                break;

                                            }
                                        }

                                    }

                                    else if (textBox3.Text != "" && textBox2.Text == "")
                                    {
                                        if (bbreak)

                                            break;


                                        if (plot.ToLower().Contains(ps.ToLower()) && trimCast.ToLower().Contains(cs.ToLower()))
                                        {

                                            matchFound = true;

                                            if (matchFound == true)
                                            {
                                                cnt = 1;
                                                richTextBox1.AppendText(String.Format("year: {0}, director: {1}, title: {2} , cast: {3} \n ", ryearmatch, directmatch, titlematch, castmatch));

                                                sw.Write("{0},{1},{2},{3}\n", ryearmatch, titlematch, "\"" + directmatch + "\"", "\"" + castmatch + "\"");

                                                break;
                                            }
                                        }



                                    }
                                    else if (textBox2.Text != "" && textBox3.Text != "" && textBox1.Text != "")
                                    {
                                        if (bbreak)

                                            break;

                                        if (director.ToLower().Contains(dS.ToLower()) && plot.ToLower().Contains(ps.ToLower()) && trimCast.Contains(cs.ToLower()))
                                        {

                                            matchFound = true;

                                            if (matchFound == true)
                                            {
                                                cnt = 1;
                                                richTextBox1.AppendText(String.Format("year: {0}, director: {1}, title: {2} , cast: {3} \n ", ryearmatch, directmatch, titlematch, castmatch));

                                                sw.Write("{0},{1},{2},{3}\n", ryearmatch, titlematch, "\"" + directmatch + "\"", "\"" + castmatch + "\"");

                                                break;
                                            }
                                        }



                                    }

                                }

                            }

                        }
                    }
                }

            }
            
            dbReader.Close();
            sw.Close();
            sw2.Close();
            isclosed = true;






            
            dbConn.Close();
           // clickcnt += 1;



        }
        }
    }

