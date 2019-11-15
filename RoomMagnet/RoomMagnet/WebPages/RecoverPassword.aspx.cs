﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebPages_RecoverPassword : System.Web.UI.Page
{
    private SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ToString());

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SignInEmail"] == null)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openLoginModal();", true);
        }
        else
        {
            var master = Master as RoomMagnet;
            master.AfterLogin();
        }
    }

    protected void RecoverPasswordSubmitEmailButton_Click(object sender, EventArgs e)
    {
        if (RecoverPasswordEnteredEmailCustomValidator.IsValid)
        {
            //Send out a email
            //generate a 5-diget random number 
            //show up aonther set of div
            string EnteredEmail = RecoverPasswordEnteredEmail.Text;
            string randomnumber = Randomnumber();
            ViewState["Number"] = randomnumber;
            EmailSender emailSender = new EmailSender();
            string emailbody = "Your Code is :" + randomnumber;
            emailSender.SendCode(EnteredEmail,emailbody);
            RecoverPasswordverification.Visible = true;
            RecoverPasswordverifiTxt.Visible = true;
            RecoverPasswordverifiButton.Visible = true;


        }

    }

    protected void RecoverPasswordEnteredEmailCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            if (cn.State == System.Data.ConnectionState.Closed)
            {
                cn.Open();
            }
            string sql = "Select Email from Users where Email=@Email";
            SqlCommand sqlCommand = new SqlCommand(sql, cn);
            sqlCommand.Parameters.AddWithValue("@Email", RecoverPasswordEnteredEmail.Text);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
            reader.Close();
            cn.Close();
        }
        catch
        {
            args.IsValid = false;
            RecoverPasswordEnteredEmailCustomValidator.ErrorMessage = "Connection Error,Please try again Later";
        }
    }
    public string Randomnumber ()
    {
        Random random = new Random();

        string number = random.Next(1, 9).ToString("D5");
        return number;
    }

    protected void RecoverPasswordverifiButton_Click(object sender, EventArgs e)
    {
        string Rnumber = RecoverPasswordverifiTxt.Text;
        if (Rnumber == ViewState["Number"].ToString())
        {
            // change password section up
        }
        else
        {
            ErrorLbl.Visible = true;
            ErrorLbl.Text = "Code Don't Match";
        }
    }
}