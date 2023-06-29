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
    /// Interaction logic for EditEmployee.xaml
    /// </summary>
    public partial class EditEmployee : Window
    {
        public delegate void sendMessage(string message);
        public event sendMessage OneMessage;
        public string role_user { get; set; }
        public string _Username { get; set; }
        public string _Password { get; set; }
        public Employee Emp { get; set; }
        private static OracleConnection con { get; set; } = null;

        public EditEmployee(string role_user, string _Username, string _Password, Employee employee)
        {
            this.role_user = role_user;
            this._Username = _Username;
            this._Password = _Password;
            this.Emp = employee;
            DataContext = this.Emp;
            InitializeComponent();
            loadPhong();
            loadGender();
            changeGui(role_user);
        }
        private void CreateConnection()
        {
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={this._Username};Password={this._Password};";
            con = new OracleConnection(conn);
        }
        private void loadGender()
        {
            if (Emp.PHAI == "Nam")
                Male.IsSelected = true;
            if(Emp.PHAI =="Nu")
                Female.IsSelected = true;
        }
        private void changeGui(string role)
        {
            if(role =="Tai chinh")
            {
                Finance.Visibility = Visibility.Visible;
                TENNV.IsReadOnly = true;
                PHAI.IsEnabled = false;
                NGAYSINH.IsReadOnly = true;
                DIACHI.IsReadOnly = true;
                SODT.IsReadOnly = true;
                MANQL.IsReadOnly = true;
                PHG.IsEnabled = false;

                TENNV.Background = new SolidColorBrush(Colors.LightGray);
                NGAYSINH.Background = new SolidColorBrush(Colors.LightGray);    
                DIACHI.Background = new SolidColorBrush(Colors.LightGray);
                SODT.Background = new SolidColorBrush(Colors.LightGray);
                MANQL.Background = new SolidColorBrush(Colors.LightGray);

                this.Height = this.ActualHeight + 500;
            }
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            string manv = MANV.Text;
            string tennv = TENNV.Text;
            string phai = ((ComboBoxItem)PHAI.SelectedItem).Content.ToString();
            string ngaysinh = NGAYSINH.Text;
            string diachi = DIACHI.Text;
            string sodt = SODT.Text;
            string luong = LUONG_Finance.Text;
            string phucap = PHUCAP_Finance.Text;
            string manql = MANQL.Text;
            string phg = (string)PHG.SelectedValue;

            RSA rsa = new RSA(512);
            string pubkey = rsa.GetPublicKey();
            rsa.ExportPrivateKeyToFile("../../../keys/" + manv + ".xml");

            // check empty
            if (tennv == "" || ngaysinh == "" || sodt == "" || diachi == "")
            {
                MessageBox.Show("Please fill TENV, NGAYSINH, SODT and DIACHI!!!");
                return;
            }

            //check birthday is valid
            DateTime date;
            bool isValid = DateTime.TryParseExact(ngaysinh, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if (isValid == false)
            {
                MessageBox.Show("Please fill NGAYSINH follow format: dd/mm/yyyy !!!");
                return;
            }
            // check phone number
            if (Regex.IsMatch(sodt, @"^\d{10}$") == false)
            {
                MessageBox.Show("Phone number invalid!!!");
                return;
            }

            if (role_user == "Tai chinh")
            { 
                if(Int32.TryParse(luong, out Int32 number)==false)
                {
                    MessageBox.Show("Salary must be a number!!!");
                    return;
                }
                else if(Int32.TryParse(phucap, out Int32 number1)==false)
                {
                    MessageBox.Show("Allowance must be a number!!!");
                    return;
                }
            }
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand command = con.CreateCommand();
                //==================================================================================Personel,  Finance update======================================================================================
                if (role_user == "Nhan su")
                {
                    command.CommandText = $"UPDATE system.nhanvien SET TENNV=:TENNV, PHAI=:PHAI, NGAYSINH = TO_DATE(:NGAYSINH, 'DD/MM/YYYY'),DIACHI=:DIACHI,SODT=:SODT,MANQL=:MANQL,PHG=:PHG where MANV=:MANV";
                    command.Parameters.Add("TENNV", OracleDbType.Varchar2).Value = tennv;
                    command.Parameters.Add("PHAI", OracleDbType.Varchar2).Value = phai;
                    command.Parameters.Add("NGAYSINH", OracleDbType.Varchar2).Value = ngaysinh;
                    command.Parameters.Add("DIACHI", OracleDbType.Varchar2).Value = diachi;
                    command.Parameters.Add("SODT", OracleDbType.Varchar2).Value = sodt;
                    command.Parameters.Add("MANQL", OracleDbType.Varchar2).Value = manql=="NULL"?DBNull.Value:manql;
                    command.Parameters.Add("PHG", OracleDbType.Varchar2).Value = phg;
                    command.Parameters.Add("MANV", OracleDbType.Varchar2).Value = manv;
                }
                else if(role_user == "Tai chinh")
                {
                    command.CommandText = $"UPDATE system.nhanvien SET LUONG=:LUONG, PHUCAP=:PHUCAP where MANV=:MANV";
                    command.Parameters.Add("LUONG", OracleDbType.Raw).Value = rsa.Encrypt(luong);
                    command.Parameters.Add("PHUCAP", OracleDbType.Raw).Value = rsa.Encrypt(phucap);
                    command.Parameters.Add("MANV", OracleDbType.Varchar2).Value = manv;
                }
                int rowsUpdated = command.ExecuteNonQuery();
                if (rowsUpdated > 0)
                    OneMessage?.Invoke(new string("Success!!!"));
            }
            catch (Exception)
            {
                MessageBox.Show("Have a problem!!!");
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
        //Load MAPB to combobox
        private void loadPhong()
        {
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
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
                {
                    int index = list.IndexOf(Emp.PHG);
                    PHG.SelectedItem = list[index];
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
