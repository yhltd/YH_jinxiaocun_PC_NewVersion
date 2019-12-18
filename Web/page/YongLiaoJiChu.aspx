<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YongLiaoJiChu.aspx.cs" Inherits="Web.page.YongLiaoJiChu" %>

<!DOCTYPE html>
<html>

	<head>
		<meta charset="UTF-8">
		<title></title>
		<link rel="stylesheet" />
		<link rel="stylesheet" href="css/Site.css" />
		<link rel="stylesheet" href="css/zy.all.css" />
		<link rel="stylesheet" href="css/font-awesome.min.css" />
		<link rel="stylesheet" href="css/amazeui.min.css" />
		<link rel="stylesheet" href="css/admin.css" />
	<script src="js/jquery.js"type="text/javascript"> </script> 	 
	<script src="js/jquery.treetable.js" type="text/javascript"> </script>
	<link href='css/jquery.treetable.css' rel='stylesheet' type='text/css' />
	<link href='css/jquery.treetable.theme.default.css' rel='stylesheet' type='text/css' />
        <script src="js/plugs/Jqueryplugs.js" type="text/javascript"></script>
              <script src="js/_layout.js"></script>
            <script src="js/calendar.js" type="text/javascript" language="javascript"></script>
          <script src="js/plugs/jquery.SuperSlide.source.js"></script>
<script type="text/javascript">
    //$(".indenter").click(function ()
    //{
    //    $(this).parent().attr("class", "branch expanded")
    //    })
    function insertSp()
    {
        window.location.href = "InsertShangping.aspx";
    }
    function insert(name)
    {
        window.location.href = "TianJiaoZiliao.aspx?name="+name+"";
    }
    function deleteShangping(name)
    {
        $.jq_Confirm({
            message: "您确定要删除吗?",
            btnOkClick: function ()
            {
                $.ajax({
                    type: "post", //要用post方式                 
                    //url: "ru_ku.aspx/selectNameAndLebie",//方法所在页面和方法名
                    url: "YongLiaoJiChu.aspx?act=delSp&name=" + name,
                    //contentType: "application/json; charset=utf-8",
                    //async: false,
                    dataType: "json",
                    data: {},
                    success: function (data) {
                        //alert("yyes")
                        //$("#sp_cplb" + row).val(data[0].leibie)
                        //$("#sp_name" + row).val(data[0].name)
                        alert(data[0].endtext)
                        history.go(0)
                    },
                    error: function (err) {
                        //alert(err.toString());
                        //alert("删除失败！")
                    }
                });
            }
        });
    }
    function cailiao_delete(id)
    {
        $.jq_Confirm({
            message: "您确定要删除吗?",
            btnOkClick: function ()
            {

                $.ajax({
                    type: "post", //要用post方式                 
                    //url: "ru_ku.aspx/selectNameAndLebie",//方法所在页面和方法名
                    url: "YongLiaoJiChu.aspx?act=del&id=" + id,
                    //contentType: "application/json; charset=utf-8",
                    //async: false,
                    dataType: "json",
                    data: {},
                    success: function (data) {
                        //alert("yyes")
                        //$("#sp_cplb" + row).val(data[0].leibie)
                        //$("#sp_name" + row).val(data[0].name)
                        alert(data[0].endtext)
                        history.go(0)
                    },
                    error: function (err) {
                        //alert(err.toString());
                        //alert("删除失败！")
                    }
                });

                return true;
            }
        });
    }
    $(function () {
        
        var option = {
            theme: 'vsStyle',
            expandLevel: 2,
            //expandable: true,// 展示
            initialState: "expanded",//默认打开所有节点
            expandable: true,//可以选定展开,默认为false,无法点击
            expanderTemplate: "<a href='#'>&nbsp;</a>",
            beforeExpand: function ($treetable, id) {
                //判断id是否已经有了孩子节点，如果有了就不再加载，这样就可以起到缓存的作用
                if ($('.' + id, $treeTable).length) { return; }
                ////这里的html可以是ajax请求
                //var html = '<tr id="8" pId="6"><td>5.1</td><td>可以是ajax请求来的内容</td></tr>'
                //         + '<tr id="9" pId="6"><td>5.2</td><td>动态的内容</td></tr>';

                //$treetable.addChilds(html);
            },
            onSelect: function ($treetable, id) {
                window.console && console.log('onSelect:' + id);

            }

        };
        $('#example-advanced').treetable(option);


     

    });
    </script>
	</head>

	<body>

        <div class ="selectdiv" style="
    margin-left: 55px;
    margin-top: 35px;
    margin-bottom: -24px;
