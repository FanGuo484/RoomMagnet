﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
public partial class WebPages_Message : System.Web.UI.Page
{
    ArrayList ReceiverIDs = new ArrayList();
    SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnectionString"].ToString());
    int ReceiverID;
    string SenderID;
    string ReceiverName = "";
    int OldConversationID = 0;
    int NewConversationID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SingInEmail"] == null)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openLoginModal();", true);
        }
        else
        {
            var master = Master as RoomMagnet;
            master.AfterLogin();
        }
        cn.Open();
        string msg = (string)Application["message"];
        txtmsg.Text = msg;
        Session["ResultPropertyID"] = 1003;
        string PID = Session["ResultPropertyID"].ToString();
        if (Session["Roles"].ToString() == "Renter")
        {
            string sql = "SELECT  Users.FirstName, Users.LastName,Property.HostID FROM Users INNER JOIN Property ON Users.UserID = Property.HostID where Property.PropertyID =" + PID;
            SqlCommand sqlCommand = new SqlCommand(sql, cn);
            SqlDataReader dataReader = sqlCommand.ExecuteReader();
            if (dataReader.Read())
            {
                string fn = dataReader.GetString(0);
                string ln = dataReader.GetString(1);
                ReceiverName = fn + " " + ln;
                ReceiverID = dataReader.GetInt32(2);
            }
            dataReader.Close();
        }
        else
        {
            int SenderID = 0;
            for (int i = 0; i < Conversation.ConversationCount; i++)
            {
                if (Convert.ToInt32(Session["UserID"].ToString()) == Conversation.conversations[i].getRecieverID())
                {
                    string sql9 = "Select SenderID, max(MessageID) as latestMessage, messageContent from Conversations inner join Message on Conversations.ConversationID = Message.ConversationID" +
                         " group by SenderID, MessageContent order by latestMessage desc ";
                    SqlCommand sqlCommand9 = new SqlCommand(sql9, cn);
                    SqlDataReader dataReader9 = sqlCommand9.ExecuteReader();
                    if (dataReader9.Read())
                    {
                        int tempSenderID = dataReader9.GetInt32(0);
                        if (tempSenderID != SenderID)
                        {
                            if (ReceiverIDs.Contains(tempSenderID) == false)
                            {
                                ReceiverIDs.Add(dataReader9.GetInt32(0));
                                Messages.Text += dataReader9.GetString(2) + Environment.NewLine;
                                SenderID = dataReader9.GetInt32(0);
                            }
                        }
                    }
                    break;
                }
                FillDropDown(ReceiverIDs); 







                string SenderID2 = Session["UserID"].ToString();

                string sql2 = "Select ConversationID FROM Conversations where SenderID=" + SenderID2 + "And ReceiverID=" + ReceiverID;
                SqlCommand sqlCommand2 = new SqlCommand(sql2, cn);
                SqlDataReader reader = sqlCommand2.ExecuteReader();
                if (reader.Read())
                {
                    OldConversationID = reader.GetInt32(0);
                }
                else
                {
                    reader.Close();
                    string sql3 = "Insert into Conversations values (@SenderId,@ReceiverID)";
                    SqlCommand sqlCommand3 = new SqlCommand(sql3, cn);
                    sqlCommand3.Parameters.AddWithValue("@SenderId", SenderID2);
                    sqlCommand3.Parameters.AddWithValue("@ReceiverID", ReceiverID);
                    sqlCommand3.ExecuteNonQuery();
                    //after instering finds the new conversation ID 
                    Conversation newConvo = new Conversation(Int32.Parse(SenderID2), ReceiverID);
                    Conversation.conversations[Conversation.ConversationCount - 1] = newConvo;


                    string sql4 = "Select ConversationID FROM Conversations where SenderID = " + SenderID2 + " AND ReceiverID = " + ReceiverID;
                    SqlCommand sqlCommand4 = new SqlCommand(sql4, cn);
                    SqlDataReader reader2 = sqlCommand4.ExecuteReader();
                    if (reader2.Read())
                    {
                        NewConversationID = reader2.GetInt32(0);
                        newConvo.setConversationID(NewConversationID);
                    }
                    reader2.Close();

                }
                reader.Close();
                //if (OldConversationID != 0)
                //{
                //    string sql5 = "Select MessageContent from Message where ConversationID=" + OldConversationID;
                //    SqlCommand sqlCommand5 = new SqlCommand(sql5, cn);
                //    SqlDataReader reader3 = sqlCommand5.ExecuteReader();
                //    if (reader3.HasRows)
                //    {
                //        while (reader3.Read())
                //        {
                //            txtmsg.Text = Application["message"] + reader3.GetString(0) + Environment.NewLine;
                //        }
                //        reader3.NextResult();
                //    }
                //    reader3.Close();
                //}
                cn.Close();
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        string Sendername = Session["FullName"].ToString();
        string message = txtsend.Text;
        string my = Sendername + ":" + message;
        string sql4 = "";

        txtsend.Text = "";



        cn.Open();
        if (NewConversationID == 0)
        {
            string sql = "Insert into Message(ConversationID,MessageContent,LastUpdated,LastUpdatedBy) values (@ConversationID,@MessageContent,@LastUpdated,@LastUpdatedBy)";
            SqlCommand command = new SqlCommand(sql, cn);
            command.Parameters.AddWithValue("@ConversationID", OldConversationID);
            command.Parameters.AddWithValue("@MessageContent", my);
            command.Parameters.AddWithValue("@LastUpdated", DateTime.Now);
            command.Parameters.AddWithValue("@LastUpdatedBy", Sendername);
            command.ExecuteNonQuery();
            sql4 = "Select MessageID from Message WHERE ConversationID= " + OldConversationID + "AND MessageContent= " + my + " AND LastUpdatedBy= " + Sendername;

        }
        else
        {
            string sql2 = "Insert into Message(ConversationID,MessageContent,LastUpdated,LastUpdatedBy) values (@ConversationID,@MessageContent,@LastUpdated,@LastUpdatedBy)";
            SqlCommand command2 = new SqlCommand(sql2, cn);
            command2.Parameters.AddWithValue("@ConversationID", NewConversationID);
            command2.Parameters.AddWithValue("@MessageContent", my);
            command2.Parameters.AddWithValue("@LastUpdated", DateTime.Now);
            command2.Parameters.AddWithValue("@LastUpdatedBy", Sendername);
            NewConversationID = 0;
            command2.ExecuteNonQuery();
            sql4 = "Select MessageID from Message WHERE ConversationID= " + NewConversationID + " AND MessageContent= " + my + " AND LastUpdatedBy= " + Sendername;
        }


        cn.Close();



        Response.Redirect("Message.aspx");
    }

    protected void Clear_Click(object sender, EventArgs e)
    {
        Application["message"] = "";
        Response.Redirect("Message.aspx");
    }
    public void FillDropDown(ArrayList ReceiverIDs)
    {
        cn.Open();
        for (int i = 0; i < ReceiverIDs.Count; i++)
        {
            string sql = "Select FirstName, LastName WHERE UserID=" + ReceiverIDs[i].ToString();
            SqlCommand sqlCommand = new SqlCommand(sql, cn);
            SqlDataReader reader3 = sqlCommand.ExecuteReader();
            if (reader3.HasRows)
            {
                while (reader3.Read())
                {
                    RenterNames.Items.Add(new ListItem ((reader3.GetString(0) + " " + reader3.GetString(1)),ReceiverIDs[i].ToString()));
                    
                }
            }
        }
        
        cn.Close();
    }
}

