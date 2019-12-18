<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="warning.aspx.cs" Inherits="Web.page.warning" %>

<!DOCTYPE html>
<html>

	<head>
		<meta charset="UTF-8">
		<title></title>
        <style type="text/css">
            .selectCss 
            {
                
            }
        </style>
		<link rel="stylesheet" />
		<link rel="stylesheet" href="css/Site.css" />
		<link rel="stylesheet" href="css/zy.all.css" />
		<link rel="stylesheet" href="css/font-awesome.min.css" />
		<link rel="stylesheet" href="css/amazeui.min.css" />
		<link rel="stylesheet" href="css/admin.css" />
        
	</head>

	<body>

        <script type ="text/javascript">
            function Delqueren(id) {

                if (!confirm("确定删除吗？")) {
                    window.event.returnValue = false;
                } else {
                    $.ajax({
                        type: "post", //要用post方式                 
                        //url: "ru_ku.aspx/selectNameAndLebie",//方法所在页面和方法名
                        url: "inbound.aspx?act=del&id=" + id,
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
            }

            function jiansuo() {
                var orderid = document.getElementById("Select_orderid").value;
                var spname = document.getElementById("Select_spname").value;
                var splb = document.getElementById("Select_splb").value;
                if (orderid != "" && spname != "" && splb != "") {
                    alert("检索值不能为空！");
                    return;
                }
                //$.ajax({
                //        type: "post", //要用post方式                 
                //        //url: "ru_ku.aspx/selectNameAndLebie",//方法所在页面和方法名
                //        url: "inbound.aspx?act=Jiansuo&orderid=" + orderid +"&spname="+spname+"&splb="+splb,
                //        //contentType: "application/json; charset=utf-8",
                //        async: false,
                //        dataType: "json",
                //        data: {},
                //        success: function (data) {
                //            //alert("yyes")
                //            //$("#sp_cplb" + row).val(data[0].leibie)
                //            //$("#sp_name" + row).val(data[0].name)
                //            //alert(data[0].endtext)
                //            //history.go(0)
                //            $("#Rep tbody").html(data);
                //        },
                //        error: function (err) {
                //            //alert(err.toString());
                //            //alert("删除失败！")
                //        }
                //    });


                window.location.href = "warning.aspx?act=Jiansuo&orderid=" + orderid + "&spname=" + spname + "&splb=" + splb;
                //window.parent.location.reload();
                return true;


            }

            function queren(Type) {

                if (!confirm("确认提交吗？")) {
                    window.event.returnValue = false;
                } else {
                    var fenlei = $("#leibie").val();
                    var name = $("#spname").val();
                    var shuliang = $("#shuliang").val();
                    var dj = $("#Danjia").val();
                    var bz = $("#ghf").val();
                    var orderid = $("#orderid").val();
                    var shijian = $("#shijian").val();
                    var url = "";
                    if (Type = "insert") {
                        url = "inbound.aspx?shijian=" + shijian + "&orderid=" + orderid + "&act=insert&fl=" + fenlei + "&name=" + name + "&sl=" + shuliang + "&dj=" + dj + "&bz=" + bz;

                    } else {
                        url = "inbound.aspx?shijian=" + shijian + "&orderid=" + orderid + "&act=update&fl=" + fenlei + "&name=" + name + "&sl=" + shuliang + "&dj=" + dj + "&bz=" + bz;
                    }
                    $.ajax({
                        type: "post", //要用post方式                 
                        //url: "ru_ku.aspx/selectNameAndLebie",//方法所在页面和方法名
                        url: url,
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
            }
        </script>

        <div class ="selectdiv" style="
    margin-left: 55px;
    margin-top: 35px;
    margin-bottom: -24px;
">
            <label>订单号&nbsp;</label><input type="text" style="width: 161px; height: 31px;display: inline-block;" ID="Select_orderid" /></input>
            <label>商品名称&nbsp;</label><input type="text" style="width: 161px;height: 31px;display: inline-block;" ID="Select_spname" /><%--</asp:TextBox>--%>
            <label>商品类别&nbsp;</label><input type="text" style="width: 161px;height: 31px;display: inline-block;"  ID="Select_splb" /><%--</asp:TextBox>--%>
            <input type="button" ID="select" onclick="jiansuo()"  value="检索" />
            <%--<asp:Button ID="select"  OnClick="select_Click" runat="server" Text="检索"/>--%>
        </div>
		<div class="dvcontent">

			<div>
				<!--tab start-->
				<div class="tabs" style="margin: 30px;">
					<div class="hd">
						<ul>
							<li class="on" style="box-sizing: initial;-webkit-box-sizing: initial;">出入库明细</li>
<%--							 <li class="" style="box-sizing: initial;-webkit-box-sizing: initial;">商品入库</li>--%>
						</ul>
					</div>
					<div class="bd">
						<ul style="display: block;padding: 0;">
							<li>
								<!--分页显示角色信息 start-->
								<div id="dv1">
                                    <asp:Repeater runat ="server" ID="Rep">
                                        <HeaderTemplate>
                                            <table class="table" id="tbRecord">
										        <thead>
											        <tr>
                                                        <th>订单编号</th>
												        <th>商品名称</th>
												        <th>明细类别</th>
												        <th>商品类别</th>
												        <th>商品单价 </th>
                                                        <th>商品售价 </th>
												        <th>商品数量</th>
<%--                                                        <th>金额</th>--%>
                                                        <th>供货方</th>
                                                        <th>日期</th>
												        <th>编辑</th>
												        <th>删除</th>
											        </tr>
										        </thead>

                                        </HeaderTemplate>
                                        <ItemTemplate>
                                              <tbody>
											    <tr>
                                                    <td><%#Eval("orderid") %></td>
												    <td><%#Eval("Cpname") %></td>
												    <td id="mxtype<%#Eval("id")%>"><%#Eval("mxtype") %></td>
												    <td><%#Eval("Cplb") %></td>
												    <td><%#Eval("Cpjg") %></td>
                                                    <td><%#Eval("Cpsj") %></td>
												    <td><%#Eval("Cpsl") %></td>
<%--                                                    <td><%#Convert.ToInt32(Eval("Cpjg") ) * Convert.ToInt32(Eval("Cpsl")) %> </td>--%>
                                                    <td><%#Eval("shou_h") %></td>
                                                    <td><%#Eval("shijian")%></td>
												    <td class="edit"><button onclick="btn_edit(<%#Eval("id") %>)"><i class="icon-edit bigger-120"></i>编辑</button></td>
												    <td class="delete"><button onclick="Delqueren(<%#Eval("id") %>)"><i class="icon-trash bigger-120"></i>删除</button></td>
											    </tr>

                                        </ItemTemplate>
                                        <FooterTemplate>
										    </tbody>
                                          </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
									
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

    <script src="js/calendar.js" type="text/javascript" language="javascript"></script>
			 <script src="js/jquery-1.7.2.min.js" type="text/javascript"></script>
              <script src="js/plugs/Jqueryplugs.js" type="text/javascript"></script>
              <script src="js/_layout.js"></script>
             <script src="js/plugs/jquery.SuperSlide.source.js"></script>
			<script>
			    var num = 1;
			    $(function () {

			        $(".tabs").slide({ trigger: "click" });

			    });


			    var btn_save = function () {
			        var pid = $("#RawMaterialsTypePageId  option:selected").val();
			        var name = $("#RawMaterialsTypeName").val();
			        var desc = $("#RawMaterialsTypeDescription").val();
			        var ramark = $("#Ramark").val();
			        $.ajax({
			            type: "post",
			            url: "/RawMaterialsType/AddRawMaterialsType",
			            data: { name: name, pid: pid, desc: desc, ramark: ramark },
			            success: function (data) {
			                if (data > 0) {
			                    $.jq_Alert({
			                        message: "添加成功",
			                        btnOktext: "确认",
			                        dialogModal: true,
			                        btnOkClick: function () {
			                            //$("#RawMaterialsTypeName").val("");
			                            //$("#RawMaterialsTypeDescription").val("");
			                            //$("#Ramark").val("");                           
			                            //page1();
			                            location.reload();

			                        }
			                    });
			                }
			            }
			        });
			        alert(t);
			    }


			    function btn_edit(id) {
			        //$.jq_Panel({
			        //    url: "/RawMaterialsType/EditRawMaterialsType?id=" + id,
			        //    title: "编辑分类",
			        //    dialogModal: true,
			        //    iframeWidth: 500,
			        //    iframeHeight: 400
			        //});
			        var type = document.getElementById("mxtype" + id).innerText;
			        if (type == "入库") {
			            window.location.href = "/page/updateRuku.aspx?id=" + id
			        } else
			        {
			            window.location.href = "/page/upchuku.aspx?id=" + id
			        }
			    }
			    var btn_delete = function (id) {
			        $.jq_Confirm({
			            message: "您确定要删除吗?",
			            btnOkClick: function () {

			            }
			        });
			    }
			</script>

		</div>
	</body>

</html>