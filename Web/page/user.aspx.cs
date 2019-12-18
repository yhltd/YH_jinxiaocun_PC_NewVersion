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
    public partial class user : System.Web.UI.Page
    {
        protected clsAllnew can = new clsAllnew();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["username"] != null && Session["gs_name"] != null)
            {

                string act = Request["act"] == null ? "" : Request["act"];
                List<jxc_z_info> list = new List<jxc_z_info>();

                if (act.Equals("Jiansuo"))
                {
                    list = jiansuo();
                }
                else
                {
                    list = can.jinxiaocun_hz(Session["username"].ToString(), Session["gs_name"].ToString());
                    for (int i = 0; i < list.Count; i++)
                    {


                        if (list[i].Cpsl_3 == "")
                        {
                            list[i].jc_jc = (0 + Convert.ToInt32(list[i].Cpsl_1) - Convert.ToInt32(list[i].Cpsl_2)).ToString();
                        }
                        else if (list[i].Cpsl_1 == "")
                        {
                            list[i].jc_jc = (Convert.ToInt32(list[i].Cpsl_3) + 0 - Convert.ToInt32(list[i].Cpsl_2)).ToString();
                        }
                        else if (list[i].Cpsl_2 == "")
                        {
                            list[i].jc_jc = (Convert.ToInt32(list[i].Cpsl_3) + Convert.ToInt32(list[i].Cpsl_1) - 0).ToString();
                        }
                        else
                        {
                            list[i].jc_jc = (Convert.ToInt32(list[i].Cpsl_3) + Convert.ToInt32(list[i].Cpsl_1) - Convert.ToInt32(list[i].Cpsl_2)).ToString();
                        }



                        //if (Convert.ToInt32(list[i].jc_jc) >= 0)
                        //{
                        //    if (list[i].Cpje_3 == "")
                        //    {
                        //        list[i].jc_je = (0 + Convert.ToInt32(list[i].Cpje_1) - Convert.ToInt32(list[i].Cpje_2)).ToString();
                        //    }
                        //    else if (list[i].Cpje_1 == "")
                        //    {
                        //        list[i].jc_je = (Convert.ToInt32(list[i].Cpje_3) + 0 - Convert.ToInt32(list[i].Cpje_2)).ToString();
                        //    }
                        //    else if (list[i].Cpje_2 == "")
                        //    {
                        //        list[i].jc_je = (Convert.ToInt32(list[i].Cpje_3) + Convert.ToInt32(list[i].Cpje_1) - 0).ToString();
                        //    }
                        //    else
                        //    {
                        //        list[i].jc_je = (Convert.ToInt32(list[i].Cpje_3) + Convert.ToInt32(list[i].Cpje_1) - Convert.ToInt32(list[i].Cpje_2)).ToString();
                        //    }
                        //}
                        //else
                        //{
                        //    list[i].jc_je = "0";
                        //}



                        if (Convert.ToInt32(list[i].jc_jc) > 0)
                        {
                            list[i].jc_dj = (Convert.ToInt32(list[i].jc_je) / Convert.ToInt32(list[i].jc_jc)).ToString();
                        }
                        else
                        {
                            list[i].jc_dj = "0";
                        }
                        list[i].yl_tx = "25";
                    }

                    

                }
                List<zl_and_jc_info> jichu = can.select_jczl(Session["username"].ToString(), Session["gs_name"].ToString());
                shf.DataSource = jichu;
                shf.DataBind();
                ghs.DataSource = jichu;
                ghs.DataBind();
                spname.DataSource = jichu;
                spname.DataBind();
                fenlei.DataSource = jichu;
                fenlei.DataBind();
                Rep.DataSource = list;
                Rep.DataBind();
            }
        }




        protected List<jxc_z_info> jiansuo() 
        {
            List<jxc_z_info> list = new List<jxc_z_info>();
            string name= Request["name"];//产品名称
            string leibie = Request["fenlei"];//产品类别
            string ghs = Request["ghs"];
            string shf = Request["shf"];
            string stardate = Request["startdate"];
            string enddate = Request["enddate"];
            list = can.jinxiaocun_hz_Where(Session["username"].ToString(), Session["gs_name"].ToString(), stardate, enddate, name, leibie, ghs, shf);
            return list;
        }
    }
}