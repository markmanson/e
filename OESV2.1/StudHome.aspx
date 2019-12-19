<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="StudHome.aspx.vb" Inherits="Unirecruite.WebForm1" 
    title="OESV2---Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- This Code is Written by Rajat Argade on 16/10/2019-->
    <meta charset="utf-8"/>
  <meta name="viewport" content="width=device-width, initial-scale=1"/>
  <meta http-equiv="x-ua-compatible" content="ie=edge"/>
    <title>Dashboard</title>
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css"/>
  <!-- overlayScrollbars -->
  <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css"/>
  <!-- Theme style -->
  <link rel="stylesheet" href="dist/css/adminlte.min.css"/>
  <!-- Google Font: Source Sans Pro -->
  <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet"/>
    <script language="javascript">
    function ChangePWD()
    {
        mypop('changepass.aspx?flag=0' + '', '', 1000, 500);//ID=ctl00_ContentPlaceHolder1_txtID,ctl00_ContentPlaceHolder1_txtPWD,aspnetForm
    }
    
    function mypop(url, title, w, h) 
    {
        if ((title == "") || (title == "undefined")) {
            title = "mypop";
        }
        if ((w == "") || (w == undefined)) {
            w = 750;
        }
        if ((h == "") || (h == undefined)) {
            h = 450;
        }
        //var param = 'width=' + screen.width * .8 + ',height=' + screen.height;
        var param = 'width=' + screen.width * .8 + ',height=' + screen.height * .8;
        //param += ',resizable=no,scrollbars=yes,status=1,top=150,left=150';
        param += ',resizable=no,scrollbars=yes,status=1';
        param += ',top='+screen.height * .06 +',left='+screen.width * .1;
        var hWnd = window.open(url, title, param);
        hWnd.focus();
    }

</script>
    <style type="text/css">
        .card-header{
            height:48px;
        }
    </style>

    

       <div class="container">
    <!-- Content Header (Page header) -->
   <section class="content" style="margin-bottom:20px">
       
        <div class="row mb-2">
          <div class="col-sm-6">
              <img src="images/oesus.png"/>
           <%-- <h1 class="m-0 text-dark">Dashboard</h1>--%>
          </div><!-- /.col -->
        </div>
    </section>
    <!-- /.content-header -->

    <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
        <!-- Info boxes -->
        <div class="row">
          <div class="col-12 col-sm-6 col-md-3">
            
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
          
          <!-- /.col -->

          <!-- fix for small devices only -->
          <div class="clearfix hidden-md-up"></div>

        
          <!-- /.col -->
          <div class="col-12 col-sm-6 col-md-3">
            
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
        </div>
          </div>
        </section>

         <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-md-6">
            <!-- AREA CHART -->
           
            <!-- /.card -->

            <!-- DONUT CHART -->
            <div class="card card-warning">
              <div class="card-header">
                <h3 class="card-title"><%=Resources.Resource.StudHome_ExamData%></h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                  </button>
                  <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i></button>
                    
                </div>
              </div>
              <div class="card-body">
                <canvas id="donutChart" style="height:230px; min-height:230px"></canvas>
              </div>
                
              <!-- /.card-body -->
            </div>
            <!-- /.card -->

            <!-- PIE CHART -->
           <%-- <div class="card card-danger">
              <div class="card-header">
                <h3 class="card-title">Pie Chart</h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                  </button>
                  <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i></button>
                </div>
              </div>
              <div class="card-body">
                <canvas id="pieChart" style="height:230px; min-height:230px"></canvas>
              </div>
              <!-- /.card-body -->
            </div>--%>
            <!-- /.card -->

          </div>
          <!-- /.col (LEFT) -->
          <div class="col-md-6">
            <!-- LINE CHART -->
            
            <!-- /.card -->

            <!-- BAR CHART -->
            <div class="card card-warning">
              <div class="card-header">
                <h3 class="card-title"><%=Resources.Resource.StudHome_SecData%></h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                  </button>
                  <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i></button>
                </div>
              </div>
              <div class="card-body">
                <div class="chart">
                  <canvas id="barChart" style="height:230px; min-height:230px"></canvas>
                </div>
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->

            <!-- STACKED BAR CHART -->
          <%--  <div class="card card-success">
              <div class="card-header">
                <h3 class="card-title">Stacked Bar Chart</h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                  </button>
                  <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i></button>
                </div>
              </div>
            <%--  <div class="card-body">
                <div class="chart">
                  <canvas id="stackedBarChart" style="height:230px; min-height:230px"></canvas>
                </div>
              </div>--%>
              <!-- /.card-body -->
            <%--</div>--%>
            <!-- /.card -->

          </div>
          <!-- /.col (RIGHT) -->
        </div>
        <!-- /.row -->
      </div><!-- /.container-fluid -->
    </section>
