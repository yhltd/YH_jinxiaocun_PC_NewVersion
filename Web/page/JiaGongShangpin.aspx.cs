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
    public partial class JiaGongShangpin : System.Web.UI.Page
    {
        protected clsAllnew cal = new clsAllnew();

        protected void Page_Load(object sender, EventArgs e)
        {
            try 
            {
                if (Session["username"] != null && Session["gs_name"] != null)
                {
                    List<yong_liao_set_info> list = cal.yl_set_select(Session["username"].ToString(), Session["gs_name"].ToString()).GroupBy(g => g.cp_name).Select(s=>s.First()).ToList<yong_liao_set_info>();
                    List<zl_and_jc_info>  jichu = cal.select_jczl(Session["username"].ToString(), Session["gs_name"].ToString());

                    repkucun.DataSource = list;
                    repkucun.DataBind();
                    shouhuo.DataSource = jichu.FindAll(fd => !fd.Gong_huo.Equals("无") && !fd.Gong_huo.Equals(string.Empty));
                    shouhuo.DataBind();

                }
            }
            catch (Exception ex) { throw ex; }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            try
            {
                List<ku_cun> kc = new List<ku_cun>();
                List<ming_xi_info> mxlist = new List<ming_xi_info>();
                string ylname = "";
                List<yong_liao_set_info> list = cal.yl_set_select(Session["username"].ToString(), Session["gs_name"].ToString());

                //string orderid = Context.Request["ckdh"];
                string orderid = Context.Request["ckdh"];
                kc = cal.SelectKucun().FindAll(fn => fn.gs_name.Equals(Session["gs_name"]) && fn.zh_name.Equals(Session["username"]));

                string shouhuo = Context.Request["ghf"];
                string date = Context.Request["shijian"];
                for (int i = 0; i < list.Count; i++)
                {
                    string name = Request["ck" + i];
                    if (name != null)
                    {
                        string shoujia = Context.Request["txt" + i];
                        string shuliang = Context.Request["txtsl" + i];

                        ylname = Context.Request["ck" + i];
                        List<yong_liao_set_info> yl = list.FindAll(fn => fn.cp_name.Equals(ylname));
                        for (int yli = 0; yli < yl.Count; yli++)
                        {
                            ku_cun k = kc.Find(fn => fn.Name.Equals(yl[yli].yl_name));

                            if (Convert.ToInt32(k.Shu_liang) > Convert.ToInt32(k.Shu_liang) - (Convert.ToInt32(yl[yli].yl_sl.ToString() )* Convert.ToInt32(shuliang)))
                            {
                                ming_xi_info mxi = new ming_xi_info();
                                mxi.kcid = k.Id;
                                mxi.Cplb = k.Lei_bie;
                                mxi.Cpname = k.Name;
                                mxi.Cpsl = yl[yli].yl_sl;
                                mxi.Cpsj = (Convert.ToInt32(shoujia)/yl.Count).ToString();
                                mxi.shou_h = shouhuo;
                                mxi.Orderid = orderid;
                                mxi.zh_name = Session["username"].ToString();
                                mxi.gs_name = Session["gs_name"].ToString();
                                mxi.Shijian = date;
                                mxi.Mxtype = "出库";
                                mxlist.Add(mxi);
                            }

                        }
                    }
                }
                cal.addinput_ku(mxlist, "");
                Page_Load(sender, e);
                Response.Write("<script>alert(销售成功！)</script>");
                //Response.Write("[{\"endtext\":\"提交成功！\"}]");
                //Response.End();

            }
            catch (Exception ex)
            {

            }

        }
    }
}