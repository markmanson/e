<%@ Page AutoEventWireup="false" Inherits="Unirecruite.unirecruite._error" Language="vb"
    CodeBehind="error.aspx.vb" MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution------Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--<form id="form1" runat="server">--%>        
     <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>StudentTimeInfo</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/adminlte.min.css">
    <style type="text/css">
        .contentcenter {
          position: absolute;
          margin-top:25%;
          height:auto;
        }
    </style>
    <!-- Google Font: Source Sans Pro -->
  <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.1/jquery.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
  <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <div class="container">
        <div class="row">
            <div class="col-sm-12 d-flex justify-content-center contentcenter">
                <strong style="color:#128b82; font-size:20px"><%=Resources.Resource.Error_MSG %></strong>
            </div>
        </div>
    </div>
<%--</form>--%>
</asp:Content>