</div>
        <!--<table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr style="height:30px;">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" border="0" >
                        <tr>
                            <td width="12">
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td valign="top" width="339" align="right">
                                <table class="tbl" id="Table3" cellspacing="0" bordercolordark="#000099" cellpadding="0" width="300" bordercolorlight="#dae0e6" border="0" >
                                    <tr>
                                        <td class="tblhead" colspan="2">
                                            Personal Details
                                        </td>
                                    </tr>
                                    <tr style="text-align:left;height:20px;">
                                        <td style="height: 13px" width="4%">
                                            &nbsp;
                                        </td>
                                        <td style="height: 13px" width="96%">
                                            <a id="lnkProfile" runat="server" href="register.aspx?E=1">Profile</a>
                                        </td>
                                    </tr>
                                    <tr style="text-align:left;height:20px;">
                                        <td style="height: 13px" width="4%">
                                            &nbsp;
                                        </td>
                                        <td style="height: 13px" width="96%">
                                            <asp:Button ID="btnChange" runat="server" CssClass="btn" Text="Change Password" OnClientClick="ChangePWD()" />
                                            <%--<input type="button" value="Change PWD" onclick="javascript:return ChangePWD();" />--%>
                                        </td>
                                    </tr>
                                    <%--<tr style="height:3px;">
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                </table>
                            </td>
                            <td valign="top" width="335" align="center">
                                <table class="tbl" cellspacing="0" bordercolordark="#000099" cellpadding="0" width="300" bordercolorlight="#dae0e6" border="0">
                                    <tr>
                                        <td class="tblhead" colspan="2">
                                            Exam Box
                                        </td>
                                    </tr>
                                    <tr style="text-align:left;height:20px;">
                                        <td width="4%" style="height: 16px">
                                            &nbsp;
                                        </td>
                                        <td width="96%" style="height: 16px">
                                            <a href="AssignedExam.aspx">Assigned Exam</a>
                                        </td>
                                    </tr>
                                    <tr style="text-align:left;height:20px;">
                                        <td width="4%" style="height: 16px">
                                            &nbsp;
                                        </td>
                                        <td width="96%" style="height: 16px">
                                            <a href="ResultBox.aspx">Result</a>
                                        </td>                                        
                                    </tr>
                                    <tr style="text-align:left;height:20px;">
                                        <td width="4%" style="height: 16px">
                                            &nbsp;
                                        </td>
                                        <td width="96%" style="height: 16px">
                                            <a href="VideoLive.aspx">Live Videos</a>
                                        </td>
                                    </tr>
                                    <%--<tr style="height:3px;">
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>--%>
                                </table>
                            </td>
                            <%--<td valign="top" width="316">
                                <table class="tbl" id="tblSearch" cellspacing="0" bordercolordark="#000099" cellpadding="0" width="300" bordercolorlight="#dae0e6" border="0" runat="server">
                                    <tr>
                                        <td class="tblhead" colspan="2">
                                            Study Material
                                        </td>
                                    </tr>
                                    <tr style="text-align:left;height:20px;">
                                        <td style="width: 4%">
                                        </td>
                                        <td style="height: 16px; width: 96%">
                                            <a href="#">Video List</a>
                                        </td>
                                    </tr>
                                    <tr style="text-align:left;height:20px;">
                                        <td style="width: 4%">
                                        </td>
                                        <td style="height: 16px; width: 96%">
                                            <a href="#">File List</a>
                                        </td>
                                    </tr>
                                    
                                </table>
                            </td>--%>
                            
                        </tr>
                        <%--<tr>
                            <td colspan="4">
                                <asp:TextBox ID="txtID" runat="server" Text=""></asp:TextBox>
                                <asp:TextBox ID="txtPWD" runat="server" Text=""></asp:TextBox>
                            </td>
                        </tr>--%>
                    </table>
                </td>
            </tr>
        </table>-->
<!-- overlayScrollbars -->
<script src="plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
<!-- AdminLTE App -->
<script src="dist/js/adminlte.js"></script>

<!-- OPTIONAL SCRIPTS -->
<script src="dist/js/demo.js"></script>
          <script src="plugins/jquery-mousewheel/jquery.mousewheel.js"></script>
<script src="plugins/raphael/raphael.min.js"></script>
<script src="plugins/jquery-mapael/jquery.mapael.min.js"></script>
<script src="../../dist/js/adminlte.min.js"></script>
<script src="plugins/jquery-mapael/maps/usa_states.min.js"></script>
<!-- ChartJS -->
<script src="plugins/chart.js/Chart.min.js"></script>

