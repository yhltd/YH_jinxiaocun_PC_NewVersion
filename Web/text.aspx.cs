using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class text : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> arr = new List<string>();
            arr.Add("a");
            arr.Add("b");
            arr.Add("c");
            Session["all"] = arr;
        }
    }
}