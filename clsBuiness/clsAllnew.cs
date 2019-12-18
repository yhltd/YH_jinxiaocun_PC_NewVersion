﻿using China_System.Common;
using Order.Common;
using SDZdb;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Windows.Forms;

namespace clsBuiness
{
    public enum ProcessStatus
    {
        初始化,
        登录界面,
        确认YES,
        第一页面,
        第二页面,
        Filter下拉,
        关闭页面,
        结束页面

    }
    public class clsAllnew
    {

        string newsth;
        public BackgroundWorker bgWorker1;
        private ProcessStatus isrun = ProcessStatus.初始化;
        public ToolStripProgressBar pbStatus { get; set; }
        public ToolStripStatusLabel tsStatusLabel1 { get; set; }
        public ToolStripStatusLabel tsStatusLabel2 { get; set; }
        private DateTime StopTime;
        List<clsuserinfo> userinfo_webResult;
        public string ConStr;
        public string ConStrPIC;
        private System.Windows.Forms.PictureBox picboxPhoto;
        string mdbpath2_Ctirx = AppDomain.CurrentDomain.BaseDirectory + "\\Hello.jpg";//记录 Status  click 和选择哪个服务器
        public string servename;

        #region dll

        public static Boolean IsConnected = false;
        public static Boolean IsAuthenticate = false;
        public static Boolean IsRead_Content = false;
        public static int Port = 0;
        public static int ComPort = 0;
        public const int cbDataSize = 128;
        public const int GphotoSize = 256 * 1024;

        [DllImport("termb.dll")]
        static extern int InitComm(int port);//连接身份证阅读器 

        [DllImport("termb.dll")]
        static extern int InitCommExt();//自动搜索身份证阅读器并连接身份证阅读器 

        [DllImport("termb.dll")]
        static extern int CloseComm();//断开与身份证阅读器连接 

        [DllImport("termb.dll")]
        static extern int Authenticate();//判断是否有放卡，且是否身份证 

        [DllImport("termb.dll")]
        public static extern int Read_Content(int index);//读卡操作,信息文件存储在dll所在下

        [DllImport("termb.dll")]
        public static extern int ReadContent(int index);//读卡操作,信息文件存储在dll所在下

        [DllImport("termb.dll")]
        static extern int GetSAMID(StringBuilder SAMID);//获取SAM模块编号

        [DllImport("termb.dll")]
        static extern int GetSAMIDEx(StringBuilder SAMID);//获取SAM模块编号（10位编号）

        [DllImport("termb.dll")]
        static extern int GetBmpPhoto(string PhotoPath);//解析身份证照片

        [DllImport("termb.dll")]
        static extern int GetBmpPhotoToMem(byte[] imageData, int cbImageData);//解析身份证照片

        [DllImport("termb.dll")]
        static extern int GetBmpPhotoExt();//解析身份证照片

        [DllImport("termb.dll")]
        static extern int Reset_SAM();//重置Sam模块

        [DllImport("termb.dll")]
        static extern int GetSAMStatus();//获取SAM模块状态 

        [DllImport("termb.dll")]
        static extern int GetCardInfo(int index, StringBuilder value);//解析身份证信息 

        [DllImport("termb.dll")]
        static extern int ExportCardImageV();//生成竖版身份证正反两面图片(输出目录：dll所在目录的cardv.jpg和SetCardJPGPathNameV指定路径)

        [DllImport("termb.dll")]
        static extern int ExportCardImageH();//生成横版身份证正反两面图片(输出目录：dll所在目录的cardh.jpg和SetCardJPGPathNameH指定路径) 

        [DllImport("termb.dll")]
        static extern int SetTempDir(string DirPath);//设置生成文件临时目录

        [DllImport("termb.dll")]
        static extern int GetTempDir(StringBuilder path, int cbPath);//获取文件生成临时目录

        [DllImport("termb.dll")]
        static extern void GetPhotoJPGPathName(StringBuilder path, int cbPath);//获取jpg头像全路径名 


        [DllImport("termb.dll")]
        static extern int SetPhotoJPGPathName(string path);//设置jpg头像全路径名

        [DllImport("termb.dll")]
        static extern int SetCardJPGPathNameV(string path);//设置竖版身份证正反两面图片全路径

        [DllImport("termb.dll")]
        static extern int GetCardJPGPathNameV(StringBuilder path, int cbPath);//获取竖版身份证正反两面图片全路径

        [DllImport("termb.dll")]
        static extern int SetCardJPGPathNameH(string path);//设置横版身份证正反两面图片全路径

        [DllImport("termb.dll")]
        static extern int GetCardJPGPathNameH(StringBuilder path, int cbPath);//获取横版身份证正反两面图片全路径

        [DllImport("termb.dll")]
        static extern int getName(StringBuilder data, int cbData);//获取姓名

        [DllImport("termb.dll")]
        static extern int getSex(StringBuilder data, int cbData);//获取性别

        [DllImport("termb.dll")]
        static extern int getNation(StringBuilder data, int cbData);//获取民族

        [DllImport("termb.dll")]
        static extern int getBirthdate(StringBuilder data, int cbData);//获取生日(YYYYMMDD)

        [DllImport("termb.dll")]
        static extern int getAddress(StringBuilder data, int cbData);//获取地址

        [DllImport("termb.dll")]
        static extern int getIDNum(StringBuilder data, int cbData);//获取身份证号

        [DllImport("termb.dll")]
        static extern int getIssue(StringBuilder data, int cbData);//获取签发机关

        [DllImport("termb.dll")]
        static extern int getEffectedDate(StringBuilder data, int cbData);//获取有效期起始日期(YYYYMMDD)

        [DllImport("termb.dll")]
        static extern int getExpiredDate(StringBuilder data, int cbData);//获取有效期截止日期(YYYYMMDD) 

        [DllImport("termb.dll")]
        static extern int getBMPPhotoBase64(StringBuilder data, int cbData);//获取BMP头像Base64编码 

        [DllImport("termb.dll")]
        static extern int getJPGPhotoBase64(StringBuilder data, int cbData);//获取JPG头像Base64编码

        [DllImport("termb.dll")]
        static extern int getJPGCardBase64V(StringBuilder data, int cbData);//获取竖版身份证正反两面JPG图像base64编码字符串

        [DllImport("termb.dll")]
        static extern int getJPGCardBase64H(StringBuilder data, int cbData);//获取横版身份证正反两面JPG图像base64编码字符串

        [DllImport("termb.dll")]
        static extern int HIDVoice(int nVoice);//语音提示。。仅适用于与带HID语音设备的身份证阅读器（如ID200）

        [DllImport("termb.dll")]
        static extern int IC_SetDevNum(int iPort, StringBuilder data, int cbdata);//设置发卡器序列号

        [DllImport("termb.dll")]
        static extern int IC_GetDevNum(int iPort, StringBuilder data, int cbdata);//获取发卡器序列号

        [DllImport("termb.dll")]
        static extern int IC_GetDevVersion(int iPort, StringBuilder data, int cbdata);//设置发卡器序列号 

        [DllImport("termb.dll")]
        static extern int IC_WriteData(int iPort, int keyMode, int sector, int idx, StringBuilder key, StringBuilder data, int cbdata, ref int snr);//写数据

        [DllImport("termb.dll")]
        static extern int IC_ReadData(int iPort, int keyMode, int sector, int idx, StringBuilder key, StringBuilder data, int cbdata, ref int snr);//du数据

        [DllImport("termb.dll")]
        static extern int IC_GetICSnr(int iPort, ref int snr);//读IC卡物理卡号 

        [DllImport("termb.dll")]
        static extern int IC_GetIDSnr(int iPort, StringBuilder data, int cbdata);//读身份证物理卡号 

        [DllImport("termb.dll")]
        static extern int getEnName(StringBuilder data, int cbdata);//获取英文名

        [DllImport("termb.dll")]
        static extern int getCnName(StringBuilder data, int cbdata);//获取中文名 

        [DllImport("termb.dll")]
        static extern int getPassNum(StringBuilder data, int cbdata);//获取港澳台居通行证号码

        [DllImport("termb.dll")]
        static extern int getVisaTimes();//获取签发次数

        #endregion
        public string rev_servename;

        public clsAllnew()
        {


            if (HttpRuntime.Cache.Get("servename") != null)
            {
                var objCache = HttpRuntime.Cache.Get("servename");

                ConStr = System.Web.Configuration.WebConfigurationManager.AppSettings[objCache.ToString()];
                ConStrPIC = ConStr.Replace("Provider=SQLOLEDB;", "");

            }
            else
            {
                if (HttpContext.Current.Request.Cookies["MyCook"] != null)
                {
                    HttpCookie cookie1 = HttpContext.Current.Request.Cookies["MyCook"];

                    if (cookie1 != null && cookie1["servename"].ToString() != "")
                    {
                        rev_servename = cookie1["servename"].ToString();

                        // var rev_servename = HttpContext.Current.Session["servename"];
                        if (rev_servename != "" && rev_servename != null)
                        {

                            //ConStr = System.Web.Configuration.WebConfigurationManager.AppSettings[cookie1["servename"].ToString()];
                            ConStr = System.Web.Configuration.WebConfigurationManager.AppSettings[HttpUtility.UrlDecode(cookie1["servename"].ToString()).ToString()];

                            ConStrPIC = ConStr.Replace("Provider=SQLOLEDB;", "");

                        }
                    }
                }
            }

        }


        public List<clsuserinfo> findUser(string findtext)
        {
            //string strSelect = "select * from JNOrder_User where name='" + findtext + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(findtext, ConStr);
            List<clsuserinfo> ClaimReport_Server = new List<clsuserinfo>();

            while (reader.Read())
            {
                clsuserinfo item = new clsuserinfo();
                //if (reader.GetValue(0) != null && Convert.ToString(reader.GetValue(0)) != "")
                //    item.Order_id = reader.GetString(0);
                //if (reader.GetValue(1) != null && Convert.ToString(reader.GetValue(1)) != "")
                //    item.name = reader.GetString(1);
                //if (reader.GetValue(2) != null && Convert.ToString(reader.GetValue(2)) != "")
                //    item.password = reader.GetString(2);
                //if (reader.GetValue(3) != null && Convert.ToString(reader.GetValue(3)) != "")
                //    item.Createdate = reader.GetString(3);
                //if (reader.GetValue(4) != null && Convert.ToString(reader.GetValue(4)) != "")
                //    item.Btype = reader.GetString(4);
                //if (reader.GetValue(5) != null && Convert.ToString(reader.GetValue(5)) != "")
                //    item.denglushijian = reader.GetString(5);
                //if (reader.GetValue(6) != null && Convert.ToString(reader.GetValue(6)) != "")
                //    item.jigoudaima = reader.GetString(6);


                //if (reader.GetValue(7) != null && Convert.ToString(reader.GetValue(7)) != "")
                //    item.userTime = reader.GetString(7);

                //if (reader.GetValue(8) != null && Convert.ToString(reader.GetValue(8)) != "")
                //    item.AdminIS = reader.GetString(8);

                //if (reader.GetValue(9) != null && Convert.ToString(reader.GetValue(9)) != "")
                //    item.Input_Date = reader.GetString(9);

                //if (reader.GetValue(10) != null && Convert.ToString(reader.GetValue(10)) != "")
                //    item.mibao = reader.GetString(10);

                //if (reader.GetValue(11) != null && Convert.ToString(reader.GetValue(11)) != "")
                //    item.xiajilist = reader.GetString(11);
                if (reader.GetValue(1) != null && Convert.ToString(reader.GetValue(1)) != "")
                    item.name = reader.GetString(7);
                if (reader.GetValue(2) != null && Convert.ToString(reader.GetValue(2)) != "")
                    item.password = reader.GetString(8);
                if (reader.GetValue(3) != null && Convert.ToString(reader.GetValue(3)) != "")
                    item.mibao = reader.GetString(9);
                if (reader.GetValue(4) != null && Convert.ToString(reader.GetValue(4)) != "")
                    item.Order_id = reader.GetString(0);
                //ClaimReport_Server.Add(item);
                if (reader.GetValue(5) != null && Convert.ToString(reader.GetValue(5)) != "")
                    item.gongsi = reader.GetString(5);
                if (reader.GetValue(reader.GetOrdinal("AdminIS")) != null && Convert.ToString(reader.GetValue(reader.GetOrdinal("AdminIS"))) != "")
                    item.AdminIS = reader.GetString(reader.GetOrdinal("AdminIS"));
                ClaimReport_Server.Add(item);
                //这里做数据处理....
            }
            return ClaimReport_Server;

        }

        private static void inputlog(string aainput)
        {
            string A_Path = AppDomain.CurrentDomain.BaseDirectory + "bin\\log.txt";
            StreamWriter sw = new StreamWriter(A_Path);
            sw.WriteLine(aainput);
            sw.Flush();
            sw.Close();
        }
        public void createUser_Server(List<clsuserinfo> AddMAPResult)
        {

            foreach (clsuserinfo item in AddMAPResult)
            {
                string sql = "";
                sql = "insert into _user(name,password,Createdate,Btype,denglushijian,jigoudaima,userTime,AdminIS,mibao) values ('" + item.name + "','" + item.password + "',N'" + item.Createdate + "','" + item.Btype + "','" + item.denglushijian + "','" + item.jigoudaima + "','" + item.userTime + "','" + item.AdminIS + "','" + item.mibao + "')";

                int isrun = MySqlHelper.ExecuteSql(sql, ConStr);


            }
            return;
        }
        public void Update_User_Server(string sql)
        {


            {

                int isrun = MySqlHelper.ExecuteSql(sql, ConStr);


            }
            return;
        }
        public List<clsuserinfo> ReadUserlistfromServer()
        {
            string conditions = "select * from _user";//成功
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(conditions, ConStr);
            List<clsuserinfo> ClaimReport_Server = new List<clsuserinfo>();

            while (reader.Read())
            {
                clsuserinfo item = new clsuserinfo();

                if (reader.GetValue(0) != null && Convert.ToString(reader.GetValue(0)) != "")
                    item.Order_id = reader.GetString(0);
                if (reader.GetValue(1) != null && Convert.ToString(reader.GetValue(1)) != "")
                    item.name = reader.GetString(1);
                if (reader.GetValue(2) != null && Convert.ToString(reader.GetValue(2)) != "")
                    item.password = reader.GetString(2);
                if (reader.GetValue(3) != null && Convert.ToString(reader.GetValue(3)) != "")
                    item.Createdate = reader.GetString(3);
                if (reader.GetValue(4) != null && Convert.ToString(reader.GetValue(4)) != "")
                    item.Btype = reader.GetString(4);
                if (reader.GetValue(5) != null && Convert.ToString(reader.GetValue(5)) != "")
                    item.denglushijian = reader.GetString(5);
                if (reader.GetValue(6) != null && Convert.ToString(reader.GetValue(6)) != "")
                    item.jigoudaima = reader.GetString(6);

                if (reader.GetValue(7) != null && Convert.ToString(reader.GetValue(7)) != "")
                    item.userTime = reader.GetString(7);

                if (reader.GetValue(8) != null && Convert.ToString(reader.GetValue(8)) != "")
                    item.AdminIS = reader.GetString(8);

                if (reader.GetValue(9) != null && Convert.ToString(reader.GetValue(9)) != "")
                    item.Input_Date = reader.GetString(9);

                if (reader.GetValue(10) != null && Convert.ToString(reader.GetValue(10)) != "")
                    item.mibao = reader.GetString(10);


                ClaimReport_Server.Add(item);

                //这里做数据处理....
            }
            return ClaimReport_Server;

        }

        public void deleteUSER(string name)
        {
            string sql2 = "delete from _user where  name='" + name + "'";
            int isrun = MySqlHelper.ExecuteSql(sql2, ConStr);

            return;

        }
        public bool changeUserpassword_Server(List<clsuserinfo> AddMAPResult)
        {
            string strSelect = "";
            foreach (clsuserinfo item in AddMAPResult)
            {
                strSelect = "UPDATE Yh_JinXiaoCun_user SET password = '" + item.password + "' WHERE _id = '" + item.Order_id + "'";

            }
            int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);
            return true;
            //创建连接对象
            //bool isok = false;
            //OleDbConnection con = new OleDbConnection(ConStr);
            //           //int isrun = MySqlHelper.ExecuteSql(sql, ConStr);
            //try
            //{
            //    if (con.State == ConnectionState.Closed)
            //        con.Open();
            //    //命令
            //    foreach (clsuserinfo item in AddMAPResult)
            //    {

