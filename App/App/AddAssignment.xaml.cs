using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static OracleConnection con { get; set; } = null;

        public AddAssignment(string _Username, string _Password)
        {
            this._Username = _Username;
            this._Password = _Password;
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

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            string manv = MANVinsert.Text;
            string mada = MADAinsert.Text;
            string thoigian = THOIGIANinsert.Text;
            CreateConnection();
            try
            {
                con.Open();
                OracleCommand command = con.CreateCommand();
                command.CommandText = "insert into system.PHANCONG values (:MANV, :MADA, TO_DATE(:THOIGIAN, 'DD/MM/YYYY'))";

                command.Parameters.Add("MANV", OracleDbType.Varchar2).Value = manv;
                command.Parameters.Add("MADA", OracleDbType.Varchar2).Value = mada;
                command.Parameters.Add("THOIGIAN", OracleDbType.Varchar2).Value = thoigian;

                int rowsInsert = command.ExecuteNonQuery();
                if (rowsInsert > 0)
                    OneMessage?.Invoke(new string("Success!!!"));
            }
            catch (Exception es)
            {
                MessageBox.Show("App have a problem!!!");
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
                oracleCommand.Parameters.Add("MANV",OracleDbType.Varchar2).Value = manv;
                OracleDataReader reader = oracleCommand.ExecuteReader();
                ObservableCollection<string> list = new ObservableCollection<string>();
                while (reader.Read())
                {
                    string MAPB = reader.GetString(0);
                    list.Add(MAPB);
                    DateTime dateTime = DateTime.Now;
                    THOIGIANinsert.Text = dateTime.ToString("dd/MM/yyyy");
                }
                if (list.Count > 0)
                {
                    MADAinsert.ItemsSource = list;
                    MADAinsert.SelectedItem = list[0];
                }
            }
            catch (Exception )
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
                    MANVinsert.ItemsSource = list;
                    MANVinsert.SelectedItem = list[0];
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
