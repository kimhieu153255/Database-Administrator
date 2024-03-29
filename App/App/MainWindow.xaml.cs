﻿using System;
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
using System.Text.RegularExpressions;
using System.Globalization;

namespace App
{
    public class roleInfor
    {
        public string role { get; set; }
        public string role_id { get; set; }

        public roleInfor(string role = "", string role_id = "")
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

        public Role(string grantee, string privilege, string tableName, string grantable, string type)
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

        public Table(string name, bool select = false, bool insert = false, bool update = false, bool delete = false, bool Sw = false, bool Iw = false, bool Uw = false, bool dW = false)
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

        public user(int stt, string fullname, string username, string role)
        {
            this.STT = stt;
            this.fullname = fullname;
            this.username = username;
            this.role = role;
        }
    }
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _Username { get; set; } = string.Empty;
        private string _Password { get; set; } = string.Empty;
        private ObservableCollection<Table> listPrivTableAdmin { get; set; }
        private static string role_user { get; set; } = string.Empty;

        private static OracleConnection con { get; set; } = null;
        
        public MainWindow()
        {
            listPrivTableAdmin = new ObservableCollection<Table>();
            Table t = new Table("USER_AM");
            listPrivTableAdmin.Add(t);
            listPrivTableAdmin.Add(new Table("NHANVIEN"));
            listPrivTableAdmin.Add(new Table("DEAN"));
            listPrivTableAdmin.Add(new Table("PHONGBAN"));
            listPrivTableAdmin.Add(new Table("PHANCONG"));
            InitializeComponent();
            this.SizeChanged += MainWindow_SizeChanged;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void CreateConnection()
        {
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            con = new OracleConnection(conn);
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // for Admin Gui
            dataTable.MaxHeight = mainScreen.ActualHeight - 100;
            dataGrantTable.MaxHeight = mainScreen.ActualHeight - 300;
            dataPriRole.MaxHeight = mainScreen.ActualHeight - 300;
            dataRole.MaxHeight = mainScreen.ActualHeight - 300;
            // for Employee Gui
            EmployeeManagement.MaxHeight = mainScreen.ActualHeight - 180;
            EmployeeManagement.MaxWidth = mainScreen.ActualWidth - 10;

            ShowDeAnTable.MaxWidth = mainScreen.ActualWidth - 20;
            ShowDeAnTable.MaxHeight = mainScreen.ActualHeight - 180;

            ShowPhongBanTable.MaxWidth = mainScreen.ActualWidth - 40;
            ShowPhongBanTable.MaxHeight = mainScreen.ActualHeight - 150;

            ShowAssignmentTable.MaxWidth = mainScreen.ActualWidth - 40;
            ShowAssignmentTable.MaxHeight = mainScreen.ActualHeight - 180;
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
            LoadListUserAdmin();
        }


        private void Btn_Delete_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("delete");
        }


