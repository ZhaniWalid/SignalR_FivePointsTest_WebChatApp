﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SingalR_FivePointsTest_WebChatApp
{
    public partial class Register : System.Web.UI.Page
    {
        ConnClass ConnC = new ConnClass();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_ServerClick(object sender, EventArgs e)
        {
            string Query = "insert into tbl_Users(UserName,Email,Password)Values('" + txtName.Value + "','" + txtEmail.Value + "','" + txtPassword.Value + "')";
            string ExistQ = "select * from tbl_Users where Email='" + txtEmail.Value + "'";
            if (!ConnC.IsExist(ExistQ))
            {
                if (ConnC.ExecuteQuery(Query))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Message", "alert('Toutes nos félicitations!! Vous avez enregistré avec succès..');", true);
                    Session["UserName"] = txtName.Value;
                    Session["Email"] = txtEmail.Value;
                    Response.Redirect("Chat.aspx");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Message", "alert(' Cet email existe déjà !! SVP essayer avec un autre email.. ');", true);
            }
        }
    }
}