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
    public partial class updatePwd : System.Web.UI.Page
    {
        protected clsAllnew can = new clsAllnew();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["username"] != null && Session["gs_name"] != null)
                {
                    string act = Request["act"] == null ? "" : Request["act"].ToString();
                    if (act.Equals("del")) 
                    {
                        del();
                    }
                    else if (act.Equals("insert"))
                    {
                        insert();
                    }
                    else if (act.Equals("update"))
                    {
                        update();
                    }
                    List<qi_chu_info> list = can.ck_sp_select(Session["username"].ToString(), Session["gs_name"].ToString());
                    int listi = 0;

                    foreach (qi_chu_info qci in list) 
                    {
                        if (qci.Cpsl.ToString() == "undefined") 
                        {
                            list[listi].Cpsl = "0";
                        }

                        if (qci.Cpsj.ToString() == "undefined")
                        {
                            list[listi].Cpsj = "0";
                        }
                        listi++;

                    }
                    List<zl_and_jc_info> jichu = can.select_jczl(Session["username"].ToString(), Session["gs_name"].ToString());
                    shangpingname.DataSource = jichu;
                    shangpingname.DataBind();
                    fenlei.DataSource = jichu;
                    fenlei.DataBind();
                    RepAll.DataSource = list;
                    RepAll.DataBind();
                }
            }
            catch (Exception ex) { throw ex; }
        }





        protected void update()
        {
            try
            {
                if (Session["username"] != null && Session["gs_name"] != null)
                {
                    string fl = Request["fl"];
                    string name = Request["name"];
                    string sl = Request["sl"];
                    string id = Request["id"];
                    string dj = Request["dj"];
                    //string gonghuofang = Request["bz"];
                    //string orderid = Request["orderid"];
                    string userid = Session["username"].ToString();
                    string gongsi = Session["gs_name"].ToString();
                    //string shijian = Request["shijian"].ToString();
                    qi_chu_info qc = new qi_chu_info();
                    qc.Cplb = fl;
                    qc.Cpname = name;
                    qc.Cpsl = sl;
                    qc.Cpsj = dj;
                    //mxi.shou_h = gonghuofang;
                    //mxi.Orderid = orderid;
                    qc.zh_name = userid;
                    qc.gs_name = gongsi;
                    //mxi.Shijian = shijian;
                    //mxi.Mxtype = "入库";
                    //List<qi_chu_info> list = new List<qi_chu_info>();
                    //list.Add(qc);
                    can.update_qichu(id, qc.Cpname, qc.Cplb, qc.Cpsj, qc.Cpsl);
                    Response.Write("[{\"endtext\":\"提交成功！\"}]");
                    Response.End();
                }

            }
            catch (Exception ex) { throw ex; }
        }








        protected void insert()
        {
            try
            {
                if (Session["username"] != null && Session["gs_name"] != null)
                {
                    string fl = Request["fl"];
                    string name = Request["name"];
                    string sl = Request["sl"];
                    string dj = Request["dj"];
                    //string gonghuofang = Request["bz"];
                    //string orderid = Request["orderid"];
                    string userid = Session["username"].ToString();
                    string gongsi = Session["gs_name"].ToString();
                    //string shijian = Request["shijian"].ToString();
                    qi_chu_info qc = new qi_chu_info();
                    qc.Cplb = fl;
                    qc.Cpname = name;
                    qc.Cpsl = sl;
                    qc.Cpsj = dj;
                    //mxi.shou_h = gonghuofang;
                    //mxi.Orderid = orderid;
                    qc.zh_name = userid;
                    qc.gs_name = gongsi;
                    //mxi.Shijian = shijian;
                    //mxi.Mxtype = "入库";
                    List<qi_chu_info> list = new List<qi_chu_info>();
                    list.Add(qc);
                    can.add_qichu(list);
                    //cal.addinput_ku(list, "");
                    Response.Write("[{\"endtext\":\"提交成功！\"}]");
                    Response.End();
                }
            }
            catch (Exception ex) { throw ex; }
        }




        private void del()
        {
            try
            {
                if (Session["username"] != null && Session["gs_name"] != null)
                {
                    string id = Request["id"];
                    //List<ming_xi_info> list =can.del_qichu_ff(Session["username"].ToString(), Session["gs_name"].ToString()).FindAll(fn => fn.Id == id);
                    can.del_qichu_ff(id);
                    Response.Write("[{\"endtext\":\"删除成功！\"}]");
                    Response.End();

                }
            }
            catch (Exception ex) { throw ex; }
        }

    }
}