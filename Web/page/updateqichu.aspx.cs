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
    public partial class updateqichu : System.Web.UI.Page
    {
        protected clsAllnew cal = new clsAllnew();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"]!=null)
                {
                    string rkid = Request["id"];
                    qi_chu_info mxlist = cal.ck_sp_select(Session["username"].ToString(), Session["gs_name"].ToString()).Find(fd => fd.Id == rkid);
                    shuliang.Text = mxlist.Cpsl;
                    spid.Value = rkid;
                    Danjia.Text = mxlist.Cpjg;
                    List<zl_and_jc_info> jichu = cal.select_jczl(Session["username"].ToString(), Session["gs_name"].ToString());
                    shangpingname.DataSource = jichu;
                    shangpingname.DataBind();
                    fenlei.DataSource = jichu;
                    fenlei.DataBind();
                    //spid.Value = rkid;
                    //oid.InnerText = "更改前信息为：" + mxlist.Orderid;
                    fl.InnerText = "更改前信息为：" + mxlist.Cplb;
                    name.InnerText = "更改前信息为：" + mxlist.Cpname;
                    //ghf.InnerText = "更改前信息为：" + mxlist.shou_h;
                    sl.InnerText = "更改前信息为：" + mxlist.Cpsl;
                    dj.InnerText = "更改前信息为：" + mxlist.Cpsj;
                    //time.InnerText = "更改前信息为：" + mxlist.Shijian;
                }
            }
        }
    }
