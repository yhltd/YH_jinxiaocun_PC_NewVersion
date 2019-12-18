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
    public partial class InsertShangping : System.Web.UI.Page
    {
        protected clsAllnew can = new clsAllnew();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["username"] != null && Session["gs_name"] != null)
                {
                    List<ku_cun> kc = can.selectkcALL();
                    Session["kc"] = kc;
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            try
            {
                List<yong_liao_set_info> list = new List<yong_liao_set_info>();
                int TableLength = Convert.ToInt32(Request["sumrow"].ToString());
                string shangpingName = spname.Text;
                if (shangpingName != "")
                {
                    for (int ti = 0; ti < TableLength; ti++)
                    {
                        yong_liao_set_info yl = new yong_liao_set_info();
                        string clname = Request["cailiaoName" + ti];
                        string xhsl = Request["sl" + ti];
                        yl.yl_name = clname;
                        yl.yl_sl = xhsl;
                        yl.cp_name = shangpingName;
                        yl.zh_name = Session["username"].ToString();
                        yl.gs_name = Session["gs_name"].ToString();

                        list.Add(yl);

                    }
                }
                else 
                {
                    Response.Write("<script>alert('添加失败！原因：商品名称不能为空！');window.location.href = 'YongLiaoJiChu.aspx';</script>");
                }
                if (list.Count != 0)
                {
                    can.add_yl(list);
                }
                Response.Write("<script>alert('添加完成！');window.location.href = 'YongLiaoJiChu.aspx';</script>");
            }
            catch (Exception ex) { throw ex; }
        }
    }
}