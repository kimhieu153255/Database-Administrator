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
using System.Collections.ObjectModel;

namespace App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class roleInfor
    {
        public string role { get; set; }
        public string role_id { get; set; }

        public roleInfor(string role = "", string role_id ="")
        {
            this.role = role;
            this.role_id = role_id;
        }
    }
    public class Role
    {
        public string grantee { get; set; }
        public string privilege { get; set; }
        public string tableName { get; set; }
        public string grantable { get; set; }
        public string type { get; set; }

        public Role( string grantee, string privilege, string tableName,string grantable, string type)
        {
            this.grantee = grantee;
            this.privilege = privilege;
            this.tableName = tableName;
            this.grantable = grantable;
            this.type = type;
        }
    }
    public class Table
    {
        public string Name { get; set; }
        public bool Select { get; set; }
        public bool Insert { get; set; }
        public bool Update { get; set; } 
        public bool SelectW { get; set; } 
        public bool InsertW { get; set; } 
        public bool UpdateW { get; set; }
        public bool Delete { get; set; }
        public bool DeleteW { get; set; }

        public Table(string name, bool select = false, bool insert = false, bool update = false, bool delete = false, bool Sw = false, bool Iw = false, bool Uw = false, bool dW=false)
        {
            this.Name = name;
            this.Select = select;
            this.Insert = insert;
            this.Update = update;
            this.Delete = delete;
            this.SelectW = Sw;
            this.InsertW = Iw;
            this.UpdateW = Uw;
            this.DeleteW = dW;
        }

    }
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
        private ObservableCollection<Table> listtable { get; set; }



        public MainWindow()
        {
            listtable = new ObservableCollection<Table>();
            Table t = new Table("USER_AM");
            listtable.Add(t);
            listtable.Add(new Table("NHANVIEN"));
            listtable.Add(new Table("DEAN"));
            listtable.Add(new Table("PHONGBAN"));
            listtable.Add(new Table("PHANCONG"));
            InitializeComponent();
            this.SizeChanged += MainWindow_SizeChanged;
        }


        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            dataTable.MaxHeight = mainScreen.ActualHeight - 100;
            dataGrantTable.MaxHeight = mainScreen.ActualHeight - 300;
            dataPriRole.MaxHeight = mainScreen.ActualHeight - 300;
            dataRole.MaxHeight = mainScreen.ActualHeight - 300;
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
            TableGui.Visibility = Visibility.Collapsed;
            LoginGui.Visibility = Visibility.Collapsed;
            RegisterGui.Visibility = Visibility.Collapsed;
            DeleteGui.Visibility = Visibility.Collapsed;
            GrantTable.Visibility = Visibility.Collapsed;
            CreateDeleteRole.Visibility = Visibility.Collapsed;
            GrantRole.Visibility = Visibility.Collapsed;
            SearchPriRoleUser.Visibility = Visibility.Collapsed;
            About.Visibility = Visibility.Collapsed;
            switch (nameGui)
            {
                case "login":
                    LabeMainField.Content = "LOGIN";
                    LoginGui.Visibility = Visibility.Visible;
                    break;
                case "add":
                    LabeMainField.Content = "ADD USER";
                    RegisterGui.Visibility = Visibility.Visible;
                    break;
                case "delete":
                    LabeMainField.Content = "DELETE USER";
                    DeleteGui.Visibility = Visibility.Visible;
                    break;
                case "listAM":
                    LabeMainField.Content = "LIST ADMIN";
                    TableGui.Visibility = Visibility.Visible;
                    break;
                case "GrantPriv":
                    LabeMainField.Content = "GRANT PRIVILEGES";
                    GrantTable.Visibility = Visibility.Visible;
                    break;
                case "SearchRolePrivUser":
                    LabeMainField.Content = "SEARCH ROLE/PRIVILEGE OF USER";
                    SearchPriRoleUser.Visibility = Visibility.Visible;
                    break;
                case "CreateDeleteRole":
                    LabeMainField.Content = "CREATE OR DELETE ROLE";
                    CreateDeleteRole.Visibility = Visibility.Visible;
                    break;
                case "GrantRoleUser":
                    LabeMainField.Content = "GRANT ROLE TO USER";
                    GrantRole.Visibility = Visibility.Visible;
                    break;
                case "PrivRole":
                    LabeMainField.Content = "SEARCH PRIVILEGE OF ROLE";
                    SearchPriRole.Visibility = Visibility.Visible;
                    break;
                case "About":
                    LabeMainField.Content = "About Member of Group 5";
                    About.Visibility = Visibility.Visible;
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

        private void cleanCreateDeleteRole()
        {
            nameRoleCreateDelete.Text = string.Empty;
            errLabelRole.Content = string.Empty;
            errLabelRole.Visibility = Visibility.Collapsed;
        }

        private void cleanSearchRole()
        {
            dataRole.ItemsSource = string.Empty;
            dataRole.Visibility = Visibility.Collapsed;
            nameRoleSearch.Text = string.Empty;
            dataPriRole.Visibility = Visibility.Collapsed;
            errLabelSearchRole.Content = string.Empty;
            errLabelSearchRole.Visibility = Visibility.Collapsed;
            
        }

        private void cleanGrantRoleUser()
        {
            nameRoleGrant.Text = string.Empty;
            errLabelGrantRole.Content = string.Empty;
            errLabelGrantRole.Visibility = Visibility.Collapsed;
        }

        private void cleanPrivRole()
        {
            nameRole.Text = string.Empty;
            errLabelSearcPrivRole.Content = string.Empty;
            errLabelSearcPrivRole.Visibility = Visibility.Collapsed;
            dataPrivRole.ItemsSource = null;
            dataPrivRole.Visibility = Visibility.Collapsed;

            SearchPriRole.Visibility = Visibility.Collapsed;
        }

        private void clean()
        {
            cleanDelete();
            cleanLoginGui();
            cleanAddGui();
            cleanCreateDeleteRole();
            cleanSearchRole();
            cleanGrantRoleUser();
            cleanPrivRole();
        }

        private void password_changed(object sender, RoutedEventArgs e)
        {
            errLabelAdd.Visibility = Visibility.Visible;
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
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand("select * from system.USER_AM", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                BindingList<user> listT = new BindingList<user>();
                while (reader.Read())
                {
                    decimal sttOracle = reader.GetDecimal(reader.GetOrdinal("STT"));
                    int stt = Convert.ToInt32(sttOracle);
                    string fullName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string username = reader.GetString(2);
                    string role = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    user user = new user(stt, fullName, username, role);
                    listT.Add(user);
                }
                dataTable.Height = mainScreen.ActualHeight - 100;
                dataTable.ItemsSource = listT;
                con.Close();
                con.Dispose();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Btn_About(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("About");
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
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
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
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
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
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={username};Password={password};";
            OracleConnection connection = new(conn);
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
            changeGuiLogged("login");
            SearchPriRole.Visibility = Visibility.Collapsed;
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

            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
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
                errLabelAdd.Content = "Can't add user!";
                errLabelAdd.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void Btn_GrantRevoke_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("GrantPriv");
            displayTableRole();
            dataGrantTable.Visibility = Visibility.Visible;
        }

        private void Btn_Revoke(object sender, RoutedEventArgs e)
        {
            GrantRevokePrivTable(false);
        }

        private void Btn_Grant(object sender, RoutedEventArgs e)
        {
            GrantRevokePrivTable(true);
        }

        private void Btn_CreateDeleteRole_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("CreateDeleteRole");
        }

        
        private void Btn_GrantRoleUser_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("GrantRoleUser");
        } 

        private void Btn_SearchRole_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("SearchRolePrivUser");
        }

        private void Btn_DeleteRole(object sender, RoutedEventArgs e)
        {
            string roleName = nameRoleCreateDelete.Text;
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open(); 
                OracleCommand oracleCommand = new OracleCommand("SYSTEM.DELETE_ROLE", con);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("ROLE_NAME", OracleDbType.Varchar2).Value = roleName;
                oracleCommand.Parameters.Add("RESULT", OracleDbType.Varchar2, 40).Direction = ParameterDirection.Output;
                oracleCommand.ExecuteNonQuery();
                string resultvalue = (string)oracleCommand.Parameters["RESULT"].Value.ToString();
                con.Close();
                con.Dispose();
                errLabelRole.Content = resultvalue;
                errLabelRole.Visibility = Visibility.Visible;
                if (resultvalue == "SUCCESS DELETE ROLE")
                    errLabelRole.Foreground = new SolidColorBrush(Colors.Green);
                else
                    errLabelRole.Foreground = new SolidColorBrush(Colors.Red);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Btn_PrivRole_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("PrivRole");
        }
        private void Btn_CreateRole(object sender, RoutedEventArgs e)
        {
            string roleName = nameRoleCreateDelete.Text;
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand("SYSTEM.CREATE_ROLE", con);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("ROLE_NAME", OracleDbType.Varchar2).Value = roleName;
                oracleCommand.Parameters.Add("RESULT", OracleDbType.Varchar2, 200).Direction = ParameterDirection.Output;
                oracleCommand.ExecuteNonQuery();
                string resultvalue = (string) oracleCommand.Parameters["RESULT"].Value.ToString();
                con.Close();
                con.Dispose();
                errLabelRole.Content = resultvalue;
                errLabelRole.Visibility = Visibility.Visible;
                if (resultvalue == "SUCCESS CREATE ROLE")
                    errLabelRole.Foreground = new SolidColorBrush(Colors.Green);
                else
                    errLabelRole.Foreground = new SolidColorBrush(Colors.Red);
            }
            catch (Exception ex)
            {
                errLabelRole.Content = ex.Message;
                errLabelRole.Visibility = Visibility.Visible;
                return;
            }
        }
        

        private void Btn_SearchRole(object sender, RoutedEventArgs e)
        {
            dataPriRole.Visibility = Visibility.Collapsed;
            string name = nameRoleSearch.Text;
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                //SELECT * FROM system.dba_role_privs where Granted_role ='KIMHIEU';
                OracleCommand oracleCommand = new OracleCommand($"SELECT * FROM DBA_ROLES WHERE ROLE = UPPER('{name}')", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                BindingList<roleInfor> roleList = new BindingList<roleInfor>();
                while (reader.Read())
                {
                    string role = reader.GetString(0);
                    string roleId = reader.GetString(1);
                    roleInfor r = new roleInfor(role, roleId);
                    roleList.Add(r);
                }
                if (roleList.Count() != 0)
                {
                    dataRole.ItemsSource = roleList;
                    dataRole.Visibility = Visibility.Visible;
                    errLabelSearchRole.Visibility = Visibility.Collapsed;
                }
                else
                {
                    errLabelSearchRole.Content = "Role isn't exists!";
                    errLabelSearchRole.Foreground = new SolidColorBrush(Colors.Red);
                    errLabelSearchRole.Visibility = Visibility.Visible;
                }
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                errLabelSearchRole.Content = ex.Message;
                errLabelSearchRole.Visibility = Visibility.Visible;
                return;
            }
        }

        private void Btn_SearchRoleUser(object sender, RoutedEventArgs e)
        {
            dataPrivRole.Visibility = Visibility.Collapsed;
            string name = nameRole.Text;
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"SELECT * FROM sys.dba_role_privs where grantee = UPPER('{name}')", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                BindingList<roleInfor> roleList = new BindingList<roleInfor>();
                while (reader.Read())
                {
                    string grated_role = reader.GetString(1);
                    string adminOption = reader.GetString(2);
                    roleInfor r = new roleInfor(grated_role, adminOption);
                    roleList.Add(r);
                }
                if (roleList.Count() != 0)
                {
                    dataPrivRole.Visibility = Visibility.Visible;
                    dataPrivRole.ItemsSource = roleList;
                }
                else
                {
                    errLabelSearcPrivRole.Content = "Role isn't exists!";
                    errLabelSearcPrivRole.Foreground = new SolidColorBrush(Colors.Red);
                    errLabelSearcPrivRole.Visibility = Visibility.Visible;
                }
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                errLabelSearchRole.Content = ex.Message;
                errLabelSearchRole.Visibility = Visibility.Visible;
                return;
            }
        }
        private void Btn_SearchPrivilege(object sender, RoutedEventArgs e)
        {
            dataRole.Visibility= Visibility.Collapsed;
            string name = nameRoleSearch.Text;
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"SELECT * FROM DBA_TAB_PRIVS WHERE GRANTEE = UPPER('{name}') AND (TYPE = 'TABLE' OR TYPE = 'VIEW')", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                BindingList<Role> roleList = new BindingList<Role>();
                while (reader.Read())
                {
                    string grantee = reader.GetString(0);
                    string tableName = reader.GetString(2);
                    string privilege = reader.GetString(4);
                    string grantable = reader.GetString(5);
                    string type = reader.GetString(8);
                    Role r = new Role(grantee,privilege,tableName,grantable,type);
                    roleList.Add(r);
                }
                if (roleList.Count() != 0)
                {
                    dataPriRole.Visibility = Visibility.Visible;
                    dataPriRole.ItemsSource = roleList;
                    errLabelSearchRole.Visibility = Visibility.Collapsed;
                }
                else { 
                    errLabelSearchRole.Content = "Role doesn't have any privileges";
                    errLabelSearchRole.Foreground = new SolidColorBrush(Colors.Red);
                    errLabelSearchRole.Visibility = Visibility.Visible;
                }
                con.Close();
                con.Dispose();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Btn_GrantRoleUser(object sender, RoutedEventArgs e)
        {
            string roleName = nameRoleGrant.Text;
            string username = nameUser.Text;
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand("SYSTEM.GRANT_ROLE_TO_USER", con);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("ROLE_NAME", OracleDbType.Varchar2).Value = roleName;
                oracleCommand.Parameters.Add("USER_NAME", OracleDbType.Varchar2).Value = username;
                oracleCommand.Parameters.Add("RESULT", OracleDbType.Varchar2, 200).Direction = ParameterDirection.Output;
                oracleCommand.ExecuteNonQuery();
                string resultvalue = (string)oracleCommand.Parameters["RESULT"].Value.ToString();
                con.Close();
                con.Dispose();
                errLabelGrantRole.Content = resultvalue;
                errLabelGrantRole.Visibility = Visibility.Visible;
                if (resultvalue == "SUCCESS")
                    errLabelGrantRole.Foreground = new SolidColorBrush(Colors.Green);
                else
                    errLabelGrantRole.Foreground = new SolidColorBrush(Colors.Red);
            }
            catch (Exception)
            {
                return;
            }

        }

        private string GrantPrivilege(bool t,string priv, string nameObject, string nameTable, int flag)
        {
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = t ? new OracleCommand("SYSTEM.GRANT_PRIVS_TAB_USER_OR_ROLE", con) : new OracleCommand("SYSTEM.REVOKE_PRIVS_VIEW_USER_OR_ROLE", con);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("NAME", OracleDbType.Varchar2).Value = nameObject;
                oracleCommand.Parameters.Add("PRIVS", OracleDbType.Varchar2).Value = priv;
                oracleCommand.Parameters.Add("TAB_NAME", OracleDbType.Varchar2).Value = nameTable;
                oracleCommand.Parameters.Add("WITH_GRANT_OPTION", OracleDbType.Int16).Value = flag;
                oracleCommand.Parameters.Add("RESULT", OracleDbType.Varchar2, 40).Direction = ParameterDirection.Output;
                oracleCommand.ExecuteNonQuery();
                string resultvalue = (string)oracleCommand.Parameters["RESULT"].Value.ToString();
                con.Close();
                con.Dispose();
                if (resultvalue == "SUCCESS")
                    return "";
                else
                    return resultvalue;
            }
            catch (Exception)
            {
                return "Privilege doesn't grant to user or role!";
            }
        }

        private void GrantRevokePrivTable(bool flag)
        {
            string nameObject = nameUserRole.Text;
            BindingList<string> listError = new BindingList<string>();
            foreach (Table table in dataGrantTable.Items) 
            {
                string name = table.Name;
                bool select = table.Select;
                if (select)
                {
                    string err = GrantPrivilege(flag, "Select", nameObject, name, 0);
                    if (err != "")
                        listError.Add(err);
                }

                bool insert = table.Insert;
                if (insert)
                {
                    string err = GrantPrivilege(flag, "insert", nameObject, name, 0);
                    if (err != "")
                        listError.Add(err);
                }
                bool update = table.Update;
                if (update)
                {
                    string err = GrantPrivilege(flag, "update", nameObject, name, 0);
                    if (err != "")
                        listError.Add(err);
                }
                bool delete = table.Delete;
                if (delete)
                {
                    string err = GrantPrivilege(flag, "delete", nameObject, name, 0);
                    if (err != "")
                        listError.Add(err);
                }
                bool selectW = table.SelectW;
                if (selectW)
                {
                    string err = GrantPrivilege(flag, "Select", nameObject, name, 1);
                    if (err != "")
                        listError.Add(err);
                }
                bool insertW = table.InsertW;
                if (insertW)
                {
                    string err = GrantPrivilege(flag, "insert", nameObject, name, 1);
                    if (err != "")
                        listError.Add(err);
                }
                bool updateW = table.UpdateW;
                if (updateW)
                {
                    string err = GrantPrivilege(flag, "update", nameObject, name, 1);
                    if (err != "")
                        listError.Add(err);
                }
                bool deleteW = table.DeleteW;
                if (deleteW)
                {
                    string err = GrantPrivilege(flag, "delete", nameObject, name, 1);
                    if (err != "")
                        listError.Add(err);
                }
                dataRoleErr.ItemsSource = listError;
                dataRoleErr.Visibility = Visibility.Visible;
            }
        }

        private void displayTableRole()
        {
            dataGrantTable.ItemsSource = listtable;
            dataRoleErr.Visibility = Visibility.Collapsed;
        }
    }
}
