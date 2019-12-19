<%@ Page ClientTarget="UpLevel" AutoEventWireup="false" Inherits="Unirecruite.Sample1" MasterPageFile="~/MasterPage.Master" Language="vb" CodeBehind="Sample1.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta content="True" name="vs_showGrid">

    <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <title>OES</title>
  <!-- Tell the browser to be responsive to screen width -->
  <meta name="viewport" content="width=device-width, initial-scale=1">

  <!-- Font Awesome -->
  <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
  <!-- Ionicons -->
  <!-- Theme style -->
  <link rel="stylesheet" href="dist/css/adminlte.min.css">
  <!-- Google Font: Source Sans Pro -->
  <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet">
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
  <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
    <script src="js/datepicker.min.js" type="text/javascript"></script>
    <link href="css/datepicker.min.css" rel="stylesheet" type="text/css" />

    <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <title>OES</title>
  <!-- Tell the browser to be responsive to screen width -->
  <meta name="viewport" content="width=device-width, initial-scale=1">

  <!-- Font Awesome -->
  <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
  <!-- Ionicons -->
  <!-- Theme style -->
  <link rel="stylesheet" href="dist/css/adminlte.min.css">
  <!-- Google Font: Source Sans Pro -->
  <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet">
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
  <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
    <script src="https://unpkg.com/gijgo@1.9.13/js/gijgo.min.js" type="text/javascript"></script>
    <link href="https://unpkg.com/gijgo@1.9.13/css/gijgo.min.css" rel="stylesheet" type="text/css" />

  <!-- Content Wrapper. Contains page content -->
<%--  <div class="content-wrapper">--%>
    <!-- Content Header (Page header) -->
    <section class="content-header">
      <div class="container-fluid">
        <div class="row">
          <div class="col-sm-12">
            <h1>User Registration Details</h1>
          </div>
        </div>
      </div><!-- /.container-fluid -->
    </section>

    <!-- Main content -->
    <section class="content">
      <div class="container-fluid">
        <div class="row">
          <!-- left column -->
          <div class="col-md-6">
            <!-- general form elements -->
            <div class="card card-info">
              <div class="card-header" style="height: 40px">
                <h1 class="card-title">User Details</h1>
             </div>
              <!-- /.card-header -->
              <!-- form start -->
              <form role="form">
                <div class="card-body">
                  <div class="form-group">
                        <label>User Type</label>
                        <select class="form-control">
                          <option>Student</option>
                          <option>Admin</option>
                        </select>
                   </div>
                  <div class="form-group">
                    <label for="exampleInputFirstName1">First Name </label>
                    <input type="text" class="form-control" id="exampleInputFirstName1" placeholder=" First Name">
                  </div>
                   <div class="form-group">
                    <label for="exampleInputMiddleName1">Middle Name </label>
                    <input type="email" class="form-control" id="exampleInputMiddleName1" placeholder=" Middle Name">
                  </div>
                   <div class="form-group">
                    <label for="exampleInputLastName1">Last Name </label>
                    <input type="email" class="form-control" id="exampleInputLastName1" placeholder=" Last Name">
                  </div>
                  <div>
                          <label> Date of Birth :</label>
                        <input id="datepicker" width="276" />
                        <script>
                         $('#datepicker').datepicker({
                         uiLibrary: 'bootstrap4'
                         });
                         </script>
                  </div>

                  <div class="form-group">
                    <label for="exampleInputLastName1">Gender</label>
                        <div class="form-check">
                          <input class="form-check-input" type="radio" name="radio1">
                          <label class="form-check-label" style="margin-right: 20px">Male</label>

                          <input class="form-check-input" type="radio" name="radio1" >
                          <label class="form-check-label">Female</label>
                        </div>
                      </div>

                  <div class="form-group">
                    <label for="exampleInputEmail1">Email address</label>
                    <input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">
                  </div>
                  <div class="form-group">
                    <label for="exampleInputTelephone1">Telephone</label>
                    <input type="text" class="form-control" id="exampleInputTelephone1" placeholder="Telephone Number">
                  </div>
                  <div class="form-group">
                    <label for="exampleInputRollNumber1">Address :</label>
                    <!--<input type="textarea" class="form-control" id="exampleInputAddress1" placeholder="Address" name="Address"> -->
                    <textarea class="form-control" id="exampleInputAddress1" placeholder="Address" name="Address" maxlength="100" ></textarea>
                  </div>
                  <div class="form-group">
                    <label for="exampleInputFile">Upload Photo</label>
                    <div class="input-group">
                      <div class="custom-file">
                        <input type="file" class="custom-file-input" id="exampleInputFile">
                        <label class="custom-file-label" for="exampleInputFile">Choose file</label>
                      </div>
                      <div class="input-group-append">

                      </div>
                    </div>
                  </div>
                </div>
                <!-- /.card-body -->
              </form>
            </div>
            <!-- /.card -->
          </div>
          <!--/.col (left) -->
          <!-- right column -->
          <div class="col-md-6">
            <!-- general form elements disabled -->
            <div class="card card-info">
              <div class="card-header" style="height: 40px">
                <h1 class="card-title">Class Details</h1>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
                <form role="form">
                  <div class="row">
                    <div class="col-md-12">
                      <!-- text input -->

                       <div class="form-group">
                    <label for="exampleInputRollNumber1">Roll Number :</label>
                    <input type="text" class="form-control" id="exampleInputRollNumber1" placeholder="Roll Number">
                  </div>

                       <div class="form-group">
                        <label>Class Name </label>
                        <select class="form-control">
                          <option>Class Name 1</option>
                          <option>Class Name 2</option>
                          <option>Class Name 3</option>
                          <option>Class Name 4</option>
                          <option>Class Name 5</option>
                          <option>Class Name 6</option>

                        </select>
                      </div>

                    </div>
                  </div>
                </form>
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
            <!-- general form elements disabled -->
            <div class="card card-info">
              <div class="card-header" style="height: 40px">
                <h1 class="card-title">Login Details</h1>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
                <form role="form">
                  <div class="row">
                    <div class="col-md-12">

                      <!-- Select multiple-->
                      <div class="form-group">
                    <label for="exampleInputLoginIDName1">Login ID </label>
                    <input type="text" class="form-control" id="exampleInputLoginID1" placeholder=" Login ID">
                  </div>
                    <div class="form-group">
                      <label for="exampleInputPassword1">Password</label>
                       <input type="password" class="form-control" id="exampleInputPassword1" placeholder="Password">
                  </div>
                   <div class="form-group">
                       <label for="exampleInputConfirmPassword1">Confirm Password</label>
                        <input type="password" class="form-control" id="exampleInputConfirmPassword1" placeholder="Confirm Password">
                  </div>
                    </div>
                  </div>

                </form>
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
          </div>
          <!--/.col (right) -->
        </div>
        <div class="row">
          <button type="submit" class="btn btn-primary" style="margin-right:10px;">Submit</button>
          <button type="reset" class="btn btn-primary">Clear</button>
        </div>
        <!-- /.row -->
      </div><!-- /.container-fluid -->
    </section>
    <!-- /.content -->
<%--  </div>--%>

    
    <!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<!-- Bootstrap 4 -->
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<!-- AdminLTE App -->
<script src="dist/js/adminlte.min.js"></script>
<!-- AdminLTE for demo purposes -->
<script src="dist/js/demo.js"></script>
</asp:Content>