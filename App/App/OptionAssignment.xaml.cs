using Oracle.ManagedDataAccess.Client;
using System;
using System.ComponentModel;
using System.Windows;

namespace App
{
    /// <summary>
    /// Interaction logic for OptionAssignment.xaml
    /// </summary>
    public partial class OptionAssignment : Window
    {
        public delegate void sendMessage(string message, Assignment assign);
        public event sendMessage OneMessage;
        public Assignment Assign { get; set; }
        public string _Username { get; set; }
        public string _Password { get; set; }

        public OptionAssignment(Assignment data,string _Username,string _Password)
        {
            this.Assign = data;
            this._Username = _Username;
            this._Password = _Password;
            MessageBox.Show(data.MADA+" "+_Password+" "+_Username);
            DataContext = this.Assign;
            InitializeComponent();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Assign.MANV = MANV.Text;
            Assign.MADA = MADA.Text;
            Assign.THOIGIAN = THOIGIAN.Text;

            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand command = con.CreateCommand();
                command.CommandText = $"UPDATE system.PHANCONG SET MANV = :MANV, MADA = :MADA, THOIGIAN = TO_DATE(:THOIGIAN, 'DD/MM/YYYY')";

                command.Parameters.Add("MANV", OracleDbType.Varchar2).Value = Assign.MANV;
                command.Parameters.Add("MADA", OracleDbType.Varchar2).Value = Assign.MADA;
                command.Parameters.Add("THOIGIAN", OracleDbType.Varchar2).Value = Assign.THOIGIAN;

                int rowsUpdated = command.ExecuteNonQuery();

                if (rowsUpdated > 0)
                {
                    OneMessage?.Invoke("Success!!!", Assign);
                }
                else
                {
                    OneMessage?.Invoke(null,null);
                }
            }
            catch (Exception)
            {
                OneMessage?.Invoke(null,null);
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
            this.Close();
        }
    }
}
