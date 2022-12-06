using Services.ViewModels;
using System;
using System.Collections.Generic;

namespace Services.ScrapServices
{
    public class Sc01Service
    {
        public Sc01Service()
        {
            //EnviarYRecibirGet();

            EnviarYRecibirPost();

        }
        public Sc01Service(string xpath, string url)
        {
            EnviarYRecibirGet(xpath, url);
        }

        private bool EnviarYRecibirGet()
        {
            string url1 = "https://www.miro.es/electrodomesticos/frigorificos-y-congeladores/frigorifico-combi/combi-bosch-kgn39awep.html";
            string xpath1 = "//span[@class='regular-price']//span[@class='price']";
            string url2 = "https://www.euronics.es/acer-aspire-3-a315-34-portatil-396-cm-156-full-hd-intel-celeron-8-gb-ddr4-sdram-256-gb-ssd-wi-fi-5-80211ac-windows-11.html";
            string xpath2 = "//span[@class='regular-price']//span[@class='price']";
            string url3 = "https://www.bazarelregalo.com/imagen-y-sonido-1/altavoces-bluetooth/altavoz-portatil-sony-srs-xb23-coral-bluetooth.html";
            string xpath3 = "//span[@class='regular-price']//span[@class='price']";
            url1 = "https://www.miro.es/electrodomesticos/frigorificos-y-congeladores/frigorifico-combi/combi-bosch-kgn39awep.html";
            xpath1 = "//span[@class='regular-price']//span[@class='price']";
            url2 = "https://www.euronics.es/samsung-qe55q65bauxxc-televisor.html";
            xpath2 = "//div[@class='sale-price']//p[@class='price']";
            url3 = "https://www.bazarelregalo.com/destacados/lavadora-hoover-hwp610ambc1s.html";
            xpath3 = "//span[@class='regular-price']//span[@class='price']";

            string valorBuscado = null;

            valorBuscado = EnviarYRecibirGet(xpath1, url1);

            Console.WriteLine(valorBuscado);

            valorBuscado = EnviarYRecibirGet(xpath2, url2);

            Console.WriteLine(valorBuscado);

            valorBuscado = EnviarYRecibirGet(xpath3, url3);

            Console.WriteLine(valorBuscado);

            return true;
        }

        private string EnviarYRecibirGet(string xpath, string url)
        {
            ScSendReceiveGetService service = new ScSendReceiveGetService();
            string valorBuscado = service.EnviarYRecibirGet(xpath, url);

            return valorBuscado;
        }

