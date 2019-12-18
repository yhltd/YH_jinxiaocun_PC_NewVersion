using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsBuiness;
using SDZdb;
namespace Web.page
{
    public partial class type : System.Web.UI.Page
    {
        protected clsAllnew cal = new clsAllnew();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["gs_name"] != null) 
            {
                List<zl_and_jc_info> ziliao = cal.select_jczl(Session["username"].ToString(), Session["gs_name"].ToString());

            }
        }
    }
}