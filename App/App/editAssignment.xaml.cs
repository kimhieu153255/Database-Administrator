using Oracle.ManagedDataAccess.Client;
using System;
using System.ComponentModel;
using System.Windows;

namespace App
{
    /// <summary>
    /// Interaction logic for EditAssignment.xaml
    /// </summary>
    public partial class EditAssignment : Window
    {
        public delegate void sendMessage(string message, Assignment assign);
        public event sendMessage OneMessage;
        public Assignment Assign { get; set; }
        public string _Username { get; set; }
        public string _Password { get; set; }

        public EditAssignment(Assignment data,string _Username,string _Password)
        {
            this.Assign = data;
            this._Username = _Username;
            this._Password = _Password;
            //MessageBox.Show(data.MADA+" "+_Password+" "+_Username);
            DataContext = this.Assign;
            InitializeComponent();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Assign.MANV = MANV.Text;
            Assign.MADA = MADA.Text;
            Assign.THOIGIAN = THOIGIAN.Text;
            MessageBox.Show(Assign.MANV + " " + Assign.MADA + " " + Assign.THOIGIAN);

            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand command = con.CreateCommand();
                command.CommandText = $"UPDATE system.PHANCONG SET  THOIGIAN = TO_DATE(:THOIGIAN, 'DD/MM/YYYY') where MANV=:MANV and MADA = :MADA";
                command.Parameters.Add("THOIGIAN", OracleDbType.Varchar2).Value = Assign.THOIGIAN;
                command.Parameters.Add("MANV", OracleDbType.Varchar2).Value = Assign.MANV;
                command.Parameters.Add("MADA", OracleDbType.Varchar2).Value = Assign.MADA;

                int rowsUpdated = command.ExecuteNonQuery();
                MessageBox.Show("update: " + rowsUpdated);
                if (rowsUpdated > 0)
                    OneMessage?.Invoke(new string("Success!!!"), Assign);
            }
            catch (Exception)
            {
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
