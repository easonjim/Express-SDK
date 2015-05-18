using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace kuaidi
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ComLib.LogLib.Log4NetBase.Log(DateTime.Now.ToString());
        }

        protected void btn_Call_Click(object sender, EventArgs e)
        {
            Response.Redirect("api.ashx?type=" + this.ddl_Type.SelectedValue + "&number=" + this.txt_Number.Text.Trim());
        }

        protected void btn_Call1_Click(object sender, EventArgs e)
        {
            Response.Redirect("api.ashx?method=get.name&name=" + this.txt_name.Text.Trim());
        }
    }
}