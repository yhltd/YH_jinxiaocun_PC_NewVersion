using clsBuiness;
using SDZdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.page
{
    public partial class outBoud : System.Web.UI.Page
    {
        protected clsAllnew cal = new clsAllnew();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["gs_name"] != null)
            {
                List<ku_cun> list = new List<ku_cun>();
                list = cal.SelectKucun().FindAll(fn => fn.zh_name.Equals(Session["username"].ToString()) && fn.gs_name.Equals(Session["gs_name"].ToString()));
                rep.DataSource = list;
                rep.DataBind();
            }

        }
    }
}