<!-- PAGE SCRIPTS -->
<script src="dist/js/pages/dashboard2.js"></script>
          <script>
  $(function () {
    /* ChartJS
     * -------
     * Here we will create a few charts using ChartJS
     */

    //--------------
    //- AREA CHART -
    //--------------

    // Get context with jQuery - using jQuery's .get() method.
    var areaChartCanvas = $('#donutChart').get(0).getContext('2d')

   /* var areaChartData = {
      labels  : ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
      datasets: [
        {
          label               : 'Digital Goods',
          backgroundColor     : 'rgba(60,141,188,0.9)',
          borderColor         : 'rgba(60,141,188,0.8)',
          pointRadius          : false,
          pointColor          : '#3b8bba',
          pointStrokeColor    : 'rgba(60,141,188,1)',
          pointHighlightFill  : '#fff',
          pointHighlightStroke: 'rgba(60,141,188,1)',
          data                : [28, 48, 40, 19, 86, 27, 90]
        },
        {
          label               : 'Electronics',
          backgroundColor     : 'rgba(210, 214, 222, 1)',
          borderColor         : 'rgba(210, 214, 222, 1)',
          pointRadius         : false,
          pointColor          : 'rgba(210, 214, 222, 1)',
          pointStrokeColor    : '#c1c7d1',
          pointHighlightFill  : '#fff',
          pointHighlightStroke: 'rgba(220,220,220,1)',
          data                : [65, 59, 80, 81, 56, 55, 40]
        },
      ]
    }*/

    var areaChartOptions = {
      maintainAspectRatio : false,
      responsive : true,
      legend: {
        display: false
      },
      scales: {
        xAxes: [{
          gridLines : {
            display : false,
          }
        }],
        yAxes: [{
          gridLines : {
            display : false,
          }
        }]
      }
    }

    // This will get the first returned node in the jQuery collection.
    //var areaChart       = new Chart(areaChartCanvas, { 
    //  type: 'line',
    //  data: areaChartData, 
    //  options: areaChartOptions
    //})

    //-------------
    //- LINE CHART -
    //--------------
   

    //-------------
    //- DONUT CHART -
    //-------------
    // Get context with jQuery - using jQuery's .get() method.
                    //var donutChartCanvas = $('#donutChart').get(0).getContext('2d')
                    //var donutData        = {
                    //  labels: [
                    //      'Chrome', 
                    //      'IE',
                    //      'FireFox', 
                    //      'Safari', 
                    //      'Opera', 
                    //      'Navigator', 
                    //  ],
                    //  datasets: [
                    //    {
                    //      data: [700,500,400,600,300,100],
                    //      backgroundColor : ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc', '#d2d6de'],
                    //    }
                    //  ]
                    //}
                    //var donutOptions     = {
                    //  maintainAspectRatio : false,
                    //  responsive : true,
                    //}
                    ////Create pie or douhnut chart
                    //// You can switch between pie and douhnut using the method below.
                    //var donutChart = new Chart(donutChartCanvas, {
                    //  type: 'doughnut',
                    //  data: donutData,
                    //  options: donutOptions      
                    //})

                    ////-------------
                    ////- PIE CHART -
                    ////-------------
                    //// Get context with jQuery - using jQuery's .get() method.
                    //var pieChartCanvas = $('#pieChart').get(0).getContext('2d')
                    //var pieData        = donutData;
                    //var pieOptions     = {
                    //  maintainAspectRatio : false,
                    //  responsive : true,
                    //}
                    ////Create pie or douhnut chart
                    //// You can switch between pie and douhnut using the method below.
                    //var pieChart = new Chart(pieChartCanvas, {
                    //  type: 'pie',
                    //  data: pieData,
                    //  options: pieOptions      
                    //})

                       // //-------------
                       // //- BAR CHART -
                       // //-------------
                       // var barChartCanvas = $('#barChart').get(0).getContext('2d')
                       // var barChartData = jQuery.extend(true, {}, areaChartData)
                       // var temp0 = areaChartData.datasets[0]
                       // var temp1 = areaChartData.datasets[1]
                       //barChartData.datasets[0] = temp1
                       // barChartData.datasets[1] = temp0

                       // var barChartOptions = {
                       //   responsive              : true,
                       //   maintainAspectRatio     : false,
                       //   datasetFill             : false
                       // }

                       // var barChart = new Chart(barChartCanvas, {
                       //   type: 'bar', 
                       //   data: barChartData,
                       //   options: barChartOptions
                       // })

    //---------------------
    //- STACKED BAR CHART -
    //---------------------
    var stackedBarChartCanvas = $('#stackedBarChart').get(0).getContext('2d')
    var stackedBarChartData = jQuery.extend(true, {}, barChartData)

    var stackedBarChartOptions = {
      responsive              : true,
      maintainAspectRatio     : false,
      scales: {
        xAxes: [{
          stacked: true,
        }],
        yAxes: [{
          stacked: true
        }]
      }
    }

    var stackedBarChart = new Chart(stackedBarChartCanvas, {
      type: 'bar', 
      data: stackedBarChartData,
      options: stackedBarChartOptions
    })
  })
