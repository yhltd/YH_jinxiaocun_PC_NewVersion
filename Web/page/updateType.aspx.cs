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
    public partial class updateType : System.Web.UI.Page
    {
        protected clsAllnew cal = new clsAllnew();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["gs_name"] != null)
            {
                if (Request["id"] != null)
                {
                    string id = Request["id"];
                    zl_and_jc_info select = cal.select_jczl(Session["username"].ToString(), Session["gs_name"].ToString()).Find(f=>f.id==id);
                    sm_spname.InnerText ="修改前信息为:"+ select.name;
                    sm_fenlei.InnerText = "修改前信息为:" + select.lei_bie;
                    sm_ghf.InnerText = "修改前信息为:" + select.Gong_huo;
                    sm_shf.InnerText = "修改前信息为:" + select.shou_huo;
                   
                }
            }

        }

        protected void update_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["gs_name"] != null)
            {
                string name = spname.Text;
                string fl = txt_fl.Text;
                string gh = txt_ghf.Text;
                string sh = txt_shf.Text;
                string id = Request["id"];
                int end =  cal.update_jczl("", name, fl, "", sh, gh, id);
                if (end > 0) 
                {
                    Response.Write("<script>alert('提交成功！');window.location.href = '/page/type.aspx';</script>");
                }

            }
        }
    }
}