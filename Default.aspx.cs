using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.Data.OleDb;
using System.Drawing;
using System.Web.Security;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Management.Automation; //Powershell Reference
using System.Management.Automation.Runspaces;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using IBM.Data.DB2;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page.User.Identity.IsAuthenticated)
        {
            Session["LoginID"] = User.Identity.Name;
            GetRole();
            if (User.IsInRole("admin"))
            {
                AdminPg.Visible = true;
                DBPg.Visible = true;
            }
            else
            {
                AdminPg.Visible = false;
                DBPg.Visible = false;
            }
        }
        else
        {
            FormsAuthentication.RedirectToLoginPage("~/login.aspx");
        }
    }

    protected void GetRole()
    {
        string query = "SELECT [Role] FROM [Users] WHERE [UserID] = '" + Session["LoginID"] + "'";
        //CAST(" + Session["LoginID"] + " AS NVARCHAR(20))";
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagementConnectionString"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.HasRows)
                    {
                        while (r.Read())
                        {
                            Session["Role"] = r["role"];
                        }
                    }
                }
            }
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList2.Style.Add("display", "block");
        imagediv.Style.Add("display", "none");
        TextBox1.Text = "";
        msg.Text = "";
        GridView1.DataSource = null;
        GridView1.DataBind();
        TextBox1.Attributes.Remove("readonly");
        Button1.Enabled = true;

        switch (DropDownList1.SelectedItem.Text)
        {
            case "SQL Server":
                commit.Visible = true;
                rollback.Visible = true;
                Button3.Visible = true;
                commit.Enabled = false;
                rollback.Enabled = false;
                break;
            case "Oracle":
            case "DB2":
                Button3.Visible = false;
                commit.Visible = false;
                rollback.Visible = false;
                break;
            default:
                Button3.Visible = true;
                commit.Visible = false;
                rollback.Visible = false;
                break;
        }

    }

    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {

        Image1.ImageUrl = "images/" + DropDownList1.SelectedItem.Text + DropDownList2.SelectedItem.Text + ".jpg";
        TextBox1.Text = "";
        msg.Text = "";
        GridView1.DataSource = null;
        GridView1.DataBind();
        TextBox1.Attributes.Remove("readonly");
        Button1.Enabled = true;
        commit.Enabled = false;
        rollback.Enabled = false;
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        if (DropDownList2.SelectedItem != null)
        {
            Image1.ImageUrl = "images/" + DropDownList1.SelectedItem.Text + DropDownList2.SelectedItem.Text + ".jpg";
            //Image1.Style.Add("display", "block");
            imagediv.Style.Add("display", "block");
        }
        else
        {
            Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Error\\nPlease choose a database');</script>"));
        }
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        Button1.Enabled = true;
        commit.Enabled = false;
        rollback.Enabled = false;
        GridView1.DataSource = null;
        GridView1.DataBind();
        msg.Text = "";
        //SqlConnection.ClearAllPools(); 
        switch (DropDownList1.SelectedItem.Text)
        {
            case "SQL Server":
                string s = "Data Source=localhost;Initial Catalog=master" + "; User ID=sa; Password=***;";
                SqlConnection ProcConn = new SqlConnection(s);

                try
                {
                    ProcConn.Open();
                    SqlCommand cmd = new SqlCommand("spCreateUserDb", ProcConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                            new SqlParameter("@sourceDb", DropDownList2.SelectedItem.Text));
                    cmd.Parameters.Add(
                            new SqlParameter("@destDb", User.Identity.Name + DropDownList2.SelectedItem.Text));
                    cmd.ExecuteNonQuery();
                    //msg.Text += "Database Created: " + User.Identity.Name + DropDownList2.SelectedItem.Text;
                    Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Database " + DropDownList2.Text + " has been created');</script>"));
                    output.Text = "Database " + DropDownList2.SelectedItem.Text + " has been created";

                }
                catch (Exception)
                {
                    Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Database "+ DropDownList2.Text + " creation failed');</script>")); 
                }
                finally
                {
                    if (ProcConn != null)
                    {
                        ProcConn.Close();
                    }

                }
                break;
            case "Access":

                RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
                try
                {
                    using (Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration))
                    {
                        runspace.Open();
                        RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);
                        Pipeline pipeline = runspace.CreatePipeline();
                        string scriptfile = "C:\\SourceDatabase\\Access\\powershellCreateDB.ps1";
                        //add a new script with arguments
                        Command myCommand = new Command(scriptfile);
                        CommandParameter SourceParam = new CommandParameter("source", DropDownList2.SelectedItem.Text);
                        CommandParameter TargetParam = new CommandParameter("target", User.Identity.Name);
                        myCommand.Parameters.Add(SourceParam);
                        myCommand.Parameters.Add(TargetParam);
                        pipeline.Commands.Add(myCommand);
                        // Execute PowerShell script; powershell should set-executionpolicy remotesigned.
                        // Otherwise Error: "File C:\SourceDatabase\Access\powershellCreateDB.ps1 cannot be loaded because running scripts is disabled on this system. 
                        // For more information, see about_Execution_Policies at http://go.microsoft.com/fwlink/?LinkID=135170."
                        // error "AuthorizationManager check failed" caused by the version of 32bit or 64 bit, solution : resave it.
                        pipeline.Invoke();
                        
                    }
                    Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Database " + DropDownList2.Text + " has been created');</script>"));
                    output.Text = "Database " + DropDownList2.SelectedItem.Text + " has been created";
                }
                catch (Exception ex)
                {
                    Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Database " + DropDownList2.Text + " creation failed');</script>"));
                    msg.Text = ex.Message;
                }
                break;
            case "MySQL":
                string str = "server=localhost;Database=" + DropDownList2.SelectedItem.Text + ";Uid=root;Pwd=***;";
                string file = "C:\\SourceDatabase\\MySQL\\" + DropDownList2.SelectedItem.Text + ".sql";
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(str))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "DROP DATABASE IF EXISTS " + User.Identity.Name + DropDownList2.SelectedItem.Text;
                            cmd.Connection = conn;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                //cmd.Connection = conn;
                                //conn.Open();
                                mb.ImportInfo.TargetDatabase = User.Identity.Name + DropDownList2.SelectedItem.Text;
                                mb.ImportFromFile(file);
                            }
                        }
                    }
                    Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Database " + DropDownList2.Text + " has been created');</script>"));
                    output.Text = "Database " + DropDownList2.SelectedItem.Text + " has been created";
                }
                catch (Exception ex)
                {
                    Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Database " + DropDownList2.Text + " creation failed');</script>"));
                    output.Text = ex.Message;
                }
                break;
            
            case "PostgreSQL":
                string ps = "Server=127.0.0.1;Port=5432;Database=template1;User Id=postgres; Password=***;Pooling=false;";
                //string ps = "Server=127.0.0.1;Port=5432;Database=" + DropDownList2.SelectedItem.Text + ";User Id=postgres; Password=Password2014;Pooling=false;";
                //string drop = "DROP DATABASE IF EXISTS" + User.Identity + DropDownList2.SelectedItem.Text;
                try
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection(ps))
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand())
                        {
                            cmd.CommandText = "DROP DATABASE IF EXISTS " + User.Identity.Name + DropDownList2.SelectedItem.Text;
                            cmd.Connection = conn;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "CREATE DATABASE " + User.Identity.Name + DropDownList2.SelectedItem.Text + " TEMPLATE " + DropDownList2.SelectedItem.Text;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Database " + DropDownList2.Text + " has been created');</script>"));
                    output.Text = "Database " + DropDownList2.SelectedItem.Text + " has been created";
                }
                catch (Exception ex)
                {
                    Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Database " + DropDownList2.Text + " creation failed');</script>"));
                    output.Text = ex.Message;
                }
                break;
            case "SQLite":
                RunspaceConfiguration SQLiterunspaceConfiguration = RunspaceConfiguration.Create();
                try
                {
                    using (Runspace runspace = RunspaceFactory.CreateRunspace(SQLiterunspaceConfiguration))
                    {
                        runspace.Open();
                        RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);
                        Pipeline pipeline = runspace.CreatePipeline();
                        string scriptfile = "C:\\SourceDatabase\\SQLite\\powershellCreateDB.ps1";
                        //add a new script with arguments
                        Command myCommand = new Command(scriptfile);
                        CommandParameter SourceParam = new CommandParameter("source", DropDownList2.SelectedItem.Text);
                        CommandParameter TargetParam = new CommandParameter("target", User.Identity.Name);
                        myCommand.Parameters.Add(SourceParam);
                        myCommand.Parameters.Add(TargetParam);
                        pipeline.Commands.Add(myCommand);
                        // Execute PowerShell script; powershell should set-executionpolicy remotesigned.
                        // Otherwise Error: "File C:\SourceDatabase\Access\powershellCreateDB.ps1 cannot be loaded because running scripts is disabled on this system. 
                        // For more information, see about_Execution_Policies at http://go.microsoft.com/fwlink/?LinkID=135170."
                        // error "AuthorizationManager check failed" caused by the version of 32bit or 64 bit, solution : resave it.
                        pipeline.Invoke();
                        
                    }
                    Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Database " + DropDownList2.Text + " has been created');</script>"));
                    output.Text = "Database " + DropDownList2.SelectedItem.Text + " has been created";
                }
                catch (Exception ex)
                {
                    Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Database " + DropDownList2.Text + " creation failed');</script>"));
                    msg.Text = ex.Message;
                }
                break;
            case "Oracle":
            case "DB2":
                Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Error\\nDataBase already exists, no need to create.');</script>"));
                break;
            default:
                //msg.Text = "no such database";
                Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Error\\nPlease choose a database');</script>"));
                break;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        GridView1.DataSource = null;
        GridView1.DataBind();
        msg.Text = "";
        commit.Enabled = false;
        rollback.Enabled = false;
        switch (DropDownList1.SelectedItem.Text)
        {
            case "SQL Server":
                string inputPattern = @"^[\s\;]*(?i)\bdelete\b|\binsert\b|\bupdate\b|\bdrop\b";
                Regex input = new Regex(inputPattern);
                Match inputmatch = input.Match(TextBox1.Text);
                if (inputmatch.Success)
                {
                    commit.Enabled = true;
                    rollback.Enabled = true;
                    Button1.Enabled = false;
                    TextBox1.Attributes.Add("readonly", "true");
                    SqlConnection(" rollback");
                    //if (GridView1.Rows.Count > 0)
                    //{
                        //commit.Enabled = true;
                        //rollback.Enabled = true;
                        //Button1.Enabled = false;
                        //TextBox1.Attributes.Add("readonly", "true");
                    //}
                }
                else
                {
                    SqlConnection(String.Empty);
                }
                break;
            case "MySQL":
                MySqlConnection();
                break;
            case "SQLite":
                SqliteConnection();
                break;
            case "Access":
                AccessConnection();
                break;
            case "PostgreSQL":
                PostgreConnection();
                break;
            case "Oracle":
                OracleConnection();
                break;
            case "DB2":
                DB2Connection();
                break;
            default:
                //msg.Text = "no such database";
                Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Error!\\nPlease Choose a Database!');</script>"));
                break;
        }
        //ProgressBarModalPopupExtender.Hide();
    }

    protected void SqlConnection(string end)
    {
        msg.Text = "";
        string Text1 = TextBox1.Text.ToString().ToLower();
        int index = Text1.Length;
        List<string> tablenames = new List<string>();
        string tablename = "";
        string inputPattern = @"^[\s\;]*\w+[^\;]*[\;\s]*$";
        Regex input = new Regex(inputPattern);
        Match inputmatch = input.Match(TextBox1.Text);
        if (inputmatch.Success)
        {
            string s = "Data Source=localhost;Initial Catalog=" + User.Identity.Name + DropDownList2.SelectedItem.Text + "; User ID=sa; Password=***;Pooling=false;";
            SqlConnection myConnection = new SqlConnection(s);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader r = null;
            try
            {
                myConnection.Open();
                if (myConnection.State == ConnectionState.Open)
                {
                    try
                    {
                        cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                        cmd.Connection = myConnection;
                        //myConnection.Open();
                        //int numberOfRecords = cmd.ExecuteNonQuery();
                        r = cmd.ExecuteReader();
                        while (r.Read())
                        {
                            tablenames.Add(r.GetString(0).ToLower());
                        }
                        r.Close();
                        //check the first word of input
                        string firstWord = Regex.Match(TextBox1.Text, @"\w+\b").ToString().ToLower();
                        var pattern = new Regex(@"\W");
                        bool isFound = pattern.Split(Text1).Any(w => tablenames.Contains(w));
                        if (isFound)
                        {
                            tablename = pattern.Split(Text1).First(w => tablenames.Contains(w));
                            index = Text1.IndexOf(tablename) + tablename.Length;                            
                            switch (firstWord)
                            {
                                case "delete":
                                    cmd.CommandText = "begin tran " + Text1.Insert(index, " output deleted.* ") + end;
                                    break;
                                case "insert":
                                    Regex containCol = new Regex(tablename + @"\s*\(");
                                    if (containCol.IsMatch(Text1))
                                    {
                                        index = Text1.IndexOf(")") + 1;
                                    }
                                    cmd.CommandText = "begin tran " + Text1.Insert(index, " output inserted.* ") + end;
                                    break;
                                case "update":
                                    int a = Text1.IndexOf("from");
                                    int b = Text1.IndexOf("where");
                                    Text1 = Text1.Replace(";", ""); //for insert output clause at the end of the query except ";"
                                    if (a >= 0 && b >= 0)
                                        index = Math.Min(a, b);
                                    else
                                        if (a < 0 && b < 0)
                                            index = Text1.Length; 
                                        else
                                            index = Math.Max(a, b);
                                    cmd.CommandText = "begin tran " + Text1.Insert(index, " output inserted.*, deleted.* ") + end;
                                    break;
                                default:
                                    cmd.CommandText = "begin tran " + Text1 + end; //in case input 'select * from x delete y'
                                    break;
                            }
                            //output.Text = "index: " + index + " TableName:" + tablename + cmd.CommandText + myConnection.State;
                            //output.Text = cmd.CommandText;
                        }
                        else
                        {cmd.CommandText = Text1;}
                        //string outstring = " output deleted.* ";
                        //cmd.CommandText = "begin tran " + Text1.Insert(index, output) + end;
                        cmd.Connection = myConnection;
                        DataTable dt = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                        GridView1.DataSource = dt;
                        if (firstWord == "select")
                        {
                            

                            r = cmd.ExecuteReader();
                            int rowCount = 0;
                            if (r.HasRows)
                            {
                                while (r.Read())
                                {
                                    rowCount++;
                                }
                                msg.Text = "Result: " + rowCount + " row(s) found";
                            }
                            else
                            {
                                msg.Text = "Result: 0 row(s) found.<br /><span style='background-color:#339933; color:black; font-size: 15pt'>";
                                for (int i = 0; i < r.FieldCount; i++)
                                {
                                    msg.Text += r.GetName(i) + " |";
                                }
                                msg.Text += "</span><br />";
                            }
                            r.Close();
                        }
                        else
                        {
                            int numberOfRecords = cmd.ExecuteNonQuery();
                            if (numberOfRecords > 0)
                            {
                                msg.Text = "Result: " + numberOfRecords + " row(s) affected.";
                                //DataTable dt = new DataTable();
                                //using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                                //{
                                //    adapter.Fill(dt);
                                //}
                                //GridView1.DataSource = dt;
                            }

                            else
                            {

                                msg.Text = "Excuted sucessfully.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToLowerInvariant().Contains("output"))
                        {
                            // do whatever to generate the real error msg
                            msg.Text = ex.Message.Replace("output", tablename);
                        }
                        else
                        {
                            msg.Text = ex.Message.Replace(User.Identity.Name, "");
                        }
                        Button1.Enabled = true;
                        TextBox1.Attributes.Remove("readonly");
                        commit.Enabled = false;
                        rollback.Enabled = false;
                    }
                    finally
                    {
                        if (myConnection != null)
                        {
                            myConnection.Close();
                        }
                        GridView1.DataBind();
                        FormatView();
                    }
                }
            }
            catch (SqlException) { Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Connection failed\\nCreate database "+ DropDownList2.Text + " firstly');</script>")); }
        }
        else
        {
            //msg.Text = "Only one statement allowed";
            Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Only one statement allowed');</script>"));
            Button1.Enabled = true;
            TextBox1.Attributes.Remove("readonly");
            commit.Enabled = false;
            rollback.Enabled = false;            
        }
    }

    protected void MySqlConnection()
    {
        string Text1 = TextBox1.Text;
        string inputPattern = @"^[\s\;]*\w+[^\;]*[\;\s]*$";
        Regex input = new Regex(inputPattern);
        Match inputmatch = input.Match(TextBox1.Text);
        if (inputmatch.Success)
        {
            string s = "Server=localhost;Database=" + User.Identity.Name + DropDownList2.SelectedItem.Text + ";Uid=root;Pwd=***;";
            MySqlConnection myConnection = new MySqlConnection(s);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader r = null;
            MySqlDataAdapter adapter = new MySqlDataAdapter(Text1, myConnection);

            try
            {
                myConnection.Open();
                if (myConnection.State == ConnectionState.Open)
                {
                    try
                    {
                        cmd.CommandText = TextBox1.Text;
                        cmd.Connection = myConnection;
                        //myConnection.Open();
                        string firstWord = Regex.Match(TextBox1.Text, @"\w+\b").ToString().ToLower();
                        if (firstWord == "select")
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            GridView1.DataSource = dt;

                            r = cmd.ExecuteReader();
                            int rowCount = 0;
                            if (r.HasRows) //results>0
                            {
                                while (r.Read())
                                {
                                    rowCount++;
                                }
                                msg.Text = "Result: " + rowCount + " row(s) found";
                            }
                            else
                            {
                                msg.Text = "Result: 0 row(s) found.<br /><span style='background-color:#339933; color:black; font-size: 15pt'>";
                                for (int i = 0; i < r.FieldCount; i++)
                                {
                                    msg.Text += r.GetName(i) + " |";
                                }
                                msg.Text += "</span><br />";
                            }
                            r.Close();
                        }
                        else
                        {
                            int numberOfRecords = cmd.ExecuteNonQuery();
                            msg.Text = "Result: " + numberOfRecords + " row(s) affected.";
                        }
                    }
                    catch (Exception ex)
                    {
                        msg.Text = ex.Message.Replace(User.Identity.Name, "");
                    }
                    finally
                    {
                        myConnection.Close();
                        GridView1.DataBind();
                        FormatView();
                    }
                }
            }
            catch (Exception) { Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Connection failed\\nCreate database " + DropDownList2.Text + " firstly');</script>")); }
        }
        else
        {
            //msg.Text = "Only one statement allowed";
            Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Only one statement allowed');</script>"));
        }
    }

    protected void SqliteConnection()
    {
        string Text1 = TextBox1.Text;
        string inputPattern = @"^[\s\;]*\w+[^\;]*[\;\s]*$";
        Regex input = new Regex(inputPattern);
        Match inputmatch = input.Match(TextBox1.Text);
        if (inputmatch.Success)
        {
            string s = @"Data Source=C:\TargetDatabase\SQLite\" + User.Identity.Name + DropDownList2.SelectedItem.Text + ".db;Version=3;";
            SQLiteConnection myConnection = new SQLiteConnection(s);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteDataReader r = null;
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(Text1, myConnection);
            try
            {
                cmd.CommandText = TextBox1.Text;
                cmd.Connection = myConnection;
                myConnection.Open();
                string firstWord = Regex.Match(TextBox1.Text, @"\w+\b").ToString().ToLower();
                if (firstWord == "select")
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    GridView1.DataSource = dt;

                    r = cmd.ExecuteReader();
                    int rowCount = 0;
                    if (r.HasRows) //results>0
                    {
                        while (r.Read())
                        {
                            rowCount++;
                        }
                        msg.Text = "Result: " + rowCount + " row(s) found";
                    }
                    else
                    {
                        msg.Text = "Result: 0 row(s) found.<br /><span style='background-color:#339933; color:black; font-size: 15pt'>";
                        for (int i = 0; i < r.FieldCount; i++)
                        {
                            msg.Text += r.GetName(i) + " |";
                        }
                        msg.Text += "</span><br />";
                    }
                    r.Close();
                }
                else
                {
                    int numberOfRecords = cmd.ExecuteNonQuery();
                    msg.Text = "Result: " + numberOfRecords + " row(s) affected.";
                }
            }
            catch (Exception ex)
            {
                msg.Text = ex.Message;
            }
            finally
            {
                //myConnection.Close();
                GridView1.DataBind();
                FormatView();
            }
        }
        else
        {
            //msg.Text = "Only one statement allowed";
            Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Only one statement allowed');</script>"));
        }
    }

    protected void AccessConnection()
    {
        string Text1 = TextBox1.Text;
        string s = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\TargetDatabase\Access\" + User.Identity.Name + DropDownList2.SelectedItem.Text + ".accdb;Persist Security Info=False;";
        OleDbConnection myConnection = new OleDbConnection(s);
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataReader r = null;
        OleDbDataAdapter adapter = new OleDbDataAdapter(Text1, myConnection);
        try
        {
            myConnection.Open();
            if (myConnection.State == ConnectionState.Open)
            {
                try
                {
                    cmd.CommandText = TextBox1.Text;
                    cmd.Connection = myConnection;
                    //myConnection.Open();
                    string firstWord = Regex.Match(TextBox1.Text, @"\w+\b").ToString().ToLower();
                    if (firstWord == "select")
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        GridView1.DataSource = dt;

                        r = cmd.ExecuteReader();
                        int rowCount = 0;
                        if (r.HasRows) //results>0
                        {
                            while (r.Read())
                            {
                                rowCount++;
                            }
                            msg.Text = "Result: " + rowCount + " row(s) found";
                        }
                        else
                        {
                            msg.Text = "Result: 0 row(s) found.<br /><span style='background-color:#339933; color:black; font-size: 15pt'>";
                            for (int i = 0; i < r.FieldCount; i++)
                            {
                                msg.Text += r.GetName(i) + " |";
                            }
                            msg.Text += "</span><br />";
                        }
                        r.Close();
                    }
                    else
                    {
                        int numberOfRecords = cmd.ExecuteNonQuery();
                        msg.Text = "Result: " + numberOfRecords + " row(s) affected.";
                    }
                }
                catch (Exception ex)
                {
                    msg.Text = ex.Message.Replace(User.Identity.Name,"");
                }
                finally
                {
                    myConnection.Close();
                    GridView1.DataBind();
                    FormatView();
                }
            }

        }
        catch (Exception e) 
        {
            Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Connection failed\\nCreate database " + DropDownList2.Text + " firstly');</script>"));
            output.Text = e.Message;
        }
    }

    protected void PostgreConnection()
    {
        string Text1 = TextBox1.Text;
        string inputPattern = @"^[\s\;]*\w+[^\;]*[\;\s]*$";
        Regex input = new Regex(inputPattern);
        Match inputmatch = input.Match(TextBox1.Text);
        if (inputmatch.Success)
        {
            string s = @"Server=127.0.0.1;Port=5432;Database=" + User.Identity.Name + DropDownList2.SelectedItem.Text + ";User Id=postgres; Password=***;Pooling=false;";
            NpgsqlConnection myConnection = new NpgsqlConnection(s);
            NpgsqlCommand cmd = new NpgsqlCommand();
            NpgsqlDataReader r = null;
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(Text1, myConnection);
            try
            {
                cmd.CommandText = TextBox1.Text;
                cmd.Connection = myConnection;
                myConnection.Open();
                string firstWord = Regex.Match(TextBox1.Text, @"\w+\b").ToString().ToLower();
                if (firstWord == "select")
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    GridView1.DataSource = dt;

                    r = cmd.ExecuteReader();
                    int rowCount = 0;
                    if (r.HasRows) //results>0
                    {
                        while (r.Read())
                        {
                            rowCount++;
                        }
                        msg.Text = "Result: " + rowCount + " row(s) found";
                    }
                    else
                    {
                        msg.Text = "Result: 0 row(s) found.<br /><span style='background-color:#339933; color:black; font-size: 15pt'>";
                        for (int i = 0; i < r.FieldCount; i++)
                        {
                            msg.Text += r.GetName(i) + " |";
                        }
                        msg.Text += "</span><br />";
                    }
                    r.Close();
                }
                else
                {
                    int numberOfRecords = cmd.ExecuteNonQuery();
                    msg.Text = "Result: " + numberOfRecords + " row(s) affected.";
                }
            }
            catch (Exception ex)
            {
                msg.Text = ex.Message;
            }
            finally
            {
                myConnection.Close();
                GridView1.DataBind();
                FormatView();
            }
        }
        else
        {
            //msg.Text = "Only one statement allowed";
            Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Only one statement allowed');</script>"));
        }
    }

    protected void OracleConnection()
    {
        string Text1 = TextBox1.Text;
        string inputPattern = @"^[\s\;]*\w+[^\;]*[\;\s]*$";
        Regex input = new Regex(inputPattern);
        Match inputmatch = input.Match(TextBox1.Text);
        if (inputmatch.Success)
        {
            string s = @"Data Source=(DESCRIPTION=
         (ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost )(PORT=1521)))
         (CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + DropDownList2.SelectedItem.Text + "))); User ID=web; Password=***;";
            OracleConnection myConnection = new OracleConnection(s);
            OracleCommand cmd = new OracleCommand();
            OracleDataReader r = null;
            OracleDataAdapter adapter = new OracleDataAdapter(Text1, myConnection);

            //string usertype = User.Identity.Name.Substring(4);
            string usertype = Session["Role"].ToString().ToLower();
            string pattern = "";
            switch (usertype)
            {
                case "level1":
                    pattern = @"((^|\;)\s*(?i)(\binsert\b|\bupdate\b|\bdelete\b|\bmerge\b|\bcreate\b|\balter\b|\bdrop\b)(?i))|(?i)\binto\b(?i)";
                    break;
                case "level2":
                    pattern = @"(^|;|\*\/)\s*(?i)(\bcreate\b|\balter\b|\bdrop\b)(?i)";
                    break;
                default:
                    pattern = @"((^|;|\*\/)\s*(?i)(\bdrop\b\s*database)(?i))";
                    break;
            }
            Regex rgx = new Regex(pattern);
            Match match = rgx.Match(TextBox1.Text);
            if (match.Success)
            {
                msg.Text = "Permission was denied on database " + DropDownList2.SelectedItem.Text;
            }
            else
            {
                try
                {
                    cmd.CommandText = TextBox1.Text;
                    cmd.Connection = myConnection;
                    myConnection.Open();
                    string firstWord = Regex.Match(TextBox1.Text, @"\w+\b").ToString().ToLower();
                    if (firstWord == "select")
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        GridView1.DataSource = dt;
                        r = cmd.ExecuteReader();
                        int rowCount = 0;
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                rowCount++;
                            }
                            msg.Text = "Result: " + rowCount + " row(s) found";
                        }
                        else
                        {
                            msg.Text = "Result: 0 row(s) found.<br /><span style='background-color:#339933; color:black; font-size: 15pt'>";
                            for (int i = 0; i < r.FieldCount; i++)
                            {
                                msg.Text += r.GetName(i) + " |";
                            }
                            msg.Text += "</span><br />";
                        }
                        r.Close();
                    }
                    else
                    {
                        int numberOfRecords = cmd.ExecuteNonQuery();
                        msg.Text = "Result: " + numberOfRecords + " row(s) affected.";
                    }
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    msg.Text = ex.Message;
                }
                finally
                {
                    myConnection.Close();
                    GridView1.DataBind();
                    FormatView();
                }
            }
        }
        else
        {
            //msg.Text = "Only one statement allowed";
            Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Only one statement allowed');</script>"));
        }
    }

    protected void DB2Connection()
    {
        string Text1 = TextBox1.Text;
        string inputPattern = @"^[\s\;]*\w+[^\;]*[\;\s]*$";
        Regex input = new Regex(inputPattern);
        Match inputmatch = input.Match(TextBox1.Text);
        if (inputmatch.Success)
        {
            string s = @"Server=localhost:50000;Database=" + DropDownList2.SelectedItem.Text + ";UID=db2admin;PWD=***;CurrentSchema=db2admin";
            DB2Connection myConnection = new DB2Connection(s);
            DB2Command cmd = new DB2Command();
            DB2DataReader r = null;
            DB2DataAdapter adapter = new DB2DataAdapter(Text1, myConnection);

            //string usertype = User.Identity.Name.Substring(4);
            string usertype = Session["Role"].ToString().ToLower();
            string pattern = "";
            switch (usertype)
            {
                case "level1":
                    pattern = @"((^|\;)\s*(?i)(\binsert\b|\bupdate\b|\bdelete\b|\bmerge\b|\bcreate\b|\balter\b|\bdrop\b)(?i))|(?i)\binto\b(?i)";
                    break;
                case "level2":
                    pattern = @"(^|;|\*\/)\s*(?i)(\bcreate\b|\balter\b|\bdrop\b)(?i)";
                    break;
                default:
                    pattern = @"((^|;|\*\/)\s*(?i)(\bdrop\b\s*database)(?i))";
                    break;
            }
            Regex rgx = new Regex(pattern);
            Match match = rgx.Match(TextBox1.Text);
            if (match.Success)
            {
                msg.Text = "Permission was denied on database " + DropDownList2.SelectedItem.Text;
            }
            else
            {
                try
                {
                    cmd.CommandText = TextBox1.Text;
                    cmd.Connection = myConnection;
                    myConnection.Open();
                    string firstWord = Regex.Match(TextBox1.Text, @"\w+\b").ToString().ToLower();
                    if (firstWord == "select")
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        GridView1.DataSource = dt;

                        r = cmd.ExecuteReader();
                        int rowCount = 0;
                        if (r.HasRows) //results>0
                        {
                            while (r.Read())
                            {
                                rowCount++;
                            }
                            msg.Text = "Result: " + rowCount + " row(s) found";
                        }
                        else
                        {
                            msg.Text = "Result: 0 row(s) found.<br /><span style='background-color:#339933; color:black; font-size: 15pt'>";
                            for (int i = 0; i < r.FieldCount; i++)
                            {
                                msg.Text += r.GetName(i) + " |";
                            }
                            msg.Text += "</span><br />";
                        }
                        r.Close();
                    }
                    else
                    {
                        int numberOfRecords = cmd.ExecuteNonQuery();
                        msg.Text = "Result: " + numberOfRecords + " row(s) affected.";
                    }
                }
                catch (Exception ex)
                {
                    msg.Text = ex.Message;
                }
                finally
                {
                    myConnection.Close();
                    GridView1.DataBind();
                    FormatView();
                }
            }
        }
        else
        {
            //msg.Text = "Only one statement allowed";
            Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\">alert('Only one statement allowed');</script>"));
        }
    }

    protected void FormatView()
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (row.RowIndex % 2 == 0)
            {
                row.BackColor = Color.LightGray;
            }
            foreach (TableCell cell in row.Cells)
            {
                if (cell.Text == "&nbsp;")
                {
                    cell.Text = "null value";
                }
            }
        }
    }

    protected void commit_Click(object sender, EventArgs e)
    {
        SqlConnection(" commit");
        msg.Text = " Result has been saved!";
        commit.Enabled = false;
        rollback.Enabled = false;
        Button1.Enabled = true;
        //TextBox1.Attributes.Add("readonly", "false");
        TextBox1.Attributes.Remove("readonly");
    }

    protected void rollback_Click(object sender, EventArgs e)
    {
        GridView1.DataSource = null;
        GridView1.DataBind();
        msg.Text = " Action Cancelled!";
        commit.Enabled = false;
        rollback.Enabled = false;
        Button1.Enabled = true;
        TextBox1.Attributes.Remove("readonly");
    }

}