</script>

<%--- DONUT CHART ---%>
 <script>
        $(function () {
            $.ajax({
                type: "POST",
                url: "StudHome.aspx/StatisticExamDataPie",
                //data: JSON.stringify({ username: username }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {

                    var result1 = result.d;

                    //alert(result1);
                    

                    var donutChartCanvas = $('#donutChart').get(0).getContext('2d');
                    var donutData = {
                        
                        labels: ['Appeared(%)','Not Appeared(%)'],
                        datasets: [
                            {
                                data: result1,
                                backgroundColor: ['#00a65a','#f39c12'],
                            }
                        ]
                    }
                    var donutOptions = {
                        maintainAspectRatio: false,
                        responsive: true,
                    }
                    var donutChart = new Chart(donutChartCanvas, {
                        type: 'doughnut',
                        data: donutData,
                        options: donutOptions
                    })

                }

            })
        })
    </script>

<%--- Pie CHART ---%>
    <script>
              $(function () {
            $.ajax({
                type: "POST",
                url: "StudHome.aspx/StatisticExamData",
                //data: JSON.stringify({ username: username }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    var result1 = result.d;

                    var pieChartCanvas = $('#pieChart').get(0).getContext('2d')
                   
                    var pieData ={
                        
                        labels: ['Appeared(%)','Not Appeared(%)'],
                        datasets: [
                            {
                                data: result1,
                                backgroundColor: ['#d2d6de','#3c8dbc'],
                            }
                        ]
                    } 
                    var pieOptions     = {
                      maintainAspectRatio : false,
                      responsive : true,
                    }
                    var pieChart = new Chart(pieChartCanvas, {
                      type: 'pie',
                      data: pieData,
                      options: pieOptions
                          })
                      }

            })
        })
  </script>

<%--Bar Chart--%>
  <script>
      $(function () {

          $.ajax({
                type: "POST",
                url: "StudHome.aspx/StatisticBarChart",
                //data: JSON.stringify({ username: username }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                },
                success: function (result) {
                    var result1 = result.d;
                    var labeldata = [];
                    var valuedata = [];
                    for (var i = 0; i < result1.length; i++)
                    {                       
                        labeldata[i] = result1[i].split("_")[0];
                        valuedata[i] = result1[i].split("_")[1];
                        //alert(labeldata[i]+"=="+valuedata[i]);
                    }
          var areaChartData = {
      labels  : ['Grammer',' ', 'Vocab',' ','Reading',' ','Listening'],
        //labels  : labeldata,
      datasets: [
        {
          label               : 'Section Wise Performance',
          backgroundColor     : 'rgba(60,141,188,0.9)',
          borderColor         : 'rgba(60,141,188,0.8)',
          pointRadius          : false,
          pointColor          : '#3b8bba',
          pointStrokeColor    : 'rgba(60,141,188,1)',
          pointHighlightFill  : '#fff',
              pointHighlightStroke: 'rgba(60,141,188,1)',
              data: [valuedata[0],0,valuedata[1],0,valuedata[2],0,valuedata[3]]
              //data: valuedata
        },
       /* {
          label               : 'Electronics',
          backgroundColor     : 'rgba(210, 214, 222, 1)',
          borderColor         : 'rgba(210, 214, 222, 1)',
          pointRadius         : false,
          pointColor          : 'rgba(210, 214, 222, 1)',
          pointStrokeColor    : '#c1c7d1',
          pointHighlightFill  : '#fff',
          pointHighlightStroke: 'rgba(220,220,220,1)',
          data                : [65, 59, 80, 81, 56, 55, 40]
        },*/
      ]
          }

          var barChartCanvas = $('#barChart').get(0).getContext('2d')
    var barChartData = jQuery.extend(true, {}, areaChartData)
    var temp0 = areaChartData.datasets[0]
    //var temp1 = areaChartData.datasets[1]
   barChartData.datasets[0] = temp0
    //barChartData.datasets[1] = temp0

    var barChartOptions = {
      responsive              : true,
      maintainAspectRatio     : false,
      datasetFill             : false
    }

    var barChart = new Chart(barChartCanvas, {
      type: 'bar', 
      data: barChartData,
      options: barChartOptions
    })

         }

            })


      })
  </script>


  
    <!-- Ended by Rajat Argade on 16/10/2019 -->
</asp:Content>
