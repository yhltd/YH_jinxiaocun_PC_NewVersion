using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using clsBuiness;
using SDZdb;
using System.Text;
using System.IO;
namespace Web.page
{
    public partial class inbound : System.Web.UI.Page
    {
        protected clsAllnew cal = new clsAllnew();
        protected string userid;
        protected string gongsi;
        protected void Page_Load(object sender, EventArgs e)
        {
            string act = "";
            try
            {
                //if (!IsPostBack)
                //{
                    List<ming_xi_info> list = new List<ming_xi_info>();

                    act = Request["act"] == null ? "" : Request["act"].ToString();
                    if (act.Equals("insert"))
                    {
                        insert();
                    }
                    else if (act.Equals("update"))
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
                    if (!act.Equals("Jiansuo") && Session["username"] != null && Session["gs_name"] != null)
                    {
                        userid = Session["username"].ToString();
                        gongsi = Session["gs_name"].ToString();
                        list = cal.ming_xi_select(Session["username"].ToString(), Session["gs_name"].ToString()).FindAll(fn => fn.Mxtype.Equals("入库"));

                        List<zl_and_jc_info> jichu = cal.select_jczl(Session["username"].ToString(), Session["gs_name"].ToString());
                        shangpingname.DataSource = jichu;
                        shangpingname.DataBind();
                        fenlei.DataSource = jichu;
                        fenlei.DataBind();
                        Gonghuofang.DataSource = jichu.FindAll(fd => !fd.Gong_huo.Equals("无") && !fd.Gong_huo.Equals(string.Empty));
                        Gonghuofang.DataBind();

                    }
                    //else
                    //{
                    //    return;
                    //}
                    Rep.DataSource = list;
                    Rep.DataBind();
                    //if (act.Equals("Jiansuo"))
                    //{
                    //    //var sb = new StringBuilder();
                    //    //var sw = new StringWriter(sb);
                    //    //HtmlTextWriter htw = new HtmlTextWriter(sw);
                    //    //Rep.RenderControl(htw);
                    //    //Response.Write(sb.ToString());
                    //    //Response.End();
                    //    //Response.Flush();
                    //}
                //}
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally 
            {
                if (act.Equals("Jiansuo"))
                {
                    //Response.End();
                    //Response.Flush();

                }
            }
        }

        private  void del()
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

        protected void Unnamed_Click(object sender, EventArgs e)
        {

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
                    mxi.Cpjg = dj;
                    mxi.Cpsj = dj;
                    mxi.shou_h = gonghuofang;
                    mxi.Orderid = orderid;
                    mxi.zh_name = userid;
                    mxi.gs_name = gongsi;
                    mxi.Shijian = shijian;
                    mxi.Mxtype = "入库";
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
                    string gonghuofang = Request["bz"];
                    string orderid = Request["orderid"];
                    userid = Session["username"].ToString();
                    gongsi = Session["gs_name"].ToString();
                    string shijian = Request["shijian"].ToString();
                    ming_xi_info mxi = new ming_xi_info();
                    mxi.Cplb = fl;
                    mxi.Cpname = name;
                    mxi.Cpsl = sl;
                    mxi.Cpjg = dj;
                    mxi.Cpsj = dj;
                    mxi.shou_h = gonghuofang;
                    mxi.Orderid = orderid;
                    mxi.zh_name = userid;
                    mxi.gs_name = gongsi;
                    mxi.Shijian = shijian;
                    mxi.Mxtype = "入库";
                    List<ming_xi_info> list = new List<ming_xi_info>();
                    list.Add(mxi);
                    cal.addinput_ku(list, "");
                    Response.Write("[{\"endtext\":\"提交成功！\"}]");
                    Response.End();
                }
            }
            catch (Exception ex) { throw ex; }
        }
        protected void insert_Click(object sender, EventArgs e)
        {
            //try 
            //{
            //    ming_xi_info mxi = new ming_xi_info();
            //    mxi.Cplb = 
            //}
            //catch (Exception ex) 
            //{
            //    throw ex;
            //}
        }

        protected void select_Click(object sender, EventArgs e)
        {
            try
            {
                selectWhere();
                //string sqlstr = "select * from yh_jinxiaocun_mingxi where mxtype='入库'";
                //if (orderid.Equals(string.Empty) && spname.Equals(string.Empty) && splb.Equals(string.Empty)) 
                //{
                //    Response.Write("<script>alert('检索条件不能为空');history.go(0)</script>");
                //}
                //else
                //{
                //    if (!orderid.Equals(string.Empty)) 
                //    {
                //            sqlstr=sqlstr+"orderid like '%"+orderid+"%'";
                //    }
                //    if (!spname.Equals(string.Empty)) 
                //    {
                //        sqlstr = sqlstr + "cpname like '%"+spname+"%'";
                //    }
                //    if (!splb.Equals(string.Empty))
                //    {
                //        sqlstr = sqlstr + "cplb like '%" + splb + "%'";
                //    }
                //}

            }
            catch (Exception ex) 
            {
                throw ex;

            }
        }

        private List<ming_xi_info> selectWhere()
        {
            string orderid = Request["orderid"];
            string spname = Request["spname"];
            string splb = Request["splb"];
            List<ming_xi_info> list = cal.ming_xi_select(Session["username"].ToString(), Session["gs_name"].ToString()).FindAll(fn => fn.Mxtype.Equals("入库"));
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