        private void changeGuiLogged(string nameGui)
        {
            cleanAdminGui();
            //for admin
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
                    LabeMainField.Content = "SEARCH PRIVILEGE OF ROLE OR USER";
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
                    LabeMainField.Content = "SEARCH ROLE OF USER";
                    SearchPriRole.Visibility = Visibility.Visible;
                    break;
                case "About":
                    LabeMainField.Content = "ABOUT MEMBERS OF GROUP 5";
                    About.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void changeGuiEmployeeLogged(string nameGui)
        {
            cleanEmployeeGui();
            LoginGui.Visibility = Visibility.Collapsed;
            //monitor
            managementEmployee.Visibility = Visibility.Collapsed;
            Btn_Employee_Edit.Visibility = Visibility.Collapsed;
            Btn_Employee_Insert.Visibility = Visibility.Collapsed;
            //about user
            AboutEmployee.Visibility = Visibility.Collapsed;
            //show Project
            ShowDeAn.Visibility = Visibility.Collapsed;
            Btn_Project_Edit.Visibility =Visibility.Collapsed;
            Btn_Project_Delete.Visibility = Visibility.Collapsed;
            Btn_Project_Insert.Visibility = Visibility.Collapsed;
            //show Department
            ShowPhongBan.Visibility = Visibility.Collapsed;
            //show Assignment
            ShowAssignment.Visibility = Visibility.Collapsed;
            Btn_Assignment_Delete.Visibility = Visibility.Collapsed;
            Btn_Assignment_Edit.Visibility = Visibility.Collapsed;
            Btn_Assignment_Insert.Visibility=Visibility.Collapsed;

            switch (nameGui)
            {
                case "login":
                    LabeMainField.Content = "LOGIN";
                    LoginGui.Visibility = Visibility.Visible;
                    break;
                case "ManagementEmployee":
                    LabeMainField.Content = "MONITOR EMPLOYEE";
                    managementEmployee.Visibility = Visibility.Visible;
                    if (role_user == "Nhan su")
                    {
                        Btn_Employee_Edit.Visibility = Visibility.Visible;
                        Btn_Employee_Insert.Visibility = Visibility.Visible;
                    }
                    else if(role_user =="Tai chinh")
                        Btn_Employee_Edit.Visibility = Visibility.Visible;
                    break;
                case "AboutEmployee":
                    LabeMainField.Content = "ABOUT EMPLOYEE";
                    AboutEmployee.Visibility = Visibility.Visible;
                    break;
                case "ShowDeAnTable":
                    LabeMainField.Content = "PROJECTS";
                    ShowDeAn.Visibility = Visibility.Visible;
                    if(role_user=="Truong de an")
                    {
                        Btn_Project_Edit.Visibility = Visibility.Visible;
                        Btn_Project_Delete.Visibility = Visibility.Visible;
                        Btn_Project_Insert.Visibility = Visibility.Visible;
                    }
                    break;
                case "ShowPhongBanTable":
                    LabeMainField.Content = "DEPARTMENTS";
                    ShowPhongBan.Visibility = Visibility.Visible;
                    break;
                case "ShowAssignmentTable":
                    LabeMainField.Content = "Assignments";
                    ShowAssignment.Visibility = Visibility.Visible;
                    if(role_user=="Truong phong")
                    {
                        Btn_Assignment_Edit.Visibility = Visibility.Visible;
                        Btn_Assignment_Delete.Visibility = Visibility.Visible;
                        Btn_Assignment_Insert.Visibility = Visibility.Visible;
                    }
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
            errLabel.Visibility = Visibility.Collapsed;
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

        private void cleanAdminGui()
        {
            cleanDelete();
            cleanLoginGui();
            cleanAddGui();
            cleanCreateDeleteRole();
            cleanSearchRole();
            cleanGrantRoleUser();
            cleanPrivRole();
        }

        // for Employee

        private void cleanAboutUser()
        {
            UpdateAboutEmployee.Text = string.Empty;
            UpdateAboutEmployee.Visibility = Visibility.Collapsed;
        }

        private void cleanEmployeeGui()
        {
            cleanAboutUser();
        }

        private void password_changed(object sender, RoutedEventArgs e)
        {
            errLabelAdd.Visibility = Visibility.Visible;
            if (passwordBoxRegister.Password != againPasswordBoxRegister.Password)
            {
                errLabelAdd.Content = "Password don't match!!!";
                errLabelAdd.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                errLabelAdd.Content = "Password match!!";
                errLabelAdd.Foreground = new SolidColorBrush(Colors.Green);
            }
        }

        public void LoadListUserAdmin()
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
            }
            catch (Exception)
            {
                return;
            }
            finally
            {
                con.Close();
                con.Dispose();
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
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
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

        // Handle login (Admin and Employee)
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
            //check username and password
            string pattern_username = @"^[a-zA-Z0-9]+$";
            string pattern_password = @"^[a-zA-Z0-9@#$%^&+=]+$";
            if (Regex.IsMatch(username, pattern_username) == false || Regex.IsMatch(password, pattern_password) == false)
            {
                errLabel.Content = "username and password contains special characters!!";
                errLabel.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }
            //authentication
            string hostName = Environment.MachineName;
            string conn = $"Data Source={hostName}/XEPDB1;User Id={username};Password={password};";
            OracleConnection connection = new(conn);
            try
            {
                connection.Open();
                // Connect and use Proc Login_User to check type User
                OracleCommand oracleCommand = new OracleCommand("SYSTEM.LOGIN_USER", connection);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username.ToUpper();
                oracleCommand.Parameters.Add("OUTPUT1", OracleDbType.Varchar2, 40).Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add("OUTPUT2", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;
                oracleCommand.ExecuteNonQuery();
                string resultvalue = (string)oracleCommand.Parameters["OUTPUT1"].Value.ToString();
                string role = oracleCommand.Parameters["OUTPUT2"].IsNullable?"null": oracleCommand.Parameters["OUTPUT2"].Value.ToString();

                if (resultvalue == "Admin")
                {
                    // Gui for Admin
                    _Username = username;
                    _Password = password;
                    startPanel.Visibility = Visibility.Collapsed;
                    AdminPanel.Visibility = Visibility.Visible;
                    changeGuiLogged("listAM");
                    //Display list admin
                    LoadListUserAdmin();
                }
                else if (resultvalue == "Employee")
                {
                    // Gui for Employee
                    _Username = username;
                    _Password = password;
                    role_user = role;
                    MonitorEmployee_Load();
                    changeGuiEmployeeLogged("ManagementEmployee");
                    startPanel.Visibility = Visibility.Collapsed;
                    EmployeePanel.Visibility = Visibility.Visible;
                }
                else
                {
                    // not a user in this App
                    errLabel.Content = "User isn't exists!";
                    errLabel.Foreground = new SolidColorBrush(Colors.Red);
                    errLabel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                errLabel.Content = "User isn't exists!";
                errLabel.Foreground = new SolidColorBrush(Colors.Red);
                errLabel.Visibility = Visibility.Visible;
                return;
            }
            finally
            {
                connection.Close();
                //connection.Dispose();
            }
        }

        // Log out both Admin and Employee
        private void Btn_Logout(object sender, RoutedEventArgs e)
        {
            _Username = string.Empty;
            _Password = string.Empty;
            changeGuiLogged("login");
            changeGuiEmployeeLogged("login");
            SearchPriRole.Visibility = Visibility.Collapsed;
            //sidebar
            startPanel.Visibility = Visibility.Visible;
            AdminPanel.Visibility = Visibility.Collapsed;
            EmployeePanel.Visibility = Visibility.Collapsed;
        }

        // Add user Admin
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

            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand("SYSTEM.USER_REGISTER", con);

                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("fullname", OracleDbType.Varchar2).Value = fullname;
                oracleCommand.Parameters.Add("username", OracleDbType.Varchar2).Value = username;
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

        // Show Grant and revoke Gui
        private void Btn_GrantRevoke_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("GrantPriv");
            displayTableRole();
            dataGrantTable.Visibility = Visibility.Visible;
        }

        // Handle event Revoke
        private void Btn_Revoke(object sender, RoutedEventArgs e)
        {
            GrantRevokePrivTable(false);
        }

        // Handle event Grant
        private void Btn_Grant(object sender, RoutedEventArgs e)
        {
            GrantRevokePrivTable(true);
        }

        // Show Create and delete role Gui
        private void Btn_CreateDeleteRole_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("CreateDeleteRole");
        }

        // Show Grant role to user Gui
        private void Btn_GrantRoleUser_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("GrantRoleUser");
        }

        // Show Search Gui
        private void Btn_SearchRole_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("SearchRolePrivUser");
        }

