using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for AddProject.xaml
    /// </summary>
    public partial class AddProject : Window
    {
        public delegate void sendMessage(string message);
        public event sendMessage OneMessage;
        public string _Username { get; set; }
        public string _Password { get; set; }
        private static OracleConnection con { get; set; } = null;


        public AddProject(string _Username, string _Password)
        {
            this._Username = _Username;
            this._Password = _Password;
            InitializeComponent();
            loadPhong();
            loadMada();
        }

        private void CreateConnection()
        {
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={this._Username};Password={this._Password};";
            con = new OracleConnection(conn);
        }

        private void loadPhong()
        {
            CreateConnection();
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"select * from system.phongban", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                ObservableCollection<string> list = new ObservableCollection<string>();
                while (reader.Read())
                {
                    string MAPB = reader.GetString(0);
                    list.Add(MAPB);
                }
                PHONGinsert.ItemsSource = list;
                if (list.Count > 0)
                    PHONGinsert.SelectedItem = list[0];
            }
            catch (Exception ex)
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
        private void loadMada()
        {
            CreateConnection();
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"select Max(MADA) from system.dean", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                
                if (reader.Read())
                {
                    string MADA = reader.GetString(0);
                    int temp = int.Parse(MADA);
                    temp++;
                    string newMADA = temp.ToString("0000");
                    DateTime dateTime = DateTime.Now;
                    NGAYBDinsert.Text = dateTime.ToString("dd/MM/yyyy");
                    MADAinsert.Text = newMADA;
                }
            }
            catch (Exception ex)
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

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            string mada = MADAinsert.Text;
            string tenda = TENDAinsert.Text;
            string ngaybd = NGAYBDinsert.Text;
            string phong =(string)PHONGinsert.SelectedValue;

            //check Day
            DateTime date;
            bool isValid = DateTime.TryParseExact(ngaybd, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if (isValid == false)
            {
                MessageBox.Show("Please fill NGAYSINH follow format: dd/mm/yyyy !!!");
                return;
            }

            //check Project name
            if (tenda == "")
            {
                MessageBox.Show("Please fill TENDA!!!");
                return;
            }

            CreateConnection();
            try
            {
                con.Open();
                OracleCommand command = con.CreateCommand();
                command.CommandText = "insert into system.DeAn values (:MADA, :TENDA, TO_DATE(:NGAYBD, 'DD/MM/YYYY'),:PHONG)";

                command.Parameters.Add("MADA", OracleDbType.Varchar2).Value = mada;
                command.Parameters.Add("TENDA", OracleDbType.Varchar2).Value = tenda;
                command.Parameters.Add("NGAYBD", OracleDbType.Varchar2).Value = ngaybd;
                command.Parameters.Add("PHONG",OracleDbType.Varchar2).Value = phong;

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
    }
}
