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
using LDAPService;

public partial class WebConsultantAgregar : System.Web.UI.Page
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
    // IList<Ciudad> oListaCiudad;

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
    LDAPServiceClient oLDAPSer;
    UsuarioLDAP[] oListaDAService;
    IList<Catalogo> oListaMes;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["Usuario"] != null)
            {
                oTUsuario = (Usuario)Session["Usuario"];

                if (oHelper.DevuelvePermiso("mnuConsultor", oTUsuario.oListaPermiso) == true)
                {
                    cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
                    cRutaContenido = Server.MapPath(cCarpetaContenido);

                    String cBDLdap = "";
                    cBDLdap = ConfigurationManager.AppSettings["BDLdap"];

                    if (cBDLdap.Trim().Equals("1"))
                    {
                        lnkImportar.Visible = true;
                    }
                    else
                    {
                        lnkImportar.Visible = false;
                    }

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

                    gridUsuario.ClientSettings.Selecting.AllowRowSelect = true;

                    ListarIdioma();
                    //ListarFecha(oTUsuario.oIdioma.cod_idi_in);
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
                            usrMenu.cPagina = "~/WebConsultantAdd.aspx?nCode=" + nCodigo.ToString() + "&";
                            lblAgregar.Visible = false;
                            lblEditar.Visible = true;
                        }
                        else
                        {
                            usrMenu.cPagina = "~/WebConsultantAdd.aspx?";
                            pnlActivo.Visible = false;
                            pnlInactivo.Visible = true;
                            litSinConexion.Text = oHelper.DevuelveSinCodigo();
                        }

                    }
                    else
                    {

                        usrMenu.cPagina = "~/WebConsultantAdd.aspx?";
                        RadTabConsultor.SelectedIndex = 0;

                        ListarDatos(oTUsuario.oIdioma.cod_idi_in);
                        ListarUsuarioDA();
                        ListarAcademico(oTUsuario.oIdioma.cod_idi_in);
                        ListarConocimiento(oTUsuario.oIdioma.cod_idi_in);

                        ListarIdioma(oTUsuario.oIdioma.cod_idi_in);
                        ListarDonantes(oTUsuario.oIdioma.cod_idi_in);
                        ListarPaises(oTUsuario.oIdioma.cod_idi_in);
                        ListarCV(oTUsuario.oIdioma.cod_idi_in);
                        ListarPlantilla(oTUsuario.oIdioma.cod_idi_in);
                        lblAgregar.Visible = true;
                        lblEditar.Visible = false;
                        txtContra.Enabled = true;
                        txtContraRep.Enabled = true;
                        cboPais.SelectedValue = oTUsuario.oPais.cod_pais_in.ToString();

                    }


                }
                else
                {
                    pnlActivo.Visible = false;
                    pnlInactivo.Visible = true;
                    litSinConexion.Text = oHelper.DevuelveSinPermiso();
                }
            }
            else
            {

                pnlActivo.Visible = false;
                pnlInactivo.Visible = true;
                litSinConexion.Text = oHelper.DevuelveSinConexion();
            }
        }
    }

    //void ListarFecha(int cod_idi_in)
    //{
    //    String cSeleccione = "";

    //    if (cod_idi_in == 1)
    //    {

    //        cSeleccione = "Month";

    //    }
    //    else
    //    {

    //        cSeleccione = "Mes";

    //    }
    //    oAdministracionSer = new AdministracionService();
    //    oListaMes = new List<Catalogo>();
    //    oListaMes = oAdministracionSer.CatalogoBuscar(2005, cod_idi_in);

    //    if (oListaMes.Count != 0)
    //    {
    //        oListaMes.Insert(0, new Catalogo() { val_cat_in = 0, nom_cat_vc = cSeleccione });
    //    }
    //    else
    //    {
    //        oListaMes.Add(new Catalogo() { val_cat_in = 0, nom_cat_vc = cSeleccione });
    //    }
    //    cboMes.DataSource = oListaMes;
    //    cboMes.DataTextField = "nom_cat_vc";
    //    cboMes.DataValueField = "val_cat_in";
    //    cboMes.DataBind();

    //    txtAnio.MaxValue = DateTime.Now.Year - 1;
    //    txtAnio.MinValue = 1910;
    //}

    String FormatoFecha(DateTime dt)
    {

        String cFecha = "";

        cFecha = (dt.Day.ToString().Trim().Length == 1 ? "0" : "") +
                dt.Day.ToString().Trim() + " / " +
                (dt.Month.ToString().Trim().Length == 1 ? "0" : "") +
                dt.Month.ToString().Trim() + " / " + dt.Year.ToString().Trim();

        return cFecha;
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

            actualizaFoto();

            txtContra.Attributes["value"] = txtContra.Text;
            txtContraRep.Attributes["value"] = txtContraRep.Text;

            oTUsuario = (Usuario)Session["Usuario"];
            oListaConCV = new List<ConsultorDocumento>();
            oTListaConEducacion = new List<TConsultorEducacion>();
            cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
            cRutaContenido = Server.MapPath(cCarpetaContenido);
            UploadedFile oFile;
            String cImagen = "";
            DateTime dfecha;

            for (int i = 0; i < gridCV.Items.Count; i++)
            {
                Label lblNro = (Label)gridCV.Items[i].FindControl("lblNro");
                Label lblCodigo = (Label)gridCV.Items[i].FindControl("lblCodigo");
                TextBox txtTitulo = (TextBox)gridCV.Items[i].FindControl("txtTitulo");
                Label lblDescarga = (Label)gridCV.Items[i].FindControl("lblDescarga");
                Label lblDescargaN = (Label)gridCV.Items[i].FindControl("lblDescargaN");

                RadAsyncUpload RadUpCV = (RadAsyncUpload)gridCV.Items[i].FindControl("RadUpCV");
                HyperLink lnkDescargar = (HyperLink)gridCV.Items[i].FindControl("lnkDescargar");

                if (RadUpCV.UploadedFiles.Count > 0)
                {


                    oFile = RadUpCV.UploadedFiles[0];
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

                if (lblDescarga.Text.Trim().Length != 0)
                {
                    lnkDescargar.NavigateUrl = "~/downloading.aspx?file=" + lblDescarga.Text.Trim();
                    lnkDescargar.Visible = true;
                    lnkDescargar.ToolTip = lblDescargaN.Text.Trim();
                }

            }



            //educacion
            oTListaConEducacion = new List<TConsultorEducacion>();
            cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
            cRutaContenido = Server.MapPath(cCarpetaContenido);
            oFile = null;
            cImagen = "";
            dfecha = new DateTime();

            for (int i = 0; i < gridEduacion.Items.Count; i++)
            {
                Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");
                Label lblCodigo = (Label)gridEduacion.Items[i].FindControl("lblCodigo");

                Label lblDescarga = (Label)gridEduacion.Items[i].FindControl("lblDescarga");
                HyperLink lnkDescargar = (HyperLink)gridEduacion.Items[i].FindControl("lnkDescargar");
                Label lblDescargaN = (Label)gridEduacion.Items[i].FindControl("lblDescargaN");
                RadAsyncUpload RadUpCertificado = (RadAsyncUpload)gridEduacion.Items[i].FindControl("RadUpCertificado");

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

                    lblDescarga.Text = cImagen.Trim();
                    lblDescargaN.Text = cImagen.Trim();
                }

                if (lblDescarga.Text.Trim().Length != 0)
                {
                    lnkDescargar.NavigateUrl = "~/downloading.aspx?file=" + lblDescargaN.Text.Trim();
                    lnkDescargar.Visible = true;
                    lnkDescargar.ToolTip = lblDescargaN.Text.Trim();
                }

            }


        }
    }

    void actualizarEducacion()
    {

        if (Session["Usuario"] != null)
        {
            actualizaFoto();
            txtContra.Attributes["value"] = txtContra.Text;
            txtContraRep.Attributes["value"] = txtContraRep.Text;

            oTUsuario = (Usuario)Session["Usuario"];

            cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
            cRutaContenido = Server.MapPath(cCarpetaContenido);
            UploadedFile oFile;
            String cImagen = "";
            DateTime dfecha;
            String cDescarga = "";

            //educacion
            oTListaConEducacion = new List<TConsultorEducacion>();
            cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
            cRutaContenido = Server.MapPath(cCarpetaContenido);
            oFile = null;
            cImagen = "";
            dfecha = new DateTime();

            for (int i = 0; i < gridEduacion.Items.Count; i++)
            {
                Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");
                Label lblCodigo = (Label)gridEduacion.Items[i].FindControl("lblCodigo");

                Label lblDescarga = (Label)gridEduacion.Items[i].FindControl("lblDescarga");
                Label lblDescargaN = (Label)gridEduacion.Items[i].FindControl("lblDescargaN");
                HyperLink lnkDescargar = (HyperLink)gridEduacion.Items[i].FindControl("lnkDescargar");

                RadAsyncUpload RadUpCertificado = (RadAsyncUpload)gridEduacion.Items[i].FindControl("RadUpCertificado");

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

                    lblDescarga.Text = cImagen.Trim();
                    lblDescargaN.Text = cImagen.Trim();

                    if (lblDescarga.Text.Trim().Length != 0)
                    {
                        lnkDescargar.NavigateUrl = "~/downloading.aspx?file=" + lblDescarga.Text.Trim();
                        lnkDescargar.ToolTip = lblDescarga.Text.Trim();
                        lnkDescargar.Visible = true;

                        if (lblDescarga.Text.Trim().Length > 8)
                        {
                            cDescarga = lblDescarga.Text.Trim().Substring(0, 8) + "...";
                            lblDescarga.Text = cDescarga;
                        }
                    }

                }
            }
        }
    }

    void actualizarCV()
    {

        if (Session["Usuario"] != null)
        {
            actualizaFoto();
            txtContra.Attributes["value"] = txtContra.Text;
            txtContraRep.Attributes["value"] = txtContraRep.Text;

            oTUsuario = (Usuario)Session["Usuario"];
            oListaConCV = new List<ConsultorDocumento>();
            oTListaConEducacion = new List<TConsultorEducacion>();
            cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
            cRutaContenido = Server.MapPath(cCarpetaContenido);
            UploadedFile oFile;
            String cImagen = "", cDescarga = "";
            DateTime dfecha;

            for (int i = 0; i < gridCV.Items.Count; i++)
            {
                Label lblNro = (Label)gridCV.Items[i].FindControl("lblNro");
                Label lblCodigo = (Label)gridCV.Items[i].FindControl("lblCodigo");
                TextBox txtTitulo = (TextBox)gridCV.Items[i].FindControl("txtTitulo");
                Label lblDescarga = (Label)gridCV.Items[i].FindControl("lblDescarga");
                Label lblDescargaN = (Label)gridCV.Items[i].FindControl("lblDescargaN");

                RadAsyncUpload RadUpCV = (RadAsyncUpload)gridCV.Items[i].FindControl("RadUpCV");
                HyperLink lnkDescargar = (HyperLink)gridCV.Items[i].FindControl("lnkDescargar");

                if (RadUpCV.UploadedFiles.Count > 0)
                {

                    oFile = RadUpCV.UploadedFiles[0];
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

                        cImagen = randomNumber.ToString() +
                              "_CV_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                              dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                              dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim();
                        oFile.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                    }
                    lblDescargaN.Text = cImagen.Trim();
                    lblDescarga.Text = cImagen.Trim();

                    if (lblDescarga.Text.Trim().Length != 0)
                    {
                        lnkDescargar.NavigateUrl = "~/downloading.aspx?file=" + lblDescarga.Text.Trim();
                        lnkDescargar.ToolTip = lblDescarga.Text.Trim();
                        lnkDescargar.Visible = true;

                        if (lblDescarga.Text.Trim().Length > 15)
                        {
                            cDescarga = lblDescarga.Text.Trim().Substring(0, 15) + "...";
                            lblDescarga.Text = cDescarga;
                        }

                    }
                }

            }


        }
    }

    protected void rbInterno_CheckedChanged(object sender, EventArgs e)
    {
        MostrarTipo();
        actualizarDatos();
    }

    protected void rbExterno_CheckedChanged(object sender, EventArgs e)
    {
        MostrarTipo();
        actualizarDatos();
    }

    protected void cboPais_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Session["Usuario"] != null)
        //{
        //    oTUsuario = (Usuario)Session["Usuario"];
        //    actualizarDatos();
        //    int nPais = 0;
        //    nPais = int.Parse(cboPais.SelectedValue.Trim());

        //    oListaCiudad = new List<Ciudad>();
        //    oAdministracionSer = new AdministracionService();
        //    oListaCiudad = oAdministracionSer.CiudadPaisListar(nPais,oTUsuario.oIdioma.cod_idi_in);

        //    String cSeleccione = "";

        //    if (oTUsuario.oIdioma.cod_idi_in == 1)
        //    {
        //        cSeleccione = "Select";
        //    }
        //    else
        //    {

        //        cSeleccione = "Seleccione";
        //    }

        //    if (oListaCiudad.Count == 0)
        //    {

        //        oListaCiudad.Add(new Ciudad() { nom_ciu_vc = cSeleccione });

        //    }
        //    else
        //    {

        //        oListaCiudad.Insert(0, new Ciudad() { nom_ciu_vc = cSeleccione });

        //    }

        //    cboCiudad.DataSource = oListaCiudad;
        //    cboCiudad.DataValueField = "cod_ciu_in";
        //    cboCiudad.DataTextField = "nom_ciu_vc";
        //    cboCiudad.DataBind();
        //}
    }

    private void MostrarTipo()
    {

        if (rbInterno.Checked == true)
        {
            pnlDA.Visible = true;
            pnlContrasenia.Visible = false;
        }

        if (rbExterno.Checked == true)
        {
            pnlDA.Visible = false;
            pnlContrasenia.Visible = true;

            nCodigo = int.Parse(lblCodigo.Text);

            if (nCodigo != 0)
            {
                chkContra.Visible = true;
            }
            else
            {
                chkContra.Visible = false;
            }
        }
    }

    LdapService oLdapService;

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
        //String cLdap = ConfigurationManager.AppSettings["servicioldap"];

        //IList<UsuarioLDAP> oListaLdap = new List<UsuarioLDAP>();

        //try
        //{
        //    if (cLdap.Trim().Equals("1"))
        //    {
        //        oLDAPSer = new LDAPServiceClient();
        //        oListaDAService = oLDAPSer.ListarTodos();
        //        oListaLdap = oListaDAService.OrderBy(x => x.cn).ToList();
        //        //oListaDAService = oListaDAService.OrderBy(x => x.cn).ToList();    
        //        oLDAPSer = null;
        //    }
        //}
        //catch (Exception ex)
        //{

        //}

        //cboDA.DataSource = oListaLdap;
        //cboDA.DataValueField = "user";
        //cboDA.DataTextField = "cn";
        //cboDA.DataBind();

        String cLdap = "", cBDLdap = "";
        cLdap = ConfigurationManager.AppSettings["servicioldap"];
        cBDLdap = ConfigurationManager.AppSettings["BDLdap"];

        if (cBDLdap.Trim().Equals("1"))
        {
            IList<Usuario> oListaBDLdap = new List<Usuario>();
            oLdapService = new LdapService();

            if (cBDLdap.Trim().Equals("1"))
            {
                oListaBDLdap = oLdapService.ListarUsuarioLdap();
            }

            cboDA.DataSource = oListaBDLdap;
            cboDA.DataValueField = "con_usu_vc";
            cboDA.DataTextField = "com_usu_vc";
            cboDA.DataBind();


        }
        else
        {

            IList<UsuarioLDAP> oListaLdap = new List<UsuarioLDAP>();

            if (cLdap.Trim().Equals("1"))
            {

                try
                {

                    oLDAPSer = new LDAPServiceClient();
                    oListaDAService = oLDAPSer.ListarTodos();
                    oListaLdap = oListaDAService.OrderBy(x => x.cn).ToList();
                    //oListaDAService = oListaDAService.OrderBy(x => x.cn).ToList();    
                    oLDAPSer = null;

                }
                catch (Exception ex)
                {

                }

            }

            cboDA.DataSource = oListaLdap;
            cboDA.DataValueField = "user";
            cboDA.DataTextField = "cn";
            cboDA.DataBind();
        }

    }

    private void Ind(int nCodigo)
    {
        cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
        cRutaContenido = Server.MapPath(cCarpetaContenido);

        lblFotoDescarga.Text = "";
        lblFotoDescarga.Visible = false;
        lnkFDescargar.Visible = false;
        lblFDescarga.Text = "";


        String cMensajeOk = "";
        if (Session["Usuario"] != null)
        {
            if (Session["cMensajeOk"] != null)
            {
                cMensajeOk = Session["cMensajeOk"].ToString().Trim();
                pnlOk.Visible = true;
                litOk.Text = cMensajeOk.Trim();
                Session.Remove("cMensajeOk");
            }

            oTUsuario = (Usuario)Session["Usuario"];

            ListarDatos(oTUsuario.oIdioma.cod_idi_in);

            oConsultorSer = new ConsultorService();
            oConsultor = oConsultorSer.ConsultorInd(nCodigo);

            if (oConsultor != null)
            {
                if (oConsultor.cod_per_in != 0)
                {

                    lblCodigo.Text = oConsultor.cod_per_in.ToString();
                    nCodigo = oConsultor.cod_per_in;
                    lblTipoConsultor.Text = oConsultor.tip_con_in.ToString();
                    rbInterno.Checked = false;
                    rbExterno.Checked = false;
                    if (oConsultor.tip_con_in == 1)
                    {
                        rbInterno.Checked = true;

                    }
                    else
                    {
                        rbExterno.Checked = true;
                    }

                    MostrarTipo();
                    ListarUsuarioDA();
                    rbInterno.Enabled = false;
                    rbExterno.Enabled = false;

                    txtApellido.Text = oConsultor.ape_per_vc;
                    txtNombre.Text = oConsultor.nom_per_vc;
                    //radDtFecha.SelectedDate = oConsultor.fec_nac_con_dt;
                    cboSexo.SelectedValue = oConsultor.sex_con_in.ToString();
                    lblCodigoUsuario.Text = oConsultor.cod_usu_in.ToString();
                    txtDireccion.Text = oConsultor.dir_con_vc;
                    cboPais.SelectedValue = oConsultor.oPais.cod_pais_in.ToString();
                    cboIdioma.SelectedValue = oConsultor.oIdioma.cod_idi_in.ToString();
                    txtContra.Enabled = false;
                    txtContraRep.Enabled = false;
                    chkContra.Checked = false;

                    //txtAnio.Text = oConsultor.fec_nac_con_dt.Year.ToString();
                    //txtDia.Text = oConsultor.fec_nac_con_dt.Day.ToString();
                    //cboMes.SelectedValue = oConsultor.fec_nac_con_dt.Month.ToString();
                    if (oConsultor.fec_nac_con_dt != null)
                    {
                        txtFechaNacimiento.TextWithLiterals = FormatoFecha(oConsultor.fec_nac_con_dt.Value);
                    }
                    txtCiudad.Text = oConsultor.ciu_con_vc.ToString();

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
                    //oListaCiudad = new List<Ciudad>();
                    //oAdministracionSer = new AdministracionService();
                    //oListaCiudad = oAdministracionSer.CiudadPaisListar(oConsultor.oPais.cod_pais_in, oTUsuario.oIdioma.cod_idi_in);

                    String cSeleccione = "";

                    if (oTUsuario.oIdioma.cod_idi_in == 1)
                    {
                        cSeleccione = "Select";
                    }
                    else
                    {

                        cSeleccione = "Seleccione";
                    }

                    //if (oListaCiudad.Count == 0)
                    //{

                    //    oListaCiudad.Add(new Ciudad() { nom_ciu_vc = cSeleccione });

                    //}
                    //else
                    //{

                    //    oListaCiudad.Insert(0, new Ciudad() { nom_ciu_vc = cSeleccione });

                    //}

                    //cboCiudad.DataSource = oListaCiudad;
                    //cboCiudad.DataValueField = "cod_ciu_in";
                    //cboCiudad.DataTextField = "nom_ciu_vc";
                    //cboCiudad.DataBind();
                    //cboCiudad.SelectedValue = oConsultor.oCiudad.cod_ciu_in.ToString();
                    txtTelefono1.Text = oConsultor.tel1_con_vc;
                    txtTelefono2.Text = oConsultor.tel2_con_vc;
                    txtLinked.Text = oConsultor.link_con_vc;
                    txtCorreo.Text = oConsultor.email_per_vc;

                    //chkCertifico.Checked = oConsultor.cer_exp_con_bo;
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
                        cboDA.SelectedValue = oConsultor.nom_usu_vc.Trim();
                    }
                    else if (oConsultor.tip_usu_in == 2)
                    {
                        pnlDA.Visible = false;
                        pnlContrasenia.Visible = true;
                        //txtContra.Text = oConsultor.con_usu_vc;
                    }

                    //detalles

                    ListarDonantes(oTUsuario.oIdioma.cod_idi_in);
                    ListarConocimiento(oTUsuario.oIdioma.cod_idi_in);
                    ListarPlantilla(oTUsuario.oIdioma.cod_idi_in);
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


                    oListaConNacion = new List<ConsultorNacionalidad>();
                    oListaConNacion = oConsultorSer.NacionalidadListar(nCodigo);

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
                    oListaConEducacion = oConsultorSer.EducacionListar(nCodigo);

                    for (int i = 0; i < oListaConEducacion.Count; i++)
                    {
                        oListaConEducacion[i].Nro = i + 1;
                    }

                    gridEduacion.DataSource = oListaConEducacion;
                    gridEduacion.DataBind();



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

                    //int nNivel = 0;
                    String cDescarga = "";
                    for (int i = 0; i < gridEduacion.Items.Count; i++)
                    {
                        DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
                        DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");

                        //DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");

                        Label lblNivel = (Label)gridEduacion.Items[i].FindControl("lblNivel");
                        //Label lblDescripcion = (Label)gridEduacion.Items[i].FindControl("lblDescripcion");
                        Label lblDuracion = (Label)gridEduacion.Items[i].FindControl("lblDuracion");

                        Label lblDescarga = (Label)gridEduacion.Items[i].FindControl("lblDescarga");
                        HyperLink lnkDescargar = (HyperLink)gridEduacion.Items[i].FindControl("lnkDescargar");


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

                        cboNivel.DataSource = oListaNivel;
                        cboNivel.DataValueField = "cod_nivaca_in";
                        cboNivel.DataTextField = "nom_nivacaidi_vc";
                        cboNivel.DataBind();
                        cboNivel.SelectedValue = lblNivel.Text.Trim();

                        //nNivel = int.Parse(lblNivel.Text.Trim());
                        //oAdministracionSer = new AdministracionService();
                        //oListaDescripcion = new List<DescripcionAcademica>();
                        //oListaDescripcion = oAdministracionSer.DescripcionAcademicaIdiomaListar(nNivel, oTUsuario.oIdioma.cod_idi_in);

                        //if (oListaDescripcion != null)
                        //{
                        //    if (oListaDescripcion.Count != 0)
                        //    {
                        //        oListaDescripcion.Insert(0, new DescripcionAcademica() { nom_desaca_vc = cSeleccione });
                        //    }
                        //    else
                        //    {
                        //        oListaDescripcion.Add(new DescripcionAcademica() { nom_desaca_vc = cSeleccione });
                        //    }
                        //}
                        //else
                        //{
                        //    oListaDescripcion.Add(new DescripcionAcademica() { nom_desaca_vc = cSeleccione });
                        //}

                        //cboDescripcion.DataSource = oListaDescripcion;
                        //cboDescripcion.DataValueField = "cod_desaca_in";
                        //cboDescripcion.DataTextField = "nom_desaca_vc";
                        //cboDescripcion.DataBind();
                        //cboDescripcion.SelectedValue = lblDescripcion.Text.Trim();

                        cboDuracion.DataSource = oListaDuracion;
                        cboDuracion.DataValueField = "val_cat_in";
                        cboDuracion.DataTextField = "nom_cat_vc";
                        cboDuracion.DataBind();
                        cboDuracion.SelectedValue = lblDuracion.Text.Trim();

                    }

                    /* conocimientos */

                    oListaConConocimiento = new List<ConsultorConocimiento>();
                    oListaConConocimiento = oConsultorSer.ConocimientoListar(nCodigo);

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
                    oListaConLenguaje = oConsultorSer.LenguajeListar(nCodigo);

                    for (int i = 0; i < oListaConLenguaje.Count; i++)
                    {
                        oListaConLenguaje[i].Nro = i + 1;
                    }

                    gridLenguaje.DataSource = oListaConLenguaje;
                    gridLenguaje.DataBind();

                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                    Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                    Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cSeleccione = "Seleccione";
                    }
                    else
                    {
                        cSeleccione = "Select";
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

                    // pais

                    oListaConPais = new List<ConsultorPais>();
                    oListaConPais = oConsultorSer.PaisListar(nCodigo);
                    for (int i = 0; i < oListaConPais.Count; i++)
                    {
                        oListaConPais[i].Nro = i + 1;
                    }

                    gridPais.DataSource = oListaConPais;
                    gridPais.DataBind();

                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                         Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                         Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cSeleccione = "Seleccione";
                    }
                    else
                    {
                        cSeleccione = "Select";
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
                        DropDownList cboT2Pais = (DropDownList)gridPais.Items[i].FindControl("cboPais");

                        cboT2Pais.DataSource = oListaPais;
                        cboT2Pais.DataTextField = "nom_pais_vc";
                        cboT2Pais.DataValueField = "cod_pais_in";
                        cboT2Pais.DataBind();

                        cboT2Pais.SelectedValue = lblPais.Text.Trim();

                    }

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


                        if (lblDescarga.Text.Trim().Length > 15)
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
                }

            }
        }

    }

    void ListarDatos(int cod_idi_in)
    {
        String cSeleccione = "";

        if (cod_idi_in == 1)
        {
            cSeleccione = "Select";
        }
        else
        {

            cSeleccione = "Seleccione";
        }

        oListaPais = new List<Pais>();
        oListaSexo = new List<Catalogo>();
        //oListaCiudad = new List<Ciudad>();

        oAdministracionSer = new AdministracionService();
        oListaPais = oAdministracionSer.PaisIdiomaListar(cod_idi_in);
        oListaSexo = oAdministracionSer.CatalogoBuscar(2003, cod_idi_in);
        oListaPais = (from pais in oListaPais
                      orderby pais.nom_pais_vc
                      select pais).ToList();
        if (oListaPais.Count == 0)
        {

            oListaPais.Add(new Pais() { nom_pais_vc = cSeleccione, gen_pais_vc = cSeleccione });

        }
        else
        {

            oListaPais.Insert(0, new Pais() { nom_pais_vc = cSeleccione, gen_pais_vc = cSeleccione });

        }

        if (oListaSexo.Count == 0)
        {

            oListaSexo.Add(new Catalogo() { nom_cat_vc = cSeleccione });

        }
        else
        {

            oListaSexo.Insert(0, new Catalogo() { nom_cat_vc = cSeleccione });

        }

        //if (oListaCiudad.Count == 0)
        //{

        //    oListaCiudad.Add(new Ciudad() { nom_ciu_vc = cSeleccione });

        //}
        //else
        //{

        //    oListaCiudad.Insert(0, new Ciudad() { nom_ciu_vc = cSeleccione });

        //}


        cboPais.DataSource = oListaPais;
        cboPais.DataTextField = "nom_pais_vc";
        cboPais.DataValueField = "cod_pais_in";
        cboPais.DataBind();

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

        //cboCiudad.DataSource = oListaCiudad;
        //cboCiudad.DataValueField = "cod_ciu_in";
        //cboCiudad.DataTextField = "nom_ciu_vc";
        //cboCiudad.DataBind();

    }

    void ListarAcademico(int cod_idi_in)
    {
        //String cSeleccione = "";

        //if (cod_idi_in == 1)
        //{
        //    cSeleccione = "Select";
        //}
        //else
        //{

        //    cSeleccione = "Seleccione";
        //}

        oListaConEducacion = new List<ConsultorEducacion>();

        //oListaConEducacion.Add(new ConsultorEducacion()
        //{
        //    Nro =  1,
        //    oNivel = new NivelAcademico(),
        //    oDescripcion = new DescripcionAcademica()
        //});
        gridEduacion.DataSource = oListaConEducacion;
        gridEduacion.DataBind();

        //oAdministracionSer = new AdministracionService();
        //oListaNivel = oAdministracionSer.NivelAcademicoIdiomaListar(cod_idi_in);
        //oListaDuracion = oAdministracionSer.CatalogoBuscar(2001, cod_idi_in);

        //oListaDescripcion = new List<DescripcionAcademica>();
        //oListaDescripcion.Add(new DescripcionAcademica() { nom_desaca_vc = cSeleccione });

        //if (oListaNivel.Count == 0)
        //{
        //    oListaNivel.Add(new NivelAcademico() { nom_nivacaidi_vc = cSeleccione });
        //}
        //else {
        //    oListaNivel.Insert(0, new NivelAcademico() { nom_nivacaidi_vc = cSeleccione });

        //}

        //if (oListaDuracion.Count == 0)
        //{
        //    oListaDuracion.Add(new Catalogo() { nom_cat_vc = cSeleccione });
        //}
        //else {
        //    oListaDuracion.Insert(0, new Catalogo() { nom_cat_vc = cSeleccione });
        //}


        //for (int i = 0; i < gridEduacion.Items.Count; i++)
        //{
        //    DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
        //    DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
        //    DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");

        //    Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");

        //    cboNivel.DataSource = oListaNivel;
        //    cboNivel.DataValueField = "cod_nivaca_in";
        //    cboNivel.DataTextField = "nom_nivacaidi_vc";
        //    cboNivel.DataBind();

        //    cboDescripcion.DataSource = oListaDescripcion;
        //    cboDescripcion.DataValueField = "cod_desaca_in";
        //    cboDescripcion.DataTextField = "nom_desaca_vc";
        //    cboDescripcion.DataBind();

        //    cboDuracion.DataSource = oListaDuracion;
        //    cboDuracion.DataValueField = "val_cat_in";
        //    cboDuracion.DataTextField = "nom_cat_vc";
        //    cboDuracion.DataBind();

        //}
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

        //String cSeleccione = "";

        //if (cod_idi_in == 1)
        //{
        //    cSeleccione = "Select";
        //}
        //else
        //{

        //    cSeleccione = "Seleccione";
        //}
        //oListaIdioma = new List<Lenguaje>();
        //oListaHablado = new List<Catalogo>();
        //oAdministracionSer = new AdministracionService();
        //oListaIdioma = oAdministracionSer.LenguajeIdiomaListar(cod_idi_in);
        //oListaHablado = oAdministracionSer.CatalogoBuscar(2002, cod_idi_in);

        //if (oListaIdioma.Count == 0)
        //{
        //    oListaIdioma.Add(new Lenguaje() { nom_len_vc = cSeleccione });
        //}
        //else {
        //    oListaIdioma.Insert(0, new Lenguaje() { nom_len_vc = cSeleccione });        
        //}

        //if (oListaHablado.Count == 0){

        //    oListaHablado.Add(new Catalogo() { nom_cat_vc = cSeleccione });

        //}else {

        //    oListaHablado.Insert(0, new Catalogo() { nom_cat_vc = cSeleccione });

        //}

        oListaConLenguaje = new List<ConsultorLenguaje>();


        //oListaConLenguaje.Add(new ConsultorLenguaje() { Nro=1});

        gridLenguaje.DataSource = oListaConLenguaje;
        gridLenguaje.DataBind();

        //for (int i = 0; i < gridLenguaje.Items.Count; i++)
        //{
        //    DropDownList cboLenguaje = (DropDownList)gridLenguaje.Items[i].FindControl("cboLenguaje");
        //    DropDownList cboHablado = (DropDownList)gridLenguaje.Items[i].FindControl("cboHablado");
        //    DropDownList cboLeido = (DropDownList)gridLenguaje.Items[i].FindControl("cboLeido");
        //    DropDownList cboEscrito = (DropDownList)gridLenguaje.Items[i].FindControl("cboEscrito");

        //    cboLenguaje.DataSource = oListaIdioma;
        //    cboLenguaje.DataValueField = "cod_len_in";
        //    cboLenguaje.DataTextField = "nom_len_vc";
        //    cboLenguaje.DataBind();

        //    cboHablado.DataSource = oListaHablado;
        //    cboHablado.DataValueField = "val_cat_in";
        //    cboHablado.DataTextField = "nom_cat_vc";
        //    cboHablado.DataBind();

        //    cboLeido.DataSource = oListaHablado;
        //    cboLeido.DataValueField = "val_cat_in";
        //    cboLeido.DataTextField = "nom_cat_vc";
        //    cboLeido.DataBind();

        //    cboEscrito.DataSource = oListaHablado;
        //    cboEscrito.DataValueField = "val_cat_in";
        //    cboEscrito.DataTextField = "nom_cat_vc";
        //    cboEscrito.DataBind();

        //}
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
        //oListaConCV.Add(new ConsultorDocumento());
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
            oTListaConEducacion = new List<TConsultorEducacion>();
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
                //DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");
                DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
                RadNumericTextBox txtDuracion = (RadNumericTextBox)gridEduacion.Items[i].FindControl("txtDuracion");
                TextBox txtInstitucion = (TextBox)gridEduacion.Items[i].FindControl("txtInstitucion");
                TextBox txtDescripcion = (TextBox)gridEduacion.Items[i].FindControl("txtDescripcion");

                Label lblDescarga = (Label)gridEduacion.Items[i].FindControl("lblDescarga");
                Label lblDescargaN = (Label)gridEduacion.Items[i].FindControl("lblDescargaN");
                HyperLink lnkDescargar = (HyperLink)gridEduacion.Items[i].FindControl("lnkDescargar");

                RadAsyncUpload RadUpCertificado = (RadAsyncUpload)gridEduacion.Items[i].FindControl("RadUpCertificado");

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

                oTListaConEducacion.Add(new TConsultorEducacion()
                {
                    cod_conedu_in = int.Parse(lblCodigo.Text),
                    Nro = int.Parse(lblNro.Text.Trim()),
                    oNivel = new NivelAcademico()
                    {
                        cod_nivaca_in = int.Parse(cboNivel.SelectedValue.Trim())
                    },
                    des_conedu_vc = txtDescripcion.Text.Trim(),
                    //oDescripcion = new DescripcionAcademica()
                    //{
                    //    cod_desaca_in = int.Parse(cboDescripcion.SelectedValue.Trim())
                    //},
                    tip_dur_conedu_in = int.Parse(cboDuracion.SelectedValue.Trim()),
                    can_dur_conedu_in = int.Parse(txtDuracion.Text),
                    ins_conedu_vc = txtInstitucion.Text,
                    adj_conedu_vc = lblDescargaN.Text.Trim()
                });
            }


            oTListaConEducacion.Add(new TConsultorEducacion()
            {
                Nro = oTListaConEducacion.Count + 1,
                oNivel = new NivelAcademico(),
                oDescripcion = new DescripcionAcademica()
            });

            gridEduacion.DataSource = oTListaConEducacion;
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
                //DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");

                Label lblNivel = (Label)gridEduacion.Items[i].FindControl("lblNivel");
                Label lblDescripcion = (Label)gridEduacion.Items[i].FindControl("lblDescripcion");
                Label lblDuracion = (Label)gridEduacion.Items[i].FindControl("lblDuracion");
                RadAsyncUpload RadUpCertificado = (RadAsyncUpload)gridEduacion.Items[i].FindControl("RadUpCertificado");

                Label lblDescarga = (Label)gridEduacion.Items[i].FindControl("lblDescarga");

                HyperLink lnkDescargar = (HyperLink)gridEduacion.Items[i].FindControl("lnkDescargar");


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

                //cboDescripcion.DataSource = oListaDescripcion;
                //cboDescripcion.DataValueField = "cod_desaca_in";
                //cboDescripcion.DataTextField = "nom_desaca_vc";
                //cboDescripcion.DataBind();
                //cboDescripcion.SelectedValue = lblDescripcion.Text.Trim();

                cboDuracion.DataSource = oListaDuracion;
                cboDuracion.DataValueField = "val_cat_in";
                cboDuracion.DataTextField = "nom_cat_vc";
                cboDuracion.DataBind();
                cboDuracion.SelectedValue = lblDuracion.Text.Trim();


            }
        }
    }

    protected void gridEduacion_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        //if (e.Item is GridDataItem)
        //{
        //    GridDataItem dataItem = (GridDataItem)e.Item;

        //    DropDownList cboNivel = (DropDownList)dataItem.FindControl("cboNivel");
        //    cboNivel.AutoPostBack = true;
        //    cboNivel.SelectedIndexChanged += new EventHandler(cboNivel_SelectedIndexChanged);

        //} 
    }

    protected void gridEduacion_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Eliminar")
        {
            int nIdioma = 0;

            if (Session["Usuario"] != null)
            {
                actualizarCV();
                UploadedFile oFile;
                String cImagen = "";
                DateTime dfecha;
                // realizando el agregar

                cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
                cRutaContenido = Server.MapPath(cCarpetaContenido);

                oTUsuario = (Usuario)Session["Usuario"];
                nIdioma = oTUsuario.oIdioma.cod_idi_in;

                int nContador = 0, nNro = 0, nCodigo = 0;

                nNro = Convert.ToInt32(e.CommandArgument);

                oListaConEducacion = new List<ConsultorEducacion>();

                for (int i = 0; i < gridEduacion.Items.Count; i++)
                {
                    Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");
                    Label lblCodigo = (Label)gridEduacion.Items[i].FindControl("lblCodigo");
                    DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
                    //DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");
                    DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
                    RadNumericTextBox txtDuracion = (RadNumericTextBox)gridEduacion.Items[i].FindControl("txtDuracion");
                    TextBox txtInstitucion = (TextBox)gridEduacion.Items[i].FindControl("txtInstitucion");
                    TextBox txtDescripcion = (TextBox)gridEduacion.Items[i].FindControl("txtDescripcion");
                    Label lblDescarga = (Label)gridEduacion.Items[i].FindControl("lblDescarga");
                    Label lblDescargaN = (Label)gridEduacion.Items[i].FindControl("lblDescargaN");
                    RadAsyncUpload RadUpCertificado = (RadAsyncUpload)gridEduacion.Items[i].FindControl("RadUpCertificado");

                    nCodigo = int.Parse(lblNro.Text.ToString().Trim());

                    if (nCodigo != nNro)
                    {
                        nContador++;
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
                        }

                        oListaConEducacion.Add(new ConsultorEducacion()
                        {
                            cod_conedu_in = int.Parse(lblCodigo.Text),
                            Nro = nContador,
                            oNivel = new NivelAcademico()
                            {
                                cod_nivaca_in = int.Parse(cboNivel.SelectedValue.Trim())
                            },
                            des_conedu_vc = txtDescripcion.Text.Trim(),
                            //oDescripcion = new DescripcionAcademica()
                            //{
                            //    cod_desaca_in = int.Parse(cboDescripcion.SelectedValue.Trim())
                            //},
                            tip_dur_conedu_in = int.Parse(cboDuracion.SelectedValue.Trim()),
                            can_dur_conedu_in = int.Parse(txtDuracion.Text.Trim()),
                            ins_conedu_vc = txtInstitucion.Text,
                            adj_conedu_vc = lblDescargaN.Text.Trim()
                        });
                    }
                }


                gridEduacion.DataSource = oListaConEducacion;
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

                //int nNivel = 0;
                String cDescarga = "";
                for (int i = 0; i < gridEduacion.Items.Count; i++)
                {
                    DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
                    DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
                    // DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");

                    Label lblNivel = (Label)gridEduacion.Items[i].FindControl("lblNivel");
                    Label lblDescripcion = (Label)gridEduacion.Items[i].FindControl("lblDescripcion");
                    Label lblDuracion = (Label)gridEduacion.Items[i].FindControl("lblDuracion");

                    Label lblDescarga = (Label)gridEduacion.Items[i].FindControl("lblDescarga");
                    HyperLink lnkDescargar = (HyperLink)gridEduacion.Items[i].FindControl("lnkDescargar");

                    if (lblDescarga.Text.Trim().Length != 0)
                    {
                        lnkDescargar.Visible = true;
                        if (lblDescarga.Text.Trim().Length > 8)
                        {
                            cDescarga = lblDescarga.Text.Trim().Substring(0, 8) + "...";
                            lblDescarga.Text = cDescarga;
                        }
                    }

                    cboNivel.DataSource = oListaNivel;
                    cboNivel.DataValueField = "cod_nivaca_in";
                    cboNivel.DataTextField = "nom_nivacaidi_vc";
                    cboNivel.DataBind();
                    cboNivel.SelectedValue = lblNivel.Text.Trim();

                    // nNivel = int.Parse(lblNivel.Text.Trim());
                    //oAdministracionSer = new AdministracionService();
                    //oListaDescripcion = new List<DescripcionAcademica>();
                    //oListaDescripcion = oAdministracionSer.DescripcionAcademicaIdiomaListar(nNivel, oTUsuario.oIdioma.cod_idi_in);

                    //if (oListaDescripcion != null)
                    //{
                    //    if (oListaDescripcion.Count != 0)
                    //    {
                    //        oListaDescripcion.Insert(0, new DescripcionAcademica() { nom_desaca_vc = cSeleccione });
                    //    }
                    //    else
                    //    {
                    //        oListaDescripcion.Add(new DescripcionAcademica() { nom_desaca_vc = cSeleccione });
                    //    }
                    //}
                    //else
                    //{
                    //    oListaDescripcion.Add(new DescripcionAcademica() { nom_desaca_vc = cSeleccione });
                    //}

                    //cboDescripcion.DataSource = oListaDescripcion;
                    //cboDescripcion.DataValueField = "cod_desaca_in";
                    //cboDescripcion.DataTextField = "nom_desaca_vc";
                    //cboDescripcion.DataBind();
                    //cboDescripcion.SelectedValue = lblDescripcion.Text.Trim();

                    cboDuracion.DataSource = oListaDuracion;
                    cboDuracion.DataValueField = "val_cat_in";
                    cboDuracion.DataTextField = "nom_cat_vc";
                    cboDuracion.DataBind();
                    cboDuracion.SelectedValue = lblDuracion.Text.Trim();

                }

            }
        }
    }

    //void cboNivel_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    int nNivel = 0;
    //    int nTidioma = 0;
    //    if (Session["Usuario"] != null)
    //    {
    //        actualizarDatos();
    //        oTUsuario = (Usuario)Session["Usuario"];           
    //        String cSeleccione = "";

    //        if (oTUsuario.oIdioma.cod_idi_in == 1)
    //        {
    //            cSeleccione = "Select";
    //            nTidioma = 1;
    //        }
    //        else
    //        {
    //            nTidioma = 2;
    //            cSeleccione = "Seleccione";
    //        }
    //        GridDataItem item = (GridDataItem)(sender as DropDownList).NamingContainer;
    //        DropDownList cboNivel = (DropDownList)item.FindControl("cboNivel");            
    //        nNivel = int.Parse(cboNivel.SelectedValue.Trim());
    //        oAdministracionSer = new AdministracionService();
    //        oListaDescripcion = new List<DescripcionAcademica>();

    //        //lblAEducacion.Text = oTUsuario.oIdioma.cod_idi_in.ToString();

    //        oListaDescripcion = oAdministracionSer.DescripcionAcademicaIdiomaListar(nNivel,
    //            nTidioma);
    //        if (oListaDescripcion != null)
    //        {
    //            if (oListaDescripcion.Count != 0)
    //            {
    //                oListaDescripcion.Insert(0, new DescripcionAcademica() { nom_desaca_vc = cSeleccione });
    //            }
    //            else {
    //                oListaDescripcion.Add(new DescripcionAcademica() { nom_desaca_vc = cSeleccione });
    //            }
    //        }
    //        else {
    //            oListaDescripcion.Add(new DescripcionAcademica() { nom_desaca_vc=cSeleccione });            
    //        }
    //        DropDownList cboDescripcion = (DropDownList)item.FindControl("cboDescripcion");
    //        cboDescripcion.DataSource = oListaDescripcion;
    //        cboDescripcion.DataTextField = "nom_desaca_vc";
    //        cboDescripcion.DataValueField = "cod_desaca_in";
    //        cboDescripcion.DataBind();
    //    }

    //}

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

            for (int i = 0; i < gridPais.Items.Count; i++)
            {
                Label lblNro = (Label)gridPais.Items[i].FindControl("lblNro");
                DropDownList cboPais = (DropDownList)gridPais.Items[i].FindControl("cboPais");

                oListaConPais.Add(new ConsultorPais()
                {
                    Nro = int.Parse(lblNro.Text.Trim()),
                    cod_pais_in = int.Parse(cboPais.SelectedValue.Trim()),
                });
            }

            oListaConPais.Add(new ConsultorPais() { Nro = oListaConPais.Count + 1 });

            gridPais.DataSource = oListaConPais;
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

    protected void gridPais_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Eliminar")
        {
            actualizarDatos();
            int nIdioma = 0;
            String cSeleccione = "";

            if (Session["Usuario"] != null)
            {

                oTUsuario = (Usuario)Session["Usuario"];
                nIdioma = oTUsuario.oIdioma.cod_idi_in;
                oListaConPais = new List<ConsultorPais>();

                int nContador = 0, nNro = 0, nCodigo = 0;

                nNro = Convert.ToInt32(e.CommandArgument);


                for (int i = 0; i < gridPais.Items.Count; i++)
                {
                    Label lblNro = (Label)gridPais.Items[i].FindControl("lblNro");
                    DropDownList cboPais = (DropDownList)gridPais.Items[i].FindControl("cboPais");

                    nCodigo = int.Parse(lblNro.Text.ToString().Trim());

                    if (nCodigo != nNro)
                    {
                        nContador++;
                        oListaConPais.Add(new ConsultorPais()
                        {
                            Nro = nContador,
                            cod_pais_in = int.Parse(cboPais.SelectedValue.Trim()),
                        });
                    }
                }

                gridPais.DataSource = oListaConPais;
                gridPais.DataBind();

                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cSeleccione = "Seleccione";
                }
                else
                {
                    cSeleccione = "Select";
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

    }

    protected void lnkAgregarCV_Click(object sender, EventArgs e)
    {
        if (Session["Usuario"] != null)
        {
            actualizarEducacion();
            oTUsuario = (Usuario)Session["Usuario"];
            oListaConCV = new List<ConsultorDocumento>();
            oTListaConEducacion = new List<TConsultorEducacion>();
            cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
            cRutaContenido = Server.MapPath(cCarpetaContenido);
            UploadedFile oFile;
            String cImagen = "";
            DateTime dfecha;

            for (int i = 0; i < gridCV.Items.Count; i++)
            {
                Label lblNro = (Label)gridCV.Items[i].FindControl("lblNro");
                Label lblCodigo = (Label)gridCV.Items[i].FindControl("lblCodigo");
                TextBox txtTitulo = (TextBox)gridCV.Items[i].FindControl("txtTitulo");
                Label lblDescarga = (Label)gridCV.Items[i].FindControl("lblDescarga");
                Label lblDescargaN = (Label)gridCV.Items[i].FindControl("lblDescargaN");
                RadAsyncUpload RadUpCV = (RadAsyncUpload)gridCV.Items[i].FindControl("RadUpCV");

                if (RadUpCV.UploadedFiles.Count > 0)
                {


                    oFile = RadUpCV.UploadedFiles[0];
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

                oListaConCV.Add(new ConsultorDocumento()
                {
                    cod_condoc_in = int.Parse(lblCodigo.Text),
                    Nro = int.Parse(lblNro.Text),
                    nom_condoc_vc = txtTitulo.Text.Trim(),
                    des_condoc_vc = lblDescargaN.Text.Trim()
                });


            }

            oListaConCV.Add(new ConsultorDocumento() { Nro = oListaConCV.Count + 1 });

            gridCV.DataSource = oListaConCV;
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
    }

    protected void gridCV_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Eliminar")
        {
            int nIdioma = 0;

            actualizarEducacion();
            if (Session["Usuario"] != null)
            {
                cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
                cRutaContenido = Server.MapPath(cCarpetaContenido);
                UploadedFile oFile;
                String cImagen = "";
                DateTime dfecha;
                oTUsuario = (Usuario)Session["Usuario"];
                nIdioma = oTUsuario.oIdioma.cod_idi_in;


                int nContador = 0, nNro = 0, nCodigo = 0;

                nNro = Convert.ToInt32(e.CommandArgument);

                oListaConCV = new List<ConsultorDocumento>();

                for (int i = 0; i < gridCV.Items.Count; i++)
                {
                    Label lblNro = (Label)gridCV.Items[i].FindControl("lblNro");
                    Label lblCodigo = (Label)gridCV.Items[i].FindControl("lblCodigo");
                    TextBox txtTitulo = (TextBox)gridCV.Items[i].FindControl("txtTitulo");
                    Label lblDescarga = (Label)gridCV.Items[i].FindControl("lblDescarga");
                    Label lblDescargaN = (Label)gridCV.Items[i].FindControl("lblDescargaN");
                    nCodigo = int.Parse(lblNro.Text.ToString().Trim());
                    RadAsyncUpload RadUpCV = (RadAsyncUpload)gridCV.Items[i].FindControl("RadUpCV");

                    if (nCodigo != nNro)
                    {

                        if (RadUpCV.UploadedFiles.Count > 0)
                        {


                            oFile = RadUpCV.UploadedFiles[0];
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
                        nContador++;

                        oListaConCV.Add(new ConsultorDocumento()
                        {
                            cod_condoc_in = int.Parse(lblCodigo.Text),
                            Nro = nContador,
                            nom_condoc_vc = txtTitulo.Text.Trim(),
                            des_condoc_vc = lblDescargaN.Text.Trim()
                        });
                    }
                }

                gridCV.DataSource = oListaConCV;
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
        }
    }

    protected void lnkAgregarIdioma_Click(object sender, EventArgs e)
    {
        if (Session["Usuario"] != null)
        {
            actualizarDatos();
            oTUsuario = (Usuario)Session["Usuario"];
            oListaConLenguaje = new List<ConsultorLenguaje>();

            for (int i = 0; i < gridLenguaje.Items.Count; i++)
            {
                DropDownList cboLenguaje = (DropDownList)gridLenguaje.Items[i].FindControl("cboLenguaje");
                DropDownList cboHablado = (DropDownList)gridLenguaje.Items[i].FindControl("cboHablado");
                DropDownList cboLeido = (DropDownList)gridLenguaje.Items[i].FindControl("cboLeido");
                DropDownList cboEscrito = (DropDownList)gridLenguaje.Items[i].FindControl("cboEscrito");
                Label lblNro = (Label)gridLenguaje.Items[i].FindControl("lblNro");
                Label lblCodigo = (Label)gridLenguaje.Items[i].FindControl("lblCodigo");

                oListaConLenguaje.Add(new ConsultorLenguaje()
                {
                    cod_conlen_in = int.Parse(lblCodigo.Text),
                    Nro = int.Parse(lblNro.Text),
                    cod_len_in = int.Parse(cboLenguaje.SelectedValue.ToString()),
                    spk_conlen_in = int.Parse(cboHablado.SelectedValue.ToString()),
                    red_conlen_in = int.Parse(cboLeido.SelectedValue.ToString()),
                    wrt_conlen_in = int.Parse(cboEscrito.SelectedValue.ToString())
                });
            }

            oListaConLenguaje.Add(new ConsultorLenguaje() { Nro = oListaConLenguaje.Count + 1 });

            gridLenguaje.DataSource = oListaConLenguaje;
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

    protected void gridLenguaje_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Eliminar")
        {
            int nIdioma = 0;
            String cSeleccione = "";
            actualizarDatos();
            if (Session["Usuario"] != null)
            {

                oTUsuario = (Usuario)Session["Usuario"];
                nIdioma = oTUsuario.oIdioma.cod_idi_in;


                int nContador = 0, nNro = 0, nCodigo = 0;

                nNro = Convert.ToInt32(e.CommandArgument);

                oListaConLenguaje = new List<ConsultorLenguaje>();

                for (int i = 0; i < gridLenguaje.Items.Count; i++)
                {
                    DropDownList cboLenguaje = (DropDownList)gridLenguaje.Items[i].FindControl("cboLenguaje");
                    DropDownList cboHablado = (DropDownList)gridLenguaje.Items[i].FindControl("cboHablado");
                    DropDownList cboLeido = (DropDownList)gridLenguaje.Items[i].FindControl("cboLeido");
                    DropDownList cboEscrito = (DropDownList)gridLenguaje.Items[i].FindControl("cboEscrito");
                    Label lblNro = (Label)gridLenguaje.Items[i].FindControl("lblNro");
                    Label lblCodigo = (Label)gridLenguaje.Items[i].FindControl("lblCodigo");

                    nCodigo = int.Parse(lblNro.Text.ToString().Trim());

                    if (nCodigo != nNro)
                    {
                        nContador++;
                        oListaConLenguaje.Add(new ConsultorLenguaje()
                        {
                            cod_conlen_in = int.Parse(lblCodigo.Text),
                            Nro = nContador,
                            cod_len_in = int.Parse(cboLenguaje.SelectedValue.ToString()),
                            spk_conlen_in = int.Parse(cboHablado.SelectedValue.ToString()),
                            red_conlen_in = int.Parse(cboLeido.SelectedValue.ToString()),
                            wrt_conlen_in = int.Parse(cboEscrito.SelectedValue.ToString())
                        });
                    }
                }

                gridLenguaje.DataSource = oListaConLenguaje;
                gridLenguaje.DataBind();

                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cSeleccione = "Seleccione";
                }
                else
                {
                    cSeleccione = "Select";
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
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
        cRutaContenido = Server.MapPath(cCarpetaContenido);

        int nCodigoUsuario = 0, nTipo = 2, nTipoUsuario = 0, nTipoConsultor = 0, nPais = 0, nEstado = 0, nExperiencia = 0;
        String cNombre = "", cApellido = "", cCorreo = "", cUsuario = "",
        cContrasenia = "", cMensajeOk = "";
        bool bExperiencia = false;
        DateTime dfecha;
        UploadedFile oFile;
        Random random;
        UploadedFile oImagen;
        int randomNumber = 0;
        String cImagen = "";
        cCarpetaContenido = ConfigurationManager.AppSettings["CartepaContenido"];
        cRutaContenido = Server.MapPath(cCarpetaContenido);
        actualizaFoto();
        nCodigo = int.Parse(lblCodigo.Text.Trim());
        pnlOk.Visible = false;
        pnlError.Visible = false;
        if (nCodigo == 0)
        {
            if (rbInterno.Checked == true)
            {
                nTipoConsultor = 1;
                nTipoUsuario = 1;
            }
            else if (rbExterno.Checked == true)
            {
                nTipoConsultor = 2;
                nTipoUsuario = 2;
            }
        }
        else
        {

            nTipoConsultor = int.Parse(lblTipoConsultor.Text.Trim());
            nTipoUsuario = int.Parse(lblTipoConsultor.Text.Trim());
        }
        // validaciones
        string cError = "";
        pnlError.Visible = false;
        pnlOk.Visible = false;

        if (txtApellido.Text.Trim().Length == 0)
        {
            if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
               Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
               Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            {
                cError += "<li>El campo \"Apellidos\" es obligatorio.</li>";
            }
            else
            {
                cError += "<li>The field \"Surnames\" is required.</li>";
            }
        }

        if (txtNombre.Text.Trim().Length == 0)
        {
            if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
               Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
               Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            {
                cError += "<li>El campo \"Nombre\" es obligatorio.</li>";
            }
            else
            {
                cError += "<li>The field \"Firstname\" is required.</li>";
            }
        }

        DateTime dFechaNacimiento = new DateTime();
        //dFechaNacimiento = DateTime.Today;

        string[] cFecha;
        cFecha = txtFechaNacimiento.TextWithLiterals.Trim().Split('/');
        //Response.Write(cFecha.Length);

        if (cFecha.Length == 3)
        {
            try
            {
                int nDia = 0, nMes = 0, nAnio = 0;

                nDia = int.Parse(cFecha[0].Trim());
                nMes = int.Parse(cFecha[1].Trim());
                nAnio = int.Parse(cFecha[2].Trim());

                dFechaNacimiento = new DateTime(nAnio, nMes,
                                   nDia);
            }
            catch (Exception ex)
            {

                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cError += "<li>El campo \"Fecha de nacimiento\" tiene un formato incorrecto.</li>";
                }
                else
                {
                    cError += "<li>The field \"Date of Birth\" is malformed.</li>";
                }
            }
        }
        else
        {
            if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
               Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
               Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            {
                cError += "<li>El campo \"Fecha de nacimiento\" es obligatorio.</li>";
            }
            else
            {
                cError += "<li>The field \"Date of Birth\" is required.</li>";
            }
        }


        //if (txtDia.Text.Trim().Length == 0 || cboMes.SelectedValue.Trim().Equals("0") ||
        //    txtAnio.Text.Trim().Length == 0)
        //{

        //    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
        //       Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
        //       Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
        //    {
        //        cError += "<li>El campo \"Fecha de nacimiento\" es obligatorio.</li>";
        //    }
        //    else
        //    {
        //        cError += "<li>The field \"Date of Birth\" is required.</li>";
        //    }
        //}
        //else {

        //    try
        //    {
        //        dFechaNacimiento = new DateTime(int.Parse(txtAnio.Text.Trim()), int.Parse(cboMes.SelectedValue.Trim()),
        //                            int.Parse(txtDia.Text.Trim()));
        //    }
        //    catch (Exception ex) {

        //        if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
        //           Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
        //           Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
        //        {
        //            cError += "<li>El campo \"Fecha de nacimiento\" tiene un formato incorrecto.</li>";
        //        }
        //        else
        //        {
        //            cError += "<li>The field \"Date of Birth\" is malformed.</li>";
        //        }
        //    }

        //}

        if (cboSexo.SelectedValue.Trim().Equals("0"))
        {
            if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            {
                cError += "<li>Debe seleccionar una opción para \"Sexo\".</li>";
            }
            else
            {
                cError += "<li>You must select one option for \"Sex\".</li>";
            }
        }

        if (cboNacionalidad1.SelectedValue.Trim().Equals("0"))
        {
            if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            {
                cError += "<li>Debe seleccionar una opción para \"Nacionalidad 1\".</li>";
            }
            else
            {
                cError += "<li>You must select one option for \"Nationality 1\".</li>";
            }
        }
        else
        {

            if (cboNacionalidad1.SelectedValue.Trim().Equals(cboNacionalidad2.SelectedValue.Trim()))
            {
                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                     Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                     Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cError += "<li>Ha seleccionado nacionalidades repetidas.</li>";
                }
                else
                {
                    cError += "<li>You have selected nationalities repeated.</li>";
                }
            }

        }



        if (cboPais.SelectedValue.Trim().Equals("0"))
        {
            if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            {
                cError += "<li>El campo \"País\" es obligatorio.</li>";
            }
            else
            {
                cError += "<li>The field \"Country\" is required.</li>";
            }
        }

        if (txtCorreo.Text.Trim().Length == 0)
        {
            if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
               Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
               Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            {
                cError += "<li>El campo \"Correo\" es obligatorio.</li>";
            }
            else
            {
                cError += "<li>The field \"E-mail\" is required.</li>";
            }
        }
        else
        {

            if (oHelper.ValidarCorreo(txtCorreo.Text.Trim()) == false)
            {
                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cError += "<li>El correo tiene un formato incorrecto. Ejm: user@mail.com.</li>";
                }
                else
                {
                    cError += "<li>The e-mail has an incorrect format. Example: user@mail.com.</li>";
                }
            }

        }
        if (nTipoUsuario == 2)
        {
            if (nCodigo != 0)
            {
                if (chkContra.Checked == true)
                {
                    if (txtContra.Text.Trim().Length == 0)
                    {

                        if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                          Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                          Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                        {
                            cError += "<li>El campo \"Contraseña\" es obligatorio.</li>";
                        }
                        else
                        {
                            cError += "<li>The field \"Password\" is required.</li>";
                        }

                    }

                    if (txtContraRep.Text.Trim().Length == 0)
                    {

                        if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                          Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                          Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                        {
                            cError += "<li>El campo \"Re-Contraseña\" es obligatorio.</li>";
                        }
                        else
                        {
                            cError += "<li>The field \"Re-Password\" is required.</li>";
                        }

                    }

                    if (!txtContra.Text.Trim().Equals(txtContraRep.Text.Trim()))
                    {
                        if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                        Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                        Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                        {
                            cError += "<li>Las contraseñas ingresadas no coinciden.</li>";
                        }
                        else
                        {
                            cError += "<li>The entered passwords do not match.</li>";
                        }
                    }

                }

            }
            else
            {

                if (txtContra.Text.Trim().Length == 0)
                {
                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                      Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                      Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cError += "<li>El campo \"Contraseña\" es obligatorio.</li>";
                    }
                    else
                    {
                        cError += "<li>The field \"Password\" is required.</li>";
                    }
                }

                if (txtContraRep.Text.Trim().Length == 0)
                {
                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                      Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                      Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cError += "<li>El campo \"Re-Contraseña\" es obligatorio.</li>";
                    }
                    else
                    {
                        cError += "<li>The field \"Re-Password\" is required.</li>";
                    }
                }

                if (!txtContra.Text.Trim().Equals(txtContraRep.Text.Trim()))
                {
                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                      Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                      Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cError += "<li>Las contraseñas ingresadas no coinciden.</li>";
                    }
                    else
                    {
                        cError += "<li>The entered passwords do not match.</li>";
                    }
                }
            }
        }
        else if (nTipoUsuario == 1)
        {
            if (cboDA.SelectedValue.Trim().Equals("0"))
            {

                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                         Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                         Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cError += "<li>Debe seleccionar un \"Usuario AD\".</li>";
                }
                else
                {
                    cError += "<li>You must select one \"User AD\".</li>";
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

                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                             Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                             Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cError += "<li>No ha registrado su respuesta para \"Experiencia en países en desarrollo\".</li>";
                }
                else
                {
                    cError += "<li>It has not registered your answer for \"Experience in developing countries\".</li>";
                }
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
            if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                               Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                               Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            {
                cError += "<li>El campo \"Años de experiencia\" es obligatorio.</li>";
            }
            else
            {
                cError += "<li>The field \"Total years of experience\" are required.</li>";
            }
        }

        String cErrorNivel = "", cErrorDescripcion = "", cErroNDuracion = "", cErrorDuracion = "", cErrorInstitucion = "";
        if (gridEduacion.Items.Count != 0)
        {
            for (int i = 0; i < gridEduacion.Items.Count; i++)
            {
                Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");
                DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
                DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
                //DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");
                TextBox txtDescripcion = (TextBox)gridEduacion.Items[i].FindControl("txtDescripcion");
                RadNumericTextBox txtDuracion = (RadNumericTextBox)gridEduacion.Items[i].FindControl("txtDuracion");
                TextBox txtInstitucion = (TextBox)gridEduacion.Items[i].FindControl("txtInstitucion");

                Label lblDescarga = (Label)gridEduacion.Items[i].FindControl("lblDescarga");
                Label lblDescargaN = (Label)gridEduacion.Items[i].FindControl("lblDescargaN");
                RadAsyncUpload RadUpCertificado = (RadAsyncUpload)gridEduacion.Items[i].FindControl("RadUpCertificado");
                if (cboNivel.SelectedValue.Trim().Equals("0"))
                {

                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                      Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                      Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cErrorNivel = "<li>El campo \"Nivel académico\" es obligatorio.</li>";
                    }
                    else
                    {
                        cErrorNivel = "<li>The \"Academic Level\" es required.</li>";
                    }

                }

                if (txtDescripcion.Text.Trim().Length == 0)
                {
                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                         Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                         Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cErrorDescripcion = "<li>El campo \"Descripción académica\" es obligatorio.</li>";
                    }
                    else
                    {
                        cErrorDescripcion = "<li>The field \"Academic Description\" es required.</li>";
                    }

                }
                //if (cboDescripcion.SelectedValue.Trim().Equals("0"))
                //{
                //    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                // Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                // Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                //    {
                //        cErrorDescripcion = "<li>El campo \"Descripción académica\" es obligatorio.</li>";
                //    }
                //    else
                //    {
                //        cErrorDescripcion = "<li>The field \"Academic Description\" es required.</li>";
                //    }

                //}

                if (txtDuracion.Value != null)
                {
                    if (txtDuracion.Value == 0)
                    {
                        if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                            Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                            Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                        {
                            cErroNDuracion = "<li>El campo \"Duración\" debe ser mayor a cero.</li>";
                        }
                        else
                        {
                            cErroNDuracion = "<li>The field \"Duration\" must be more than zero.</li>";
                        }
                    }
                }

                if (cboDuracion.SelectedValue.Trim().Equals("0"))
                {
                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                 Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                 Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cErrorDuracion = "<li>El campo \"Tipo de duración\" es obligatorio.</li>";
                    }
                    else
                    {
                        cErrorDuracion = "<li>The field \"Duration type\" is required.</li>";

                    }

                }

                if (txtInstitucion.Text.Trim().Length == 0)
                {
                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                          Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                          Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cErrorInstitucion = "<li>El campo \"Institución\" es obligatorio.</li>";
                    }
                    else
                    {
                        cErrorInstitucion = "<li>The field \"Institution\" is required.</li>";
                    }
                }

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
                        random = new Random();
                        randomNumber = random.Next(0, 2000);
                        cImagen = randomNumber.ToString().Trim() +
                              "_CE_" + lblNro.Text.Trim() + dfecha.Year.ToString().Trim() + dfecha.Month.ToString().Trim() +
                              dfecha.Day.ToString().Trim() + dfecha.Hour.ToString().Trim() +
                              dfecha.Minute.ToString().Trim() + dfecha.Second.ToString().Trim();
                        oFile.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                    }

                    lblDescargaN.Text = cImagen.Trim();
                    lblDescarga.Text = cImagen.Trim();

                }
            }

            if (cErrorNivel.Trim().Length != 0)
            {
                cError += cErrorNivel;
            }

            if (cErrorDescripcion.Trim().Length != 0)
            {
                cError += cErrorDescripcion;
            }

            if (cErroNDuracion.Trim().Length != 0)
            {
                cError += cErroNDuracion;
            }

            if (cErrorDuracion.Trim().Length != 0)
            {
                cError += cErrorDuracion;
            }

            if (cErrorInstitucion.Trim().Length != 0)
            {
                cError += cErrorInstitucion;
            }


        }
        /*else {
            if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
            {
                cError += "<li>Debe ingresar al menos un registro en \"Educación\".</li>";
            }
            else
            {
                cError += "<li>You must enter at least one record in \"Education\".</li>";
            }
        }*/

        //bool bHabilidad=false;
        //for (int i = 0; i < gridHabilidades.Items.Count; i++)
        //{

        //    RadListBox lstTecnico = (RadListBox)gridHabilidades.Items[i].FindControl("lstTecnico");
        //    IList<RadListBoxItem> oColTecnico = lstTecnico.CheckedItems;

        //    if (oColTecnico.Count != 0) {
        //        bHabilidad = true;
        //        break;
        //    }            
        //}

        //if (bHabilidad == false) {
        //    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
        //     Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
        //     Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
        //    {
        //        cError += "<li>Debe seleccionar al menos una \"Habilidad\".</li>";
        //    }
        //    else
        //    {
        //        cError += "<li>You must select at least one \"Skill\".</li>";
        //    }
        //}

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
                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                      Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                      Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cError += "<li>Debe ingresar al menos una opción para el pais en \"Experiencia previa\".</li>";
                }
                else
                {
                    cError += "<li>You must enter at least one option for the country in \"Previous experience\".</li>";
                }
            }
        }

        //cvs
        int contadorArchivo = 0, contadorLonguitudCero = 0;

        if (gridCV.Items.Count != 0)
        {
            for (int i = 0; i < gridCV.Items.Count; i++)
            {
                Label lblNro = (Label)gridCV.Items[i].FindControl("lblNro");

                TextBox txtTitulo = (TextBox)gridCV.Items[i].FindControl("txtTitulo");
                Label lblDescarga = (Label)gridCV.Items[i].FindControl("lblDescarga");
                Label lblDescargaN = (Label)gridCV.Items[i].FindControl("lblDescargaN");

                RadAsyncUpload RadUpCV = (RadAsyncUpload)gridCV.Items[i].FindControl("RadUpCV");

                if (RadUpCV.UploadedFiles.Count > 0)
                {

                    oFile = RadUpCV.UploadedFiles[0];
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

                if (lblDescarga.Text.Trim().Length == 0)
                {
                    contadorArchivo++;
                }

                if (txtTitulo.Text.Trim().Length == 0)
                {
                    //contadorLonguitudCero++;
                }
            }

            if (contadorLonguitudCero > 0)
            {
                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cError += "<li>Debe agregar un \"Título\" al CV.</li>";
                }
                else
                {
                    cError += "<li>You must add a \"Title\" for the CV.</li>";
                }
            }

            if (contadorArchivo > 0)
            {
                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cError += "<li>Debe de agregar al menos un archivo en \"CV\".</li>";
                }
                else
                {
                    cError += "<li>You must add at least one file in \"CV\".</li>";
                }
            }
        }
        //else {
        //    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
        //      Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
        //      Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
        //    {
        //        cError += "<li>Debe ingresar al menos un registro en \"CVs\".</li>";
        //    }
        //    else
        //    {
        //        cError += "<li>You must add at least one record in \"CVs\".</li>";
        //    }
        //}
        //lenguaje

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
                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                        Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                        Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cErrorLenguaje = "<li>El campo \"Lenguaje\" es obligatorio. </li>";
                    }
                    else
                    {
                        cErrorLenguaje = "<li>The field \"Language\" is required. </li>";
                    }

                }
                if (cboHablado.SelectedValue.Trim().Equals("0"))
                {
                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                       Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                       Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cErrorHablado = "<li>El campo \"Hablado\" dentro de Lenguaje es obligatorio.</li>";
                    }
                    else
                    {
                        cErrorHablado = "<li>The field \"Speak\" in Language is required.</li>";
                    }

                }
                if (cboLeido.SelectedValue.Trim().Equals("0"))
                {
                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                     Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                     Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cErrorLeido = "<li>El campo \"Leído\" dentro de Lenguaje es obligatorio.</li>";
                    }
                    else
                    {
                        cErrorLeido = "<li>The field \"Read\" in Language is required.</li>";
                    }

                }
                if (cboEscrito.SelectedValue.Trim().Equals("0"))
                {
                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                     Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                     Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cErrorEscrito = "<li>El campo \"Escrito\" dentro de Lenguaje es obligatorio.</li>";
                    }
                    else
                    {
                        cErrorEscrito = "<li>The field \"Write\" in Language is required.</li>";
                    }

                }

            }

            if (cErrorLenguaje.Trim().Length != 0)
            {
                cError += cErrorLenguaje;
            }

            if (cErrorHablado.Trim().Length != 0)
            {
                cError += cErrorHablado;
            }

            if (cErrorLeido.Trim().Length != 0)
            {
                cError += cErrorLeido;
            }

            if (cErrorEscrito.Trim().Length != 0)
            {
                cError += cErrorEscrito;
            }
        }
        //else {
        //    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
        //           Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
        //           Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
        //    {
        //        cError += "<li>Debe ingresar al menos un registro en \"Lenguaje\".</li>";
        //    }
        //    else
        //    {
        //        cError += "<li>You must enter at least one record in \"Language\".</li>";
        //    }
        //}

        if (cError.Trim().Length != 0)
        {
            pnlError.Visible = true;
            litError.Text = "<ul>" + cError.Trim() + "</ul>";

            txtContra.Attributes["value"] = txtContra.Text;

            txtContraRep.Attributes["value"] = txtContraRep.Text;

        }
        else
        {


            cNombre = txtNombre.Text.Trim();
            cApellido = txtApellido.Text.Trim();
            cCorreo = txtCorreo.Text.Trim();
            nPais = int.Parse(cboPais.SelectedValue.Trim());
            nEstado = 1;

            nCodigo = int.Parse(lblCodigo.Text.Trim());
            nCodigoUsuario = int.Parse(lblCodigoUsuario.Text.Trim());

            // usuario
            if (nTipoUsuario == 1)
            {
                cUsuario = cboDA.SelectedValue.Trim();
                cContrasenia = cboDA.SelectedValue.Trim();
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
            oPersona.est_per_in = nEstado;

            oPersona.oIdioma = new Idioma()
            {
                cod_idi_in = int.Parse(cboIdioma.SelectedValue.Trim())
            };

            oPersona.oPais = new Pais
            {
                cod_pais_in = nPais
            };

            oConsultorSer = new ConsultorService();
            int nValida = 0;

            nValida = oConsultorSer.ConsultorCorreoValida(nCodigo, txtCorreo.Text.Trim());

            if (nValida != 0)
            {

                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                   Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                   Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cMensajeOk += "Ya existe un consultor o usuario registrado con este correo.";
                }
                else
                {
                    cMensajeOk += "Consultant or User is already registered with this e-mail.";
                }

                pnlError.Visible = true;
                litError.Text = cMensajeOk.Trim();

                txtContra.Attributes["value"] = txtContra.Text;

                txtContraRep.Attributes["value"] = txtContraRep.Text;
                return;
            }

            if (nTipoUsuario == 1)
            {
                nValida = 0;
                nValida = oConsultorSer.ConsultorNombreValida(nCodigoUsuario, cboDA.SelectedValue.ToString());

                if (nValida != 0)
                {

                    if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                       Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                       Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                    {
                        cMensajeOk += "Ya existe un consultor o usuario registrado con este usuario.";
                    }
                    else
                    {
                        cMensajeOk += "Consultant or user is already registered with this user.";
                    }

                    pnlError.Visible = true;
                    litError.Text = cMensajeOk.Trim();

                    txtContra.Attributes["value"] = txtContra.Text;

                    txtContraRep.Attributes["value"] = txtContraRep.Text;
                    return;
                }
            }

            oSeguridadSer = new SeguridadService();
            oAdministracionSer = new AdministracionService();

            if (nCodigo == 0)
            {

                nCodigo = oSeguridadSer.PersonaInsertar(oPersona);


                cImagen = "";
                cImagen = lblFotoDescarga.Text;
                //if (RadUpFoto.UploadedFiles.Count > 0)
                //{
                //    oImagen = RadUpFoto.UploadedFiles[0];
                //    cImagen = oHelper.ReplaceCaracter(txtNombre.Text.Trim()) + "_" + oHelper.ReplaceCaracter(txtApellido.Text.Trim()) +
                //              "_IMG_" + nCodigo.ToString().Trim() + oImagen.GetExtension();
                //    try
                //    {
                //        oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                //    }
                //    catch
                //    {
                //        cImagen = oHelper.ReplaceCaracter(txtNombre.Text.Trim()) + "_" + oHelper.ReplaceCaracter(txtApellido.Text.Trim()) +
                //              "_IMG_" + nCodigo.ToString().Trim() + oImagen.GetExtension();
                //        oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                //    }
                //}
                //else {

                //}



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
                oConsultor.img_con_vc = cImagen;
                oConsultor.dir_con_vc = txtDireccion.Text;
                oConsultor.cer_exp_con_bo = bExperiencia;
                //oConsultor.oCiudad = new Ciudad() { cod_ciu_in = int.Parse(cboCiudad.SelectedValue.Trim()) };
                oConsultor.ciu_con_vc = txtCiudad.Text.Trim();

                oConsultorSer = new ConsultorService();
                oConsultorSer.ConsultorInsertar(oConsultor);

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
                    DropDownList cboNivel = (DropDownList)gridEduacion.Items[i].FindControl("cboNivel");
                    DropDownList cboDuracion = (DropDownList)gridEduacion.Items[i].FindControl("cboDuracion");
                    //DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");
                    RadNumericTextBox txtDuracion = (RadNumericTextBox)gridEduacion.Items[i].FindControl("txtDuracion");
                    TextBox txtDescripcion = (TextBox)gridEduacion.Items[i].FindControl("txtDescripcion");
                    TextBox txtInstitucion = (TextBox)gridEduacion.Items[i].FindControl("txtInstitucion");
                    Label lblNro = (Label)gridEduacion.Items[i].FindControl("lblNro");
                    Label lblDescarga = (Label)gridEduacion.Items[i].FindControl("lblDescargaN");
                    //RadAsyncUpload RadUpCertificado = (RadAsyncUpload)gridEduacion.Items[i].FindControl("RadUpCertificado");

                    //if (RadUpCertificado.UploadedFiles.Count > 0)
                    //{
                    //    oImagen = RadUpCertificado.UploadedFiles[0];
                    //    cImagen = nCodigo.ToString().Trim() + lblNro.Text.Trim() + "Cer" +
                    //        oHelper.ReplaceCaracter(txtNombre.Text.Trim() + txtApellido.Text.Trim()) + oImagen.GetExtension();
                    //    try
                    //    {
                    //        oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                    //    }
                    //    catch
                    //    {
                    //        cImagen = nCodigo.ToString().Trim() + lblNro.Text.Trim() + "Cer" + "Consultant" + oImagen.GetExtension();
                    //        oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                    //    }
                    //    lblDescarga.Text = cImagen.Trim();

                    //}

                    oConsultorSer.EducacionInsertar(new ConsultorEducacion()
                    {
                        oNivel = new NivelAcademico() { cod_nivaca_in = int.Parse(cboNivel.SelectedValue.Trim()) },
                        //oDescripcion = new DescripcionAcademica() { cod_desaca_in = int.Parse(cboDescripcion.SelectedValue.Trim()) },
                        tip_dur_conedu_in = int.Parse(cboDuracion.SelectedValue.Trim()),
                        can_dur_conedu_in = int.Parse(txtDuracion.Text),
                        ins_conedu_vc = txtInstitucion.Text,
                        adj_conedu_vc = lblDescarga.Text,
                        cod_per_in = nCodigo,
                        des_conedu_vc = txtDescripcion.Text.Trim()
                    });

                }

                // habilidadesType

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

                //Donante
                IList<RadListBoxItem> oColDonante = lstDonante.CheckedItems;
                foreach (RadListBoxItem item in oColDonante)
                {
                    oConsultorSer.DonanteInsertar(new ConsultorDonante()
                    {
                        cod_don_in = int.Parse(item.Value),
                        cod_per_in = nCodigo
                    });
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

                    cImagen = "";

                    //if (RadUpCV.UploadedFiles.Count > 0)
                    //{
                    //    oImagen = RadUpCV.UploadedFiles[0];
                    //    cImagen = nCodigo.ToString().Trim() + lblNro.Text.Trim() + "Doc" + oHelper.ReplaceCaracter(txtNombre.Text + txtApellido.Text) + oImagen.GetExtension();
                    //    try
                    //    {
                    //        oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                    //    }
                    //    catch
                    //    {
                    //        cImagen = nCodigo.ToString().Trim() + lblNro.Text.Trim() + "Doc" + "Consultant";
                    //        oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                    //    }
                    //}

                    oConsultorSer.DocumentoInsertar(new ConsultorDocumento()
                    {
                        cod_per_in = nCodigo,
                        nom_condoc_vc = txtTitulo.Text.Trim(),
                        des_condoc_vc = lblDescarga.Text.Trim(),
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

                oUsuario = new Usuario();
                oUsuario.tip_usu_in = nTipoUsuario;
                oUsuario.nom_usu_vc = cUsuario;

                oUsuario.con_usu_vc = cContrasenia;
                oUsuario.est_usu_in = nEstado;
                oUsuario.cod_per_in = nCodigo;
                oSeguridadSer.UsuarioInsertar(oUsuario);

                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cMensajeOk += "Se registró el consultor satisfactoriamente.";
                }
                else
                {
                    cMensajeOk += "Consultant was recorded successfully.";
                }
                Session["cMensajeOk"] = cMensajeOk;
                Response.Redirect("WebConsultantAgregar.aspx?nCode=" + nCodigo.ToString().Trim());


            }
            else
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
                //if (RadUpFoto.UploadedFiles.Count > 0)
                //{

                //    oImagen = RadUpFoto.UploadedFiles[0];
                //    cImagen = nCodigo.ToString().Trim() + oHelper.ReplaceCaracter(txtNombre.Text + txtApellido.Text) + oImagen.GetExtension();
                //    try
                //    {
                //        oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                //    }
                //    catch
                //    {
                //        cImagen = nCodigo.ToString().Trim() + "Consultant";
                //        oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                //    }
                //}


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
                oConsultor.cer_exp_con_bo = bExperiencia;
                //oConsultor.oCiudad = new Ciudad() { cod_ciu_in = int.Parse(cboCiudad.SelectedValue.Trim()) };
                oConsultor.ciu_con_vc = txtCiudad.Text.Trim();

                oConsultor.img_con_vc = cImagen;
                oConsultorSer = new ConsultorService();
                oConsultorSer.ConsultorEditar(oConsultor);


                oConsultorSer.NacionalidadEliminar(nCodigo);
                oConsultorSer.EducacionEliminar(nCodigo);
                oConsultorSer.ConocimientoEliminar(nCodigo);
                oConsultorSer.DonanteEliminar(nCodigo);
                oConsultorSer.PaisEliminar(nCodigo);
                oConsultorSer.DocumentoEliminar(nCodigo);
                oConsultorSer.LenguajeEliminar(nCodigo);

                oUsuario = new Usuario();
                oUsuario.cod_usu_in = nCodigoUsuario;
                oUsuario.tip_usu_in = nTipoUsuario;
                oUsuario.nom_usu_vc = cUsuario;

                oUsuario.con_usu_vc = cContrasenia;
                oUsuario.est_usu_in = nEstado;
                oUsuario.cod_per_in = nCodigo;
                oSeguridadSer.UsuarioEditar(oUsuario);

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
                    //DropDownList cboDescripcion = (DropDownList)gridEduacion.Items[i].FindControl("cboDescripcion");
                    RadNumericTextBox txtDuracion = (RadNumericTextBox)gridEduacion.Items[i].FindControl("txtDuracion");
                    TextBox txtInstitucion = (TextBox)gridEduacion.Items[i].FindControl("txtInstitucion");
                    TextBox txtDescripcion = (TextBox)gridEduacion.Items[i].FindControl("txtDescripcion");
                    RadAsyncUpload RadUpCertificado = (RadAsyncUpload)gridEduacion.Items[i].FindControl("RadUpCertificado");
                    Label lblDescarga = (Label)gridEduacion.Items[i].FindControl("lblDescargaN");

                    //if (RadUpCertificado.UploadedFiles.Count > 0)
                    //{
                    //    oImagen = RadUpCertificado.UploadedFiles[0];
                    //    cImagen = nCodigo.ToString().Trim() + lblNro.Text.Trim() + "Cer" + oHelper.ReplaceCaracter(txtNombre.Text + txtApellido.Text) + oImagen.GetExtension();
                    //    try
                    //    {
                    //        oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                    //    }
                    //    catch
                    //    {
                    //        cImagen = nCodigo.ToString().Trim() + lblNro.Text.Trim() + "Cer" + "Consultant";
                    //        oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                    //    }
                    //}
                    //else
                    //{
                    //    cImagen = lblDescarga.Text.Trim();
                    //}

                    oConsultorSer.EducacionInsertar(new ConsultorEducacion()
                    {
                        oNivel = new NivelAcademico() { cod_nivaca_in = int.Parse(cboNivel.SelectedValue.Trim()) },
                        des_conedu_vc = txtDescripcion.Text.Trim(),
                        //oDescripcion = new DescripcionAcademica() { cod_desaca_in = int.Parse(cboDescripcion.SelectedValue.Trim()) },
                        tip_dur_conedu_in = int.Parse(cboDuracion.SelectedValue.Trim()),
                        can_dur_conedu_in = int.Parse(txtDuracion.Text),
                        ins_conedu_vc = txtInstitucion.Text,
                        adj_conedu_vc = lblDescarga.Text.Trim(),
                        cod_per_in = nCodigo
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
                    cImagen = "";

                    //if (RadUpCV.UploadedFiles.Count > 0)
                    //{
                    //    oImagen = RadUpCV.UploadedFiles[0];
                    //    cImagen = nCodigo.ToString().Trim() + lblNro.Text.Trim() + "Doc" + oHelper.ReplaceCaracter(txtNombre.Text + txtApellido.Text) + oImagen.GetExtension();
                    //    try
                    //    {
                    //        oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                    //    }
                    //    catch
                    //    {
                    //        cImagen = nCodigo.ToString().Trim() + lblNro.Text.Trim() + "Doc" + "Consultant";
                    //        oImagen.SaveAs(cRutaContenido + "/" + cImagen.Trim());
                    //    }
                    //}
                    //else
                    //{
                    //    cImagen = lblDescarga.Text.Trim();
                    //}

                    oConsultorSer.DocumentoInsertar(new ConsultorDocumento()
                    {
                        cod_per_in = nCodigo,
                        nom_condoc_vc = txtTitulo.Text.Trim(),
                        des_condoc_vc = lblDescarga.Text.Trim(),
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

                if (Thread.CurrentThread.CurrentCulture.Name.Equals("es") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-PE") ||
                Thread.CurrentThread.CurrentCulture.Name.Equals("es-ES"))
                {
                    cMensajeOk += "Se modificaron los datos del consultor satisfactoriamente.";
                }
                else
                {
                    cMensajeOk += "Consultant data were changed successfully.";
                }

                pnlOk.Visible = true;
                litOk.Text = cMensajeOk.Trim();

                Ind(nCodigo);

            }

            //Label1.Text= RadUpFoto.UploadedFiles[0].InputStream;
        }
    }

    protected void chkContra_CheckedChanged(object sender, EventArgs e)
    {
        actualizarDatos();
        txtContra.Enabled = chkContra.Checked;
        txtContraRep.Enabled = chkContra.Checked;

    }

    protected override void InitializeCulture()
    {
        if (Request.QueryString["lang"] != null)
        {
            if (Session["Usuario"] != null)
            {
                oTUsuario = (Usuario)Session["Usuario"];
                oTUsuario.oIdioma.cul_idi_vc = Request.QueryString["lang"].Trim();

                if (Request.QueryString["lang"].Trim().Equals("ES"))
                {
                    oTUsuario.oIdioma.cod_idi_in = 2;
                }
                else
                {
                    oTUsuario.oIdioma.cod_idi_in = 1;
                }
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

        base.InitializeCulture();
    }

    protected void lnkImportar_Click(object sender, EventArgs e)
    {
        IList<UsuarioLDAP> oListaLdap = new List<UsuarioLDAP>();
        UsuarioLDAP[] oListaDAService;
        String cValor = cboDA.SelectedValue.Trim();

        try
        {
            if (Session["Usuario"] != null)
            {

                oTUsuario = (Usuario)Session["Usuario"];

                //llamar web service
                oLDAPSer = new LDAPServiceClient();
                oListaDAService = oLDAPSer.ListarTodos();
                oListaLdap = oListaDAService.OrderBy(x => x.cn).ToList();
                oLDAPSer = null;

                //Response.Write(oListaLdap.Count.ToString());

                // BD Ldap
                IList<Usuario> oListaBDLdap = new List<Usuario>();
                oLdapService = new LdapService();

                oListaBDLdap = oLdapService.ListarUsuarioLdap();

                bool bExiste = false;

                foreach (UsuarioLDAP oUsuarioLDAP in oListaLdap)
                {
                    bExiste = false;

                    foreach (Usuario oUsuario in oListaBDLdap)
                    {

                        if (oUsuario.con_usu_vc.Trim().Equals(oUsuarioLDAP.user.Trim()))
                        {
                            bExiste = true;
                            break;
                        }
                    }

                    if (bExiste == false)
                    {
                        //insertar
                        oLdapService.InsertarUsuarioLdap(new Usuario()
                        {
                            nom_usu_vc = oUsuarioLDAP.cn.Trim(),
                            con_usu_vc = oUsuarioLDAP.user.Trim()
                        });
                    }
                }

                ListarUsuarioDA();

                cboDA.SelectedValue = cValor.Trim();

            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnUsuario_Click(object sender, EventArgs e)
    {
        if (gridUsuario.SelectedItems.Count != 0)
        {
            winCopiar.DestroyOnClose = true;
            cboDA.SelectedValue = gridUsuario.SelectedValue.ToString();
            SeleccionarUsuario();
        }
    }
    
    protected void cboDA_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        SeleccionarUsuario();
    }

    private void SeleccionarUsuario() {
        oUsuario = new Usuario();
        oLdapService = new LdapService();

        oUsuario = oLdapService.IndUsuarioLdap(cboDA.SelectedValue.ToString());

        if (oUsuario != null)
        {
            txtCorreo.Text = oUsuario.cor_usu_vc;
            txtApellido.Text = oUsuario.ape_usu_vc;
            txtNombre.Text = oUsuario.nom_usu_vc;

        }
    }

}