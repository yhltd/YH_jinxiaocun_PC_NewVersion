<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InsertShangping.aspx.cs" Inherits="Web.page.InsertShangping" %>

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
            function tianjiaHang() {
                var row = $(".table tr").length
                var insertStr = "<tr>"
                                                   + " <td>"
                                                        + "<select name ='cailiaoName" + (row - 1) + "'  id='cailiaoName" + (row - 1) + "' style='width:200px'>"

                                                        + "<%if (Session["kc"] != null) 
                                                          {
                                                            List<SDZdb.ku_cun> kc = Session["kc"] as List<SDZdb.ku_cun>;
                                                            for(int ki = 0 ; ki<kc.Count;ki++)
                                                            {
                                                            %><option value='<%=kc[ki].Name %>'><%=kc[ki].Name %></option>"<%
                                                            }
                                                          } %>

                                                           + " </select></td>"
												    + "<td><input name ='sl" + (row - 1) + "' type='text' id='sl" + (row - 1) + "'  style='width:200px'/></td>"
												    + "<td class='delete'><button onclick='btn_delete(this)'><i class='icon-trash bigger-120'></i>删除</button></td>"
											    + "</tr>"
                $(".table tbody").append(insertStr);
                document.getElementById("sumrow").value = row;

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
                    var spid = $("#spid").val();
                    url = "inventory.aspx?id=" + spid + "&shijian=" + shijian + "&orderid=" + orderid + "&act=update&fl=" + fenlei + "&name=" + name + "&sl=" + shuliang + "&dj=" + dj + "&bz=" + bz;

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
                            self.location = document.referrer;
                            //history.go(-1)
                            //location.replace(this.href); event.returnValue = false;
                            //window.location.href = "inventory.aspx"
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
							 <li class="" style="box-sizing: initial;-webkit-box-sizing: initial;">添加材料</li>
						</ul>
					</div>
					<div class="bd">
						<ul style="display: block;padding: 20px;">
							<li>
								<!--分页显示角色信息 start-->
								<div id="dv1">
                                    <form id="cailiaoFrm" runat="server">
                                        <input id="sumrow" type="hidden" value="1" name ="sumrow"/>
                                        <table class="table" id="tbRecord">
                                            <caption style="
                                                margin-left: 520px;
                                                margin-bottom: 5px;
                                                margin-top: -8px;
                                            ">
                                                <span>
                                                    商品名
                                                    <asp:TextBox ID="spname" runat="server" />
                                                </span>
                                                <asp:Button ID="submit"  Text="提交" runat="server"  OnClick="submit_Click"/>
                                                <input type="button"  onclick="tianjiaHang()" value="添加行"></input>
                                            </caption>
										    <thead>
											    <tr>
                                                    <th>材料名称</th>
												    <th>用料数量</th>
												    <th>删除</th>
											    </tr>
										    </thead>
                                                  <tbody>
											        <tr>
                                                        <td>
                                                            <select name="cailiaoName0" id="cailiaoName0" style="width:200px">
                                                            
                                                            <%if (Session["kc"] != null) 
                                                              {
                                                                  List<SDZdb.ku_cun> kc = Session["kc"] as List<SDZdb.ku_cun>;
                                                                for(int ki = 0 ; ki<kc.Count;ki++)
                                                                {
                                                                %><option value="<%=kc[ki].Name %>"><%=kc[ki].Name %></option><%
                                                                }
                                                              } %>
                                                            
                                                                </select></td>
												        <td><input  type="text" name="sl0" id="sl0" style="width:200px"/></td>
												        <td class="delete"><button onclick="btn_delete(this)"><i class="icon-trash bigger-120"></i>删除</button></td>
											        </tr>
										        </tbody>
                                              </table>
                                    </form>
								</div>
								<!--分页显示角色信息 end-->
							</li>
						</ul>
						<ul class="theme-popbod dform" style="display: none;">
								<div class="am-cf admin-main" style="padding-top: 0px;">
			<!-- content start -->
<%--	    <div class="am-cf admin-main" style="padding-top: 0px;">
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
						    <div class="am-form-group">
                                <input type="hidden" id="spid" runat="server"/>
								<label for="name" class="am-u-sm-3 am-form-label">
									材料名称</label>
								<div class="am-u-sm-9">
									<asp:TextBox   ID="orderid" required
										placeholder="订单编号" name="name"  runat="server">
                                        </asp:TextBox>
										<small id="oid" runat="server">材料名称</small>
								</div>
							</div>
							
                            <div class="am-form-group">
								<label for="name" class="am-u-sm-3 am-form-label">
									用料数量</label>
								<div class="am-u-sm-9">
									<asp:TextBox ID="TextBox1" required
										placeholder="数量" name="name" runat="server"></asp:TextBox>
										<small id="Small1" runat="server">用料数量</small>
								</div>
							</div>
							
                            <div class="am-form-group">
								<label for="name" class="am-u-sm-3 am-form-label">
									日期</label>
								<div class="am-u-sm-9">
                                    <input type="date" style="width: 100%"id="shijian" name="shijian"     />
                                    <small id="time" runat="server">日期</small>
								</div>
							</div>--%>
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

			    function getRowObj(obj) {
			        var i = 0;
			        while (obj.tagName.toLowerCase() != "tr") {
			            obj = obj.parentNode;
			            if (obj.tagName.toLowerCase() == "table")
			                return null;
			        }
			        return obj;
			    }
			    var btn_delete = function (id) {
			        $.jq_Confirm({
			            message: "您确定要删除吗?",
			            btnOkClick: function () {
			                var tr = getRowObj(id);
			                if (tr != null) {
			                    tr.parentNode.removeChild(tr);
			                    //alert(removeRow + "----removeRow");
			                    //$("#p_" + removeRow).show();
			                    //$("#p1_" + removeRow).show();
			                } else {
			                    throw new Error("the given object is not contained by the table");
			                }
			            }
			        });
			    }
			</script>

		</div>
	</body>

</html>