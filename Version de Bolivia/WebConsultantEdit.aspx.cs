﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using IT.Service;
using IT.Domain;
using IT.Webconsultor.Util;

using System.Threading;
using System.Globalization;
using Telerik.Web.UI;
using System.Configuration;

using IT.Persistence;

public partial class WebConsultantEdit : System.Web.UI.Page
{
    #region Declaración de variables

    ISeguridadService oSeguridadSer;
    IAdministracionService oAdministracionSer;
    IConsultorService oConsultorSer;

    Usuario oTUsuario = new Usuario();
    IList<Usuario> oListaDA;
    IList<ConsultorEducacion> oListaConEducacion;
    IList<ConsultorLenguaje> oListaConLenguaje;
    IList<ConsultorDonante> oListaConDonante;
    IList<ConsultorPais> oListaConPais;
    IList<ConsultorNacionalidad> oListaConNacion;
    IList<ConsultorDocumento> oListaConCV;
    IList<ConsultorConocimiento> oListaConConocimiento;
    IList<ConsultorReferencia> oListaCReferencia;
    IList<TConsultorEducacion> oTListaConEducacion;
    IList<Catalogo> oListaMoneda;
    IList<Catalogo> oListaSexo;
    IList<Catalogo> oListaRelationship;

    IList<Pais> oListaGentilicio;
    // IList<Ciudad> oListaCiudad;

    IList<NivelAcademico> oListaNivel;
    IList<DescripcionAcademica> oListaDescripcion;
    IList<Catalogo> oListaDuracion;

    IList<Lenguaje> oListaIdioma;
    IList<Catalogo> oListaHablado;

    IList<Donante> oListaDonante;
    IList<DonantePlantilla> oListaPlantilla;
    IList<Catalogo> oListaMes;
    IList<Pais> oListaPais;

    IList<ConocimientoGeneral> oListaCGeneral;
    IList<ConocimientoEspecifico> oListaCEspecifico;
    IList<Idioma> oListaLenguaje;

    Helper oHelper = new Helper();

    int nCodigo = 0, nEstado = 0;

    Persona oPersona;
    Consultor oConsultor;
    Usuario oUsuario;
    string cCarpetaContenido = "";
    String cRutaContenido = "";
    String cTipo = "";
    Dictionary<String, String> oListaControl = new Dictionary<string, string>();
    Dictionary<String, String> oListaControlES = new Dictionary<string, string>();
    Diccionario oHelperDiccionario;

    List<IT.Domain.ConsultorCuenta> cuentasDeConsultor = new List<IT.Domain.ConsultorCuenta>();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        oHelperDiccionario = new Diccionario();

