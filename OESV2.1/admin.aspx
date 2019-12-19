<%@ Page Language="vb" AutoEventWireup="false" Inherits="Unirecruite.unirecruite.admin"
    MasterPageFile="~/MasterPage.Master" Title="OESV2---Dashboard"
    CodeBehind="admin.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!-- This Code is Written by Rajat Argade on 16/10/2019-->
     <meta charset="utf-8"/>
  <meta name="viewport" content="width=device-width, initial-scale=1"/>
  <meta http-equiv="x-ua-compatible" content="ie=edge"/>
    <title>Home</title>

    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css"/>
  <!-- overlayScrollbars -->
  <link rel="stylesheet" href="plugins/overlayScrollbars/css/OverlayScrollbars.min.css"/>
  <!-- Theme style -->
  <link rel="stylesheet" href="dist/css/adminlte.min.css"/>
  <!-- Google Font: Source Sans Pro -->
  <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet"/>
    <style type="text/css">
        #_ctl0_ContentPlaceHolder1_Labelcentre{
            font-size: 22px;
            font-display: auto;
            font-weight:550;
        }
        #_ctl0_ContentPlaceHolder1_ddlCenters{
            display: block;
            width: 100%;
            height: calc(2.25rem + 2px);
            padding: .375rem .75rem;
            font-size: 1rem;
            font-weight: 400;
            line-height: 1.5;
            color: #495057;
            background-color: #fff;
            background-clip: padding-box;
            border: 1px solid #495057;
            border-radius: .25rem;
            box-shadow: 0px 0px 10px 0px;
            transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out;
            border: 1px solid #1f2d3d;            
        }
        #ClassName {
                margin-bottom: 10px;
            }
        .card-header{
            height:48px;
        }
    </style>

    <script src="build/js/jquery.min.js"></script>
    <script src="build/js/popper.min.js"></script>
    <script src="build/js/bootstrap.min.js"></script>



 <%--   <form id="form1" runat="server">--%>
    <div class="container-fluid">
    <section class="content">
        <div class="row mb-2">
          <div class="col-sm-6">
              <img src="images/oesus.png"/>
           <%-- <h1 class="m-0 text-dark">Dashboard</h1>--%>
          </div><!-- /.col -->
        </div>
    </section>

        
      
        <section class="content" id="ClassName">
          <div class="row">
              <div class="col-lg-3"></div>
              <div class="col-lg-6">
                  <div class="row">
                      <div class="col-lg-3">
                          <asp:Label runat="server" ID="Labelcentre" Text="<%$Resources:Resource,register_ClsNm%>"></asp:Label>
                      </div>
                      <div class="col-lg-9">
                         
                          <asp:DropDownList ID="ddlCenters" runat="server" AutoPostBack="false" Font-Names="Times New Roman"></asp:DropDownList>
                      </div>
                  </div>
              </div>
              <div class="col-lg-3"></div>
          </div>
         </section>
        <!-- Info boxes -->
       <%-- <div class="row">
          <div class="col-12 col-sm-6 col-md-3">
           <%-- <div class="info-box">
              <span class="info-box-icon bg-info elevation-1"><i class="fas fa-cog"></i></span>

              <div class="info-box-content">
                <span class="info-box-text">cpu traffic</span>
                <span class="info-box-number">
                  10
                  <small>%</small>
                </span>
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
          <div class="col-12 col-sm-6 col-md-3">
            <div class="info-box mb-3">
              <span class="info-box-icon bg-danger elevation-1"><i class="fas fa-thumbs-up"></i></span>

              <div class="info-box-content">
                <span class="info-box-text">Likes</span>
                <span class="info-box-number">41,410</span>
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->

          <!-- fix for small devices only -->
          <div class="clearfix hidden-md-up"></div>

          <div class="col-12 col-sm-6 col-md-3">
            <div class="info-box mb-3">
              <span class="info-box-icon bg-success elevation-1"><i class="fas fa-shopping-cart"></i></span>

              <div class="info-box-content">
                <span class="info-box-text">Sales</span>
                <span class="info-box-number">760</span>
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
          <div class="col-12 col-sm-6 col-md-3">
            <div class="info-box mb-3">
              <span class="info-box-icon bg-warning elevation-1"><i class="fas fa-users"></i></span>

              <div class="info-box-content">
                <span class="info-box-text">New Members</span>
                <span class="info-box-number">2,000</span>
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
        </div>--%>


         <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
        <div class="row">
          <div class="col-md-6">


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
                <div>
              <div class="card-body">
                <canvas id="donutChart" style="height:230px; min-height:230px"></canvas>
              </div>
            </div>
              <!-- /.card-body -->
            </div>
           
                <!-- vocab Bar CHART -->
               <div class="card card-warning">
              <div class="card-header">
                <h3 class="card-title"><%=Resources.Resource.admin_vbpr%></h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                  </button>
                  <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i></button>
                </div>
              </div>
              <div class="card-body">
                <div class="chart">
                  <canvas id="vocabbarChart" style="height:230px; min-height:230px"></canvas>
                </div>
              </div>           
            </div>


               <!-- Listening Bar CHART -->
               <div class="card card-warning">
              <div class="card-header">
                <h3 class="card-title"><%=Resources.Resource.admin_listningper%></h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                  </button>
                  <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i></button>
                </div>
              </div>
              <div class="card-body">
                <div class="chart">
                  <canvas id="listeningbarChart" style="height:230px; min-height:230px"></canvas>
                </div>
              </div>           
            </div>


          </div>
         
          <div class="col-md-6">
            <!-- BAR CHART -->
            <div class="card card-warning">
              <div class="card-header">
                <h3 class="card-title"><%=Resources.Resource.admin_grmrper%></h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                  </button>
                  <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i></button>
                </div>
              </div>
              <div class="card-body">
                <div class="chart">
                  <canvas id="grammerbarChart" style="height:230px; min-height:230px"></canvas>
                </div>
              </div>           
            </div>
         

            <!-- Reading BAR CHART -->
           <div class="card card-warning">
              <div class="card-header">
                <h3 class="card-title"><%=Resources.Resource.admin_Readingper%></h3>

                <div class="card-tools">
                  <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i>
                  </button>
                  <button type="button" class="btn btn-tool" data-card-widget="remove"><i class="fas fa-times"></i></button>
                </div>
              </div>
              <div class="card-body">
                <div class="chart">
                  <canvas id="readingbarChart" style="height:230px; min-height:230px"></canvas>
                </div>
              </div>           
            </div>
           

          </div>
         
        </div>
        
      </div>
    </section>
  </div>

    <!--<table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <table cellspacing="0" cellpadding="0" border="0" >
                    <tr>
                        <td width="12">
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td valign="top" width="339" align="right">
                            <table class="tbl" id="Table3" cellspacing="0" bordercolordark="#000099" cellpadding="0"
                                width="300" bordercolorlight="#dae0e6" border="0" >
                                <tr>
                                    <td class="tblhead" colspan="2">
                                        New User
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td style="height: 13px" width="4%">
                                        &nbsp;
                                    </td>
                                    <td style="height: 13px" width="96%">
                                        <a href="register.aspx">Registration</a>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td style="height: 13px" width="4%">
                                        &nbsp;
                                    </td>
                                    <td style="height: 13px" width="96%">
                                        <a href="AdminList.aspx">Administrator List</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 13px" width="4%">
                                        &nbsp;
                                    </td>
                                    <%-- <td style="height: 13px" width="96%">
                                                        <a href="ClientInformation.aspx">Register Client Information</a>
                                                    </td>--%>
                                </tr>
                                <tr>
                                    <!--<td><A href="question_ans.aspx?qid="  ??>Question Bank</A></td>-->
                               <!-- </tr>
                            </table>
                        </td>
                        <td valign="top" width="335" align="center">
                            <table class="tbl" cellspacing="0" bordercolordark="#000099" cellpadding="0" width="300"
                                bordercolorlight="#dae0e6" border="0">
                                <tr>
                                    <td class="tblhead" colspan="2">
                                        Maintenance
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td width="4%" style="height: 16px">
                                        &nbsp;
                                    </td>
                                    <td width="96%" style="height: 16px">
                                        <a href="CentreMaintenance.aspx">Class Maintenance</a><font face="MS UI Gothic">&nbsp;</font>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td width="4%" style="height: 16px">
                                        &nbsp;
                                    </td>
                                    <td width="96%" style="height: 16px">
                                        <a href="CourseMaintenance.aspx">Course Maintenance</a><font face="MS UI Gothic">&nbsp;</font>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td width="4%" style="height: 16px">
                                        &nbsp;
                                    </td>
                                    <td width="96%" style="height: 16px">
                                        <a href="ManageCourse.aspx">Manage Course</a><font face="MS UI Gothic">&nbsp;</font>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td>
                                    </td>
                                    <td>
                                        <a href="SubjectRegistration.aspx">Subject Maintenance</a>
                                        <%--<a href="CourseRegistration.aspx">Course Registration</a><font face="MS UI Gothic">&nbsp;</font>--%>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td>
                                    </td>
                                    <td>
                                        <a href="WeightSearch.aspx">Assign Weightage</a>
                                        <%--<a href="CourseRegistration.aspx">Course Registration</a><font face="MS UI Gothic">&nbsp;</font>--%>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <a href="searchQuestion.aspx">Question Maintenance</a>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td style="height: 15px">
                                    </td>
                                    <td style="height: 15px">
                                        <a href="CandStatus.aspx">Candidate Status (Provisional mark-sheet)</a>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td>
                                    </td>
                                    <td>
                                        <a href="BulkImport.aspx">BulkImport UserData</a><font face="MS UI Gothic">&nbsp;</font>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td>
                                    </td>
                                    <td>
                                        <a href="StudentSearch.aspx">Student List</a><font face="MS UI Gothic">&nbsp;</font>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td>
                                    </td>
                                    <td>
                                        <a href="ManageVideo.aspx">Upload Videos</a>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td>
                                    </td>
                                    <td>
                                        <a href="VideoList.aspx">Videos List</a>
                                    </td>
                                </tr>
                                <%--  <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                             <a href="SubjectRegistration.aspx">Subject Maintenance</a>
                                                            <%--<a href="CourseRegistration.aspx">Course Registration</a><font face="MS UI Gothic">&nbsp;</font>
                                                        </td>
                                                    </tr>--%>
                            </table>
                        </td>
                        <td valign="top" width="316">
                            <table class="tbl" id="tblSearch" cellspacing="0" bordercolordark="#000099" cellpadding="0"
                                width="300" bordercolorlight="#dae0e6" border="0" runat="server">
                                <tr>
                                    <td class="tblhead" colspan="2">
                                        Exam
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td style="width: 4%">
                                    </td>
                                    <td style="height: 16px; width: 96%">
                                        <a href="search.aspx">Assign Exam to student</a>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td style="width: 4%">
                                    </td>
                                    <td style="height: 16px; width: 96%">
                                        <a href="ExamCount.aspx">Count of question published</a>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td style="width: 4%">
                                    </td>
                                    <td style="height: 16px; width: 96%">
                                        <a href="AppearedExam.aspx">Exam Report</a>
                                    </td>
                                </tr>
                                <tr style="text-align:left">
                                    <td style="width: 4%">
                                    </td>
                                    <td style="height: 16px; width: 96%">
                                        <a href="StudentTimeInfo.aspx">Candidate Time Information</a>
                                    </td>
                                </tr>
                               <%-- <tr style="text-align:left">
                                    <td style="width: 4%">
                                    </td>
                                    <td style="height: 16px; width: 96%">
                                        <a href="ReassignExam.aspx">Reassign Exam</a>
                                    </td>
                                </tr>--%>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="12">
                        </td>
                        <td valign="top" width="339">
                            <font face="MS UI Gothic"></font>
                        </td>
                        <td valign="top" width="335">
                            <font face="MS UI Gothic"></font>
                        </td>
                        <td valign="top" width="316">
                            <font face="MS UI Gothic"></font>
                        </td>
                    </tr>
                </table>
                <br>
            </td>
            <td style="height: 280px">
                &nbsp;
            </td>
        </tr>
    </table>-->
