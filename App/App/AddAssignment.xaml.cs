using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace App
{
    /// <summary>
    /// Interaction logic for AddAssignment.xaml
    /// </summary>
    public partial class AddAssignment : Window
    {
        public delegate void sendMessage(string message);
        public event sendMessage OneMessage;
        public string _Username { get; set; }
        public string _Password { get; set; }

        public AddAssignment(string _Username, string _Password)
        {
            this._Username = _Username;
            this._Password = _Password;
            InitializeComponent();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            string manv = MANVinsert.Text;
            string mada = MADAinsert.Text;
            string thoigian = THOIGIANinsert.Text;
            MessageBox.Show(manv + " " + mada + " " + thoigian+" "+_Username+" "+_Password);
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand command = con.CreateCommand();
                command.CommandText = "insert into system.PHANCONG values (:MANV, :MADA, TO_DATE(:THOIGIAN, 'DD/MM/YYYY'))";

                command.Parameters.Add("MANV", OracleDbType.Varchar2).Value = manv;
                command.Parameters.Add("MADA", OracleDbType.Varchar2).Value = mada;
                command.Parameters.Add("THOIGIAN", OracleDbType.Varchar2).Value = thoigian;

                int rowsInsert = command.ExecuteNonQuery();
                MessageBox.Show("insert: " + rowsInsert);
                if (rowsInsert > 0)
                    OneMessage?.Invoke(new string("Success!!!"));
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
                return;
            }
            finally
            {
                con.Close();
                con.Dispose();
                this.Close();
            }
        }

        private void Cancle_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("cancel");
            this.Close();
        }
    }
}