        if (!IsPostBack)
        {
            if (Session["Usuario"] != null)
            {
                oTUsuario = (Usuario)Session["Usuario"];

                this.RadTabConsultor.Tabs[6].Visible = oTUsuario.oPais.cod_pais_in.Equals(1) || oTUsuario.oPais.cod_pais_in.Equals(22);// 1 define el país de PERÚ y 2 BOLIVIA

                if (oTUsuario.oPais.cod_pais_in.Equals(1))
                {
                    this.lblImagenDNI.InnerText = "Imagen de DNI";
                    this.lblExoneracion4ta.InnerText = "Exoneración de Retención de 4ta";
                    this.divSuspension.Visible = true;
                }

                if (oTUsuario.oPais.cod_pais_in.Equals(22))
                {
                    this.lblImagenDNI.InnerText = "Imagen de Doc.";
                    this.lblExoneracion4ta.InnerText = "¿Tiene factura?";
                    this.divSuspension.Visible = false;
                }

                // El feedback del consultor solo se muestra para lo usuarios internos.
                if (oTUsuario.tip_usu_in.Equals(Constantes.USUARIOINTERNO))
                    this.divFeedback.Visible = true;
                else
                    this.divFeedback.Visible = false;

                if (oTUsuario.oIdioma.cod_idi_in == 1)
                    lblMensajeHabilidadesAbajo.Font.Size = new FontUnit(12, UnitType.Pixel);
                else
                    lblMensajeHabilidadesAbajo.Font.Size = new FontUnit(9.5, UnitType.Pixel);

                if (oTUsuario.tip_per_in == 2)
                {
                    cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
                    cRutaContenido = Server.MapPath(cCarpetaContenido);
                    //rbNo.Checked = true;
                    ListarControl();
                    ListarIdioma();

                    this.cargarCombosAdicionales();

                    //ListarFecha(oTUsuario.oIdioma.cod_idi_in);
                    pnlActivo.Visible = true;
                    pnlInactivo.Visible = false;

                    gridLenguaje.ClientSettings.Scrolling.AllowScroll = true;
                    gridLenguaje.ClientSettings.Scrolling.UseStaticHeaders = true;

                    gridPais.ClientSettings.Scrolling.AllowScroll = true;
                    gridPais.ClientSettings.Scrolling.UseStaticHeaders = true;


                    gridCV.ClientSettings.Scrolling.AllowScroll = true;
                    gridCV.ClientSettings.Scrolling.UseStaticHeaders = true;

                    gridEduacion.ClientSettings.Scrolling.AllowScroll = true;
                    gridEduacion.ClientSettings.Scrolling.UseStaticHeaders = true;
                    //gridHabilidades.ClientSettings.Scrolling.AllowScroll = true;
                    //gridHabilidades.ClientSettings.Scrolling.UseStaticHeaders = true;

                    gridPlantilla.ClientSettings.Scrolling.AllowScroll = true;
                    gridPlantilla.ClientSettings.Scrolling.UseStaticHeaders = true;
                    gridReferencia.ClientSettings.Scrolling.AllowScroll = true;
                    gridReferencia.ClientSettings.Scrolling.UseStaticHeaders = true;

                    try
                    {
                        nCodigo = oTUsuario.cod_per_in;
                        this.ObtenerDatosDeConsultor(nCodigo);
                    }
                    catch (Exception ex)
                    {
                        nCodigo = 0;
                    }

                    if (nCodigo != 0)
                    {
                        usrMenu.cPagina = "~/WebConsultantEdit.aspx?";
                        lblAgregar.Visible = false;
                        lblEditar.Visible = true;
                    }
                    else
                    {
                        usrMenu.cPagina = "~/WebConsultantEdit.aspx?";
                        pnlActivo.Visible = false;
                        pnlInactivo.Visible = true;
                        litSinConexion.Text = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "SinCodigo");
                    }
                }
                else
                {
                    pnlActivo.Visible = false;
                    pnlInactivo.Visible = true;
                    litSinConexion.Text = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "SinPermiso");
                }
            }
            else
            {

                pnlActivo.Visible = false;
                pnlInactivo.Visible = true;
                litSinConexion.Text = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "SinConexion");
            }
        }
        else
        {
            pnlOk.Visible = false;

            if (Session["Usuario"] == null)
            {
                pnlActivo.Visible = false;
                pnlInactivo.Visible = true;
                litSinConexion.Text = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "SinConexion");
            }

        }
    }

    void actualizaFoto()
    {
        cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
        cRutaContenido = Server.MapPath(cCarpetaContenido);

        UploadedFile oImagen;
        String cImagen = "";
        DateTime dfecha = new DateTime();
        dfecha = DateTime.Now;
        if (RadUpFoto.UploadedFiles.Count > 0)
        {
            oImagen = RadUpFoto.UploadedFiles[0];
            cImagen = oHelper.ReplaceCaracter(txtNombre.Text.Trim()) + "_" + oHelper.ReplaceCaracter(txtApellido.Text.Trim()) +
                      "_IMG_" + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                                      dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                                      dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim() + oImagen.GetExtension();
            try
            {
                oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
            }
            catch
            {

            }

            lblFotoDescarga.Text = cImagen.Trim();

            if (lblFotoDescarga.Text.Trim().Length != 0)
            {
                lnkFDescargar.NavigateUrl = "~/downloading.aspx?file=" + lblFotoDescarga.Text.Trim();
                lnkFDescargar.ToolTip = lblFotoDescarga.Text.Trim();
                lnkFDescargar.Visible = true;

                if (lblFotoDescarga.Text.Trim().Length > 15)
                {
                    String cDescarga = lblFotoDescarga.Text.Trim().Substring(0, 15) + "...";
                    lblFDescarga.Text = cDescarga;
                }
            }

        }

    }

    void actualizarDatos()
    {
        if (Session["Usuario"] != null)
        {

            txtContra.Attributes["value"] = txtContra.Text;
            txtContraRep.Attributes["value"] = txtContraRep.Text;

            actualizaFoto();

            //CV 

            oTUsuario = (Usuario)Session["Usuario"];
            oListaConCV = new List<ConsultorDocumento>();
            oListaConCV = DevolverCV();
            FormatoCV(oListaConCV);


            //Educacion

            oListaConEducacion = new List<ConsultorEducacion>();
            oListaConEducacion = DevolverEducacion();
            FormatoEducacion(oListaConEducacion);

        }
    }

    void actualizarEducacion()
    {
        if (Session["Usuario"] != null)
        {
            actualizaFoto();
            txtContra.Attributes["value"] = txtContra.Text;
            txtContraRep.Attributes["value"] = txtContraRep.Text;


            oListaConEducacion = new List<ConsultorEducacion>();
            oListaConEducacion = DevolverEducacion();
            FormatoEducacion(oListaConEducacion);

        }
    }

    void actualizarCV()
    {

        if (Session["Usuario"] != null)
        {
            actualizaFoto();
            txtContra.Attributes["value"] = txtContra.Text;
            txtContraRep.Attributes["value"] = txtContraRep.Text;

            oListaConCV = new List<ConsultorDocumento>();
            oListaConCV = DevolverCV();
            FormatoCV(oListaConCV);
        }
    }

    void ListarControl()
    {
        oListaControl = new Dictionary<string, string>();
        oListaControlES = new Dictionary<string, string>();
        oListaControl.Add("lblConsultorTipoInteno", "Internal");
        oListaControlES.Add("lblConsultorTipoInteno", "Interno");

        oListaControl.Add("lblConsultorTipoExterno", "External");
        oListaControlES.Add("lblConsultorTipoExterno", "Externo");
    }

    String Mostrar(string cControl)
    {

        string cTexto = "";

        if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
            Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
             Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
        {
            if (oListaControlES.ContainsKey(cControl))
            {
                cTexto = oListaControlES[cControl];
            }

        }
        else
        {
            if (oListaControl.ContainsKey(cControl))
            {
                cTexto = oListaControl[cControl];
            }

        }

        return cTexto;
    }

    private void MostrarTipo()
    {
        int nTipo = 0;
        nTipo = int.Parse(lblCTipo.Text);
        nEstado = int.Parse(lblEstado.Text);
        if (nTipo == 1) //rbInterno.Checked == true
        {
            pnlDA.Visible = true;
            pnlContrasenia.Visible = false;
        }

        if (nTipo == 2) //rbExterno.Checked == true
        {
            pnlDA.Visible = false;
            pnlContrasenia.Visible = true;

            nCodigo = int.Parse(lblCodigo.Text);
            txtContra.Enabled = false;
            txtContraRep.Enabled = false;
            if (nCodigo != 0)
            {
                if (nEstado == 3)
                {
                    chkContra.Visible = false;
                    txtContra.Enabled = true;
                    txtContraRep.Enabled = true;
                }
                else
                {
                    chkContra.Visible = true;
                }

            }
            else
            {
                chkContra.Visible = false;
            }

        }
    }

    private void ListarUsuarioDA()
    {
        //oListaDA = new List<Usuario>() { 
        //                new Usuario("Seleccione","0"),
        //                new Usuario("admin","admin"),
        //                new Usuario("jpena","jpena"),      
        //                 new Usuario("jwatanabe","jwatanabe"),
        //                new Usuario("jvasquez","jvasquez"),
        //                new Usuario("mcordova","mcordova"),
        //                new Usuario("macca","macca"),
        //                new Usuario("lennon","lennon"),
        //                new Usuario("harrison","harrison"),
        //                new Usuario("ringo","ringo"),
        //                new Usuario("jpage","jpage"),
        //                new Usuario("morrison","morrison"),
        //                new Usuario("morrissey","morrissey"),
        //    };

        //cboDA.DataSource = oListaDA;
        //cboDA.DataValueField = "email_per_vc";
        //cboDA.DataTextField = "nom_usu_vc";
        //cboDA.DataBind();
    }

    String FormatoFecha(DateTime dt)
    {

        String cFecha = "";

        cFecha = (dt.Day.ToString().Trim().Length == 1 ? "0" : "") +
                dt.Day.ToString().Trim() + " / " +
                (dt.Month.ToString().Trim().Length == 1 ? "0" : "") +
                dt.Month.ToString().Trim() + " / " + dt.Year.ToString().Trim();

        return cFecha;
    }

    private void ObtenerDatosDeConsultor(int codigo)
    {
        cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
        cRutaContenido = Server.MapPath(cCarpetaContenido);
        oHelperDiccionario = new Diccionario();

        lblFotoDescarga.Text = "";
        lblFotoDescarga.Visible = false;
        lnkFDescargar.Visible = false;
        lblFDescarga.Text = "";

        if (Session["Usuario"] != null)
        {
            oTUsuario = (Usuario)Session["Usuario"];

            ListarDatos(oTUsuario.oIdioma.cod_idi_in);

            oConsultorSer = new ConsultorService();
            oConsultor = oConsultorSer.ConsultorInd(codigo);

            if (oConsultor != null)
            {
                if (oConsultor.cod_per_in != 0)
                {
                    ListarControl();
                    lblCodigo.Text = oConsultor.cod_per_in.ToString();
                    lblUsuarioCodigo.Text = oConsultor.cod_usu_in.ToString();
                    codigo = oConsultor.cod_per_in;

                    if (oConsultor.tip_con_in == 1)
                    {
                        cTipo = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorTipoInteno");
                    }
                    else
                    {
                        cTipo = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorTipoExterno");
                    }

                    lblConsultorTipo.Text = cTipo;
                    lblCTipo.Text = oConsultor.tip_con_in.ToString().Trim();
                    lblEstado.Text = oConsultor.est_per_in.ToString().Trim();
                    MostrarTipo();

                    txtApellido.Text = oConsultor.ape_per_vc;
                    txtNombre.Text = oConsultor.nom_per_vc;

                    if (oConsultor.fec_nac_con_dt != null)
                    {
                        try
                        {
                            if (oConsultor.fec_nac_con_dt != null)
                            {
                                if (oConsultor.fec_nac_con_dt.Value.Year != 1)
                                {
                                    txtFechaNacimiento.TextWithLiterals = FormatoFecha(oConsultor.fec_nac_con_dt.Value);
                                }
                            }
                        }
                        catch { }
                    }

                    cboSexo.SelectedValue = oConsultor.sex_con_in.ToString();
                    cboIdioma.SelectedValue = oConsultor.oIdioma.cod_idi_in.ToString();

                    txtDireccion.Text = oConsultor.dir_con_vc;
                    cboPais.SelectedValue = oConsultor.oPais.cod_pais_in.ToString();

                    if (oConsultor.pag_con_dc != 0)
                    {
                        txtMoneda.Text = oConsultor.pag_con_dc.ToString().Trim();
                    }
                    cboMoneda.SelectedValue = oConsultor.tip_pag_con_in.ToString().Trim();

                    if (oConsultor.img_con_vc != null)
                    {

                        if (oConsultor.img_con_vc.Trim().Length != 0)
                        {
                            imgConsultor.ImageUrl = cCarpetaContenido + "/" + oConsultor.img_con_vc.Trim();
                        }
                        else
                        {
                            imgConsultor.ImageUrl = "~/Imagen/usuario.png";
                        }
                    }
                    else
                    {
                        imgConsultor.ImageUrl = "~/Imagen/usuario.png";
                    }

                    String cSeleccione = "";
                    lblActualizacion.Visible = true;

                    cSeleccione = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "Seleccione");
                    lblActualizacion.Text = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorUltimaActualizacion", oConsultor.fec_act_con_dt.ToShortDateString().Trim());

                    //if (oTUsuario.oIdioma.cod_idi_in == 1)
                    //{
                    //    cSeleccione = "Select";
                    //    lblActualizacion.Text = "Last updated on " + oConsultor.fec_act_con_dt.ToShortDateString().Trim();
                    //}
                    //else
                    //{

                    //    cSeleccione = "Seleccione";
                    //    lblActualizacion.Text = "Última actualización el " + oConsultor.fec_act_con_dt.ToShortDateString().Trim();
                    //}

                    //if (oTUsuario.oIdioma.cod_idi_in == 1)
                    //{
                    //    cSeleccione = "Select";
                    //}
                    //else
                    //{

                    //    cSeleccione = "Seleccione";
                    //}
                    cSeleccione = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "Seleccione");

                    txtCiudad.Text = oConsultor.ciu_con_vc.Trim();
                    txtBiografia.Text = oConsultor.bio_con_vc.Trim();
                    txtTelefono1.Text = oConsultor.tel1_con_vc;
                    txtTelefono2.Text = oConsultor.tel2_con_vc;
                    txtLinked.Text = oConsultor.link_con_vc;
                    txtCorreo.Text = oConsultor.email_per_vc;

                    if (oConsultor.cer_exp_con_bo == true)
                    {
                        rbSI.Checked = true;
                    }
                    else
                    {
                        rbNo.Checked = true;
                    }
                    if (oConsultor.tot_exp_con_in == 1)
                    {
                        rbExperiencia1.Checked = true;
                    }

                    if (oConsultor.tot_exp_con_in == 2)
                    {
                        rbExperiencia2.Checked = true;
                    }

                    if (oConsultor.tot_exp_con_in == 3)
                    {
                        rbExperiencia3.Checked = true;
                    }

                    if (oConsultor.tot_exp_con_in == 4)
                    {
                        rbExperiencia4.Checked = true;
                    }

                    if (oConsultor.tip_usu_in == 1)
                    {
                        pnlDA.Visible = true;
                        pnlContrasenia.Visible = false;
                        lblConsultorDA.Text = oConsultor.nom_usu_vc.Trim();
                        RadTabConsultor.Tabs[5].Visible = true;
                        RadPageView6.Visible = true;

                        if (oConsultor.Relationship != null)
                            this.ddlRelationShip.SelectedValue = oConsultor.Relationship.ToString();
                        else
                            this.ddlRelationShip.SelectedValue = Constantes.RELATIONSHIPPORDEFECTO;

                        this.txtComentario.Text = oConsultor.com_con_vc.Trim();
                    }
                    else if (oConsultor.tip_usu_in == 2)
                    {
                        pnlDA.Visible = false;
                        pnlContrasenia.Visible = true;
                        RadTabConsultor.Tabs[5].Visible = false;
                        RadPageView6.Visible = false;
                    }

                    //detalles

                    ListarDonantes(oTUsuario.oIdioma.cod_idi_in);
                    ListarConocimiento(oTUsuario.oIdioma.cod_idi_in);
                    ListarPlantilla(oTUsuario.oIdioma.cod_idi_in);
                    oListaConDonante = new List<ConsultorDonante>();
                    oListaConDonante = oConsultorSer.DonanteListar(codigo);

                    int nCodigoDonante = 0;
                    for (int i = 0; i < lstDonante.Items.Count; i++)
                    {
                        nCodigoDonante = int.Parse(lstDonante.Items[i].Value.ToString());

                        foreach (ConsultorDonante ent in oListaConDonante)
                        {

                            if (ent.cod_don_in == nCodigoDonante)
                            {
                                lstDonante.Items[i].Checked = true;
                                break;
                            }
                        }
                    }

                    oListaConNacion = new List<ConsultorNacionalidad>();
                    oListaConNacion = oConsultorSer.NacionalidadListar(codigo);

                    if (oListaConNacion.Count != 0)
                    {
                        if (oListaConNacion.Count == 1)
                        {
                            cboNacionalidad1.SelectedValue = oListaConNacion[0].cod_pais_in.ToString();
                        }

                        if (oListaConNacion.Count == 2)
                        {
                            cboNacionalidad1.SelectedValue = oListaConNacion[0].cod_pais_in.ToString();
                            cboNacionalidad2.SelectedValue = oListaConNacion[1].cod_pais_in.ToString();
                        }
                    }

                    oListaConEducacion = new List<ConsultorEducacion>();
                    oListaConEducacion = oConsultorSer.EducacionListar(codigo);

                    for (int i = 0; i < oListaConEducacion.Count; i++)
                    {
                        oListaConEducacion[i].Nro = i + 1;
                    }

                    gridEduacion.DataSource = oListaConEducacion;
                    gridEduacion.DataBind();

                    //if (oTUsuario.oIdioma.cod_idi_in == 1)
                    //{
                    //    cSeleccione = "Select";
                    //}
                    //else
                    //{

                    //    cSeleccione = "Seleccione";
                    //}
                    cSeleccione = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "Seleccione");

                    oAdministracionSer = new AdministracionService();
                    oListaNivel = oAdministracionSer.NivelAcademicoIdiomaListar(oTUsuario.oIdioma.cod_idi_in);
                    oListaDuracion = oAdministracionSer.CatalogoBuscar(2001, oTUsuario.oIdioma.cod_idi_in);

                    oListaDescripcion = new List<DescripcionAcademica>();
                    oListaDescripcion.Add(new DescripcionAcademica() { nom_desaca_vc = cSeleccione });

                    if (oListaNivel.Count == 0)
                    {
                        oListaNivel.Add(new NivelAcademico() { nom_nivacaidi_vc = cSeleccione });
                    }
                    else
                    {
                        oListaNivel.Insert(0, new NivelAcademico() { nom_nivacaidi_vc = cSeleccione });

                    }

                    if (oListaDuracion.Count == 0)
                    {
                        oListaDuracion.Add(new Catalogo() { nom_cat_vc = cSeleccione });
                    }
                    else
                    {
                        oListaDuracion.Insert(0, new Catalogo() { nom_cat_vc = cSeleccione });
                    }

                    int nNivel = 0;
                    String cDescarga = "";
                    for (int i = 0; i < gridEduacion.Items.Count; i++)
                    {
                        DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
                        DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
                        //DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");

                        Label lblNivel = (Label)gridEduacion.Items[i].FindControl("lblNivel");
                        //Label lblDescripcion = (Label)gridEduacion.Items[i].FindControl("lblDescripcion");
                        Label lblDuracion = (Label)gridEduacion.Items[i].FindControl("lblDuracion");

                        GridNestedViewItem nestedItem = (GridNestedViewItem)gridEduacion.MasterTableView.Items[i].ChildItem;


                        Label lblDescarga = (Label)nestedItem.FindControl("lblDescarga");
                        HyperLink lnkDescargar = (HyperLink)nestedItem.FindControl("lnkDescargar");
                        cboNivel.DataSource = oListaNivel;
                        cboNivel.DataValueField = "cod_nivaca_in";
                        cboNivel.DataTextField = "nom_nivacaidi_vc";
                        cboNivel.DataBind();
                        cboNivel.SelectedValue = lblNivel.Text.Trim();

                        if (lblDescarga.Text.Trim().Length != 0)
                        {
                            lnkDescargar.NavigateUrl = "~/downloading.aspx?file=" + lblDescarga.Text.Trim();
                            lnkDescargar.ToolTip = lblDescarga.Text;
                            lnkDescargar.Visible = true;

                            if (lblDescarga.Text.Trim().Length > 8)
                            {
                                cDescarga = lblDescarga.Text.Trim().Substring(0, 8) + "...";
                                lblDescarga.Text = cDescarga;
                            }

                        }


                        nNivel = int.Parse(lblNivel.Text.Trim());


                        cboDuracion.DataSource = oListaDuracion;
                        cboDuracion.DataValueField = "val_cat_in";
                        cboDuracion.DataTextField = "nom_cat_vc";
                        cboDuracion.DataBind();
                        cboDuracion.SelectedValue = lblDuracion.Text.Trim();

                    }

                    /* conocimientos */

                    oListaConConocimiento = new List<ConsultorConocimiento>();
                    oListaConConocimiento = oConsultorSer.ConocimientoListar(codigo);

                    int nCodigoConocimiento = 0;

                    for (int j = 0; j < listHabilidades.Items.Count; j++)
                    {
                        RadListBox lstTecnico = (RadListBox)listHabilidades.Items[j].FindControl("lstTecnico");

                        for (int i = 0; i < lstTecnico.Items.Count; i++)
                        {
                            nCodigoConocimiento = int.Parse(lstTecnico.Items[i].Value.ToString());

                            foreach (ConsultorConocimiento ent in oListaConConocimiento)
                            {

                                if (ent.cod_conesp_in == nCodigoConocimiento)
                                {
                                    lstTecnico.Items[i].Checked = true;
                                    break;
                                }

                            }

                        }
                    }



                    oListaConLenguaje = new List<ConsultorLenguaje>();
                    oListaConLenguaje = oConsultorSer.LenguajeListar(codigo);

                    for (int i = 0; i < oListaConLenguaje.Count; i++)
                    {
                        oListaConLenguaje[i].Nro = i + 1;
                    }

                    gridLenguaje.DataSource = oListaConLenguaje;
                    gridLenguaje.DataBind();

                    //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    //Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    //Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    //{
                    //    cSeleccione = "Seleccione";
                    //}
                    //else
                    //{
                    //    cSeleccione = "Select";
                    //}
                    cSeleccione = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "Seleccione");

                    oListaIdioma = new List<Lenguaje>();
                    oListaHablado = new List<Catalogo>();
                    oAdministracionSer = new AdministracionService();
                    oListaIdioma = oAdministracionSer.LenguajeIdiomaListar(oTUsuario.oIdioma.cod_idi_in);
                    oListaHablado = oAdministracionSer.CatalogoBuscar(2002, oTUsuario.oIdioma.cod_idi_in);

                    if (oListaIdioma.Count == 0)
                    {
                        oListaIdioma.Add(new Lenguaje() { nom_len_vc = cSeleccione });
                    }
                    else
                    {
                        oListaIdioma.Insert(0, new Lenguaje() { nom_len_vc = cSeleccione });
                    }

                    if (oListaHablado.Count == 0)
                    {

                        oListaHablado.Add(new Catalogo() { nom_cat_vc = cSeleccione });

                    }
                    else
                    {

                        oListaHablado.Insert(0, new Catalogo() { nom_cat_vc = cSeleccione });

                    }

                    for (int i = 0; i < gridLenguaje.Items.Count; i++)
                    {
                        DropDownList cboLenguaje = (DropDownList)gridLenguaje.Items[i].FindControl("cboLenguaje");
                        DropDownList cboHablado = (DropDownList)gridLenguaje.Items[i].FindControl("cboHablado");
                        DropDownList cboLeido = (DropDownList)gridLenguaje.Items[i].FindControl("cboLeido");
                        DropDownList cboEscrito = (DropDownList)gridLenguaje.Items[i].FindControl("cboEscrito");

                        Label lblLenguaje = (Label)gridLenguaje.Items[i].FindControl("lblLenguaje");
                        Label lblHablado = (Label)gridLenguaje.Items[i].FindControl("lblHablado");
                        Label lblLeido = (Label)gridLenguaje.Items[i].FindControl("lblLeido");
                        Label lblEscrito = (Label)gridLenguaje.Items[i].FindControl("lblEscrito");

                        cboLenguaje.DataSource = oListaIdioma;
                        cboLenguaje.DataValueField = "cod_len_in";
                        cboLenguaje.DataTextField = "nom_len_vc";
                        cboLenguaje.DataBind();
                        cboLenguaje.SelectedValue = lblLenguaje.Text.Trim();

                        cboHablado.DataSource = oListaHablado;
                        cboHablado.DataValueField = "val_cat_in";
                        cboHablado.DataTextField = "nom_cat_vc";
                        cboHablado.DataBind();
                        cboHablado.SelectedValue = lblHablado.Text.Trim();

                        cboLeido.DataSource = oListaHablado;
                        cboLeido.DataValueField = "val_cat_in";
                        cboLeido.DataTextField = "nom_cat_vc";
                        cboLeido.DataBind();
                        cboLeido.SelectedValue = lblLeido.Text.Trim();

                        cboEscrito.DataSource = oListaHablado;
                        cboEscrito.DataValueField = "val_cat_in";
                        cboEscrito.DataTextField = "nom_cat_vc";
                        cboEscrito.DataBind();
                        cboEscrito.SelectedValue = lblEscrito.Text.Trim();

                    }

                    // pais

                    oListaConPais = new List<ConsultorPais>();
                    oListaConPais = oConsultorSer.PaisListar(codigo);
                    for (int i = 0; i < oListaConPais.Count; i++)
                    {
                        oListaConPais[i].Nro = i + 1;
                    }

                    gridPais.DataSource = oListaConPais;
                    gridPais.DataBind();

                    //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    //     Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    //     Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    //{
                    //    cSeleccione = "Seleccione";
                    //}
                    //else
                    //{
                    //    cSeleccione = "Select";
                    //}
                    cSeleccione = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "Seleccione");

                    oAdministracionSer = new AdministracionService();
                    oListaPais = oAdministracionSer.PaisIdiomaListar(oTUsuario.oIdioma.cod_idi_in);
                    oListaPais = (from pais in oListaPais
                                  orderby pais.nom_pais_vc
                                  select pais).ToList();

                    if (oListaPais.Count != 0)
                    {
                        oListaPais.Insert(0, new Pais()
                        {
                            cod_pais_in = 0,
                            nom_pais_vc = cSeleccione
                        });
                    }
                    else
                    {
                        oListaPais.Add(new Pais()
                        {
                            cod_pais_in = 0,
                            nom_pais_vc = cSeleccione
                        });
                    }


                    for (int i = 0; i < gridPais.Items.Count; i++)
                    {
                        Label lblPais = (Label)gridPais.Items[i].FindControl("lblPais");
                        DropDownList cboT2Pais = (DropDownList)gridPais.Items[i].FindControl("cboPais");

                        cboT2Pais.DataSource = oListaPais;
                        cboT2Pais.DataTextField = "nom_pais_vc";
                        cboT2Pais.DataValueField = "cod_pais_in";
                        cboT2Pais.DataBind();

                        cboT2Pais.SelectedValue = lblPais.Text.Trim();

                    }

                    //dcumentos

                    oListaConCV = new List<ConsultorDocumento>();
                    oListaConCV = oConsultorSer.DocumentoListar(codigo);

                    for (int i = 0; i < oListaConCV.Count; i++)
                    {
                        oListaConCV[i].Nro = i + 1;
                    }


                    gridCV.DataSource = oListaConCV;
                    gridCV.DataBind();

                    for (int i = 0; i < gridCV.Items.Count; i++)
                    {
                        Label lblDescarga = (Label)gridCV.Items[i].FindControl("lblDescarga");
                        HyperLink lnkDescargar = (HyperLink)gridCV.Items[i].FindControl("lnkDescargar");



                        if (lblDescarga.Text.Trim().Length != 0)
                        {
                            lnkDescargar.NavigateUrl = "~/downloading.aspx?file=" + lblDescarga.Text.Trim();
                            lnkDescargar.ToolTip = lblDescarga.Text;
                            lnkDescargar.Visible = true;

                            if (lblDescarga.Text.Trim().Length > 15)
                            {
                                cDescarga = lblDescarga.Text.Trim().Substring(0, 15) + "...";
                                lblDescarga.Text = cDescarga;
                            }

                        }

                    }
                    oListaCReferencia = new List<ConsultorReferencia>();
                    oListaCReferencia = oConsultorSer.ReferenciaInd(codigo, oTUsuario.oIdioma.cod_idi_in);

                    int nNroReferencia = 0;
                    foreach (ConsultorReferencia ent in oListaCReferencia)
                    {
                        nNroReferencia++;
                        ent.Nro = nNroReferencia;
                    }
                    
                    this.gridReferencia.DataSource = oListaCReferencia;
                    this.gridReferencia.DataBind();

                    // El feedback del consultor solo se muestra para lo usuarios internos.
                    if (oTUsuario.tip_usu_in.Equals(Constantes.USUARIOINTERNO))
                        this.gridReferencia.Columns.FindByUniqueName("temFeedback").Visible = true;
                    else
                        this.gridReferencia.Columns.FindByUniqueName("temFeedback").Visible = false;

                    // Cuentas bancarias del consultor

                    Consultor consultor = new Consultor();

                    consultor = oConsultorSer.ObtenerDatosAdicionalesDeConsultor(codigo);

                    this.cbo_tipo_doc.SelectedValue = consultor.TipoDocumento.ToString();
                    this.txt_nro_doc.Text = consultor.NroDocumento;

                    // DNI
                    this.lnkDDescargar.NavigateUrl = "~/downloading.aspx?file=" + consultor.DNI.Trim();
                    this.lnkDDescargar.ToolTip = consultor.DNI.Trim();
                    this.lnkDDescargar.Visible = true;
                    this.lblDDescarga.Visible = true;

                    if (consultor.DNI.Length > 15)
                        this.lblDDescarga.Text = consultor.DNI.Trim().Substring(0, 15) + "...";
                    else
                        this.lblDDescarga.Text = consultor.DNI.Trim();

                    ViewState["vws_fileDNI"] = String.IsNullOrEmpty(consultor.DNI.Trim()) ? null : consultor.DNI.Trim();

                    this.cbo_exonera_retencion.SelectedValue = (consultor.ExoneracionRetencion == true ? "1" : "0");

                    // SUSPENSIÓN DE 4TA
                    this.lnkS4taDescargar.NavigateUrl = "~/downloading.aspx?file=" + consultor.Suspension4ta.Trim();
                    this.lnkS4taDescargar.ToolTip = consultor.Suspension4ta.Trim();
                    this.lnkS4taDescargar.Visible = true;
                    this.lblS4taDescarga.Visible = true;

                    if (consultor.Suspension4ta.Length > 15)
                        this.lblS4taDescarga.Text = consultor.Suspension4ta.Trim().Substring(0, 15) + "...";
                    else
                        this.lblS4taDescarga.Text = consultor.Suspension4ta.Trim();

                    ViewState["vws_fileSuspension4ta"] = String.IsNullOrEmpty(consultor.Suspension4ta.Trim()) ? null : consultor.Suspension4ta.Trim();

                    this.gvwCuentas.DataSource = oConsultorSer.CuentasBancariasListar(codigo);
                    this.gvwCuentas.DataBind();

                    IList<IT.Domain.ConsultorCuenta> obj = oConsultorSer.CuentasBancariasListar(codigo);

                    for (int i = 0; i < this.gvwCuentas.Items.Count; i++)
                    {
                        DropDownList cbo_banco = (DropDownList)this.gvwCuentas.Items[i].FindControl("cbo_banco");
                        DropDownList cbo_moneda = (DropDownList)this.gvwCuentas.Items[i].FindControl("cbo_moneda");
                        RadComboBox cbo_tipo_cuenta = (RadComboBox)this.gvwCuentas.Items[i].FindControl("cbo_tipo_cuenta");
                        TextBox txt_nro_cuenta = (TextBox)this.gvwCuentas.Items[i].FindControl("txt_nro_cuenta");

                        cbo_tipo_cuenta.Text = obj[i].TipoCuenta;
                        txt_nro_cuenta.Text = obj[i].NroCuenta;

                        this.ListarBancos(cbo_banco);

                        cbo_banco.SelectedValue = obj[i].CodigoBanco.ToString();
                        cbo_banco.ValidationGroup = (obj[i].CodigoItem - 1).ToString();

                        this.ListarMonedas(cbo_moneda);

                        cbo_moneda.SelectedValue = obj[i].CodigoMoneda.ToString();
                        cbo_moneda.ValidationGroup = (obj[i].CodigoItem - 1).ToString();

                        this.ListarTipoCuentas(cbo_tipo_cuenta);
                    }
                }
            }
        }
    }

    private void cargarCombosAdicionales()
    {
        this.cbo_tipo_doc.Items.Add(new ListItem("-- SELECCIONAR --", ""));
        this.cbo_tipo_doc.Items.Add(new ListItem("DNI", "1"));
        this.cbo_tipo_doc.Items.Add(new ListItem("CARNET DE EXTRANJERÍA", "4"));
        this.cbo_tipo_doc.Items.Add(new ListItem("RUC", "6"));
        this.cbo_tipo_doc.Items.Add(new ListItem("PASAPORTE", "7"));
        this.cbo_tipo_doc.Items.Add(new ListItem("CÉDULA DIPLOMÁTICA DE IDENTIDAD", "9"));
        this.cbo_tipo_doc.Items.Add(new ListItem("OTROS TIPOS DE DOCUMENTOS", "0"));

        this.cbo_exonera_retencion.Items.Add(new ListItem("-- SELECCIONAR --", ""));
        this.cbo_exonera_retencion.Items.Add(new ListItem("SÍ", "1"));
        this.cbo_exonera_retencion.Items.Add(new ListItem("NO", "0"));
    }

    void ListarIdioma()
    {
        oListaLenguaje = new List<Idioma>();
        oAdministracionSer = new AdministracionService();
        oListaLenguaje = oAdministracionSer.IdiomaListar();
        cboIdioma.DataSource = oListaLenguaje;
        cboIdioma.DataTextField = "nom_idi_vc";
        cboIdioma.DataValueField = "cod_idi_in";
        cboIdioma.DataBind();
    }

    void ListarDatos(int cod_idi_in)
    {
        String cSeleccione = "";
        oHelperDiccionario = new Diccionario();
        cSeleccione = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "Seleccione");

        //if (cod_idi_in == 1)
        //{
        //    cSeleccione = "Select";
        //}
        //else
        //{
        //    cSeleccione = "Seleccione";
        //}

        oListaPais = new List<Pais>();
        oListaSexo = new List<Catalogo>();
        oListaMoneda = new List<Catalogo>();
        oListaRelationship = new List<Catalogo>();
        //oListaCiudad = new List<Ciudad>();

        oAdministracionSer = new AdministracionService();
        oListaPais = oAdministracionSer.PaisIdiomaListar(cod_idi_in);
        oListaSexo = oAdministracionSer.CatalogoBuscar(2003, cod_idi_in);
        oListaRelationship = oAdministracionSer.CatalogoBuscar(2007, cod_idi_in);

        oListaPais = (from pais in oListaPais
                      orderby pais.nom_pais_vc
                      select pais).ToList();
        oListaMoneda = oAdministracionSer.CatalogoBuscar(2006, cod_idi_in);

        if (oListaPais.Count == 0)
        {
            oListaPais.Add(new Pais() { nom_pais_vc = cSeleccione });
        }
        else
        {
            oListaPais.Insert(0, new Pais() { nom_pais_vc = cSeleccione });
        }

        if (oListaSexo.Count == 0)
        {
            oListaSexo.Add(new Catalogo() { nom_cat_vc = cSeleccione });
        }
        else
        {
            oListaSexo.Insert(0, new Catalogo() { nom_cat_vc = cSeleccione });
        }

        if (oListaMoneda.Count == 0)
        {
            oListaMoneda.Add(new Catalogo() { nom_cat_vc = cSeleccione });
        }
        else
        {
            oListaMoneda.Insert(0, new Catalogo() { nom_cat_vc = cSeleccione });
        }

        //if (oListaCiudad.Count == 0)
        //{
        //    oListaCiudad.Add(new Ciudad() { nom_ciu_vc = cSeleccione });
        //}
        //else
        //{
        //    oListaCiudad.Insert(0, new Ciudad() { nom_ciu_vc = cSeleccione });
        //}

        cboCompaniaPais.DataSource = oListaPais;
        cboCompaniaPais.DataTextField = "nom_pais_vc";
        cboCompaniaPais.DataValueField = "cod_pais_in";
        cboCompaniaPais.DataBind();

        cboPais.DataSource = oListaPais;
        cboPais.DataTextField = "nom_pais_vc";
        cboPais.DataValueField = "cod_pais_in";
        cboPais.DataBind();

        if (oTUsuario.oPais.cod_pais_in.Equals(1))// 1 define el país de PERÚ
        {
            cboPais.SelectedValue = "1";
            cboPais.Enabled = false;
        }

        if (oTUsuario.oPais.cod_pais_in.Equals(22))// 1 define el país de BOLIVIA
        {
            cboPais.SelectedValue = "22";
            cboPais.Enabled = false;
        }

        cboNacionalidad1.DataSource = oListaPais;
        cboNacionalidad1.DataTextField = "nom_pais_vc";
        cboNacionalidad1.DataValueField = "cod_pais_in";
        cboNacionalidad1.DataBind();

        cboNacionalidad2.DataSource = oListaPais;
        cboNacionalidad2.DataTextField = "nom_pais_vc";
        cboNacionalidad2.DataValueField = "cod_pais_in";
        cboNacionalidad2.DataBind();

        cboSexo.DataSource = oListaSexo;
        cboSexo.DataValueField = "val_cat_in";
        cboSexo.DataTextField = "nom_cat_vc";
        cboSexo.DataBind();

        this.ddlRelationShip.DataSource = oListaRelationship;
        this.ddlRelationShip.DataValueField = "val_cat_in";
        this.ddlRelationShip.DataTextField = "nom_cat_vc";
        this.ddlRelationShip.DataBind();
        this.ddlRelationShip.SelectedValue = Constantes.RELATIONSHIPPORDEFECTO;

        cboMoneda.DataSource = oListaMoneda;
        cboMoneda.DataValueField = "val_cat_in";
        cboMoneda.DataTextField = "nom_cat_vc";
        cboMoneda.DataBind();
        //cboCiudad.DataSource = oListaCiudad;
        //cboCiudad.DataValueField = "cod_ciu_in";
        //cboCiudad.DataTextField = "nom_ciu_vc";
        //cboCiudad.DataBind();
    }

    void ListarAcademico(int cod_idi_in)
    {
        String cSeleccione = "";
        oHelperDiccionario = new Diccionario();
        cSeleccione = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "Seleccione");
        //if (cod_idi_in == 1)
        //{
        //    cSeleccione = "Select";
        //}
        //else
        //{
        //    cSeleccione = "Seleccione";
        //}

        oListaConEducacion = new List<ConsultorEducacion>();

        oListaConEducacion.Add(new ConsultorEducacion()
        {
            Nro = 1,
            oNivel = new NivelAcademico(),
            oDescripcion = new DescripcionAcademica()
        });
        gridEduacion.DataSource = oListaConEducacion;
        gridEduacion.DataBind();

        oAdministracionSer = new AdministracionService();
        oListaNivel = oAdministracionSer.NivelAcademicoIdiomaListar(cod_idi_in);
        oListaDuracion = oAdministracionSer.CatalogoBuscar(2001, cod_idi_in);

        oListaDescripcion = new List<DescripcionAcademica>();
        oListaDescripcion.Add(new DescripcionAcademica() { nom_desaca_vc = cSeleccione });

        if (oListaNivel.Count == 0)
        {
            oListaNivel.Add(new NivelAcademico() { nom_nivacaidi_vc = cSeleccione });
        }
        else
        {
            oListaNivel.Insert(0, new NivelAcademico() { nom_nivacaidi_vc = cSeleccione });

        }

        if (oListaDuracion.Count == 0)
        {
            oListaDuracion.Add(new Catalogo() { nom_cat_vc = cSeleccione });
        }
        else
        {
            oListaDuracion.Insert(0, new Catalogo() { nom_cat_vc = cSeleccione });
        }

        String cDescarga = "";
        for (int i = 0; i < gridEduacion.Items.Count; i++)
        {
            DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
            DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
            DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");

            Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");
            Label lblDescarga = (Label)gridEduacion.Items[i].FindControl("lblDescarga");
            HyperLink lnkDescargar = (HyperLink)gridEduacion.Items[i].FindControl("lblDescarga");

            cboNivel.DataSource = oListaNivel;
            cboNivel.DataValueField = "cod_nivaca_in";
            cboNivel.DataTextField = "nom_nivacaidi_vc";
            cboNivel.DataBind();

            cboDescripcion.DataSource = oListaDescripcion;
            cboDescripcion.DataValueField = "cod_desaca_in";
            cboDescripcion.DataTextField = "nom_desaca_vc";
            cboDescripcion.DataBind();

            cboDuracion.DataSource = oListaDuracion;
            cboDuracion.DataValueField = "val_cat_in";
            cboDuracion.DataTextField = "nom_cat_vc";
            cboDuracion.DataBind();

            if (lblDescarga.Text.Trim().Length != 0)
            {
                if (lblDescarga.Text.Trim().Length > 8)
                {
                    cDescarga = lblDescarga.Text.Trim().Substring(0, 8) + "...";
                    lblDescarga.Text = cDescarga;
                }
                lnkDescargar.Visible = true;
            }
        }
    }

    void ListarConocimiento(int cod_idi_in)
    {

        oListaCGeneral = new List<ConocimientoGeneral>();
        oAdministracionSer = new AdministracionService();
        oListaCGeneral = oAdministracionSer.ConocimientoGeneralIdiomaListar(cod_idi_in);
        oListaCEspecifico = oAdministracionSer.ConocimientoEspecificoTodoListar(cod_idi_in);


        listHabilidades.DataSource = oListaCGeneral;
        listHabilidades.DataBind();
        int nCodigoEspecifico = 0;
        for (int i = 0; i < listHabilidades.Items.Count; i++)
        {
            RadListBox lstTecnico = (RadListBox)listHabilidades.Items[i].FindControl("lstTecnico");
            Label lblCodigo = (Label)listHabilidades.Items[i].FindControl("lblCodigo");
            nCodigoEspecifico = int.Parse(lblCodigo.Text.Trim());
            //CheckBoxList chkTecnico = (CheckBoxList)gridHabilidades.Items[i].FindControl("chkTecnico");
            foreach (ConocimientoEspecifico ent in oListaCEspecifico)
            {
                if (ent.oGeneral.cod_congen_in == nCodigoEspecifico)
                {
                    lstTecnico.Items.Add(new RadListBoxItem(ent.nom_conesp_vc, ent.cod_conesp_in.ToString()));

                }
            }
        }
    }

    void ListarIdioma(int cod_idi_in)
    {
        String cSeleccione = "";
        oHelperDiccionario = new Diccionario();
        cSeleccione = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "Seleccione");
        //if (cod_idi_in == 1)
        //{
        //    cSeleccione = "Select";
        //}
        //else
        //{
        //    cSeleccione = "Seleccione";
        //}
        oListaIdioma = new List<Lenguaje>();
        oListaHablado = new List<Catalogo>();
        oAdministracionSer = new AdministracionService();
        oListaIdioma = oAdministracionSer.LenguajeIdiomaListar(cod_idi_in);
        oListaHablado = oAdministracionSer.CatalogoBuscar(2002, cod_idi_in);

        if (oListaIdioma.Count == 0)
        {
            oListaIdioma.Add(new Lenguaje() { nom_len_vc = cSeleccione });
        }
        else
        {
            oListaIdioma.Insert(0, new Lenguaje() { nom_len_vc = cSeleccione });
        }

        if (oListaHablado.Count == 0)
        {

            oListaHablado.Add(new Catalogo() { nom_cat_vc = cSeleccione });
        }
        else
        {
            oListaHablado.Insert(0, new Catalogo() { nom_cat_vc = cSeleccione });
        }

        oListaConLenguaje = new List<ConsultorLenguaje>();


        oListaConLenguaje.Add(new ConsultorLenguaje() { Nro = 1 });

        gridLenguaje.DataSource = oListaConLenguaje;
        gridLenguaje.DataBind();

        for (int i = 0; i < gridLenguaje.Items.Count; i++)
        {
            DropDownList cboLenguaje = (DropDownList)gridLenguaje.Items[i].FindControl("cboLenguaje");
            DropDownList cboHablado = (DropDownList)gridLenguaje.Items[i].FindControl("cboHablado");
            DropDownList cboLeido = (DropDownList)gridLenguaje.Items[i].FindControl("cboLeido");
            DropDownList cboEscrito = (DropDownList)gridLenguaje.Items[i].FindControl("cboEscrito");

            cboLenguaje.DataSource = oListaIdioma;
            cboLenguaje.DataValueField = "cod_len_in";
            cboLenguaje.DataTextField = "nom_len_vc";
            cboLenguaje.DataBind();

            cboHablado.DataSource = oListaHablado;
            cboHablado.DataValueField = "val_cat_in";
            cboHablado.DataTextField = "nom_cat_vc";
            cboHablado.DataBind();

            cboLeido.DataSource = oListaHablado;
            cboLeido.DataValueField = "val_cat_in";
            cboLeido.DataTextField = "nom_cat_vc";
            cboLeido.DataBind();

            cboEscrito.DataSource = oListaHablado;
            cboEscrito.DataValueField = "val_cat_in";
            cboEscrito.DataTextField = "nom_cat_vc";
            cboEscrito.DataBind();

        }
    }

    void ListarDonantes(int cod_idi_in)
    {

        oListaDonante = new List<Donante>();
        oAdministracionSer = new AdministracionService();
        oListaDonante = oAdministracionSer.DonanteIdiomaListar(cod_idi_in);

        lstDonante.DataSource = oListaDonante;
        lstDonante.DataTextField = "nom_don_vc";
        lstDonante.DataValueField = "cod_don_in";
        lstDonante.DataBind();
    }

    void ListarPaises(int cod_idi_in)
    {
        oListaConPais = new List<ConsultorPais>();
        gridPais.DataSource = oListaConPais;
        gridPais.DataBind();
    }

    void ListarCV(int cod_idi_in)
    {
        oListaConCV = new List<ConsultorDocumento>();
        gridCV.DataSource = oListaConCV;
        gridCV.DataBind();
    }

    void ListarPlantilla(int cod_idi_in)
    {
        oListaPlantilla = new List<DonantePlantilla>();
        oAdministracionSer = new AdministracionService();
        oListaPlantilla = oAdministracionSer.DonantePlantillaTodoListar(cod_idi_in);

        gridPlantilla.DataSource = oListaPlantilla;
        gridPlantilla.DataBind();

    }

    protected void lnkAgregarEducacion_Click(object sender, EventArgs e)
    {
        if (Session["Usuario"] != null)
        {
            actualizarCV();
            oTUsuario = (Usuario)Session["Usuario"];
            oListaConEducacion = new List<ConsultorEducacion>();
            oListaConEducacion = DevolverEducacion();

            oListaConEducacion.Add(new ConsultorEducacion()
            {
                Nro = oListaConEducacion.Count + 1,
                oNivel = new NivelAcademico(),
                oDescripcion = new DescripcionAcademica(),
                bExpand = true
            });

            gridEduacion.DataSource = oListaConEducacion;
            gridEduacion.DataBind();

            FormatoEducacion(oListaConEducacion);

        }
    }

    protected void gridEduacion_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Eliminar")
        {
            int nIdioma = 0;

            if (Session["Usuario"] != null)
            {
                actualizarCV();

                int nNro = 0, nOrden = 0;
                nNro = Convert.ToInt32(e.CommandArgument);

                oListaConEducacion = new List<ConsultorEducacion>();
                oListaConEducacion = DevolverEducacion();
                oListaConEducacion.RemoveAt(nNro - 1);

                foreach (ConsultorEducacion oTControl in oListaConEducacion)
                {
                    nOrden++;
                    oTControl.Nro = nOrden;
                }

                FormatoEducacion(oListaConEducacion);
            }
        }

        if (e.CommandName == "ExpandCollapse")
        {
            int nIdioma = 0;

            if (Session["Usuario"] != null)
            {
                actualizarCV();
                UploadedFile oFile;
                String cImagen = "";
                DateTime dfecha;
                // realizando el agregar

                for (int i = 0; i < gridEduacion.Items.Count; i++)
                {
                    Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");
                    Label lblCodigo = (Label)gridEduacion.Items[i].FindControl("lblCodigo");
                    DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
                    //DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");
                    DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
                    RadNumericTextBox txtDuracion = (RadNumericTextBox)gridEduacion.Items[i].FindControl("txtDuracion");
                    TextBox txtDescripcion = (TextBox)gridEduacion.Items[i].FindControl("txtDescripcion");

                    GridNestedViewItem nestedItem = (GridNestedViewItem)gridEduacion.MasterTableView.Items[i].ChildItem;

                    TextBox txtInstitucion = (TextBox)nestedItem.FindControl("txtInstitucion");

                    Label lblDescarga = (Label)nestedItem.FindControl("lblDescarga");
                    Label lblDescargaN = (Label)nestedItem.FindControl("lblDescargaN");
                    RadAsyncUpload RadUpCertificado = (RadAsyncUpload)nestedItem.FindControl("RadUpCertificado");

                    nCodigo = int.Parse(lblNro.Text.ToString().Trim());


                    if (RadUpCertificado.UploadedFiles.Count > 0)
                    {

                        oFile = RadUpCertificado.UploadedFiles[0];
                        dfecha = new DateTime();
                        dfecha = DateTime.Now;

                        cImagen = oHelper.ReplaceCaracter(txtNombre.Text.Trim()) + "_" + oHelper.ReplaceCaracter(txtApellido.Text.Trim()) +
                                  "_CE_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                                  dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                                  dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim() + oFile.GetExtension();

                        try
                        {
                            oFile.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                        }
                        catch
                        {
                            Random random = new Random();
                            int randomNumber = random.Next(0, 2000);
                            cImagen = randomNumber.ToString().Trim() +
                                  "_CE_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                                  dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                                  dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim();
                            oFile.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                        }
                        lblDescarga.Text = cImagen;
                        lblDescargaN.Text = cImagen;

                        HyperLink lnkDescargar = (HyperLink)nestedItem.FindControl("lnkDescargar");
                        lnkDescargar.NavigateUrl = "~/downloading.aspx?file=" + cImagen.Trim();
                        lnkDescargar.ToolTip = cImagen;
                        if (lblDescarga.Text.Trim().Length != 0)
                        {
                            lnkDescargar.Visible = true;
                            if (lblDescarga.Text.Trim().Length > 8)
                            {
                                cImagen = lblDescarga.Text.Trim().Substring(0, 8) + "...";
                                lblDescarga.Text = cImagen;
                            }
                        }
                    }

                }


            }

        }
    }

    protected void ExpandCollapse(object sender, EventArgs e)
    {
        bool expandido = false;
        try
        {
            string opc = (sender as LinkButton).CommandArgument;

            switch (opc)
            {
                case "C": { expandido = true; } break;
                case "E": { expandido = false; } break;
            }

            if (Session["Usuario"] != null)
            {
                actualizarCV();
                UploadedFile oFile;
                String cImagen = "";
                DateTime dfecha;
                // realizando el agregar
                for (int i = 0; i < gridEduacion.MasterTableView.Items.Count; i++)
                {
                    Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");
                    Label lblCodigo = (Label)gridEduacion.Items[i].FindControl("lblCodigo");
                    DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
                    //DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");
                    DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
                    RadNumericTextBox txtDuracion = (RadNumericTextBox)gridEduacion.Items[i].FindControl("txtDuracion");
                    TextBox txtDescripcion = (TextBox)gridEduacion.Items[i].FindControl("txtDescripcion");

                    GridNestedViewItem nestedItem = (GridNestedViewItem)gridEduacion.MasterTableView.Items[i].ChildItem;

                    TextBox txtInstitucion = (TextBox)nestedItem.FindControl("txtInstitucion");

                    Label lblDescarga = (Label)nestedItem.FindControl("lblDescarga");
                    Label lblDescargaN = (Label)nestedItem.FindControl("lblDescargaN");
                    RadAsyncUpload RadUpCertificado = (RadAsyncUpload)nestedItem.FindControl("RadUpCertificado");

                    nCodigo = int.Parse(lblNro.Text.ToString().Trim());


                    if (RadUpCertificado.UploadedFiles.Count > 0)
                    {

                        oFile = RadUpCertificado.UploadedFiles[0];
                        dfecha = new DateTime();
                        dfecha = DateTime.Now;

                        cImagen = oHelper.ReplaceCaracter(txtNombre.Text.Trim()) + "_" + oHelper.ReplaceCaracter(txtApellido.Text.Trim()) +
                                  "_CE_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                                  dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                                  dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim() + oFile.GetExtension();

                        try
                        {
                            oFile.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                        }
                        catch
                        {
                            Random random = new Random();
                            int randomNumber = random.Next(0, 2000);
                            cImagen = randomNumber.ToString().Trim() +
                                  "_CE_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                                  dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                                  dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim();
                            oFile.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                        }
                        lblDescarga.Text = cImagen;
                        lblDescargaN.Text = cImagen;

                        HyperLink lnkDescargar = (HyperLink)nestedItem.FindControl("lnkDescargar");
                        lnkDescargar.NavigateUrl = "~/downloading.aspx?file=" + cImagen.Trim();
                        lnkDescargar.ToolTip = cImagen;
                        if (lblDescarga.Text.Trim().Length != 0)
                        {
                            lnkDescargar.Visible = true;
                            if (lblDescarga.Text.Trim().Length > 8)
                            {
                                cImagen = lblDescarga.Text.Trim().Substring(0, 8) + "...";
                                lblDescarga.Text = cImagen;
                            }
                        }
                    }

                    gridEduacion.MasterTableView.Items[i].Expanded = expandido;
                }
            }

        }
        catch { }
    }

    protected void gridHabilidades_PreRender(object sender, EventArgs e)
    {
        int itemCount = (sender as RadGrid).MasterTableView.GetItems(GridItemType.Item).Length + (sender as RadGrid).MasterTableView.GetItems(GridItemType.AlternatingItem).Length;
        foreach (GridItem item in (sender as RadGrid).Items)
        {
            if (item is GridDataItem && item.ItemIndex < itemCount - 1)
            {
                ((item as GridDataItem)["cod_congen_in"] as TableCell).Controls.Add(new LiteralControl("<table style='display:none;'><tr><td>"));
            }
        }
    }

    protected void lnkAgregarPais_Click(object sender, EventArgs e)
    {
        if (Session["Usuario"] != null)
        {
            actualizarDatos();
            oTUsuario = (Usuario)Session["Usuario"];
            oListaConPais = new List<ConsultorPais>();
            oListaConPais = DevolverPais();
            oListaConPais.Add(new ConsultorPais() { Nro = oListaConPais.Count + 1 });

            FormatoPais(oListaConPais);

        }
    }

    protected void gridPais_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Eliminar")
        {
            actualizarDatos();


            if (Session["Usuario"] != null)
            {
                int nOrden = 0, nNro = 0;
                nNro = Convert.ToInt32(e.CommandArgument);
                oListaConPais = new List<ConsultorPais>();
                oListaConPais = DevolverPais();
                oListaConPais.RemoveAt(nNro - 1);

                foreach (ConsultorPais oTControl in oListaConPais)
                {
                    nOrden++;
                    oTControl.Nro = nOrden;
                }

                FormatoPais(oListaConPais);
            }
        }

    }

    protected void lnkAgregarCV_Click(object sender, EventArgs e)
    {
        if (Session["Usuario"] != null)
        {
            actualizarEducacion();

            oListaConCV = new List<ConsultorDocumento>();
            oListaConCV = DevolverCV();
            oListaConCV.Add(new ConsultorDocumento() { Nro = oListaConCV.Count + 1 });

            FormatoCV(oListaConCV);
        }
    }

    protected void gridCV_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Eliminar")
        {
            if (Session["Usuario"] != null)
            {
                actualizarEducacion();

                int nNro = 0, nOrden = 0;
                nNro = Convert.ToInt32(e.CommandArgument);

                oListaConCV = new List<ConsultorDocumento>();
                oListaConCV = DevolverCV();

                oListaConCV.RemoveAt(nNro - 1);


                foreach (ConsultorDocumento oTControl in oListaConCV)
                {
                    nOrden++;
                    oTControl.Nro = nOrden;
                }

                FormatoCV(oListaConCV);
            }
        }
    }

    protected void lnkAgregarIdioma_Click(object sender, EventArgs e)
    {
        if (Session["Usuario"] != null)
        {
            actualizarDatos();
            oTUsuario = (Usuario)Session["Usuario"];
            oListaConLenguaje = new List<ConsultorLenguaje>();

            oListaConLenguaje = DevolverLenguaje();
            oListaConLenguaje.Add(new ConsultorLenguaje() { Nro = oListaConLenguaje.Count + 1 });

            FormatoLenguaje(oListaConLenguaje);
        }
    }

    protected void gridLenguaje_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Eliminar")
        {
            actualizarDatos();
            if (Session["Usuario"] != null)
            {

                int nNro = 0, nOrden = 0;

                nNro = Convert.ToInt32(e.CommandArgument);

                oListaConLenguaje = new List<ConsultorLenguaje>();
                oListaConLenguaje = DevolverLenguaje();

                oListaConLenguaje.RemoveAt(nNro - 1);


                foreach (ConsultorLenguaje oTControl in oListaConLenguaje)
                {
                    nOrden++;
                    oTControl.Nro = nOrden;
                }

                FormatoLenguaje(oListaConLenguaje);

            }
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        int nTipo = 2, nTipoUsuario = 0, nTipoConsultor = 0, nPais = 0, nEstado = 0, nExperiencia = 0, nCodigoUsuario = 0;
        String cNombre = "", cApellido = "", cCorreo = "", cUsuario = "",
        cContrasenia = "", cMensajeOk = "";
        bool bExperiencia = false;
        oHelperDiccionario = new Diccionario();
        nTipoConsultor = int.Parse(lblCTipo.Text.Trim());
        nTipoUsuario = int.Parse(lblCTipo.Text.Trim());
        nEstado = int.Parse(lblEstado.Text.Trim());
        DateTime dfecha;
        UploadedFile oFile;
        Random random;
        UploadedFile oImagen;
        int randomNumber = 0;
        String cImagen = "";
        double nPago = 0;

        // validaciones
        string msg = "";
        pnlError.Visible = false;
        pnlOk.Visible = false;
        cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
        cRutaContenido = Server.MapPath(cCarpetaContenido);

        this.actualizaFoto();
        this.actualizarDNI();
        this.actualizarSuspension4ta();

        if (txtApellido.Text.Trim().Length == 0)
        {
            msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                             oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorApellido"));
        }

        if (txtNombre.Text.Trim().Length == 0)
        {
            msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                            oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorNombre"));
        }

        DateTime? dFechaNacimiento = new DateTime?();

        //dFechaNacimiento = DateTime.Today;

        string[] cFecha;
        cFecha = txtFechaNacimiento.TextWithLiterals.Trim().Split('/');
        //Response.Write(cFecha.Length);

        if (txtFechaNacimiento.Text.Trim().Length != 0)
        {
            if (cFecha.Length == 3)
            {
                try
                {
                    int nDia = 0, nMes = 0, nAnio = 0;

                    nDia = int.Parse(cFecha[0].Trim());
                    nMes = int.Parse(cFecha[1].Trim());
                    nAnio = int.Parse(cFecha[2].Trim());

                    dFechaNacimiento = new DateTime(nAnio, nMes, nDia);
                }
                catch (Exception ex)
                {
                    msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorNacimiento");
                }
            }
        }

        if (cboSexo.SelectedValue.Trim().Equals("0"))
        {
            msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorSeleccionar",
                             oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorSexo"));
        }

        if (cboNacionalidad1.SelectedValue.Trim().Equals("0"))
        {
            msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorSeleccionar",
                           oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorNacionalidad") + " 1");
            //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
            //       Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
            //       Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            //{
            //    cError += "<li>Debe seleccionar una opción para \"Nacionalidad 1\".</li>";
            //}
            //else
            //{
            //    cError += "<li>You must select one option for \"Nationality 1\".</li>";
            //}
        }
        else
        {
            if (cboNacionalidad1.SelectedValue.Trim().Equals(cboNacionalidad2.SelectedValue.Trim()))
            {
                msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorNacionalidadRepetida");
                //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                //     Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                //     Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                //{
                //    cError += "<li>Ha seleccionado nacionalidades repetidas.</li>";
                //}
                //else
                //{
                //    cError += "<li>You have selected nationalities repeated.</li>";
                //}
            }
        }

        if (cboPais.SelectedValue.Trim().Equals("0"))
        {
            msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                          oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorPais"));
            //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
            //       Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
            //       Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            //{
            //    cError += "<li>El campo \"País\" es obligatorio.</li>";
            //}
            //else
            //{
            //    cError += "<li>The field \"Country\" is required.</li>";
            //}
        }

        if (txtCorreo.Text.Trim().Length == 0)
        {
            msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                         oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorCorreo"));
            //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
            //    Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
            //    Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            //{
            //    cError += "<li>El campo \"Correo\" es obligatorio.</li>";
            //}
            //else
            //{
            //    cError += "<li>The field \"E-mail\" is required.</li>";
            //}
        }
        else
        {

            if (oHelper.ValidarCorreo(txtCorreo.Text.Trim()) == false)
            {
                msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorFormatoCorreo");
                //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                //    Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                //    Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                //{
                //    cError += "<li>El correo tiene un formato incorrecto. Ejm: user@mail.com.</li>";
                //}
                //else
                //{
                //    cError += "<li>The e-mail has an incorrect format. Example: user@mail.com.</li>";
                //}
            }
        }

        if (nTipoUsuario == 2)
        {
            if (nEstado != 3)
            {
                if (chkContra.Checked == true)
                {
                    if (txtContra.Text.Trim().Length == 0)
                    {
                        msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                                      oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorContrasenia"));
                        //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                        // Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                        // Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                        //{
                        //    cError += "<li>El campo \"Contraseña\" es obligatorio.</li>";
                        //}
                        //else
                        //{
                        //    cError += "<li>The field \"Password\" is required.</li>";
                        //}
                    }

                    if (txtContraRep.Text.Trim().Length == 0)
                    {
                        msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                                        oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorReContrasenia"));
                        //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                        //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                        //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                        //{
                        //    cError += "<li>El campo \"Re-Contraseña\" es obligatorio.</li>";
                        //}
                        //else
                        //{
                        //    cError += "<li>The field \"Re-Password\" is required.</li>";
                        //}

                    }

                    if (!txtContra.Text.Trim().Equals(txtContraRep.Text.Trim()))
                    {
                        msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorContraseniaRepetido");
                        //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                        //  Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                        //  Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                        //{
                        //    cError += "<li>Las contraseñas ingresadas no coinciden.</li>";
                        //}
                        //else
                        //{
                        //    cError += "<li>The entered passwords do not match.</li>";
                        //}
                    }
                }

            }
            else if (nEstado == 3)
            {

                if (txtContra.Text.Trim().Length == 0)
                {
                    msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                                       oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorContrasenia"));
                    //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    //  Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    //  Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    //{
                    //    cError += "<li>El campo \"Contraseña\" es obligatorio.</li>";
                    //}
                    //else
                    //{
                    //    cError += "<li>The field \"Password\" is required.</li>";
                    //}


                    if (txtContraRep.Text.Trim().Length == 0)
                    {
                        msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                                    oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorReContrasenia"));

                        //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                        //     Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                        //     Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                        //{
                        //    cError += "<li>El campo \"Re-Contraseña\" es obligatorio.</li>";
                        //}
                        //else
                        //{
                        //    cError += "<li>The field \"Re-Password\" is required.</li>";
                        //}
                    }

                    if (!txtContra.Text.Trim().Equals(txtContraRep.Text.Trim()))
                    {
                        msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorContraseniaRepetido");
                        //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                        //  Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                        //  Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                        //{
                        //    cError += "<li>Las contraseñas ingresadas no coinciden.</li>";
                        //}
                        //else
                        //{
                        //    cError += "<li>The entered passwords do not match.</li>";
                        //}
                    }

                }
            }
        }

        if (cboMoneda.SelectedValue.Trim().Equals("0"))
        {
            if (txtMoneda.Text.Trim().Length != 0)
            {
                msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "Seleccionar",
                                 oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorMoneda"));
                //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                //         Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                //         Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                //{
                //    cError += "<li>Debe seleccionar \"Moneda de tarifa diaria\".</li>";
                //}
                //else
                //{
                //    cError += "<li>You must select one \"Fee Rate Currency\".</li>";
                //}
            }
        }
        else
        {
            double valor = 0;

            try
            {
                valor = double.Parse(txtMoneda.Text.Trim());
            }
            catch { }

            if (valor == 0)
            {
                try
                {
                    nPago = double.Parse(txtMoneda.Text.Trim());
                }
                catch
                {
                    nPago = 0;
                }

                if (nPago == 0)
                {
                    msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorMonedaCero");

                    //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    //         Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    //         Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    //{
                    //    cError += "<li>El campo \"Tarifa Diaria\" debe ser mayor a cero.</li>";
                    //}
                    //else
                    //{
                    //    cError += "<li>The field \"Daily Fee Rate\" must be more than zero.</li>";
                    //}
                }
            }
            else
            {
                try
                {
                    nPago = double.Parse(txtMoneda.Text.Trim());
                }
                catch
                {
                    nPago = 0;
                }
            }
        }


        if (rbSI.Checked == true)
        {
            bExperiencia = true;
        }

        if (bExperiencia == false)
        {

            if (rbNo.Checked == false)
            {
                msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorExperiencia");
                //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                //             Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                //             Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                //{
                //    cError += "<li>No ha registrado su respuesta para \"Experiencia en países en desarrollo\".</li>";
                //}
                //else
                //{
                //    cError += "<li>It has not registered your answer for \"Experience in developing countries\".</li>";
                //}
            }
        }


        if (rbExperiencia1.Checked == true)
        {
            nExperiencia = 1;
        }

        if (rbExperiencia2.Checked == true)
        {
            nExperiencia = 2;
        }

        if (rbExperiencia3.Checked == true)
        {
            nExperiencia = 3;
        }

        if (rbExperiencia4.Checked == true)
        {
            nExperiencia = 4;
        }

        if (nExperiencia == 0)
        {
            msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                              oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorAnioExperiencia"));
            //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
            //                     Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
            //                     Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            //{
            //    cError += "<li>El campo \"Años de experiencia\" es obligatorio.</li>";
            //}
            //else
            //{
            //    cError += "<li>The field \"Total years of experience\" are required.</li>";
            //}
        }


        String cErrorNivel = "", cErrorDescripcion = "", cErroNDuracion = "", cErrorDuracion = "", cErrorInstitucion = "";
        if (gridEduacion.Items.Count != 0)
        {

            for (int i = 0; i < gridEduacion.Items.Count; i++)
            {
                DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
                DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
                // DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");
                RadNumericTextBox txtDuracion = (RadNumericTextBox)gridEduacion.Items[i].FindControl("txtDuracion");
                TextBox txtDescripcion = (TextBox)gridEduacion.Items[i].FindControl("txtDescripcion");
                Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");

                GridNestedViewItem nestedItem = (GridNestedViewItem)gridEduacion.MasterTableView.Items[i].ChildItem;
                RadAsyncUpload RadUpCertificado = (RadAsyncUpload)nestedItem.FindControl("RadUpCertificado");

                Label lblDescarga = (Label)nestedItem.FindControl("lblDescarga");
                Label lblDescargaN = (Label)nestedItem.FindControl("lblDescargaN");
                TextBox txtInstitucion = (TextBox)nestedItem.FindControl("txtInstitucion");


                if (cboNivel.SelectedValue.Trim().Equals("0"))
                {
                    cErrorNivel = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                                   oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorNivel"));
                    //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    //  Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    //  Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    //{
                    //    cErrorNivel = "<li>El campo \"Nivel académico\" es obligatorio.</li>";
                    //}
                    //else
                    //{
                    //    cErrorNivel = "<li>The field \"Academic Level\" es required.</li>";
                    //}

                }

                if (txtDescripcion.Text.Trim().Length == 0)
                {
                    cErrorDescripcion = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                                  oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorDescripcion"));
                    //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    // Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    // Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    //{
                    //    cErrorDescripcion = "<li>El campo \"Descripción académica\" es obligatorio.</li>";
                    //}
                    //else
                    //{
                    //    cErrorDescripcion = "<li>The field \"Academic Description\" es required.</li>";
                    //}                     
                }

                if (txtDuracion.Value != null)
                {
                    if (txtDuracion.Value == 0)
                    {
                        cErroNDuracion = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorDuracion");
                        //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                        //    Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                        //    Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                        //{
                        //    cErroNDuracion = "<li>El campo \"Duración\" debe ser mayor a cero.</li>";
                        //}
                        //else
                        //{
                        //    cErroNDuracion = "<li>The field \"Duration\" must be more than zero.</li>";
                        //}
                    }
                }

                if (cboDuracion.SelectedValue.Trim().Equals("0"))
                {
                    cErrorDuracion = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                               oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorTipoDuracion"));
                    //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    //    Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    //    Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    //{
                    //    cErrorDuracion = "<li>El campo \"Tipo de duración\" es obligatorio.</li>";
                    //}
                    //else
                    //{
                    //    cErrorDuracion = "<li>The field \"Duration type \" is required.</li>";
                    //}
                }

                if (txtInstitucion.Text.Trim().Length == 0)
                {
                    cErrorInstitucion = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                               oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorInstitucion"));
                    //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    //     Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    //     Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    //{
                    //    cErrorInstitucion = "<li>El campo \"Institución\" es obligatorio.</li>";
                    //}
                    //else
                    //{
                    //    cErrorInstitucion = "<li>The field \"Institution\" is required.</li>";
                    //}
                }

                if (RadUpCertificado.UploadedFiles.Count > 0)
                {


                    oFile = RadUpCertificado.UploadedFiles[0];
                    dfecha = new DateTime();
                    dfecha = DateTime.Now;

                    dfecha = new DateTime();
                    dfecha = DateTime.Now;

                    cImagen = oHelper.ReplaceCaracter(txtNombre.Text.Trim()) + "_" + oHelper.ReplaceCaracter(txtApellido.Text.Trim()) +
                                  "_CE_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                              dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                              dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim() + oFile.GetExtension();

                    try
                    {

                        oFile.SaveAs(String.Format("{0}/{1}", cRutaContenido, cImagen.Trim()));
                    }
                    catch
                    {
                        random = new Random();
                        randomNumber = random.Next(0, 2000);

                        cImagen = randomNumber +
                                  "_CE_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                              dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                              dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim();
                        oFile.SaveAs(String.Format("{0}/{1}", cRutaContenido, cImagen.Trim()));
                    }

                    lblDescargaN.Text = cImagen.Trim();
                    lblDescarga.Text = cImagen.Trim();

                }
            }

            if (cErrorNivel.Trim().Length != 0)
            {
                msg += cErrorNivel;
            }

            if (cErrorDescripcion.Trim().Length != 0)
            {
                msg += cErrorDescripcion;
            }

            if (cErroNDuracion.Trim().Length != 0)
            {
                msg += cErroNDuracion;
            }

            if (cErrorDuracion.Trim().Length != 0)
            {
                msg += cErrorDuracion;
            }

            if (cErrorInstitucion.Trim().Length != 0)
            {
                msg += cErrorInstitucion;
            }
        }


        // paises

        bool bValidaPais = false;
        if (gridPais.Items.Count != 0)
        {
            for (int i = 0; i < gridPais.Items.Count; i++)
            {
                DropDownList cboTPais = (DropDownList)gridPais.Items[i].FindControl("cboPais");

                if (!cboTPais.SelectedValue.Trim().Equals("0"))
                {

                    bValidaPais = true; break;
                }
            }

            if (bValidaPais == false)
            {
                msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorExperienciaPrevia");
                //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                //        Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                //        Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                //{
                //    cError += "<li>Debe ingresar al menos una opción para el pais en \"Experiencia previa\".</li>";
                //}
                //else
                //{
                //    cError += "<li>You must enter at least one option for the country in \"Previous experience\".</li>";
                //}
            }
        }


        //cvs
        int contadorArchivo = 0, contadorLonguitudCero = 0;
        String cTextoOriginal = "", cTexto = "";
        if (gridCV.Items.Count != 0)
        {
            for (int i = 0; i < gridCV.Items.Count; i++)
            {
                Label lblNro = (Label)gridCV.Items[i].FindControl("lblNro");

                TextBox txtTitulo = (TextBox)gridCV.Items[i].FindControl("txtTitulo");
                Label lblDescarga = (Label)gridCV.Items[i].FindControl("lblDescarga");
                Label lblDescargaN = (Label)gridCV.Items[i].FindControl("lblDescargaN");
                Label lblOriginal = (Label)gridCV.Items[i].FindControl("lblOriginal");
                RadAsyncUpload RadUpCV = (RadAsyncUpload)gridCV.Items[i].FindControl("RadUpCV");

                if (RadUpCV.UploadedFiles.Count > 0)
                {
                    oFile = RadUpCV.UploadedFiles[0];

                    lblOriginal.Text = oFile.GetNameWithoutExtension().Trim();

                    if (txtTitulo.Text.Trim().Length != 0)
                    {
                        cTexto = txtTitulo.Text.Trim();
                    }
                    else
                    {
                        cTexto = oFile.GetNameWithoutExtension().Trim();
                    }

                    dfecha = new DateTime();
                    dfecha = DateTime.Now;

                    cImagen = oHelper.ReplaceCaracter(txtNombre.Text.Trim()) + "_" + oHelper.ReplaceCaracter(txtApellido.Text.Trim()) +
                                  "_CV_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                              dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                              dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim() + oFile.GetExtension();

                    try
                    {
                        oFile.SaveAs(String.Format("{0}/{1}", cRutaContenido, cImagen.Trim()));
                    }
                    catch
                    {
                        random = new Random();
                        randomNumber = random.Next(0, 2000);
                        cImagen = randomNumber.ToString().Trim() +
                                  "_CV_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                              dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                              dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim();
                        oFile.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                    }

                    lblDescarga.Text = cImagen.Trim();
                    lblDescargaN.Text = cImagen.Trim();
                }
                else
                {
                    if (txtTitulo.Text.Trim().Length != 0)
                    {
                        cTexto = txtTitulo.Text.Trim();
                    }
                    else if (txtTitulo.Text.Trim().Length == 0)
                    {

                        if (lblOriginal.Text.Trim().Length != 0)
                        {
                            cTexto = lblOriginal.Text.Trim();
                        }
                        else
                        {
                            cTexto = lblDescargaN.Text.Trim();
                        }
                    }
                }

                txtTitulo.Text = cTexto;

                if (lblDescarga.Text.Trim().Length == 0)
                {
                    contadorArchivo++;
                }

                if (txtTitulo.Text.Trim().Length == 0)
                {
                    contadorLonguitudCero++;
                }
            }

            if (contadorLonguitudCero > 0)
            {
                msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorCVTitulo");
                //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                //Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                //Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                //{
                //    cError += "<li>Debe agregar un \"Título\" al CV.</li>";
                //}
                //else
                //{
                //    cError += "<li>You must add a \"Title\" for the CV.</li>";
                //}
            }

            if (contadorArchivo > 0)
            {
                msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorCVArchivo");
                //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                //Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                //Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                //{
                //    cError += "<li>Debe de agregar al menos un archivo en \"CV\".</li>";
                //}
                //else
                //{
                //    cError += "<li>You must add at least one file in \"CV\".</li>";
                //}
            }

        }

        if (gridLenguaje.Items.Count != 0)
        {
            String cErrorLenguaje = "", cErrorHablado = "", cErrorLeido = "", cErrorEscrito = "";
            for (int i = 0; i < gridLenguaje.Items.Count; i++)
            {
                DropDownList cboLenguaje = (DropDownList)gridLenguaje.Items[i].FindControl("cboLenguaje");
                DropDownList cboHablado = (DropDownList)gridLenguaje.Items[i].FindControl("cboHablado");
                DropDownList cboLeido = (DropDownList)gridLenguaje.Items[i].FindControl("cboLeido");
                DropDownList cboEscrito = (DropDownList)gridLenguaje.Items[i].FindControl("cboEscrito");

                if (cboLenguaje.SelectedValue.Trim().Equals("0"))
                {
                    cErrorLenguaje = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                               oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorLenguaje"));
                    //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    //  Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    //  Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    //{
                    //    cErrorLenguaje = "<li>El campo \"Lenguaje\" es obligatorio.</li>";
                    //}
                    //else
                    //{
                    //    cErrorLenguaje = "<li>The field \"Language\" is required.</li>";
                    //}

                }

                if (cboHablado.SelectedValue.Trim().Equals("0"))
                {
                    cErrorHablado = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorHablado");
                    //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    //{
                    //    cErrorHablado = "<li>El campo \"Hablado\" dentro de Lenguaje es obligatorio.</li>";
                    //}
                    //else
                    //{
                    //    cErrorHablado = "<li>The field \"Speak\" in Language is required.</li>";
                    //}
                }
                if (cboLeido.SelectedValue.Trim().Equals("0"))
                {
                    cErrorLeido = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorLeido");
                    //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    // Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    // Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    //{
                    //    cErrorLeido = "<li>El campo \"Leído\" dentro de Lenguaje es obligatorio.</li>";
                    //}
                    //else
                    //{
                    //    cErrorLeido = "<li>The field \"Read\" in Language is required.</li>";
                    //}

                }
                if (cboEscrito.SelectedValue.Trim().Equals("0"))
                {
                    cErrorEscrito = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorEscrito");
                    //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    //  Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    //  Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    //{
                    //    cErrorEscrito = "<li>El campo \"Escrito\" dentro de Lenguaje es obligatorio.</li>";
                    //}
                    //else
                    //{
                    //    cErrorEscrito = "<li>The field \"Write\" in Language is required.</li>";
                    //}

                }

            }

            if (cErrorLenguaje.Trim().Length != 0)
            {
                msg += cErrorLenguaje;
            }

            if (cErrorHablado.Trim().Length != 0)
            {
                msg += cErrorHablado;
            }

            if (cErrorLeido.Trim().Length != 0)
            {
                msg += cErrorLeido;
            }

            if (cErrorEscrito.Trim().Length != 0)
            {
                msg += cErrorEscrito;
            }
        }

        // Validación de campos de Retención SP y otras validaciones (Tipo / N° de documento, DNI, dirección, fecha de nacimiento)
        if (oTUsuario.oPais.cod_pais_in.Equals(1))// 1 define el país de PERÚ
        {
            if (this.txtFechaNacimiento.Text == null) msg += "El ingreso de la fecha de nacimiento es obligatorio.";
            if (String.IsNullOrEmpty(this.txtDireccion.Text.Trim())) msg += "El ingreso de la Dirección es obligatoria.";

            if (this.cbo_tipo_doc.SelectedIndex == 0) msg += "La selección del Tipo de Documento es obligatoria.";
            if (String.IsNullOrEmpty(this.txt_nro_doc.Text.Trim())) msg += "El ingreso del N° de Documento es obligatorio.";

            switch (this.cbo_tipo_doc.SelectedItem.Value)
            {
                case "1": if (this.txt_nro_doc.Text.Trim().Length != 8) msg += "La longitud del N° de DNI debe ser de 8 dígitos."; break; // DNI
                case "6": if (this.Validar_RUC(this.txt_nro_doc.Text.Trim())) msg += "El N° de RUC ingresado no es válido."; break; // RUC
            }

            string fileDNI = ViewState["vws_fileDNI"] != null ? ViewState["vws_fileDNI"].ToString() : string.Empty;
            string fileSuspension4ta = ViewState["vws_fileSuspension4ta"] != null ? ViewState["vws_fileSuspension4ta"].ToString() : string.Empty;

            //if (this.cbo_tipo_doc.SelectedItem.Value.Equals("1")) msg += String.IsNullOrEmpty(fileDNI) ? "La adjunción del DNI es obligatoria." : string.Empty;

            if (this.cbo_exonera_retencion.SelectedIndex == 0) msg += "La selección de Exoneración de 4ta Categoría es obligatoria.";
            if (this.cbo_exonera_retencion.SelectedItem.Value.Equals("1")) msg += String.IsNullOrEmpty(fileSuspension4ta) ? "La adjunción de la Suspensión de 4ta Categoría es obligatoria." : string.Empty;

            #region Validación de cuentas

            List<IT.Domain.ConsultorCuenta> cuentas = new List<IT.Domain.ConsultorCuenta>();
            List<string> lstCuentas = new List<string>();

            for (int i = 0; i < this.gvwCuentas.Items.Count; i++)
            {
                Label lblNro = (Label)this.gvwCuentas.Items[i].FindControl("lblNro");
                DropDownList cbo_banco = (DropDownList)this.gvwCuentas.Items[i].FindControl("cbo_banco");
                DropDownList cbo_moneda = (DropDownList)this.gvwCuentas.Items[i].FindControl("cbo_moneda");
                RadComboBox cbo_tipo_cuenta = (RadComboBox)this.gvwCuentas.Items[i].FindControl("cbo_tipo_cuenta");
                TextBox txt_nro_cuenta = (TextBox)this.gvwCuentas.Items[i].FindControl("txt_nro_cuenta");

                if (cbo_banco.SelectedIndex == 0) { msg += "La selección del Banco es obligatoria en la fila " + (i + 1).ToString(); }
                if (cbo_moneda.SelectedIndex == 0) { msg += "La selección de la Moneda es obligatoria en la fila " + (i + 1).ToString(); }
                if (String.IsNullOrEmpty(cbo_tipo_cuenta.Text.Trim())) { msg += "El ingreso del Tipo de Cuenta es obligatorio en la fila " + (i + 1).ToString(); }
                if (String.IsNullOrEmpty(txt_nro_cuenta.Text.Trim())) { msg += "El ingreso del N° de Cuenta es obligatorio en la fila " + (i + 1).ToString(); }

                lstCuentas.Add(txt_nro_cuenta.Text.Trim());
            }

            if (lstCuentas.Distinct().Count() != lstCuentas.Count) msg += "Los N° de Cuenta del Consultor no deben repetirse.";

            #endregion
        }

        // Validación de campos de Retención SP y otras validaciones (Tipo / N° de documento, DNI, dirección, fecha de nacimiento)
        if (oTUsuario.oPais.cod_pais_in.Equals(22))// 22 define el país de BOLIVIA
        {
            if (this.txtFechaNacimiento.Text == null) msg += "El ingreso de la fecha de nacimiento es obligatorio.";
            if (String.IsNullOrEmpty(this.txtDireccion.Text.Trim())) msg += "El ingreso de la Dirección es obligatoria.";

            if (this.cbo_tipo_doc.SelectedIndex == 0) msg += "La selección del Tipo de Documento es obligatoria.";
            if (String.IsNullOrEmpty(this.txt_nro_doc.Text.Trim())) msg += "El ingreso del N° de Documento es obligatorio.";

            //switch (this.cbo_tipo_doc.SelectedItem.Value)
            //{
            //    case "9": if (this.txt_nro_doc.Text.Trim().Length != 6 && this.txt_nro_doc.Text.Trim().Length != 7) msg += "La longitud del N° de documento debe ser de 6 ó 7 dígitos."; break; // DNI
            //}

            string fileDNI = ViewState["vws_fileDNI"] != null ? ViewState["vws_fileDNI"].ToString() : string.Empty;
            string fileSuspension4ta = ViewState["vws_fileSuspension4ta"] != null ? ViewState["vws_fileSuspension4ta"].ToString() : string.Empty;

            //if (this.cbo_tipo_doc.SelectedItem.Value.Equals("1")) msg += String.IsNullOrEmpty(fileDNI) ? "La adjunción del DNI es obligatoria." : string.Empty;

            if (this.cbo_exonera_retencion.SelectedIndex == 0) msg += "La selección de si el consultor tiene factura o no es obligatoria.";
            //if (this.cbo_exonera_retencion.SelectedItem.Value.Equals("1")) msg += String.IsNullOrEmpty(fileSuspension4ta) ? "La adjunción de la Suspensión de 4ta Categoría es obligatoria." : string.Empty;

            #region Validación de cuentas

            List<IT.Domain.ConsultorCuenta> cuentas = new List<IT.Domain.ConsultorCuenta>();
            List<string> lstCuentas = new List<string>();

            for (int i = 0; i < this.gvwCuentas.Items.Count; i++)
            {
                Label lblNro = (Label)this.gvwCuentas.Items[i].FindControl("lblNro");
                DropDownList cbo_banco = (DropDownList)this.gvwCuentas.Items[i].FindControl("cbo_banco");
                DropDownList cbo_moneda = (DropDownList)this.gvwCuentas.Items[i].FindControl("cbo_moneda");
                RadComboBox cbo_tipo_cuenta = (RadComboBox)this.gvwCuentas.Items[i].FindControl("cbo_tipo_cuenta");
                TextBox txt_nro_cuenta = (TextBox)this.gvwCuentas.Items[i].FindControl("txt_nro_cuenta");

                if (cbo_banco.SelectedIndex == 0) { msg += "La selección del Banco es obligatoria en la fila " + (i + 1).ToString(); }
                if (cbo_moneda.SelectedIndex == 0) { msg += "La selección de la Moneda es obligatoria en la fila " + (i + 1).ToString(); }
                if (String.IsNullOrEmpty(cbo_tipo_cuenta.Text.Trim())) { msg += "El ingreso del Tipo de Cuenta es obligatorio en la fila " + (i + 1).ToString(); }
                if (String.IsNullOrEmpty(txt_nro_cuenta.Text.Trim())) { msg += "El ingreso del N° de Cuenta es obligatorio en la fila " + (i + 1).ToString(); }

                lstCuentas.Add(txt_nro_cuenta.Text.Trim());
            }

            if (lstCuentas.Distinct().Count() != lstCuentas.Count) msg += "Los N° de Cuenta del Consultor no deben repetirse.";

            #endregion
        }

        // Skills & Education

        if (!oTUsuario.Administrador)
        {
            int cantidadPorRubro = 0;
            int cantidadGeneral = 0;

            for (int i = 0; i < this.listHabilidades.Items.Count; i++)
            {
                RadListBox lstTecnico = (RadListBox)listHabilidades.Items[i].FindControl("lstTecnico");
                IList<RadListBoxItem> oColTecnico = lstTecnico.CheckedItems;

                cantidadPorRubro = oColTecnico.Count;

                if (cantidadPorRubro > Constantes.CANTIDADLIMITEPORRUBRO)
                    msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "CantidadLimitePorRubro");

                cantidadGeneral += cantidadPorRubro;
            }

            if (cantidadGeneral > Constantes.CANTIDADLIMITEGENERAL)
                msg += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "CantidadLimiteGeneral");
        }

        if (msg.Trim().Length != 0)
        {
            pnlError.Visible = true;
            litError.Text = oHelperDiccionario.DevuelveFormatoWeb(msg.Trim());
            txtContra.Attributes["value"] = txtContra.Text;

            txtContraRep.Attributes["value"] = txtContraRep.Text;
            return;
        }

        nCodigo = int.Parse(lblCodigo.Text.Trim());

        int nValida = 0;
        oConsultorSer = new ConsultorService();

        nValida = oConsultorSer.ConsultorCorreoValida(nCodigo, txtCorreo.Text.Trim());

        if (nValida != 0)
        {
            cMensajeOk += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "UsuarioExisteCorreo");
            //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
            //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
            //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            //{
            //    cMensajeOk += "Ya existe un consultor o usuario registrado con este correo.";
            //}
            //else
            //{
            //    cMensajeOk += "Consultant or User is already registered with this e-mail.";
            //}

            pnlError.Visible = true;
            txtContra.Attributes["value"] = txtContra.Text;

            txtContraRep.Attributes["value"] = txtContraRep.Text;
            litError.Text = oHelperDiccionario.DevuelveFormatoWeb(cMensajeOk.Trim());
            return;
        }


        //UploadedFile oImagen;
        //String cImagen = "";
        cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
        cRutaContenido = Server.MapPath(cCarpetaContenido);



        nTipoConsultor = int.Parse(lblCTipo.Text.Trim());
        nTipoUsuario = int.Parse(lblCTipo.Text.Trim());

        cNombre = txtNombre.Text.Trim();
        cApellido = txtApellido.Text.Trim();
        cCorreo = txtCorreo.Text.Trim();
        nPais = int.Parse(cboPais.SelectedValue.Trim());
        nEstado = 1;



        // usuario
        if (nTipoUsuario == 1)
        {
            cUsuario = lblConsultorDA.Text.Trim();
            cContrasenia = lblConsultorDA.Text.Trim();
        }
        else if (nTipoUsuario == 2)
        {
            cUsuario = txtCorreo.Text.Trim();
            cContrasenia = txtContra.Text.Trim();
        }

        //insertar
        oPersona = new Persona();
        oPersona.cod_per_in = nCodigo;
        oPersona.tip_per_in = nTipo;
        oPersona.nom_per_vc = cNombre;
        oPersona.ape_per_vc = cApellido;
        oPersona.email_per_vc = cCorreo;
        oPersona.est_per_in = 1;

        oPersona.oIdioma = new Idioma()
        {
            cod_idi_in = int.Parse(cboIdioma.SelectedValue.ToString().Trim())
        };

        oPersona.oPais = new Pais
        {
            cod_pais_in = nPais
        };

        oSeguridadSer = new SeguridadService();
        oAdministracionSer = new AdministracionService();

        if (nCodigo != 0)
        {

            oSeguridadSer.PersonaEditar(oPersona);

            if (rbExperiencia1.Checked == true)
            {
                nExperiencia = 1;
            }

            if (rbExperiencia2.Checked == true)
            {
                nExperiencia = 2;
            }

            if (rbExperiencia3.Checked == true)
            {
                nExperiencia = 3;
            }

            if (rbExperiencia4.Checked == true)
            {
                nExperiencia = 4;
            }
            cImagen = "";

            cImagen = lblFotoDescarga.Text.Trim();

            oConsultor = new Consultor();
            oConsultor.cod_per_in = nCodigo;
            oConsultor.tip_con_in = nTipoConsultor;
            //oConsultor.fec_nac_con_dt = radDtFecha.SelectedDate.Value;
            oConsultor.fec_nac_con_dt = dFechaNacimiento;
            oConsultor.sex_con_in = int.Parse(cboSexo.SelectedValue);

            oConsultor.tel1_con_vc = txtTelefono1.Text;
            oConsultor.tel2_con_vc = txtTelefono2.Text;

            oConsultor.link_con_vc = txtLinked.Text;
            oConsultor.tot_exp_con_in = nExperiencia;
            oConsultor.cod_con_vc = "";

            oConsultor.dir_con_vc = txtDireccion.Text;
            //oConsultor.cer_exp_con_bo = chkCertifico.Checked;
            oConsultor.cer_exp_con_bo = bExperiencia;
            //oConsultor.oCiudad = new Ciudad() { cod_ciu_in = int.Parse(cboCiudad.SelectedValue.Trim()) };
            oConsultor.ciu_con_vc = txtCiudad.Text.Trim();
            oConsultor.img_con_vc = cImagen;
            oConsultor.tip_pag_con_in = int.Parse(cboMoneda.SelectedValue.Trim());
            oConsultor.pag_con_dc = nPago;
            oConsultor.bio_con_vc = txtBiografia.Text.Trim();

            int TipoDocumento = 0;
            string NroDocumento = string.Empty;
            bool ExoneracionRetencion = false;
            string Suspension4ta = string.Empty;
            string ConstanciaAfiliacionSP = string.Empty;
            int TipoSP = 0;
            int TipoSPP = 0;
            int TipoComision = 0;
            string CUSPP = string.Empty;
            string DNI = string.Empty;

            if (oTUsuario.oPais.cod_pais_in.Equals(1) || oTUsuario.oPais.cod_pais_in.Equals(22)) // 1 define el país de PERÚ y 22 define el país de BOLIVIA
            {
                string fileDNI = ViewState["vws_fileDNI"] != null ? ViewState["vws_fileDNI"].ToString() : string.Empty;
                string fileSuspension4ta = ViewState["vws_fileSuspension4ta"] != null ? ViewState["vws_fileSuspension4ta"].ToString() : string.Empty;

                TipoDocumento = Convert.ToInt32(this.cbo_tipo_doc.SelectedItem.Value);
                NroDocumento = this.txt_nro_doc.Text.Trim();
                ExoneracionRetencion = this.cbo_exonera_retencion.SelectedItem.Value == "1" ? true : false;
                Suspension4ta = this.cbo_exonera_retencion.SelectedItem.Value == "1" ? fileSuspension4ta : string.Empty;
                DNI = fileDNI;
            }

            oConsultor.TipoDocumento = TipoDocumento;
            oConsultor.NroDocumento = NroDocumento;
            oConsultor.ExoneracionRetencion = ExoneracionRetencion;
            oConsultor.Suspension4ta = Suspension4ta;
            oConsultor.ConstanciaAfiliacionSP = ConstanciaAfiliacionSP;
            oConsultor.TipoSP = TipoSP;
            oConsultor.TipoSPP = TipoSPP;
            oConsultor.TipoComision = TipoComision;
            oConsultor.CUSPP = CUSPP;
            oConsultor.DNI = DNI;
            oConsultor.Relationship = Convert.ToInt32(this.ddlRelationShip.SelectedValue.ToString());

            oConsultorSer = new ConsultorService();
            oConsultorSer.ConsultorEditar(oConsultor);

            if (nTipoConsultor == 1)
            {
                oConsultorSer.ConsultorComentarioEditar(nCodigo, txtComentario.Text.Trim());
            }

            oConsultorSer.NacionalidadEliminar(nCodigo);
            oConsultorSer.EducacionEliminar(nCodigo);

            oConsultorSer.CuentasBancariasEliminar(nCodigo);

            oConsultorSer.ConocimientoEliminar(nCodigo);
            oConsultorSer.DonanteEliminar(nCodigo);
            oConsultorSer.PaisEliminar(nCodigo);
            oConsultorSer.DocumentoEliminar(nCodigo);
            oConsultorSer.LenguajeEliminar(nCodigo);
            oConsultorSer.ReferenciaEliminar(nCodigo);

            if (nTipoUsuario == 2)
            {
                nCodigoUsuario = int.Parse(lblUsuarioCodigo.Text);

                oUsuario = new Usuario();
                oUsuario.cod_usu_in = nCodigoUsuario;
                oUsuario.nom_usu_vc = txtCorreo.Text.Trim();
                oUsuario.con_usu_vc = txtContra.Text.Trim();
                oUsuario.est_usu_in = 1;
                oUsuario.cod_per_in = nCodigo;
                oUsuario.tip_usu_in = nTipoUsuario;

                oSeguridadSer = new SeguridadService();
                oSeguridadSer.UsuarioEditar(oUsuario);
            }

            // detalle
            if (!cboNacionalidad1.SelectedValue.Trim().Equals("0"))
            {
                oConsultorSer.NacionalidadInsertar(new ConsultorNacionalidad()
                {
                    cod_per_in = nCodigo,
                    cod_pais_in = int.Parse(cboNacionalidad1.SelectedValue.Trim())
                });
            }

            if (!cboNacionalidad2.SelectedValue.Trim().Equals("0"))
            {
                oConsultorSer.NacionalidadInsertar(new ConsultorNacionalidad()
                {
                    cod_per_in = nCodigo,
                    cod_pais_in = int.Parse(cboNacionalidad2.SelectedValue.Trim())
                });
            }

            //educacion
            for (int i = 0; i < gridEduacion.Items.Count; i++)
            {
                cImagen = "";
                Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");
                DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
                DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
                TextBox txtDescripcion = (TextBox)gridEduacion.Items[i].FindControl("txtDescripcion");
                //DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");
                RadNumericTextBox txtDuracion = (RadNumericTextBox)gridEduacion.Items[i].FindControl("txtDuracion");

                GridNestedViewItem nestedItem = (GridNestedViewItem)gridEduacion.MasterTableView.Items[i].ChildItem;

                TextBox txtInstitucion = (TextBox)nestedItem.FindControl("txtInstitucion");
                RadAsyncUpload RadUpCertificado = (RadAsyncUpload)nestedItem.FindControl("RadUpCertificado");
                Label lblDescarga = (Label)nestedItem.FindControl("lblDescargaN");

                oConsultorSer.EducacionInsertar(new ConsultorEducacion()
                {
                    oNivel = new NivelAcademico() { cod_nivaca_in = int.Parse(cboNivel.SelectedValue.Trim()) },
                    des_conedu_vc = txtDescripcion.Text.Trim(),
                    tip_dur_conedu_in = int.Parse(cboDuracion.SelectedValue.Trim()),
                    can_dur_conedu_in = int.Parse(txtDuracion.Text),
                    ins_conedu_vc = txtInstitucion.Text,
                    adj_conedu_vc = lblDescarga.Text.Trim(),
                    cod_per_in = nCodigo
                });
            }

            // cuentas bancarias
            for (int i = 0; i < this.gvwCuentas.Items.Count; i++)
            {
                DropDownList cbo_banco = (DropDownList)this.gvwCuentas.Items[i].FindControl("cbo_banco");
                DropDownList cbo_moneda = (DropDownList)this.gvwCuentas.Items[i].FindControl("cbo_moneda");
                RadComboBox cbo_tipo_cuenta = (RadComboBox)gvwCuentas.Items[i].FindControl("cbo_tipo_cuenta");
                TextBox txt_nro_cuenta = (TextBox)this.gvwCuentas.Items[i].FindControl("txt_nro_cuenta");

                oConsultorSer.CuentasBancariasInsertar(new IT.Domain.ConsultorCuenta()
                {
                    CodigoConsultor = nCodigo,
                    CodigoBanco = Convert.ToInt32(cbo_banco.SelectedValue),
                    CodigoMoneda = Convert.ToInt32(cbo_moneda.SelectedValue),
                    TipoCuenta = cbo_tipo_cuenta.Text.ToUpper(),
                    NroCuenta = txt_nro_cuenta.Text.Trim(),
                    Estado = 1
                });
            }

            //Donante
            IList<RadListBoxItem> collection = lstDonante.CheckedItems;
            foreach (RadListBoxItem item in collection)
            {
                oConsultorSer.DonanteInsertar(new ConsultorDonante()
                {
                    cod_don_in = int.Parse(item.Value),
                    cod_per_in = nCodigo
                });
            }

            // habilidades

            for (int i = 0; i < listHabilidades.Items.Count; i++)
            {
                RadListBox lstTecnico = (RadListBox)listHabilidades.Items[i].FindControl("lstTecnico");
                IList<RadListBoxItem> oColTecnico = lstTecnico.CheckedItems;

                foreach (RadListBoxItem item in oColTecnico)
                {
                    oConsultorSer.ConocimientoInsertar(new ConsultorConocimiento()
                    {
                        cod_conesp_in = int.Parse(item.Value.ToString()),
                        cod_per_in = nCodigo
                    });
                }
            }

            // paises
            for (int i = 0; i < gridPais.Items.Count; i++)
            {
                DropDownList cboTPais = (DropDownList)gridPais.Items[i].FindControl("cboPais");

                oConsultorSer.PaisInsertar(new ConsultorPais()
                {
                    cod_pais_in = int.Parse(cboTPais.SelectedValue.Trim()),
                    cod_per_in = nCodigo
                });
            }

            // documento
            for (int i = 0; i < gridCV.Items.Count; i++)
            {
                Label lblNro = (Label)gridCV.Items[i].FindControl("lblNro");
                TextBox txtTitulo = (TextBox)gridCV.Items[i].FindControl("txtTitulo");
                RadAsyncUpload RadUpCV = (RadAsyncUpload)gridCV.Items[i].FindControl("RadUpCV");
                Label lblDescarga = (Label)gridCV.Items[i].FindControl("lblDescargaN");
                Label lblOriginal = (Label)gridCV.Items[i].FindControl("lblOriginal");
                cImagen = "";

                oConsultorSer.DocumentoInsertar(new ConsultorDocumento()
                {
                    cod_per_in = nCodigo,
                    nom_condoc_vc = txtTitulo.Text.Trim(),
                    des_condoc_vc = lblDescarga.Text.Trim(),
                    ori_condoc_vc = lblOriginal.Text.Trim()
                });
            }

            // idioma
            for (int i = 0; i < gridLenguaje.Items.Count; i++)
            {
                DropDownList cboLenguaje = (DropDownList)gridLenguaje.Items[i].FindControl("cboLenguaje");
                DropDownList cboHablado = (DropDownList)gridLenguaje.Items[i].FindControl("cboHablado");
                DropDownList cboLeido = (DropDownList)gridLenguaje.Items[i].FindControl("cboLeido");
                DropDownList cboEscrito = (DropDownList)gridLenguaje.Items[i].FindControl("cboEscrito");

                oConsultorSer.LenguajeInsertar(new ConsultorLenguaje()
                {
                    cod_len_in = int.Parse(cboLenguaje.SelectedValue),
                    spk_conlen_in = int.Parse(cboHablado.SelectedValue.Trim()),
                    red_conlen_in = int.Parse(cboLeido.SelectedValue.Trim()),
                    wrt_conlen_in = int.Parse(cboEscrito.SelectedValue.Trim()),
                    cod_per_in = nCodigo
                });
            }

            oListaCReferencia = new List<ConsultorReferencia>();

            foreach (Telerik.Web.UI.GridDataItem item in gridReferencia.Items)
            {
                Label lblCompania = (Label)item.FindControl("lblCompania");
                Label lblPaisCodigo = (Label)item.FindControl("lblPaisCodigo");
                Label lblContacto = (Label)item.FindControl("lblContacto");
                Label lblPuesto = (Label)item.FindControl("lblPuesto");
                Label lblTelefono = (Label)item.FindControl("lblTelefono");
                Label lblCorreo = (Label)item.FindControl("lblCorreo");
                Label lblFeedback = (Label)item.FindControl("lblFeedback");

                oConsultorSer.ReferenciaInsertar(new ConsultorReferencia()
                {
                    cod_per_in = nCodigo,
                    com_ref_vc = lblCompania.Text.Trim(),
                    cod_pais_in = int.Parse(lblPaisCodigo.Text.Trim()),
                    con_ref_vc = lblContacto.Text.Trim(),
                    tel_ref_vc = lblTelefono.Text.Trim(),
                    cor_ref_vc = lblCorreo.Text.Trim(),
                    pue_ref_vc = lblPuesto.Text.Trim(),
                    Feedback = lblFeedback.Text.Trim()
                });
            }

            ObtenerDatosDeConsultor(nCodigo);

            pnlOk.Visible = true;
            cMensajeOk += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorE");
            litOk.Text = cMensajeOk.Trim();
            //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
            //    Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
            //    Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            //{
            //    cMensajeOk += "Se actualizaron los datos del consultor satisfactoriamente.";
            //}
            //else
            //{
            //    cMensajeOk += "Consultant data were updated successfully.";
            //}

        }

        //Label1.Text= RadUpFoto.UploadedFiles[0].InputStream;
    }

    protected void chkContra_CheckedChanged(object sender, EventArgs e)
    {
        txtContra.Enabled = chkContra.Checked;
        txtContraRep.Enabled = chkContra.Checked;
    }

    private void LimpiarReferencia()
    {
        this.pnlErrorReferencia.Visible = false;

        this.lblNumero.Text = string.Empty;
        this.txtCompania.Text = "";
        this.cboCompaniaPais.SelectedValue = "0";
        this.txtCompaniaContacto.Text = "";
        this.txtCompaniaTelefono.Text = "";
        this.txtCompaniaCorreo.Text = "";
        this.txtCompaniaPuesto.Text = "";
        this.txtFeedback.Text = string.Empty;
        this.txtCompania.Focus();
    }

    protected void lnkLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarReferencia();
    }

    protected void lnkAgregarReferencia_Click(object sender, EventArgs e)
    {
        // validar
        pnlErrorReferencia.Visible = false;
        oHelperDiccionario = new Diccionario();
        String cError = "";
        if (txtCompania.Text.Trim().Length == 0)
        {
            cError += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                              oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorCompania"));
            //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
            //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
            //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            //{
            //    cError += "<li>El campo \"Compañia\" es obligatorio.</li>";
            //}
            //else
            //{
            //    cError += "<li>The field \"Company\" is required.</li>";
            //}
        }

        if (cboCompaniaPais.SelectedValue.Trim().Equals("0"))
        {
            cError += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                               oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorPais"));
            //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
            //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
            //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            //{
            //    cError += "<li>El campo \"País\" es obligatorio.</li>";
            //}
            //else
            //{
            //    cError += "<li>The field \"Country\" is required.</li>";
            //}
        }

        if (txtCompaniaContacto.Text.Trim().Length == 0)
        {
            cError += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                              oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorContacto"));
            //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
            //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
            //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            //{
            //    cError += "<li>El campo \"Contacto\" es obligatorio.</li>";
            //}
            //else
            //{
            //    cError += "<li>The field \"Contact\" is required.</li>";
            //}
        }

        if (txtCompaniaPuesto.Text.Trim().Length == 0)
        {
            cError += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio",
                             oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorPuesto"));
            //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
            //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
            //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            //{
            //    cError += "<li>El campo \"Puesto\" es obligatorio.</li>";
            //}
            //else
            //{
            //    cError += "<li>The field \"Job Title\" is required.</li>";
            //}
        }

        if (txtCompaniaCorreo.Text.Trim().Length != 0)
        {
            if (oHelper.ValidarCorreo(txtCompaniaCorreo.Text.Trim()) == false)
            {
                cError += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "UsuarioErrorCorreo");
                //if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                //   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                //{
                //    cError += "<li>El correo tiene un formato incorrecto. Ejm: user@mail.com.</li>";
                //}
                //else
                //{
                //    cError += "<li>The e-mail has an incorrect format. Example: user@mail.com.</li>";
                //}
            }

        }

        if (cError.Trim().Length != 0)
        {
            cError = oHelperDiccionario.DevuelveFormatoWeb(cError);
            pnlErrorReferencia.Visible = true;
            litErrorReferencia.Text = cError;
            return;
        }

        oListaCReferencia = new List<ConsultorReferencia>();

        foreach (Telerik.Web.UI.GridDataItem item in gridReferencia.Items)
        {
            Label lblCompania = (Label)item.FindControl("lblCompania");
            Label lblPaisCodigo = (Label)item.FindControl("lblPaisCodigo");
            Label lblPais = (Label)item.FindControl("lblPais");
            Label lblContacto = (Label)item.FindControl("lblContacto");
            Label lblPuesto = (Label)item.FindControl("lblPuesto");
            Label lblTelefono = (Label)item.FindControl("lblTelefono");
            Label lblCorreo = (Label)item.FindControl("lblCorreo");
            Label lblNro = (Label)item.FindControl("lblNro");
            Label lblFeedback = (Label)item.FindControl("lblFeedback");

            oListaCReferencia.Add(new ConsultorReferencia()
            {
                com_ref_vc = lblCompania.Text.Trim(),
                cod_pais_in = int.Parse(lblPaisCodigo.Text.Trim()),
                con_ref_vc = lblContacto.Text.Trim(),
                tel_ref_vc = lblTelefono.Text.Trim(),
                cor_ref_vc = lblCorreo.Text.Trim(),
                pue_ref_vc = lblPuesto.Text.Trim(),
                nom_pais_vc = lblPais.Text.Trim(),
                Nro = int.Parse(lblNro.Text.Trim()),
                Feedback = lblFeedback.Text.Trim()
            });
        }

        oListaCReferencia.Add(new ConsultorReferencia()
        {
            com_ref_vc = txtCompania.Text.Trim(),
            cod_pais_in = int.Parse(cboCompaniaPais.SelectedValue.Trim()),
            con_ref_vc = txtCompaniaContacto.Text.Trim(),
            tel_ref_vc = txtCompaniaTelefono.Text.Trim(),
            cor_ref_vc = txtCompaniaCorreo.Text.Trim(),
            pue_ref_vc = txtCompaniaPuesto.Text.Trim(),
            nom_pais_vc = cboCompaniaPais.SelectedItem.Text.Trim(),
            Nro = oListaCReferencia.Count + 1,
            Feedback = this.txtFeedback.Text.Trim()
        });

        this.gridReferencia.DataSource = oListaCReferencia;
        this.gridReferencia.DataBind();
        this.LimpiarReferencia();

        this.lnkAgregarReferencia.Visible = true;
        this.lnkEditarReferencia.Visible = false;
        this.lnkLimpiar.Visible = true;

        this.lblAgregarReferencia.Visible = true;
        this.lblEditarReferencia.Visible = false;
        this.lblLimpiar.Visible = true;
    }

    protected void lnkEditarReferencia_Click(object sender, EventArgs e)
    {
        this.pnlErrorReferencia.Visible = false;
        oHelperDiccionario = new Diccionario();

        string mensaje = string.Empty;

        if (this.lblNumero.Text.Trim().Length == 0) mensaje += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "SeleccionDeReferencia");

        if (string.IsNullOrEmpty(mensaje))
        {
            if (this.txtCompania.Text.Trim().Length == 0) mensaje += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio", oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorCompania"));
            if (this.cboCompaniaPais.SelectedValue.Trim().Equals("0")) mensaje += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio", oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorPais"));
            if (this.txtCompaniaContacto.Text.Trim().Length == 0) mensaje += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio", oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorContacto"));
            if (this.txtCompaniaPuesto.Text.Trim().Length == 0) mensaje += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "EsObligatorio", oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorPuesto"));

            if (txtCompaniaCorreo.Text.Trim().Length != 0)
                if (oHelper.ValidarCorreo(txtCompaniaCorreo.Text.Trim()) == false)
                    mensaje += oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "UsuarioErrorCorreo");
        }

        if (mensaje.Trim().Length != 0)
        {
            mensaje = oHelperDiccionario.DevuelveFormatoWeb(mensaje);
            this.pnlErrorReferencia.Visible = true;
            this.litErrorReferencia.Text = mensaje;
            return;
        }

        oListaCReferencia = new List<ConsultorReferencia>();

        foreach (Telerik.Web.UI.GridDataItem item in gridReferencia.Items)
        {
            Label lblCompania = (Label)item.FindControl("lblCompania");
            Label lblPaisCodigo = (Label)item.FindControl("lblPaisCodigo");
            Label lblPais = (Label)item.FindControl("lblPais");
            Label lblContacto = (Label)item.FindControl("lblContacto");
            Label lblPuesto = (Label)item.FindControl("lblPuesto");
            Label lblTelefono = (Label)item.FindControl("lblTelefono");
            Label lblCorreo = (Label)item.FindControl("lblCorreo");
            Label lblNro = (Label)item.FindControl("lblNro");
            Label lblFeedback = (Label)item.FindControl("lblFeedback");

            oListaCReferencia.Add(new ConsultorReferencia()
            {
                com_ref_vc = lblCompania.Text.Trim(),
                cod_pais_in = int.Parse(lblPaisCodigo.Text.Trim()),
                con_ref_vc = lblContacto.Text.Trim(),
                tel_ref_vc = lblTelefono.Text.Trim(),
                cor_ref_vc = lblCorreo.Text.Trim(),
                pue_ref_vc = lblPuesto.Text.Trim(),
                nom_pais_vc = lblPais.Text.Trim(),
                Nro = int.Parse(lblNro.Text.Trim()),
                Feedback = lblFeedback.Text.Trim()
            });
        }

        foreach (ConsultorReferencia referencia in oListaCReferencia)
        {
            if (referencia.Nro == Convert.ToInt32(this.lblNumero.Text))
            {
                referencia.com_ref_vc = txtCompania.Text.Trim();
                referencia.cod_pais_in = int.Parse(cboCompaniaPais.SelectedValue.Trim());
                referencia.con_ref_vc = txtCompaniaContacto.Text.Trim();
                referencia.tel_ref_vc = txtCompaniaTelefono.Text.Trim();
                referencia.cor_ref_vc = txtCompaniaCorreo.Text.Trim();
                referencia.pue_ref_vc = txtCompaniaPuesto.Text.Trim();
                referencia.nom_pais_vc = cboCompaniaPais.SelectedItem.Text.Trim();
                referencia.Feedback = this.txtFeedback.Text.Trim();
            }
        }

        this.gridReferencia.DataSource = oListaCReferencia;
        this.gridReferencia.DataBind();
        this.LimpiarReferencia();

        this.lnkAgregarReferencia.Visible = true;
        this.lnkEditarReferencia.Visible = false;
        this.lnkLimpiar.Visible = true;

        this.lblAgregarReferencia.Visible = true;
        this.lblEditarReferencia.Visible = false;
        this.lblLimpiar.Visible = true;
    }

    protected void gridReferencia_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Eliminar")
        {
            #region Eliminar

            actualizarDatos();

            if (Session["Usuario"] != null)
            {
                oTUsuario = (Usuario)Session["Usuario"];
                int nContador = 0, nNro = 0, nCodigo = 0;
                nNro = Convert.ToInt32(e.CommandArgument);
                oListaCReferencia = new List<ConsultorReferencia>();

                foreach (Telerik.Web.UI.GridDataItem item in gridReferencia.Items)
                {
                    Label lblNro = (Label)item.FindControl("lblNro");
                    Label lblCompania = (Label)item.FindControl("lblCompania");
                    Label lblPaisCodigo = (Label)item.FindControl("lblPaisCodigo");
                    Label lblPais = (Label)item.FindControl("lblPais");
                    Label lblContacto = (Label)item.FindControl("lblContacto");
                    Label lblPuesto = (Label)item.FindControl("lblPuesto");
                    Label lblTelefono = (Label)item.FindControl("lblTelefono");
                    Label lblCorreo = (Label)item.FindControl("lblCorreo");
                    Label lblFeedback = (Label)item.FindControl("lblFeedback");

                    nCodigo = int.Parse(lblNro.Text.ToString().Trim());

                    if (nCodigo != nNro)
                    {
                        nContador++;
                        oListaCReferencia.Add(new ConsultorReferencia()
                        {
                            Nro = nContador,
                            com_ref_vc = lblCompania.Text.Trim(),
                            cod_pais_in = int.Parse(lblPaisCodigo.Text.Trim()),
                            con_ref_vc = lblContacto.Text.Trim(),
                            tel_ref_vc = lblTelefono.Text.Trim(),
                            cor_ref_vc = lblCorreo.Text.Trim(),
                            pue_ref_vc = lblPuesto.Text.Trim(),
                            nom_pais_vc = lblPais.Text.Trim(),
                            Feedback = lblFeedback.Text.Trim()
                        });
                    }
                }

                this.gridReferencia.DataSource = oListaCReferencia;
                this.gridReferencia.DataBind();

                this.LimpiarReferencia();
            }

            this.lnkAgregarReferencia.Visible = true;
            this.lnkEditarReferencia.Visible = false;
            this.lnkLimpiar.Visible = true;

            this.lblAgregarReferencia.Visible = true;
            this.lblEditarReferencia.Visible = false;
            this.lblLimpiar.Visible = true;

            #endregion
        }
        else
        {
            if (e.CommandName == "Editar")
            {
                #region Editar

                actualizarDatos();

                if (Session["Usuario"] != null)
                {
                    oTUsuario = (Usuario)Session["Usuario"];
                    int nNro = 0, nCodigo = 0;
                    nNro = Convert.ToInt32(e.CommandArgument);
                    oListaCReferencia = new List<ConsultorReferencia>();

                    foreach (Telerik.Web.UI.GridDataItem item in gridReferencia.Items)
                    {
                        Label lblNro = (Label)item.FindControl("lblNro");
                        Label lblCompania = (Label)item.FindControl("lblCompania");
                        Label lblPaisCodigo = (Label)item.FindControl("lblPaisCodigo");
                        Label lblPais = (Label)item.FindControl("lblPais");
                        Label lblContacto = (Label)item.FindControl("lblContacto");
                        Label lblPuesto = (Label)item.FindControl("lblPuesto");
                        Label lblTelefono = (Label)item.FindControl("lblTelefono");
                        Label lblCorreo = (Label)item.FindControl("lblCorreo");
                        Label lblFeedback = (Label)item.FindControl("lblFeedback");

                        nCodigo = int.Parse(lblNro.Text.ToString().Trim());

                        if (nCodigo == nNro)
                        {
                            this.lblNumero.Text = nNro.ToString();
                            this.txtCompania.Text = lblCompania.Text.Trim();
                            this.cboCompaniaPais.SelectedValue = lblPaisCodigo.Text.Trim();
                            this.txtCompaniaContacto.Text = lblContacto.Text.Trim();
                            this.txtCompaniaTelefono.Text = lblTelefono.Text.Trim();
                            this.txtCompaniaCorreo.Text = lblCorreo.Text.Trim();
                            this.txtCompaniaPuesto.Text = lblPuesto.Text.Trim();
                            this.txtFeedback.Text = lblFeedback.Text.Trim();
                            this.txtCompania.Focus();
                        }
                    }

                    this.pnlErrorReferencia.Visible = false;

                    this.lnkAgregarReferencia.Visible = false;
                    this.lnkEditarReferencia.Visible = true;
                    this.lnkLimpiar.Visible = false;

                    this.lblAgregarReferencia.Visible = false;
                    this.lblEditarReferencia.Visible = true;
                    this.lblLimpiar.Visible = false;
                }

                #endregion
            }
        }
    }

    //helpers 
    private IList<ConsultorDocumento> DevolverCV()
    {
        IList<ConsultorDocumento> oListaTemCV = new List<ConsultorDocumento>();
        cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
        cRutaContenido = Server.MapPath(cCarpetaContenido);
        UploadedFile oFile;
        String cImagen = "", cTexto = "", cTextoOriginal = "";
        DateTime dfecha;

        for (int i = 0; i < gridCV.Items.Count; i++)
        {
            cTexto = ""; cTextoOriginal = "";
            Label lblNro = (Label)gridCV.Items[i].FindControl("lblNro");
            Label lblCodigo = (Label)gridCV.Items[i].FindControl("lblCodigo");
            Label lblOriginal = (Label)gridCV.Items[i].FindControl("lblOriginal");
            TextBox txtTitulo = (TextBox)gridCV.Items[i].FindControl("txtTitulo");
            Label lblDescarga = (Label)gridCV.Items[i].FindControl("lblDescarga");
            Label lblDescargaN = (Label)gridCV.Items[i].FindControl("lblDescargaN");
            RadAsyncUpload RadUpCV = (RadAsyncUpload)gridCV.Items[i].FindControl("RadUpCV");


            if (RadUpCV.UploadedFiles.Count > 0)
            {
                oFile = RadUpCV.UploadedFiles[0];
                if (txtTitulo.Text.Trim().Length != 0)
                {
                    cTexto = txtTitulo.Text.Trim();
                }
                else
                {
                    cTexto = oFile.GetNameWithoutExtension().Trim();
                }

                cTextoOriginal = oFile.GetNameWithoutExtension().Trim();
                dfecha = new DateTime();
                dfecha = DateTime.Now;

                cImagen = oHelper.ReplaceCaracter(txtNombre.Text.Trim()) + "_" + oHelper.ReplaceCaracter(txtApellido.Text.Trim()) +
                          "_CV_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                          dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                          dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim() + oFile.GetExtension();

                try
                {
                    oFile.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                }
                catch
                {
                    Random random = new Random();
                    int randomNumber = random.Next(0, 2000);

                    cImagen = randomNumber.ToString().Trim() +
                          "_CV_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                          dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                          dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim();
                    oFile.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                }

                lblDescarga.Text = cImagen.Trim();
                lblDescargaN.Text = cImagen.Trim();
            }
            else if (RadUpCV.UploadedFiles.Count == 0)
            {

                if (txtTitulo.Text.Trim().Length != 0)
                {
                    cTexto = txtTitulo.Text.Trim();
                }
                else if (txtTitulo.Text.Trim().Length == 0)
                {

                    if (lblOriginal.Text.Trim().Length != 0)
                    {
                        cTexto = lblOriginal.Text.Trim();
                    }
                    else
                    {
                        cTexto = lblDescargaN.Text.Trim();
                    }
                }

                cTextoOriginal = lblOriginal.Text.Trim();
            }

            oListaTemCV.Add(new ConsultorDocumento()
            {
                cod_condoc_in = int.Parse(lblCodigo.Text),
                Nro = int.Parse(lblNro.Text),
                nom_condoc_vc = cTexto,
                des_condoc_vc = lblDescargaN.Text.Trim(),
                ori_condoc_vc = cTextoOriginal
            });

        }

        return oListaTemCV;
    }

    private void FormatoCV(IList<ConsultorDocumento> oListaTemCV)
    {
        gridCV.DataSource = oListaTemCV;
        gridCV.DataBind();
        String cDescarga = "";
        for (int i = 0; i < gridCV.Items.Count; i++)
        {
            Label lblDescarga = (Label)gridCV.Items[i].FindControl("lblDescarga");
            HyperLink lnkDescargar = (HyperLink)gridCV.Items[i].FindControl("lnkDescargar");

            if (lblDescarga.Text.Trim().Length != 0)
            {
                if (lblDescarga.Text.Trim().Length > 15)
                {
                    cDescarga = lblDescarga.Text.Trim().Substring(0, 15) + "...";
                    lblDescarga.Text = cDescarga;
                }
                lnkDescargar.Visible = true;
            }

        }
    }

    private IList<ConsultorEducacion> DevolverEducacion()
    {

        IList<ConsultorEducacion> oListaTemDocumento = new List<ConsultorEducacion>();
        cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
        cRutaContenido = Server.MapPath(cCarpetaContenido);

        UploadedFile oFile;
        String cImagen = "";
        DateTime dfecha;

        for (int i = 0; i < gridEduacion.Items.Count; i++)
        {
            Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");
            Label lblCodigo = (Label)gridEduacion.Items[i].FindControl("lblCodigo");
            DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");

            TextBox txtDescripcion = (TextBox)gridEduacion.Items[i].FindControl("txtDescripcion");
            DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
            RadNumericTextBox txtDuracion = (RadNumericTextBox)gridEduacion.Items[i].FindControl("txtDuracion");

            GridNestedViewItem nestedItem = (GridNestedViewItem)gridEduacion.MasterTableView.Items[i].ChildItem;

            TextBox txtInstitucion = (TextBox)nestedItem.FindControl("txtInstitucion");

            Label lblDescarga = (Label)nestedItem.FindControl("lblDescarga");
            Label lblDescargaN = (Label)nestedItem.FindControl("lblDescargaN");
            HyperLink lnkDescargar = (HyperLink)nestedItem.FindControl("lnkDescargar");

            RadAsyncUpload RadUpCertificado = (RadAsyncUpload)nestedItem.FindControl("RadUpCertificado");

            if (RadUpCertificado.UploadedFiles.Count > 0)
            {


                oFile = RadUpCertificado.UploadedFiles[0];
                dfecha = new DateTime();
                dfecha = DateTime.Now;

                cImagen = oHelper.ReplaceCaracter(txtNombre.Text.Trim()) + "_" + oHelper.ReplaceCaracter(txtApellido.Text.Trim()) +
                          "_CE_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                          dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                          dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim() + oFile.GetExtension();

                try
                {
                    oFile.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                }
                catch
                {
                    Random random = new Random();
                    int randomNumber = random.Next(0, 2000);
                    cImagen = randomNumber.ToString().Trim() +
                          "_CE_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                          dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                          dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim();
                    oFile.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                }

                lblDescargaN.Text = cImagen.Trim();
                lblDescarga.Text = cImagen.Trim();

            }


            oListaTemDocumento.Add(new ConsultorEducacion()
            {
                cod_conedu_in = int.Parse(lblCodigo.Text),
                Nro = int.Parse(lblNro.Text.Trim()),
                oNivel = new NivelAcademico()
                {
                    cod_nivaca_in = int.Parse(cboNivel.SelectedValue.Trim())
                },
                des_conedu_vc = txtDescripcion.Text.Trim(),

                tip_dur_conedu_in = int.Parse(cboDuracion.SelectedValue.Trim()),
                can_dur_conedu_in = int.Parse(txtDuracion.Text),
                ins_conedu_vc = txtInstitucion.Text,
                adj_conedu_vc = lblDescargaN.Text.Trim(),

                bExpand = gridEduacion.Items[i].Expanded
            });
        }

        return oListaTemDocumento;
    }

    private void FormatoEducacion(IList<ConsultorEducacion> oListaTemDocumento)
    {
        if (Session["Usuario"] != null)
        {
            oTUsuario = (Usuario)Session["Usuario"];

            gridEduacion.DataSource = oListaTemDocumento;
            gridEduacion.DataBind();

            String cSeleccione = "";

            if (oTUsuario.oIdioma.cod_idi_in == 1)
            {
                cSeleccione = "Select";
            }
            else
            {

                cSeleccione = "Seleccione";
            }

            oAdministracionSer = new AdministracionService();
            oListaNivel = oAdministracionSer.NivelAcademicoIdiomaListar(oTUsuario.oIdioma.cod_idi_in);
            oListaDuracion = oAdministracionSer.CatalogoBuscar(2001, oTUsuario.oIdioma.cod_idi_in);

            oListaDescripcion = new List<DescripcionAcademica>();
            oListaDescripcion.Add(new DescripcionAcademica() { nom_desaca_vc = cSeleccione });

            if (oListaNivel.Count == 0)
            {
                oListaNivel.Add(new NivelAcademico() { nom_nivacaidi_vc = cSeleccione });
            }
            else
            {
                oListaNivel.Insert(0, new NivelAcademico() { nom_nivacaidi_vc = cSeleccione });

            }

            if (oListaDuracion.Count == 0)
            {
                oListaDuracion.Add(new Catalogo() { nom_cat_vc = cSeleccione });
            }
            else
            {
                oListaDuracion.Insert(0, new Catalogo() { nom_cat_vc = cSeleccione });
            }

            int nNivel = 0;
            int nNro = 0;
            String cDescarga = "";

            for (int i = 0; i < gridEduacion.Items.Count; i++)
            {
                Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");
                DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
                DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");

                Label lblNivel = (Label)gridEduacion.Items[i].FindControl("lblNivel");
                Label lblDuracion = (Label)gridEduacion.Items[i].FindControl("lblDuracion");

                GridNestedViewItem nestedItem = (GridNestedViewItem)gridEduacion.MasterTableView.Items[i].ChildItem;

                Label lblDescarga = (Label)nestedItem.FindControl("lblDescarga");

                HyperLink lnkDescargar = (HyperLink)nestedItem.FindControl("lnkDescargar");


                nNro = int.Parse(lblNro.Text.Trim());

                cboNivel.DataSource = oListaNivel;
                cboNivel.DataValueField = "cod_nivaca_in";
                cboNivel.DataTextField = "nom_nivacaidi_vc";
                cboNivel.DataBind();
                cboNivel.SelectedValue = lblNivel.Text.Trim();

                nNivel = int.Parse(lblNivel.Text.Trim());
                oAdministracionSer = new AdministracionService();
                oListaDescripcion = new List<DescripcionAcademica>();
                oListaDescripcion = oAdministracionSer.DescripcionAcademicaIdiomaListar(nNivel, oTUsuario.oIdioma.cod_idi_in);

                if (oListaDescripcion != null)
                {
                    if (oListaDescripcion.Count != 0)
                    {
                        oListaDescripcion.Insert(0, new DescripcionAcademica() { nom_desaca_vc = cSeleccione });
                    }
                    else
                    {
                        oListaDescripcion.Add(new DescripcionAcademica() { nom_desaca_vc = cSeleccione });
                    }
                }
                else
                {
                    oListaDescripcion.Add(new DescripcionAcademica() { nom_desaca_vc = cSeleccione });
                }


                if (lblDescarga.Text.Trim().Length != 0)
                {
                    if (lblDescarga.Text.Trim().Length > 8)
                    {
                        cDescarga = lblDescarga.Text.Trim().Substring(0, 8) + "...";
                        lblDescarga.Text = cDescarga;
                    }
                    lnkDescargar.Visible = true;
                }

                cboDuracion.DataSource = oListaDuracion;
                cboDuracion.DataValueField = "val_cat_in";
                cboDuracion.DataTextField = "nom_cat_vc";
                cboDuracion.DataBind();
                cboDuracion.SelectedValue = lblDuracion.Text.Trim();


                foreach (ConsultorEducacion ent in oListaTemDocumento)
                {
                    if (ent.Nro == nNro)
                    {

                        gridEduacion.Items[i].Expanded = ent.bExpand;
                    }
                }
            }
        }

    }

    private IList<ConsultorLenguaje> DevolverLenguaje()
    {

        IList<ConsultorLenguaje> oListaTemLenguaje = new List<ConsultorLenguaje>();

        for (int i = 0; i < gridLenguaje.Items.Count; i++)
        {
            DropDownList cboLenguaje = (DropDownList)gridLenguaje.Items[i].FindControl("cboLenguaje");
            DropDownList cboHablado = (DropDownList)gridLenguaje.Items[i].FindControl("cboHablado");
            DropDownList cboLeido = (DropDownList)gridLenguaje.Items[i].FindControl("cboLeido");
            DropDownList cboEscrito = (DropDownList)gridLenguaje.Items[i].FindControl("cboEscrito");
            Label lblNro = (Label)gridLenguaje.Items[i].FindControl("lblNro");
            Label lblCodigo = (Label)gridLenguaje.Items[i].FindControl("lblCodigo");

            oListaTemLenguaje.Add(new ConsultorLenguaje()
            {
                cod_conlen_in = int.Parse(lblCodigo.Text),
                Nro = int.Parse(lblNro.Text),
                cod_len_in = int.Parse(cboLenguaje.SelectedValue.ToString()),
                spk_conlen_in = int.Parse(cboHablado.SelectedValue.ToString()),
                red_conlen_in = int.Parse(cboLeido.SelectedValue.ToString()),
                wrt_conlen_in = int.Parse(cboEscrito.SelectedValue.ToString())
            });
        }

        return oListaTemLenguaje;
    }

    private void FormatoLenguaje(IList<ConsultorLenguaje> oListaTemLenguaje)
    {

        if (Session["Usuario"] != null)
        {
            oTUsuario = (Usuario)Session["Usuario"];

            gridLenguaje.DataSource = oListaTemLenguaje;
            gridLenguaje.DataBind();

            String cSeleccione = "";

            if (oTUsuario.oIdioma.cod_idi_in == 1)
            {
                cSeleccione = "Select";
            }
            else
            {

                cSeleccione = "Seleccione";
            }
            oListaIdioma = new List<Lenguaje>();
            oListaHablado = new List<Catalogo>();
            oAdministracionSer = new AdministracionService();
            oListaIdioma = oAdministracionSer.LenguajeIdiomaListar(oTUsuario.oIdioma.cod_idi_in);
            oListaHablado = oAdministracionSer.CatalogoBuscar(2002, oTUsuario.oIdioma.cod_idi_in);

            if (oListaIdioma.Count == 0)
            {
                oListaIdioma.Add(new Lenguaje() { nom_len_vc = cSeleccione });
            }
            else
            {
                oListaIdioma.Insert(0, new Lenguaje() { nom_len_vc = cSeleccione });
            }

            if (oListaHablado.Count == 0)
            {
                oListaHablado.Add(new Catalogo() { nom_cat_vc = cSeleccione });
            }
            else
            {
                oListaHablado.Insert(0, new Catalogo() { nom_cat_vc = cSeleccione });
            }

            for (int i = 0; i < gridLenguaje.Items.Count; i++)
            {
                DropDownList cboLenguaje = (DropDownList)gridLenguaje.Items[i].FindControl("cboLenguaje");
                DropDownList cboHablado = (DropDownList)gridLenguaje.Items[i].FindControl("cboHablado");
                DropDownList cboLeido = (DropDownList)gridLenguaje.Items[i].FindControl("cboLeido");
                DropDownList cboEscrito = (DropDownList)gridLenguaje.Items[i].FindControl("cboEscrito");

                Label lblLenguaje = (Label)gridLenguaje.Items[i].FindControl("lblLenguaje");
                Label lblHablado = (Label)gridLenguaje.Items[i].FindControl("lblHablado");
                Label lblLeido = (Label)gridLenguaje.Items[i].FindControl("lblLeido");
                Label lblEscrito = (Label)gridLenguaje.Items[i].FindControl("lblEscrito");

                cboLenguaje.DataSource = oListaIdioma;
                cboLenguaje.DataValueField = "cod_len_in";
                cboLenguaje.DataTextField = "nom_len_vc";
                cboLenguaje.DataBind();
                cboLenguaje.SelectedValue = lblLenguaje.Text.Trim();

                cboHablado.DataSource = oListaHablado;
                cboHablado.DataValueField = "val_cat_in";
                cboHablado.DataTextField = "nom_cat_vc";
                cboHablado.DataBind();
                cboHablado.SelectedValue = lblHablado.Text.Trim();

                cboLeido.DataSource = oListaHablado;
                cboLeido.DataValueField = "val_cat_in";
                cboLeido.DataTextField = "nom_cat_vc";
                cboLeido.DataBind();
                cboLeido.SelectedValue = lblLeido.Text.Trim();

                cboEscrito.DataSource = oListaHablado;
                cboEscrito.DataValueField = "val_cat_in";
                cboEscrito.DataTextField = "nom_cat_vc";
                cboEscrito.DataBind();
                cboEscrito.SelectedValue = lblEscrito.Text.Trim();

            }
        }
    }

    private IList<ConsultorPais> DevolverPais()
    {
        IList<ConsultorPais> oListaTemPais = new List<ConsultorPais>();

        for (int i = 0; i < gridPais.Items.Count; i++)
        {
            Label lblNro = (Label)gridPais.Items[i].FindControl("lblNro");
            DropDownList cboPais = (DropDownList)gridPais.Items[i].FindControl("cboPais");

            oListaTemPais.Add(new ConsultorPais()
            {
                Nro = int.Parse(lblNro.Text.Trim()),
                cod_pais_in = int.Parse(cboPais.SelectedValue.Trim()),
            });
        }

        return oListaTemPais;
    }

    private void FormatoPais(IList<ConsultorPais> oListaTemPais)
    {

        if (Session["Usuario"] != null)
        {
            oTUsuario = (Usuario)Session["Usuario"];

            gridPais.DataSource = oListaTemPais;
            gridPais.DataBind();


            String cSeleccione = "";

            if (oTUsuario.oIdioma.cod_idi_in == 1)
            {
                cSeleccione = "Select";
            }
            else
            {

                cSeleccione = "Seleccione";
            }

            oAdministracionSer = new AdministracionService();
            oListaPais = oAdministracionSer.PaisIdiomaListar(oTUsuario.oIdioma.cod_idi_in);
            oListaPais = (from pais in oListaPais
                          orderby pais.nom_pais_vc
                          select pais).ToList();
            if (oListaPais.Count != 0)
            {
                oListaPais.Insert(0, new Pais()
                {
                    cod_pais_in = 0,
                    nom_pais_vc = cSeleccione
                });
            }
            else
            {
                oListaPais.Add(new Pais()
                {
                    cod_pais_in = 0,
                    nom_pais_vc = cSeleccione
                });
            }
            for (int i = 0; i < gridPais.Items.Count; i++)
            {
                Label lblPais = (Label)gridPais.Items[i].FindControl("lblPais");
                DropDownList cboPais = (DropDownList)gridPais.Items[i].FindControl("cboPais");

                cboPais.DataSource = oListaPais;
                cboPais.DataTextField = "nom_pais_vc";
                cboPais.DataValueField = "cod_pais_in";
                cboPais.DataBind();

                cboPais.SelectedValue = lblPais.Text.Trim();

            }
        }
    }

    protected override void InitializeCulture()
    {
        oHelperDiccionario = new Diccionario();
        if (Request.QueryString["lang"] != null)
        {
            if (Session["Usuario"] != null)
            {
                oTUsuario = (Usuario)Session["Usuario"];
                oTUsuario.oIdioma.cul_idi_vc = Request.QueryString["lang"].Trim();
                oTUsuario.oIdioma.cod_idi_in = oHelperDiccionario.DevolverConexionConfiguracionIdioma(Request.QueryString["lang"].Trim());

                //actualizar
                oSeguridadSer = new SeguridadService();
                oSeguridadSer.UsuarioIdiomaEditar(oTUsuario);

                Session["Usuario"] = oTUsuario;
            }

        }

        if (Session["Usuario"] != null)
        {
            oTUsuario = (Usuario)Session["Usuario"];
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(oTUsuario.oIdioma.cul_idi_vc.Trim());
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(oTUsuario.oIdioma.cul_idi_vc.Trim());

        }
        else
        {
            String cIdioma = oHelperDiccionario.DevolverSinConexionConfiguracionIdioma(Thread.CurrentThread.CurrentCulture.Name);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cIdioma);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cIdioma);
        }

        base.InitializeCulture();
    }

    void actualizarDNI()
    {
        cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
        cRutaContenido = Server.MapPath(cCarpetaContenido);

        UploadedFile oImagen;
        String cImagen = "";
        DateTime dfecha = new DateTime();
        dfecha = DateTime.Now;

        if (rauDNI.UploadedFiles.Count > 0)
        {
            oImagen = rauDNI.UploadedFiles[0];
            cImagen = "DNI_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + oImagen.GetExtension();

            ViewState["vws_fileDNI"] = cImagen;

            try
            {
                oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
            }
            catch
            {

            }

            lblDNIDescarga.Text = cImagen.Trim();

            if (lblDNIDescarga.Text.Trim().Length != 0)
            {
                lnkDDescargar.NavigateUrl = "~/downloading.aspx?file=" + lblDNIDescarga.Text.Trim();
                lnkDDescargar.ToolTip = lblFotoDescarga.Text.Trim();
                lnkDDescargar.Visible = true;

                if (lblDNIDescarga.Text.Trim().Length > 15)
                {
                    String cDescarga = lblDNIDescarga.Text.Trim().Substring(0, 15) + "...";
                    lblDDescarga.Text = cDescarga;
                }
            }
        }
    }

    void actualizarSuspension4ta()
    {
        cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
        cRutaContenido = Server.MapPath(cCarpetaContenido);

        UploadedFile oImagen;
        String cImagen = "";
        DateTime dfecha = new DateTime();
        dfecha = DateTime.Now;

        if (rauSuspension4ta.UploadedFiles.Count > 0)
        {
            oImagen = rauSuspension4ta.UploadedFiles[0];
            cImagen = "SUSPENSION DE 4TA_" + DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + oImagen.GetExtension();

            ViewState["vws_fileSuspension4ta"] = cImagen;

            try
            {
                oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
            }
            catch
            {

            }

            lblSuspension4taDescarga.Text = cImagen.Trim();

            if (lblSuspension4taDescarga.Text.Trim().Length != 0)
            {
                lnkS4taDescargar.NavigateUrl = "~/downloading.aspx?file=" + lblSuspension4taDescarga.Text.Trim();
                lnkS4taDescargar.ToolTip = lblSuspension4taDescarga.Text.Trim();
                lnkS4taDescargar.Visible = true;

                if (lblSuspension4taDescarga.Text.Trim().Length > 15)
                {
                    String cDescarga = lblSuspension4taDescarga.Text.Trim().Substring(0, 15) + "...";
                    lblS4taDescarga.Text = cDescarga;
                }
            }

        }

    }

    public bool Validar_RUC(string RUC)
    {
        bool Flag = false;

        if (RUC.Length == 11)
        {
            //Recorriendo el RUC para las Validaciones Previas
            for (int i = 0; i < RUC.Length; i++)
            {
                string strCaracter = RUC.Substring(i, 1);
                int keyascii = Convert.ToInt32(System.Text.Encoding.ASCII.GetBytes(strCaracter)[0].ToString());

                if (keyascii >= 48 && keyascii <= 57)
                {//Si el carácter leído es un número
                    if (i == 0 && keyascii == 48)
                    {//Si el primer dígito es igual a 0
                        Flag = true;
                        break;
                    }
                }
                else
                {
                    Flag = true;
                    break;
                }
            }

            string UltDigito = RUC.Substring(10);
            int AcumRUC = 0, Residuo = 0, Complemento = 0;
            string Resultado = string.Empty;
            int[] Pesos = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

            for (int i = 0; i < RUC.Length - 1; i++)
            {
                AcumRUC = AcumRUC + (Convert.ToInt32(RUC.Substring(i, 1)) * Pesos[i]);
            }
            Residuo = AcumRUC % 11;
            Complemento = 11 - Residuo;
            Resultado = Complemento.ToString().Substring(Complemento.ToString().Length - 1);

            if (Resultado != UltDigito)
            {
                Flag = true;
                //objcIniArray.IniWrite("C:\\LogErrores.log", "LogErrores", "Error " + intContadorError.ToString(), "El RUC " + RUC + " no es válido.");
            }
        }
        else
        {
            Flag = true;
            //objcIniArray.IniWrite("C:\\LogErrores.log", "LogErrores", "Error " + intContadorError.ToString(), "El RUC " + RUC + " no tiene 11 dígitos.");
        }

        return Flag;
    }

    protected void gvwCuentas_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Eliminar")
            {
                int nContador = 0, nNro = 0, nCodigo = 0;

                nNro = Convert.ToInt32(e.CommandArgument);

                cuentasDeConsultor = new List<IT.Domain.ConsultorCuenta>();

                for (int i = 0; i < gvwCuentas.Items.Count; i++)
                {
                    Label lblNro = (Label)gvwCuentas.Items[i].FindControl("lblNro");
                    nCodigo = int.Parse(lblNro.Text.ToString().Trim());

                    DropDownList cbo_banco = (DropDownList)gvwCuentas.Items[i].FindControl("cbo_banco");
                    DropDownList cbo_moneda = (DropDownList)gvwCuentas.Items[i].FindControl("cbo_moneda");
                    RadComboBox cbo_tipo_cuenta = (RadComboBox)gvwCuentas.Items[i].FindControl("cbo_tipo_cuenta");
                    TextBox txt_nro_cuenta = (TextBox)gvwCuentas.Items[i].FindControl("txt_nro_cuenta");

                    if (nCodigo != nNro)
                    {
                        nContador++;
                        cuentasDeConsultor.Add(new IT.Domain.ConsultorCuenta()
                        {
                            CodigoItem = nContador,
                            CodigoBanco = int.Parse(cbo_banco.SelectedValue),
                            CodigoMoneda = int.Parse(cbo_moneda.SelectedValue),
                            TipoCuenta = cbo_tipo_cuenta.Text.ToUpper(),
                            NroCuenta = txt_nro_cuenta.Text.Trim(),
                            Estado = 1
                        });
                    }
                }

                this.gvwCuentas.DataSource = cuentasDeConsultor;
                this.gvwCuentas.DataBind();

                for (int i = 0; i < this.gvwCuentas.Items.Count; i++)
                {
                    DropDownList cbo_banco = (DropDownList)gvwCuentas.Items[i].FindControl("cbo_banco");
                    DropDownList cbo_moneda = (DropDownList)gvwCuentas.Items[i].FindControl("cbo_moneda");
                    RadComboBox cbo_tipo_cuenta = (RadComboBox)gvwCuentas.Items[i].FindControl("cbo_tipo_cuenta");
                    TextBox txt_nro_cuenta = (TextBox)gvwCuentas.Items[i].FindControl("txt_nro_cuenta");

                    cbo_tipo_cuenta.Text = cuentasDeConsultor[i].TipoCuenta;
                    txt_nro_cuenta.Text = cuentasDeConsultor[i].NroCuenta;

                    this.ListarBancos(cbo_banco);
                    cbo_banco.SelectedValue = cuentasDeConsultor[i].CodigoBanco.ToString();
                    cbo_banco.ValidationGroup = (cuentasDeConsultor[i].CodigoItem - 1).ToString();

                    this.ListarMonedas(cbo_moneda);
                    cbo_moneda.SelectedValue = cuentasDeConsultor[i].CodigoMoneda.ToString();
                    cbo_moneda.ValidationGroup = (cuentasDeConsultor[i].CodigoItem - 1).ToString();

                    this.ListarTipoCuentas(cbo_tipo_cuenta);
                }
            }
        }
        catch (Exception ex)
        {
            this.litError.Text = "No se pudo eliminar la cuenta del consultor.";
            this.pnlError.Visible = true;
        }
    }

    private void ListarMonedas(DropDownList cbo)
    {
        try
        {
            EntitiesConnection db = new EntitiesConnection();

            Moneda oSeleccione = new Moneda() { CodigoMoneda = 0, Descripcion = "-- SELECCIONAR MONEDA --" };

            List<Moneda> oList = new List<Moneda>();

            oList = (from mon in db.Moneda select mon).ToList();
            oList.Insert(0, oSeleccione);

            cbo.DataSource = oList;
            cbo.DataTextField = "Descripcion";
            cbo.DataValueField = "CodigoMoneda";
            cbo.DataBind();
        }
        catch (Exception ex)
        {
            this.pnlError.Visible = true;
            this.litError.Text = ex.Message;
        }
    }

    private void ListarBancos(DropDownList cbo)
    {
        try
        {
            EntitiesConnection db = new EntitiesConnection();

            Banco oSeleccione = new Banco() { CodigoBanco = 0, Descripcion = "-- SELECCIONAR BANCO --" };
            List<Banco> oList = new List<Banco>();
            oList = (from banco in db.Banco select banco).ToList();
            oList.Insert(0, oSeleccione);

            cbo.DataSource = oList;
            cbo.DataTextField = "Descripcion";
            cbo.DataValueField = "CodigoBanco";
            cbo.DataBind();
        }
        catch (Exception ex)
        {
            this.pnlError.Visible = true;
            this.litError.Text = ex.Message;
        }
    }

    private void ListarTipoCuentas(RadComboBox cbo)
    {
        try
        {
            EntitiesConnection db = new EntitiesConnection();

            List<string> oList = db.ConsultorCuenta.Select(s => s.TipoCuenta).Distinct().ToList();
            cbo.DataSource = oList;
            foreach (string s in oList)
            {
                cbo.Items.Add(new RadComboBoxItem(s, s));
            }
        }
        catch (Exception ex)
        {
            this.pnlError.Visible = true;
            this.litError.Text = ex.Message;
        }
    }

    protected void lnkAgregarCuenta_Click(object sender, EventArgs e)
    {
        try
        {
            string fileDNI = ViewState["vws_fileDNI"] != null ? ViewState["vws_fileDNI"].ToString() : string.Empty;
            string fileSuspension4ta = ViewState["vws_fileSuspension4ta"] != null ? ViewState["vws_fileSuspension4ta"].ToString() : string.Empty;

            // DNI
            this.lnkDDescargar.NavigateUrl = "~/downloading.aspx?file=" + fileDNI;
            this.lnkDDescargar.ToolTip = fileDNI;
            this.lnkDDescargar.Visible = true;
            this.lblDDescarga.Visible = true;

            // SUSPENSIÓN DE 4TA
            this.lnkS4taDescargar.NavigateUrl = "~/downloading.aspx?file=" + fileSuspension4ta;
            this.lnkS4taDescargar.ToolTip = fileSuspension4ta;
            this.lnkS4taDescargar.Visible = true;
            this.lblS4taDescarga.Visible = true;

            // Cuentas bancarias del consultor
            for (int i = 0; i < this.gvwCuentas.Items.Count; i++)
            {
                DropDownList cbo_banco = (DropDownList)this.gvwCuentas.Items[i].FindControl("cbo_banco");
                DropDownList cbo_moneda = (DropDownList)this.gvwCuentas.Items[i].FindControl("cbo_moneda");
                RadComboBox cbo_tipo_cuenta = (RadComboBox)this.gvwCuentas.Items[i].FindControl("cbo_tipo_cuenta");
                TextBox txt_nro_cuenta = (TextBox)this.gvwCuentas.Items[i].FindControl("txt_nro_cuenta");

                cuentasDeConsultor.Add(new IT.Domain.ConsultorCuenta
                {
                    CodigoItem = i + 1,
                    CodigoBanco = int.Parse(cbo_banco.SelectedValue),
                    CodigoMoneda = int.Parse(cbo_moneda.SelectedValue),
                    TipoCuenta = cbo_tipo_cuenta.Text.ToUpper().Trim(),
                    NroCuenta = txt_nro_cuenta.Text.Trim(),
                    Estado = 1
                });
            }

            cuentasDeConsultor.Add(new IT.Domain.ConsultorCuenta()
            {
                CodigoItem = cuentasDeConsultor.Count + 1,
                CodigoBanco = 0,
                CodigoMoneda = 0,
                Estado = 1
            });

            this.gvwCuentas.DataSource = cuentasDeConsultor;
            this.gvwCuentas.DataBind();

            for (int i = 0; i < this.gvwCuentas.Items.Count; i++)
            {
                DropDownList cbo_banco = (DropDownList)gvwCuentas.Items[i].FindControl("cbo_banco");
                DropDownList cbo_moneda = (DropDownList)gvwCuentas.Items[i].FindControl("cbo_moneda");
                RadComboBox cbo_tipo_cuenta = (RadComboBox)gvwCuentas.Items[i].FindControl("cbo_tipo_cuenta");
                TextBox txt_nro_cuenta = (TextBox)gvwCuentas.Items[i].FindControl("txt_nro_cuenta");

                cbo_tipo_cuenta.Text = cuentasDeConsultor[i].TipoCuenta;
                txt_nro_cuenta.Text = cuentasDeConsultor[i].NroCuenta;

                this.ListarBancos(cbo_banco);

                cbo_banco.SelectedValue = cuentasDeConsultor[i].CodigoBanco.ToString();
                cbo_banco.ValidationGroup = (cuentasDeConsultor[i].CodigoItem - 1).ToString();

                this.ListarMonedas(cbo_moneda);

                cbo_moneda.SelectedValue = cuentasDeConsultor[i].CodigoMoneda.ToString();
                cbo_moneda.ValidationGroup = (cuentasDeConsultor[i].CodigoItem - 1).ToString();

                this.ListarTipoCuentas(cbo_tipo_cuenta);
            }
        }
        catch (Exception ex) { }
    }
}