using SDZdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsBuiness;
namespace Web.page
{
    public partial class YongLiaoJiChu : System.Web.UI.Page
    {
        protected clsAllnew can = new clsAllnew();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string act = Request["act"] == null ? "" : Request["act"].ToString();
                List<yong_liao_set_info> list = new List<yong_liao_set_info>();
                List<yong_liao_set_info> ziji = new List<yong_liao_set_info>();
                if (act.Equals("del"))
                {
                    delete();
                }
                else if (act.Equals("delSp")) 
                {
                    delectsp();
                }

                if (Session["username"] != null && Session["gs_name"] != null)
                {
                    list = can.yl_set_select(Session["username"].ToString(), Session["gs_name"].ToString()).GroupBy(g => g.cp_name).Select(s => s.First()).ToList<yong_liao_set_info>();
                    ziji = can.yl_set_select(Session["username"].ToString(), Session["gs_name"].ToString());

                }
                Session["fuji"] = list;
                Session["ziji"] = ziji;
                //zi.DataSource = ziji;
                //zi.DataBind();
                //fuji.DataSource = list;
                //fuji.DataBind();
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }


        protected void delete() 
        {
            string delid =Request["id"].ToString();
            int pd= can.del_yl_ff(delid);
            if (pd > 0) 
            {
                Response.Write("[{\"endtext\":\"删除成功！\"}]");
                Response.End();

            }
        }


        protected void delectsp() 
        {
            string delname = Request["name"].ToString();
            int pd = can.del_yl_ff_name(delname);
            if (pd > 0) 
            {
                Response.Write("[{\"endtext\":\"删除成功！\"}]");
                Response.End(); 
            }
        }
    }
}