">
<%--            <label>订单号&nbsp;</label><input type="text" style="width: 161px; height: 31px;display: inline-block;" ID="Select_orderid" /></input>
            <label>商品名称&nbsp;</label><input type="text" style="width: 161px;height: 31px;display: inline-block;" ID="Select_spname" /><%--</asp:TextBox>--%>
            <%--<label>商品类别&nbsp;</label><input type="text" style="width: 161px;height: 31px;display: inline-block;"  ID="Select_splb" /><%--</asp:TextBox>--%>
            <%--<input type="button" ID="select" onclick="jiansuo()"  value="检索" />--%>
            <%--<asp:Button ID="select"  OnClick="select_Click" runat="server" Text="检索"/>--%>
        </div>
		<div class="dvcontent">

			<div>
				<!--tab start-->
				<div class="tabs" style="margin: 30px;">
					<div class="hd">
						<ul>
							<li class="on" style="box-sizing: initial;-webkit-box-sizing: initial;">用料设定</li>
							 <%--<li class="" style="box-sizing: initial;-webkit-box-sizing: initial;"></li>--%>
						</ul>
					</div>
					<div class="bd">
						<ul style="display: block;padding: 20px;">
							<li>
								<!--分页显示角色信息 start-->
								<div id="dv1">
                                        <table id="example-advanced" class="table table-striped table-bordered table-how">
                                            <caption>
                                              <a href="#" onclick="jQuery('#example-advanced').treetable('expandAll'); return false;">全部打开</a>
                                              <a href="#" onclick="jQuery('#example-advanced').treetable('collapseAll'); return false;">全部折叠</a>
                                              <input type="button" onclick="insertSp()" value="添加商品" style="width: 103px;/* position: relative; */float: right;" />
                                            </caption>
                                            <thead>
                                                <th>商品名称</th>
                                                <th>材料名称</th>
                                                <th>用料数量</th>
                                                <th>操作</th>
                                            </thead>
                                            <tbody>
                                               <%-- <asp:Repeater ID="fuji" runat="server">
                                                    <ItemTemplate>
                                                        <tr data-tt-id="<%#Eval("id") %>">
                                                            <td>
                                                                <span >
                                                                <%#Eval("cp_name") %>
                                                                    </span>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            <asp:Repeater ID="zi" runat="server">
                                                    <ItemTemplate>
                                                        <tr data-tt-id="<%#Eval("yl_name")+"-"+Eval("id") %>" data-tt-parent-id="<%#Eval("id") %>">
                                                            <td></td>
                                                            <td>
                                                                <%#Eval("yl_name") %>
                                                            </td>
                                                            <td>
                                                                <%#Eval("yl_sl") %>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>--%>
                                                <%
                                                    if (Session["fuji"] != null && Session["ziji"] != null)
                                                    {
                                                        List<SDZdb.yong_liao_set_info> fuji = Session["fuji"] as List<SDZdb.yong_liao_set_info>;
                                                        List<SDZdb.yong_liao_set_info> zijiAll = Session["ziji"] as List<SDZdb.yong_liao_set_info>;
                                                        for (int fi = 0; fi < fuji.Count; fi++) 
                                                        {
                                                        %><tr data-tt-id="<%=fuji[fi].id%>">
                                                            <td><span><%=fuji[fi].cp_name%></span> </td>
                                                            <td></td>
                                                            <td></td>
                                                            <td><button style="width:80px;" onclick="insert('<%=fuji[fi].cp_name%>')"><i style="width:100px;" class="icon-edit bigger-120"></i>添加材料
                                                                    <button  style="width:80px;" onclick="deleteShangping('<%=fuji[fi].cp_name%>')"><i style="width:100px;" class="icon-trash bigger-120"></i>删除
                                                                </td>
                                                          </tr>
                                                            <%
                                                            List<SDZdb.yong_liao_set_info> ziji = zijiAll.FindAll(f => f.cp_name.Equals(fuji[fi].cp_name));
                                                            for (int zi = 0; zi < ziji.Count; zi++) 
                                                            {
                                                                %><tr data-tt-id="<%=ziji[zi].yl_name+"-"+ziji[zi].id%>" data-tt-parent-id="<%=fuji[fi].id%>">
                                                                    <td></td>
                                                                    <td><%=ziji[zi].yl_name %></td>
                                                                    <td><%=ziji[zi].yl_sl%></td>
                                                                    <td><button onclick="cailiao_delete('<%=ziji[zi].id %>')"><i class="icon-trash bigger-120"></i>删除</button></td>
                                                                    </tr>
                                                                <%
                                                                  
                                                            }
                                                        }
                                                    }
                                                     %>
                                            </tbody>
                                        </table>									
								</div>
								<!--分页显示角色信息 end-->
							</li>
						</ul>
						<ul class="theme-popbod dform" style="display: none;">
								<div class="am-cf admin-main" style="padding-top: 0px;">
			<!-- content start -->
            
	<div class="am-cf admin-main" style="padding-top: 0px;">
		<!-- content start -->
        <form id="Form1" class="am-form am-form-horizontal"