        private bool EnviarYRecibirPost()
        {
            ScSendReceivePostService service = new ScSendReceivePostService();
            ObjetoGordoViewModel objeto = null;

            ////scrapeamos: https://store.fcbarcelona.com/es/login

            //objeto = new ObjetoGordoViewModel();
            //objeto.url01 = "https://store.fcbarcelona.com/es/login";
            //objeto.url02 = "https://store.fcbarcelona.com/es/j_spring_security_check";
            ////objeto.request_Method = "GET";
            //objeto.request_ContentType = "text/html;charset=UTF-8";
            //objeto.request_Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            //objeto.request_UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
            //objeto.request_AllowAutoRedirect = true;
            ////request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            ////request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            ////request.AllowWriteStreamBuffering = true;
            //objeto.requestGuardado = null;
            //objeto.nomParam01 = "j_username";
            //objeto.valueParam01 = "01@rom.cat";
            //objeto.nomParam02 = "j_password";
            //objeto.valueParam02 = "01@R0m.c4t";
            //objeto.parametros = "";
            //objeto.responseHtml01 = "";
            //objeto.responseHtml02 = "";
            ////<input type="hidden" name="CSRFToken" value="359f0ce9-08b3-4c7e-88cb-727b035e1939" />
            //objeto.xpathToken = "//form[@id='lang-form-mobile']/div";// No funciona, no sé porqué"//form[@id='lang-form-mobile']//input[@type='hidden'][@name='CSRFToken']/@value";
            //objeto.attributeToken = "";
            //objeto.nomParamToken = "CSRFToken";
            //objeto.token = "";
            ////<p class="user-link mobile__menu--link">Bienvenido Paco
            //objeto.xpathValor01 = "//p[@class='user-link mobile__menu--link']";
            //objeto.attributeValor01 = "";
            //objeto.valor01 = "";

            //objeto = service.EnviarYRecibirPost(objeto);

            ////FIIIIIIIIIN


            ////scrapeamos: https://multestransit.gencat.cat/sctPagaments/AppJava/views/expedients/cerca.xhtml?set-locale=es_ES
            ////action="/sctPagaments/AppJava/views/expedients/cerca.xhtml?index=0"
            //objeto = new ObjetoGordoViewModel();
            //objeto.url01 = "https://multestransit.gencat.cat/sctPagaments/AppJava/views/expedients/cerca.xhtml?set-locale=es_ES";
            //objeto.url02 = "https://multestransit.gencat.cat/sctPagaments/AppJava/views/expedients/cerca.xhtml?index=0";
            ////objeto.request_Method = "GET";
            //objeto.request_ContentType = "application/x-www-form-urlencoded";// "text/html;charset=ISO-8859-1";
            //objeto.request_Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            //objeto.request_UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
            //objeto.request_AllowAutoRedirect = true;
            //objeto.requestGuardado = null;
            //objeto.nomParam01 = "";
            //objeto.valueParam01 = "";
            //objeto.nomParam02 = "";
            //objeto.valueParam02 = "";
            //objeto.nomParams = new List<string>(){"formCercaExpedients"
            //        ,"formCercaExpedients:panelCercaExpedients-value"
            //        ,"formCercaExpedients:pc_serveiTerritorial"
            //        ,"formCercaExpedients:pc_numExpedient"
            //        ,"formCercaExpedients:pc_digiControl"
            //        ,"formCercaExpedients:pc_tipusDoc"
            //        ,"formCercaExpedients:pc_tipusDocInput"
            //        ,"formCercaExpedients:pc_numDoc"
            //        ,"formCercaExpedients:j_idt79"};
            //objeto.valueParams = new List<string>(){"formCercaExpedients"
            //        ,"true"
            //        ,"08"
            //        ,"5554760"
            //        ,"4"
            //        ,"CIF"
            //        ,"CIF"
            //        ,"B64666324"
            //        ,"Buscar"};
            //objeto.parametros = "";// "formCercaExpedients=formCercaExpedients&formCercaExpedients:panelCercaExpedients-value=true&formCercaExpedients:pc_serveiTerritorial=08&formCercaExpedients:pc_numExpedient=5554760&formCercaExpedients:pc_digiControl=4&formCercaExpedients:pc_tipusDoc=CIF&formCercaExpedients:pc_tipusDocInput=CIF&formCercaExpedients:pc_numDoc=B64666324&formCercaExpedients:j_idt79=Buscar";
            //objeto.responseHtml01 = "";
            //objeto.responseHtml02 = "";
            //objeto.xpathToken = "//input[contains(@id, 'javax.faces.ViewState')]";
            //objeto.attributeToken = "value";
            //objeto.nomParamToken = "javax.faces.ViewState";//CSRFToken
            //objeto.token = "";
            //objeto.xpathValor01 = "//*[@id='formNoTrobatExpedient:noTrobat_body']/div/fieldset/div[1]/label";
            //objeto.attributeValor01 = "";
            //objeto.valor01 = "";

            //objeto = service.EnviarYRecibirPost(objeto);

            //FIIIIIIIIIN

            //scrapeamos: https://localhost:44378/Site
            //http://www.northwindtuneado.somee.com/
            //action="/sctPagaments/AppJava/views/expedients/cerca.xhtml?index=0"
            //Creamos un nuevo objeto ObjetoGordoViewModel,y lo rellenamos con los datos importantes de este site
            objeto = new ObjetoGordoViewModel();

            objeto.url01 = "http://www.northwindtuneado.somee.com/AspNetUsers/Create"; //para el GET
            objeto.url02 = "http://www.northwindtuneado.somee.com/AspNetUsers/Create"; //para el POST
            //objeto.request_Method = "GET" o "POST";
            objeto.request_ContentType = "application/x-www-form-urlencoded"; //HA DE SER ESTE QUE HEMOS PUESTO //"text/html; charset=utf-8";// "text/html;charset=ISO-8859-1";
            objeto.request_Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            objeto.request_UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
            objeto.request_AllowAutoRedirect = true;
            objeto.requestGuardado = null;
            //objeto.nomParam01 = "Email";
            //objeto.valueParam01 = "1234"; // "angel@rom.cat";
            //objeto.nomParam02 = "PasswordHash";
            //objeto.valueParam02 = "1234"; // "angel@rom.cat";
            objeto.nomParams = new List<string>(){"Email"
                    ,"PasswordHash"};
            //objeto.valueParams = new List<string>(){"angel@rom.cat"
            //        ,"angel@rom.cat"};
            objeto.valueParams = new List<string>(){ "1234"
                    ,"1234"};
            objeto.parametros = ""; //Montaremos un string con todos los parametros
            objeto.responseHtml01 = ""; //Guardaremos el html Recibido en nuestra llamada por GET
            objeto.responseHtml02 = ""; //Guardaremos el html Recibido en nuestra llamada por POST 
            objeto.xpathToken = "//input[@name='__RequestVerificationToken']";
            objeto.attributeToken = "value";
            objeto.nomParamToken = "__RequestVerificationToken";
            objeto.token = "";
            objeto.xpathValor01 = "//table[@class='table']/tr[3]/td[2]";
            objeto.attributeValor01 = "";
            objeto.valor01 = "";

            objeto = service.EnviarYRecibirPost(objeto);

            return true;
        }
    }
}
