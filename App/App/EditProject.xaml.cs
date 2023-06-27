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
    /// Interaction logic for EditProject.xaml
    /// </summary>
    public partial class EditProject : Window
    {
        public delegate void sendMessage(string message, Project assign);
        public event sendMessage OneMessage;
        public Project Pro { get; set; }
        public string _Username { get; set; }
        public string _Password { get; set; }

        private static OracleConnection con { get; set; } = null;
        public EditProject(Project data, string _Username, string _Password)
        {
            this.Pro = data;
            this._Username = _Username;
            this._Password = _Password;
            DataContext = this.Pro;
            InitializeComponent();
            //loadMada();
            loadPhong();
        }

        private void CreateConnection()
        {
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={this._Username};Password={this._Password};";
            con = new OracleConnection(conn);
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Pro.MADA = MADA.Text;
            Pro.TENDA = TENDA.Text;
            Pro.NGAYBD = NGAYBD.Text;
            Pro.PHONG = (string)PHONG_Update.SelectedValue;

            CreateConnection();
            try
            {
                con.Open();
                OracleCommand command = con.CreateCommand();
                command.CommandText = $"UPDATE system.DEAN SET TENDA=:TENDA, NGAYBD = TO_DATE(:NGAYBD, 'DD/MM/YYYY'), PHONG=:PHONG where MADA = :MADA";
                command.Parameters.Add("TENDA", OracleDbType.Varchar2).Value = Pro.TENDA;
                command.Parameters.Add("NGAYBD", OracleDbType.Varchar2).Value = Pro.NGAYBD;
                command.Parameters.Add("PHONG", OracleDbType.Varchar2).Value = Pro.PHONG;
                command.Parameters.Add("MADA", OracleDbType.Varchar2).Value = Pro.MADA;

                int rowsUpdated = command.ExecuteNonQuery();
                if (rowsUpdated > 0)
                    OneMessage?.Invoke(new string("Success!!!"), Pro);
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
                PHONG_Update.ItemsSource = list;
                if (list.Count > 0)
                {
                    int index = list.IndexOf(Pro.PHONG);
                    PHONG_Update.SelectedItem = list[index];
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
    }
}
