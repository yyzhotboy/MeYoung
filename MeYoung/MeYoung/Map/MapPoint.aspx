<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapPoint.aspx.cs" Inherits="MeYoung.Map.MapPoint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/map.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jquery1.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.3"></script>

   <script type="text/javascript">

       $(document).ready(function () {
           var map = new BMap.Map("l-map");
           var point;
           point = new BMap.Point(115.222961,37.923931);
           map.centerAndZoom(point, 14);
           map.enableScrollWheelZoom();
//           function myFun(result) {
//               var cityName = result.name;
//               map.setCenter(cityName);
//           }
//           var myCity = new BMap.LocalCity();
//           myCity.get(myFun);
           map.addEventListener("click", function (e) {
               document.getElementById("r-result2").innerHTML = e.point.lng + "，" + e.point.lat;
               map.clearOverlays();
               var icon1 = new BMap.Icon("../../image/商铺.png", new BMap.Size(33, 51));
               var marker1 = new BMap.Marker(new BMap.Point(e.point.lng, e.point.lat), { icon: icon1 });  // 创建标注
               map.addOverlay(marker1);              // 将标注
           });
           $("#btnok").click(function () {
               var point = document.getElementById("r-result2").innerHTML;
               if (point == '') {
                   alert('请先在地图上点击鼠标选取坐标');
                   return;
               }
               window.parent.LoadPoint(point);
               //               $.ajax({
               //                   type: "post",
               //                   url: "Map/Point",
               //                   data: "{'p_strPoint':'" + point + "'}",
               //                   dataType: "json",
               //                   contentType: "application/json; charset=utf-8",
               //                   success: function (data) {
               //                       alert("您当前的坐标为:" + data.d);
               //                   }
               //               });
               //               var click = MeYoung.Map.MapPoint.Point(point).value;
               //               alert("您当前的坐标为:" + click);

           });
       });
   
</script>
</head>
<body>
<form id="form1" runat="server">  
    </form>
    <div class="add">




   <div id="l-map"></div>
   <div class="add_text">点击地图设置商铺经纬度</div><h3>您商铺的经纬度是：</h3>
<div id="r-result2">
   </div>
   
   <div class="add_btn"> <input id="btnok" type="button" class="btn2_4"  value="确认坐标" /></div></div>
</body>
</html>
