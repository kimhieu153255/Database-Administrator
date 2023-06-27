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
        public EditProject(Project data, string _Username, string _Password)
        {
            this.Pro = data;
            this._Username = _Username;
            this._Password = _Password;
            DataContext = this.Pro;
            InitializeComponent();
            loadMada();
            loadPhong();
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Pro.MADA = MADA.Text;
            Pro.TENDA = TENDA.Text;
            Pro.NGAYBD = NGAYBD.Text;
            Pro.PHONG = (string)PHONG_Update.SelectedValue;
            MessageBox.Show(Pro.MADA + " " + Pro.TENDA + " " + Pro.NGAYBD + " " + Pro.PHONG);

            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand command = con.CreateCommand();
                command.CommandText = $"UPDATE system.DEAN SET TENDA=:TENDA, NGAYBD =: TO_DATE(:NGAYBD, 'DD/MM/YYYY'), PHONG=:PHONG where MADA = :MADA";
                command.Parameters.Add("TENDA", OracleDbType.Varchar2).Value = Pro.TENDA;
                command.Parameters.Add("NGAYBD", OracleDbType.Varchar2).Value = Pro.NGAYBD;
                command.Parameters.Add("PHONG", OracleDbType.Varchar2).Value = Pro.PHONG;
                command.Parameters.Add("MADA", OracleDbType.Varchar2).Value = Pro.MADA;

                int rowsUpdated = command.ExecuteNonQuery();
                MessageBox.Show("update: " + rowsUpdated);
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
            MessageBox.Show("cancel");
            this.Close();
        }

        private void loadPhong()
        {
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
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
                MessageBox.Show(ex.Message);
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
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"select Max(MADA) from system.dean", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();

                if (reader.Read())
                {
                    string mada = reader.GetString(0);
                    int temp = int.Parse(mada);
                    temp++;
                    string newMADA = temp.ToString("0000");
                    MADA.Text = newMADA;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
