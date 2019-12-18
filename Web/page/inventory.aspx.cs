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
    public partial class inventory : System.Web.UI.Page
    {
        protected clsAllnew cal = new clsAllnew();
        private string userid;
        private string gongsi;

        protected void Page_Load(object sender, EventArgs e)
        {
            try 
            {
                //if (!IsPostBack)
                //{
                    

                    List<ming_xi_info> list = new List<ming_xi_info>();
                    List<ku_cun> kc = new List<ku_cun>();
                    List<zl_and_jc_info> jichu = new List<zl_and_jc_info>();
                    string act = Request["act"] == null ? "" : Request["act"].ToString();
                    if (act.Equals("update"))
                    {
                        update();
                    }
                    else if (act.Equals("del"))
                    {
                        del();
                    }
                    else if (act.Equals("Jiansuo"))
                    {
                        list = selectWhere();
                    }
                    if (!act.Equals("Jiansuo")&&Session["username"] != null && Session["gs_name"] != null)
                    {
                        userid = Session["username"].ToString();
                        gongsi = Session["gs_name"].ToString();
                        list = cal.ming_xi_select(Session["username"].ToString(), Session["gs_name"].ToString()).FindAll(fn => fn.Mxtype.Equals("出库"));
                        kc = cal.SelectKucun().FindAll(fn => fn.gs_name.Equals(Session["gs_name"]) && fn.zh_name.Equals(Session["username"]));
                        jichu = cal.select_jczl(Session["username"].ToString(), Session["gs_name"].ToString());

                    }
                    RepAll.DataSource = list;
                    RepAll.DataBind();
                    repkucun.DataSource = kc;
                    repkucun.DataBind();
                    shouhuo.DataSource = jichu.FindAll(fd => !fd.Gong_huo.Equals("无") && !fd.Gong_huo.Equals(string.Empty));
                    shouhuo.DataBind();
                //}

            }
            catch (Exception ex) { throw ex; }
        }

        private List<ming_xi_info> selectWhere()
        {
            string orderid = Request["orderid"];
            string spname = Request["spname"];
            string splb = Request["splb"];
            List<ming_xi_info> list = cal.ming_xi_select(Session["username"].ToString(), Session["gs_name"].ToString()).FindAll(fn => fn.Mxtype.Equals("出库"));
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


        private void del()
        {
            try
            {
                if (Session["username"] != null && Session["gs_name"] != null)
                {
                    string id = Request["id"];
                    List<ming_xi_info> list = cal.ming_xi_select(Session["username"].ToString(), Session["gs_name"].ToString()).FindAll(fn => fn.Id == id);
                    cal.buindess_Delinput_ku(list);
                    Response.Write("[{\"endtext\":\"删除成功！\"}]");
                    Response.End();

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
                    string dj = Request["dj"];
                    string gonghuofang = Request["bz"];
                    string orderid = Request["orderid"];
                    userid = Session["username"].ToString();
                    gongsi = Session["gs_name"].ToString();
                    string shijian = Request["shijian"].ToString();
                    ming_xi_info mxi = new ming_xi_info();
                    mxi.Cplb = fl;
                    mxi.Cpname = name;
                    mxi.Cpsl = sl;
                    mxi.Cpsj = dj;
                    mxi.shou_h = gonghuofang;
                    mxi.Orderid = orderid;
                    mxi.zh_name = userid;
                    mxi.gs_name = gongsi;
                    mxi.Shijian = shijian;
                    mxi.Mxtype = "出库";
                    mxi.Id = Request["id"];
                    List<ming_xi_info> list = new List<ming_xi_info>();
                    list.Add(mxi);
                    cal.buindess_upinput_ku(list);
                    Response.Write("[{\"endtext\":\"提交成功！\"}]");
                    Response.End();
                }

            }
            catch (Exception ex) { throw ex; }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            try
            {
                List<ku_cun> kc = new List<ku_cun>();
                List<ming_xi_info> list = new List<ming_xi_info>();
                string kcid = "";
                string orderid = Context.Request["ckdh"];
                string shouhuo = Context.Request["ghf"];
                string date = Context.Request["shijian"];
                kc = cal.SelectKucun().FindAll(fn => fn.gs_name.Equals(Session["gs_name"]) && fn.zh_name.Equals(Session["username"]));
                for (int i = 0; i < kc.Count; i++) 
                {
                    string name = Request["ck" + i];
                    if (name != null)
                    {
                        string shoujia = Context.Request["txt" + i];
                        string shuliang = Context.Request["txtsl" + i];
                        kcid = Context.Request["ck" + i];
                        ku_cun k = kc.Find(fn => fn.Id.Equals(kcid));
                        if (Convert.ToInt32(k.Shu_liang) >Convert.ToInt32(k.Shu_liang) - Convert.ToInt32(shuliang)  )
                        {
                            ming_xi_info mxi = new ming_xi_info();
                            mxi.kcid = kcid;
                            mxi.Cplb = k.Lei_bie;
                            mxi.Cpname = k.Name;
                            mxi.Cpsl = shuliang;
                            mxi.Cpsj = shoujia;
                            mxi.shou_h = shouhuo;
                            mxi.Orderid = orderid;
                            mxi.zh_name = userid;
                            mxi.gs_name = gongsi;
                            mxi.Shijian = date;
                            mxi.Mxtype = "出库";
                            list.Add(mxi);
                        }
                    }
                }
                cal.addinput_ku(list,kcid);
                Page_Load(sender,e);
                Response.Write("<script>alert(出库成功！)</script>");
                //Response.Write("[{\"endtext\":\"提交成功！\"}]");
                //Response.End();

            }
            catch (Exception ex) 
            {
            
            }
        }
    }
}