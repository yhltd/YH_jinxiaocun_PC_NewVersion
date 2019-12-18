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
    public partial class warning : System.Web.UI.Page
    {
        protected clsAllnew cal = new clsAllnew();
        protected string userid;
        protected string gongsi;

        protected void Page_Load(object sender, EventArgs e)
        {
            string act = Request["act"]==null ? "" : Request["act"].ToString();
            List<ming_xi_info> list = new List<ming_xi_info>();
            if (act.Equals("Jiansuo")) 
            {
                list = selectWhere();

            }
            if (!act.Equals("Jiansuo") && Session["username"] != null && Session["gs_name"] != null)
            {
                userid = Session["username"].ToString();
                gongsi = Session["gs_name"].ToString();
                list = cal.ming_xi_select(Session["username"].ToString(), Session["gs_name"].ToString());

            }
            Rep.DataSource = list;
            Rep.DataBind();

        }




        private List<ming_xi_info> selectWhere()
        {
            string orderid = Request["orderid"];
            string spname = Request["spname"];
            string splb = Request["splb"];
            List<ming_xi_info> list = cal.ming_xi_select(Session["username"].ToString(), Session["gs_name"].ToString());
            List<ming_xi_info> selectList = new List<ming_xi_info>();
            if (orderid.Equals(string.Empty) && spname.Equals(string.Empty) && splb.Equals(string.Empty))
            {
                //Response.Write("<script>alert('检索条件不能为空');history.go(0)</script>");
            }
            else
            {
                if (!orderid.Equals(string.Empty) && !spname.Equals(string.Empty) && !splb.Equals(string.Empty))
                {
                    selectList = list.FindAll(fn => fn.Orderid.Contains(orderid) && fn.Cpname.Contains(spname) && fn.Cplb.Contains(splb));
                }
                else if (!orderid.Equals(string.Empty) && spname.Equals(string.Empty) && splb.Equals(string.Empty))
                {
                    selectList = list.FindAll(fn => fn.Orderid.Contains(orderid));
                }
                else if (!orderid.Equals(string.Empty) && !spname.Equals(string.Empty) && splb.Equals(string.Empty))
                {
                    selectList = list.FindAll(fn => fn.Orderid.Contains(orderid) && fn.Cpname.Contains(spname));
                }
                else if (!orderid.Equals(string.Empty) && spname.Equals(string.Empty) && !splb.Equals(string.Empty))
                {
                    selectList = list.FindAll(fn => fn.Orderid.Contains(orderid) && fn.Cplb.Contains(splb));
                }
                else if (orderid.Equals(string.Empty) && !spname.Equals(string.Empty) && splb.Equals(string.Empty))
                {
                    selectList = list.FindAll(fn => fn.Cpname.Contains(spname));
                }
                else if (orderid.Equals(string.Empty) && !spname.Equals(string.Empty) && !splb.Equals(string.Empty))
                {
                    selectList = list.FindAll(fn => fn.Cpname.Contains(spname) && fn.Cplb.Contains(splb));
                }
                else if (orderid.Equals(string.Empty) && spname.Equals(string.Empty) && !splb.Equals(string.Empty))
                {
                    selectList = list.FindAll(fn => fn.Cplb.Contains(splb));
                }

                //Rep.DataSource = selectList;
                //Rep.DataBind();
                //Response.Write("[{\"endtext\":\"检索成功！\"}]");
                //Response.End();
            }
            return selectList;
        }

    }
}