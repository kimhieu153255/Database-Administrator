using System;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.ComponentModel;

namespace App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class user
    {
        public int STT { get; set; }
        public string fullname { get; set; }
        public string username { get; set; }
        public string role { get; set; }

        public user(int stt,string fullname, string username, string role)
        {
            this.STT = stt;
            this.fullname = fullname;
            this.username = username;
            this.role = role;
        }
     }
    public partial class MainWindow : Window
    {
        private string _Username { get; set; } = string.Empty;
        private string _Password { get; set; } = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            this.SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            dataTable.Height = mainScreen.ActualHeight - 100;
        }

        private void Btn_Login_show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("login");
        }


        private void Btn_Add_show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("add");
        }


        private void Btn_ListAdmin_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("listAM");
            displayTableListUser();
        }


        private void Btn_Delete_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("delete");
        }


        private void changeGuiLogged(string nameGui)
        {
            clean();
            switch (nameGui)
            {
                case "login":
                    LabeMainField.Content = "LOGIN";
                    TableGui.Visibility = Visibility.Collapsed;
                    LoginGui.Visibility = Visibility.Visible;
                    RegisterGui.Visibility = Visibility.Collapsed;
                    DeleteGui.Visibility = Visibility.Collapsed;
                    break;
                case "add":
                    LabeMainField.Content = "ADD USER";
                    TableGui.Visibility = Visibility.Collapsed;
                    LoginGui.Visibility = Visibility.Collapsed;
                    RegisterGui.Visibility = Visibility.Visible;
                    DeleteGui.Visibility = Visibility.Collapsed;
                    break;
                case "delete":
                    LabeMainField.Content = "DELETE USER";
                    TableGui.Visibility = Visibility.Collapsed;
                    LoginGui.Visibility = Visibility.Collapsed;
                    RegisterGui.Visibility = Visibility.Collapsed;
                    DeleteGui.Visibility = Visibility.Visible;
                    break;
                case "listAM":
                    LabeMainField.Content = "LIST ADMIN";
                    TableGui.Visibility = Visibility.Visible;
                    LoginGui.Visibility = Visibility.Collapsed;
                    RegisterGui.Visibility = Visibility.Collapsed;
                    DeleteGui.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }


        private void cleanAddGui()
        {
            fullnameRegister.Clear();
            usernameBoxRegister.Clear();
            passwordBoxRegister.Clear();
            againPasswordBoxRegister.Clear();
            errLabelAdd.Content = string.Empty;
        }


        private void cleanLoginGui()
        {
            usernameBox.Clear();
            passwordBox.Clear();
            errLabel.Content = string.Empty;
        }


        private void cleanDelete()
        {
            dataTableSearch.Visibility = Visibility.Collapsed;
            dataTableSearch.ItemsSource = null;
            usernameDelete.Text = "";
            errLabelDelete.Content = "";
            errLabelDelete.Visibility = Visibility.Collapsed;
        }


        private void clean()
        {
            cleanDelete();
            cleanLoginGui();
            cleanAddGui();
        }


        private void password_changed(object sender, RoutedEventArgs e)
        {
            if (passwordBoxRegister.Password != againPasswordBoxRegister.Password)
            {
                errLabelAdd.Content = "Password dont match!!!";
                errLabelAdd.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                errLabelAdd.Content = "Password match!!";
                errLabelAdd.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
        

        public void displayTableListUser()
        {
            string conn = $"Data Source=Legion5/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand("select * from system.USER_AM", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                BindingList<user> list = new BindingList<user>();
                while (reader.Read())
                {
                    decimal sttOracle = reader.GetDecimal(reader.GetOrdinal("STT"));
                    int stt = Convert.ToInt32(sttOracle);
                    string fullName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string username = reader.GetString(2);
                    string role = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    user user = new user(stt, fullName, username, role);
                    list.Add(user);
                }
                dataTable.Height = mainScreen.ActualHeight - 100;
                dataTable.ItemsSource = list;
                con.Close();
                con.Dispose();
            }
            catch (Exception)
            {
                return;
            }
        }


        private void Btn_Search(object sender, RoutedEventArgs e)
        {
            string username = usernameDelete.Text.ToUpper();

            if (string.IsNullOrEmpty(username))
            {
                errLabelDelete.Content = "Please fill Username!";
                errLabelDelete.Visibility = Visibility.Visible;
                errLabelDelete.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }
            string conn = $"Data Source=Legion5/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"select * from system.USER_AM where username = '{username}'", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                BindingList<user> list = new BindingList<user>();
                while (reader.Read())
                {
                    decimal sttOracle = reader.GetDecimal(reader.GetOrdinal("STT"));
                    int stt = Convert.ToInt32(sttOracle);
                    string fullName = reader.GetString(1);
                    user user = new user(stt, fullName, username, "");
                    list.Add(user);
                }
                dataTableSearch.Visibility = Visibility.Visible;
                dataTableSearch.ItemsSource = list;
                con.Close();
                con.Dispose();
            }
            catch (Exception)
            {
                return;
            }

        }


        private void Btn_Delete(object sender, RoutedEventArgs e)
        {
            dataTableSearch.Visibility = Visibility.Collapsed;

            errLabelDelete.Content = "";
            errLabelDelete.Visibility = Visibility.Collapsed;
            string username = usernameDelete.Text.ToUpper();

            if (string.IsNullOrEmpty(username))
            {
                errLabelDelete.Content = "Please fill Username!";
                errLabelDelete.Visibility = Visibility.Visible;
                errLabelDelete.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }
            string conn = $"Data Source=Legion5/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand("SYSTEM.DELETE_USER", con);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("username", OracleDbType.Varchar2).Value = username;
                oracleCommand.Parameters.Add("RESULT", OracleDbType.Varchar2, 40).Direction = ParameterDirection.Output;
                oracleCommand.ExecuteNonQuery();
                string resultvalue = (string)oracleCommand.Parameters["RESULT"].Value.ToString();
                con.Close();
                con.Dispose();
                errLabelDelete.Content = resultvalue;
                errLabelDelete.Visibility = Visibility.Visible;
                if (resultvalue == "User deleted successfully")
                    errLabelDelete.Foreground = new SolidColorBrush(Colors.Green);
                else
                    errLabelDelete.Foreground = new SolidColorBrush(Colors.Red);
            }
            catch (Exception)
            {
                return;
            }
        }


        private void Btn_Login(object sender, RoutedEventArgs e)
        {
            string username = usernameBox.Text;
            IntPtr bstr = Marshal.SecureStringToBSTR(passwordBox.SecurePassword);
            string password = Marshal.PtrToStringBSTR(bstr);
            Marshal.ZeroFreeBSTR(bstr);
            if (username == "" || password == "")
            {
                errLabel.Content = "Please fill usernam and password!!";
                errLabel.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }
            string connectionString = $"Data Source=Legion5/XEPDB1;User Id={username};Password={password};";
            OracleConnection connection = new(connectionString);
            try
            {
                connection.Open();
                _Username = username;
                _Password = password;
                startPanel.Visibility = Visibility.Collapsed;
                LoggedPanel.Visibility = Visibility.Visible;
                changeGuiLogged("listAM");
                //Display list admin
                displayTableListUser();
                cleanLoginGui();
            }
            catch (Exception)
            {
                errLabel.Content = "User isn't exists!";
                errLabel.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }


        private void Btn_Logout(object sender, RoutedEventArgs e)
        {
            _Username = string.Empty;
            _Password = string.Empty;
            Btn_Login_show(sender, e);
            //sidebar
            startPanel.Visibility = Visibility.Visible;
            LoggedPanel.Visibility = Visibility.Collapsed;
        }


        private void Btn_Add(object sender, RoutedEventArgs e)
        {
            string username = usernameBoxRegister.Text;
            string fullname = fullnameRegister.Text;

            IntPtr bstr = Marshal.SecureStringToBSTR(passwordBoxRegister.SecurePassword);
            string password = Marshal.PtrToStringBSTR(bstr);
            Marshal.ZeroFreeBSTR(bstr);

            IntPtr bstr1 = Marshal.SecureStringToBSTR(againPasswordBoxRegister.SecurePassword);
            string passwordAgain = Marshal.PtrToStringBSTR(bstr);
            Marshal.ZeroFreeBSTR(bstr1);

            if (username == "" || password == "" || fullname == "" || passwordAgain == "")
            {
                errLabelAdd.Content = "Please fill all Field!";
                errLabelAdd.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }
            if (password != passwordAgain)
            {
                errLabelAdd.Content = "Password don't match";
                return;
            }

            string conn = $"Data Source=Legion5/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand("SYSTEM.USER_REGISTER", con);

                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("username", OracleDbType.Varchar2).Value = username;
                oracleCommand.Parameters.Add("fullname", OracleDbType.Varchar2).Value = fullname;
                oracleCommand.Parameters.Add("password", OracleDbType.Varchar2).Value = password;
                oracleCommand.Parameters.Add("RESULT", OracleDbType.Varchar2, 40).Direction = ParameterDirection.Output;
                oracleCommand.ExecuteNonQuery();
                string resultvalue = (string)oracleCommand.Parameters["RESULT"].Value.ToString();
                con.Close();
                con.Dispose();
                if (resultvalue == "SUCCESS")
                {
                    Btn_ListAdmin_Show(sender, e);
                    cleanAddGui();
                }
            }
            catch (Exception)
            {
                errLabelAdd.Content = "User already exists!";
                errLabelAdd.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
