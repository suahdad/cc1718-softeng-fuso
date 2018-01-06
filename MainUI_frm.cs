using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;





namespace WindowsFormsApplication1
{


    public partial class MainUI_frm : Form
    {

 
        List<String> jobOrder = new List<String>();
        public MainUI_frm()
        {
            InitializeComponent();
        }

        public void OledbCon(bool access, string path, out OleDbConnection acsCon)
        {
            //this function opens a connection
            string acs;
            if (access == true)
            {
                acs = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Persist Security Info=False;";
            }else
            { 
                acs = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+ path+";Extended Properties='Excel 12.0 Xml;HDR=YES';";
            }
            acsCon = new OleDbConnection(acs);
            acsCon.Open();


        }
        public void datGrViewDS(DataGridView dat,OleDbConnection datcon, string dattable)
        {//this function connects a data source to a datagridview
            string adapt = "Select * from "+dattable;
            OleDbDataAdapter datadapt = new OleDbDataAdapter(adapt, datcon);
            DataTable datdt = new DataTable();
            datadapt.Fill(datdt);
            dat.DataSource = datdt;
        }

            
        private void MainUI_frm_Load(object sender, EventArgs e)
        {
            dt_lbl.Text = DateTime.Now.ToString("MMMM dd, yyyy");

            OleDbConnection con;
            OledbCon(access: true, path: @"C:\Users\Arandov\Desktop\Database5.accdb", acsCon: out con);
            datGrViewDS(dataGridView1, con, "TotalProcData");
            con.Close();

            string sSheetName = null;
            string sPath = @"C:\Users\Arandov\Desktop\Billing Invoice 2017\OSG\Bring In.xlsx";
            DataTable dtTablesList = default(DataTable);
            OleDbCommand oleExcelCommand = default(OleDbCommand);
            OleDbDataReader oleExcelReader = default(OleDbDataReader);
            OleDbConnection oleExcelConnection = default(OleDbConnection);


            OledbCon(access: false, path: sPath, acsCon: out oleExcelConnection);

            dtTablesList = oleExcelConnection.GetSchema("Tables");
            dataGridView2.DataSource = dtTablesList;

            if (dtTablesList.Rows.Count > 0)
            {

                sSheetName = dtTablesList.Rows[0]["TABLE_NAME"].ToString();
            }


            if (!string.IsNullOrEmpty(sSheetName))
            {
                oleExcelCommand = oleExcelConnection.CreateCommand();
                oleExcelCommand.CommandText = "Select * From [" + sSheetName + "]";
                oleExcelCommand.CommandType = CommandType.Text;
                oleExcelReader = oleExcelCommand.ExecuteReader();

                while (oleExcelReader.Read())
                {
                }
                oleExcelReader.Close();
            }
            oleExcelConnection.Close();

        }



        static void Main()
        {

            Application.Run(new MainUI_frm());



        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void createBillingInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           
           Options_frm ldOpt = new Options_frm();
           ldOpt.Visible = true;

        }




    }
}