<%--<script src="plugins/jquery/jquery.min.js"></script>
<!-- Bootstrap -->
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>--%>
<!-- overlayScrollbars -->
<script src="plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
<!-- AdminLTE App -->
<script src="dist/js/adminlte.js"></script>

<!-- OPTIONAL SCRIPTS -->
<script src="dist/js/demo.js"></script>

<!-- PAGE PLUGINS -->
<!-- jQuery Mapael -->
<%--<script src="plugins/jquery-mousewheel/jquery.mousewheel.js"></script>--%>
<script src="plugins/raphael/raphael.min.js"></script>
<script src="plugins/jquery-mapael/jquery.mapael.min.js"></script>
<script src="dist/js/adminlte.min.js"></script>
<script src="plugins/jquery-mapael/maps/usa_states.min.js"></script>
<!-- ChartJS -->
<script src="plugins/chart.js/Chart.min.js"></script>
    

<!-- PAGE SCRIPTS -->
<script src="dist/js/pages/dashboard2.js"></script>


    <script src='https://cdn.rawgit.com/pguso/jquery-plugin-circliful/master/js/jquery.circliful.min.js'></script>
<script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js" ></script>

  <%-- <script src="plugins/jquery-ui/jquery-ui.min.js"></script>
    <script src="plugins/jquery-ui/jquery-ui.js"></script>--%>
  
     

