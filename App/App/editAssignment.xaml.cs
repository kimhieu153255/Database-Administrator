using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.ObjectModel;
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
        private static OracleConnection con { get; set; } = null;


        public EditAssignment(Assignment data,string _Username,string _Password)
        {
            this.Assign = data;
            this._Username = _Username;
            this._Password = _Password;
            DataContext = this.Assign;
            InitializeComponent();
            loadMADA();
            loadMANV();
        }
        private void CreateConnection()
        {
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={this._Username};Password={this._Password};";
            con = new OracleConnection(conn);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string manv = MANV.Text;
            string mada = MADA.Text;
            string thoigian = THOIGIAN.Text;

            CreateConnection();
            try
            {
                con.Open();
                OracleCommand command = con.CreateCommand();
                command.CommandText = $"UPDATE system.PHANCONG SET MANV=:MANV, MADA = :MADA, THOIGIAN = TO_DATE(:THOIGIAN, 'DD/MM/YYYY') where MANV=:MANV1 and MADA = :MADA1";
                command.Parameters.Add("MANV", OracleDbType.Varchar2).Value = manv;
                command.Parameters.Add("MADA", OracleDbType.Varchar2).Value = mada;
                command.Parameters.Add("THOIGIAN", OracleDbType.Varchar2).Value = thoigian;
                command.Parameters.Add("MANV1", OracleDbType.Varchar2).Value = Assign.MANV;
                command.Parameters.Add("MADA1", OracleDbType.Varchar2).Value = Assign.MADA;

                int rowsUpdated = command.ExecuteNonQuery();
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
            this.Close();
        }
        private void loadMADA()
        {
            CreateConnection();
            string manv = this._Username.Substring(2);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"select distinct(MADA) from system.dean where phong in (select phg from system.nhanvien where manv =:MANV)", con);
                oracleCommand.Parameters.Add("MANV", OracleDbType.Varchar2).Value = manv;
                OracleDataReader reader = oracleCommand.ExecuteReader();
                ObservableCollection<string> list = new ObservableCollection<string>();
                while (reader.Read())
                {
                    string MAPB = reader.GetString(0);
                    list.Add(MAPB);
                }
                if (list.Count > 0)
                {
                    MADA.ItemsSource = list;
                    int index = list.IndexOf(Assign.MADA);
                    MADA.SelectedItem = list[index];
                }
            }
            catch (Exception)
            {
                MessageBox.Show("App have a problem!!!");
                return;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        private void loadMANV()
        {
            CreateConnection();
            string manv = this._Username.Substring(2);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"select manv from system.nhanvien where manv != :MANV and phg in (select phg from system.nhanvien where manv =:MANV1)", con);
                oracleCommand.Parameters.Add("MANV", OracleDbType.Varchar2).Value = manv;
                oracleCommand.Parameters.Add("MANV1", OracleDbType.Varchar2).Value = manv;
                OracleDataReader reader = oracleCommand.ExecuteReader();
                ObservableCollection<string> list = new ObservableCollection<string>();
                while (reader.Read())
                {
                    string MAPB = reader.GetString(0);
                    list.Add(MAPB);
                }
                if (list.Count > 0)
                {
                    int index = list.IndexOf(Assign.MANV);
                    MANV.ItemsSource = list;
                    MANV.SelectedItem = list[index];
                }
            }
            catch (Exception)
            {
                MessageBox.Show("App have a problem!!!");
                return;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}
