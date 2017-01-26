﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebConsultantAgregar.aspx.cs" Inherits="WebConsultantAgregar" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/Control/WebPie.ascx" TagName="PiePagina" TagPrefix="usrpagina" %>
<%@ Register Src="~/Control/WebPieJs.ascx" TagName="PiePaginaJs" TagPrefix="usrpagina" %>
<%@ Register Src="~/Control/WebMenu.ascx" TagName="Menu" TagPrefix="usrpagina" %>
<%@ Register Src="~/Control/WebNav.ascx" TagName="Nav" TagPrefix="usrpagina" %>
<%@ Register Src="~/Control/WebCabecera.ascx" TagName="Cabecera" TagPrefix="usrpagina" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <!--Cabecera-->
   <usrpagina:Cabecera ID="usrCabecera" runat="server" />
   <!--Cabecera-->   
    <style type="text/css">
        .pageView
        {
            border: 1px solid #898c95;
            /*border-top: none;*/
            margin-top: -1px;
            height: 730px;
        }
        .pageView img
        {
            margin: 0;
        }
        
        .control-label-sec
        {
            float: left;
            padding-top: 5px;
            text-align: right;
            width:100px;
        }
        
        .rlbText
        {
            padding-left:5px;
        }
        
        .module
        {
            margin-bottom: 1em;
        }
        
        .RadListBox
        {
            margin:0 auto !important;           
        }
          
         
       .RadUpload input.ruFakeInput
        {
            display: none;
        }
        .RadUpload input.ruBrowse
        {
            width: 115px; /*115px*/
        }
        .RadUpload span.ruFileWrap input.ruButtonHover
        {
            background-position: 100% -46px;
        }
        .RadUpload input.ruButton
        {
            background-position: 0 -46px;
        }
        
        .RadUpload span.ruUploadProgress 
        {
            width:100px;     
        }
        
        .RadUpload span.ruUploadSuccess
        {
              width:100px; 
        }
        
        .invalid
        {
            color:Red;
        }
        
        .binary-image
        {
            margin-bottom:5px;
        }
        
        .rbDecorated
        {
            padding-bottom:2px;
            margin-left:4px;
            width:111px;
        }
 
        
        @media screen and (-webkit-min-device-pixel-ratio:0) {
         #RadButton1 { padding-right: 4px }
        }
        
        .item, .alternatingItem
        {
            float: left;
            padding: 5px;
            margin: 5px;
            width: 270px;
            /*height: 100px;*/
            /*border: 1px solid threedshadow;*/
        }      

    </style>
    
    <script language="javascript" type="text/javascript">

        function openCopiar() {
            $find("winCopiar").show();
        }

        function cerrarCopiar() {
            $find("winCopiar").close();
        }


        // imagen
        function validationOk(sender, args) {
            $("#errorupload").css("display", "none");

        }

        function validationFailed(sender, args) {
            $("#errorupload").css("display", "block");
            sender.deleteFileInputAt(0);
        }

        // certificado
        function validationOkCer(sender, args) {
            $("#errorcerupload").css("display", "none");
        }

        function validationFailedCer(sender, args) {
            $("#errorcerupload").css("display", "block");
            sender.deleteFileInputAt(0);
        }

        // cv
        function validationOkCV(sender, args) {
            $("#errorcvupload").css("display", "none");
        }

        function validationFailedCV(sender, args) {
            $("#errorcvupload").css("display", "block");
            sender.deleteFileInputAt(0);
        }
    </script>
    

