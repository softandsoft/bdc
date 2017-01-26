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

public partial class WebConsultantInd : System.Web.UI.Page
{
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

    IList<TConsultorEducacion> oTListaConEducacion;

    IList<Catalogo> oListaSexo;
    IList<Pais> oListaGentilicio;
    IList<Ciudad> oListaCiudad;

    IList<NivelAcademico> oListaNivel;
    IList<DescripcionAcademica> oListaDescripcion;
    IList<Catalogo> oListaDuracion;

    IList<Lenguaje> oListaIdioma;
    IList<Catalogo> oListaHablado;

    IList<Donante> oListaDonante;
    IList<DonantePlantilla> oListaPlantilla;

    IList<Pais> oListaPais;

    IList<ConocimientoGeneral> oListaCGeneral;
    IList<ConocimientoEspecifico> oListaCEspecifico;
    IList<Idioma> oListaLenguaje;

    Helper oHelper = new Helper();

    int nCodigo;

    Persona oPersona;
    Consultor oConsultor;
    Usuario oUsuario;
    string cCarpetaContenido = "";
    String cRutaContenido = "";
    Diccionario oHelperDiccionario;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            oHelperDiccionario = new Diccionario();

            if (Session["Usuario"] != null)
            {
                oTUsuario = (Usuario)Session["Usuario"];

                cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
                cRutaContenido = Server.MapPath(cCarpetaContenido);


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
                gridHabilidades.ClientSettings.Scrolling.AllowScroll = true;
                gridHabilidades.ClientSettings.Scrolling.UseStaticHeaders = true;
                gridReferencia.ClientSettings.Scrolling.AllowScroll = true;
                gridReferencia.ClientSettings.Scrolling.UseStaticHeaders = true;


                if (Request.QueryString["nCode"] != null)
                {
                    try
                    {
                        nCodigo = int.Parse(Request.QueryString["nCode"].ToString().Trim());
                        Ind(nCodigo);
                    }
                    catch (Exception ex)
                    {
                        nCodigo = 0;
                    }

                    if (nCodigo != 0)
                    {
                        usrMenu.cPagina = "~/WebConsultantInd.aspx?nCode=" + nCodigo.ToString() + "&";
                        lblAgregar.Visible = false;
                        lblEditar.Visible = true;
                    }
                    else
                    {
                        usrMenu.cPagina = "~/WebConsultantInd.aspx?";
                        pnlActivo.Visible = false;
                        pnlInactivo.Visible = true;
                        litSinConexion.Text = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "SinCodigo");
                    }
                }
                else
                {

                    usrMenu.cPagina = "~/WebConsultantInd.aspx?";
                    RadTabConsultor.SelectedIndex = 0;

                    lblAgregar.Visible = true;
                    lblEditar.Visible = false;
                }


            }
            else
            {

                pnlActivo.Visible = false;
                pnlInactivo.Visible = true;
                litSinConexion.Text = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "SinConexion");
            }
        }
    }

    private void Ind(int nCodigo)
    {
        cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
        cRutaContenido = Server.MapPath(cCarpetaContenido);
        oHelperDiccionario = new Diccionario();

        if (Session["Usuario"] != null)
        {
            oTUsuario = (Usuario)Session["Usuario"];

            oConsultorSer = new ConsultorService();
            oConsultor = oConsultorSer.ConsultorBusquedaInd(nCodigo, oTUsuario.oIdioma.cod_idi_in);

            if (oConsultor != null)
            {
                if (oConsultor.cod_per_in != 0)
                {
                    lblActualizacion.Visible = true;
                    lblCodigo.Text = oConsultor.cod_per_in.ToString();
                    nCodigo = oConsultor.cod_per_in;
                    lblCTipo.Text = oConsultor.Tipo;
                    lblActualizacion.Text = oHelperDiccionario.DevolverMensaje(Thread.CurrentThread.CurrentCulture.Name, "ConsultorUltimaActualizacion", oConsultor.fec_act_con_dt.ToShortDateString().Trim());

                    lblCApellido.Text = oConsultor.ape_per_vc;
                    lblCNombre.Text = oConsultor.nom_per_vc;

                    if (oConsultor.fec_nac_con_dt != null)
                    {
                        lblCFecha.Text = oConsultor.fec_nac_con_dt.Value.ToShortDateString();
                    }
                    lblCSexo.Text = oConsultor.Sexo;

                    lblCDireccion.Text = oConsultor.dir_con_vc + "&nbsp;";
                    lblCPais.Text = oConsultor.oPais.nom_pais_vc;
                    lblCIdioma.Text = oConsultor.oIdioma.nom_idi_vc;

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



                    lblCCiudad.Text = oConsultor.ciu_con_vc;

                    this.lblCRelationShip.Text = oConsultor.RelationshipNombre;
                    this.lblCComentario.Text = oConsultor.com_con_vc.Trim().Replace("\n", "<br/>");

                    lblCTelefono1.Text = oConsultor.tel1_con_vc;
                    lblCTelefono2.Text = oConsultor.tel2_con_vc;
                    lblCLinked.Text = oConsultor.link_con_vc;
                    lblCCorreo.Text = oConsultor.email_per_vc;
                    lblCBiografia.Text = oConsultor.bio_con_vc.Trim().Replace("\n", "<br/>") + "&nbsp;";


                    chkCertifico.Checked = oConsultor.cer_exp_con_bo;

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

                    rbExperiencia1.Enabled = false;
                    rbExperiencia2.Enabled = false;
                    rbExperiencia3.Enabled = false;
                    rbExperiencia4.Enabled = false;

                    if (oConsultor.tip_usu_in == 1)
                    {
                        pnlDA.Visible = true;
                        pnlContrasenia.Visible = false;
                        lblCDA.Text = oConsultor.nom_usu_vc.Trim();
                    }
                    else if (oConsultor.tip_usu_in == 2)
                    {
                        pnlDA.Visible = false;
                        pnlContrasenia.Visible = true;
                    }

                    if (oConsultor.Moneda.Trim().Length != 0)
                    {
                        lblCMoneda.Text = oConsultor.Moneda.Trim() + " " + oConsultor.pag_con_dc.ToString().Trim();
                    }

                    this.txtFechaActualizacion.Text = oConsultor.fec_act_con_dt.ToShortDateString();

                    //lblMCompania.Text = oConsultor.com_con_vc;
                    //lblMCompaniaContacto.Text = oConsultor.con_com_con_vc;
                    //lblMCompaniaTelefono.Text = oConsultor.tel_com_con_vc;
                    //lblMCompaniaCorreo.Text = oConsultor.cor_com_con_vc;
                    //lblMCompaniaPuesto.Text = oConsultor.pue_com_con_vc;
                    //lblMCompaniaPais.Text = oConsultor.oPaisCompania.nom_pais_vc;
                    //cboCompaniaPais.SelectedValue = oConsultor.com_cod_pais_in.ToString().Trim();

                    //detalles
                    chkCertifico.Enabled = false;
                    ListarDonantes(oTUsuario.oIdioma.cod_idi_in);
                    ListarConocimiento(oTUsuario.oIdioma.cod_idi_in);
                    oListaConDonante = new List<ConsultorDonante>();
                    oListaConDonante = oConsultorSer.DonanteListar(nCodigo);

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
                    lstDonante.Enabled = false;

                    oListaConNacion = new List<ConsultorNacionalidad>();
                    oListaConNacion = oConsultorSer.NacionalidadInd(nCodigo, oTUsuario.oIdioma.cod_idi_in);

                    if (oListaConNacion.Count != 0)
                    {
                        if (oListaConNacion.Count == 1)
                        {
                            lblCNacionalidad1.Text = oListaConNacion[0].nom_pais_vc.ToString();
                        }

                        if (oListaConNacion.Count == 2)
                        {
                            lblCNacionalidad1.Text = oListaConNacion[0].nom_pais_vc.ToString();
                            lblCNacionalidad2.Text = oListaConNacion[1].nom_pais_vc.ToString();
                        }
                    }


                    oListaConEducacion = new List<ConsultorEducacion>();
                    oListaConEducacion = oConsultorSer.EducacionInd(nCodigo, oTUsuario.oIdioma.cod_idi_in);

                    for (int i = 0; i < oListaConEducacion.Count; i++)
                    {
                        oListaConEducacion[i].Nro = i + 1;
                    }

                    gridEduacion.DataSource = oListaConEducacion;
                    gridEduacion.DataBind();

                    for (int i = 0; i < gridEduacion.Items.Count; i++)
                    {
                        Label lblDescarga = (Label)gridEduacion.Items[i].FindControl("lblDescarga");
                        HyperLink lnkDescargar = (HyperLink)gridEduacion.Items[i].FindControl("lnkDescargar");

                        if (lblDescarga.Text.Trim().Length != 0)
                        {
                            lnkDescargar.Visible = true;
                        }
                    }

                    /* conocimientos */

                    oListaConConocimiento = new List<ConsultorConocimiento>();
                    oListaConConocimiento = oConsultorSer.ConocimientoListar(nCodigo);

                    int nCodigoConocimiento = 0;

                    for (int j = 0; j < gridHabilidades.Items.Count; j++)
                    {
                        RadListBox lstTecnico = (RadListBox)gridHabilidades.Items[j].FindControl("lstTecnico");

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
                        lstTecnico.Enabled = false;
                    }

                    oListaConLenguaje = new List<ConsultorLenguaje>();
                    oListaConLenguaje = oConsultorSer.LenguajeInd(nCodigo, oTUsuario.oIdioma.cod_idi_in);

                    for (int i = 0; i < oListaConLenguaje.Count; i++)
                    {
                        oListaConLenguaje[i].Nro = i + 1;
                    }

                    gridLenguaje.DataSource = oListaConLenguaje;
                    gridLenguaje.DataBind();


                    // pais

                    oListaConPais = new List<ConsultorPais>();
                    oListaConPais = oConsultorSer.PaisInd(nCodigo, oTUsuario.oIdioma.cod_idi_in);

                    for (int i = 0; i < oListaConPais.Count; i++)
                    {
                        oListaConPais[i].Nro = i + 1;
                    }

                    gridPais.DataSource = oListaConPais;
                    gridPais.DataBind();

                    //dcumentos

                    oListaConCV = new List<ConsultorDocumento>();
                    oListaConCV = oConsultorSer.DocumentoListar(nCodigo);

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
                            lnkDescargar.Visible = true;
                        }
                    }

                    IList<ConsultorReferencia> oListaCReferencia = new List<ConsultorReferencia>();
                    oListaCReferencia = oConsultorSer.ReferenciaInd(nCodigo, oTUsuario.oIdioma.cod_idi_in);


                    this.gridReferencia.DataSource = oListaCReferencia;
                    this.gridReferencia.DataBind();

                    // El feedback del consultor solo se muestra para lo usuarios internos.
                    if (oTUsuario.tip_usu_in.Equals(Constantes.USUARIOINTERNO))
                        this.gridReferencia.Columns.FindByUniqueName("temFeedback").Visible = true;
                    else
                        this.gridReferencia.Columns.FindByUniqueName("temFeedback").Visible = false;
                }

            }
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

    void ListarConocimiento(int cod_idi_in)
    {

        oListaCGeneral = new List<ConocimientoGeneral>();
        oAdministracionSer = new AdministracionService();
        oListaCGeneral = oAdministracionSer.ConocimientoGeneralIdiomaListar(cod_idi_in);
        oListaCEspecifico = oAdministracionSer.ConocimientoEspecificoTodoListar(cod_idi_in);


        gridHabilidades.DataSource = oListaCGeneral;
        gridHabilidades.DataBind();
        int nCodigoEspecifico = 0;
        for (int i = 0; i < gridHabilidades.Items.Count; i++)
        {
            RadListBox lstTecnico = (RadListBox)gridHabilidades.Items[i].FindControl("lstTecnico");
            Label lblCodigo = (Label)gridHabilidades.Items[i].FindControl("lblCodigo");
            nCodigoEspecifico = int.Parse(lblCodigo.Text.Trim());
            foreach (ConocimientoEspecifico ent in oListaCEspecifico)
            {
                if (ent.oGeneral.cod_congen_in == nCodigoEspecifico)
                {
                    lstTecnico.Items.Add(new RadListBoxItem(ent.nom_conesp_vc, ent.cod_conesp_in.ToString()));

                }
            }
        }
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


}