<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="updateqichu.aspx.cs" Inherits="Web.page.updateqichu" %>

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
	</head>

	<body>
        <script type ="text/javascript">
            function queren(Type) {

                if (!confirm("确认提交吗？")) {
                    window.event.returnValue = false;
                } else {
                    var fenlei = $("#leibie").val();
                    var name = $("#spname").val();
                    var shuliang = $("#shuliang").val();
                    var dj = $("#Danjia").val();
                    var url = "";
                    var spid = $("#spid").val();

                    url = "updatePwd.aspx?id="+spid+"&act=update&fl=" + fenlei + "&name=" + name + "&sl=" + shuliang + "&dj=" + dj ;

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
                            //window.location.go(-1);
                            //history.go(-1)
                            self.location = document.referrer;

                            //location.replace(this.href); event.returnValue = false;
                            //window.location.href="inbound.aspx"
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
		<div class="dvcontent">
			<div>
				<!--tab start-->
				<div class="tabs" style="margin: 30px;">
					<div class="hd">
						<ul>
<%--							<li class="on" style="box-sizing: initial;-webkit-box-sizing: initial;">入库记录</li>--%>
							 <li class="" style="box-sizing: initial;-webkit-box-sizing: initial;">修改期初</li>
						</ul>
					</div>
				<%--	<div class="bd">
						<ul style="display: block;padding: 20px;">
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
												        <th>商品代码</th>
												        <th>商品类别</th>
												        <th>商品单价 </th>
												        <th>商品数量</th>
                                                        <th>金额</th>
                                                        <th>供货方</th>
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
												    <td><%#Eval("sp_dm") %></td>
												    <td><%#Eval("Cplb") %></td>
												    <td><%#Eval("Cpsj") %></td>
												    <td><%#Eval("Cpsl") %></td>
                                                    <td><%#Convert.ToInt32(Eval("Cpsj")) * Convert.ToInt32(Eval("Cpsl")) %> </td>
                                                    <td><%#Eval("shou_h") %></td>
												    <td class="edit"><button onclick="btn_edit(1)"><i class="icon-edit bigger-120"></i>编辑</button></td>
												    <td class="delete"><button onclick="btn_delete(1)"><i class="icon-trash bigger-120"></i>删除</button></td>
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
			<!-- content start -->--%>
	    <div class="am-cf admin-main" style="padding-top: 0px;">
		<!-- content start -->
		<div class="admin-content">
			<div class="admin-content-body">
				
				<div class="am-g">
					<div class="am-u-sm-12 am-u-md-4 am-u-md-push-8">
						
					</div>
					<div class="am-u-sm-12 am-u-md-8 am-u-md-pull-4"
						style="padding-top: 30px;">
						<form id="Form1" class="am-form am-form-horizontal"
							action="user/addUser1Submit.action" method="post" runat="server">
							                                <input type="hidden" id="spid" runat="server"/>

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
									        </select> 
                                        </FooterTemplate>
                                    </asp:Repeater>
										<small id="fl" runat="server">分类</small>
										
								</div>


							</div>
							<div class="am-form-group">
								<label for="user-email" class="am-u-sm-3 am-form-label">
							商品名称</label>
								<div class="am-u-sm-9">
                                    <asp:Repeater ID="shangpingname" runat="server">
                                        <HeaderTemplate>
									    <select name="spname" required id="spname">
										    <option value="">请选择商品</option>
                                        </HeaderTemplate>
										<ItemTemplate>
                                            <option><%#Eval("name") %></option>
										</ItemTemplate>
										<FooterTemplate>
                                            </select> 
										</FooterTemplate>
									    
                                    </asp:Repeater>
                                    <small id="name" runat="server">商品</small>
								</div>
							</div>
							

							<div class="am-form-group">
								<label for="name" class="am-u-sm-3 am-form-label">
									数量</label>
								<div class="am-u-sm-9">
									<asp:TextBox ID="shuliang" required
										placeholder="数量" name="name" runat="server"></asp:TextBox>
										<small id="sl" runat="server">数量</small>
								</div>
							</div>
                            <div class="am-form-group">
								<label for="name" class="am-u-sm-3 am-form-label">
									单价</label>
								<div class="am-u-sm-9">
									<asp:TextBox   ID="Danjia" required
										placeholder="单价" name="name" runat="server">
                                        </asp:TextBox>
										<small id="dj" runat="server">单价</small>
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
						</form>
					</div>
				</div>
			
		</div>
		<!-- content end -->
	</div>
							<!--添加 end-->
							<!--end-->
						</ul>

					</div>
    </asp:Repeater>
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

			    var btn_edit = function (id) {
			        $.jq_Panel({
			            url: "/RawMaterialsType/EditRawMaterialsType?id=" + id,
			            title: "编辑分类",
			            dialogModal: true,
			            iframeWidth: 500,
			            iframeHeight: 400
			        });

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