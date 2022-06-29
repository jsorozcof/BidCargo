using System;
using System.Globalization;
using System.Net.Mail;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using BidCargo_.Models;

public class EnviarCorreos
{
    private static Random random = new Random();
    public void EnviarCorreo(string correoAlQueEnvio, string asuntoDelCorreo, string copiaCorreoEnvio, string textoDelCorreo, string correoDesdeElQueEnvio, string usuarioCorreEnvio, string contrasenaCorreoEnvio, string archivo)
    {             /*-------------------------MENSAJE DE CORREO----------------------*/
        /*-------------------------MENSAJE DE CORREO----------------------*/
        System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
        //Direccion de correo electronico a la que queremos enviar el mensaje
        mmsg.To.Add(correoAlQueEnvio);
        //Asunto
        mmsg.Subject = asuntoDelCorreo;
        mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
        //Direccion de correo electronico que queremos que reciba una copia del mensaje
        mmsg.Bcc.Add(copiaCorreoEnvio);
        //Cuerpo del Mensaje
        mmsg.Body = textoDelCorreo;

        //Cargar Imagen
        //string htmlBody = "";
        //htmlBody = htmlBody + "<img src=\"cid:closetEscritorio\">" + Environment.NewLine;
        //mmsg.IsBodyHtml = true;
        //mmsg.Body = htmlBody;
        //AlternateView htmlview = default(AlternateView);
        //htmlview = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");
        //MemoryStream logo = new MemoryStream();
        //LinkedResource imageResourceEs = new LinkedResource(HttpContext.Current.Server.MapPath("~//Images//" + "closetEscritorio.jpg"));
        //imageResourceEs.ContentId = "photo";
        //imageResourceEs.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
        //htmlview.LinkedResources.Add(imageResourceEs);
        //mmsg.AlternateViews.Add(htmlview);

        mmsg.BodyEncoding = System.Text.Encoding.UTF8;
        mmsg.IsBodyHtml = false;
        if (archivo.Length > 2) mmsg.Attachments.Add(new Attachment(archivo));
        //Correo electronico desde la que enviamos el mensaje
        mmsg.From = new System.Net.Mail.MailAddress(correoDesdeElQueEnvio);

        /*-------------------------CLIENTE DE CORREO----------------------*/
        System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
        //Hay que crear las credenciales del correo emisor
        cliente.Credentials = new System.Net.NetworkCredential(usuarioCorreEnvio, contrasenaCorreoEnvio);
        //Lo siguiente es obligatorio si enviamos el mensaje desde Gmail
        cliente.Port = 587;
        cliente.EnableSsl = true;
        cliente.Host = "smtp.office365.com";
        // cliente.Host = "smtp.gmail.com";
        cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
        cliente.Timeout = 5000;
        mmsg.IsBodyHtml = true;

        try
        {
            cliente.Send(mmsg);
        }
        catch (SmtpFailedRecipientsException ex)
        {
            for (int i = 0; i < ex.InnerExceptions.Length; i++)
            {
                SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                if (status == SmtpStatusCode.MailboxBusy ||
                    status == SmtpStatusCode.MailboxUnavailable)
                {
                    Console.WriteLine("Delivery failed - retrying in 5 seconds.");
                    System.Threading.Thread.Sleep(5000);
                    //cliente.Send(mmsg);
                }
                else
                {
                    Console.WriteLine("Failed to deliver message to {0}",
                        ex.InnerExceptions[i].FailedRecipient);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception caught in RetryIfBusy(): {0}",
                    ex.ToString());
        }

    }

    public string ArmarCorreoConfimacion(string Nombre)
    {
        string stringBody = @"
                <html>
                    <body> 
                        <h2> Equipo de BidCargo </h2>  
                        <div style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>
                            <p>Un placer saludarl@ "+Nombre+@", de parte del Equipo de BidCargo...</p>

                            <p>El registro en nuestra plataforma ha sido completado satisfactoriamente, ya puede acceder a su cuenta creada.</p>

                            <p>Saludos...</p>
                        </div>
                    </body>
                </html>
            ";

        return stringBody;
    }


    public string ArmarCorreoConfimacion2(string Nombre)
    {
        string stringBody = @"
                <html>
                    <body> 
                        <h2> Equipo de BidCargo </h2>  
                        <div style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>

                            <p>El usuario " + Nombre + @" subió documentos satisfactoriamente “ ya puede acceder y verificar su información y los documentos cargados</p>

                            <p>Buen trabajo...</p>
                        </div>
                    </body>
                </html>
            ";

        return stringBody;
    }


    public string ArmarCorreoConfirmacionAdmin(string Nombre, string Cargo)
    {

        string stringBody = @"
            <html>
                <body> 
                    <h2> Equipo de BidCargo </h2>  
                    <div style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>
                        <p>Administrativo bidcargo...</p>
                        <p>Nos complace informarle que" +Nombre+ @" cargo los documentos a  su plataforma,
                        revisar la documentacion pertinente</p>
                        <p>Saludos...</p>
                    </div>
                </body>
            </html>
        ";
        return stringBody;
    }
   
    public string ArmarCorreoPhone(string phone = "")
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>La persona de este numero celular: "+phone+ ", esta esperando a que se pongan en contacto con el...";
        strBody += "<br> para conocer así sus necesidades";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }
    public string ArmarCorreoAprovedProfile(dynamic row = null, string urlpath = "", int prof= 0)
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarl@ "+row["usuarioFaceBook"]+", de parte del Equipo de BidCargo...";
        if(prof == 0)
        {
            strBody += "<br>Lamentamos informarle que su perfil en nuestra plataforma NO FUE APROBADO, y NO PUEDE INGRESAR a la misma.";
            strBody += "<br>Por favor, contacte con nuestros administradores, disculpe las molestias ocasionadas";
        }
        else
        {
            strBody += "<br>Nos complace informarle que su perfil en nuestra plataforma fue APROBADO, y ya PUEDE INGRESAR a la misma,";
            strBody += "<br>para disfrutar de nuestros servicios";
        }
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }

    public string sendMailAccepOffer(dynamic row = null, dynamic row2 = null, string urlpath = "")
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarl@ " + row["usuarioFaceBook"] + ", de parte del Equipo de BidCargo...";

        strBody += "<br>Nos complace informarle, que la oferta que realizo a la Cotizacion "+row["codeOffer"]+", ha sido aceptada.";
        strBody += "<br>Le invitamos a ingresar a nuestra plataforma para conocer mas detalles, los datos de contacto de la empresa contratante son los siguientes:";
        strBody += "<br>Empresa Contratante: " +row2["usuarioFaceBook"]+"";
        strBody += "<br>Dirección: " + row2["direccion"] + "";
        strBody += "<br>Persona de Contacto: " + row2["nombre"] + "  "+ row2["apellidoPaterno"]+"";
        strBody += "<br>Telefono de Contacto: " + row2["numeroCelular"] + "";
        strBody += "<br>Email de Contacto: " + row2["email"] + "";
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    } 


    public string sendMailNOAccepOffer(dynamic row = null,  string urlpath = "")
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";

        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br>Un placer saludarlo " + row["usuarioFaceBook"] + ", de parte del equipo de Bidcargo,";

        strBody += "<br>Estimados señores " + row["usuarioFaceBook"] + ": Le notificamos que la solicitud de carga  " + row["codeOffer"] + ", ha sido asignada a otra empresa;";
        strBody += "<br>le invitamos a seguir ofertando sus servicios para una nueva solicitud.";
     
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Equipo de Atención al Cliente BidCargo</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }

    public string sendMailContraOffer(string codeOffer = "" , string urlpath = "")
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarlo...";

        strBody += "<br>Nos complace informarle, que a la oferta, con  codigo, "+ codeOffer + " le han realizado una Oferta de Servicio.";
        strBody += "<br>Le invitamos a ingresar a nuestra plataforma para conocer mas detalles";
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }





    public string sendMailContraOffer2(string codeOffer, string urlpath)
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarlo...";

        strBody += "<br>Nos complace informarle, que la a oferta, codigo, " + codeOffer + " le han realizado una Oferta de Servicio.";
        strBody += "<br>Le invitamos a ingresar a nuestra plataforma para conocer mas detalles";
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }



    public string ArmarCorreoActiveProfile(dynamic row = null, string urlpath = "", int prof = 0)
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarl@ " + row["usuarioFaceBook"] + ", de parte del Equipo de BidCargo...";
        if (prof == 0)
        {
            strBody += "<br>Lamentamos informarle que su perfil en nuestra plataforma FUE DESACTIVADO.";
        }
        else
        {
            strBody += "<br>Nos complace informarle que su perfil en nuestra plataforma fue ACTIVADO, y se ENCUENTRA EN REVISION,";
        }
        strBody += "<br>Por favor, contacte con nuestros administradores, disculpe las molestias ocasionadas";
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }
    
    public string ArmarCorreoRecuperacionContrasena(string nombreUsuario, string contrasena, int genero, string celular, string urlpath, bool propietario)
    {
        string strBody = "<HTML>";
        strBody += "<head><style type=\"text/css\">.curpointer {cursor: pointer;}</style></head> ";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Estimad@ " + nombreUsuario + ", <br>";
        strBody += "<br> Si has solicitado una actualización de contraseña, realiza clic en el siguiente botón.";
        strBody += "<br><br> Si no has realizado la solicitud, ignora este email";
        strBody += "<br><br> Tu contraseña temporal es: " + contrasena + "<br><br>";
        if (propietario)
        {
            strBody += "<a href='" + urlpath + "/Home/ActualizarContrasenaUsuarios?id={1}'><button class=\"curpointer\" style=\"background:#0219ba;border-radius:5px;padding:15px 23px;color:#ffffff;display:inline-block;font:normal bold 30px \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\">Cambiar Contraseña</button></a> ";
        }
        else
        {
            strBody += "<a href='" + urlpath + "/Home/actualizarContrasena?id={1}'><button class=\"curpointer\" style=\"background:#0219ba;border-radius:5px;padding:15px 23px;color:#ffffff;display:inline-block;font:normal bold 30px \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\">Cambiar Contraseña</button></a> ";
        }
        strBody += "<br><br>Gracias,";
        strBody += "<br><br>BidCargo equipo de soporte";
        strBody += "<br><br><img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        strBody = strBody.Replace("{1}", celular);
        return strBody;
    }

    public string ArmarCorreoElectronico(string nombreUsuario, int genero, string celular, string tema)
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Estimado(a) " + nombreUsuario + " <br><br> Muchas grácias por comunicarse con nosotros";
        strBody += "<br><br> Por medio del presente, deseo confirmarle que una persona de nuestro equipo se comunicará con usted";
        strBody += "<br> para conocer así sus necesidades";
        strBody += "<br> El número al que lo llamaremos es: " + celular;
        strBody += "<br> El tema del que quieres saber es: " + tema;
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";   
        return strBody;
    }

    public string ArmarCorreoElectronicoPrimerContacto(string name, string LastName, string contrasena, string usuario, string celular, string urlpath)
    {
        string strBody = "<html xmlns='http://www.w3.org/1999/xhtml' xmlns:o='urn:schemas-microsoft-com:office:office' style='width:100%;font-family:arial, 'helvetica neue', helvetica, sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0'>";
        strBody += "<head>";
        strBody += "<meta charset='UTF-8'>";
        strBody += "<meta content='width=device-width, initial-scale=1' name='viewport'>";
        strBody += "<meta name='x-apple-disable-message-reformatting'>";
        strBody += "<meta http-equiv='X-UA-Compatible' content='IE = edge'>";
        strBody += "<meta content='telephone=no' name='format-detection'>";
        strBody += "<style type='text/css'>";
        strBody += "@media only screen and (max-width:600px) {p, ul li, ol li, a { font-size:14px!important; line-height:150%!important } h1 { font-size:30px!important; text-align:center; line-height:120%!important } h2 { font-size:26px!important; text-align:center; line-height:120%!important } h3 { font-size:20px!important; text-align:center; line-height:120%!important } h1 a { font-size:30px!important } h2 a { font-size:26px!important } h3 a { font-size:20px!important } .es-menu td a { font-size:14px!important } .es-header-body p, .es-header-body ul li, .es-header-body ol li, .es-header-body a { font-size:14px!important } .es-footer-body p, .es-footer-body ul li, .es-footer-body ol li, .es-footer-body a { font-size:14px!important } .es-infoblock p, .es-infoblock ul li, .es-infoblock ol li, .es-infoblock a { font-size:12px!important } *[class='gmail - fix'] { display:none!important } .es-m-txt-c, .es-m-txt-c h1, .es-m-txt-c h2, .es-m-txt-c h3 { text-align:center!important } .es-m-txt-r, .es-m-txt-r h1, .es-m-txt-r h2, .es-m-txt-r h3 { text-align:right!important } .es-m-txt-l, .es-m-txt-l h1, .es-m-txt-l h2, .es-m-txt-l h3 { text-align:left!important } .es-m-txt-r img, .es-m-txt-c img, .es-m-txt-l img { display:inline!important } .es-button-border { display:block!important } a.es-button { font-size:20px!important; display:block!important; border-left-width:0px!important; border-right-width:0px!important } .es-btn-fw { border-width:10px 0px!important; text-align:center!important } .es-adaptive table, .es-btn-fw, .es-btn-fw-brdr, .es-left, .es-right { width:100%!important } .es-content table, .es-header table, .es-footer table, .es-content, .es-footer, .es-header { width:100%!important; max-width:600px!important } .es-adapt-td { display:block!important; width:100%!important } .adapt-img { width:100%!important; height:auto!important } .es-m-p0 { padding:0px!important } .es-m-p0r { padding-right:0px!important } .es-m-p0l { padding-left:0px!important } .es-m-p0t { padding-top:0px!important } .es-m-p0b { padding-bottom:0!important } .es-m-p20b { padding-bottom:20px!important } .es-mobile-hidden, .es-hidden { display:none!important } tr.es-desk-hidden, td.es-desk-hidden, table.es-desk-hidden { display:table-row!important; width:auto!important; overflow:visible!important; float:none!important; max-height:inherit!important; line-height:inherit!important } .es-desk-menu-hidden { display:table-cell!important } table.es-table-not-adapt, .esd-block-html table { width:auto!important } table.es-social { display:inline-block!important } table.es-social td { display:inline-block!important } }";
        strBody += ".rollover div { font-size:0;}";
        strBody += ".rollover:hover .rollover-first { max-height:0px!important;	display:none!important;}";
        strBody += ".rollover:hover .rollover-second { max-height:none!important; display:inline-block!important;}";
        strBody += "#outlook a { padding:0; }";
        strBody += ".ExternalClass { width:100%; }.ExternalClass,.ExternalClass p,.ExternalClass span,.ExternalClass font,.ExternalClass td,.ExternalClass div { line-height:100%;}";
        strBody += ".es-button { mso-style-priority:100!important; text-decoration:none!important;}";
        strBody += "a[x-apple-data-detectors] { color:inherit!important;text-decoration:none!important;font-size:inherit!important;font-family:inherit!important;font-weight:inherit!important;line-height:inherit!important;}";

        strBody += ".es-desk-hidden { display:none;float:left;overflow:hidden;width:0;max-height:0;line-height:0;mso-hide:all;}";
        strBody += ".es-button-border:hover a.es-button {background:#cca300!important;border-color:#cca300!important;}";
        strBody += "</style>";
        strBody += "</head>";
        strBody += "<body>";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Bienvenido " + name + " " + LastName + " A la plataforma de BidCargo,plataforma comprometida con tu crecimiento empresarial.";
        strBody += "<br> Has iniciado el proceso de pre-registro en la plataforma colaborativa de BidCargo; para continuar con el proceso de inscripción definitivo ";
        strBody += "te estamos enviando el usuario y la contraseña temporal. Al ingresar nuevamente, la plataforma te solicitará que cambies tu Clave. Si tienes algún inconveniente, por favor contáctanos a través del correo contacto@bidcargo.com.co";
        strBody += "<br> USUARIO: " + usuario;
        strBody += "<br> CONTRASEÑA: " + contrasena + "<br><br>";
        strBody += "<a href='" + urlpath + "?id={1}'><button type='button' style='background:#0219ba;border-radius:5px;padding:15px 23px;color:#ffffff;display:inline-block;font:normal bold 30px, sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;'>Activar Cuenta</button></a> ";
        strBody += "<br><br>Cordialmente Servicio al cliente BidCargo";
        strBody += "<br><br><img src='http://www.bidcargo.com.co/Content/images/logoLetras.png' width='428' height='78' alt='logo'>";
        strBody += "</label>";
        strBody += "</body>";
        strBody += "</html>";
        strBody = strBody.Replace("{1}", celular);
        return strBody;

    }

    //public string ArmarCorreoElectronicoPrimerContacto(string name, string LastName, string contrasena, string usuario, string celular, string urlpath)
    //{
    //    string strBody = "<HTML>";
    //    strBody += "<Body> ";
    //    strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
    //    strBody += "<br> Bienvenido " + name + " " + LastName + " A la plataforma de BidCargo, <br><br>plataforma comprometida con tu crecimiento empresarial.";
    //    strBody += "<br> Has iniciado el proceso de pre-registro en la plataforma colaborativa de BidCargo; para continuar con el proceso de inscripción definitivo ";
    //    strBody += "te estamos enviando el usuario y la contraseña temporal. Al ingresar nuevamente, la plataforma te solicitará que cambies tu Clave. Si tienes algún inconveniente, por favor contáctanos a través del correo contacto@bidcargo.com.co";
    //    strBody += "<br> USUARIO: " + usuario;
    //    strBody += "<br> CONTRASEÑA: " + contrasena + "<br><br>";
    //    strBody += "<a href='"+urlpath+"/Home/actualizarContrasena?id={1}'><button class=\"curpointer\" style=\"background:#0219ba;border-radius:5px;padding:15px 23px;color:#ffffff;display:inline-block;font:normal bold 30px \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\">Activar Cuenta</button></a> ";
    //    strBody += "<br><br>Cordialmente Servicio al cliente BidCargo";
    //    strBody += "<br><br><img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
    //    strBody += "</label>";
    //    strBody += "</Body>";
    //    strBody += "</HTML>";
    //    strBody = strBody.Replace("{1}", celular);
    //    return strBody;


    //}
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJLMNOPQRSTUVWXYZabcdefghijlmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    public string sendOfferMail( System.Data.DataRowCollection model, string apellidoPaterno, string urlpath)
    {
        string strBody = "<HTML>";
        strBody += "<head>";
        ////strBody += "<style type=\"text/css\">";
        //strBody += "h4 {display: block;margin-block-start: 1.33em;margin-block-end: 1.33em;margin-inline-start: 0px;margin-inline-end: 0px;font-weight: bold;font-size: 1.5rem;}";
        //strBody += "</style>";
        strBody += "</head>";
        strBody += "<Body> ";
        strBody += "<label> Informacion </label><br>";
        strBody += "<label><strong> Cliente: </strong>" + apellidoPaterno + " </label><br>";
        strBody += "----------------------------------------------------<br/>";
        foreach (System.Data.DataRow models in model)
        {
            strBody += "<label><strong> Trayecto Solicitado: </strong>" + models["fromDepartament"] + " - " + models["fromCity"] + " <strong>hacia</strong> " + models["toDepartament"] + " - " + models["toCity"] + " </label><br>";
            strBody += "<label><strong> Direccion Remitente: </strong>" + models["directionFrom"] + "</label><br>";
            strBody += "<label><strong> Direccion Destinatario: </strong>" + models["directionTo"] + "</label><br>";
            strBody += "<label><strong> Transportar: </strong>" + models["typeContainer"] + ", " + models["typeMerchandise"] + " en " + models["typeCargoString"] + " </label><br>";
            if (Convert.ToInt32(models["typeCargo"]) == 5 || Convert.ToInt32(models["typeCargo"]) == 4 || Convert.ToInt32(models["typeCargo"]) == 2 || Convert.ToInt32(models["typeCargo"]) == 1)
            {
                strBody += "<label><strong> Peso: </strong>" + Convert.ToString(models["weightContainer"]) + " <strong>" + models["typeMeasuredString"] + "</strong></label><br>";
            }
            else {
                strBody += "<label><strong> Peso: </strong>" + models["numberUnitsTons"] + " <strong>  " + "Toneladas" + "</strong></label><br>";
            }

            strBody += "<label><strong> Dias de pago: </strong>" + models["payDays"] + "</label><br>";
            if (Convert.ToInt32(models["typeCargo"]) == 2 || Convert.ToInt32(models["typeCargo"]) == 3 || Convert.ToInt32(models["typeCargo"]) == 4)
            {
                strBody += "<label><strong>Medidas: </strong> <strong>Largo: </strong>"+ models["longTied"] +", <strong>Ancho: </strong> "+ models["widthPlates"]+", <strong>Alto: </strong>"+ models["highLoose"]+ " en <strong>" + models["typeDimensionString"] +"</strong></label><br>";
                strBody += "<label><strong>Número: </strong>" + models["numberUnitsTons"]+"</label><br>";
            }//numberUnitsTons

            //if(Convert.ToInt32(models["typeCargo"]) >= 3)
            //{
            //    strBody += "<label><strong>Presentación: </strong> <strong>Atado: </strong>" + models["longTied"] + ", <strong>Planchas: </strong>" + models["widthPlates"] + ", <strong>Suelta: </strong>" + models["highLoose"] + "</label><br>";
            //}
            strBody += "<label><strong> Valor por Contenedor:</strong> " + String.Format("{0:C0}", Convert.ToInt32(models["valueMerchandise"])) + " </label><br>";
            strBody += "<label><strong> Fecha de Viaje:</strong> " + models["departure"] + "</label><br>";
            strBody += "<label><strong>Fecha Estimada de Arribo</strong>: " + models["arrival"] + "<strong>" +  models["DateOfServiceIText"] + "</strong>" + " </label>  <br>";
            strBody += "<label><strong> Observacion:</strong> " + models["observation"] + " </label><br>";
            strBody += "----------------------------------------------------<br/>";

        }
        //*/
        strBody += "    <label> Presiona el boton para ir a nuestros servicios</label></br> ";
        strBody += "<a href='"+ urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br>Cordialmente Servicio al cliente BidCargo";
        strBody += "<br><br><img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"249\" height=\"69\" alt=\"logo\">";
        strBody += "</Body>";
        strBody += "</HTML>";
        //strBody = strBody.Replace("{1}", celular);
        return strBody;
    }
    public string sendOfferMailTypeCompany(int pidclient,System.Data.DataRowCollection model, string apellidoPaterno, int pidTypeCompany, string urlpath)
    {
        string strBody = "<HTML>";
        strBody += "<head>";
        ////strBody += "<style type=\"text/css\">";
        //strBody += "h4 {display: block;margin-block-start: 1.33em;margin-block-end: 1.33em;margin-inline-start: 0px;margin-inline-end: 0px;font-weight: bold;font-size: 1.5rem;}";
        //strBody += "</style>";
        strBody += "</head>";
        strBody += "<Body> ";
        strBody += "<label> Informacion </label><br>";
        strBody += "<label><strong> Cliente: </strong>" + apellidoPaterno + " </label><br>";
        strBody += "----------------------------------------------------<br/>";
        foreach (dynamic models in model)
        {
            strBody += "<label><strong> Trayecto Solicitado: </strong>" + models["fromDepartament"] + " - " + models["fromCity"] + " <strong>hacia</strong> " + models["toDepartament"] + " - " + models["toCity"] + " </label><br>";
            strBody += "<label><strong> Direccion Remitente: </strong>" + models["directionFrom"] + "</label><br>";
            strBody += "<label><strong> Direccion Destinatario: </strong>" + models["directionTo"] + "</label><br>";
            strBody += "<label><strong> Transportar: </strong>" + models["typeContainer"] + ", " + models["typeMerchandise"] + " en " + models["typeCargoString"] + " </label><br>";
            if (Convert.ToInt32(models["typeCargo"]) == 5 || Convert.ToInt32(models["typeCargo"]) == 4 || Convert.ToInt32(models["typeCargo"]) == 2 || Convert.ToInt32(models["typeCargo"]) == 1)
            {
                strBody += "<label><strong> Peso: </strong>" + Convert.ToString(models["weightContainer"]) + " <strong>" + models["typeMeasuredString"] + "</strong></label><br>";
            }
            else
            {
                strBody += "<label><strong> Peso: </strong>" + models["numberUnitsTons"] + " <strong>  " + "Toneladas" + "</strong></label><br>";
            }



            strBody += "<label><strong> Dias de pago: </strong>" + models["payDays"] + "</label><br>";
            if (Convert.ToInt32(models["typeCargo"]) == 2 || Convert.ToInt32(models["typeCargo"]) == 4)
            {
                strBody += "<label><strong>Medidas: </strong> <strong>Largo: </strong>" + models["longTied"] + ", <strong>Ancho: </strong> " + models["widthPlates"] + ", <strong>Alto: </strong>" + models["highLoose"] + " en <strong>" + models["typeDimensionString"] + "</strong></label><br>";
                strBody += "<label><strong>Número Unidades: </strong>" + models["numberUnitsTons"] + "</label><br>";
            }

            //if (Convert.ToInt32(models["typeCargo"]) >= 3)
            //{
            //    strBody += "<label><strong>Presentación: </strong> <strong>Atado: </strong>" + models["longTied"] + ", <strong>Planchas: </strong>" + models["widthPlates"] + ", <strong>Suelta: </strong>" + models["highLoose"] + " en <strong>" + models["typeDimensionString"] + "</strong></label><br>";
            //}
            strBody += "<label><strong> Valor Declarado Mercancia:</strong> " + models["CoinTypeText"] + " " + String.Format("{0:C0}", Convert.ToInt32(models["valueMerchandise"])) + " </label><br>";
            if(pidTypeCompany == 2)
                strBody += "<label><strong> Valor de la Oferta:</strong> " + String.Format("{0:C0}", models["valorOferta"]) + "</label><br>";
            strBody += "<label><strong> Fecha de Viaje:</strong> " + models["departure"] +  "- <strong>" + models["DateOfServiceIText"] +  "</strong>" +  "</label><br>";
            strBody += "<label><strong>Fecha Estimada de Arribo</strong>: " + models["arrival"] + " </label><br>";
            strBody += "<label><strong> Observacion:</strong> " + models["observation"] + " </label><br>";
            strBody += "----------------------------------------------------<br/>";

        }

        dynamic roww = model[0];
        string variable =   RandomString(5) + "k"+ pidclient.ToString()+"k"+ 
                            RandomString(5)+"k"+roww["codeOffer"].ToString()+"k"+ 
                            RandomString(5) + "k" + pidTypeCompany.ToString() + "k" + 
                            RandomString(5);
        //*/
        strBody += "<label> Presiona el boton para ir a la pagina si quieres ofertar</label><br><br> ";
        strBody += "<a href='"+ urlpath+"/Home/OffersToken/" + variable + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\">Aceptar</button></a> ";
        strBody += "<br><br>Cordialmente Servicio al cliente BidCargo";
        strBody += "<br><br><img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png \" width= \"249\" height=\"69\" alt=\"logo\">";
        strBody += "</Body>";
        strBody += "</HTML>";
        //strBody = strBody.Replace("{1}", celular);
        return strBody;
    }

    public void EnviarCorreoPropietarios(DataTable dd, string asuntoDelCorreo, string copiaCorreoEnvio, string textoDelCorreo, string correoDesdeElQueEnvio, string usuarioCorreEnvio, string contrasenaCorreoEnvio, string archivo)
    {             /*-------------------------MENSAJE DE CORREO----------------------*/
        /*-------------------------MENSAJE DE CORREO----------------------*/
        System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
        //Direccion de correo electronico a la que queremos enviar el mensaje
        DataRow dataRow;

        for (int i = 0; i < dd.Rows.Count; i++)
        {
            dataRow = dd.Rows[i];
            mmsg.Bcc.Add(new MailAddress(dataRow["correo"].ToString()));
        }
        //Asunto
        mmsg.Subject = asuntoDelCorreo;
        mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
        //Cuerpo del Mensaje
        mmsg.Body = textoDelCorreo;

        //Cargar Imagen
        //string htmlBody = "";
        //htmlBody = htmlBody + "<img src=\"cid:closetEscritorio\">" + Environment.NewLine;
        //mmsg.IsBodyHtml = true;
        //mmsg.Body = htmlBody;
        //AlternateView htmlview = default(AlternateView);
        //htmlview = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");
        //MemoryStream logo = new MemoryStream();
        //LinkedResource imageResourceEs = new LinkedResource(HttpContext.Current.Server.MapPath("~//Images//" + "closetEscritorio.jpg"));
        //imageResourceEs.ContentId = "photo";
        //imageResourceEs.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
        //htmlview.LinkedResources.Add(imageResourceEs);
        //mmsg.AlternateViews.Add(htmlview);

        mmsg.BodyEncoding = System.Text.Encoding.UTF8;
        mmsg.IsBodyHtml = false;
        if (archivo.Length > 2) mmsg.Attachments.Add(new Attachment(archivo));
        //Correo electronico desde la que enviamos el mensaje
        mmsg.From = new System.Net.Mail.MailAddress(correoDesdeElQueEnvio);

        /*-------------------------CLIENTE DE CORREO----------------------*/
        System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
        //Hay que crear las credenciales del correo emisor
        cliente.Credentials = new System.Net.NetworkCredential(usuarioCorreEnvio, contrasenaCorreoEnvio);
        //Lo siguiente es obligatorio si enviamos el mensaje desde Gmail
        cliente.Port = 587;
        cliente.EnableSsl = true;
        cliente.Host = "smtp.office365.com";
        //cliente.Host = "smtp.gmail.com";
        cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
        cliente.Timeout = 5000;
        mmsg.IsBodyHtml = true;
        try
        {
            cliente.Send(mmsg);
        }
        catch (System.Net.Mail.SmtpException)
        {
        }
    }

    public string ArmarCorreoNuevaOferta(BidCargo_.Models.storeoffer model)
    {
        string stringBody = @"
                <html>
                    <body> 
                        <h2> Amig@ propietari@ </h2>  
                        <div style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>
                            <p>Un placer saludarl@, hay una nueva oferta disponible en nuestra plataforma.</p>
                            <p>Le invitamos a ingresar a la plataforma para que pueda revisarla.</p>
                            <p>Codido Oferta:"+model.codeOffer + @"</p>
                            <p>Fecha salida:" + model.dayTraveler + @"</p>
                            <p>Fecha llegada:" + model.dayTraveler2 + @"</p>
                            <p>Costo Oferta:" + model.valorOferta + @"</p>
                            <p>Tipo de mercancia:" + model.typeMerchandise + @"</p>
                            
                            <p>Saludos...</p>
                        </div>
                    </body>
                </html>
            ";

        return stringBody;
    }



    //  NOTIFICACIONES PARA PROPIETARIO , VEHICULOS  Y CONDUCTORES 

    public string ArmarCorreoPropietarioAprobado(string NombrePropietario, string urlpath = "", int prof = 0  )
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarl@ "+ NombrePropietario + @", de parte del Equipo de BidCargo...";
        if (prof == 0)
        {
            strBody += "<br>Lamentamos informarle que su perfil en nuestra plataforma NO FUE APROBADO, y NO PUEDE INGRESAR a la misma.";
            strBody += "<br>Por favor, contacte con nuestros administradores, disculpe las molestias ocasionadas";
        }
        else
        {
            strBody += "<br>Nos complace informarle que su perfil en nuestra plataforma fue APROBADO, y ya PUEDE INGRESAR a la misma,";
            strBody += "<br>para disfrutar de nuestros servicios";
        }
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }


    public string ArmarCorreoPropietarioActivado(string NombrePropietario, string urlpath = "", int prof = 0)
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarl@ " + NombrePropietario + ", de parte del Equipo de BidCargo...";
        if (prof == 0)
        {
            strBody += "<br>Lamentamos informarle que su perfil en nuestra plataforma FUE DESACTIVADO.";
        }
        else
        {
            strBody += "<br>Nos complace informarle que su perfil en nuestra plataforma fue ACTIVADO, y se ENCUENTRA EN REVISION,";
        }
        strBody += "<br>Por favor, contacte con nuestros administradores, disculpe las molestias ocasionadas";
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }
    public string ArmarCorreoVehiculoAprobado(string usuario, string urlpath = "", int prof = 0)
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarl@ " + usuario+ ", de parte del Equipo de BidCargo...";
        if (prof == 0)
        {
            strBody += "<br>Lamentamos informarle que su Vehiculo en nuestra plataforma NO FUE APROBADO, y NO PUEDE INGRESAR a la misma.";
            strBody += "<br>Por favor, contacte con nuestros administradores, disculpe las molestias ocasionadas";
        }
        else
        {
            strBody += "<br>Nos complace informarle que su Vehiculo en nuestra plataforma fue APROBADO, y ya PUEDE INGRESAR a la misma,";
            strBody += "<br>para disfrutar de nuestros servicios";
        }
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }
    public string ArmarCorreoVehiculoActivado(string usuario, string urlpath = "", int prof = 0)
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarl@ " + usuario + ", de parte del Equipo de BidCargo...";
        if (prof == 0)
        {
            strBody += "<br>Lamentamos informarle que su Vehiculo en nuestra plataforma FUE DESACTIVADO.";
        }
        else
        {
            strBody += "<br>Nos complace informarle que su Vehiculo en nuestra plataforma fue ACTIVADO, y se ENCUENTRA EN REVISION,";
        }
        strBody += "<br>Por favor, contacte con nuestros administradores, disculpe las molestias ocasionadas";
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }
    public string ArmarCorreoConductorAprobado(string nombre, string urlpath = "", int prof = 0)
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarl@ " + nombre + ", de parte del Equipo de BidCargo...";
        if (prof == 0)
        {
            strBody += "<br>Lamentamos informarle que conductor en nuestra plataforma NO FUE APROBADO, y NO PUEDE INGRESAR a la misma.";
            strBody += "<br>Por favor, contacte con nuestros administradores, disculpe las molestias ocasionadas";
        }
        else
        {
            strBody += "<br>Nos complace informarle que su conductor en nuestra plataforma fue APROBADO, y ya PUEDE INGRESAR a la misma,";
            strBody += "<br>para disfrutar de nuestros servicios";
        }
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }
    public string ArmarCorreoConductorActivado(string nombre , string urlpath = "", int prof = 0)
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarl@ " + nombre + ", de parte del Equipo de BidCargo...";
        if (prof == 0)
        {
            strBody += "<br>Lamentamos informarle que su conductor en nuestra plataforma FUE DESACTIVADO.";
        }
        else
        {
            strBody += "<br>Nos complace informarle que su conductor en nuestra plataforma fue ACTIVADO, y se ENCUENTRA EN REVISION,";
        }
        strBody += "<br>Por favor, contacte con nuestros administradores, disculpe las molestias ocasionadas";
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }
    public string correoAdministrativos(string Nombre , string cedula , string tipo)
    {
        if (tipo == "C")
        {
            string stringBody = @"
            <html>
                <body> 
                    <h2> Equipo de BidCargo </h2>  
                    <div style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>
            
                        <p>Administrativo bidcargo...</p>
                        <p>Nos complace informarle que los documentos del  Conductor: @ " + Nombre + @" , de  licencia: @"+ cedula+ @" se encuentran en su plataforma,
                        revisar la documentacion pertinente</p>
                         
                        <p>Saludos...</p>
                    </div>
                </body>
            </html>";
            return stringBody;


        }
        else 
        {
            string stringBody = @"
            <html>
                <body> 
                    <h2> Equipo de BidCargo </h2>  
                    <div style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>
            
                        <p>Administrativo bidcargo...</p>
                        <p>Nos complace informarle que los documentos del Vehiculo: @ " + Nombre + @" se encuentran en su plataforma,
                        revisar la documentacion pertinente</p>
                         
                        <p>Saludos...</p>
                    </div>
                </body>
            </html>";
            return stringBody;


        }
       
    }


    public string ArmarCorreoPapelesConductor(string Nombre , string licencia)
    {
        string stringBody = @"
                <html>
                    <body> 
                        <h2> Equipo de BidCargo </h2>  
                        <div style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>

                            <p>El Conductor " + Nombre + @"  identificado con el numero de licencia: "+ licencia+ 
                            @"subió documentos satisfactoriamente ya puede acceder y verificar su información y los documentos cargados</p>

                            <p>Buen trabajo...</p>
                        </div>
                    </body>
                </html>
            ";

        return stringBody;
    }




    public string ArmarCorreoPapelesVehiculo( string placa)
    {
        string stringBody = @"
                <html>
                    <body> 
                        <h2> Equipo de BidCargo </h2>  
                        <div style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>

                            <p>El Vehiculo  con placa:" + placa + @" 
                            tiene cargados los docuementos 
                            satisfactoriamente  ya puede acceder y verificar su información y los documentos cargados</p>

                            <p>Buen trabajo...</p>
                        </div>
                    </body>
                </html>
            ";

        return stringBody;
    }

    public string sendMailAccepOffer2(dynamic row = null, dynamic row2 = null, string urlpath = "")
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarl@ " + row["usuarioFaceBook"] + ", de parte del Equipo de BidCargo...";

        strBody += "<br>Nos complace informarle, que la oferta que realizo a la Cotizacion " + row["codeOffer"] + ", ha sido aceptada.";
        strBody += "<br>Le invitamos a ingresar a nuestra plataforma para conocer mas detalles, los datos de contacto de la empresa contratante son los siguientes:";
        strBody += "<br>Empresa Contratante: " + row2["usuarioFaceBook"] + "";
        strBody += "<br>Dirección: " + row2["direccion"] + "";
        strBody += "<br>Persona de Contacto: " + row2["nombre"] + "  " + row2["apellidoPaterno"] + "";
        strBody += "<br>Telefono de Contacto: " + row2["numeroCelular"] + "";
        strBody += "<br>Email de Contacto: " + row2["email"] + "";
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }


    public string sendMailNOAccepOffer2(dynamic row = null,  string urlpath = "")
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";

        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br>Un placer saludarlo " + row["usuarioFaceBook"] + ", de parte del equipo de Bidcargo,";

        strBody += "<br>Estimados señores " + row["usuarioFaceBook"] + ": Le notificamos que la solicitud de carga  " + row["codeOffer"] + ", ha sido asignada a otra empresa;";
        strBody += "<br>le invitamos a seguir ofertando sus servicios para una nueva solicitud.";

        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Equipo de Atención al Cliente BidCargo</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }




    public string sendMailAccepOffer3(dynamic row = null, dynamic row2 = null, string urlpath = "")
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarl@ " + row["nombreUsuario"] + ", de parte del Equipo de BidCargo...";

        strBody += "<br>Nos complace informarle, que la oferta que realizo a la Cotizacion " + row["codeOffer"] + ", ha sido aceptada.";
        strBody += "<br>Le invitamos a ingresar a nuestra plataforma para conocer mas detalles, los datos de contacto de la empresa contratante son los siguientes:";
        strBody += "<br>Empresa Contratante: " + row2["usuarioFaceBook"] + "";
        strBody += "<br>Dirección: " + row2["direccion"] + "";
        strBody += "<br>Persona de Contacto: " + row2["nombre"] + "  " + row2["apellidoPaterno"] + "";
        strBody += "<br>Telefono de Contacto: " + row2["numeroCelular"] + "";
        strBody += "<br>Email de Contacto: " + row2["email"] + "";
        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }


    public string sendMailNOAccepOffer3(dynamic row = null, string urlpath = "")
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";

        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br>Un placer saludarlo " + row["nombreUsuario"] + ", de parte del equipo de Bidcargo,";

        strBody += "<br>Estimados señores " + row["nombreUsuario"] + ": Le notificamos que la solicitud de carga, ha sido asignada a otra empresa;";
        strBody += "<br>le invitamos a seguir ofertando sus servicios para una nueva solicitud.";

        strBody += "<br><br><a href='" + urlpath + "'><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Equipo de Atención al Cliente BidCargo</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }





  
    public string CarguePN(PropietarioViewModel propietarioViewModel)
    {
        string strBody = "<HTML>";
        strBody += "<Body> ";
        strBody += "<label style='font-family: Arial, icomoon, sans-serif; font-size: 12px; color: #1F1F1F'>";
        strBody += "<br> Equipo de BidCargo <br>";
        strBody += "<br>Un placer saludarl@ admisitrativo de BIDCARGO ";

        strBody += "<br>TEMA: Nueva documentacion";
        strBody += "<br> Un nuevo propietario de tipo:"+ propietarioViewModel.tipoUsuario;
        strBody += "<br> Nombre  Propietario:"+ propietarioViewModel.nombres;
        strBody += "<br>Apellido Propietario:"+ propietarioViewModel.apellidos;
        strBody += "<br>Cedula   Propietario:"+ propietarioViewModel.cedula;
        strBody += "<br>Telefono Propietario:"+ propietarioViewModel.telefono;
        strBody += "<br>Correo   Propietario:"+ propietarioViewModel.correo;
        strBody += "<br><br><a href=''><button class=\"curpointer\" style=\"cursor:pointer;background:#42a098;border-radius:5px;padding:15px 23px;color:#ffffff;" +
            "           display:inline-block;font:normal bold 30px/1 \"Calibri\", sans-serif;text-align:center;text-shadow:1px 1px #000000;cursor:pointer;\"> BidCargo </button></a> ";
        strBody += "<br><br><strong><span style=\"color:#000000\">Coordialmente</span></br></strong>";
        strBody += "<span style =\"color:#000000\">Servicio Al Cliente</span> </br></br>";
        strBody += "<br><br>";
        strBody += "<img src=\"http://www.bidcargo.com.co/Content/images/logoLetras.png\" width= \"428\" height=\"78\" alt=\"logo\">";
        strBody += "<br><br>";
        strBody += "<span style=\"color:#00ccff\">Antes de imprimir este correo electrónico, piense bien si es necesario hacerlo: El medio ambiente es cuestión de todos. Si decide imprimirlo, piense si es necesario hacerlo en color: el consumo de tinta o tóner será&nbsp;mucho mayor. Si decide imprimirlo en color, piense si necesita imprimir todo el documento o sólo una parte.</span > ";
        strBody += "</label>";
        strBody += "</Body>";
        strBody += "</HTML>";
        return strBody;
    }









}
