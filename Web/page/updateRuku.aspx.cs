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
    public partial class update : System.Web.UI.Page
    {
        protected clsAllnew cal = new clsAllnew();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null && Session["gs_name"] != null) 
            {
                if (Request["id"]!=null)
                {
                    string rkid = Request["id"];
                    ming_xi_info mxlist = cal.ming_xi_select(Session["username"].ToString(), Session["gs_name"].ToString()).Find(fd => fd.Id == rkid);
                    orderid.Text = mxlist.Orderid;
                    shuliang.Text = mxlist.Cpsl;
                    Danjia.Text = mxlist.Cpjg;
                    List<zl_and_jc_info> jichu = cal.select_jczl(Session["username"].ToString(), Session["gs_name"].ToString());
                    shangpingname.DataSource = jichu;
                    shangpingname.DataBind();
                    fenlei.DataSource = jichu;
                    fenlei.DataBind();
                    Gonghuofang.DataSource = jichu.FindAll(fd => !fd.Gong_huo.Equals("无") && !fd.Gong_huo.Equals(string.Empty));
                    Gonghuofang.DataBind();
                    spid.Value = rkid;
                    oid.InnerText = "更改前信息为：" + mxlist.Orderid;
                    fl.InnerText = "更改前信息为：" + mxlist.Cplb;
                    name.InnerText = "更改前信息为：" + mxlist.Cpname;
                    ghf.InnerText = "更改前信息为：" + mxlist.shou_h;
                    sl.InnerText = "更改前信息为：" + mxlist.Cpsl;
                    dj.InnerText = "更改前信息为：" + mxlist.Cpjg;
                    time.InnerText = "更改前信息为：" + mxlist.Shijian;
                }
            }
        }
    }
}