            //        string sql = "";
            //        string conditions = "";
            //        if (item.password != null)
            //        {
            //            conditions += " password ='" + item.password + "'";
            //        }
            //        if (item.name != null)
            //        {
            //            conditions += " ,name ='" + item.name + "'";
            //        }
            //        //if (item.Btype != null)
            //        //{
            //        //    conditions += " ,Btype ='" + item.Btype + "'";
            //        //}
            //        //if (item.denglushijian != null)
            //        //{
            //        //    conditions += " ,denglushijian ='" + item.denglushijian + "'";
            //        //}
            //        //if (item.Createdate != null)
            //        //{
            //        //    conditions += " ,Createdate ='" + item.Createdate + "'";
            //        //}
            //        //if (item.AdminIS != null)
            //        //{
            //        //    conditions += " ,AdminIS ='" + item.AdminIS + "'";
            //        //}
            //        //if (item.jigoudaima != null)
            //        //{
            //        //    conditions += " ,jigoudaima ='" + item.jigoudaima + "'";
            //        //}
            //        //if (item.userTime != null)
            //        //{
            //        //    conditions += " ,userTime ='" + item.userTime + "'";
            //        //}
            //        if (item.mibao != null)
            //        {
            //            conditions += " ,mibao ='" + item.mibao + "'";
            //        }


            //        conditions = "update Yh_JinXiaoCun_user set  " + conditions + " where _id = " + item.Order_id + " ";
            //        sql = conditions;

            //        OleDbCommand cmd = new OleDbCommand(sql, con);
            //        cmd.ExecuteNonQuery();
            //        isok = true;

            //    }
            //    //con.Close();
            //    return isok;
            //}
            //catch (Exception ex)
            //{
            //    if (con.State == ConnectionState.Open) con.Close();
            //    if (con != null)
            //        con.Dispose();
            //    return false;

            //    throw;
            //}
            //finally { if (con.State == ConnectionState.Open) con.Close(); con.Dispose(); }

        }


