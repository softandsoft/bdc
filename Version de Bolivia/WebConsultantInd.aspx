<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebConsultantInd.aspx.cs" Inherits="WebConsultantInd" %>

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
        .RadListBox_Default .rlbDisabled .rlbButtonText, .RadListBox_Default .rlbDisabled:hover .rlbButtonText {
            color: #000000;
        }
        
        .RadListBox_Default .rlbDisabled .rlbText {
            color: #000000;
        }
        
        .pageView
        {
            border: 1px solid #898c95;
            /*border-top: none;*/
            margin-top: -1px;
            height: 640px;
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
            
        } 
   
         
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    <!--Menu-->
    <usrpagina:Menu ID="usrMenu" runat="server" />
    <!--Menu-->
       <div class="container-fluid" style="min-width:940px">
      
       <div class="row-fluid">
       
        <div class="span12">
            <div class="widget-box">
                <div class="widget-title">
		            <h5><asp:Label ID="lblAgregar" runat="server" meta:resourcekey="lblAgregar"></asp:Label><asp:Label ID="lblEditar" runat="server" meta:resourcekey="lblEditar" Visible="false"></asp:Label></h5>                    

                     <div style="float:right">  
                        <h5 style="font-style:italic;font-weight:normal">
                      <asp:Label ID="lblActualizacion" runat="server" Text="" Visible="false"></asp:Label>
                        </h5>
                    </div>

                    <div style="clear:both">
                    </div>
                </div>
                <div class="widget-content" style="padding-bottom:20px;">
                
                <asp:Panel ID="pnlActivo" runat="server">
                    
                <asp:Label ID="lblCodigo" runat="server" Text="0" Visible="false"></asp:Label>                               
                
                 <asp:Panel ID="pnlError" runat="server" Visible="false">
                    <div class="alert alert-error">
                        <button type="button" class="close" data-dismiss="alert">×</button> 
                        <strong>Oh Error!</strong>
                        <asp:Literal ID="litError" runat="server"></asp:Literal>
                    </div>        
                 </asp:Panel>         

                    <telerik:RadTabStrip ID="RadTabConsultor" runat="server" MultiPageID="RadMultiPage1"
                          SelectedIndex="0" Align="Justify" ReorderTabsOnSelect="True" Width="700px">
                        <Tabs>
                            <telerik:RadTab Text="Informacion General" Selected="True" meta:resourcekey="tabInfo">
                            </telerik:RadTab>
                            <telerik:RadTab Text="Educacion"  meta:resourcekey="tabEdu">
                            </telerik:RadTab>
                            <telerik:RadTab Text="Experiencia"  meta:resourcekey="tabExperiencia">
                            </telerik:RadTab>
                            <telerik:RadTab Text="Idioma"  meta:resourcekey="tabIdioma">
                            </telerik:RadTab>
                            <telerik:RadTab Text="Referencia"  meta:resourcekey="tabReferencia">
                            </telerik:RadTab>
                            <telerik:RadTab Text="Uso Interno"  meta:resourcekey="tabInternal">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>

                    
                <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0"  CssClass="pageView"
                Width="950px">
                    <telerik:RadPageView ID="RadPageView1" runat="server">
                      <div class="form-horizontal" style="padding-top:20px;">

                       <div style="float:left; width:750px;">
                        <div class="control-group">
                            <asp:Label ID="lblTipo" Text="Tipo" runat="server" CssClass="control-label" meta:resourcekey="lblTipo" 
                               ></asp:Label>
                            <div class="controls">
                                <span class="input-xlarge uneditable-input">
                                    <asp:Label ID="lblCTipo" runat="server" Text="User"></asp:Label>
                                </span>                              
                            </div>                        
                        </div>

                        <div class="control-group">
                        <asp:Label ID="lblApellido" runat="server" Text="* Apellido" 
                        CssClass="control-label" meta:resourcekey="lblApellido" 
                             ></asp:Label>
                        <div class="controls">
                            <span class="input-xlarge uneditable-input">
                                <asp:Label ID="lblCApellido" runat="server" Text="User" >
                                </asp:Label>
                            </span>                         
                        </div>                        
                        </div>

                        <div class="control-group">
                        <asp:Label ID="lblNombre" runat="server" Text="* Nombre" 
                                CssClass="control-label" meta:resourcekey="lblNombre" 
                           ></asp:Label>
                        <div class="controls">
                            <span class="input-xlarge uneditable-input">
                                <asp:Label ID="lblCNombre" runat="server" Text="User" >
                                </asp:Label>
                            </span>                                
                        </div>                        
                        </div>

                        <div>
                        <div style="float:left; width:400px;">
                            <div class="control-group">
                            <asp:Label ID="lblFecha" runat="server" Text="* Fecha de Nacimiento" 
                                    CssClass="control-label" meta:resourcekey="lblFecha" 
                                ></asp:Label>
                            <div class="controls">
                                 <span class="input-medium uneditable-input">
                                    <asp:Label ID="lblCFecha" runat="server" Text="" >
                                    </asp:Label>
                                </span>                                 
                            </div>
                            </div>
                        </div>
                        <div style="float:left; ">
                            <div class="control-group">
                            <asp:Label ID="lblSexo" runat="server" Text="*Sexo" 
                                    CssClass="control-label-sec" meta:resourcekey="lblSexo" 
                                ></asp:Label>
                            <div class="controls" style="margin-left: 120px;">
                                <span class="input-medium uneditable-input">
                                    <asp:Label ID="lblCSexo" runat="server" Text="User" >
                                    </asp:Label>
                                </span>         
                            </div>
                            </div>
                        </div>
                        <div style="clear:both"></div>
                        </div>  


                         <div>
                        <div style="float:left; width:400px;">
                            <div class="control-group">
                            <asp:Label ID="lblNacionalidad1" runat="server" Text="* Nacionalidad 1" 
                                    CssClass="control-label" meta:resourcekey="lblNacionalidad1" 
                                ></asp:Label>
                            <div class="controls">
                                 <span class="input-medium uneditable-input">
                                    <asp:Label ID="lblCNacionalidad1" runat="server" Text="" >
                                    </asp:Label>
                                </span>      
                            </div>
                            </div>
                        </div>
                        <div style="float:left; ">
                            <div class="control-group">
                            <asp:Label ID="lblNacionalidad2" runat="server" Text="Nacionalidad 2" 
                                    CssClass="control-label-sec" meta:resourcekey="lblNacionalidad2" 
                               ></asp:Label>
                            <div class="controls" style="margin-left: 120px;">
                                <span class="input-medium uneditable-input">
                                    <asp:Label ID="lblCNacionalidad2" runat="server" Text="" >
                                    </asp:Label>
                                </span>      
                            </div>
                            </div>
                        </div>
                        <div style="clear:both"></div>
                        </div>  

                         <div class="control-group">
                        <asp:Label ID="lblDireccion" runat="server" Text="Direccion" 
                        CssClass="control-label" meta:resourcekey="lblDireccion" 
                             ></asp:Label>
                        <div class="controls">                           
                             <span class="input-xxlarge uneditable-input uneditable-textarea">
                                    <asp:Label ID="lblCDireccion" runat="server" Text="User" >
                                    </asp:Label>
                                </span>      
                        </div>                        
                        </div>

                        <div>
                        <div style="float:left; width:400px;">
                            <div class="control-group">
                            <asp:Label ID="lblPais" runat="server" Text="* Pais" 
                                    CssClass="control-label" meta:resourcekey="lblPais" 
                                ></asp:Label>
                            <div class="controls">
                                <span class="input-medium uneditable-input">
                                    <asp:Label ID="lblCPais" runat="server" Text="User" >
                                    </asp:Label>
                                </span>                  
                            </div>
                            </div>
                        </div>
                        <div style="float:left; ">
                            <div class="control-group">
                            <asp:Label ID="lblCiudad" runat="server" Text="Ciudad" 
                                    CssClass="control-label-sec" meta:resourcekey="lblCiudad" 
                                 ></asp:Label>
                            <div class="controls" style="margin-left: 120px;">
                                <span class="input-medium uneditable-input">
                                    <asp:Label ID="lblCCiudad" runat="server" Text="User" >
                                    </asp:Label>
                                </span>            
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
                                ></asp:Label>
                            <div class="controls">
                                <span class="input-medium uneditable-input">
                                    <asp:Label ID="lblCTelefono1" runat="server" Text="User" >
                                    </asp:Label>
                                </span>     
                            </div>
                            </div>
                        </div>
                        <div style="float:left; ">
                            <div class="control-group">
                            <asp:Label ID="lblTelefono2" runat="server" Text="Telefono 2" 
                                    CssClass="control-label-sec" meta:resourcekey="lblTelefono2" 
                                ></asp:Label>
                            <div class="controls" style="margin-left: 120px;">
                               <span class="input-medium uneditable-input">
                                    <asp:Label ID="lblCTelefono2" runat="server" Text="User" >
                                    </asp:Label>
                                </span>     
                            </div>
                            </div>
                        </div>
                        <div style="clear:both"></div>
                        </div>

                        <div class="control-group">
                            <asp:Label ID="lblBiografia" runat="server" Text="Bio" 
                            CssClass="control-label" meta:resourcekey="lblBiografia"></asp:Label>
                            <div class="controls">                           
                                 <span class="input-xxlarge uneditable-input uneditable-textarea">
                                        <asp:Label ID="lblCBiografia" runat="server" Text="User" >
                                        </asp:Label>
                                    </span>      
                            </div>                        
                        </div>


                        <div class="control-group">
                        <asp:Label ID="lblLinked" runat="server" Text="LinkedIn URL" 
                        CssClass="control-label" meta:resourcekey="lblLinked" ></asp:Label>
                        <div class="controls">
                            <span class="input-xlarge uneditable-input">
                                <asp:Label ID="lblCLinked" runat="server" Text="" >
                                </asp:Label>
                            </span>                                           
                        </div>                        
                        </div>

                         <asp:Panel ID="Panel1" runat="server">                        
                       
                           <div class="control-group">
                            <asp:Label ID="lblIdioma" runat="server" Text="Idioma" 
                                    CssClass="control-label" meta:resourcekey="lblIdioma" 
                                 ></asp:Label>
                            <div class="controls">
                                 <span class="input-xlarge uneditable-input">
                                    <asp:Label ID="lblCIdioma" runat="server" Text="User" >
                                    </asp:Label>
                                </span>        
                            </div>
                            </div>
                         </asp:Panel>

                         <div class="control-group">
                        <asp:Label ID="lblCorreo" runat="server" Text="* Correo" 
                        CssClass="control-label" meta:resourcekey="lblCorreo" 
                           ></asp:Label>
                        <div class="controls">
                            <span class="input-xlarge uneditable-input">
                                <asp:Label ID="lblCCorreo" runat="server" Text="User" >
                                </asp:Label>
                            </span>                                    
                        </div>                        
                        </div>
                       
                       <asp:Panel ID="pnlDA" runat="server" Visible="true">                        
                        <div class="control-group">
                            <asp:Label ID="lblDA" runat="server" Text="Usuario DA" CssClass="control-label" meta:resourcekey="lblDA" 
                                ></asp:Label>
                            <div class="controls">
                                <span class="input-xlarge uneditable-input">
                                    <asp:Label ID="lblCDA" runat="server" Text="User" >
                                    </asp:Label>
                                </span>             
                            </div>                        
                        </div>
                       </asp:Panel>

                       <asp:Panel ID="pnlContrasenia" runat="server" Visible="false">                       
                       
                       </asp:Panel>

                        <div class="control-group">
                            <span class="control-label">  
                                <asp:Label ID="lblMoneda" runat="server" Text="* Correo" 
                                 CssClass="pull-right" meta:resourcekey="lblMoneda" 
                                    ></asp:Label>                             
                             </span>
                            <div class="controls">
                                <span class="input-large uneditable-input">
                                    <asp:Label ID="lblCMoneda" runat="server" Text="" >
                                    </asp:Label>
                                </span>        
                                                                   
                            </div>                        
                        </div>

                        <div class="control-group">
                            <span class="control-label">
                                <asp:Label ID="lblFechaActualizacion" runat="server" Text="Fecha de actualización" CssClass="pull-right" meta:resourcekey="lblFechaActualizacion" AssociatedControlID="txtFechaActualizacion"></asp:Label>
                            </span>
                            <div class="controls">
                                <telerik:RadTextBox ID="txtFechaActualizacion" runat="server" Font-Size="Medium" Height="28px" Width="120px" CssClass="input-small" Enabled="false"></telerik:RadTextBox>
                            </div>
                        </div>                        
                      
                      </div>
                       <div style="float:left; width:200px;">
                          <div style="text-align:center">
                              <asp:Label ID="lblFoto" runat="server" Text="Foto" meta:resourcekey="lblFoto" ></asp:Label>
                          </div>
                          <div style="text-align:center;padding-top:10px;">
                            <asp:Image ID="imgConsultor" runat="server" Width="150px" Height="150px" ImageUrl="~/Imagen/usuario.png" CssClass="img-polaroid"/>
                          </div>

                          <div style="text-align:center;padding-top:10px;">
                           <span class="invalid"></span>
                                               
                          </div>                          
                      </div>
                      <div style="clear:both"></div>
                      </div>
                    </telerik:RadPageView>
                            
                    <telerik:RadPageView ID="RadPageView2" runat="server">
                        <div style="padding:10px;padding-left:10px;padding-right:10px;">
                        <h5>
                            <asp:Label ID="lblEducacion" runat="server" Text="Educacion" meta:resourcekey="lblEducacion" ></asp:Label>                           
                        </h5>
                        <hr />
                        </div>
                        <div style="padding-left:10px;padding-right:10px;">
                             
                        <telerik:RadGrid ID="gridEduacion" runat="server" AutoGenerateColumns="False" 
                            CellSpacing="0" GridLines="None" Culture="es-ES" Width="100%"  
                            Height="190px" CellPadding="0">
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
                                         <asp:Label ID="lblNro" runat="server" Text='<%# Bind("Nro") %>' ></asp:Label>
                                  </ItemTemplate>
                                     <HeaderStyle Width="20px" />
                                 </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                    UniqueName="temNivel" HeaderText="Nivel" meta:resourcekey="ColNivel" HeaderStyle-Width="100px">
                                     <ItemTemplate>
                                        <asp:Label ID="lblNivel" runat="server" Text='<%# Bind("oNivel.nom_nivaca_vc") %>' ></asp:Label>                                    
                                     </ItemTemplate>
                                     <HeaderStyle Width="120px" />
                                 </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                    UniqueName="temDescripcion"  HeaderText="Descripcion" meta:resourcekey="ColDescripcion" HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("des_conedu_vc") %>'></asp:Label>                                       
                                    </ItemTemplate>
                                     <HeaderStyle Width="120px" />
                                 </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                    UniqueName="temDuracion" HeaderText="Duracion"  meta:resourcekey="ColDuracion" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDuracion" runat="server" Text='<%# Bind("can_dur_conedu_in") %>'>
                                        </asp:Label>&nbsp;
                                         <asp:Label ID="lblTipo" runat="server" Text='<%# Bind("Tipo") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                     <HeaderStyle Width="100px" />
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                    UniqueName="temInstitucion" HeaderText="Institucion"  meta:resourcekey="ColInstitucion" 
                                    HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                         <asp:Label ID="lblInstitucion" runat="server" Text='<%# Bind("ins_conedu_vc") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                </telerik:GridTemplateColumn>

                                 <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                    UniqueName="temCertificado" HeaderText="Certificado"  meta:resourcekey="ColCertificado" HeaderStyle-Width="150px">
                                    <ItemTemplate>  
                                            
                                        <asp:Label ID="lblDescarga" runat="server" Visible="false"
                                                Text= '<%# Bind("adj_conedu_vc") %>'></asp:Label>
                                        <asp:HyperLink ID="lnkDescargar" runat="server"  Visible="false"
                                                NavigateUrl='<%# "~/downloading.aspx?file="+Eval("adj_conedu_vc") %>' >
                                            <i class="icon-download-alt"></i>
                                        </asp:HyperLink>                                                
                                            
                                    </ItemTemplate>
                                    <HeaderStyle Width="180px" />
                                </telerik:GridTemplateColumn>

                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                            
                        </MasterTableView>
                     
                    </telerik:RadGrid> 
                    </div>
                    
                    <div style="padding:10px;padding-left:10px;padding-right:10px;">
                        <h5>
                            <asp:Label ID="lblHabilidades" runat="server" Text="Habilidades Técnicas" meta:resourcekey="lblHabilidades" ></asp:Label>                             
                        </h5>
                        <hr />
                    </div>

                    <div style="padding-left:10px;padding-right:10px;">

                    <telerik:RadGrid ID="gridHabilidades"
                        runat="server" GridLines="None" ShowHeader="false" Height="160px"
                        OnPreRender="gridHabilidades_PreRender" >
            
                    <MasterTableView TableLayout="Fixed">
                        <ItemTemplate>
                            <%# (((GridItem)Container).ItemIndex != 0)? "</td></tr></table>" : "" %>
                            <asp:Panel ID="ItemContainer" CssClass='<%# (((GridItem)Container).ItemType == GridItemType.Item)? "item" : "alternatingItem" %>'
                                runat="server">
                                <b><asp:Label ID="lblGeneral" runat="server" Text='<%# Eval("nom_congen_vc")%>' Visible="true"></asp:Label></b>
                                <asp:Label ID="lblCodigo" runat="server" Text='<%# Eval("cod_congen_in")%>' Visible="false"></asp:Label>
                                <div style="padding-top:10px;">
                                   <telerik:RadListBox ID="lstTecnico" runat="server" CheckBoxes="true" BorderWidth="0px" Width="270px" >                                  
                                   </telerik:RadListBox>                                    
                                </div>
                            </asp:Panel>
                        </ItemTemplate>
                    </MasterTableView>
                    <GroupingSettings CaseSensitive="false" />
                </telerik:RadGrid>

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
                            
                            <div style="padding-left:10px;padding-right:10px;">
                                <label class="checkbox">
                                    <asp:CheckBox ID="chkCertifico" runat="server"/>          
                                    <asp:Label ID="lblCertifico" Text="Certifico que tengo experiencia en paises en desarrollo" 
                                            runat="server" 
                                            meta:resourcekey="lblCertifico" 
                                        AssociatedControlID="chkCertifico"></asp:Label>
                                </label>
                            </div>
                                 
                          
                        </div>

                        <div style="width:50%; float:left">
                            <div style="padding-left:10px;padding-right:10px;">                                 
                                    <h5>
                                        <asp:Label ID="lblExperienciaTotal" runat="server" Text="Experiencia Total" meta:resourcekey="lblExperienciaTotal" ></asp:Label>
                                    </h5>
                                   <hr/>  
                            </div>
                            <div style="padding-left:10px;padding-right:10px;">
                                 <div style="width:100%">
                                 
                                 <div style="width:50%;float:left">
                                     <div style="padding-left:10px;padding-right:10px;">
                                        <label class="radio">
                                          <asp:RadioButton ID="rbExperiencia1" runat="server" 
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
                            </h5>
                            <hr />
                        </div>
                        
                        <div style="padding-left:10px;padding-right:10px;">
                             
                        <telerik:RadGrid ID="gridPais" runat="server" AutoGenerateColumns="False" 
                                CellSpacing="0" GridLines="None" Culture="es-ES" Width="100%"  ShowHeader="false"
                                Height="120px" >
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
                                             <asp:Label ID="lblNro" runat="server" Text='<%# Bind("Nro") %>' Visible="true"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="20px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temPais" HeaderText="Pais" meta:resourcekey="ColPais"
                                         HeaderStyle-Width="150px">
                                         <ItemTemplate>
                                            <asp:Label ID="lblPais" runat="server" Text='<%# Bind("nom_pais_vc") %>' Visible="true"></asp:Label>                                         
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

                         <div style="padding-left:10px;padding-right:10px;">
                            <h5><asp:Label ID="lblCV" runat="server" Text="CVs" meta:resourcekey="lblCV"></asp:Label>
                            </h5>
                            <hr />
                        </div>
                        
                        <div style="padding-top:10px;padding-left:10px;padding-right:10px;">
                             
                        <telerik:RadGrid ID="gridCV" runat="server" AutoGenerateColumns="False" 
                                CellSpacing="0" GridLines="None" Culture="es-ES" Width="100%"  
                                Height="130px" >
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
                                             <asp:Label ID="lblNro" runat="server" Text='<%# Bind("Nro") %>'></asp:Label>                                             
                                             <asp:Label ID="lblCodigo" runat="server" Text='<%# Bind("cod_condoc_in") %>' Visible="false"></asp:Label>                                         
                                      </ItemTemplate>
                                         <HeaderStyle Width="20px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temTitulo" HeaderText="Titulo" meta:resourcekey="ColTitulo"
                                         HeaderStyle-Width="120px">
                                         <ItemTemplate>
                                             <asp:Label ID="lblTitulo" runat="server" Text= '<%# Bind("nom_condoc_vc") %>'></asp:Label>                                                                                                                     
                                         </ItemTemplate>
                                         <HeaderStyle Width="120px" />
                                     </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temCV" HeaderText="CV"  meta:resourcekey="ColCV" HeaderStyle-Width="200px">
                                        <ItemTemplate>                                      
                                       
                                            <asp:Label ID="lblDescarga" runat="server" Visible="false"
                                                    Text= '<%# Bind("des_condoc_vc") %>'></asp:Label>
                                            <asp:HyperLink ID="lnkDescargar" runat="server"  Visible="false"
                                                    NavigateUrl='<%# "~/downloading.aspx?file="+Eval("des_condoc_vc") %>' >
                                                <i class="icon-download-alt"></i>
                                            </asp:HyperLink>
                                          
                                        </ItemTemplate>
                                        <HeaderStyle Width="200px" />
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

                        <div style="width:50%; float:left">
                         

                        </div>
                       <div style="clear:both"></div>
                       </div>

                    </telerik:RadPageView>

                    <telerik:RadPageView ID="RadPageView4" runat="server" >
                        <div style="padding:10px;padding-left:10px;padding-right:10px;">
                        <h5><asp:Label ID="lblIdiomas" runat="server" Text="Idiomas" meta:resourcekey="lblIdiomas"></asp:Label>                        
                        </h5>
                        <hr />
                        </div>
                        
                        <div style="padding:0px;padding-left:10px;padding-right:10px;">
                             
                        <telerik:RadGrid ID="gridLenguaje" runat="server" AutoGenerateColumns="False" 
                                CellSpacing="0" GridLines="None" Culture="es-ES" Width="100%"  
                                Height="400px"   >
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
                                             <asp:Label ID="lblNro" runat="server" Text='<%# Bind("Nro") %>' Visible="true"></asp:Label>
                                      </ItemTemplate>
                                         <HeaderStyle Width="20px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temLenguaje" HeaderText="Lenguaje" meta:resourcekey="ColLenguaje" HeaderStyle-Width="100px">
                                         <ItemTemplate>                                            
                                            <asp:Label ID="lblLenguaje" runat="server" Text='<%# Bind("nom_len_vc") %>' Visible="true"></asp:Label>
                                            <asp:Label ID="lblCodigo" runat="server" Text='<%# Bind("cod_conlen_in") %>' Visible="false"></asp:Label>
                                         </ItemTemplate>
                                         <HeaderStyle Width="100px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temHablado"  HeaderText="Hablado" meta:resourcekey="ColHablado" HeaderStyle-Width="100px">
                                        <ItemTemplate>                                          
                                            <asp:Label ID="lblHablado" runat="server" Text='<%# Bind("Hablar") %>'></asp:Label>                                       
                                        </ItemTemplate>
                                         <HeaderStyle Width="100px" />
                                     </telerik:GridTemplateColumn>

                                      
                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temLeido"  HeaderText="Leido" meta:resourcekey="ColLeido" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeido" runat="server" Text='<%# Bind("Leer") %>'></asp:Label>                                       
                                        </ItemTemplate>
                                         <HeaderStyle Width="100px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temEscrito"  HeaderText="Escrito" meta:resourcekey="ColEscrito" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEscrito" runat="server" Text='<%# Bind("Escritura") %>'></asp:Label>                                       
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
                
                    <telerik:RadPageView ID="RadPageView5" runat="server" >
                          <div style="padding-top:20px;padding-left:10px;padding-right:10px;">
                             
                        <telerik:RadGrid ID="gridReferencia" runat="server" AutoGenerateColumns="False" 
                                CellSpacing="0" GridLines="None" Culture="es-ES" Width="100%"  
                                Height="220px" >
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
                                        UniqueName="temCompania"  HeaderText="Compania" meta:resourcekey="ColCompania" HeaderStyle-Width="100px">
                                        <ItemTemplate>                                         
                                            <asp:Label ID="lblCompania" runat="server" Text='<%# Bind("com_ref_vc") %>' Visible="true"></asp:Label>                                       
                                        </ItemTemplate>
                                         <HeaderStyle Width="100px" />
                                     </telerik:GridTemplateColumn>

                                      
                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temPais"  HeaderText="Pais" meta:resourcekey="ColPais" HeaderStyle-Width="100px">
                                        <ItemTemplate>                     
                                            <asp:Label ID="lblPais" runat="server" Text='<%# Bind("nom_pais_vc") %>' Visible="true"></asp:Label>                       
                                            <asp:Label ID="lblPaisCodigo" runat="server" Text='<%# Bind("cod_pais_in") %>' Visible="false"></asp:Label>                                       
                                        </ItemTemplate>
                                         <HeaderStyle Width="100px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temContacto"  HeaderText="Contacto" meta:resourcekey="ColContacto" HeaderStyle-Width="80px">
                                        <ItemTemplate>                                           
                                            <asp:Label ID="lblContacto" runat="server" Text='<%# Bind("con_ref_vc") %>' Visible="true"></asp:Label>                                       
                                        </ItemTemplate>
                                         <HeaderStyle Width="80px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temPuesto"  HeaderText="Puesto" meta:resourcekey="ColPuesto" HeaderStyle-Width="80px">
                                        <ItemTemplate>                                           
                                            <asp:Label ID="lblPuesto" runat="server" Text='<%# Bind("pue_ref_vc") %>' Visible="true"></asp:Label>                                       
                                        </ItemTemplate>
                                         <HeaderStyle Width="80px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temTelefono"  HeaderText="Telefono" meta:resourcekey="ColTelefono" HeaderStyle-Width="60px">
                                        <ItemTemplate>                                           
                                            <asp:Label ID="lblTelefono" runat="server" Text='<%# Bind("tel_ref_vc") %>' Visible="true"></asp:Label>                                       
                                        </ItemTemplate>
                                         <HeaderStyle Width="60px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" 
                                        UniqueName="temCorreo"  HeaderText="Contacto" meta:resourcekey="ColCorreo" HeaderStyle-Width="60px">
                                        <ItemTemplate>                                           
                                            <asp:Label ID="lblCorreo" runat="server" Text='<%# Bind("cor_ref_vc") %>' Visible="true"></asp:Label>                                       
                                        </ItemTemplate>
                                         <HeaderStyle Width="60px" />
                                     </telerik:GridTemplateColumn>

                                     <telerik:GridTemplateColumn FilterControlAltText="Filter TemplateColumn column" UniqueName="temFeedback" HeaderText="Feeback" meta:resourcekey="ColFeedback">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFeedback" runat="server" Text='<%# Bind("feedback") %>' Visible="true"></asp:Label>
                                        </ItemTemplate>
                                         <HeaderStyle Width="20%" />
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

                    <telerik:RadPageView ID="RadPageView6" runat="server">
                        <div class="form-horizontal" style="padding-top:20px;">
                            <div class="control-group">
                                <asp:Label ID="lblRelationShip" Text="lblRelationShip" runat="server" CssClass="control-label" meta:resourcekey="lblRelationShip" AssociatedControlID="lblCRelationShip"></asp:Label>
                                <div class="controls">
                                    <span class="input-xlarge uneditable-input">
                                        <asp:Label ID="lblCRelationShip" runat="server" Text="User"></asp:Label>
                                    </span>
                                </div>
                            </div>

                            <div class="control-group">
                                <asp:Label ID="lblComentario" Text="lblComentario" runat="server" CssClass="control-label" meta:resourcekey="lblComentario"></asp:Label>
                                <div class="controls">
                                    <asp:TextBox ID="lblCComentario" runat="server" TextMode="MultiLine" Width="700px" Rows="16" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>

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
      
        <!--Pie-->
        <usrpagina:PiePagina ID="usrPie" runat="server" />
        <!---Pie-->
    </div>
    </form>
    
     <style type="text/css">
             .RadListBox_Default .rlbDisabled .rlbButtonText, .RadListBox_Default .rlbDisabled:hover .rlbButtonText {
            color: #000000;
        }
        
        .RadListBox_Default .rlbDisabled .rlbText {
            color: #000000;
        }
         </style> 
     <!--PieJs-->
    <usrpagina:PiePaginaJs ID="usrPieJs" runat="server" />
    <!---PieJs--> 
</body>
</html>