</head>
<body>
    <form id="form1" runat="server">
    <!--Menu-->
    <usrpagina:Menu ID="usrMenu" runat="server" />
    <!--Menu-->
    <div class="container-fluid" style="min-width:940px">
        <usrpagina:Nav ID="usrNav" runat="server" />

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>        
            <telerik:AjaxSetting AjaxControlID="gridUsuario">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gridUsuario"   LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnUsuario" EventName="btnUsuario_Click" >
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlCopiar" />
                    <telerik:AjaxUpdatedControl ControlID="pnlActivo"   LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server"  LoadingPanelID="RadAjaxLoadingPanel1">
        
       <div class="row-fluid">
       
        <div class="span12">
            <div class="widget-box">
                <div class="widget-title">
		            <h5><asp:Label ID="lblAgregar" runat="server" meta:resourcekey="lblAgregar"></asp:Label><asp:Label ID="lblEditar" runat="server" meta:resourcekey="lblEditar" Visible="false"></asp:Label></h5>                    
                </div>
                <div class="widget-content">
               
                   
                <asp:Panel ID="pnlActivo" runat="server">
                    
                 <asp:Label ID="lblCodigo" runat="server" Text="0" Visible="false"></asp:Label>
                  <asp:Label ID="lblCodigoUsuario" runat="server" Text="0" Visible="false"></asp:Label>
                   <asp:Label ID="lblTipoConsultor" runat="server" Text="0" Visible="false"></asp:Label>
                
                
                 <asp:Panel ID="pnlOk" runat="server" Visible="false">
                    <div class="alert alert-success">
                        <button type="button" class="close" data-dismiss="alert">×</button> 
                        <strong>Ok!</strong>&nbsp;<asp:Literal ID="litOk" runat="server"></asp:Literal>                        
                    </div>
                 </asp:Panel>

                 <asp:Panel ID="pnlError" runat="server" Visible="false">
                    <div class="alert alert-error">
                        <button type="button" class="close" data-dismiss="alert">×</button> 
                        <strong>Oh Error!</strong>
                        <asp:Literal ID="litError" runat="server"></asp:Literal>
                    </div>        
                 </asp:Panel>                 
                 
                   
                   <telerik:RadTabStrip ID="RadTabConsultor" runat="server" MultiPageID="RadMultiPage1"
                          SelectedIndex="0" Align="Justify" ReorderTabsOnSelect="True" Width="500px">
                        <Tabs>
                            <telerik:RadTab Text="Informacion General" Selected="True" meta:resourcekey="tabInfo">
                            </telerik:RadTab>
                            <telerik:RadTab Text="Educacion"  meta:resourcekey="tabEdu">
                            </telerik:RadTab>
                            <telerik:RadTab Text="Experiencia"  meta:resourcekey="tabExperiencia">
                            </telerik:RadTab>
                            <telerik:RadTab Text="Idioma"  meta:resourcekey="tabIdioma">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>

                <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0"  CssClass="pageView"
                Width="940px">
                    <telerik:RadPageView ID="RadPageView1" runat="server">
                      <div class="form-horizontal" style="padding-top:20px;">

                       <div style="float:left; width:750px;">
                        <div class="control-group">
                            <span class="control-label">   
                                <asp:Label ID="lblTipo" Text="Tipo" runat="server" CssClass="pull-right" meta:resourcekey="lblTipo" 
                                    AssociatedControlID="rbInterno"></asp:Label>
                                <span class="pull-right requerido">*</span>    
                            </span>
                            <div class="controls">
                                <asp:RadioButton ID="rbInterno" runat="server"  CssClass="radio inline" 
                                    Text="Interno" GroupName="GpoTipo" meta:resourcekey="rbInterno" 
                                    AutoPostBack="True" Checked="True" 
                                    oncheckedchanged="rbInterno_CheckedChanged"   />
                                <asp:RadioButton ID="rbExterno" runat="server"  CssClass="radio inline"
                                    Text="Externo" GroupName="GpoTipo" meta:resourcekey="rbExterno" 
                                    AutoPostBack="True" oncheckedchanged="rbExterno_CheckedChanged"   />
                              
                            </div>                        
                        </div>

                                         <asp:Panel ID="pnlDA" runat="server" Visible="true">                        
                        <div class="control-group">
                            <span class="control-label">  
                                <asp:Label ID="lblDA" runat="server" Text="Usuario DA" CssClass="pull-right" meta:resourcekey="lblDA" 
                                AssociatedControlID="cboDA"></asp:Label>
                              <span class="pull-right requerido">*</span>    
                            </span>
                            <div class="controls">
                                <telerik:RadComboBox ID="cboDA" runat="server" Filter="Contains" Width="210px" AutoPostBack="true"
                                    onselectedindexchanged="cboDA_SelectedIndexChanged" >    
                                </telerik:RadComboBox>
                                  &nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkUsuario" runat="server" CssClass="btn btn-mini"  OnClientClick="openCopiar(); return false;" >
                                    <i class="icon-search"></i>
                                    <asp:Label ID="lblUsuario" runat="server" Text="Buscar" meta:resourcekey="lblUsuario" ></asp:Label>
                                </asp:LinkButton>
                                  &nbsp;
                                 <asp:LinkButton ID="lnkImportar" runat="server" onclick="lnkImportar_Click" CssClass="btn btn-mini" >
                                    <asp:Label ID="lblImportar" runat="server" Text="lblImportar"  meta:resourcekey="lblImportar"></asp:Label>
                                </asp:LinkButton>     
                            </div>                        
                        </div>
                       </asp:Panel>

                        <div class="control-group">
                        <span class="control-label">                               
                            <asp:Label ID="lblApellido" runat="server" Text="* Apellido" 
                                CssClass="pull-right" meta:resourcekey="lblApellido" 
                                AssociatedControlID="txtApellido"></asp:Label>
                            <span class="pull-right requerido">*</span>    
                        </span>
                        <div class="controls">
                            <telerik:RadTextBox ID="txtApellido" runat="server" Font-Size="Medium" CssClass="input-xlarge"
                                Height="28px" Width="350px" >                           
                            </telerik:RadTextBox>
                            
                        </div>                        
                        </div>

                        <div class="control-group">
                        <span class="control-label">
                            <asp:Label ID="lblNombre" runat="server" Text="* Nombre" 
                                    CssClass="pull-right" meta:resourcekey="lblNombre" 
                                AssociatedControlID="txtNombre"></asp:Label>
                            <span class="pull-right requerido">*</span>    
                        </span>
                        <div class="controls">
                            <telerik:RadTextBox ID="txtNombre" runat="server" Font-Size="Medium" 
                                Height="28px" Width="350px" >                           
                            </telerik:RadTextBox>                            
                            
                        </div>                        
                        </div>

                        <div>
                        
                       <div style="float:left; width:400px;">
                            <div class="control-group">
                            <span class="control-label">                         
                                <asp:Label ID="lblFecha" runat="server" Text="* Fecha de Nacimiento" 
                                    CssClass="pull-right" meta:resourcekey="lblFecha" 
                                    AssociatedControlID="txtFechaNacimiento"></asp:Label>
                                 <span class="pull-right requerido">*</span>    
                            </span>
                             
                            <div class="controls">
                             
                                <telerik:RadMaskedTextBox ID="txtFechaNacimiento" runat="server" EmptyMessage="dd/MM/yyyy"
                                    Mask="## / ## / ####" Height="28px" DisplayPromptChar=""  Width="90px"
                                    TextWithLiterals="">
                                </telerik:RadMaskedTextBox>
                               
                                <span class="help-block"> 
                                    <asp:Label ID="lblMensajeFecha" runat="server" Text="Label" 
                                        meta:resourcekey="lblMensajeFecha" Font-Italic="True"
                                        Font-Bold="true"></asp:Label>                             
                                </span>
                            </div>

                           
                               
                            </div>
                       </div>  
                        <div style="float:left; ">
                        
                            <div class="control-group">
                            <span class="control-label-sec">
                                <asp:Label ID="lblSexo" runat="server" Text="*Sexo" 
                                       CssClass="pull-right" meta:resourcekey="lblSexo" 
                                    AssociatedControlID="cboSexo"></asp:Label>
                                <span class="pull-right requerido">*</span>    
                            </span>
                            <div class="controls" style="margin-left: 120px;">
                                <asp:DropDownList ID="cboSexo" runat="server">
                                </asp:DropDownList>
                            </div>
                            </div>
                          </div>
                        <div style="clear:both"></div>
                        </div>

                        <div>
                        <div style="float:left; width:400px;">
                            <div class="control-group">
                            <span class="control-label">
                                <asp:Label ID="lblNacionalidad1" runat="server" Text="* Nacionalidad 1" 
                                    CssClass="pull-right" meta:resourcekey="lblNacionalidad1" 
                                    AssociatedControlID="cboNacionalidad1"></asp:Label>                                
                                <span class="pull-right requerido">*</span>    
                            </span>
                            <div class="controls">
                                <asp:DropDownList ID="cboNacionalidad1" runat="server">
                                </asp:DropDownList>               
                            </div>
                            </div>
                        </div>
                        <div style="float:left; ">
                            <div class="control-group">
                            <span class="control-label-sec">
                                <asp:Label ID="lblNacionalidad2" runat="server" Text="Nacionalidad 2" 
                                    CssClass="pull-right" meta:resourcekey="lblNacionalidad2" 
                                    AssociatedControlID="cboNacionalidad2"></asp:Label>                              
                            </span>
                            <div class="controls" style="margin-left: 120px;">
                                <asp:DropDownList ID="cboNacionalidad2" runat="server">
                                </asp:DropDownList>
                            </div>
                            </div>
                        </div>
                        <div style="clear:both"></div>
                        </div>  

                         <div class="control-group">
                        <asp:Label ID="lblDireccion" runat="server" Text="Direccion" 
                        CssClass="control-label" meta:resourcekey="lblDireccion" 
                            AssociatedControlID="txtDireccion"></asp:Label>
                        <div class="controls">                           
                            <asp:TextBox ID="txtDireccion" runat="server" TextMode="MultiLine"  Width="330px" ></asp:TextBox>              
                        </div>                        
                        </div>

                        <div>
                        <div style="float:left; width:400px;">
                            <div class="control-group">
                            <span class="control-label">
                                <asp:Label ID="lblPais" runat="server" Text="* Pais" 
                                    CssClass="pull-right" meta:resourcekey="lblPais" 
                                    AssociatedControlID="cboPais"></asp:Label>
                                <span class="pull-right requerido">*</span>  
                            </span>
                            <div class="controls">
                                    <asp:DropDownList ID="cboPais" runat="server" >
                                </asp:DropDownList>               
                            </div>
                            </div>
                        </div>
                        <div style="float:left; ">
                            <div class="control-group">
                            <asp:Label ID="lblCiudad" runat="server" Text="Ciudad" 
                                    CssClass="control-label-sec" meta:resourcekey="lblCiudad" 
                                AssociatedControlID="txtCiudad"></asp:Label>
                            <div class="controls" style="margin-left: 120px;">
                                   <telerik:RadTextBox ID="txtCiudad" runat="server" Font-Size="Medium" 
                                        Height="28px" Width="210px" >                           
                                    </telerik:RadTextBox>   
                            </div>
                            </div>
                        </div>
                        <div style="clear:both"></div>
                        </div>
                        
                        <div>
                        <div style="float:left; width:400px;">
                            <div class="control-group">
                            <asp:Label ID="lblTelefono1" runat="server" Text="Telefono 1" 
                                    CssClass="control-label" meta:resourcekey="lblTelefono1" 
                                AssociatedControlID="txtTelefono1"></asp:Label>
                            <div class="controls">
                                <telerik:RadTextBox ID="txtTelefono1" runat="server" Font-Size="Medium" 
                                Height="28px" Width="210px" >                           
                            </telerik:RadTextBox>      
                            </div>
                            </div>
                        </div>
                        <div style="float:left; ">
                            <div class="control-group">
                            <asp:Label ID="lblTelefono2" runat="server" Text="Telefono 2" 
                                    CssClass="control-label-sec" meta:resourcekey="lblTelefono2" 
                                AssociatedControlID="txtTelefono2"></asp:Label>
                            <div class="controls" style="margin-left: 120px;">
                                   <telerik:RadTextBox ID="txtTelefono2" runat="server" Font-Size="Medium" 
                                Height="28px" Width="210px" >                           
                            </telerik:RadTextBox>     
                            </div>
                            </div>
                        </div>
                        <div style="clear:both"></div>
                        </div>


                        <div class="control-group">
                        <asp:Label ID="lblLinked" runat="server" Text="LinkedIn URL" 
                        CssClass="control-label" meta:resourcekey="lblLinked" 
                            AssociatedControlID="txtLinked"></asp:Label>
                        <div class="controls">
                            <telerik:RadTextBox ID="txtLinked" runat="server" Font-Size="Medium" CssClass="input-xlarge"
                                Height="28px" Width="350px" >                           
                            </telerik:RadTextBox>        
                           
                            <span class="help-block"> 
                                <asp:Label ID="lblMensajeLinked" runat="server" Text="Label" 
                                    meta:resourcekey="lblMensajeLinked" Font-Italic="True"
                                    Font-Bold="true"></asp:Label>                             
                            </span>
                                                                  
                        </div>                        
                        </div>
                            
                         <div style="width: 930px;">
                            <div style="float: left; width: 600px;">
                           
                         
                         <asp:Panel ID="Panel1" runat="server">                        
                       
                           <div class="control-group">
                           <span class="control-label">   
                                <asp:Label ID="lblIdioma" runat="server" Text="Idioma" 
                                        CssClass="pull-right" meta:resourcekey="lblIdioma" 
                                    AssociatedControlID="cboIdioma"></asp:Label>
                                 <span class="pull-right requerido">*</span>    
                            </span>
                           <div class="controls">
                                <asp:DropDownList ID="cboIdioma" runat="server">
                                </asp:DropDownList>
                                <span class="help-block"> 
                                    <asp:Label ID="lblMensajeIdioma" runat="server" Text="Label" 
                                        meta:resourcekey="lblMensajeIdioma" Font-Italic="True"
                                        Font-Bold="true"></asp:Label>                             
                                </span>
                            </div>
                            </div>
                         </asp:Panel>

                        <div class="control-group">
                        <span class="control-label">  
                            <asp:Label ID="lblCorreo" runat="server" Text="* Correo" 
                             CssClass="pull-right" meta:resourcekey="lblCorreo" 
                                AssociatedControlID="txtCorreo"></asp:Label>
                            <span class="pull-right requerido">*</span>    
                         </span>
                        <div class="controls">
                            <telerik:RadTextBox ID="txtCorreo" runat="server" Font-Size="Medium" CssClass="input-xlarge"
                                Height="28px" Width="350px" >                           
                            </telerik:RadTextBox>                                              
                        </div>                        
                        </div>

                       <asp:Panel ID="pnlTipo" runat="server">      
                      
      

                       <asp:Panel ID="pnlContrasenia" runat="server" Visible="false">                        
                        <div class="control-group">
                            <span class="control-label">  
                                <asp:Label ID="lblContra" runat="server" Text="* Contraseña" 
                                    CssClass="pull-right" meta:resourcekey="lblContra" 
                                    AssociatedControlID="txtContra"></asp:Label>
                                <span class="pull-right requerido">*</span>    
                            </span>
                            <div class="controls">
                                <div class="pull-left">                              
                                   <asp:TextBox ID="txtContra" runat="server" CssClass="input-medium" 
                                        Text="" TextMode="Password" ViewStateMode="Enabled"></asp:TextBox> 
                                </div>
                                <div class="pull-left" style="padding-left:10px">                            
                                     <asp:CheckBox ID="chkContra" runat="server" Text="Actualizar"  
                                        CssClass="checkbox inline"  meta:resourcekey="chkContra" AutoPostBack="True" 
                                        oncheckedchanged="chkContra_CheckedChanged"/>
                                     <asp:HiddenField ID="hdnContra" runat="server" />

                                </div>
                            </div>                        
                        </div>  
                        
                        <div class="control-group">
                            <span class="control-label">
                                <asp:Label ID="lblContraseniaRep" runat="server" Text="* Contraseña"
                                    CssClass="pull-right" meta:resourcekey="lblContraRep" 
                                    AssociatedControlID="txtContraRep"></asp:Label>
                                <span class="pull-right requerido">*</span>    
                            </span>
                            <div class="controls">
                                <asp:TextBox ID="txtContraRep" runat="server" CssClass="input-medium"  TextMode="Password" Text="" ></asp:TextBox>&nbsp;&nbsp;
                             <asp:HiddenField ID="hdnContraRep" runat="server" />

                            </div>                        
                        </div>
                       </asp:Panel>

                       </asp:Panel>
                       </div>
                            <div style="float:right ;width: 330px;">
                                <div style="padding-left:40px;padding-top:30px;">
                                    
                                    <asp:Image ID="imgLogo" runat="server" ImageUrl="~/Imagen/logoportada.gif" Width="280px" Height="120px"  /> 
                                </div>
                            </div>
                            <div style="clear:both"></div>
                         </div>
                      </div>
                       <div style="float:left; width:190px;">
                          <div style="text-align:center">
                              <asp:Label ID="lblFoto" runat="server" Text="Foto" meta:resourcekey="lblFoto" ></asp:Label>
                          </div>
                          <div style="text-align:center;padding-top:10px;">
                            <asp:Image ID="imgConsultor" runat="server" Width="150px" Height="150px" ImageUrl="~/Imagen/usuario.png" CssClass="img-polaroid"/>
                          </div>

                          <div id="errorupload" style="display:none;">
                               <asp:Label ID="lblMensajeUpload" runat="server" Text="dfdffd" ForeColor="#FF0000"
                               meta:resourcekey="lblMensajeUpload"></asp:Label>                               
                          </div>       

                          <div style="text-align:center;padding-top:10px;">
                                               
                            <telerik:RadAsyncUpload ID="RadUpFoto" runat="server"  meta:resourcekey="RadUpFoto"
                                AllowedFileExtensions="jpeg,jpg,png" AutoAddFileInputs="False"
                                OnClientValidationFailed="validationFailed" OnClientFileUploading="validationOk" TemporaryFileExpiration="05:00:00">
                              <Localization Select="Choose Avatar" Cancel="Cancel" Remove="Remove" />
                            </telerik:RadAsyncUpload>     
                            
                            <div>
                                <asp:Label ID="lblFotoDescarga" runat="server" Visible="false"></asp:Label>
                                <asp:HyperLink ID="lnkFDescargar" runat="server"  Visible="false" ToolTip="" NavigateUrl="">
                                    <asp:Label ID="lblFDescarga" runat="server" Visible="true"></asp:Label>
                                </asp:HyperLink>
                            </div>                     
                          </div>                          
                      </div>
                      <div style="clear:both"></div>
                      </div>
                    </telerik:RadPageView>
                            
                    <telerik:RadPageView ID="RadPageView2" runat="server">
                        <div style="padding-top:10px;padding-left:10px;padding-right:10px;padding-bottom:5px">
                        <h5>
                            <asp:Label ID="lblEducacion" runat="server" Text="Educacion" meta:resourcekey="lblEducacion" ></asp:Label>                            
                             &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkAgregarEducacion" runat="server" 
                                 onclick="lnkAgregarEducacion_Click"><i class="icon-plus"></i><asp:Label ID="lblAEducacion" runat="server" Text="Agregar Educacion" meta:resourcekey="lblAEducacion"></asp:Label>
                                </asp:LinkButton>
                        </h5> <hr />
                         <asp:Label ID="lblMEducacion" runat="server" Text="Educacion" Visible="false"
                                meta:resourcekey="lblMEducacion" Font-Bold="False" Font-Italic="True" ></asp:Label>
                       
                        </div>

                        <div id="errorcerupload" style="display:none;padding-left:10px;padding-right:10px;">
                             <asp:Panel ID="pnlCerErrorUpload" runat="server">
                                <div class="alert alert-error">                    
                                    <strong>Oh Error!</strong>
                                    <asp:Label ID="lblMensajeCerUpload" runat="server" meta:resourcekey="lblMensajeCerUpload" ></asp:Label>                      
                                </div>        
                             </asp:Panel> 
                        </div>
                        <div style="padding-left:10px;padding-right:10px;">
                             
                        <telerik:RadGrid ID="gridEduacion" runat="server" AutoGenerateColumns="False" HeaderStyle-Width="1500px"
                            CellSpacing="0" GridLines="None" Culture="es-ES" Width="920px"  
                            Height="190px" onitemcommand="gridEduacion_ItemCommand" 
                                onitemcreated="gridEduacion_ItemCreated"  CellPadding="0">

                        <ClientSettings EnableRowHoverStyle="true" ></ClientSettings>
                        <MasterTableView NoMasterRecordsText="No records to display." meta:resourcekey="lblNoDato" >
                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                Visible="True">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                Visible="True">
                            </ExpandCollapseColumn>
                            <Columns>
                                 <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                    UniqueName="temSel" HeaderText="" HeaderStyle-Width="20px">
                                    <ItemTemplate>
                                         <asp:LinkButton ID="lnkPEliminar" runat="server" meta:resourcekey="lnkEliminar" 
                                        CommandName="Eliminar" ToolTip=""
                                        CommandArgument='<%# Bind("Nro") %>'>
                                        <i class="icon-remove"></i>
                                        </asp:LinkButton>
                                         <asp:Label ID="lblNro" runat="server" Text='<%# Bind("Nro") %>' Visible="false"></asp:Label>
                                     </ItemTemplate>
                                     <HeaderStyle Width="20px" />
                                 </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                    UniqueName="temNivel" HeaderText="Nivel" meta:resourcekey="ColNivel" HeaderStyle-Width="180px">
                                     <ItemTemplate>
                                        <asp:DropDownList ID="cboNivel" runat="server" CssClass="input-medium">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblNivel" runat="server" Text='<%# Bind("oNivel.cod_nivaca_in") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblCodigo" runat="server" Text='<%# Bind("cod_conedu_in") %>' Visible="false"></asp:Label>
                                     </ItemTemplate>
                                     <HeaderStyle Width="180px" />
                                 </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                    UniqueName="temDescripcion"  HeaderText="Descripcion" meta:resourcekey="ColDescripcion" 
                                    HeaderStyle-Width="330px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# Bind("des_conedu_vc") %>' Width="300px"></asp:TextBox>                                                                           
                                    </ItemTemplate>
                                     <HeaderStyle Width="330px" />
                                 </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                    UniqueName="temDuracion" HeaderText="Duracion"  meta:resourcekey="ColDuracion" HeaderStyle-Width="160px">
                                    <ItemTemplate>

                                        <table border="0" >
                                        <tr>
                                            <td>
                                                <telerik:RadNumericTextBox runat="server"  Height="30px" MaxLength="2" CssClass="pagination-right" 
                                                  ID="txtDuracion" Width="30px"  DbValue='<%# Bind("can_dur_conedu_in")%>' MinValue="0" MaxValue="20"
                                                   Type="Number" SelectionOnFocus="SelectAll" NumberFormat-DecimalDigits="0" >
                                                </telerik:RadNumericTextBox>
                                                <asp:Label ID="lblDuracion" runat="server" Text='<%# Bind("tip_dur_conedu_in") %>' Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cboDuracion" runat="server" CssClass="input-small">
                                                </asp:DropDownList>
                                            </td>                                            
                                        </tr>
                                        </table> 
                                            
                                    </ItemTemplate>
                                     <HeaderStyle Width="160px" />
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                    UniqueName="temInstitucion" HeaderText="Institucion"  meta:resourcekey="ColInstitucion" 
                                    HeaderStyle-Width="450px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtInstitucion" runat="server" Text='<%# Bind("ins_conedu_vc") %>' Width="410px"></asp:TextBox>                                      
                                    </ItemTemplate>
                                    <HeaderStyle Width="450px" />
                                </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                    UniqueName="temCertificado" HeaderText="Certificado"  meta:resourcekey="ColCertificado" 
                                    HeaderStyle-Width="300px">
                                    <ItemTemplate>  
                                        <div>
                                        <div style="float:left;padding-left:5px;width:60px">         
                                            <asp:Label ID="lblDescargaN" runat="server" Visible="false" 
                                                    Text= '<%# Bind("adj_conedu_vc") %>'></asp:Label>                                   
                                            <asp:HyperLink ID="lnkDescargar" runat="server"  Visible="false" ToolTip='<%# Bind("adj_conedu_vc") %>'
                                                NavigateUrl='<%# "~/downloading.aspx?file="+Eval("adj_conedu_vc") %>' >
                                                    <asp:Label ID="lblDescarga" runat="server" Visible="true"
                                                        Text= '<%# Bind("adj_conedu_vc") %>'></asp:Label>
                                            </asp:HyperLink>
                                        </div>
                                        <div style="float:left;width:180px;padding-left:5px">
                                            <telerik:RadAsyncUpload ID="RadUpCertificado" runat="server"  meta:resourcekey="RadUpCertificado" 
                                                AllowedFileExtensions="doc,docx,pdf" AutoAddFileInputs="False" Width="220px"
                                                OnClientValidationFailed="validationFailedCer" OnClientFileUploading="validationOkCer">
                                                <Localization Select="Choose File" Remove="Remove"/>
                                            </telerik:RadAsyncUpload>     
                                        </div>
                                        <div style="clear:both"></div>                          
                                         </div>
                                            
                                    </ItemTemplate>
                                    <HeaderStyle Width="300px" />
                                </telerik:GridTemplateColumn>

                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                            
                        </MasterTableView>
                     
                    </telerik:RadGrid> 
                    </div>
                    
                    <div style="padding-top:10px;padding-left:10px;padding-right:10px;padding-bottom:5px">
                        <h5>
                            <asp:Label ID="lblHabilidades" runat="server" Text="Habilidades Técnicas" meta:resourcekey="lblHabilidades" ></asp:Label>                             
                        </h5>
                        <hr />
                        <asp:Label ID="lblMHabilidades" runat="server" Text="Educacion" Visible="false"
                                meta:resourcekey="lblMHabilidades" Font-Bold="False" Font-Italic="True" ></asp:Label>                       
                      
                    </div>

                    <div style="padding-left:10px;padding-right:10px;">

                     <div style="padding-left:20px; overflow:auto; overflow-x:hidden;overflow-y:scroll; width:98%;height: 300px;border:1px solid">
                       <telerik:RadListView ID="listHabilidades" runat="server"  Height="300px"
                       ItemPlaceholderID="itemPlaceholder" >
                       
                         <LayoutTemplate>
                            <fieldset style="width: 910px;" id="FieldSet1">                                 
                                <asp:Panel ID="itemPlaceholder" runat="server">
                                </asp:Panel>
                            </fieldset>
                        </LayoutTemplate>
                        <ItemTemplate>
                                  <div style="float: left;">
                                 <div style="width: 300px;padding-top:10px;height:430px;">
                                   <b>
                                     <div>
                                        <div style="float:left;padding-left:5px;">
                                        <asp:CheckBox ID="chkGeneral" runat="server"  CssClass="check inline"  /> 
                                        </div>
                                        
                                        <div style="float:left;padding-left:5px;" >                                        
                                        <asp:Label ID="lblGeneral" runat="server" Text='<%# Eval("nom_congen_vc")%>' Visible="true"></asp:Label>
                                            <asp:Label ID="lblCodigo" runat="server" Text='<%# Eval("cod_congen_in")%>' Visible="false"></asp:Label>
                                        </div>
                                        <div style="clear:both"></div>
                                     </div>
                                   </b>
                                      
                                    <div style="padding-top:10px;">
                                       <telerik:RadListBox ID="lstTecnico" runat="server" CheckBoxes="true" BorderWidth="0px" Width="270px" >                                  
                                       </telerik:RadListBox>                                    
                                    </div>
                                    </div>
                                    </div>
                           </ItemTemplate>

                       </telerik:RadListView>
                        </div>

                    </div>
                    
                    <div style="padding-left:10px;padding-right:10px;padding-top:10px">
                        <div class="alert">
                            <asp:Label ID="lblMensajeHabilidades" runat="server" Text="Label" 
                                meta:resourcekey="lblMensajeHabilidades" Font-Italic="True"
                                Font-Bold="true"></asp:Label>
                        </div>
                    </div>

                    </telerik:RadPageView>

                    <telerik:RadPageView ID="RadPageView3" runat="server" >
                        <div style="padding-top:10px;width: 100%;">
                        
                        <div style="width:50%; float:left">
                            <div style="padding-left:10px;padding-right:10px;">                           
                                <h5>
                                    <asp:Label ID="lblPaisesDesarrollo" runat="server" Text="Paises en Desarrollo" meta:resourcekey="lblPaisesDesarrollo" ></asp:Label>
                                </h5>
                                <hr />
                            </div>
                            
                            <div style="padding-left:10px;padding-right:0px;">    
                                <div>
                                    <div class="pull-left">                                                                                           
                                        <asp:Label ID="lblCertifico" Text="Certifico que tengo experiencia en paises en desarrollo"                   
                                            runat="server" meta:resourcekey="lblCertifico" ></asp:Label>            
                                    </div>
                                    
                                    <div class="pull-left" style="padding-left:10px;">
                                        <label class="radio"> 
                                            <asp:RadioButton ID="rbSI" runat="server" GroupName="GPResultado" 
                                            meta:resourcekey="rbSI"  Text="Si" /> 
                                         </label>
                                    </div>
                                    
                                    <div class="pull-left" style="padding-left:10px;">
                                        <label class="radio"> 
                                        <asp:RadioButton ID="rbNo" runat="server" GroupName="GPResultado" Checked="true"
                                        meta:resourcekey="rbNo"  Text="No" />
                                        </label>
                                    </div>
                                </div>   
                                    
                               
                            </div>
                               
                            <div style="padding-left:10px;padding-right:10px;padding-top:30px">
                                <div class="alert">
                                    <asp:Label ID="lblMensajeCertifico" runat="server" Text="Label"
                                        meta:resourcekey="lblMensajeCertifico"
                                         Font-Italic="True" Font-Bold="true"></asp:Label>
                                 </div>
                            </div>  
                          
                        </div>

                        <div style="width:50%; float:left">
                            <div style="padding-left:10px;padding-right:10px;">                                 
                                   <h5>
                                        <span class="requerido">*</span>
                                        <asp:Label ID="lblExperienciaTotal" runat="server" Text="Experiencia Total" meta:resourcekey="lblExperienciaTotal" ></asp:Label>
                                   </h5>
                                   <hr/>  
                            </div>
                            <div style="padding-left:10px;padding-right:10px;">
                                 <div style="width:100%">
                                 
                                 <div style="width:50%;float:left">
                                     <div style="padding-left:10px;padding-right:10px;">
                                        <label class="radio">
                                          <asp:RadioButton ID="rbExperiencia1" runat="server" Checked="true"
                                            Text="0 - 5 años" GroupName="GpoExperiencia" meta:resourcekey="rbExperiencia1" 
                                           />
                                        </label>
                                   </div>         
                                 </div>
                                 <div style="width:50%;float:left">
                                     <div style="padding-left:10px;padding-right:10px;">
                                        <label class="radio"> 
                                            <asp:RadioButton ID="rbExperiencia3" runat="server"   
                                            Text="11 - 15 años" GroupName="GpoExperiencia" meta:resourcekey="rbExperiencia3" 
                                            />
                                        </label>
                                  </div>
                                 </div>
                                 <div style="clear:both"></div>
                                 </div>
                                 
                                 <div style="width:100%">
                                 
                                 <div style="width:50%;float:left">
                                     <div style="padding-left:10px;padding-right:10px;">
                                        <label class="radio">
                                          <asp:RadioButton ID="rbExperiencia2" runat="server" 
                                            Text="6 - 10 años" GroupName="GpoExperiencia" meta:resourcekey="rbExperiencia2" 
                                           />
                                        </label>
                                 </div>         
                                 </div>
                                 <div style="width:50%;float:left">
                                     <div style="padding-left:10px;padding-right:10px;">
                                        <label class="radio"> 
                                            <asp:RadioButton ID="rbExperiencia4" runat="server"   
                                            Text="16 años a más" GroupName="GpoExperiencia" meta:resourcekey="rbExperiencia4" 
                                            />
                                        </label>
                                       </div>
                                 </div>
                                 <div style="clear:both"></div>
                                 </div>
                            </div>
                        </div>
                        <div style="clear:both"></div>
                        </div>


                       <div style="padding-top:10px;width: 100%">
                        
                        <div style="width:50%; float:left">
                            <div style="padding-left:10px;padding-right:10px;">                            
                                <h5><asp:Label ID="lblDonante" runat="server" Text="Cliente - Donante Experimientado" 
                                meta:resourcekey="lblDonante"></asp:Label></h5>
                                <hr/>
                            </div>   
                            <div style="padding-left:10px;padding-right:10px;">
                                <telerik:RadListBox ID="lstDonante" runat="server" CheckBoxes="true" Width="100%"
                                    Height="120px">                                  
                                </telerik:RadListBox>
                            </div>
                        </div>
                        <div style="width:50%; float:left">

                         <div style="padding-left:10px;padding-right:10px;">
                            <h5><asp:Label ID="lblExperienciaPrevia" runat="server" Text="Experiencia Previa (Paises)" 
                            meta:resourcekey="lblExperienciaPrevia"></asp:Label>
                               &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkAgregarPais" runat="server" 
                               onclick="lnkAgregarPais_Click"  ><i class="icon-plus"></i><asp:Label ID="lblAPais" runat="server" Text="Agregar Pais" meta:resourcekey="lblAPais"></asp:Label>
                            </asp:LinkButton>
                            </h5>
                            <hr />
                        </div>
                        
                        <div style="padding-left:10px;padding-right:10px;">
                             
                        <telerik:RadGrid ID="gridPais" runat="server" AutoGenerateColumns="False" 
                                CellSpacing="0" GridLines="None" Culture="es-ES" Width="100%"  ShowHeader="false"
                                Height="120px" onitemcommand="gridPais_ItemCommand" >
                            <ClientSettings EnableRowHoverStyle="true" ></ClientSettings>
                            <MasterTableView NoMasterRecordsText="No records to display." meta:resourcekey="lblNoDato" >
                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                    Visible="True">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                    Visible="True">
                                </ExpandCollapseColumn>
                                <Columns>
                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temSel" HeaderText="" HeaderStyle-Width="20px">
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lnkPEliminar" runat="server" meta:resourcekey="lnkEliminar" 
                                            CommandName="Eliminar" ToolTip=""
                                            CommandArgument='<%# Bind("Nro") %>'>
                                            <i class="icon-remove"></i>
                                            </asp:LinkButton>
                                             <asp:Label ID="lblNro" runat="server" Text='<%# Bind("Nro") %>' Visible="false"></asp:Label>
                                      </ItemTemplate>
                                         <HeaderStyle Width="20px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temPais" HeaderText="Pais" meta:resourcekey="ColPais"
                                         HeaderStyle-Width="150px">
                                         <ItemTemplate>
                                            <asp:DropDownList ID="cboPais" runat="server" CssClass="input-medium">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblPais" runat="server" Text='<%# Bind("cod_pais_in") %>' Visible="false"></asp:Label>                                         
                                         </ItemTemplate>
                                         <HeaderStyle Width="150px" />
                                     </telerik:GridTemplateColumn>
 

                                </Columns>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                            
                            </MasterTableView>
                     
                        </telerik:RadGrid> 
             
                        </div>


                        </div>
                        <div style="clear:both"></div>
                        </div>


                        
                       <div style="padding-top:10px;width: 100%">
                         <div style="width:50%; float:left">

                         <div style="padding-left:10px;padding-right:10px;padding-bottom:5px">
                            <h5><asp:Label ID="lblCV" runat="server" Text="CVs" meta:resourcekey="lblCV"></asp:Label>
                                &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkAgregarCV" runat="server" 
                               onclick="lnkAgregarCV_Click"><i class="icon-plus"></i><asp:Label ID="lblACV" runat="server" meta:resourcekey="lblACV" Text="Agregar CV"></asp:Label>
                                </asp:LinkButton>                                                  
                            
                            </h5><hr />
                           <asp:Label ID="Label1" runat="server" Text="Educacion" Visible="false"
                                meta:resourcekey="lblMEducacion" Font-Bold="False" Font-Italic="True" ></asp:Label>   
                           
                        </div>
                        
                        <div id="errorcvupload" style="display:none;padding-left:10px;padding-right:10px;">
                             <asp:Panel ID="Panel2" runat="server">
                                <div class="alert alert-error">                    
                                    <strong>Oh Error!</strong>
                                    <asp:Label ID="lblMensajeCVUpload" runat="server" meta:resourcekey="lblMensajeCVUpload" ></asp:Label>                      
                                </div>        
                             </asp:Panel> 
                        </div>

                        <div style="padding-left:10px;padding-right:10px;padding-top:5px">
                             
                        <telerik:RadGrid ID="gridCV" runat="server" AutoGenerateColumns="False" 
                                CellSpacing="0" GridLines="None" Culture="es-ES" Width="450px"  
                                Height="130px" HeaderStyle-Width="700px"
                                onitemcommand="gridCV_ItemCommand" >
                            <ClientSettings EnableRowHoverStyle="true" ></ClientSettings>
                            <MasterTableView NoMasterRecordsText="No records to display." meta:resourcekey="lblNoDato" >
                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                    Visible="True">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                    Visible="True">
                                </ExpandCollapseColumn>
                                <Columns>
                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temSel" HeaderText="" HeaderStyle-Width="20px">
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lnkPEliminar" runat="server" meta:resourcekey="lnkEliminar" 
                                            CommandName="Eliminar" ToolTip=""
                                            CommandArgument='<%# Bind("Nro") %>'><i class="icon-remove"></i></asp:LinkButton>
                                             <asp:Label ID="lblNro" runat="server" Text='<%# Bind("Nro") %>' Visible="false"></asp:Label>                                             
                                             <asp:Label ID="lblCodigo" runat="server" Text='<%# Bind("cod_condoc_in") %>' Visible="false"></asp:Label>                                         
                                      </ItemTemplate>
                                         <HeaderStyle Width="20px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temTitulo" HeaderText="Titulo" meta:resourcekey="ColTitulo"
                                         HeaderStyle-Width="300px">
                                         <ItemTemplate>
                                             <asp:TextBox ID="txtTitulo" runat="server" Text='<%# Bind("nom_condoc_vc") %>' 
                                             CssClass="input-xlarge"
                                            ></asp:TextBox>                                                                          
                                         </ItemTemplate>
                                         <HeaderStyle Width="300px" />
                                     </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temCV" HeaderText="CV"  meta:resourcekey="ColCV" HeaderStyle-Width="400px">
                                        <ItemTemplate>                                      
                                        <div>
                                            <div style="float:left;padding-left:5px;width:110px">    
                                                <asp:Label ID="lblDescargaN" runat="server" Visible="false" 
                                                    Text= '<%# Bind("des_condoc_vc") %>'>
                                                </asp:Label>  
                                                       
                                                <asp:HyperLink ID="lnkDescargar" runat="server"  Visible="false" Width="100px" ToolTip='<%# Bind("des_condoc_vc") %>'
                                                        NavigateUrl='<%# "~/downloading.aspx?file="+Eval("des_condoc_vc") %>' >
                                                     <asp:Label ID="lblDescarga" runat="server" Width="50px"
                                                        Text= '<%# Bind("des_condoc_vc") %>'></asp:Label>
                                                </asp:HyperLink>
                                            </div>
                                            <div style="float:left;width:180px;padding-left:5px">
                                                 <telerik:RadAsyncUpload ID="RadUpCV" runat="server"    meta:resourcekey="RadUpCV"
                                                    Width="220px"
                                                    AllowedFileExtensions="doc,docx" AutoAddFileInputs="False"
                                                    OnClientValidationFailed="validationFailedCV" OnClientFileUploading="validationOkCV">
                                                  <Localization Select="Choose File" />
                                                </telerik:RadAsyncUpload>     
                                            </div>
                                        <div style="clear:both"></div>                          
                                         </div>
                                        </ItemTemplate>
                                        <HeaderStyle Width="400px" />
                                   </telerik:GridTemplateColumn>

                                </Columns>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                            
                            </MasterTableView>
                     
                        </telerik:RadGrid> 
             
                        </div>

                        <div style="padding-left:10px;padding-right:10px;padding-top:10px">
                            <div class="alert">
                                <asp:Label ID="lblMensajeCV" runat="server" Text="Label" Font-Italic="True"   Font-Bold="true"
                                    meta:resourcekey="lblMensajeCV"></asp:Label>
                            </div>
                        </div>  


                        </div>

                        <div style="width:50%; float:left">
                        
                        <div style="padding-left:10px;padding-right:10px;">
                            <h5><asp:Label ID="lblPlantillaDonante" runat="server" Text="Plantillas de CVs" 
                            meta:resourcekey="lblPlantillaDonante"
                            ></asp:Label>
                            </h5>
                            <hr />
                        </div>
                        <div style="padding-top:10px;padding-left:10px;padding-right:10px;">

                           <telerik:RadGrid ID="gridPlantilla" runat="server" AutoGenerateColumns="False" 
                                CellSpacing="0" GridLines="None" Culture="es-ES" Width="100%"  
                                Height="130px" >
                            <ClientSettings EnableRowHoverStyle="true" ></ClientSettings>
                            <MasterTableView NoMasterRecordsText="No records to display." meta:resourcekey="lblNoDato" >
                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                    Visible="True">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                    Visible="True">
                                </ExpandCollapseColumn>
                                <Columns>
                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temSel" HeaderText="" HeaderStyle-Width="30px">
                                        <ItemTemplate>                                              
                                            <asp:HyperLink ID="lnkDescargar" runat="server"   NavigateUrl='<%# "~/downloading.aspx?file="+Eval("desc_donpla_vc") %>' >
                                            <i class="icon-download-alt"></i><asp:Label ID="lblDescargar" runat="server" meta:resourcekey="lblDescargar" Text="Descargar"></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                         <HeaderStyle Width="30px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temDonante" HeaderText="Donante" meta:resourcekey="ColDonante" HeaderStyle-Width="80px">
                                         <ItemTemplate>                                                                                        
                                            <asp:Label ID="lblDonante" runat="server" Text='<%# Bind("nom_don_vc") %>'></asp:Label>&nbsp;
                                            <asp:Label ID="lblPlantilla" runat="server" Text='<%# Bind("nom_donpla_vc") %>'></asp:Label> 
                                            <asp:Label ID="lblCodigo" runat="server" Text='<%# Bind("cod_donpla_in") %>' Visible="false"></asp:Label>
                                         </ItemTemplate>
                                         <HeaderStyle Width="80px" />
                                     </telerik:GridTemplateColumn>

                                   

                                </Columns>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                            
                            </MasterTableView>
                     
                        </telerik:RadGrid> 

                        </div>

                        </div>
                       <div style="clear:both"></div>
                       </div>

                    </telerik:RadPageView>

                    <telerik:RadPageView ID="RadPageView4" runat="server" >
                        <div style="padding-top:10px;padding-left:10px;padding-right:10px;padding-bottom:5px">
                        <h5><asp:Label ID="lblIdiomas" runat="server" Text="Idiomas" meta:resourcekey="lblIdiomas"></asp:Label>
                         &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lnkAgregarIdioma" runat="server" 
                                onclick="lnkAgregarIdioma_Click" ><i class="icon-plus"></i><asp:Label ID="lblAIdioma" runat="server" 
                                Text="Agregar Idioma" meta:resourcekey="lblAIdioma"></asp:Label>
                            </asp:LinkButton>
                        </h5>
                        <hr />
                        <asp:Label ID="lblMLenguaje" runat="server" Text="Educacion" Visible="false"
                                meta:resourcekey="lblMEducacion" Font-Bold="False" Font-Italic="True" ></asp:Label>           
                        </div>
                        
                        <div style="padding:0px;padding-left:10px;padding-right:10px;">
                             
                        <telerik:RadGrid ID="gridLenguaje" runat="server" AutoGenerateColumns="False" 
                                CellSpacing="0" GridLines="None" Culture="es-ES" Width="100%"  
                                Height="400px" onitemcommand="gridLenguaje_ItemCommand" >
                            <ClientSettings EnableRowHoverStyle="true" ></ClientSettings>
                            <MasterTableView NoMasterRecordsText="No records to display." meta:resourcekey="lblNoDato" >
                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                    Visible="True">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                    Visible="True">
                                </ExpandCollapseColumn>
                                <Columns>
                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temSel" HeaderText="" HeaderStyle-Width="20px">
                                        <ItemTemplate>
                                             <asp:LinkButton ID="lnkPEliminar" runat="server" meta:resourcekey="lnkEliminar" 
                                            CommandName="Eliminar" ToolTip=""
                                            CommandArgument='<%# Bind("Nro") %>'>
                                            <i class="icon-remove"></i>
                                            </asp:LinkButton>
                                             <asp:Label ID="lblNro" runat="server" Text='<%# Bind("Nro") %>' Visible="false"></asp:Label>
                                      </ItemTemplate>
                                         <HeaderStyle Width="20px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temLenguaje" HeaderText="Lenguaje" meta:resourcekey="ColLenguaje" HeaderStyle-Width="100px">
                                         <ItemTemplate>
                                            <asp:DropDownList ID="cboLenguaje" runat="server" CssClass="input-medium">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblLenguaje" runat="server" Text='<%# Bind("cod_len_in") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblCodigo" runat="server" Text='<%# Bind("cod_conlen_in") %>' Visible="false"></asp:Label>
                                         </ItemTemplate>
                                         <HeaderStyle Width="100px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temHablado"  HeaderText="Hablado" meta:resourcekey="ColHablado" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="cboHablado" runat="server" CssClass="input-medium">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblHablado" runat="server" Text='<%# Bind("spk_conlen_in") %>' Visible="false"></asp:Label>                                       
                                        </ItemTemplate>
                                         <HeaderStyle Width="100px" />
                                     </telerik:GridTemplateColumn>

                                      
                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temLeido"  HeaderText="Leido" meta:resourcekey="ColLeido" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="cboLeido" runat="server" CssClass="input-medium">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblLeido" runat="server" Text='<%# Bind("red_conlen_in") %>' Visible="false"></asp:Label>                                       
                                        </ItemTemplate>
                                         <HeaderStyle Width="100px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temEscrito"  HeaderText="Escrito" meta:resourcekey="ColEscrito" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="cboEscrito" runat="server" CssClass="input-medium">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblEscrito" runat="server" Text='<%# Bind("wrt_conlen_in") %>' Visible="false"></asp:Label>                                       
                                        </ItemTemplate>
                                         <HeaderStyle Width="100px" />
                                     </telerik:GridTemplateColumn>

                                </Columns>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                            
                            </MasterTableView>
                     
                        </telerik:RadGrid> 
             
                        </div>
                    </telerik:RadPageView>
                
                </telerik:RadMultiPage>
               
       
              

                  <div class="form-horizontal">
                        <div class="form-actions">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" 
                                meta:resourcekey="btnGuardar" CssClass="btn btn-primary" 
                                onclick="btnGuardar_Click"  />
                            <asp:HyperLink ID="lnkVolver" runat="server" CssClass="btn" NavigateUrl="~/WebConsultant.aspx">
                                <asp:Label ID="lblVolver" runat="server" Text="Volver" meta:resourcekey="lblVolver"></asp:Label>
                            </asp:HyperLink>                            
                        </div>
                    </div>
                    
                </asp:Panel>
            
                <asp:Panel ID="pnlInactivo" runat="server" Visible="False">
                    <div class="bs-docs-example">
                        <div class="alert alert-error">
                        <button type="button" class="close" data-dismiss="alert">×</button>
                        <strong>Oh Error!</strong>
                         <asp:Literal ID="litSinConexion" runat="server"></asp:Literal>   
                        </div>
                    </div>        
                </asp:Panel>
          
          
          </div>
          </div>
        
         </div>

        </div>
        </telerik:RadAjaxPanel>
        <asp:Panel ID="pnlCopiar" runat="server">
    
          <telerik:RadWindow ID="winCopiar" runat="server" Title="Timesheet" Width="820" 
                        Height="640"   Behaviors="Move, Resize, Close" Modal="true" meta:resourcekey="winCopiar">
                    <ContentTemplate>
                        <div class="form-horizontal" style="padding-top:30px;padding-left:10px;padding-right:10px;">
                            

                            <telerik:RadGrid ID="gridUsuario" runat="server" CellSpacing="0" 
                                Culture="es-ES" DataSourceID="SqlConsulta" GridLines="None" PageSize="15"
                                AllowPaging="True" AllowFilteringByColumn="True" AllowSorting="True">
                                  <GroupingSettings CaseSensitive="false" />
                                <MasterTableView AutoGenerateColumns="False" DataKeyNames="con_usu_vc" 
                                    DataSourceID="SqlConsulta">
                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                        Visible="True">
                                        <HeaderStyle Width="20px" />
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                        Visible="True">
                                        <HeaderStyle Width="20px" />
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="com_usu_vc"  meta:resourcekey="ColPersona"
                                            FilterControlAltText="Filter com_usu_vc column" HeaderText="com_usu_vc" 
                                            SortExpression="com_usu_vc" UniqueName="com_usu_vc">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="con_usu_vc"  meta:resourcekey="ColUsuario"
                                            FilterControlAltText="Filter con_usu_vc column" HeaderText="con_usu_vc" 
                                            ReadOnly="True" SortExpression="con_usu_vc" UniqueName="con_usu_vc">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="cor_usu_vc"  meta:resourcekey="ColCorreo"
                                            FilterControlAltText="Filter cor_usu_vc column" HeaderText="cor_usu_vc" 
                                            SortExpression="cor_usu_vc" UniqueName="cor_usu_vc">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <EditFormSettings>
                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                        </EditColumn>
                                    </EditFormSettings>
                                    <PagerStyle PageSizeControlType="RadComboBox" />
                                </MasterTableView>
                                <PagerStyle PageSizeControlType="RadComboBox" />
                                <FilterMenu EnableImageSprites="False">
                                </FilterMenu>
                            </telerik:RadGrid>

                            <asp:SqlDataSource ID="SqlConsulta" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:ConnectionStringLDAP %>" 
                                SelectCommand="usp_UsuarioBuscar" SelectCommandType="StoredProcedure">                                 
                            </asp:SqlDataSource>

                           
                            <div class="form-actions">
                                    <asp:Button ID="btnUsuario" runat="server" Text="Consultar"  
                                        meta:resourcekey="btnUsuario" CssClass="btn btn-primary" onclick="btnUsuario_Click" 
                                     />   
                                        
                                    <asp:LinkButton ID="lnkCerrar" runat="server" CssClass="btn"  OnClientClick="cerrarCopiar(); return false;" >
                                        <asp:Label ID="lblCerrar" runat="server" Text="Imprimir" meta:resourcekey="lblCerrar" ></asp:Label>
                                    </asp:LinkButton>                                           
                            </div>
                            </div>
                        </div>        
                    </ContentTemplate>
                    </telerik:RadWindow>
        </asp:Panel>
        <!--Pie-->
        <usrpagina:PiePagina ID="usrPie" runat="server" />
        <!---Pie-->
    </div>
  
    </form>   

    
     <!--PieJs-->
    <usrpagina:PiePaginaJs ID="usrPieJs" runat="server" />
    <!---PieJs-->
</body>
</html>