action="user/addUser1Submit.action" method="post" runat="server">
		<div class="admin-content">
			<div class="admin-content-body">
				
				<div class="am-g">
					<div class="am-u-sm-12 am-u-md-4 am-u-md-push-8">
						
					</div>
					<div class="am-u-sm-12 am-u-md-8 am-u-md-pull-4"
						style="padding-top: 30px;">
						    <div class="am-form-group">
								<label for="name" class="am-u-sm-3 am-form-label">
									订单编号</label>
								<div class="am-u-sm-9">
									<asp:TextBox   ID="orderid" required
										placeholder="订单编号" name="name" runat="server">
                                        </asp:TextBox>
										<small>订单编号</small>
								</div>
							</div>
							<div class="am-form-group">
								<label for="user-email" class="am-u-sm-3 am-form-label">
								分类</label>
								<div class="am-u-sm-9">
                                    <asp:Repeater ID="fenlei" runat="server">
                                        <HeaderTemplate>
									        <select  id="leibie" name="leibie" required >
                                                <option value="">请选择分类</option>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <option >
                                                <%#Eval("lei_bie") %>
                                            </option>
                                        </ItemTemplate>
                                        <FooterTemplate>
									        </select> <small>分类</small>
                                        </FooterTemplate>
                                    </asp:Repeater>
										
										
								</div>


							</div>
							<div class="am-form-group">
								<label for="user-email" class="am-u-sm-3 am-form-label">
							商品名称</label>
								<div class="am-u-sm-9">
                                    <asp:Repeater ID="shangpingname" runat="server">
                                        <HeaderTemplate>
									    <select id="spname"  name="spname"  required>
										    <option value="">请选择商品</option>
                                        </HeaderTemplate>
										<ItemTemplate>
                                            <option ><%#Eval("name") %></option>
										</ItemTemplate>
										<FooterTemplate>
                                            </select> <small>商品</small>
										</FooterTemplate>
									    
                                    </asp:Repeater>
								</div>
							</div>
							
                            <div class="am-form-group">
								<label for="user-email" class="am-u-sm-3 am-form-label">
							供货方</label>
								<div class="am-u-sm-9">
                                    <asp:Repeater ID="Gonghuofang" runat="server">
                                        <HeaderTemplate>
									    <select name="ghf" required id="ghf">
										    <option value="">请选择供货方</option>
                                        </HeaderTemplate>
										<ItemTemplate>
                                            <option ><%#Eval("gong_huo") %></option>
										</ItemTemplate>
										<FooterTemplate>
                                            </select> <small>供货方</small>
										</FooterTemplate>
									    
                                    </asp:Repeater>
								</div>
							</div>

							<div class="am-form-group">
								<label for="name" class="am-u-sm-3 am-form-label">
									数量</label>
								<div class="am-u-sm-9">
									<asp:TextBox ID="shuliang" required
										placeholder="数量" name="name" runat="server"></asp:TextBox>
										<small>数量</small>
								</div>
							</div>
                            <div class="am-form-group">
								<label for="name" class="am-u-sm-3 am-form-label">
									单价</label>
								<div class="am-u-sm-9">
									<asp:TextBox   ID="Danjia" required
										placeholder="单价" name="name" runat="server">
                                        </asp:TextBox>
										<small>单价</small>
								</div>
							</div>
                            <div class="am-form-group">
								<label for="name" class="am-u-sm-3 am-form-label">
									日期</label>
								<div class="am-u-sm-9">
                                    <input type="date" style="width: 100%"id="shijian" name="shijian" />
                                    <small>日期</small>
								</div>
							</div>
							<%--<div class="am-form-group">
								<label for="user-intro" class="am-u-sm-3 am-form-label">
									备注</label>
								<div class="am-u-sm-9">
									<textarea class="" rows="5" id="user-intro" name="remark"
										placeholder="输入备注"></textarea>
									<small>250字以内...</small>
								</div>
							</div>--%>
							<div class="am-form-group">
								<div class="am-u-sm-9 am-u-sm-push-3">
									<input type="button" onclick="queren('insert')" class="am-btn am-btn-success" ID ="insert"  value="提交" />
								</div>
							</div>
				
					</div>
				</div>
			
		</div>
		<!-- content end -->
	</div>
							<!--添加 end-->
							<!--end-->
            			</form>

						</ul>

					</div>
                            		

				</div>
				<!--tab end-->

			</div>

		</div>
	</body>

</html>