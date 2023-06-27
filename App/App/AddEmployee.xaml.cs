using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for InsertEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        public AddEmployee(string _Username,string _Password)
        {
            this._Username = _Username;
            this._Password = _Password;
            InitializeComponent();
            loadPhong();
            loadMANV();
        }

        public delegate void sendMessage(string message);
        public event sendMessage OneMessage;
        public string _Username { get; set; }
        public string _Password { get; set; }


        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            string manv = MANV.Text;
            string tennv = TENNV.Text;
            string phai = ((ComboBoxItem)PHAI.SelectedItem).Content.ToString();
            string ngaysinh = NGAYSINH.Text;
            string diachi = DIACHI.Text;
            string sodt = SODT.Text;
            string manql = MANQL.Text;
            string phg = (string)PHG.SelectedValue;

            // check empty
            if(tennv == ""||ngaysinh==""||sodt == ""||diachi == "" )
            {
                MessageBox.Show("Please fill TENV, NGAYSINH, DIACHI and VAITRO!!!");
                return;
            }

            //check birthday is valid
            DateTime date;
            bool isValid = DateTime.TryParseExact(ngaysinh, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if(isValid == false)
            {
                MessageBox.Show("Please fill NGAYSINH follow format: dd/mm/yyyy !!!");
                return;
            }
            // check phone number
            if(Regex.IsMatch(sodt, @"^\d{10}$")==false)
            {
                MessageBox.Show("Phone number invalid!!!");
                return;
            }    

            MessageBox.Show(manv + " " + tennv + " " + phai + " " + ngaysinh + " " + diachi + " " + sodt + " " + manql + " " + phg);
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand command = con.CreateCommand();
                command.CommandText = "insert into system.nhanvien(manv,tennv,phai,ngaysinh,diachi,sodt,manql,phg) values (:MANV, :TENNV,:PHAI , TO_DATE(:NGAYSINH, 'DD/MM/YYYY'), :DIACHI, :SODT, :MANQL, :PHG)";
                command.Parameters.Add("MANV", OracleDbType.Varchar2).Value = manv;
                command.Parameters.Add("TENNV", OracleDbType.Varchar2).Value = tennv;
                command.Parameters.Add("PHAI", OracleDbType.Varchar2).Value = phai;
                command.Parameters.Add("NGAYSINH", OracleDbType.Varchar2).Value = ngaysinh;
                command.Parameters.Add("DIACHI", OracleDbType.Varchar2).Value = diachi;
                command.Parameters.Add("SODT", OracleDbType.Varchar2).Value = sodt; 
                command.Parameters.Add("MANQL", OracleDbType.Varchar2).Value = (manql.Equals("") ? DBNull.Value : manql);
                command.Parameters.Add("PHG", OracleDbType.Varchar2).Value = (phg.Equals("") ? DBNull.Value : phg);

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

        // generate MANV next
        private void loadMANV()
        {
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"select Max(manv) from system.nhanvien", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                if (reader.Read())
                {
                    string manv = reader.GetString(0);
                    int temp = int.Parse(manv);
                    temp++;
                    string newMANV = temp.ToString("0000");
                    MessageBox.Show(newMANV);
                    MANV.Text = newMANV;
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

        //Load MAPB to combobox
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
                PHG.ItemsSource = list;
                if (list.Count > 0)
                    PHG.SelectedItem = list[0];
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

        private void Cancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