        // Handle event Delete role
        private void Btn_DeleteRole(object sender, RoutedEventArgs e)
        {
            string roleName = nameRoleCreateDelete.Text;
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
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

        // Show Priv Role Gui
        private void Btn_PrivRole_Show(object sender, RoutedEventArgs e)
        {
            changeGuiLogged("PrivRole");
        }

        //Handle Event Create Role
        private void Btn_CreateRole(object sender, RoutedEventArgs e)
        {
            string roleName = nameRoleCreateDelete.Text;
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand("SYSTEM.CREATE_ROLE", con);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("ROLE_NAME", OracleDbType.Varchar2).Value = roleName;
                oracleCommand.Parameters.Add("RESULT", OracleDbType.Varchar2, 200).Direction = ParameterDirection.Output;
                oracleCommand.ExecuteNonQuery();
                string resultvalue = (string)oracleCommand.Parameters["RESULT"].Value.ToString();
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

        // Handle Search Role event
        private void Btn_SearchRole(object sender, RoutedEventArgs e)
        {
            dataPriRole.Visibility = Visibility.Collapsed;
            string name = nameRoleSearch.Text;
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
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

        // Handle Search Role of User Event
        private void Btn_SearchRoleUser(object sender, RoutedEventArgs e)
        {
            dataPrivRole.Visibility = Visibility.Collapsed;
            string name = nameRole.Text;
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
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

        // Handle Search Priv Event
        private void Btn_SearchPrivilege(object sender, RoutedEventArgs e)
        {
            dataRole.Visibility = Visibility.Collapsed;
            string name = nameRoleSearch.Text;
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
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
                    Role r = new Role(grantee, privilege, tableName, grantable, type);
                    roleList.Add(r);
                }
                if (roleList.Count() != 0)
                {
                    dataPriRole.Visibility = Visibility.Visible;
                    dataPriRole.ItemsSource = roleList;
                    errLabelSearchRole.Visibility = Visibility.Collapsed;
                }
                else
                {
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

        // Handle revoke role to user Event
        private void Btn_RevokeRoleUser(object sender, RoutedEventArgs e)
        {
            string roleName = nameRoleGrant.Text;
            string username = nameUser.Text;
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand("SYSTEM.REVOKE_ROLE_TO_USER", con);
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

        // Handle Grant Role to user event
        private void Btn_GrantRoleUser(object sender, RoutedEventArgs e)
        {
            string roleName = nameRoleGrant.Text;
            string username = nameUser.Text;
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
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

        // Handle Grant Priv
        private string GrantPrivilege(bool t, string priv, string nameObject, string nameTable, int flag)
        {
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = t ? new OracleCommand("SYSTEM.GRANT_PRIVS_TAB_USER_OR_ROLE", con) : new OracleCommand("SYSTEM.REVOKE_PRIVS_TABLE_USER_OR_ROLE", con);
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

        // Handle Revoke Priv in table
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
                    string err = GrantPrivilege(flag, "Insert", nameObject, name, 0);
                    if (err != "")
                        listError.Add(err);
                }
                bool update = table.Update;
                if (update)
                {
                    string err = GrantPrivilege(flag, "Update", nameObject, name, 0);
                    if (err != "")
                        listError.Add(err);
                }
                bool delete = table.Delete;
                if (delete)
                {
                    string err = GrantPrivilege(flag, "Delete", nameObject, name, 0);
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
                    string err = GrantPrivilege(flag, "Insert", nameObject, name, 1);
                    if (err != "")
                        listError.Add(err);
                }
                bool updateW = table.UpdateW;
                if (updateW)
                {
                    string err = GrantPrivilege(flag, "Update", nameObject, name, 1);
                    if (err != "")
                        listError.Add(err);
                }
                bool deleteW = table.DeleteW;
                if (deleteW)
                {
                    string err = GrantPrivilege(flag, "Delete", nameObject, name, 1);
                    if (err != "")
                        listError.Add(err);
                }
                dataRoleErr.ItemsSource = listError;
                dataRoleErr.Visibility = Visibility.Visible;
            }
        }

        // Show role Table
        private void displayTableRole()
        {
            dataGrantTable.ItemsSource = listPrivTableAdmin;
            dataRoleErr.Visibility = Visibility.Collapsed;
        }

        //========================================================================================================================================================\\
        
        //Event show Employee for monitor
        private void Btn_MonitorEmployee_show(object sender, RoutedEventArgs e)
        {
            changeGuiEmployeeLogged("ManagementEmployee");
        }

        //Event load Employee
        private void MonitorEmployee_Load()
        {
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
            string p_MANV = _Username.Substring(2);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"select * from system.nhanvien where MANV!={p_MANV}", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                BindingList<Employee> listEmployeeMonitor = new BindingList<Employee>();
                while (reader.Read())
                {
                    string manv = reader.GetString(0);
                    string tennv = reader.GetString(1);
                    string phai = reader.GetString(2);
                    string ngaysinh = reader.GetDateTime(3).ToString("dd/MM/yyyy");
                    string diachi = reader.GetString(4);
                    string sodt = reader.GetString(5);

                    RSA rsa = new RSA(512);
                    string luong = "NULL";
                    string phucap = "NULL";
                    if (rsa.ImportPrivateKeyFromFile("../../../keys/" + manv + ".xml") == 1)
                    {
                        if (!reader.IsDBNull(6))
                        {
                            long bufferSize = reader.GetBytes(6, 0, null, 0, 0); ;
                            byte[] buffer = new byte[bufferSize];
                            reader.GetBytes(6, 0, buffer, 0, (int)bufferSize);
                            luong = rsa.Decrypt(buffer);
                        }
                        else
                        {
                            luong = "NULL";
                        }

                        if (!reader.IsDBNull(7))
                        {
                            long bufferSize = reader.GetBytes(7, 0, null, 0, 0);
                            byte[] buffer = new byte[bufferSize];
                            reader.GetBytes(7, 0, buffer, 0, (int)bufferSize);
                            phucap = rsa.Decrypt(buffer);
                        }
                        else
                        {
                            phucap = "NULL";
                        }
                    }
                    
                    string vaitro = reader.GetString(8);
                    string manql = reader.IsDBNull(9) ? "NULL" : reader.GetString(9);
                    string phg = reader.IsDBNull(10) ? "NULL" : reader.GetString(10);
                    Employee employee = new Employee(manv, tennv, phai, ngaysinh, diachi, sodt, luong, phucap, vaitro, manql, phg);
                    listEmployeeMonitor.Add(employee);
                }
                if (listEmployeeMonitor.Count > 0)
                {
                    ErrEmployeeManagement.Content = "";
                    ErrEmployeeManagement.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ErrEmployeeManagement.Content = "You don't manager any employee!!";
                    ErrEmployeeManagement.Visibility = Visibility.Visible;
                }
                EmployeeManagement.ItemsSource = listEmployeeMonitor;
            }
            catch (Exception)
            {
                ErrEmployeeManagement.Content = "App have a problem";
                ErrEmployeeManagement.Visibility = Visibility.Visible;
                return;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        private void Btn_AboutEmployee(object sender, RoutedEventArgs e)
        {
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
            string p_MANV = _Username.Substring(2);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"select * from system.nhanvien where MANV={p_MANV}", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                BindingList<Employee> listT = new BindingList<Employee>();
                if (reader.Read())
                {
                    string manv = reader.GetString(0);
                    string tennv = reader.GetString(1);
                    string phai = reader.GetString(2);
                    string ngaysinh = reader.GetDateTime(3).ToString("dd/MM/yyyy");
                    string diachi = reader.GetString(4);
                    string sodt = reader.GetString(5);

                    RSA rsa = new RSA(512);
                    string luong = "NULL";
                    string phucap = "NULL";
                    if (rsa.ImportPrivateKeyFromFile("../../../keys/" + manv + ".xml") == 1)
                    {
                        if (!reader.IsDBNull(6))
                        {
                            long bufferSize = reader.GetBytes(6, 0, null, 0, 0); ;
                            byte[] buffer = new byte[bufferSize];
                            reader.GetBytes(6, 0, buffer, 0, (int)bufferSize);
                            luong = rsa.Decrypt(buffer);
                        }
                        else
                        {
                            luong = "NULL";
                        }

                        if (!reader.IsDBNull(7))
                        {
                            long bufferSize = reader.GetBytes(7, 0, null, 0, 0);
                            byte[] buffer = new byte[bufferSize];
                            reader.GetBytes(7, 0, buffer, 0, (int)bufferSize);
                            phucap = rsa.Decrypt(buffer);
                        }
                        else
                        {
                            phucap = "NULL";
                        }
                    }
                    string vaitro = reader.GetString(8);
                    string manql = reader.IsDBNull(9) ? "NULL" : reader.GetString(9);
                    string phg = reader.IsDBNull(10) ? "NULL" : reader.GetString(10);
                    Employee employee = new Employee(manv, tennv, phai, ngaysinh, diachi, sodt, luong, phucap, vaitro, manql, phg);
                    AboutEmployee.DataContext = employee;
                    changeGuiEmployeeLogged("AboutEmployee");
                }
            }
            catch (Exception)
            {
                ErrEmployeeManagement.Content = "App have a problem";
                ErrEmployeeManagement.Visibility = Visibility.Visible;
                return;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        private void Btn_UpdateEmployee(object sender, RoutedEventArgs e)
        {
            string ngaysinh = NGAYSINH.Text;
            string diachi = DIACHI.Text;
            string sodt = SODT.Text;

            //check Day
            DateTime date;
            bool isValid = DateTime.TryParseExact(ngaysinh, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if (isValid == false)
            {
                MessageBox.Show("Please fill NGAYSINH follow format: dd/mm/yyyy !!!");
                return;
            }

            //check Project name
            if (diachi == "")
            {
                MessageBox.Show("Please fill DIACHI!!!");
                return;
            }

            // check phone number
            if (Regex.IsMatch(sodt, @"^\d{10}$") == false)
            {
                MessageBox.Show("Phone number invalid!!!");
                return;
            }

            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand command = con.CreateCommand();

                command.CommandText = $"UPDATE system.NHANVIEN SET NGAYSINH = TO_DATE(:NGAYSINH, 'DD/MM/YYYY'), DIACHI = :DIACHI, SODT = :SODT";

                command.Parameters.Add("NGAYSINH", OracleDbType.Varchar2).Value = ngaysinh;
                command.Parameters.Add("DIACHI", OracleDbType.Varchar2).Value = diachi;
                command.Parameters.Add("SODT", OracleDbType.Varchar2).Value = sodt;

                int rowsUpdated = command.ExecuteNonQuery();

                if (rowsUpdated > 0)
                {
                    Btn_AboutEmployee(sender, e);
                    UpdateAboutEmployee.Text = "Success!!! App only change NGAYSINH, DIACHI, SODT!!!";
                    UpdateAboutEmployee.Visibility = Visibility.Visible;
                    UpdateAboutEmployee.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    UpdateAboutEmployee.Text = "Failure!!!";
                    UpdateAboutEmployee.Visibility = Visibility.Visible;
                    UpdateAboutEmployee.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            catch (Exception)
            {
                UpdateAboutEmployee.Text = "Failure!!!";
                UpdateAboutEmployee.Visibility = Visibility.Visible;
                UpdateAboutEmployee.Foreground = new SolidColorBrush(Colors.Red);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        private void Btn_DeAn_show(object sender, RoutedEventArgs e)
        {
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"select * from system.DEAN", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                BindingList<Project> projects = new BindingList<Project>();
                while (reader.Read())
                {
                    string MADA = reader.GetString(0);
                    string TENDA = reader.GetString(1);
                    string NGAYBD = reader.GetDateTime(2).ToString("dd/MM/yyyy");
                    string PHONG = reader.GetString(3);
                    Project project = new Project(MADA, TENDA, NGAYBD, PHONG);
                    projects.Add(project);
                }
                if (projects.Count > 0)
                {
                    ErrShowDeAn.Content = "";
                    ErrShowDeAn.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ErrShowDeAn.Content = "You don't see any Projects!!";
                    ErrShowDeAn.Visibility = Visibility.Visible;
                }
                ShowDeAnTable.ItemsSource = projects;
                changeGuiEmployeeLogged("ShowDeAnTable");
            }
            catch (Exception)
            {
                ErrShowDeAn.Content = "App have a problem";
                ErrShowDeAn.Visibility = Visibility.Visible;
                return;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        private void Btn_PhongBan_show(object sender, RoutedEventArgs e)
        {
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"select * from system.PhongBan", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                BindingList<Department> Departments = new BindingList<Department>();
                while (reader.Read())
                {
                    string MAPB = reader.GetString(0);
                    string TENPB = reader.GetString(1);
                    string TRPHG = reader.IsDBNull(2) ? "NULL": reader.GetString(2);
                    Department  department = new Department(MAPB,TENPB,TRPHG);
                    Departments.Add(department);
                }
                if (Departments.Count > 0)
                {
                    ErrShowPhongBan.Content = "";
                    ErrShowPhongBan.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ErrShowPhongBan.Content = "You don't see any Department!!";
                    ErrShowPhongBan.Visibility = Visibility.Visible;
                }
                ShowPhongBanTable.ItemsSource = Departments;
                changeGuiEmployeeLogged("ShowPhongBanTable");
            }
            catch (Exception)
            {
                ErrShowDeAn.Content = "App have a problem";
                ErrShowDeAn.Visibility = Visibility.Visible;
                return;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        private void Btn_Assignment_show(object sender, RoutedEventArgs e)
        {
            CreateConnection();
            //string conn = $"Data Source={hostName}/XEPDB1;User Id={_Username};Password={_Password};";
            //OracleConnection con = new OracleConnection(conn);
            try
            {
                con.Open();
                OracleCommand oracleCommand = new OracleCommand($"select * from system.PhanCong", con);
                OracleDataReader reader = oracleCommand.ExecuteReader();
                BindingList<Assignment> assignments = new BindingList<Assignment>();
                while (reader.Read())
                {
                    string MANV = reader.GetString(0);
                    string MADA = reader.GetString(1);
                    string THOIGIAN = reader.GetDateTime(2).ToString("dd/MM/yyyy");
                    Assignment assignment = new Assignment(MANV, MADA, THOIGIAN);
                    assignments.Add(assignment);
                }
                if (assignments.Count > 0)
                {
                    ErrShowPhongBan.Content = "";
                    ErrShowPhongBan.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ErrShowPhongBan.Content = "You don't see any Department!!";
                    ErrShowPhongBan.Visibility = Visibility.Visible;
                }
                ShowAssignmentTable.ItemsSource = assignments;
                changeGuiEmployeeLogged("ShowAssignmentTable");
            }
            catch (Exception)
            {
                ErrShowDeAn.Content = "App have a problem";
                ErrShowDeAn.Visibility = Visibility.Visible;
                return;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        private void Btn_DeleteAssignment(object sender, RoutedEventArgs e)
        {
            Assignment temp = (Assignment) ShowAssignmentTable.SelectedItem;
            if(temp != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this assignment?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    CreateConnection();
                    try
                    {
                        con.Open();
                        OracleCommand oracleCommand = new OracleCommand($"Delete from system.phancong where manv=:MANV and mada=:MADA", con);
                        oracleCommand.Parameters.Add("MANV", OracleDbType.Varchar2).Value = temp.MANV;
                        oracleCommand.Parameters.Add("MADA", OracleDbType.Varchar2).Value = temp.MADA;

                        int rowsDelete = oracleCommand.ExecuteNonQuery();
                        if (rowsDelete > 0)
                            Btn_Assignment_show(sender, e);
                        changeGuiEmployeeLogged("ShowAssignmentTable");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("App have a problem");
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

        private void Btn_EditAssignment(object sender, RoutedEventArgs e)
        {
            Assignment selected = (Assignment)ShowAssignmentTable.SelectedItem;
            if (selected != null)
            {
                Assignment temp = new Assignment(selected.MANV, selected.MADA, selected.THOIGIAN);
                var optionPanel = new EditAssignment(temp, _Username,_Password);
                optionPanel.Show();
                optionPanel.OneMessage += (string mess, Assignment asign) =>
                {
                    if (mess == "Success!!!")
                        Btn_Assignment_show(sender, e);
                };
            }
        }

        private void InsertAssignment_Click(object sender, RoutedEventArgs e)
        {
            var addAssignment = new AddAssignment(_Username, _Password);
            addAssignment.Show();
            addAssignment.OneMessage += (string mess) =>
            {
                if (mess == "Success!!!")
                    Btn_Assignment_show(sender, e);
            };
        }

        private void Btn_EditProject(object sender, RoutedEventArgs e)
        {
            Project selected = (Project)ShowDeAnTable.SelectedItem;
            if (selected != null)
            {
                Project temp = new Project(selected.MADA,selected.TENDA,selected.NGAYBD,selected.PHONG);
                var optionPanel = new EditProject(temp, _Username, _Password);
                optionPanel.Show();
                optionPanel.OneMessage += (string mess, Project pro) =>
                {
                    if (mess == "Success!!!")
                        Btn_DeAn_show(sender, e);
                };
            }
        }

        private void Btn_DeleteProject(object sender, RoutedEventArgs e)
        {
            Project project =(Project) ShowDeAnTable.SelectedItem;
            if (project != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this Project?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    CreateConnection();
                    try
                    {
                        con.Open();
                        OracleCommand oracleCommand = new OracleCommand($"Delete from system.dean where mada=:MADA", con);
                        oracleCommand.Parameters.Add("MADA", OracleDbType.Varchar2).Value = project.MADA;

                        int rowsDelete = oracleCommand.ExecuteNonQuery();
                        if (rowsDelete > 0)
                            Btn_DeAn_show(sender, e);

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("App have a problem");
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

        private void InsertProject_Click(object sender, RoutedEventArgs e)
        {
            //0038
            var addProject = new AddProject(_Username, _Password);
            addProject.Show();
            addProject.OneMessage += (string mess) =>
            {
                if (mess == "Success!!!")
                    Btn_DeAn_show(sender, e);
            };
        }
        private void Btn_EditEmployee(object sender, RoutedEventArgs e)
        {
            Employee employee = (Employee)EmployeeManagement.SelectedItem;
            if (employee != null)
            {
                Employee temp = new Employee(employee.MANV, employee.TENNV,employee.PHAI,employee.NGAYSINH,employee.DIACHI,employee.SODT,employee.LUONG,employee.PHUCAP,employee.VAITRO,employee.MANQL,employee.PHG);
                var optionPanel = new EditEmployee(role_user, _Username, _Password, temp);
                optionPanel.Show();
                optionPanel.OneMessage += (string mess) =>
                {
                    if (mess == "Success!!!")
                        MonitorEmployee_Load();
                };
            }
        }

        private void InsertEmployee_Click(object sender, RoutedEventArgs e)
        {
            var addEmployee = new AddEmployee(_Username, _Password);
            addEmployee.Show();
            addEmployee.OneMessage += (string mess) =>
            {
                if (mess == "Success!!!")
                    MonitorEmployee_Load();
            };
        }
    }
}