        #region 读取IC卡设备
        public List<clCard_info> Read_card()
        {
            #region 假数据
            //string image64 = ImgToBase64String(@"D:\Devlop\身份证阅读器二次开发软件说明\cardv.jpg");
            ////string m_strPath = Application.StartupPath;

            ////Base64ToImage(image64).Save(m_strPath + "\\Hello.jpg");
            //Base64ToImage(image64).Save(mdbpath2_Ctirx);


            //List<clCard_info> reads1 = new List<clCard_info>();


            //clCard_info item = new clCard_info();
            //item.daima_gonghao = "d1ll";
            //item.zhengjianhaoma = "12345";
            //item.tupian = item.zhengjianhaoma;
            //item.FData = image64;

            //reads1.Add(item);
            //return reads1; 
            #endregion
            try
            {

                int AutoSearchReader = InitCommExt();
                if (AutoSearchReader > 0)
                {
                    Port = AutoSearchReader;
                    IsConnected = true;
                    //textBox_Name.Text = AutoSearchReader.ToString();

                    StringBuilder sb = new StringBuilder(cbDataSize);
                    GetSAMID(sb);
                    //  MessageBox.Show("连接身份证阅读器成功,SAM模块编号:" + sb);
                    //button_Connect.Enabled = false;
                    //button_ReadCard.Enabled = true;
                    //button_DisConnect.Enabled = true;
                    #region 读取

                    List<clCard_info> reads = button_ReadCard();



                    #endregion



                    return reads;
                }
                else
                {
                    MessageBox.Show("检查是否正确连接设备");
                }
            }
            catch (Exception exx)
            {

                throw;
            }
            return null;
        }
        private List<clCard_info> button_ReadCard()
        {

            List<clCard_info> resulits = new List<clCard_info>();

            //卡认证
            int FindCard = Authenticate();

            int rs = Read_Content(1);

            if (rs != 1 && rs != 2 && rs != 3)
            {

                return null;
            }
            clCard_info item = new clCard_info();

            //读卡成功
            //姓名
            StringBuilder sb = new StringBuilder(cbDataSize);
            getName(sb, cbDataSize);
            item.mingcheng = sb.ToString();

            //民族/国家
            getNation(sb, cbDataSize);
            item.minzu = sb.ToString();

            //性别 
            getSex(sb, cbDataSize);
            item.xingbie = sb.ToString();

            //出生 
            getBirthdate(sb, cbDataSize);
            item.chushengriqi = sb.ToString();
            // sb.ToString().Substring(0, 4) - sb.ToString().Substring(4, 2) - sb.ToString().Substring(6, 2);

            //地址 
            getAddress(sb, cbDataSize);
            string ad = sb.ToString();
            item.jiatingzhuzhi = ad;

            //号码 
            getIDNum(sb, cbDataSize);
            item.zhengjianhaoma = sb.ToString();

            //机关 
            getIssue(sb, cbDataSize);
            //textBox_Issue.Text = sb.ToString();

            //有效期 
            getEffectedDate(sb, cbDataSize);
            string aa = sb.ToString();
            getExpiredDate(sb, cbDataSize);
            item.zhengjianyouxiao = aa + sb.ToString();

            //通行证号  
            getPassNum(sb, cbDataSize);
            //textBox_PassNum.Text = sb.ToString();

            //签证次数  
            //textBox_VisaTimes.Text = "" + getVisaTimes();

            //英文名 
            getEnName(sb, cbDataSize);
            //textBox_EnName.Text = sb.ToString();

            //中文名  
            getCnName(sb, cbDataSize);
            //textBox_CnName.Text = sb.ToString();

            //证件类型
            GetCardInfo(105, sb);
            if ("1" == sb.ToString())
            {
                //textBox_CardType.Text = "居民身份证";
            }
            else if ("3" == sb.ToString())
            {
                //textBox_CardType.Text = "港澳台居住证";
            }
            else
            {
                //textBox_CardType.Text = "外国人居住证";
            }
            //图片
            getJPGCardBase64H(sb, cbDataSize);
            item.tupian = item.zhengjianhaoma;
            item.FData = sb.ToString();

            resulits.Add(item);
            return resulits;

        }
        public string ImgToBase64String(string Imagefilename)
        {
            try
            {
                Bitmap bmp = new Bitmap(Imagefilename);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public System.Drawing.Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }

        #endregion
        #region 写入金蝶数据库

        public void createICcard_info_Server(List<clCard_info> AddMAPResult)
        {


            //创建连接对象
            bool isok = false;
            OleDbConnection con = new OleDbConnection(ConStr);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //命令
                foreach (clCard_info item in AddMAPResult)
                {

                    string sql = "";
                    sql = "insert into t_Item_3002(FNumber,FName,F_101,F_108,F_109,F_103,F_104,F_106,F_127,F_105,FItemID) values ('" + item.daima_gonghao + "','" + item.mingcheng + "',N'" + item.xingbie + "',N'" + item.minzu + "','" + item.chushengriqi + "','" + item.zhengjianleixing + "','" + item.zhengjianhaoma + "','" + item.jiatingzhuzhi + "','" + item.zhengjianyouxiao + "','" + item.jiguan + "','" + item.Order_id + "')";

                    OleDbCommand cmd = new OleDbCommand(sql, con);
                    cmd.ExecuteNonQuery();
                    isok = true;

                }
                //con.Close();
                return;
            }
            catch (Exception ex)
            {

                if (con.State == ConnectionState.Open) con.Close();
                if (con != null)
                    con.Dispose();

                HttpContext.Current.Response.Redirect("~/ErrorPage/ErrorPage.aspx?Error=" + ex.ToString());

                throw ex;
                return;

                throw;
            }
            finally { if (con.State == ConnectionState.Open) con.Close(); con.Dispose(); }
        }
        public void create_t_Item_info_Server(List<clt_Item_info> AddMAPResult)
        {


            //创建连接对象
            bool isok = false;
            OleDbConnection con = new OleDbConnection(ConStr);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //命令
                foreach (clt_Item_info item in AddMAPResult)
                {

                    string sql = "";
                    sql = "insert into t_Item(FItemID,FItemClassID,FExternID,FNumber,FParentID,FLevel,FDetail,FName,FUnUsed,FBrNo,FFullNumber,FDiff,FDeleted,FShortNumber,FFullName,FGRCommonID,FSystemType,FUseSign,FAccessory,FGrControl,FHavePicture) values ('" + item.FItemID + "','" + item.FItemClassID + "',N'" + item.FExternID + "',N'" + item.FNumber + "','" + item.FParentID + "','" + item.FLevel + "','" + item.FDetail + "','" + item.FName + "','" + item.FUnUsed + "','" + item.FBrNo + "','" + item.FFullNumber + "','" + item.FDiff + "','" + item.FDeleted + "','" + item.FShortNumber + "','" + item.FFullName + "','" + item.FGRCommonID + "','" + item.FSystemType + "','" + item.FUseSign + "','" + item.FAccessory + "','" + item.FGrControl + "','" + item.FHavePicture + "')";

                    OleDbCommand cmd = new OleDbCommand(sql, con);
                    cmd.ExecuteNonQuery();
                    isok = true;

                }
                //con.Close();
                return;
            }
            catch (Exception ex)
            {

                if (con.State == ConnectionState.Open) con.Close();
                if (con != null)
                    con.Dispose();

                HttpContext.Current.Response.Redirect("~/ErrorPage/ErrorPage.aspx?Error=" + ex.ToString());

                throw ex;
                return;

                throw;
            }
            finally { if (con.State == ConnectionState.Open) con.Close(); con.Dispose(); }
        }

        public void createPIC_info_Server(List<clCard_info> AddMAPResult)
        {
            //创建连接对象
            bool isok = false;
            OleDbConnection con = new OleDbConnection(ConStr);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //命令
                foreach (clCard_info item in AddMAPResult)
                {

                    #region 之前
                    //string sql = "";
                    //sql = "insert into t_Accessory(FTypeID,FItemID,FFileName,FData,FVersion,FSaveMode,FPage,FEntryID) values ('" + item.FTypeID + "','" + item.Order_id + "',N'" + item.tupian + "','" + item.FData + "','" + item.FVersion + "','" + item.FSaveMode + "','" + item.FPage + "','" + item.FEntryID + "')";

                    //OleDbCommand cmd = new OleDbCommand(sql, con);
                    //cmd.ExecuteNonQuery();
                    //isok = true;
                    #endregion

                    #region sql 插入图片
                    string A_Path = AppDomain.CurrentDomain.BaseDirectory + "bin\\img.txt";//记录 Status  click 和选择哪个服务器
                    //if (File.Exists(A_Path))
                    {
                        //   mdbpath2_Ctirx = Base64StringToImage(A_Path, AddMAPResult[0].FTypeID);
                    }
                    #region 读取本地记事本
                    //FileStream ifs = new FileStream(A_Path, FileMode.Open, FileAccess.Read);
                    //StreamReader sr = new StreamReader(ifs);
                    ////读取txt里面的内容
                    //String inputStr = sr.ReadToEnd();

                    //AddMAPResult[0].FTypeID = inputStr;
                    #endregion
                    //  FileStream fs = new FileStream(mdbpath2_Ctirx, FileMode.Open, FileAccess.Read);
                    //Byte[] btye2 = new byte[fs.Length];
                    byte[] btye2 = Convert.FromBase64String(AddMAPResult[0].FData);
                    //fs.Read(btye2, 0, Convert.ToInt32(fs.Length));
                    //fs.Close();
                    using (SqlConnection conn = new SqlConnection(ConStrPIC))
                    {
                        conn.Open();
                        SqlCommand cmd1 = new SqlCommand();
                        cmd1.Connection = conn;
                        cmd1.CommandText = "insert into t_Accessory(FData,FTypeID,FItemID,FFileName) values(@imgfile,@FTypeID,@FItemID,@FFileName)";
                        SqlParameter par = new SqlParameter("@imgfile", SqlDbType.Image);
                        par.Value = btye2;
                        cmd1.Parameters.Add(par);

                        SqlParameter par1 = new SqlParameter("@FTypeID", SqlDbType.Int);
                        par1.Value = 3002;
                        cmd1.Parameters.Add(par1);

                        SqlParameter par2 = new SqlParameter("@FItemID", SqlDbType.Int);
                        par2.Value = item.Order_id;
                        cmd1.Parameters.Add(par2);

                        SqlParameter par3 = new SqlParameter("@FFileName", SqlDbType.Char);
                        par3.Value = item.zhengjianhaoma;
                        cmd1.Parameters.Add(par3);
                        int t = (int)(cmd1.ExecuteNonQuery());
                        if (t > 0)
                        {
                            Console.WriteLine("插入成功");
                        }
                        conn.Close();

                    }
                    //byte[] MyData = new byte[0];
                    //using (SqlConnection conn = new SqlConnection(bew))
                    //{
                    //    conn.Open();
                    //    SqlCommand cmd2 = new SqlCommand();
                    //    cmd2.Connection = conn;
                    //    cmd2.CommandText = "select * from t_Accessory";
                    //    SqlDataReader sdr = cmd2.ExecuteReader();
                    //    sdr.Read();
                    //    MyData = (byte[])sdr["FData"];//读取第一个图片的位流
                    //    int ArraySize = MyData.GetUpperBound(0);//获得数据库中存储的位流数组的维度上限，用作读取流的上限

                    //    FileStream fs2 = new FileStream(@"c:\00.jpg", FileMode.OpenOrCreate, FileAccess.Write);
                    //    fs2.Write(MyData, 0, ArraySize);
                    //    fs2.Close();   //-- 写入到c:\00.jpg。
                    //    conn.Close();
                    //    Console.WriteLine("读取成功");//查看硬盘上的文件
                    //}
                    #endregion
                    //con.Close();
                    return;
                }
            }
            catch (Exception ex)
            {

                if (con.State == ConnectionState.Open) con.Close();
                if (con != null)
                    con.Dispose();
                HttpContext.Current.Response.Redirect("~/ErrorPage/ErrorPage.aspx?Error=" + ex.ToString());

                return;

                throw;
            }
            finally { if (con.State == ConnectionState.Open) con.Close(); con.Dispose(); }
        }
        /// <summary>
        /// base64编码的文本 转为    图片
        /// </summary>
        /// <param name="txtFileName">Base编码的txt文本路径 </param>
        private string Base64StringToImage(string txtFileName, string bodytx)
        {
            try
            {
                //FileStream ifs = new FileStream(txtFileName, FileMode.Open, FileAccess.Read);
                //StreamReader sr = new StreamReader(ifs);
                ////读取txt里面的内容
                //String inputStr = sr.ReadToEnd();

                //转图片 
                byte[] bt = Convert.FromBase64String(bodytx);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(bt);
                Bitmap bmp = new Bitmap(stream);

                string fileName = txtFileName.Substring(0, txtFileName.IndexOf("."));

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                //存储到服务器 上 
                bmp.Save(fileName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                //bmp.Save(fileName + ".bmp", ImageFormat.Bmp);
                //bmp.Save(fileName + ".gif", ImageFormat.Gif);
                //bmp.Save(fileName + ".png", ImageFormat.Png);
                stream.Close();
                //sr.Close();
                //ifs.Close();
                return fileName + ".jpg";
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("~/ErrorPage/ErrorPage.aspx?Error=" + ex.ToString());

                //("Base64StringToImage 转换失败\nException：" + ex.Message);
            }
            return "";

        }

        public List<clCard_info> Readt_PICServer(string conditions)
        {

            OleDbConnection aConnection = new OleDbConnection(ConStr);

            List<clCard_info> ClaimReport_Server = new List<clCard_info>();
            if (aConnection.State == ConnectionState.Closed)
                aConnection.Open();

            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(conditions, aConnection);
            OleDbCommandBuilder mybuilder = new OleDbCommandBuilder(myDataAdapter);
            DataSet ds = new DataSet();
            myDataAdapter.Fill(ds, "t_Accessory");
            foreach (DataRow reader in ds.Tables["t_Accessory"].Rows)
            {
                clCard_info item = new clCard_info();

                if (reader["FTypeID"].ToString() != "")
                    item.FTypeID = reader["FTypeID"].ToString();
                if (reader["FItemID"].ToString() != "")
                    item.Order_id = reader["FItemID"].ToString();
                if (reader["FData"].ToString() != "")
                {
                    item.FData = reader["FData"].ToString();
                    item.imagebytes = (byte[])reader["FData"];
                }
                if (reader["FFileName"].ToString() != "")
                    item.tupian = reader["FFileName"].ToString();
                if (reader["FVersion"].ToString() != "")
                    item.FVersion = reader["FVersion"].ToString();

                if (reader["FSaveMode"].ToString() != "")
                    item.FSaveMode = reader["FSaveMode"].ToString();
                if (reader["FPage"].ToString() != "")
                    item.FPage = reader["FPage"].ToString();
                if (reader["FEntryID"].ToString() != "")
                    item.zhengjianleixing = reader["FEntryID"].ToString();



                ClaimReport_Server.Add(item);

                //这里做数据处理....
            }
            return ClaimReport_Server;

        }

        public List<clCard_info> Readt_ItemServer(string conditions)
        {

            try
            {
                OleDbConnection aConnection = new OleDbConnection(ConStr);

                List<clCard_info> ClaimReport_Server = new List<clCard_info>();
                if (aConnection.State == ConnectionState.Closed)
                    aConnection.Open();

                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(conditions, aConnection);
                OleDbCommandBuilder mybuilder = new OleDbCommandBuilder(myDataAdapter);
                DataSet ds = new DataSet();
                myDataAdapter.Fill(ds, "t_Item_3002");
                foreach (DataRow reader in ds.Tables["t_Item_3002"].Rows)
                {
                    clCard_info item = new clCard_info();

                    if (reader["FItemID"].ToString() != "")
                        item.Order_id = reader["FItemID"].ToString();
                    if (reader["FNumber"].ToString() != "")
                        item.daima_gonghao = reader["FNumber"].ToString();
                    if (reader["FName"].ToString() != "")
                        item.mingcheng = reader["FName"].ToString();
                    //if (reader["FName"].ToString() != "")
                    //    item.quanming = reader["FName"].ToString();
                    if (reader["F_101"].ToString() != "")
                        item.xingbie = reader["F_101"].ToString();

                    if (reader["F_108"].ToString() != "")
                        item.minzu = reader["F_108"].ToString();
                    if (reader["F_109"].ToString() != "")
                    {
                        item.chushengriqi = reader["F_109"].ToString();
                        item.chushengriqi = clsCommHelp.objToDateTime1(reader["F_109"].ToString());

                    }
                    if (reader["F_103"].ToString() != "")
                        item.zhengjianleixing = reader["F_103"].ToString();

                    if (reader["F_104"].ToString() != "")
                        item.zhengjianhaoma = reader["F_104"].ToString();

                    if (reader["F_106"].ToString() != "")
                        item.jiatingzhuzhi = reader["F_106"].ToString();


                    if (reader["F_127"].ToString() != "")
                    {
                        item.zhengjianyouxiao = reader["F_127"].ToString();
                        item.zhengjianyouxiao = clsCommHelp.objToDateTime1(reader["F_127"].ToString());

                    }
                    if (reader["F_105"].ToString() != "")
                        item.jiguan = reader["F_105"].ToString();


                    //if (reader["shenheren"].ToString() != "")
                    //    item.shenheren = reader["shenheren"].ToString();


                    //if (reader["fujian"].ToString() != "")
                    //    item.fujian = reader["fujian"].ToString();

                    //if (reader["tupian"].ToString() != "")
                    item.tupian = item.zhengjianhaoma.ToString();


                    ClaimReport_Server.Add(item);

                    //这里做数据处理....
                }
                return ClaimReport_Server;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect("~/ErrorPage/ErrorPage.aspx?Error=" + "网络访问较慢或网络不通无法访问 ：" + ex.ToString());

                // inputlog(ex.Message + "//" + ex.Source + "//" + ex.StackTrace);

                throw ex;
            }

        }
        public bool deleteCard(string sql2)
        {
            OleDbConnection con = new OleDbConnection(ConStr);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                OleDbCommand cmd = new OleDbCommand(sql2, con);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {

                throw ex;

                if (con.State == ConnectionState.Open) con.Close();
                if (con != null)
                    con.Dispose();

                HttpContext.Current.Response.Redirect("~/ErrorPage/ErrorPage.aspx?Error=" + ex.ToString());

                return false;

                throw;
            }
            finally { if (con.State == ConnectionState.Open) con.Close(); con.Dispose(); }

        }

        public bool changeCardServer(List<clCard_info> AddMAPResult)
        {
            //创建连接对象
            bool isok = false;
            OleDbConnection con = new OleDbConnection(ConStr);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //命令
                foreach (clCard_info item in AddMAPResult)
                {

                    string sql = "";
                    string conditions = "";
                    if (item.daima_gonghao != null)
                    {
                        conditions += " FNumber ='" + item.daima_gonghao + "'";
                    }
                    if (item.mingcheng != null)
                    {
                        conditions += " ,FName ='" + item.mingcheng + "'";
                    }
                    if (item.xingbie != null)
                    {
                        conditions += " ,F_101 ='" + item.xingbie + "'";
                    }
                    if (item.minzu != null)
                    {
                        conditions += " ,F_108 ='" + item.minzu + "'";
                    }
                    if (item.chushengriqi != null)
                    {
                        conditions += " ,F_109 ='" + item.chushengriqi + "'";
                    }
                    if (item.zhengjianleixing != null)
                    {
                        conditions += " ,F_103 ='" + item.zhengjianleixing + "'";
                    }
                    if (item.zhengjianhaoma != null)
                    {
                        conditions += " ,F_104 ='" + item.zhengjianhaoma + "'";
                    }
                    if (item.jiatingzhuzhi != null)
                    {
                        conditions += " ,F_106 ='" + item.jiatingzhuzhi + "'";
                    }
                    if (item.zhengjianyouxiao != null)
                    {
                        conditions += " ,F_127 ='" + item.zhengjianyouxiao + "'";
                    }
                    if (item.jiguan != null)
                    {
                        conditions += " ,F_105 ='" + item.jiguan + "'";
                    }

                    conditions = "update t_Item_3002 set  " + conditions + " where FItemID = " + item.Order_id + " ";
                    sql = conditions;

                    OleDbCommand cmd = new OleDbCommand(sql, con);
                    cmd.ExecuteNonQuery();
                    isok = true;

                }
                //con.Close();
                return isok;
            }
            catch (Exception ex)
            {

                if (con.State == ConnectionState.Open) con.Close();
                if (con != null)
                    con.Dispose();
                HttpContext.Current.Response.Redirect("~/ErrorPage/ErrorPage.aspx?Error=" + ex.ToString());

                return false;

                throw;
            }
            finally { if (con.State == ConnectionState.Open) con.Close(); con.Dispose(); }

        }

        #endregion

        public List<clt_POS_info> HandingChargeKEY(string ZFCEPath)
        {


            List<clt_POS_info> KEYResult = GetHandingChargenfo(ZFCEPath);



            return KEYResult;

        }
        public List<clt_POS_info> GetHandingChargenfo(string Alist)
        {

            List<clt_POS_info> MAPPINGResult = new List<clt_POS_info>();
            try
            {
                List<clt_POS_info> WANGYINResult = new List<clt_POS_info>();
                System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Microsoft.Office.Interop.Excel.Application excelApp;
                {
                    string path = Alist;
                    excelApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook analyWK = excelApp.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing,
                        "htc", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                    Microsoft.Office.Interop.Excel.Worksheet WS = (Microsoft.Office.Interop.Excel.Worksheet)analyWK.Worksheets[1];
                    Microsoft.Office.Interop.Excel.Range rng;
                    // rng = WS.Range[WS.Cells[1, 1], WS.Cells[WS.UsedRange.Rows.Count, 16]];
                    //rng = WS.get_Range(WS.Cells[1, 1], WS.Cells[WS.UsedRange.Rows.Count, 30]);
                    rng = WS.Range[WS.Cells[2, 1], WS.Cells[WS.UsedRange.Rows.Count, 30]];
                    int rowCount = WS.UsedRange.Rows.Count - 1;
                    object[,] o = new object[1, 1];
                    o = (object[,])rng.Value2;
                    int wscount = analyWK.Worksheets.Count;
                    clsCommHelp.CloseExcel(excelApp, analyWK);

                    for (int i = 1; i <= rowCount; i++)
                    {
                        clt_POS_info temp = new clt_POS_info();

                        #region 基础信息

                        temp.shangpinbianhao = "";
                        if (o[i, 1] != null)
                            temp.shangpinbianhao = o[i, 1].ToString().Trim();

                        temp.zhucemingcheng = "";
                        if (o[i, 2] != null)
                            temp.zhucemingcheng = o[i, 2].ToString().Trim();


                        temp.jingyingmingcheng = "";
                        if (o[i, 3] != null)
                            temp.jingyingmingcheng = o[i, 3].ToString().Trim();

                        //卖场代码

                        temp.suoshujigou = "";
                        if (o[i, 4] != null)
                            temp.suoshujigou = o[i, 4].ToString().Trim();
                        if (temp.suoshujigou == null || temp.suoshujigou == "")
                            continue;

                        temp.jiaoyileixing = "";
                        if (o[i, 5] != null)
                            temp.jiaoyileixing = o[i, 5].ToString().Trim();

                        temp.jiaoyizhuangtai = "";
                        if (o[i, 6] != null)
                            temp.jiaoyizhuangtai = o[i, 6].ToString().Trim();
                        temp.jiaoyijine = "";
                        if (o[i, 7] != null)
                            temp.jiaoyijine = o[i, 7].ToString().Trim();

                        temp.jiaoyishouxufei = "";
                        if (o[i, 8] != null)
                            temp.jiaoyishouxufei = o[i, 8].ToString().Trim();

                        //卖场名称
                        temp.jiaoyifujiashouxufei = "";
                        if (o[i, 9] != null)
                            temp.jiaoyifujiashouxufei = o[i, 9].ToString().Trim();

                        temp.jiaoyishijian = "";
                        if (o[i, 10] != null)
                            temp.jiaoyishijian = o[i, 10].ToString().Trim();

                        temp.jiansuocankaohao = "";
                        if (o[i, 11] != null)
                            temp.jiansuocankaohao = o[i, 11].ToString().Trim();
                        //

                        temp.Input_Date = DateTime.Now.ToString("yyyy/MM/dd");

                        #endregion
                        MAPPINGResult.Add(temp);
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: 01032" + ex);
                return null;

                throw;
            }
            return MAPPINGResult;

        }
        public void createPOS_Server(List<clt_POS_info> AddMAPResult)
        {

            try
            {
                foreach (clt_POS_info item in AddMAPResult)
                {
                    string sql = "";
                    sql = "insert into pos_detail(shangpinbianhao,zhucemingcheng,jingyingmingcheng,suoshujigou,jiaoyileixing,jiaoyizhuangtai,jiaoyijine,jiaoyishouxufei,jiaoyifujiashouxufei,jiaoyishijian,jiansuocankaohao,Input_Date,mark1,mark2,mark3,mark4,mark5) values ('" + item.shangpinbianhao + "','" + item.zhucemingcheng + "','" + item.jingyingmingcheng + "','" + item.suoshujigou + "','" + item.jiaoyileixing + "','" + item.jiaoyizhuangtai + "','" + item.jiaoyijine + "','" + item.jiaoyishouxufei + "','" + item.jiaoyifujiashouxufei + "','" + item.jiaoyishijian + "','" + item.jiansuocankaohao + "','" + item.Input_Date + "','" + item.mark1 + "','" + item.mark2 + "','" + item.mark3 + "','" + item.mark4 + "','" + item.mark5 + "')";

                    int isrun = MySqlHelper.ExecuteSql(sql, ConStr);


                }
                return;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<clt_POS_info> ReadPOSServer(string conditions)
        {

            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(conditions, ConStr);
            List<clt_POS_info> ClaimReport_Server = new List<clt_POS_info>();

            while (reader.Read())
            {
                clt_POS_info item = new clt_POS_info();

                if (reader.GetValue(0) != null && Convert.ToString(reader.GetValue(0)) != "")
                    item.Order_id = reader.GetString(0);
                if (reader.GetValue(1) != null && Convert.ToString(reader.GetValue(1)) != "")
                    item.shangpinbianhao = reader.GetString(1);
                if (reader.GetValue(2) != null && Convert.ToString(reader.GetValue(2)) != "")
                    item.zhucemingcheng = reader.GetString(2);
                if (reader.GetValue(3) != null && Convert.ToString(reader.GetValue(3)) != "")
                    item.jingyingmingcheng = reader.GetString(3);
                if (reader.GetValue(4) != null && Convert.ToString(reader.GetValue(4)) != "")
                    item.suoshujigou = reader.GetString(4);
                if (reader.GetValue(5) != null && Convert.ToString(reader.GetValue(5)) != "")
                    item.jiaoyileixing = reader.GetString(5);
                if (reader.GetValue(6) != null && Convert.ToString(reader.GetValue(6)) != "")
                    item.jiaoyizhuangtai = reader.GetString(6);

                if (reader.GetValue(7) != null && Convert.ToString(reader.GetValue(7)) != "")
                    item.jiaoyijine = reader.GetString(7);

                if (reader.GetValue(8) != null && Convert.ToString(reader.GetValue(8)) != "")
                    item.jiaoyishouxufei = reader.GetString(8);

                if (reader.GetValue(9) != null && Convert.ToString(reader.GetValue(9)) != "")
                    item.jiaoyifujiashouxufei = reader.GetString(9);

                if (reader.GetValue(10) != null && Convert.ToString(reader.GetValue(10)) != "")
                    item.jiaoyishijian = reader.GetString(10);

                if (reader.GetValue(11) != null && Convert.ToString(reader.GetValue(11)) != "")
                    item.jiansuocankaohao = reader.GetString(11);
                if (reader.GetValue(12) != null && Convert.ToString(reader.GetValue(12)) != "")
                    item.Input_Date = reader.GetString(12);
                if (reader.GetValue(13) != null && Convert.ToString(reader.GetValue(13)) != "")
                    item.mark1 = reader.GetString(13);
                if (reader.GetValue(14) != null && Convert.ToString(reader.GetValue(14)) != "")
                    item.mark2 = reader.GetString(14);
                if (reader.GetValue(15) != null && Convert.ToString(reader.GetValue(15)) != "")
                    item.mark3 = reader.GetString(15);
                if (reader.GetValue(16) != null && Convert.ToString(reader.GetValue(16)) != "")
                    item.mark4 = reader.GetString(16);
                if (reader.GetValue(17) != null && Convert.ToString(reader.GetValue(17)) != "")
                    item.mark5 = reader.GetString(17);

                ClaimReport_Server.Add(item);

                //这里做数据处理....
            }
            return ClaimReport_Server;

        }
        public void addinput_ku(List<ming_xi_info> mxif,string kcid)
        {
            try
            {
                string sql = "";

                bool isworng = false;
                //List<ku_cun> kclist = SelectKucun();
                foreach (ming_xi_info item in mxif)
                {
                    //ku_cun kc = new ku_cun();
                    sql = "insert into Yh_JinXiaoCun_mingxi(_openid,cpid,cpjj,cplb,cpname,cpsj,cpsl,finduser,gongsi,mxtype,orderid,shijian,sp_dm,shou_h,zh_name,gs_name) values ('" + item.Openid + "','" + item.Cpid + "','" + item.Cpjg + "','" + item.Cplb + "','" + item.Cpname + "','" + item.Cpsj + "','" + item.Cpsl + "','" + item.Finduser + "','" + item.Gongsi + "','" + item.Mxtype + "','" + item.Orderid + "','" + item.Shijian + "','" + item.sp_dm + "','" + item.shou_h + "','" + item.zh_name + "','" + item.gs_name + "')";
                    int isrun = MySqlHelper.ExecuteSql(sql, ConStr);
                    if (kcid.Equals(string.Empty))
                    {
                        //kc = kclist.Find(fn => fn.Name.Equals(item.Cpname) && fn.Jin_jia.Equals(item.Cpjg) && fn.Lei_bie.Equals(item.Cplb) && Convert.ToInt32(fn.Shu_liang) > 0 && fn.Gong_huo.Equals(item.shou_h) && fn.Dan_hao.Equals(item.Orderid) && fn.Ri_qi.Equals(item.Shijian) && fn.zh_name.Equals(item.zh_name) && fn.gs_name.Equals(item.gs_name)) ;
                        //if (kc.Id != null && !kc.Id.Equals(string.Empty))
                        //{
                        //    sql = "update yh_jinxiaocun_kucun set shu_liang = shu_liang+  where ID=";
                        //}
                        //else 
                        //{
                            sql = "insert into yh_jinxiaocun_kucun(name,jin_jia,lei_bie,mx_lb,shu_liang,ri_qi,gong_huo,dan_hao,zh_name,gs_name) values ('" + item.Cpname + "','" + item.Cpjg + "','" + item.Cplb + "','" + item.Mxtype + "','" + item.Cpsl + "','" + item.Shijian + "','" + item.shou_h + "','" + item.Orderid + "','" + item.zh_name + "','" + item.gs_name + "')";
                        //}
                    }
                    else
                    {
                        sql = "insert into yh_jinxiaocun_kucun(name,jin_jia,lei_bie,mx_lb,shu_liang,ri_qi,gong_huo,dan_hao,zh_name,gs_name) values ('" + item.Cpname + "','" + item.Cpsj + "','" + item.Cplb + "','" + item.Mxtype + "','" + item.Cpsl + "','" + item.Shijian + "','" + item.shou_h + "','" + item.Orderid + "','" + item.zh_name + "','" + item.gs_name + "')";
                    }
                    int isrun2 = MySqlHelper.ExecuteSql(sql, ConStr);
                    if (isrun == 0 && isrun2 == 0)
                    {
                        if (MessageBox.Show("写入数据有误请是否继续下一条 , 继续 ?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {

                        }
                        else
                        {

                            isworng = true;
                            break;
                        }

                    }
                }


                if (isworng == true)
                {

                    //继续进行


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入数据有误请重新维护" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;


                throw;
            }
        }

        public void buindess_addinput_ku(List<ming_xi_info> mxif)
        {
            try
            {
                string sql = "";

                bool isworng = false;

                foreach (ming_xi_info item in mxif)
                {

                    sql = "insert into Yh_JinXiaoCun_mingxi(_openid,cpid,cpjj,cplb,cpname,cpsj,cpsl,finduser,gongsi,mxtype,orderid,shijian,sp_dm,shou_h,zh_name,gs_name) values ('" + item.Openid + "','" + item.Cpid + "','" + item.Cpjg + "','" + item.Cplb + "','" + item.Cpname + "','" + item.Cpsj + "','" + item.Cpsl + "','" + item.Finduser + "','" + item.Gongsi + "','" + item.Mxtype + "','" + item.Orderid + "','" + item.Shijian + "','" + item.sp_dm + "','" + item.shou_h + "','" + item.zh_name + "','" + item.gs_name + "')";
                    int isrun = MySqlHelper.ExecuteSql(sql, ConStr);
                    sql = "insert into yh_jinxiaocun_kucun(name,jin_jia,lei_bie,mx_lb,shu_liang,ri_qi,gong_huo,dan_hao,zh_name,gs_name) values ('" + item.Cpname + "','" + item.Cpjg + "','" + item.Cplb + "','入库','" + item.Cpsl + "','" + item.Shijian + "','" + item.shou_h + "','" + item.Orderid + "','" + item.zh_name + "','" + item.gs_name + "')";

                    int isrun2 = MySqlHelper.ExecuteSql(sql, ConStr);
                    if (isrun == 0 && isrun2 == 0)
                    {
                        if (MessageBox.Show("写入数据有误请是否继续下一条 , 继续 ?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {

                        }
                        else
                        {

                            isworng = true;
                            break;
                        }

                    }
                }


                if (isworng == true)
                {

                    //继续进行


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入数据有误请重新维护" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;


                throw;
            }
        }
        public void buindess_upinput_ku(List<ming_xi_info> mxif)
        {
            try
            {
                string sql = "";
                bool isworng = false;

                foreach (ming_xi_info item in mxif)
                {
                    sql = "update yh_jinxiaocun_mingxi set cpjj='" + item.Cpjg + "', _openid='" + item.Openid + "',cplb='" + item.Cplb + "',Cpname='" + item.Cpname + "',Cpsl='" + item.Cpsl + "',Cpsj='" + item.Cpsj + "',shou_h='" + item.shou_h + "',Orderid='" + item.Orderid + "',Shijian='" + item.Shijian + "' where _id ='" + item.Id + "' ";
                    int isrun = MySqlHelper.ExecuteSql(sql, ConStr);
                    //ku_cun k = selectkcALL().Find(fn => fn.Name.Equals(item.Cpname) && fn.Jin_jia.Equals(item.Cpjg) && fn.Lei_bie.Equals(item.Cplb) && fn.Gong_huo.Equals(item.shou_h) && fn.Dan_hao.Equals(item.Orderid) && fn.Ri_qi.Equals(item.Shijian) && fn.zh_name.Equals(item.zh_name) && fn.gs_name.Equals(item.gs_name));
                    //if (k != null)
                    //{
                    //    int jg = 0;
                    //    int chuanj = 0;
                    //    if (item.Mxtype.Equals("入库"))
                    //    {
                    //        if (Convert.ToInt32(item.Cpjg) > Convert.ToInt32(k.Jin_jia))
                    //        {
                    //            jg = Convert.ToInt32(k.Jin_jia) - (Convert.ToInt32(k.Jin_jia) - Convert.ToInt32(item.Cpjg));

                    //        }
                    //        else
                    //        {
                    //            jg = Convert.ToInt32(k.Jin_jia) + (Convert.ToInt32(k.Jin_jia) - Convert.ToInt32(item.Cpjg));
                    //        }
                    //    }
                    //    else 
                    //    {
                    //        if (Convert.ToInt32(item.Cpjg) > Convert.ToInt32(k.Jin_jia))
                    //        {
                    //            jg = Convert.ToInt32(k.Jin_jia) - (Convert.ToInt32(k.Jin_jia) - Convert.ToInt32(item.Cpsj));

                    //        }
                    //        else
                    //        {
                    //            jg = Convert.ToInt32(k.Jin_jia) + (Convert.ToInt32(k.Jin_jia) - Convert.ToInt32(item.Cpsj));
                    //        }
                    //    }
                    //    sql = "update yh_jinxiaocun_kucun set  ";

                    //}
                    if (isrun == 0)
                    {
                        if (MessageBox.Show("写入数据有误请是否继续下一条 , 继续 ?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            isworng = true;
                            break;
                        }

                    }
                }


                if (isworng == true)
                {

                    //继续进行


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入数据有误请重新维护" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;


                throw;
            }
        }
        public void buindess_Delinput_ku(List<ming_xi_info> mxif)
        {
            try
            {
                string sql = "";
                string selectsql = "";
                bool isworng = false;

                foreach (ming_xi_info item in mxif)
                {
                    sql = "delete  from yh_jinxiaocun_mingxi where _id = '"+item.Id+"'";
                    int isrun = MySqlHelper.ExecuteSql(sql, ConStr);
                    selectsql = "select * from yh_jinxiaocun_kucun where name ='" + item.Cpname + "' and lei_bie='" + item.Cplb + "' and mx_lb='"+item.Mxtype+"' and shu_liang='" + item.Cpsl + "' and jin_jia='" + item.Cpjg + "' and ri_qi='" + item.Shijian + "' and gong_huo='" + item.shou_h + "' AND dan_hao ='" + item.Orderid + "' and zh_name ='" + item.zh_name + "' and gs_name = '" + item.gs_name + "' ORDER by ri_qi desc";
                    //int isrun2 = MySqlHelper.ExecuteSql(sql, ConStr);
                    MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(selectsql, ConStr);
                    List<ku_cun> dellist = new List<ku_cun>();
                    while (reader.Read()) 
                    {
                        ku_cun kc = new ku_cun();
                        if (reader.GetValue(14) != null) 
                        {
                            kc.Id = reader.GetString(14);

                        }
                        dellist.Add(kc);
                    }
                    if (dellist.Count != 0)
                    {
                        string id = dellist[0].Id;
                        sql = "delete  from yh_jinxiaocun_kucun where ID = '" + id + "'";
                        int isrun1 = MySqlHelper.ExecuteSql(sql, ConStr);
                    }
                  
                    if (isrun == 0 )
                    {
                        if (MessageBox.Show("写入数据有误请是否继续下一条 , 继续 ?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            isworng = true;
                            break;
                        }

                    }
                }


                if (isworng == true)
                {

                    //继续进行


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入数据有误请重新维护" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;


                throw;
            }
        }
        public List<ming_xi_info> ru_ku_select(string rk_dh, string zh_name, string gs_name)
        {
            string strSelect = "select * from Yh_JinXiaoCun_mingxi where orderid='" + rk_dh + "' and _openid = '1' and zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<ming_xi_info> list = new List<ming_xi_info>();
            while (reader.Read())
            {
                ming_xi_info mxi = new ming_xi_info();
                mxi.Cpname = reader.GetString(5);
                mxi.sp_dm = reader.GetString(13);
                mxi.Cplb = reader.GetString(4);
                mxi.Cpsj = reader.GetString(6);
                mxi.Cpsl = reader.GetString(7);
                mxi.Cpjg = reader.GetString(3);
                list.Add(mxi);
            }
            reader.Close();
            return list;
        }

        public List<qi_chu_info> ck_sp_select(string zh_name, string gs_name)
        {
            string strSelect = "select * from Yh_JinXiaoCun_qichushu where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<qi_chu_info> list = new List<qi_chu_info>();
            while (reader.Read())
            {
                qi_chu_info qci = new qi_chu_info();
                if (reader.GetValue(0) != null && reader.GetValue(0).ToString() != "")
                    qci.Id = reader.GetString(0);
                if(reader.GetValue(2)!=null && reader.GetValue(2).ToString()!="")
                qci.Cpid = reader.GetString(2);
                if (reader.GetValue(3) != null && reader.GetValue(3).ToString() != "")
                qci.Cpjg = reader.GetString(3);
                if (reader.GetValue(4) != null && reader.GetValue(4).ToString() != "")
                qci.Cpjj = reader.GetString(4);
                if(reader.GetValue(5)!=null && reader.GetValue(5).ToString()!="")
                qci.Cplb = reader.GetString(5);
                if(reader.GetValue(6)!=null && reader.GetValue(6).ToString()!="")
                qci.Cpname = reader.GetString(6);
                if(reader.GetValue(7)!=null && reader.GetValue(7).ToString()!="")
                qci.Cpsj = reader.GetString(7);
                if(reader.GetValue(8)!=null && reader.GetValue(8).ToString()!="")
                qci.Cpsl = reader.GetString(8);
                if(reader.GetValue(9)!=null && reader.GetValue(9).ToString()!="")
                qci.Mxtype = reader.GetString(9);
                if(reader.GetValue(10)!=null && reader.GetValue(10).ToString()!="")
                qci.Shijian = reader.GetString(10);
                list.Add(qci);
            }
            reader.Close();
            return list;
        }

        public int ck_update(string cpname, string cpsl)
        {
            string strSelect = "UPDATE `Yh_JinXiaoCun_qichushu` SET cpsl = " + cpsl + " WHERE `cpname` = '" + cpname + "'";
            int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);

            return isrun;
        }

        public List<userTable> selectUser() 
        {
            List<userTable> list = new List<userTable>();
            string strSelect = "select * from Yh_JinXiaoCun_user";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            while (reader.Read())
            {
                userTable ut = new userTable();
                if (reader.GetString(reader.GetOrdinal("_id")) != null)
                {
                    ut._id = reader.GetString(reader.GetOrdinal("_id"));
                }
                if (reader.GetString(reader.GetOrdinal("AdminIS")) != null)
                {
                    ut.AdminIS = reader.GetString(reader.GetOrdinal("AdminIS"));
                }
                if (reader.GetValue(2) != null && reader.GetValue(2).ToString()!="")
                {
                    ut.Btype = reader.GetString(reader.GetOrdinal("Btype"));
                }
                if (reader.GetValue(3) != null && reader.GetValue(3).ToString() != "")
                {
                    ut.Createdate = reader.GetString(reader.GetOrdinal("Createdate"));
                }
                if (reader.GetValue(4) != null && reader.GetValue(4).ToString() != "")
                {
                    ut._openid = reader.GetString(reader.GetOrdinal("_openid"));
                }
                if (reader.GetValue(5) != null && reader.GetValue(5).ToString() != "")
                {
                    ut.gongsi = reader.GetString(reader.GetOrdinal("gongsi"));
                }
                if (reader.GetValue(6) != null && reader.GetValue(6).ToString() != "")
                {
                    ut.jigoudaima = reader.GetString(reader.GetOrdinal("jigoudaima"));
                }
                if (reader.GetValue(7) != null && reader.GetValue(7).ToString() != "")
                {
                    ut.name = reader.GetString(reader.GetOrdinal("name"));
                }
                if (reader.GetValue(8) != null && reader.GetValue(8).ToString() != "")
                {
                    ut.password = reader.GetString(reader.GetOrdinal("password"));
                }
                if (reader.GetValue(9) != null && reader.GetValue(9).ToString() != "")
                {
                    ut.mi_bao = reader.GetString(reader.GetOrdinal("mi_bao"));
                }
                list.Add(ut); 
            }
            return list;
        }

        public List<ming_xi_info> ru_ku_fenye(int yi_c, int er_c, string zh_name, string gs_name)
        {
            string strSelect = "select * from Yh_JinXiaoCun_mingxi where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "' limit " + yi_c + "," + er_c + "";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<ming_xi_info> list = new List<ming_xi_info>();
            while (reader.Read())
            {
                ming_xi_info mxi = new ming_xi_info();
                if (reader.GetValue(1) != null )
                {
                    mxi.Openid = reader.GetString(1);
                }
                if (reader.GetValue(2) != null)
                {
                    mxi.Cpid = reader.GetString(2);
                }
                if (reader.GetValue(3) != null)
                {
                    mxi.Cpjg = reader.GetString(3);
                }
                if (reader.GetValue(4) != null)
                {
                    mxi.Cplb = reader.GetString(4);
                }
                if (reader.GetValue(5) != null)
                {
                    mxi.Cpname = reader.GetString(5);

                }
                if (reader.GetValue(6) != null)
                {
                    mxi.Cpsj = reader.GetString(6);
                }
                if (reader.GetValue(7) != null)
                {
                    mxi.Cpsl = reader.GetString(7);
                }
                if (reader.GetValue(8) != null)
                {
                    mxi.Finduser = reader.GetString(8);
                }
                if (reader.GetValue(9) != null)
                {
                    mxi.Gongsi = reader.GetString(9);
                }
                if (reader.GetValue(10) != null)
                {
                    mxi.Mxtype = reader.GetString(10);
                }
                if (reader.GetValue(11) != null)
                {
                    mxi.Orderid = reader.GetString(11);
                }
                if (reader.GetValue(12) != null)
                {
                    mxi.Shijian = reader.GetString(12);
                }
                if (reader.GetValue(13) != null)
                {
                    mxi.sp_dm = reader.GetString(13);
                }
                if (reader.GetValue(14) != null)
                {
                    mxi.shou_h = reader.GetString(14);
                }
                list.Add(mxi);
            }
            reader.Close();
            return list;
        }
        //111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111
        public List<ming_xi_info> ru_ku_select_row()
        {
            string strSelect = "SELECT _id FROM Yh_JinXiaoCun_mingxi where";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<ming_xi_info> list = new List<ming_xi_info>();
            while (reader.Read())
            {
                ming_xi_info mxi = new ming_xi_info();
                mxi.Id = reader.GetString(0);
                list.Add(mxi);
            }
            reader.Close();
            return list;
        }



        public List<qi_chu_info> ming_xi_fenye(int yi_c, int er_c, string zh_name, string gs_name)
        {
            string strSelect = "select * from Yh_JinXiaoCun_qichushu where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "' limit " + yi_c + "," + er_c + "";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<qi_chu_info> list = new List<qi_chu_info>();
            while (reader.Read())
            {
                qi_chu_info qci = new qi_chu_info();
                qci.Cpid = reader.GetString(2);
                qci.Cpjg = reader.GetString(3);
                qci.Cpjj = reader.GetString(4);
                qci.Cplb = reader.GetString(5);
                qci.Cpname = reader.GetString(6);
                qci.Cpsj = reader.GetString(7);
                qci.Cpsl = reader.GetString(8);
                qci.Mxtype = reader.GetString(9);
                qci.Shijian = reader.GetString(10);
                list.Add(qci);
            }
            reader.Close();
            return list;
        }

        public List<qi_chu_info> qi_chu_select_row(string zh_name, string gs_name)
        {
            string strSelect = "SELECT cpid FROM Yh_JinXiaoCun_qichushu where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<qi_chu_info> list = new List<qi_chu_info>();
            while (reader.Read())
            {
                qi_chu_info mxi = new qi_chu_info();
                mxi.Cpid = reader.GetString(0);
                list.Add(mxi);
            }
            reader.Close();
            return list;
        }

        public List<ming_xi_info> ming_xi_select(string zh_name, string gs_name)
        {
            string strSelect = "SELECT * FROM Yh_JinXiaoCun_mingxi where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<ming_xi_info> list = new List<ming_xi_info>();
            while (reader.Read())
            {
                ming_xi_info mxi = new ming_xi_info();
                if (reader.GetValue(0) != null && reader.GetValue(0).ToString() != "")
                {
                    mxi.Id = reader.GetString(0);
                }
                if (reader.GetValue(1) != null && reader.GetValue(1).ToString()!="")
                {
                    mxi.Openid = reader.GetString(1);
                }
                if (reader.GetValue(2) != null && reader.GetValue(2).ToString() != "")
                {
                    mxi.Cpid = reader.GetString(2);
                }
                if (reader.GetValue(3) != null && reader.GetValue(3).ToString() != "")
                {
                    mxi.Cpjg = reader.GetString(3);
                }
                if (reader.GetValue(4) != null && reader.GetValue(4).ToString() != "")
                {
                    mxi.Cplb = reader.GetString(4);
                }
                if (reader.GetValue(5) != null && reader.GetValue(5).ToString() != "")
                {
                    mxi.Cpname = reader.GetString(5);
                }
                if (reader.GetValue(6) != null && reader.GetValue(6).ToString() != "")
                {
                    mxi.Cpsj = reader.GetString(6);
                }
                if (reader.GetValue(7) != null && reader.GetValue(7).ToString() != "")
                {
                    mxi.Cpsl = reader.GetString(7);
                }
                if (reader.GetValue(8) != null && reader.GetValue(8).ToString() != "")
                {
                    mxi.Finduser = reader.GetString(8);
                }
                if (reader.GetValue(9) != null && reader.GetValue(9).ToString() != "")
                {
                    mxi.Gongsi = reader.GetString(9);
                }
                if (reader.GetValue(10) != null && reader.GetValue(10).ToString() != "")
                {
                    mxi.Mxtype = reader.GetString(10);
                }
                if (reader.GetValue(11) != null && reader.GetValue(11).ToString() != "")
                {
                    mxi.Orderid = reader.GetString(11);
                }
                if (reader.GetValue(12) != null && reader.GetValue(12).ToString() != "")
                {
                    mxi.Shijian =Convert.ToDateTime(reader.GetString(12)).ToString("yyyy-mm-dd");
                }
                if (reader.GetValue(13) != null && reader.GetValue(13).ToString() != "")
                {
                    mxi.sp_dm = reader.GetString(13);
                }
                if (reader.GetValue(14) != null && reader.GetValue(14).ToString() != "")
                {
                    mxi.shou_h = reader.GetString(14);
                }
                if (reader.GetValue(15) != null && reader.GetValue(15).ToString() != "")
                {
                    mxi.zh_name = reader.GetString(15);
                }
                if (reader.GetValue(16) != null && reader.GetValue(16).ToString() != "")
                {
                    mxi.gs_name = reader.GetString(16);
                }
                list.Add(mxi);
            }
            reader.Close();
            return list;
        }

        public void add_kucun(List<ku_cun> kc)
        {
            string strSelect = "";
            foreach (ku_cun item in kc)
            {
                strSelect = "insert into Yh_JinXiaoCun_kucun(name,shou_jia,jin_jia,lei_bie,mx_lb,shu_liang,jia_ge,ri_qi,gong_huo,shou_huo,dan_hao,tou_xiang,sp_dm,bei_zhu,ID,zh_name,gs_name) values ('" + item.Name + "','" + item.Shou_jia + "','" + item.Jin_jia + "','" + item.Lei_bie + "','" + item.Mx_lb + "','" + item.Shu_liang + "','" + item.Jia_ge + "','" + item.Ri_qi + "','" + item.Gong_huo + "','" + item.Shou_huo + "','" + item.Dan_hao + "','" + item.Tou_xiang + "','" + item.Sp_dm + "','" + item.Bei_zhu + "','" + item.Id + "','" + item.zh_name + "','" + item.gs_name + "')";
                int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);

            }
        }
        
        public List<ku_cun> seletrkc(string zh_name, string gs_name, string spdm) 
        {
            string strSelect = "select * from Yh_JinXiaoCun_kucun where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "' and sp_dm='"+spdm+"'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<ku_cun> list = new List<ku_cun>();
            while (reader.Read())
            {
                ku_cun kc = new ku_cun();
                kc.Name = reader.GetString(0);
                kc.Shu_liang = reader.GetString(5);
                kc.Id = reader.GetString(14);
                kc.Sp_dm = reader.GetString(12);
                kc.Lei_bie = reader.GetString(reader.GetOrdinal("Lei_bie"));
                list.Add(kc);
            }
            reader.Close();
            return list;
        }

        public List<ku_cun> select_kc(string zh_name, string gs_name)
        {
            string strSelect = "select * from Yh_JinXiaoCun_kucun where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<ku_cun> list = new List<ku_cun>();
            while (reader.Read())
            {
                ku_cun kc = new ku_cun();
                kc.Name = reader.GetString(0);
                kc.Shu_liang = reader.GetString(5);
                kc.Id = reader.GetString(14);
                kc.Sp_dm = reader.GetString(12);
                kc.Lei_bie = reader.GetString(reader.GetOrdinal("Lei_bie"));
                list.Add(kc);
            }
            reader.Close();
            return list;

        }

        public int update_kc(string id, string cpsl)
        {
            string strSelect = "UPDATE Yh_JinXiaoCun_kucun SET shu_liang = " + cpsl + " WHERE sp_dm = '" + id + "'";
            int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);
            return isrun;
        }

        public void add_kucun_t_mx(List<ming_xi_info> mxi)
        {
            string strSelect = "";
            foreach (ming_xi_info item in mxi)
            {
                strSelect = "insert into Yh_JinXiaoCun_mingxi(_id,_openid,cpid,cpjj,cplb,cpname,cpsj,cpsl,finduser,gongsi,mxtype,orderid,shijian,sp_dm,shou_h,zh_name,gs_name) values ('" + item.Id + "','" + item.Openid + "','" + item.Cpid + "','" + item.Cpjg + "','" + item.Cplb + "','" + item.Cpname + "','" + item.Cpsj + "','" + item.Cpsl + "','" + item.Finduser + "','" + item.Gongsi + "','" + item.Mxtype + "','" + item.Orderid + "','" + item.Shijian + "','" + item.sp_dm + "','" + item.shou_h + "','" + item.zh_name + "','" + item.gs_name + "')";
                int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);

            }
        }



        public List<ming_xi_info> ri_qi_select(string time_qs, string time_jz, string zh_name, string gs_name)
        {
            string strSelect = "SELECT * FROM Yh_JinXiaoCun_mingxi WHERE shijian >='" + time_qs + "' and shijian <= '" + time_jz + "' and zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<ming_xi_info> list = new List<ming_xi_info>();
            while (reader.Read())
            {
                ming_xi_info mxi = new ming_xi_info();
                mxi.Openid = reader.GetString(1);
                mxi.Cpid = reader.GetString(2);
                mxi.Cpjg = reader.GetString(3);
                mxi.Cplb = reader.GetString(4);
                mxi.Cpname = reader.GetString(5);
                mxi.Cpsj = reader.GetString(6);
                mxi.Cpsl = reader.GetString(7);
                mxi.Finduser = reader.GetString(8);
                mxi.Gongsi = reader.GetString(9);
                mxi.Mxtype = reader.GetString(10);
                mxi.Orderid = reader.GetString(11);
                mxi.Shijian = reader.GetString(12);
                mxi.sp_dm = reader.GetString(13);
                mxi.shou_h = reader.GetString(14);
                list.Add(mxi);
            }
            reader.Close();
            return list;

        }

        public void add_qichu(List<qi_chu_info> qci)
        {
            string strSelect = "";
            foreach (qi_chu_info item in qci)
            {
                strSelect = "insert into Yh_JinXiaoCun_qichushu(_openid,cpid,cpjg,cpjj,cplb,cpname,cpsj,cpsl,mxtype,shijian,zh_name,gs_name) values ('" + item.Openid + "','" + item.Cpid + "','" + item.Cpjg + "','" + item.Cpjj + "','" + item.Cplb + "','" + item.Cpname + "','" + item.Cpsj + "','" + item.Cpsl + "','" + item.Mxtype + "','" + item.Shijian + "','" + item.zh_name + "','" + item.gs_name + "')";
                int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);

            }
        }

        public int update_qichu(string cpid, string cpname, string cplb, string cpsj, string cpsl)
        {
            string strSelect = "UPDATE Yh_JinXiaoCun_qichushu SET cplb = '" + cplb + "',cpname = '" + cpname + "',cpsj = '" + cpsj + "' ,cpsl = '" + cpsl + "' WHERE _id = '" + cpid + "'";
            int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);
            return isrun;
        }

        public int del_qichu_ff(string cpid)
        {
            string strSelect = "DELETE FROM Yh_JinXiaoCun_qichushu WHERE id = '" + cpid + "'";
            int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);
            return isrun;
        }


        public int add_User(userTable us)
        {
            string strSelect = "";
            strSelect = "insert yh_jinxiaocun_user (_id,AdminIS,gongsi,name,password,Btype) values('"+us._id+"','"+us.AdminIS+"','"+us.gongsi+"','"+us.name+"','"+us.password+"',false)";
            int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);
            return isrun;

        }
        public int up_User(userTable us)
        {
            string strSelect = "";
            strSelect = "update yh_jinxiaocun_user set AdminIS='" + us.AdminIS + "' , gongsi='" + us.gongsi + "',name ='" + us.name + "',password = '"+us.password+"' where _id = '"+us._id+"'";
            int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);
            return isrun;

        }
        public int del_Usertable(string id, string gongsi)
        {

                string strSelect = "";
                strSelect = "delete from yh_jinxiaocun_user where _id = '" + id + "' and gongsi = '" + gongsi + "'";
                int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);
                return isrun;
     

        }

        public List<jxc_1_info> jxc_1_select()
        {
            string strSelect = "select kc.sp_dm,kc.name,kc.lei_bie,qc.cpsl,qc.cpsj from Yh_JinXiaoCun_kucun as kc ,Yh_JinXiaoCun_qichushu as qc WHERE kc.ID in (select qc._id from Yh_JinXiaoCun_qichushu)";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<jxc_1_info> list = new List<jxc_1_info>();
            while (reader.Read())
            {
                jxc_1_info j1i = new jxc_1_info();
                j1i.Sp_dm = reader.GetString(0);
                j1i.Name = reader.GetString(1);
                j1i.Lei_bie = reader.GetString(2);
                j1i.Cpsl = reader.GetString(3);
                j1i.Cpsj = reader.GetString(4);
                list.Add(j1i);
            }
            reader.Close();
            return list;

        }

        public List<jxc_2_info> jxc_2_select()
        {


            string strSelect = "select cpsl,cpsj from Yh_JinXiaoCun_mingxi WHERE _openid = '1' and _id in(select _id from Yh_JinXiaoCun_qichushu)";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<jxc_2_info> list = new List<jxc_2_info>();
            while (reader.Read())
            {
                jxc_2_info j1i = new jxc_2_info();
                j1i.Cpsl = reader.GetString(0);
                j1i.Cpsj = reader.GetString(1);
                list.Add(j1i);
            }


            reader.Close();
            return list;

        }

        public List<jxc_3_info> jxc_3_select()
        {
            string strSelect = "select cpsl,cpsj from Yh_JinXiaoCun_mingxi WHERE _openid = '2' and _id in(select _id from Yh_JinXiaoCun_qichushu)";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<jxc_3_info> list = new List<jxc_3_info>();
            while (reader.Read())
            {
                jxc_3_info j1i = new jxc_3_info();
                j1i.Cpsl = reader.GetString(0);
                j1i.Cpsj = reader.GetString(1);
                list.Add(j1i);
            }
            reader.Close();
            return list;

        }


        public List<jxc_4_info> jxc_4_select()
        {
            string strSelect = "select cpsl,cpsj from Yh_JinXiaoCun_mingxi WHERE _openid = '2' and _id in(select _id from Yh_JinXiaoCun_qichushu)";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<jxc_4_info> list = new List<jxc_4_info>();
            while (reader.Read())
            {
                jxc_4_info j1i = new jxc_4_info();
                j1i.Cpjc = "0";
                j1i.Cpsj = "0";
                j1i.Cpsj = "0";
                j1i.Cptx = "20";
                list.Add(j1i);
            }
            reader.Close();
            return list;

        }

        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        public List<jxc_z_info> jinxiaocun_hz(string zh_name, string gs_name)
        {

            List<jxc_z_info> list = new List<jxc_z_info>();
            string sqlselect = "select * from Yh_JinXiaoCun_qichushu where zh_name = '" + zh_name + "' and gs_name= '" + gs_name + "' ";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(sqlselect, ConStr);
            List<qi_chu_info> qichushu = new List<qi_chu_info>();
            List<ming_xi_info> mingxi = new List<ming_xi_info>();
            qi_chu_info qc = new qi_chu_info();
            while (reader.Read())
            {
                qc = new qi_chu_info();
                if (reader.GetValue(2) != null && reader.GetValue(2).ToString() != "")
                    qc.Cpid = reader.GetString(reader.GetOrdinal("cpid"));
                if (reader.GetValue(6) != null && reader.GetValue(6).ToString() != "")
                    qc.Cpname = reader.GetString(reader.GetOrdinal("Cpname"));
                if (reader.GetValue(5) != null && reader.GetValue(5).ToString() != "")
                    qc.Cplb = reader.GetString(reader.GetOrdinal("Cplb"));
                if (reader.GetValue(8) != null && reader.GetValue(8).ToString() != "")
                    qc.Cpsl = reader.GetString(reader.GetOrdinal("Cpsl"));
                if (reader.GetValue(7) != null && reader.GetValue(7).ToString() != "")
                    qc.Cpsj = reader.GetString(reader.GetOrdinal("Cpsj"));
                qichushu.Add(qc);
            }

            sqlselect = "select sp_dm,cpname,mxtype,cplb,sum(cpsl),sum(cpsj) from Yh_JinXiaoCun_mingxi where zh_name= '" + zh_name + "' and gs_name = '" + gs_name + "' GROUP BY cpname,mxtype ";
            MySql.Data.MySqlClient.MySqlDataReader reader2 = MySqlHelper.ExecuteReader(sqlselect, ConStr);
            ming_xi_info mx = new ming_xi_info();
            while (reader2.Read())
            {
                mx = new ming_xi_info();
                if (reader2.GetValue(0) != null && reader2.GetValue(0).ToString() != "")
                    mx.sp_dm = reader2.GetString(reader2.GetOrdinal("sp_dm"));
                if (reader2.GetValue(1) != null && reader2.GetValue(1).ToString() != "")
                    mx.Cpname = reader2.GetString(reader2.GetOrdinal("Cpname"));
                if (reader2.GetValue(2) != null && reader2.GetValue(2).ToString() != "")
                    mx.Mxtype = reader2.GetString(reader2.GetOrdinal("Mxtype"));
                if (reader2.GetValue(3) != null && reader2.GetValue(3).ToString() != "")
                    mx.Cplb = reader2.GetString(reader2.GetOrdinal("cplb"));
                if (reader2.GetValue(4) != null && reader2.GetValue(4).ToString() != "")
                    mx.Cpsj = reader2.GetString(reader2.GetOrdinal("sum(cpsj)"));
                if (reader2.GetValue(5) != null && reader2.GetValue(5).ToString() != "")
                    mx.Cpsl = reader2.GetString(reader2.GetOrdinal("sum(cpsl)"));
                mingxi.Add(mx);
            }
            jxc_z_info jxc = new jxc_z_info();
            for (int mxi = 0; mxi < mingxi.Count; mxi++)
            {
                int i = 0;
                bool pd = false;
                for (int jxci = 0; jxci < list.Count; jxci++)
                {
                    if (list[jxci].Name == mingxi[mxi].Cpname)
                    {
                        pd = true;
                        i = jxci;
                        break;
                    }
                }
                if (pd == false)
                {
                    jxc = new jxc_z_info();
                    jxc.Sp_dm = mingxi[mxi].sp_dm;
                    jxc.Name = mingxi[mxi].Cpname;
                    jxc.Lei_bie = mingxi[mxi].Cplb;
                    if (mingxi[mxi].Mxtype == "出库")
                    {
                        jxc.Cpje_2 = mingxi[mxi].Cpsj;
                        jxc.Cpsl_2 = mingxi[mxi].Cpsl;
                        if (jxc.Cpsl_2 != null && jxc.Cpje_2 != null && IsNumeric(jxc.Cpje_2) && IsNumeric(jxc.Cpsl_2))
                            jxc.Cpsj_2 = (Convert.ToInt32(jxc.Cpje_2) / Convert.ToInt32(jxc.Cpsl_2)).ToString();
                    }
                    else if (mingxi[mxi].Mxtype == "入库")
                    {
                        jxc.Cpje_1 = mingxi[mxi].Cpsj;
                        jxc.Cpsl_1 = mingxi[mxi].Cpsl;
                        if (jxc.Cpsl_1 != null && jxc.Cpje_1 != null && IsNumeric(jxc.Cpje_1) && IsNumeric(jxc.Cpsl_1))

                            jxc.Cpsj_1 = (Convert.ToInt32(jxc.Cpje_1) / Convert.ToInt32(jxc.Cpsl_1)).ToString();
                    }
                    list.Add(jxc);
                }
                else
                {
                    if (mingxi[mxi].Mxtype == "出库")
                    {
                        list[i].Cpje_2 = mingxi[mxi].Cpsj;
                        list[i].Cpsl_2 = mingxi[mxi].Cpsl;
                        if (jxc.Cpsl_2 != null && jxc.Cpje_2 != null && IsNumeric(jxc.Cpje_2) && IsNumeric(jxc.Cpsl_2))

                            list[i].Cpsj_2 = (Convert.ToInt32(list[i].Cpje_2) / Convert.ToInt32(list[i].Cpsl_2)).ToString();
                    }
                    else if (mingxi[mxi].Mxtype == "入库")
                    {
                        list[i].Cpje_1 = mingxi[mxi].Cpsj;
                        list[i].Cpsl_1 = mingxi[mxi].Cpsl;
                        if (jxc.Cpsl_1 != null && jxc.Cpje_1 != null && IsNumeric(jxc.Cpje_1) && IsNumeric(jxc.Cpsl_1))

                            list[i].Cpsj_1 = (Convert.ToInt32(list[i].Cpje_1) / Convert.ToInt32(list[i].Cpsl_1)).ToString();
                    }

                }

            }
            for (int qci = 0; qci < qichushu.Count; qci++)
            {
                bool pd = false;
                int i = 0;
                for (int li = 0; li < list.Count; li++)
                {
                    if (list[li].Name == qichushu[qci].Cpname)
                    {
                        pd = true;
                        i = li;
                        break;
                    }
                }
                if (pd == false)
                {
                    jxc = new jxc_z_info();
                    jxc.Sp_dm = qichushu[qci].Cpid;
                    jxc.Name = qichushu[qci].Cpname;
                    jxc.Lei_bie = qichushu[qci].Cplb;
                    jxc.Cpje_3 = qichushu[qci].Cpsj;
                    jxc.Cpsl_3 = qichushu[qci].Cpsl;
                    if (jxc.Cpsl_3 != null && jxc.Cpje_3 != null && IsNumeric(jxc.Cpje_3) && IsNumeric(jxc.Cpsl_3))

                        jxc.Cpsj_3 = (Convert.ToInt32(jxc.Cpje_3) / Convert.ToInt32(jxc.Cpsl_3)).ToString();
                    list.Add(jxc);
                }
                else
                {
                    list[i].Cpje_3 = qichushu[qci].Cpsj;
                    list[i].Cpsl_3 = qichushu[qci].Cpsl;
                    if (list[i].Cpje_3 != null && list[i].Cpsl_3 != null && IsNumeric(list[i].Cpje_3) && IsNumeric(list[i].Cpsl_3))

                        list[i].Cpsj_3 = (Convert.ToInt32(list[i].Cpje_3) / Convert.ToInt32(list[i].Cpsl_3)).ToString();
                }
            }
            reader.Close();
            reader2.Close();




            return list;

        }



        public List<jxc_z_info> jinxiaocun_hz_Where(string zh_name, string gs_name,string startDate,string EndDate , string name,string leibie ,string ghs,string shf)
        {

            List<jxc_z_info> list = new List<jxc_z_info>();
            string sqlselect = "select * from Yh_JinXiaoCun_qichushu where zh_name = '" + zh_name + "' and gs_name= '" + gs_name + "' ";
            if (startDate != "" && EndDate!="")
            {
                sqlselect = sqlselect + "and  shijian between '" + startDate + "' and '" + EndDate + "'";
            }
            if (name != "请选择") 
            {
                sqlselect = sqlselect + "and cpname like '%" + name + "%'";
            }
            if (leibie != "请选择")
            {
                sqlselect = sqlselect + "and cplb like '%" + leibie + "%'";
            }
            //if (ghs != "请选择")
            //{
            //    sqlselect = sqlselect + "and shou_h like '%" + ghs + "%'";
            //}
            //if (shf != "请选择")
            //{
            //    sqlselect = sqlselect + "and shou_h like '%" + shf + "%'";
            //}
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(sqlselect, ConStr);
            List<qi_chu_info> qichushu = new List<qi_chu_info>();
            List<ming_xi_info> mingxi = new List<ming_xi_info>();
            qi_chu_info qc = new qi_chu_info();
            while (reader.Read())
            {
                qc = new qi_chu_info();
                if (reader.GetValue(2) != null && reader.GetValue(2).ToString() != "")
                    qc.Cpid = reader.GetString(reader.GetOrdinal("cpid"));
                if (reader.GetValue(6) != null && reader.GetValue(6).ToString() != "")
                    qc.Cpname = reader.GetString(reader.GetOrdinal("Cpname"));
                if (reader.GetValue(5) != null && reader.GetValue(5).ToString() != "")
                    qc.Cplb = reader.GetString(reader.GetOrdinal("Cplb"));
                if (reader.GetValue(8) != null && reader.GetValue(8).ToString() != "")
                    qc.Cpsl = reader.GetString(reader.GetOrdinal("Cpsl"));
                if (reader.GetValue(7) != null && reader.GetValue(7).ToString() != "")
                    qc.Cpsj = reader.GetString(reader.GetOrdinal("Cpsj"));
                qichushu.Add(qc);
            }

            sqlselect = "select sp_dm,cpname,mxtype,cplb,sum(cpsl),sum(cpsj) from Yh_JinXiaoCun_mingxi where zh_name= '" + zh_name + "' and gs_name = '" + gs_name + "' ";
            if (startDate != "" && EndDate != "")
            {
                sqlselect = sqlselect + "and  shijian between '" + startDate + "' and '" + EndDate + "'";
            }
            if (name != "请选择")
            {
                sqlselect = sqlselect + "and cpname like '%" + name + "%'";
            }
            if (leibie != "请选择")
            {
                sqlselect = sqlselect + "and cplb like '%" + leibie + "%'";
            }
            if (ghs != "请选择")
            {
                sqlselect = sqlselect + "and shou_h like '%" + ghs + "%'";
            }
            if (shf != "请选择")
            {
                sqlselect = sqlselect + "and shou_h like '%" + shf + "%'";
            }
            sqlselect = sqlselect + " GROUP BY cpname,mxtype";
            MySql.Data.MySqlClient.MySqlDataReader reader2 = MySqlHelper.ExecuteReader(sqlselect, ConStr);
            ming_xi_info mx = new ming_xi_info();
            while (reader2.Read())
            {
                mx = new ming_xi_info();
                if (reader2.GetValue(0) != null && reader2.GetValue(0).ToString() != "")
                    mx.sp_dm = reader2.GetString(reader2.GetOrdinal("sp_dm"));
                if (reader2.GetValue(1) != null && reader2.GetValue(1).ToString() != "")
                    mx.Cpname = reader2.GetString(reader2.GetOrdinal("Cpname"));
                if (reader2.GetValue(2) != null && reader2.GetValue(2).ToString() != "")
                    mx.Mxtype = reader2.GetString(reader2.GetOrdinal("Mxtype"));
                if (reader2.GetValue(3) != null && reader2.GetValue(3).ToString() != "")
                    mx.Cplb = reader2.GetString(reader2.GetOrdinal("cplb"));
                if (reader2.GetValue(4) != null && reader2.GetValue(4).ToString() != "")
                    mx.Cpsj = reader2.GetString(reader2.GetOrdinal("sum(cpsj)"));
                if (reader2.GetValue(5) != null && reader2.GetValue(5).ToString() != "")
                    mx.Cpsl = reader2.GetString(reader2.GetOrdinal("sum(cpsl)"));
                mingxi.Add(mx);
            }
            jxc_z_info jxc = new jxc_z_info();
            for (int mxi = 0; mxi < mingxi.Count; mxi++)
            {
                int i = 0;
                bool pd = false;
                for (int jxci = 0; jxci < list.Count; jxci++)
                {
                    if (list[jxci].Name == mingxi[mxi].Cpname)
                    {
                        pd = true;
                        i = jxci;
                        break;
                    }
                }
                if (pd == false)
                {
                    jxc = new jxc_z_info();
                    jxc.Sp_dm = mingxi[mxi].sp_dm;
                    jxc.Name = mingxi[mxi].Cpname;
                    jxc.Lei_bie = mingxi[mxi].Cplb;
                    if (mingxi[mxi].Mxtype == "出库")
                    {
                        jxc.Cpje_2 = mingxi[mxi].Cpsj;
                        jxc.Cpsl_2 = mingxi[mxi].Cpsl;
                        if (jxc.Cpsl_2 != null && jxc.Cpje_2 != null && IsNumeric(jxc.Cpje_2) && IsNumeric(jxc.Cpsl_2))
                            jxc.Cpsj_2 = (Convert.ToInt32(jxc.Cpje_2) / Convert.ToInt32(jxc.Cpsl_2)).ToString();
                    }
                    else if (mingxi[mxi].Mxtype == "入库")
                    {
                        jxc.Cpje_1 = mingxi[mxi].Cpsj;
                        jxc.Cpsl_1 = mingxi[mxi].Cpsl;
                        if (jxc.Cpsl_1 != null && jxc.Cpje_1 != null && IsNumeric(jxc.Cpje_1) && IsNumeric(jxc.Cpsl_1))

                            jxc.Cpsj_1 = (Convert.ToInt32(jxc.Cpje_1) / Convert.ToInt32(jxc.Cpsl_1)).ToString();
                    }
                    list.Add(jxc);
                }
                else
                {
                    if (mingxi[mxi].Mxtype == "出库")
                    {
                        list[i].Cpje_2 = mingxi[mxi].Cpsj;
                        list[i].Cpsl_2 = mingxi[mxi].Cpsl;
                        if (jxc.Cpsl_2 != null && jxc.Cpje_2 != null && IsNumeric(jxc.Cpje_2) && IsNumeric(jxc.Cpsl_2))

                            list[i].Cpsj_2 = (Convert.ToInt32(list[i].Cpje_2) / Convert.ToInt32(list[i].Cpsl_2)).ToString();
                    }
                    else if (mingxi[mxi].Mxtype == "入库")
                    {
                        list[i].Cpje_1 = mingxi[mxi].Cpsj;
                        list[i].Cpsl_1 = mingxi[mxi].Cpsl;
                        if (jxc.Cpsl_1 != null && jxc.Cpje_1 != null && IsNumeric(jxc.Cpje_1) && IsNumeric(jxc.Cpsl_1))

                            list[i].Cpsj_1 = (Convert.ToInt32(list[i].Cpje_1) / Convert.ToInt32(list[i].Cpsl_1)).ToString();
                    }

                }

            }
            for (int qci = 0; qci < qichushu.Count; qci++)
            {
                bool pd = false;
                int i = 0;
                for (int li = 0; li < list.Count; li++)
                {
                    if (list[li].Name == qichushu[qci].Cpname)
                    {
                        pd = true;
                        i = li;
                        break;
                    }
                }
                if (pd == false)
                {
                    jxc = new jxc_z_info();
                    jxc.Sp_dm = qichushu[qci].Cpid;
                    jxc.Name = qichushu[qci].Cpname;
                    jxc.Lei_bie = qichushu[qci].Cplb;
                    jxc.Cpje_3 = qichushu[qci].Cpsj;
                    jxc.Cpsl_3 = qichushu[qci].Cpsl;
                    if (jxc.Cpsl_3 != null && jxc.Cpje_3 != null && IsNumeric(jxc.Cpje_3) && IsNumeric(jxc.Cpsl_3))

                        jxc.Cpsj_3 = (Convert.ToInt32(jxc.Cpje_3) / Convert.ToInt32(jxc.Cpsl_3)).ToString();
                    list.Add(jxc);
                }
                else
                {
                    list[i].Cpje_3 = qichushu[qci].Cpsj;
                    list[i].Cpsl_3 = qichushu[qci].Cpsl;
                    if (list[i].Cpje_3 != null && list[i].Cpsl_3 != null && IsNumeric(list[i].Cpje_3) && IsNumeric(list[i].Cpsl_3))

                        list[i].Cpsj_3 = (Convert.ToInt32(list[i].Cpje_3) / Convert.ToInt32(list[i].Cpsl_3)).ToString();
                }
            }
            reader.Close();
            reader2.Close();




            return list;

        }




        public List<jxc_z_info> jxc_z_select(string zh_name, string gs_name)
        {
            
            List<jxc_z_info> list = new List<jxc_z_info>();
            string sqlselect = "select * from Yh_JinXiaoCun_qichushu where zh_name = '"+zh_name+"' and gs_name= '"+gs_name+"' ";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(sqlselect, ConStr);
            List<qi_chu_info> qichushu = new List<qi_chu_info>();
            List<ming_xi_info> mingxi = new List<ming_xi_info>();
            qi_chu_info qc = new qi_chu_info(); 
            while (reader.Read())
            {
                qc= new qi_chu_info();
                if(reader.GetValue(2)!=null && reader.GetValue(2).ToString()!="")
                    qc.Cpid = reader.GetString(reader.GetOrdinal("cpid"));
                if (reader.GetValue(6) != null && reader.GetValue(6).ToString() != "")
                    qc.Cpname = reader.GetString(reader.GetOrdinal("Cpname"));
                if (reader.GetValue(5) != null && reader.GetValue(5).ToString() != "")
                    qc.Cplb = reader.GetString(reader.GetOrdinal("Cplb"));
                if (reader.GetValue(8) != null && reader.GetValue(8).ToString() != "")
                    qc.Cpsl = reader.GetString(reader.GetOrdinal("Cpsl"));
                if (reader.GetValue(7) != null && reader.GetValue(7).ToString() != "")
                    qc.Cpsj = reader.GetString(reader.GetOrdinal("Cpsj"));
                qichushu.Add(qc);
            }

            sqlselect = "select sp_dm,cpname,mxtype,cplb,sum(cpsl),sum(cpsj) from Yh_JinXiaoCun_mingxi where zh_name= '" + zh_name + "' and gs_name = '" + gs_name + "' GROUP BY sp_dm,mxtype ";
            MySql.Data.MySqlClient.MySqlDataReader reader2 = MySqlHelper.ExecuteReader(sqlselect, ConStr);
            ming_xi_info mx = new ming_xi_info();
            while (reader2.Read()) 
            {
                mx = new ming_xi_info();
                if (reader2.GetValue(0) != null && reader2.GetValue(0).ToString() != "")
                mx.sp_dm = reader2.GetString(reader2.GetOrdinal("sp_dm"));
                if (reader2.GetValue(1) != null && reader2.GetValue(1).ToString() != "")
                mx.Cpname = reader2.GetString(reader2.GetOrdinal("Cpname"));
                if (reader2.GetValue(2) != null && reader2.GetValue(2).ToString() != "")
                mx.Mxtype = reader2.GetString(reader2.GetOrdinal("Mxtype"));
                if (reader2.GetValue(3) != null && reader2.GetValue(3).ToString() != "")
                mx.Cplb = reader2.GetString(reader2.GetOrdinal("cplb"));
                if (reader2.GetValue(4) != null && reader2.GetValue(4).ToString() != "")
                mx.Cpsj = reader2.GetString(reader2.GetOrdinal("sum(cpsj)"));
                if (reader2.GetValue(5) != null && reader2.GetValue(5).ToString() != "")
                mx.Cpsl = reader2.GetString(reader2.GetOrdinal("sum(cpsl)"));
                mingxi.Add(mx);
            }
            jxc_z_info jxc = new jxc_z_info();
            for (int mxi = 0; mxi < mingxi.Count; mxi++ ) 
            {
                int i = 0;
                bool pd = false;
                for (int jxci = 0; jxci < list.Count; jxci++) 
                {
                    if (list[jxci].Sp_dm == mingxi[mxi].sp_dm)
                    {
                        pd = true;
                        i = jxci;
                        break;
                    }
                }
                if (pd == false)
                {
                    jxc = new jxc_z_info();
                    jxc.Sp_dm = mingxi[mxi].sp_dm;
                    jxc.Name = mingxi[mxi].Cpname;
                    jxc.Lei_bie = mingxi[mxi].Cplb;
                    if (mingxi[mxi].Mxtype == "出库")
                    {
                        jxc.Cpje_2 = mingxi[mxi].Cpsj;
                        jxc.Cpsl_2 = mingxi[mxi].Cpsl;
                        if (jxc.Cpsl_2!=null && jxc.Cpje_2 !=null &&  IsNumeric(jxc.Cpje_2) && IsNumeric(jxc.Cpsl_2))
                        jxc.Cpsj_2 = (Convert.ToInt32(jxc.Cpje_2) / Convert.ToInt32(jxc.Cpsl_2)).ToString();
                    }
                    else if (mingxi[mxi].Mxtype == "入库")
                    {
                        jxc.Cpje_1 = mingxi[mxi].Cpsj;
                        jxc.Cpsl_1 = mingxi[mxi].Cpsl;
                        if (jxc.Cpsl_1 != null && jxc.Cpje_1 != null && IsNumeric(jxc.Cpje_1) && IsNumeric(jxc.Cpsl_1))

                        jxc.Cpsj_1 = (Convert.ToInt32(jxc.Cpje_1) / Convert.ToInt32(jxc.Cpsl_1)).ToString();
                    }
                    list.Add(jxc);
                }
                else 
                {
                    if (mingxi[mxi].Mxtype == "出库")
                    {
                        list[i].Cpje_2 = mingxi[mxi].Cpsj;
                        list[i].Cpsl_2 = mingxi[mxi].Cpsl;
                        if (jxc.Cpsl_2 != null && jxc.Cpje_2 != null && IsNumeric(jxc.Cpje_2) && IsNumeric(jxc.Cpsl_2))

                        list[i].Cpsj_2 = (Convert.ToInt32(list[i].Cpje_2) / Convert.ToInt32(list[i].Cpsl_2)).ToString();
                    }
                    else if (mingxi[mxi].Mxtype == "入库")
                    {
                        list[i].Cpje_1 = mingxi[mxi].Cpsj;
                        list[i].Cpsl_1 = mingxi[mxi].Cpsl;
                        if (jxc.Cpsl_1 != null && jxc.Cpje_1 != null && IsNumeric(jxc.Cpje_1) && IsNumeric(jxc.Cpsl_1))

                        list[i].Cpsj_1 = (Convert.ToInt32(list[i].Cpje_1) / Convert.ToInt32(list[i].Cpsl_1)).ToString();
                    }
                    
                }
                
            }
            for (int qci = 0; qci < qichushu.Count; qci++) 
            {
                bool pd=false;
                int i=0;
                for (int li = 0; li < list.Count; li++) 
                {
                    if (list[li].Sp_dm == qichushu[qci].Cpid)
                    {
                        pd = true;
                        i = li;
                        break;
                    }
                }
                if (pd == false)
                {
                    jxc = new jxc_z_info();
                    jxc.Sp_dm = qichushu[qci].Cpid;
                    jxc.Name = qichushu[qci].Cpname;
                    jxc.Lei_bie = qichushu[qci].Cplb;
                    jxc.Cpje_3 = qichushu[qci].Cpsj;
                    jxc.Cpsl_3 = qichushu[qci].Cpsl;
                    if (jxc.Cpsl_3 != null && jxc.Cpje_3 != null && IsNumeric(jxc.Cpje_3) && IsNumeric(jxc.Cpsl_3))

                    jxc.Cpsj_3 = (Convert.ToInt32(jxc.Cpje_3) / Convert.ToInt32(jxc.Cpsl_3)).ToString();
                    list.Add(jxc);
                }
                else 
                {
                    list[i].Cpje_3 = qichushu[qci].Cpsj;
                    list[i].Cpsl_3 = qichushu[qci].Cpsl;
                    if (list[i].Cpje_3 != null && list[i].Cpsl_3 != null && IsNumeric(list[i].Cpje_3) && IsNumeric(list[i].Cpsl_3))

                    list[i].Cpsj_3 = (Convert.ToInt32(list[i].Cpje_3) / Convert.ToInt32(list[i].Cpsl_3)).ToString();
                }
            }
                reader.Close();
            reader2.Close();




            return list;

        }


        public List<jxc_z_info> jxc_z_select_where(string time_qs, string time_jz, string zh_name, string gs_name,string spdm , string spmc )
        {
            List<jxc_z_info> list = new List<jxc_z_info>();
            string sqlselect = "select * from Yh_JinXiaoCun_qichushu where zh_name = '" + zh_name + "' and gs_name= '" + gs_name + "' ";
            if (spdm != "") 
            {
                sqlselect = sqlselect +" and cpid='" + spdm+"'";
            }
            if (spmc != "") 
            {
                sqlselect = sqlselect + " and cpname='" + spmc+"'";
            }

            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(sqlselect, ConStr);
            List<qi_chu_info> qichushu = new List<qi_chu_info>();
            List<ming_xi_info> mingxi = new List<ming_xi_info>();
            qi_chu_info qc = new qi_chu_info();
            while (reader.Read())
            {
                qc = new qi_chu_info();
                qc.Cpid = reader.GetString(reader.GetOrdinal("cpid"));
                qc.Cpname = reader.GetString(reader.GetOrdinal("Cpname"));
                qc.Cplb = reader.GetString(reader.GetOrdinal("Cplb"));
                qc.Cpsl = reader.GetString(reader.GetOrdinal("Cpsl"));
                qc.Cpsj = reader.GetString(reader.GetOrdinal("Cpsj"));
                qichushu.Add(qc);
            }

            sqlselect = "select sp_dm,cpname,mxtype,cplb,sum(cpsl),sum(cpsj) from Yh_JinXiaoCun_mingxi where zh_name= '"+zh_name+"' and gs_name = '"+gs_name+"'";
            if (spdm != "")
            {
                sqlselect = sqlselect + " and sp_dm='" + spdm + "'";
            }
            if (spmc != "")
            {
                sqlselect = sqlselect + " and cpname='" + spmc + "'";
            }
            if (time_jz != "") 
            {
                sqlselect = sqlselect + "and  shijian between '" + time_qs + "' and '" + time_jz + "'";
            }
            sqlselect = sqlselect + " GROUP BY sp_dm,mxtype ";
            MySql.Data.MySqlClient.MySqlDataReader reader2 = MySqlHelper.ExecuteReader(sqlselect, ConStr);
            ming_xi_info mx = new ming_xi_info();
            while (reader2.Read())
            {
                mx = new ming_xi_info();
                mx.sp_dm = reader2.GetString(reader2.GetOrdinal("sp_dm"));
                mx.Cpname = reader2.GetString(reader2.GetOrdinal("Cpname"));
                mx.Mxtype = reader2.GetString(reader2.GetOrdinal("Mxtype"));
                mx.Cplb = reader2.GetString(reader2.GetOrdinal("Cpname"));
                mx.Cpsj = reader2.GetString(reader2.GetOrdinal("sum(cpsj)"));
                mx.Cpsl = reader2.GetString(reader2.GetOrdinal("sum(cpsl)"));
                mingxi.Add(mx);
            }
            jxc_z_info jxc = new jxc_z_info();
            for (int mxi = 0; mxi < mingxi.Count; mxi++)
            {
                int i = 0;
                bool pd = false;
                for (int jxci = 0; jxci < list.Count; jxci++)
                {
                    if (list[jxci].Sp_dm == mingxi[mxi].sp_dm)
                    {
                        pd = true;
                        i = jxci;
                        break;
                    }
                }
                if (pd == false)
                {
                    jxc = new jxc_z_info();
                    jxc.Sp_dm = mingxi[mxi].sp_dm;
                    jxc.Name = mingxi[mxi].Cpname;
                    jxc.Lei_bie = mingxi[mxi].Cplb;
                    if (mingxi[mxi].Mxtype == "出库")
                    {
                        jxc.Cpje_2 = mingxi[mxi].Cpsj;
                        jxc.Cpsl_2 = mingxi[mxi].Cpsl;
                        jxc.Cpsj_2 = (Convert.ToInt32(jxc.Cpje_2) / Convert.ToInt32(jxc.Cpsl_2)).ToString();
                    }
                    else if (mingxi[mxi].Mxtype == "入库")
                    {
                        jxc.Cpje_1 = mingxi[mxi].Cpsj;
                        jxc.Cpsl_1 = mingxi[mxi].Cpsl;
                        jxc.Cpsj_1 = (Convert.ToInt32(jxc.Cpje_1) / Convert.ToInt32(jxc.Cpsl_1)).ToString();
                    }
                    list.Add(jxc);
                }
                else
                {
                    if (mingxi[mxi].Mxtype == "出库")
                    {
                        list[i].Cpje_2 = mingxi[mxi].Cpsj;
                        list[i].Cpsl_2 = mingxi[mxi].Cpsl;
                        list[i].Cpsj_2 = (Convert.ToInt32(list[i].Cpje_2) / Convert.ToInt32(list[i].Cpsl_2)).ToString();
                    }
                    else if (mingxi[mxi].Mxtype == "入库")
                    {
                        list[i].Cpje_1 = mingxi[mxi].Cpsj;
                        list[i].Cpsl_1 = mingxi[mxi].Cpsl;
                        list[i].Cpsj_1 = (Convert.ToInt32(list[i].Cpje_1) / Convert.ToInt32(list[i].Cpsl_1)).ToString();
                    }

                }

            }
            for (int qci = 0; qci < qichushu.Count; qci++)
            {
                bool pd = false;
                int i = 0;
                for (int li = 0; li < list.Count; li++)
                {
                    if (list[li].Sp_dm == qichushu[qci].Cpid)
                    {
                        pd = true;
                        i = li;
                        break;
                    }
                }
                if (pd == false)
                {
                    jxc = new jxc_z_info();
                    jxc.Sp_dm = qichushu[qci].Cpid;
                    jxc.Name = qichushu[qci].Cpname;
                    jxc.Lei_bie = qichushu[qci].Cplb;
                    jxc.Cpje_3 = qichushu[qci].Cpsj;
                    jxc.Cpsl_3 = qichushu[qci].Cpsl;
                    jxc.Cpsj_3 = (Convert.ToInt32(jxc.Cpje_3) / Convert.ToInt32(jxc.Cpsl_3)).ToString();
                    list.Add(jxc);
                }
                else
                {
                    list[i].Cpje_3 = qichushu[qci].Cpsj;
                    list[i].Cpsl_3 = qichushu[qci].Cpsl;
                    list[i].Cpsj_3 = (Convert.ToInt32(list[i].Cpje_3) / Convert.ToInt32(list[i].Cpsl_3)).ToString();
                }
            }
            reader.Close();
            reader2.Close();
            return list;

        }

        //改这！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！

        public List<rc_ku_info> rc_ku_select(string name, string zh_name, string gs_name)
        {
            rc_ku_info rki = new rc_ku_info();
            string strSelect = "select * FROM Yh_JinXiaoCun_mingxi where zh_name ='"+zh_name+"' and gs_name='"+gs_name+"' and cpname = '"+name+"'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<rc_ku_info> list = new List<rc_ku_info>();
            while (reader.Read())
            {
                rki = new rc_ku_info();
                rki.Ri_qi = reader.GetString(reader.GetOrdinal("shijian"));
                rki.Gong_huo = reader.GetString(reader.GetOrdinal("gongsi"));
                rki.Orderid = reader.GetString(reader.GetOrdinal("Orderid"));
                rki.Sp_dm = reader.GetString(reader.GetOrdinal("Sp_dm"));
                rki.Name = reader.GetString(reader.GetOrdinal("cpname"));
                rki.Lei_bie = reader.GetString(reader.GetOrdinal("cplb"));
                if (reader.GetString(reader.GetOrdinal("mxtype")).Equals("入库"))
                {
                    rki.Shou_jia = reader.GetString(reader.GetOrdinal("cpsj"));
                    rki.Shu_liang = reader.GetString(reader.GetOrdinal("cpsl"));
                }
                else 
                {
                    rki.Shou_jia_2 = reader.GetString(reader.GetOrdinal("cpsj"));
                    rki.Shu_liang_2 = reader.GetString(reader.GetOrdinal("cpsl"));
                }
                
                
                list.Add(rki);
            }
            reader.Close();
            return list;
            ////string strSelect1 = "SELECT mx.cpsj,mx.cpsl FROM Yh_JinXiaoCun_kucun as kc INNER JOIN Yh_JinXiaoCun_mingxi as mx ON kc.ID = mx._id where kc.name = '" + name + "' and mx._openid = '2' and kc.gs_name = '" + gs_name + "' and kc.zh_name = '" + zh_name + "' and mx.gs_name = '" + gs_name + "' and mx.zh_name = '" + zh_name + "'";
            ////MySql.Data.MySqlClient.MySqlDataReader reader1 = MySqlHelper.ExecuteReader(strSelect1, ConStr);
            ////List<rc_ku_info> list2 = new List<rc_ku_info>();
            ////int i = 0;
            ////while (reader1.Read())
            ////{
            ////    rki = new rc_ku_info();
            ////    rki.Ri_qi = list[i].Ri_qi;
            ////    rki.Gong_huo = list[i].Gong_huo;
            ////    rki.Orderid = list[i].Orderid;
            ////    rki.Sp_dm = list[i].Sp_dm;
            ////    rki.Name = list[i].Name;
            ////    rki.Lei_bie = list[i].Lei_bie;
            ////    rki.Shou_jia = list[i].Shou_jia;
            ////    rki.Shu_liang = list[i].Shu_liang;

                
            ////    i++;
            ////    list2.Add(rki);
            ////}
            //reader1.Close();
            //return list2;

        }
        public List<rc_ku_info> rc_ku_r_select(string name, string zh_name, string gs_name)
        {
            rc_ku_info rki = new rc_ku_info();
            string strSelect = "SELECT kc.ri_qi,kc.gong_huo,mx.orderid,kc.sp_dm,kc.name,kc.lei_bie,kc.shou_jia,kc.shu_liang FROM Yh_JinXiaoCun_kucun as kc INNER JOIN Yh_JinXiaoCun_mingxi as mx ON kc.ID = mx._id where kc.name = '" + name + "' and mx._openid = '1' and kc.gs_name = '" + gs_name + "' and kc.zh_name = '" + zh_name + "' and mx.gs_name = '" + gs_name + "' and mx.zh_name = '" + zh_name + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<rc_ku_info> list = new List<rc_ku_info>();
            while (reader.Read())
            {
                rki = new rc_ku_info();
                rki.Ri_qi = reader.GetString(0);
                rki.Gong_huo = reader.GetString(1);
                rki.Orderid = reader.GetString(2);
                rki.Sp_dm = reader.GetString(3);
                rki.Name = reader.GetString(4);
                rki.Lei_bie = reader.GetString(5);
                rki.Shou_jia = reader.GetString(6);
                rki.Shu_liang = reader.GetString(7);
                list.Add(rki);
            }
            reader.Close();

            string strSelect1 = "SELECT mx.cpsj,mx.cpsl FROM Yh_JinXiaoCun_kucun as kc INNER JOIN Yh_JinXiaoCun_mingxi as mx ON kc.ID = mx._id where kc.name = '" + name + "' and mx._openid = '2' and kc.gs_name = '" + gs_name + "' and kc.zh_name = '" + zh_name + "' and mx.gs_name = '" + gs_name + "' and mx.zh_name = '" + zh_name + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader1 = MySqlHelper.ExecuteReader(strSelect1, ConStr);
            List<rc_ku_info> list2 = new List<rc_ku_info>();
            int i = 0;
            while (reader1.Read())
            {
                rki = new rc_ku_info();
                rki.Ri_qi = list[i].Ri_qi;
                rki.Gong_huo = list[i].Gong_huo;
                rki.Orderid = list[i].Orderid;
                rki.Sp_dm = list[i].Sp_dm;
                rki.Name = list[i].Name;
                rki.Lei_bie = list[i].Lei_bie;
                rki.Shou_jia = list[i].Shou_jia;
                rki.Shu_liang = list[i].Shu_liang;

                rki.Shou_jia_2 = reader1.GetString(0);
                rki.Shu_liang_2 = reader1.GetString(1);
                i++;
                list2.Add(rki);
            }
            reader1.Close();
            return list2;

        }

        public List<rc_ku_info> rc_ku_c_select(string name)
        {
            string strSelect = "SELECT kc.shou_jia,kc.shu_liang FROM Yh_JinXiaoCun_kucun as kc INNER JOIN Yh_JinXiaoCun_mingxi as mx ON kc.ID = mx._id where kc.name = '" + name + "' and mx._openid = '2' ";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<rc_ku_info> list = new List<rc_ku_info>();
            while (reader.Read())
            {
                rc_ku_info rki = new rc_ku_info();
                rki.Shou_jia = reader.GetString(0);
                rki.Shu_liang = reader.GetString(1);
                list.Add(rki);
            }
            reader.Close();
            return list;

        }

        public List<rc_ku_info> rc_ku_xl_select(string zh_name, string gs_name)
        {
            string strSelect = "select name FROM Yh_JinXiaoCun_kucun where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "' ";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<rc_ku_info> list = new List<rc_ku_info>();
            while (reader.Read())
            {
                rc_ku_info rki = new rc_ku_info();

                rki.Name = reader.GetString(0);

                list.Add(rki);
            }
            reader.Close();
            return list;

        }

        public List<rc_ku_info> kh_mx_xl_select(string zh_name, string gs_name)
        {
            string strSelect = "select shou_huo FROM Yh_JinXiaoCun_kucun where zh_name = '" + zh_name + "'and gs_name = " + gs_name + " GROUP  BY shou_huo";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<rc_ku_info> list = new List<rc_ku_info>();
            while (reader.Read())
            {
                rc_ku_info rki = new rc_ku_info();

                rki.Gong_huo = reader.GetString(0);

                list.Add(rki);
            }
            reader.Close();
            return list;

        }



        public List<rc_ku_info> rc_ku_kh_select(string shou_huo, string zh_name, string gs_name)
        {
            string strSelect = "SELECT kc.ri_qi,kc.gong_huo,mx.orderid,kc.sp_dm,kc.name,kc.lei_bie,kc.shou_jia,kc.shu_liang FROM Yh_JinXiaoCun_kucun as kc INNER JOIN Yh_JinXiaoCun_mingxi as mx ON kc.ID = mx._id where kc.shou_huo = '" + shou_huo + "' and mx._openid = '2'  and kc.gs_name = '" + gs_name + "' and kc.zh_name = '" + gs_name + "' and mx.gs_name = '" + gs_name + "' and mx.zh_name = '" + zh_name + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<rc_ku_info> list = new List<rc_ku_info>();
            while (reader.Read())
            {
                rc_ku_info rki = new rc_ku_info();
                rki.Ri_qi = reader.GetString(0);
                rki.Gong_huo = reader.GetString(1);
                rki.Orderid = reader.GetString(2);
                rki.Sp_dm = reader.GetString(3);
                rki.Name = reader.GetString(4);
                rki.Lei_bie = reader.GetString(5);
                rki.Shou_jia = reader.GetString(6);
                rki.Shu_liang = reader.GetString(7);
                list.Add(rki);
            }
            reader.Close();
            return list;

        }


        public List<zl_and_jc_info> zl_select(string zh_name, string gs_name)
        {
            string strSelect = "select * from Yh_JinXiaoCun_zhengli where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<zl_and_jc_info> list = new List<zl_and_jc_info>();
            while (reader.Read())
            {
                zl_and_jc_info zaji = new zl_and_jc_info();
                zaji.sp_dm = reader.GetString(0);
                zaji.name = reader.GetString(1);
                zaji.lei_bie = reader.GetString(2);
                zaji.dan_wei = reader.GetString(3);

                //zaji.shou_huo = reader.GetString(4);
                //zaji.Gong_huo = reader.GetString(5);
                zaji.id = reader.GetString(4);

                list.Add(zaji);
            }
            return list;
            reader.Close();
        }

        public int del_zl_ff(string id)
        {
            string strSelect = "DELETE FROM Yh_JinXiaoCun_zhengli WHERE id = '" + id + "'";
            int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);
            return isrun;
        }

        public int update_zl(string sp_dm, string name, string lei_bie, string dan_wei, string id)
        {
            string strSelect = "UPDATE Yh_JinXiaoCun_zhengli SET sp_dm = '" + sp_dm + "',name = '" + name + "',lei_bie = '" + lei_bie + "' ,dan_wei = '" + dan_wei + "' WHERE id = '" + id + "'";
            int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);
            return isrun;
        }

        public void add_zl(List<zl_and_jc_info> zaji)
        {
            string strSelect = "";
            foreach (zl_and_jc_info item in zaji)
            {
                strSelect = "insert into Yh_JinXiaoCun_zhengli(sp_dm,name,lei_bie,dan_wei,zh_name,gs_name) values ('" + item.sp_dm + "','" + item.name + "','" + item.lei_bie + "','" + item.dan_wei + "','" + item.zh_name + "','" + item.gs_name + "')";
                int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);

            }
        }

        public List<zl_and_jc_info> zl_fenye(int yi_c, int er_c, string zh_name, string gs_name)
        {
            string strSelect = "select * from Yh_JinXiaoCun_zhengli where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "' limit " + yi_c + "," + er_c + "";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<zl_and_jc_info> list = new List<zl_and_jc_info>();
            while (reader.Read())
            {
                zl_and_jc_info zaji = new zl_and_jc_info();
                zaji.sp_dm = reader.GetString(0);
                zaji.name = reader.GetString(1);
                zaji.lei_bie = reader.GetString(2);
                zaji.dan_wei = reader.GetString(3);

                //zaji.shou_huo = reader.GetString(4);
                //zaji.Gong_huo = reader.GetString(5);
                zaji.id = reader.GetString(4);

                list.Add(zaji);
            }
            return list;
            reader.Close();
        }




        public List<zl_and_jc_info> jczl_select(string zh_name, string gs_name)
        {
            string strSelect = "select * from Yh_JinXiaoCun_jichuziliao where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<zl_and_jc_info> list = new List<zl_and_jc_info>();
            while (reader.Read())
            {
                zl_and_jc_info zaji = new zl_and_jc_info();
                zaji.sp_dm = reader.GetString(0);
                zaji.name = reader.GetString(1);
                zaji.lei_bie = reader.GetString(2);
                zaji.dan_wei = reader.GetString(3);
                zaji.shou_huo = reader.GetString(4);
                zaji.Gong_huo = reader.GetString(5);
                zaji.id = reader.GetString(6);
                list.Add(zaji);
            }
            reader.Close();
            return list;

        }



        public int del_jczl_ff(string id)
        {
            string strSelect = "DELETE FROM Yh_JinXiaoCun_jichuziliao WHERE id = '" + id + "'";
            int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);
            return isrun;
        }

        public int update_jczl(string sp_dm, string name, string lei_bie, string dan_wei, string shou_huo, string gong_huo, string id)
        {
            string strSelect = "UPDATE Yh_JinXiaoCun_jichuziliao SET sp_dm = '" + sp_dm + "',name = '" + name + "',lei_bie = '" + lei_bie + "' ,dan_wei = '" + dan_wei + "' ,shou_huo = '" + shou_huo + "' ,gong_huo = '" + gong_huo + "' WHERE id = '" + id + "'";
            int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);
            return isrun;
        }

        public void add_jczl(List<zl_and_jc_info> zaji)
        {
            string strSelect = "";
            foreach (zl_and_jc_info item in zaji)
            {
                strSelect = "insert into Yh_JinXiaoCun_jichuziliao(sp_dm,name,lei_bie,dan_wei,shou_huo,gong_huo,zh_name,gs_name) values ('" + item.sp_dm + "','" + item.name + "','" + item.lei_bie + "','" + item.dan_wei + "','" + item.shou_huo + "','" + item.Gong_huo + "','" + item.zh_name + "','" + item.gs_name + "')";
                int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);

            }
        }


        public List<zl_and_jc_info> jczl_fenye(int yi_c, int er_c, string zh_name, string gs_name)
        {
            string strSelect = "select * from Yh_JinXiaoCun_jichuziliao where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "' limit " + yi_c + "," + er_c + "";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<zl_and_jc_info> list = new List<zl_and_jc_info>();
            while (reader.Read())
            {
                zl_and_jc_info zaji = new zl_and_jc_info();
                zaji.sp_dm = reader.GetString(0);
                zaji.name = reader.GetString(1);
                zaji.lei_bie = reader.GetString(2);
                zaji.dan_wei = reader.GetString(3);
                zaji.shou_huo = reader.GetString(4);
                zaji.Gong_huo = reader.GetString(5);
                zaji.id = reader.GetString(6);
                list.Add(zaji);
            }
            reader.Close();
            return list;
        }



        public List<yong_liao_set_info> yl_set_select(string zh_name, string gs_name)
        {
            string strSelect = "select * from Yh_JinXiaoCun_yongliaosheding where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<yong_liao_set_info> list = new List<yong_liao_set_info>();
            while (reader.Read())
            {
                yong_liao_set_info ylsi = new yong_liao_set_info();
                if(reader.GetValue(0)!=null && reader.GetValue(0).ToString()!="")
                ylsi.cp_name = reader.GetString(0);
                if (reader.GetValue(1) != null && reader.GetValue(1).ToString() != "")
                ylsi.yl_dm = reader.GetString(1);
                if (reader.GetValue(2) != null && reader.GetValue(2).ToString() != "")
                ylsi.yl_name = reader.GetString(2);
                if (reader.GetValue(3) != null && reader.GetValue(3).ToString() != "")
                ylsi.yl_sl = reader.GetString(3);
                if (reader.GetValue(4) != null && reader.GetValue(4).ToString() != "")
                ylsi.id = reader.GetString(4);
                if (reader.GetValue(5) != null && reader.GetValue(5).ToString() != "")
                ylsi.yl_tx = reader.GetString(5);
                list.Add(ylsi);
            }
            reader.Close();
            return list;
        }

        public int del_yl_ff(string id)
        {
            int isrun = 0; 
            if (id != null && id != "")
            {
                string strSelect = "DELETE FROM Yh_JinXiaoCun_yongliaosheding WHERE id = '" + id + "'";
                isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);

            }
           
            return isrun;
        }
        public int del_yl_ff_name(string name)
        {
            int isrun = 0;
            if (name != null && name != "")
            {
                string strSelect = "DELETE FROM Yh_JinXiaoCun_yongliaosheding WHERE name = '" + name + "'";
                isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);

            }

            return isrun;
        }

        public int update_yl(string cp_name, string yl_dm, string yl_name, string yl_sl, string id, string yl_tx)
        {
            string strSelect = "UPDATE Yh_JinXiaoCun_yongliaosheding SET name = '" + cp_name + "',sp_dm = '" + yl_dm + "',yuan_liao_name = '" + yl_name + "' ,yuan_liao_sl = '" + yl_sl + "' ,yl_tx = '" + yl_tx + "'  WHERE id = '" + id + "'";
            int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);
            return isrun;
        }

        public void add_yl(List<yong_liao_set_info> ylsi)
        {
            string strSelect = "";
            foreach (yong_liao_set_info item in ylsi)
            {
                strSelect = "insert into Yh_JinXiaoCun_yongliaosheding(name,sp_dm,yuan_liao_name,yuan_liao_sl,yl_tx,zh_name,gs_name) values ('" + item.cp_name + "','" + item.yl_dm + "','" + item.yl_name + "','" + item.yl_sl + "','" + item.yl_tx + "','" + item.zh_name + "','" + item.gs_name + "')";
                int isrun = MySqlHelper.ExecuteSql(strSelect, ConStr);

            }
        }

        public List<yong_liao_set_info> yl_fenye(int yi_c, int er_c, string zh_name, string gs_name)
        {
            string strSelect = "select * from Yh_JinXiaoCun_yongliaosheding where zh_name = '" + zh_name + "' and gs_name = '" + gs_name + "' limit " + yi_c + "," + er_c + "";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<yong_liao_set_info> list = new List<yong_liao_set_info>();
            while (reader.Read())
            {
                yong_liao_set_info ylsi = new yong_liao_set_info();
                ylsi.cp_name = reader.GetString(0);
                ylsi.yl_dm = reader.GetString(1);
                ylsi.yl_name = reader.GetString(2);
                ylsi.yl_sl = reader.GetString(3);
                ylsi.id = reader.GetString(4);
                list.Add(ylsi);
            }
            reader.Close();
            return list;
        }

        public List<zl_and_jc_info> select_jczl(string zh ,string gs) 
        {
            string sql = "select * from Yh_JinXiaoCun_jichuziliao where zh_name = '"+zh+"' and gs_name = '"+gs+"'";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(sql, ConStr);
            List<zl_and_jc_info> list = new List<zl_and_jc_info>();
            while(reader.Read())
            {
                zl_and_jc_info zl = new zl_and_jc_info();
                if (reader.GetValue(6)!=null && reader.GetValue(6).ToString()!="")
                    zl.id = reader.GetString(reader.GetOrdinal("id"));
                if (reader.GetValue(0) != null && reader.GetValue(0).ToString() != "")
                 zl.sp_dm = reader.GetString(reader.GetOrdinal("sp_dm"));
                if (reader.GetValue(1) != null && reader.GetValue(1).ToString() != "")
                    zl.name = reader.GetString(reader.GetOrdinal("name"));
                if (reader.GetValue(2) != null && reader.GetValue(2).ToString() != "")
                    zl.lei_bie = reader.GetString(reader.GetOrdinal("lei_bie"));
                if (reader.GetValue(3) != null && reader.GetValue(3).ToString() != "")
                    zl.dan_wei = reader.GetString(reader.GetOrdinal("dan_wei"));
                if (reader.GetValue(4) != null && reader.GetValue(4).ToString() != "")
                    zl.shou_huo = reader.GetString(reader.GetOrdinal("shou_huo"));
                if (reader.GetValue(5) != null && reader.GetValue(5).ToString() != "")
                    zl.Gong_huo = reader.GetString(reader.GetOrdinal("Gong_huo"));
                if (reader.GetValue(7) != null && reader.GetValue(7).ToString() != "")
                    zl.zh_name = reader.GetString(reader.GetOrdinal("zh_name"));
                if (reader.GetValue(8) != null && reader.GetValue(8).ToString() != "")
                    zl.gs_name = reader.GetString(reader.GetOrdinal("gs_name"));
                if (reader.GetValue(9) != null && reader.GetValue(9).ToString() != "")
                    zl.mark1 = reader.GetString(reader.GetOrdinal("mark1"));

                list.Add(zl);
            }
            return list;
        }
        public List<ku_cun> selectkcALL() 
        {
            List<ku_cun> list = new List<ku_cun>();
            string sql = "select * from yh_jinxiaocun_kucun";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(sql, ConStr);
            while (reader.Read())
            {
                ku_cun kc = new ku_cun();
                if (reader.GetValue(0) != null)
                    kc.Name = reader.GetString(0);
                if (reader.GetValue(1) != null)
                    kc.Shou_jia = reader.GetString(1);
                if (reader.GetValue(2) != null)
                    kc.Jin_jia = reader.GetString(2);
                if (reader.GetValue(3) != null)
                    kc.Lei_bie = reader.GetString(3);
                if (reader.GetValue(4) != null)
                    kc.Mx_lb = reader.GetString(4);
                if (reader.GetValue(5) != null)
                    kc.Shu_liang = reader.GetString(5);
                if (reader.GetValue(6) != null)
                    kc.Jia_ge = reader.GetString(6);
                if (reader.GetValue(7) != null)
                    kc.Ri_qi = reader.GetString(7);
                if (reader.GetValue(8) != null)
                    kc.Gong_huo = reader.GetString(8);
                if (reader.GetValue(9) != null)
                    kc.Shou_huo = reader.GetString(9);
                if (reader.GetValue(10) != null)
                    kc.Dan_hao = reader.GetString(10);
                if (reader.GetValue(11) != null)
                    kc.Tou_xiang = reader.GetString(11);
                if (reader.GetValue(12) != null)
                    kc.Sp_dm = reader.GetString(12);
                if (reader.GetValue(13) != null)
                    kc.Bei_zhu = reader.GetString(13);
                if (reader.GetValue(14) != null)
                    kc.Id = reader.GetString(14);
                if (reader.GetValue(15) != null)
                    kc.zh_name = reader.GetString(15);
                if (reader.GetValue(16) != null)
                    kc.gs_name = reader.GetString(16);
                list.Add(kc);
            }
            return list;
        }
        public List<ku_cun> SelectKucun() 
        {
            List<ku_cun> list = selectkcALL();
            ku_cun kcc = new ku_cun();
            List<ku_cun> endlist = new List<ku_cun>();
            int i = 0;
            foreach (ku_cun k in list) 
            {
                int index =0;
                if (endlist != null)
                {
                    kcc = endlist.Find(fn => fn.Name.Equals(k.Name) && fn.Lei_bie.Equals(k.Lei_bie));
                }
                if (kcc!=null)
                {
                    index = endlist.FindIndex(fn => fn.Name.Equals(k.Name)  && fn.Lei_bie.Equals(k.Lei_bie) );
                    if (index != -1)
                    {
                        if (k.Mx_lb.Equals("入库"))
                        {
                            endlist[index].Shu_liang = (Convert.ToInt32(endlist[index].Shu_liang) + Convert.ToInt32(k.Shu_liang)).ToString();
                        }
                        else
                        {
                            endlist[index].Shu_liang = (Convert.ToInt32(endlist[index].Shu_liang) - Convert.ToInt32(k.Shu_liang)).ToString();

                        }
                    }
                }
                else 
                {
                    if (k.Mx_lb == "出库") 
                    {
                        k.Shu_liang = (Convert.ToInt32(k.Shu_liang)-Convert.ToInt32(k.Shu_liang) * 2).ToString();
                    }
                    endlist.Add(k);
                }
                i++;
            }
            return endlist;
        }
        public List<ming_xi_info> kc_id_select()
        {
            string strSelect = "SELECT ID FROM Yh_JinXiaoCun_kucun ORDER BY ID DESC";
            MySql.Data.MySqlClient.MySqlDataReader reader = MySqlHelper.ExecuteReader(strSelect, ConStr);
            List<ming_xi_info> list = new List<ming_xi_info>();
            while (reader.Read())
            {
                ming_xi_info mxi = new ming_xi_info();
                mxi.Id = reader.GetString(0);
                list.Add(mxi);
            }
            reader.Close();
            return list;
        }

    }


}