<%-- Donut Chart --%>
    <script>
        $(function () {
            var donutData = {
                        labels:   [],
                        datasets: [{ },]
            }
            var donutChartCanvas = $('#donutChart').get(0).getContext('2d');
             var donutOptions = {
                            maintainAspectRatio: false,
                 responsive: true,
                  tooltips: {
      callbacks: {
        label: function(tooltipItem, data) {
          return data['labels'][tooltipItem['index']] + ': ' + data['datasets'][0]['data'][tooltipItem['index']] + '%';
        }
      }
    }

   }
     var donutChart = new Chart(donutChartCanvas, {
        type: 'doughnut',
        data: donutData,
        options: donutOptions
            })

            //Default Loading
            var Classid1 = $('#_ctl0_ContentPlaceHolder1_ddlCenters').val();
            if (Classid1 == Classid1) {
                  $.ajax({
                      type: "POST",
                      url: "admin.aspx/NotApperedDountChart",
                      data: JSON.stringify({ Classid1: Classid1 }),
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      error: function (XMLHttpRequest, textStatus, errorThrown) {
                          alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                      },
                      success: function (result) {
                          var result1 = result.d;
                         var labeldata1 = [];
                          var valuedata1 = [];
                          for (var i = 0; i < result1.length; i++) {
                              labeldata1[i] = result1[i].split("_")[0];
                              valuedata1[i] = result1[i].split("_")[1];
                            
                          }  

                          donutChart.data.labels=labeldata1
                          donutChart.data.datasets = [{
                              data: valuedata1,
                              backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#A09717", "#971550","#971550"]                            
                          }]
                          donutChart.update()
                      }
                  })
              }


            //On Change Loading
            $("#_ctl0_ContentPlaceHolder1_ddlCenters").change(function () {
                //alert($('#_ctl0_ContentPlaceHolder1_ddlCenters').val());
                var Classid1 = $('#_ctl0_ContentPlaceHolder1_ddlCenters').val();

                if ($('#_ctl0_ContentPlaceHolder1_ddlCenters').val() == Classid1) {
                  $.ajax({
                      type: "POST",
                      url: "admin.aspx/NotApperedDountChart",
                      data: JSON.stringify({ Classid1: Classid1 }),
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      error: function (XMLHttpRequest, textStatus, errorThrown) {
                          alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                      },
                      success: function (result) {
                          var result1 = result.d;
                         var labeldata1 = [];
                          var valuedata1 = [];
                          for (var i = 0; i < result1.length; i++) {
                              labeldata1[i] = result1[i].split("_")[0];
                              valuedata1[i] = result1[i].split("_")[1];
                            
                          }  

                          donutChart.data.labels=labeldata1
                          donutChart.data.datasets = [{
                              data: valuedata1,
                              backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#A09717", "#971550","#971550"]                            
                          }]
                          donutChart.update()
                      }
                  })
              }

            })
        })
    </script>

     <%--Grammer Bar Chart--%>
  <script>
      $(document).ready(function () {            
          var areaChartData =
          {
                        labels:   [],
                        datasets: [{ },]
          }
        
    var barChartCanvas = $('#grammerbarChart').get(0).getContext('2d')
    var barChartData = jQuery.extend(true, {}, areaChartData)
    var temp0 = areaChartData.datasets[0]
    //var temp1 = areaChartData.datasets[1]
   barChartData.datasets[0] = temp0
    //barChartData.datasets[1] = temp0
    var barChartOptions = {     
           scales: {
            
            yAxes: [{
            ticks: {
            
                   min: 0,
                   max: 100,
                   callback: function(value){return value+ "%"}
                },  
								scaleLabel: {
                   display: true,
                   //labelString: "Percentage"
                }
            }]
        },

        responsive: true,
    tooltips: {
      callbacks: {
        label: function(tooltipItem, data) {
          return data['labels'][tooltipItem['index']] + ': ' + data['datasets'][0]['data'][tooltipItem['index']] + '%';
        }
      }
    }
  }   
    var barChart = new Chart(barChartCanvas, {
      type: 'bar', 
      data: barChartData,
      options: barChartOptions
          })

          //Default loading
          var Classid1 = $('#_ctl0_ContentPlaceHolder1_ddlCenters').val();
                //alert(Cls);
                if (Classid1 == Classid1)
                {
                    $.ajax({
                        type: "POST",
                        url: "admin.aspx/GrammerBarChart",
                        data: JSON.stringify({ Classid1: Classid1 }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            var result1 = result.d;
                            var labeldata1 = [];
                            var valuedata1 = [];
                            for (var i = 0; i < result1.length; i++) {
                                labeldata1[i] = result1[i].split("_")[0];
                                valuedata1[i] = result1[i].split("_")[1];
                                //alert(labeldata1[i]+"=="+valuedata1[i]);
                            }

                            barChart.data.labels = labeldata1
                            barChart.data.datasets = [{
                                label: "Grammer",
                                backgroundColor: "#3cba9f"/* ["#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f"]*/,
                                data: [valuedata1[0], valuedata1[1], valuedata1[2], valuedata1[3], valuedata1[4], valuedata1[5], valuedata1[6], valuedata1[7], valuedata1[8]] //[10, 25, 45, 84, 4]
                            }]
                            barChart.update()
                        }
                    
                    })
                }

          //On Change loading
          $("#_ctl0_ContentPlaceHolder1_ddlCenters").change(function () {
              //alert($('#_ctl0_ContentPlaceHolder1_ddlCenters').val());
              var Classid1 = $('#_ctl0_ContentPlaceHolder1_ddlCenters').val();
          
              if ($('#_ctl0_ContentPlaceHolder1_ddlCenters').val() == Classid1) {
                  $.ajax({
                      type: "POST",
                      url: "admin.aspx/GrammerBarChart",
                      data: JSON.stringify({ Classid1: Classid1 }),
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      error: function (XMLHttpRequest, textStatus, errorThrown) {
                          alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                      },
                      success: function (result) {
                          var result1 = result.d;
                         var labeldata1 = [];
                          var valuedata1 = [];
                          for (var i = 0; i < result1.length; i++) {
                              labeldata1[i] = result1[i].split("_")[0];
                              valuedata1[i] = result1[i].split("_")[1];
                              //alert(labeldata1[i]+"=="+valuedata1[i]);
                          }
                          var color ="#3cba9f";
                          barChart.data.labels=labeldata1
                          barChart.data.datasets = [{
                              label: "Grammer",
                              backgroundColor:"#3cba9f"/* ["#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f"]*/,
                              data: [valuedata1[0],valuedata1[1],valuedata1[2],valuedata1[3],valuedata1[4],valuedata1[5], valuedata1[6],valuedata1[7],valuedata1[8]] //[10, 25, 45, 84, 4]
                          }]
                          barChart.update()
                      }
                  })
              }                     
          })
      })
  </script>

     <%--vocab Bar Chart--%>
    <script>
      $(document).ready(function () {            
          var areaChartData =
          {
                        labels:   [],
                        datasets: [{ },]
          }
        
    var barChartCanvas = $('#vocabbarChart').get(0).getContext('2d')
    var barChartData = jQuery.extend(true, {}, areaChartData)
    var temp0 = areaChartData.datasets[0]
    //var temp1 = areaChartData.datasets[1]
   barChartData.datasets[0] = temp0
    //barChartData.datasets[1] = temp0
    var barChartOptions = {     
           scales: {
            
            yAxes: [{
            ticks: {
            
                   min: 0,
                   max: 100,
                   callback: function(value){return value+ "%"}
                },  
								scaleLabel: {
                   display: true,
                   //labelString: "Percentage"
                }
            }]
        },

        responsive: true,
    tooltips: {
      callbacks: {
        label: function(tooltipItem, data) {
          return data['labels'][tooltipItem['index']] + ': ' + data['datasets'][0]['data'][tooltipItem['index']] + '%';
        }
      }
    }
          }


    var barChart = new Chart(barChartCanvas, {
      type: 'bar', 
      data: barChartData,
      options: barChartOptions
          })

          //Default loading
          var Classid1 = $('#_ctl0_ContentPlaceHolder1_ddlCenters').val();
                //alert(Cls);
                if (Classid1 == Classid1)
                {
                    $.ajax({
                        type: "POST",
                        url: "admin.aspx/VocabBarChart",
                        data: JSON.stringify({ Classid1: Classid1 }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            var result1 = result.d;
                            var labeldata1 = [];
                            var valuedata1 = [];
                            for (var i = 0; i < result1.length; i++) {
                                labeldata1[i] = result1[i].split("_")[0];
                                valuedata1[i] = result1[i].split("_")[1];
                                //alert(labeldata1[i]+"=="+valuedata1[i]);
                            }

                            barChart.data.labels = labeldata1
                            barChart.data.datasets = [{
                                label: "Vocab",
                                backgroundColor: "#8e5ea2"/* ["#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f"]*/,
                                data: [valuedata1[0], valuedata1[1], valuedata1[2], valuedata1[3], valuedata1[4], valuedata1[5], valuedata1[6], valuedata1[7], valuedata1[8]] //[10, 25, 45, 84, 4]
                            }]
                            barChart.update()
                        }
                    
                    })
                }

          //On Change Loading
          $("#_ctl0_ContentPlaceHolder1_ddlCenters").change(function () {
              //alert($('#_ctl0_ContentPlaceHolder1_ddlCenters').val());
              var Classid1 = $('#_ctl0_ContentPlaceHolder1_ddlCenters').val();
          
              if ($('#_ctl0_ContentPlaceHolder1_ddlCenters').val() == Classid1) {
                  $.ajax({
                      type: "POST",
                      url: "admin.aspx/VocabBarChart",
                      data: JSON.stringify({ Classid1: Classid1 }),
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      error: function (XMLHttpRequest, textStatus, errorThrown) {
                          alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                      },
                      success: function (result) {
                          var result1 = result.d;
                         var labeldata1 = [];
                          var valuedata1 = [];
                          for (var i = 0; i < result1.length; i++) {
                              labeldata1[i] = result1[i].split("_")[0];
                              valuedata1[i] = result1[i].split("_")[1];
                              //alert(labeldata1[i]+"=="+valuedata1[i]);
                          }
                          
                          barChart.data.labels=labeldata1
                          barChart.data.datasets = [{
                              label: "Vocab",
                              backgroundColor:"#8e5ea2"/* ["#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f"]*/,
                              data: [valuedata1[0],valuedata1[1],valuedata1[2],valuedata1[3],valuedata1[4],valuedata1[5], valuedata1[6],valuedata1[7],valuedata1[8],0] //[10, 25, 45, 84, 4]
                          }]
                          barChart.update()
                      }
                  })
              }                     
          })
      })
  </script>


 <%--Reading Bar Chart--%>
      <script>
      $(document).ready(function () {            
          var areaChartData =
          {
                        labels:   [],
                        datasets: [{ },]
          }
        
    var barChartCanvas = $('#readingbarChart').get(0).getContext('2d')
    var barChartData = jQuery.extend(true, {}, areaChartData)
    var temp0 = areaChartData.datasets[0]
    //var temp1 = areaChartData.datasets[1]
   barChartData.datasets[0] = temp0
    //barChartData.datasets[1] = temp0
    var barChartOptions = {     
           scales: {
            
            yAxes: [{
            ticks: {
            
                   min: 0,
                   max: 100,
                   callback: function(value){return value+ "%"}
                },  
								scaleLabel: {
                   display: true,
                   //labelString: "Percentage"
                }
            }]
        },

        responsive: true,
    tooltips: {
      callbacks: {
        label: function(tooltipItem, data) {
          return data['labels'][tooltipItem['index']] + ': ' + data['datasets'][0]['data'][tooltipItem['index']] + '%';
        }
      }
    }
  }
    var barChart = new Chart(barChartCanvas, {
      type: 'bar', 
      data: barChartData,
      options: barChartOptions
          })

          //Default Loading
          var Classid1 = $('#_ctl0_ContentPlaceHolder1_ddlCenters').val();
                //alert(Cls);
                if (Classid1 == Classid1)
                {
                    $.ajax({
                        type: "POST",
                        url: "admin.aspx/ReadingBarChart",
                        data: JSON.stringify({ Classid1: Classid1 }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            var result1 = result.d;
                            var labeldata1 = [];
                            var valuedata1 = [];
                            for (var i = 0; i < result1.length; i++) {
                                labeldata1[i] = result1[i].split("_")[0];
                                valuedata1[i] = result1[i].split("_")[1];
                                //alert(labeldata1[i]+"=="+valuedata1[i]);
                            }

                            barChart.data.labels = labeldata1
                            barChart.data.datasets = [{
                                label: "Reading",
                                backgroundColor: "#578503"/* ["#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f"]*/,
                                data: [valuedata1[0], valuedata1[1], valuedata1[2], valuedata1[3], valuedata1[4], valuedata1[5], valuedata1[6], valuedata1[7], valuedata1[8]] //[10, 25, 45, 84, 4]
                            }]
                            barChart.update()
                        }
                    
                    })
                }

          //On Change Loading
          $("#_ctl0_ContentPlaceHolder1_ddlCenters").change(function () {
              //alert($('#_ctl0_ContentPlaceHolder1_ddlCenters').val());
              var Classid1 = $('#_ctl0_ContentPlaceHolder1_ddlCenters').val();
          
              if ($('#_ctl0_ContentPlaceHolder1_ddlCenters').val() == Classid1) {
                  $.ajax({
                      type: "POST",
                      url: "admin.aspx/ReadingBarChart",
                      data: JSON.stringify({ Classid1: Classid1 }),
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      error: function (XMLHttpRequest, textStatus, errorThrown) {
                          alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                      },
                      success: function (result) {
                          var result1 = result.d;
                         var labeldata1 = [];
                          var valuedata1 = [];
                          for (var i = 0; i < result1.length; i++) {
                              labeldata1[i] = result1[i].split("_")[0];
                              valuedata1[i] = result1[i].split("_")[1];
                              //alert(labeldata1[i]+"=="+valuedata1[i]);
                          }
                          
                          barChart.data.labels=labeldata1
                          barChart.data.datasets = [{
                              label: "Reading",
                              backgroundColor:"#578503"/* ["#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f"]*/,
                              data: [valuedata1[0],valuedata1[1],valuedata1[2],valuedata1[3],valuedata1[4],valuedata1[5], valuedata1[6],valuedata1[7],valuedata1[8],0] //[10, 25, 45, 84, 4]
                          }]
                          barChart.update()
                      }
                  })
              }                     
          })
      })
  </script>

 <%--Listening Bar Chart--%>
        <script>
            $(document).ready(function () {
                var areaChartData =
                {
                    labels: [],
                    datasets: [{},]
                }

                var barChartCanvas = $('#listeningbarChart').get(0).getContext('2d')
                var barChartData = jQuery.extend(true, {}, areaChartData)
                var temp0 = areaChartData.datasets[0]
                //var temp1 = areaChartData.datasets[1]
                barChartData.datasets[0] = temp0
                //barChartData.datasets[1] = temp0
                var barChartOptions = {
                    scales: {

                        yAxes: [{
                            ticks: {

                                min: 0,
                                max: 100,
                                callback: function (value) { return value + "%" }
                            },
                            scaleLabel: {
                                display: true,
                                //labelString: "Percentage"
                            }
                        }]
                    },

                    responsive: true,
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                return data['labels'][tooltipItem['index']] + ': ' + data['datasets'][0]['data'][tooltipItem['index']] + '%';
                            }
                        }
                    }
                }
                var barChart = new Chart(barChartCanvas, {
                    type: 'bar',
                    data: barChartData,
                    options: barChartOptions
                })

                var Classid1 = $('#_ctl0_ContentPlaceHolder1_ddlCenters').val();
                //alert(Cls);
                if (Classid1 == Classid1)
                {
                    $.ajax({
                        type: "POST",
                        url: "admin.aspx/ListeningBarChart",
                        data: JSON.stringify({ Classid1: Classid1 }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                        },
                        success: function (result) {
                            var result1 = result.d;
                            var labeldata1 = [];
                            var valuedata1 = [];
                            for (var i = 0; i < result1.length; i++) {
                                labeldata1[i] = result1[i].split("_")[0];
                                valuedata1[i] = result1[i].split("_")[1];
                                //alert(labeldata1[i]+"=="+valuedata1[i]);
                            }

                            barChart.data.labels = labeldata1
                            barChart.data.datasets = [{
                                label: "Listening",
                                backgroundColor: "#A8094B"/* ["#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f", "#3cba9f"]*/,
                                data: [valuedata1[0], valuedata1[1], valuedata1[2], valuedata1[3], valuedata1[4], valuedata1[5], valuedata1[6], valuedata1[7], valuedata1[8]] //[10, 25, 45, 84, 4]
                            }]
                            barChart.update()
                        }
                    
                    })
                }
 
          $("#_ctl0_ContentPlaceHolder1_ddlCenters").change(function () {
              //alert($('#_ctl0_ContentPlaceHolder1_ddlCenters').val());
              var Classid1 = $('#_ctl0_ContentPlaceHolder1_ddlCenters').val();
          
              if ($('#_ctl0_ContentPlaceHolder1_ddlCenters').val() == Classid1) {
                  $.ajax({
                      type: "POST",
                      url: "admin.aspx/ListeningBarChart",
                      data: JSON.stringify({ Classid1: Classid1 }),
                      contentType: "application/json; charset=utf-8",
                      dataType: "json",
                      error: function (XMLHttpRequest, textStatus, errorThrown) {
                          alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                      },
                      success: function (result) {
                          var result1 = result.d;
                         var labeldata1 = [];
                          var valuedata1 = [];
                          for (var i = 0; i < result1.length; i++) {
                              labeldata1[i] = result1[i].split("_")[0];
                              valuedata1[i] = result1[i].split("_")[1];
                              //alert(labeldata1[i]+"=="+valuedata1[i]);
                          }
                          
                          barChart.data.labels=labeldata1
                          barChart.data.datasets = [{
                              label: "Listening",
                              backgroundColor:"#A8094B",
                              data: [valuedata1[0],valuedata1[1],valuedata1[2],valuedata1[3],valuedata1[4],valuedata1[5], valuedata1[6],valuedata1[7],valuedata1[8]] //[10, 25, 45, 84, 4]
                          }]
                          barChart.update()
                      }
                  })
              }                     
          })

      })
  </script>
</asp:Content>
