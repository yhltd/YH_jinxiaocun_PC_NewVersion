using clsBuiness;
using SDZdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.page{

    public partial class type1 : System.Web.UI.Page
    {
        protected clsAllnew cal = new clsAllnew();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string act = Request["act"] == null ? "" : Request["act"].ToString();
                 if (act.Equals("del"))
                {
                    del();
                }
                List<zl_and_jc_info> jichu = cal.select_jczl(Session["username"].ToString(), Session["gs_name"].ToString());
                Rep.DataSource = jichu;
                Rep.DataBind();
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }


        private void del()
        {
            try
            {
                if (Session["username"] != null && Session["gs_name"] != null)
                {
                    string id = Request["id"];
                    //List<ming_xi_info> list = cal.ming_xi_select(Session["username"].ToString(), Session["gs_name"].ToString()).FindAll(fn => fn.Id == id);
                    List<zl_and_jc_info> list = cal.select_jczl(Session["username"].ToString(), Session["gs_name"].ToString()).FindAll(fn => fn.id == id);
                    cal.del_jczl_ff(list[0].id);
                    Response.Write("[{\"endtext\":\"删除成功！\"}]");
                    Response.End();

                }
            }
            catch (Exception ex) { throw ex; }
        }



        protected void insert_Click(object sender, EventArgs e)
        {
            try 
            {
                string name = spname.Text;
                string fl = txt_fl.Text;
                string ghs = txt_ghf.Text;
                string shf = txt_shf.Text;
                zl_and_jc_info jc = new zl_and_jc_info() 
                {
                    name=name,
                    lei_bie = fl,
                    Gong_huo = ghs,
                    shou_huo=shf,
                    zh_name=Session["username"].ToString(),
                    gs_name= Session["gs_name"].ToString()
                };
                List<zl_and_jc_info> jczl = new List<zl_and_jc_info>();
                jczl.Add(jc);
                cal.add_jczl(jczl);
                Response.Write("<script>alert('添加成功');        window.location.href = 'type.aspx';</script>");
            }
            catch (Exception ex) { throw ex; }
        }
    }
}