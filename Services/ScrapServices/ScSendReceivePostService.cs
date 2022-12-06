using HtmlAgilityPack;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Services.ScrapServices
{
    public class ScSendReceivePostService
    {
        public ObjetoGordoViewModel EnviarYRecibirPost(ObjetoGordoViewModel objeto)
        {   
            //Un solo método:
            objeto = LlamadasAlFormulario2Veces(objeto);

            ////Sencillo
            //objeto = DevuelveObjetoConTodo(objeto);
            ////Complejo Sin Proxy
            //objeto = DevuelveObjetoConTodo(objeto, false);
            ////Complejo Con Proxy
            //objeto = DevuelveObjetoConTodo(objeto, true);

            return objeto;
        }

        //Sencillo
        public ObjetoGordoViewModel LlamadasAlFormulario2Veces(ObjetoGordoViewModel objeto)
        {
            //1) ENVÍAS URL DEL FORMULARIO SIN COOKIE, (PORQUE NO LA TENEMOS) Por GET
            //Y TE DEVUELVE EL FORMULARIO VACÍO + LA COOKIE
            string url = "";
            //1A) CONFIGURACIÓN:
            url = objeto.url01;
            // Creamos una variable WebRequest
            WebRequest wRequest = WebRequest.Create(url);
            wRequest.Timeout = 20000;
            // Parseamos el objeto WebRequest para convertirlo en un HttpWebRequest
            HttpWebRequest request = (HttpWebRequest)wRequest;
            //Le pasamos los parámetros CONSTANTES Que dependen del Site
            request.Method = "GET"; //objeto.request_Method;              
            request.ContentType = objeto.request_ContentType;
            request.Accept = objeto.request_Accept;
            request.UserAgent = objeto.request_UserAgent;
            request.AllowAutoRedirect = objeto.request_AllowAutoRedirect;
            request.CookieContainer = new CookieContainer();

            //1B) ENVÍO:
            //Enviamos el Request y obtenemos el Response:
            //Primero como WebResponse:
            WebResponse wResponse = null;
            HttpWebResponse response = null;
            try
            {
                wResponse = request.GetResponse();
                // Parseamos el objeto WebResponse para convertirlo en un HttpWebResponse
                response = (HttpWebResponse)wResponse;
            }
            catch (Exception e)
            {
                string error = e.ToString();
                Console.WriteLine(error);
                throw;
            }

            //1C) CAPTURAR COOKIES:
            //Tras el envío
            //Ambos reciben cookies: request y response
            CookieContainer container = new CookieContainer();
            CookieCollection cookieCollectionDelRequest = new CookieCollection();
            CookieCollection cookieCollectionDelResponse = new CookieCollection();

            //Podemos capturar las cookies de TRES maneras distintas:
            //1C1) Primera:
            cookieCollectionDelResponse = response.Cookies;
            container = request.CookieContainer;
            cookieCollectionDelRequest = container.GetCookies(request.RequestUri);
            Console.WriteLine("");
            Console.WriteLine("cookies del cookieCollectionDelResponse");
            foreach (Cookie cookie in cookieCollectionDelResponse)
            {
                Console.WriteLine("");
                Console.WriteLine(cookie.ToString());
            }
            Console.WriteLine("");
            Console.WriteLine("cookies del cookieCollectionDelRequest");
            foreach (Cookie cookie in cookieCollectionDelRequest)
            {
                Console.WriteLine("");
                Console.WriteLine(cookie.ToString());
            }

            //1C2) Segunda:
            //string cookieSession = "";
            for (int i = 0; i < response.Headers.Count; ++i)
            {
                string name = response.Headers.GetKey(i);
                string value = response.Headers.Get(i);
                Console.WriteLine(name + ": " + value);
                if (name == "Set-Cookie")
                {
                    Console.WriteLine("cookies del header");
                    IList<string> strCookies = value.Split(new[] { "path=/;" }, StringSplitOptions.None);
                    foreach (var strCookie in strCookies)
                    {
                        Console.WriteLine("");
                        Console.WriteLine(strCookie);
                    }
                }
            }

            //La cookie que nos interesa es la cookie de sesión:
            Console.WriteLine("cookie SESSION");
            string cookieSession = cookieCollectionDelResponse[0].Name + "=" + cookieCollectionDelResponse[0].Value;
            Console.WriteLine(cookieSession);
            Console.WriteLine("cookie TOKEN");
            string cookieToken = 
                //cookieCollectionDelResponse[1].Name + "=" + 
                cookieCollectionDelResponse[1].Value;
            Console.WriteLine(cookieToken);
            //;jsessionid=4B45B7F73C56B2D8FC1515F945D86FE3.nodePROA2?index=0
            //JSESSIONID=9BED2B42102612F3D57061AA642AA2BE.nodePROA2

            //A veces, no podemos capturar las cookies
            //https://medium.com/swlh/7-keys-to-the-mystery-of-a-missing-cookie-fdf22b012f09

            ////1C3) Tercera:
            //Una vez cargado el request,
            //Con todo lo que le hemos metido
            //Y con lo que le ha metido el site, (COOKIES)
            //guardamos el REQUEST ENTERO en nuestro objeto:
            objeto.requestGuardado = request;

            //1D) CAPTURAMOS EL HTML
            string responseHtml = "";
            Stream streamResponse = wResponse.GetResponseStream();
            //Añadimos System.Text.Encoding.Default; para capturar acentos y símbolos raros
            StreamReader streamReader = new StreamReader(streamResponse, Encoding.Default);
            responseHtml = streamReader.ReadToEnd();
            objeto.responseHtml01 = responseHtml;

            //1E) COMPROBAMOS QUE NO NOS HAYAN BANEADO
            ScHtmlParserService scHtmlParserService = new ScHtmlParserService();
            //Comprobamos que no nos hayan baneado:
            bool noHaDevueltoError = scHtmlParserService.NoHaDevueltoError(responseHtml);
            //Testeamos
            if (noHaDevueltoError == false)
            {
                string eliminar = "";
                //return null;
            }

            //1F) SACAMOS EL TOKEN DEL HTML:
            //DevuelveValorBuscado(string xpath, string url, string html, bool conProxy, string attribute)
            objeto.token = scHtmlParserService.DevuelveValorBuscado(objeto.xpathToken, null, objeto.responseHtml01, false, objeto.attributeToken);

            Console.WriteLine("TOKEN scrapeado");
            Console.WriteLine(objeto.token);

            ////EL TOKEN DEL HTML NO ES EL MISMO DE LAS COOKIES
            ////VAMOS A PROBAR EL DE LA COOKIE:
            //objeto.token = cookieToken;
            ////Limpiamos el objeto token:
            //if (objeto.token != null && objeto.token != "")
            //{
            //    objeto.token = objeto.token.Split('\"')[5];
            //}

            //1G) AÑADIMOS LOS PARÁMETROS QUE ENVIAREMOS POR POST
            //objeto = CreamosStringParametros(objeto);
            objeto = CreamosStringParametrosConIlist(objeto);

            //1H) AÑADIMOS EL TOKEN COMO UN PARÁMETRO MÁS
            if (objeto.token != null && objeto.token != "")
            {
                //objeto.parametros = objeto.parametros + "&" + objeto.nomParamToken + "=" + objeto.token;
                objeto.parametros = objeto.nomParamToken + "=" + objeto.token + "&" + objeto.parametros;
            }

            ////2) ENVÍAS EL FORMULARIO SIN COOKIES Y/O SIN TOKEN
            ////TE DEVUELVE EL FORMULARIO VACÍO + OTRA COOKIE + OTRO TOKEN.

            //3) VUELVES A ENVIAR EL FORMULARIO DE LA PÁGINA RECEPTORA + PARÁMETROS + TOKEN + COOKIES
            //Y TE DEVUELVE LA RESPUESTA
            url = objeto.url02;

            //3A) CONFIGURACIÓN:
            // Creamos una variable WebRequest
            wRequest = WebRequest.Create(url);
            wRequest.Timeout = 20000;
            // Parseamos el objeto WebRequest para convertirlo en un HttpWebRequest
            request = (HttpWebRequest)wRequest;
            //Le pasamos los parámetros CONSTANTES Que dependen del Site
            request.Method = "POST"; //objeto.request_Method;              
            request.ContentType = objeto.request_ContentType;
            request.Accept = objeto.request_Accept;
            request.UserAgent = objeto.request_UserAgent;
            request.AllowAutoRedirect = objeto.request_AllowAutoRedirect;

            //3B) INSERTAMOS LA COOKIE DE SESIÓN
            //3B1y2
            if (cookieCollectionDelResponse != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookieCollectionDelResponse);
            }

            ////3B3)
            //request.CookieContainer = objeto.requestGuardado.CookieContainer;

            ////3B3b) NO FUNCIONA
            //request = objeto.requestGuardado;

            //3C) INSERTAMOS LOS PARÁMETROS EN EL REQUEST
            //Para pasar los parámetros, convertimos el string parametros en un array de bytes
            //Decodificados
            ////Así, no va, se lanza antes de tiempo...
            //byte[] arrayBytesParametros = System.Text.Encoding.UTF8.GetBytes(objeto.parametros);
            //request.ContentLength = arrayBytesParametros.Length;

            ////Pasamos el arrayBytesParametros al request, (No tengo claro como...)
            //using (System.IO.Stream streamRequest = request.GetRequestStream())
            //{
            //    streamRequest.Write(arrayBytesParametros, 0, arrayBytesParametros.Length);
            //}
            ////...Así, sí
            //Pasamos el string con los param + token
            //a un valor binario Array de Bytes byte[]
            byte[] data = Encoding.UTF8.GetBytes(objeto.parametros);
            //ese binario se le pasa al request, de una forma rarísima
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            //3D) ENVÍO:
            //Enviamos el Request y obtenemos el Response:
            //Primero como WebResponse:
            //WebResponse wResponse = null;
            //HttpWebResponse response = null;
            try
            {
                wResponse = request.GetResponse();
                // Parseamos el objeto WebResponse para convertirlo en un HttpWebResponse
                response = (HttpWebResponse)wResponse;
            }
            catch (Exception e)
            {
                string error = e.ToString();
                Console.WriteLine(error);
                throw;
            }

            //3E) CAPTURAR HTML:
            //Capturamos el Html de la página respondida:
            responseHtml = "";
            streamResponse = wResponse.GetResponseStream();
            //Añadimos , System.Text.Encoding.Default); para capturar acentos y símbolos raros
            streamReader = new StreamReader(streamResponse, Encoding.Default);
            responseHtml = streamReader.ReadToEnd();
            objeto.responseHtml02 = responseHtml;

            //3F) COMPROBAMOS QUE NO NOS HAYAN BANEADO
            noHaDevueltoError = scHtmlParserService.NoHaDevueltoError(responseHtml);
            //Testeamos
            if (noHaDevueltoError == false)
            {
                string eliminar = "";
                //return null;
            }

            //3G) EXTRAEMOS LOS VALORES QUE NECESITAMOS DEL HTML
            //Y LOS CARGAMOS EN EL OBJETO
            //DevuelveValorBuscado(string xpath, string url, string html, bool conProxy, string attribute)
            objeto.valor01 = scHtmlParserService.DevuelveValorBuscado(objeto.xpathValor01, null, objeto.responseHtml02, false, objeto.attributeValor01);
            System.Console.WriteLine("L'aranya ha obtingut les dades");

            return objeto;
        }


        //Sencillo
        public ObjetoGordoViewModel DevuelveObjetoConTodo(ObjetoGordoViewModel objeto)
        {
            //ENVÍAS URL DEL FORMULARIO SIN COOKIE, (PORQUE NO LA TENEMOS) Por GET
            //Y TE DEVUELVE EL FORMULARIO VACÍO + LA COOKIE
            objeto = SendForm(objeto, 1);
            if (objeto == null)
            {
                return objeto;
            }
            ////ENVÍAS URL DEL FORMULARIO OTRA VEZ SIN COOKIE
            ////Y TE DEVUELVE EL FORMULARIO VACÍO + OTRA COOKIE. (NO FUNCIONA. HAY QUE ENVIAR LA COOKIE O TE PROPORCIONA OTRA)
            //objeto = SendForm(objeto, 1);

            //VUELVES A ENVIAR URL DE LA PÁGINA RECEPTORA + PARÁMETROS + COOKIE por POST
            //Y TE DEVUELVE LA RESPUESTA
            objeto = SendForm(objeto, 2);
            if (objeto == null)
            {
                return objeto;
            }

            //EXTRAEMOS LOS VALORES QUE NECESITAMOS DE responseHtml
            //Y LOS CARGAMOS EN EL OBJETO
            ScHtmlParserService scHtmlParserService = new ScHtmlParserService();
            //DevuelveValorBuscado(string xpath, string url, string html, bool conProxy, string attribute)
            objeto.valor01 = scHtmlParserService.DevuelveValorBuscado(objeto.xpathValor01, null, objeto.responseHtml02, false, objeto.attributeValor01);

            System.ConsoleColor color = System.ConsoleColor.White;
            System.Console.ForegroundColor = color;
            System.Console.WriteLine("L'aranya ha obtingut les dades");
            return objeto;
        }

        public ObjetoGordoViewModel CreamosStringParametrosConIlist(ObjetoGordoViewModel objeto)
        {
            if (objeto == null)
            {
                return objeto;
            }
            string parametros = "";
            for (int i = 0; i < objeto.nomParams.Count; i++)
            {
                parametros = parametros + objeto.nomParams[i] + "=" + objeto.valueParams[i];
                if (i < objeto.nomParams.Count -1)
                {
                    parametros = parametros + "&";
                }
            }
            parametros = parametros.Replace("@", "%40");
            objeto.parametros = parametros;
            return objeto;
        }

        public ObjetoGordoViewModel CreamosStringParametros(ObjetoGordoViewModel objeto)
        {
            if (objeto == null)
            {
                return objeto;
            }
            string parametros = objeto.nomParam01 + "=" + objeto.valueParam01
                        + "&" + objeto.nomParam02 + "=" + objeto.valueParam02
                        //+ "&" + viewState...?
                        ;
            parametros = parametros.Replace("@", "%40");
            objeto.parametros = parametros;
            return objeto;
        }

        //Sencillo
        private ObjetoGordoViewModel SendForm(ObjetoGordoViewModel objeto, int envio)//
        {
            string url = "";

            url = envio == 1 ? objeto.url01 : objeto.url02;
            System.ConsoleColor color = envio == 1 ? System.ConsoleColor.Yellow : System.ConsoleColor.Cyan;
            System.Console.ForegroundColor = color;
            System.Console.WriteLine("Url: " + url);

            // Creamos una variable WebRequest
            WebRequest wRequest = WebRequest.Create(url);
            wRequest.Timeout = 20000;
            // Parseamos el objeto WebRequest para convertirlo en un HttpWebRequest
            HttpWebRequest request = (HttpWebRequest)wRequest;
            //Le pasamos los parámetros CONSTANTES Que dependen del Site
            request.Method = envio == 1 ? "GET" : "POST"; //objeto.request_Method;              
            request.ContentType = objeto.request_ContentType;
            request.Accept = objeto.request_Accept;
            request.UserAgent = objeto.request_UserAgent;
            request.AllowAutoRedirect = objeto.request_AllowAutoRedirect;
            //Cookies
            if (objeto.requestGuardado == null || objeto.requestGuardado.CookieContainer == null || objeto.requestGuardado.CookieContainer.Count == 0)
            {
                if (request.CookieContainer == null)
                {
                    request.CookieContainer = new CookieContainer();
                }
            }
            else
            {
                request.CookieContainer = objeto.requestGuardado.CookieContainer;
            }
            
            //Tras el envío
            //Ambos reciben cookies: request y response
            CookieContainer container_ = new CookieContainer();
            CookieCollection cookiesDelRequest_ = new CookieCollection();
            container_ = request.CookieContainer;
            cookiesDelRequest_ = container_.GetCookies(request.RequestUri);
            foreach (Cookie cookie in cookiesDelRequest_)
            {
                Console.WriteLine(cookie.ToString());
            }
            //Le pasamos los parámetros VARIABLES Que dependen de lo que queramos poner en el formulario
            if (envio == 2)
            {
                //Para pasar los parámetros, convertimos el string parametros en un array de bytes
                //Decodificados
                byte[] arrayBytesParametros = System.Text.Encoding.UTF8.GetBytes(objeto.parametros);
                request.ContentLength = arrayBytesParametros.Length;

                //Pasamos el arrayBytesParametros al request, (No tengo claro como...)
                using (System.IO.Stream streamRequest = request.GetRequestStream())
                {
                    streamRequest.Write(arrayBytesParametros, 0, arrayBytesParametros.Length);
                }
            }

            //AQUÍ SE HACE EL ENVÍO:

            //Enviamos el Request y obtenemos el Response:
            //Primero como WebResponse:
            WebResponse wResponse = null;
            HttpWebResponse response = null;
            try
            {
                wResponse = request.GetResponse();
                // Parseamos el objeto WebResponse para convertirlo en un HttpWebResponse
                response = (HttpWebResponse)wResponse;
            }
            catch (Exception e)
            {
                string error = e.ToString();
                Console.WriteLine(error);
                throw;
            }
            //Capturamos las Cookies:
            response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
            CookieCollection _cookieCollection = new CookieCollection();
            _cookieCollection = response.Cookies;

            string cookieSession_ = _cookieCollection[0].Name + "=" + _cookieCollection[0].Value;
            //JSESSIONID=9BED2B42102612F3D57061AA642AA2BE.nodePROA2

            //Tras el envío
            //Ambos reciben cookies: request y response
            CookieContainer container = new CookieContainer();
            CookieCollection cookiesDelRequest = new CookieCollection();
            CookieCollection cookiesDelResponse = new CookieCollection();

            //Podemos capturar las cookies de dos maneras distintas:
            //Primera:
            cookiesDelResponse = response.Cookies;
            container = request.CookieContainer;
            cookiesDelRequest = container.GetCookies(request.RequestUri);
            foreach (Cookie cookie in cookiesDelResponse)
            {
                Console.WriteLine(cookie.ToString());
            }
            foreach (Cookie cookie in cookiesDelRequest)
            {
                Console.WriteLine(cookie.ToString());
            }

            //Segunda:
            for (int i = 0; i < response.Headers.Count; ++i)
            {
                string name = response.Headers.GetKey(i);
                string value = response.Headers.Get(i);
                Console.WriteLine(name + ": " + value);
                if (name == "Set-Cookie")
                {
                    IList<string> strCookies = value.Split(new[] { "path=/," }, StringSplitOptions.None);
                    foreach (var strCookie in strCookies)
                    {
                        Console.WriteLine(strCookie);
                    }
                }
            }

            string cookieSession = cookiesDelResponse[0].Name + "=" + cookiesDelResponse[0].Value;
            //;jsessionid=4B45B7F73C56B2D8FC1515F945D86FE3.nodePROA2?index=0
            //JSESSIONID=9BED2B42102612F3D57061AA642AA2BE.nodePROA2

            //A veces, no podemos capturar las cookies
            //https://medium.com/swlh/7-keys-to-the-mystery-of-a-missing-cookie-fdf22b012f09

            //Capturamos el Html de la página respondida:
            string responseHtml = "";
            //System.IO.Stream streamResponse = request.GetResponse().GetResponseStream();
            System.IO.Stream streamResponse = wResponse.GetResponseStream();
            //Añadimos System.Text.Encoding.Default; para capturar acentos y símbolos raros
            System.IO.StreamReader streamReader = new System.IO.StreamReader(streamResponse, System.Text.Encoding.Default);
            responseHtml = streamReader.ReadToEnd();
            if (envio==1)
            {
                objeto.responseHtml01 = responseHtml;
            }
            else
            {
                objeto.responseHtml02 = responseHtml;
            }

            ScHtmlParserService scHtmlParserService = new ScHtmlParserService();
            //Comprobamos que no nos hayan baneado:
            bool noHaDevueltoError = scHtmlParserService.NoHaDevueltoError(responseHtml);
            //Testeamos
            if (noHaDevueltoError == false)
            {
                string eliminar = "";
                //return null;
            }

            //Una vez cargado el request,
            //Con todo lo que le hemos metido
            //Y con lo que le ha metido el site,
            //lo guardamos en nuestro objeto:
            objeto.requestGuardado = request;

            if (envio == 1)
            {
                ////Capturamos el parámetro Token CSRFToken = '359f0ce9-08b3-4c7e-88cb-727b035e1939';
                //ScHtmlParserService scHtmlParserService = new ScHtmlParserService();
                //DevuelveValorBuscado(string xpath, string url, string html, bool conProxy, string attribute)
                objeto.token = scHtmlParserService.DevuelveValorBuscado(objeto.xpathToken, null, objeto.responseHtml01, false, objeto.attributeToken);
                ////Limpiamos el objeto token:
                //if (objeto.token != null && objeto.token != "")
                //{
                //    objeto.token = objeto.token.Split('\"')[5];
                //}
                if (objeto.token != null && objeto.token != "")
                {
                    objeto.parametros = objeto.parametros + "&" + objeto.nomParamToken + "=" + objeto.token;
                }
            }

            //Devolvemos:
            return objeto;
        }

        //Complejo
        public ObjetoGordoViewModel DevuelveObjetoConTodo(ObjetoGordoViewModel objeto, bool conProxy)
        {
            //ENVÍAS URL DEL FORMULARIO SIN COOKIE, (PORQUE NO LA TENEMOS) Por GET
            //Y TE DEVUELVE EL FORMULARIO VACÍO + LA COOKIE
            objeto = SendForm(objeto, 1, conProxy);
            if (objeto == null)
            {
                return objeto;
            }

            ////ENVÍAS URL DEL FORMULARIO OTRA VEZ SIN COOKIE
            ////Y TE DEVUELVE EL FORMULARIO VACÍO + OTRA COOKIE. (NO FUNCIONA. HAY QUE ENVIAR LA COOKIE O TE PROPORCIONA OTRA)
            //objeto = SendForm(objeto, 1);

            //////////Añadimos los parámetros que enviaremos por POST
            ////////objeto = CreamosStringParametros(objeto);

            //VUELVES A ENVIAR URL DE LA PÁGINA RECEPTORA + PARÁMETROS + COOKIE por POST
            //Y TE DEVUELVE LA RESPUESTA
            objeto = SendForm(objeto, 2, conProxy);
            if (objeto == null)
            {
                return objeto;
            }

            //EXTRAEMOS LOS VALORES QUE NECESITAMOS DE responseHtml
            //Y LOS CARGAMOS EN EL OBJETO
            ScHtmlParserService scHtmlParserService = new ScHtmlParserService();
            //DevuelveValorBuscado(string xpath, string url, string html, bool conProxy, string attribute)
            objeto.valor01 = scHtmlParserService.DevuelveValorBuscado(objeto.xpathValor01, null, objeto.responseHtml02, false, objeto.attributeValor01);

            System.ConsoleColor color = System.ConsoleColor.White;
            System.Console.ForegroundColor = color;
            System.Console.WriteLine("L'aranya ha obtingut les dades");
            return objeto;
        }

        //Complejo
        private ObjetoGordoViewModel SendForm(ObjetoGordoViewModel objeto, int envio, bool conProxy)
        {
            string url = "";

            url = envio == 1 ? objeto.url01 : objeto.url02;
            System.ConsoleColor color = envio == 1 ? System.ConsoleColor.Yellow : System.ConsoleColor.Cyan;
            System.Console.ForegroundColor = color;
            System.Console.WriteLine("Url: " + url);
            WebRequest wRequest = null;
            HttpWebRequest request = null;
            WebResponse wResponse = null;
            HttpWebResponse response = null;
            int cuentaErrores = 0;
            bool noHaDevueltoError = false;
            ScHtmlParserService scHtmlParserService = new ScHtmlParserService();
            while (noHaDevueltoError == false && conProxy == true)
            {
                // Creamos una variable WebRequest
                //WebRequest wRequest = WebRequest.Create(url);
                wRequest = scHtmlParserService.DevuelveWRequest(url, cuentaErrores, conProxy);
                wRequest.Timeout = 20000;
                // Parseamos el objeto WebRequest para convertirlo en un HttpWebRequest
                request = (HttpWebRequest)wRequest;
                //Le pasamos los parámetros CONSTANTES Que dependen del Site
                request.Method = envio == 1 ? "GET" : "POST"; //objeto.request_Method;              
                request.ContentType = objeto.request_ContentType;
                request.Accept = objeto.request_Accept;
                request.UserAgent = objeto.request_UserAgent;
                request.AllowAutoRedirect = objeto.request_AllowAutoRedirect;
                //Cookies
                if (objeto.requestGuardado == null || objeto.requestGuardado.CookieContainer == null || objeto.requestGuardado.CookieContainer.Count == 0)
                {
                    if (request.CookieContainer == null)
                    {
                        request.CookieContainer = new CookieContainer();
                    }
                }
                else
                {
                    request.CookieContainer = objeto.requestGuardado.CookieContainer;
                }

                //Le pasamos los parámetros VARIABLES Que dependen de lo que queramos poner en el formulario
                if (envio == 2)
                {
                    //Para pasar los parámetros, convertimos el string parametros en un array de bytes
                    //Decodificados
                    byte[] arrayBytesParametros = System.Text.Encoding.UTF8.GetBytes(objeto.parametros);
                    request.ContentLength = arrayBytesParametros.Length;

                    //Pasamos el arrayBytesParametros al request, (No tengo claro como...)
                    using (System.IO.Stream streamRequest = request.GetRequestStream())
                    {
                        streamRequest.Write(arrayBytesParametros, 0, arrayBytesParametros.Length);
                    }

                    //Añadimos el parámetro Token CSRFToken = '359f0ce9-08b3-4c7e-88cb-727b035e1939';
                    objeto.parametros = objeto.parametros + "&CSRFToken=" + objeto.token;
                }

                //AQUÍ SE HACE EL ENVÍO:
                //Enviamos el Request y obtenemos el Response:
                //Primero como WebResponse:
                wResponse = request.GetResponse();
                // Parseamos el objeto WebResponse para convertirlo en un HttpWebResponse
                response = (HttpWebResponse)wResponse;

                //Capturamos el Html de la página respondida:
                string responseHtml = "";
                //System.IO.Stream streamResponse = request.GetResponse().GetResponseStream();
                System.IO.Stream streamResponse = wResponse.GetResponseStream();
                //Añadimos System.Text.Encoding.Default; para capturar acentos y símbolos raros
                System.IO.StreamReader streamReader = new System.IO.StreamReader(streamResponse, System.Text.Encoding.Default);
                responseHtml = streamReader.ReadToEnd();
                if (envio == 1)
                {
                    objeto.responseHtml01 = responseHtml;
                }
                else
                {
                    objeto.responseHtml02 = responseHtml;
                }

                cuentaErrores = cuentaErrores + 1;
                //Comprobamos que no nos hayan baneado:
                noHaDevueltoError = scHtmlParserService.NoHaDevueltoError(responseHtml);
            }

            //Testeamos
            if (noHaDevueltoError == false)
            {
                string eliminar = "";
                //return null;
            }

            if (envio == 1)
            {
                ////Capturamos el parámetro Token CSRFToken = '359f0ce9-08b3-4c7e-88cb-727b035e1939';
                //ScHtmlParserService scHtmlParserService = new ScHtmlParserService();
                //DevuelveValorBuscado(string xpath, string url, string html, bool conProxy, string attribute)
                objeto.token = scHtmlParserService.DevuelveValorBuscado(objeto.xpathToken, null, objeto.responseHtml01, false, objeto.attributeToken);
                ////Con Proxy NO, PORQUE YA TENEMOS HTML
                //objeto.token = scHtmlParserService.DevuelveValorBuscado(objeto.xpathToken, null, objeto.responseHtml, true);
                ////Limpiamos el objeto token:
                //if (objeto.token != null && objeto.token != "")
                //{
                //    objeto.token = objeto.token.Split('\"')[5];
                //}
                if (objeto.token != null && objeto.token != "")
                {
                    objeto.parametros = objeto.parametros + "&" + objeto.nomParamToken + "=" + objeto.token;
                }
            }

            //Tras el envío
            //Ambos reciben cookies: request y response
            CookieContainer container = new CookieContainer();
            CookieCollection cookiesDelRequest = new CookieCollection();
            CookieCollection cookiesDelResponse = new CookieCollection();

            //Podemos capturar las cookies de dos maneras distintas:
            //Primera:
            cookiesDelResponse = response.Cookies;
            container = request.CookieContainer;
            cookiesDelRequest = container.GetCookies(request.RequestUri);
            foreach (Cookie cookie in cookiesDelResponse)
            {
                Console.WriteLine(cookie.ToString());
            }
            foreach (Cookie cookie in cookiesDelRequest)
            {
                Console.WriteLine(cookie.ToString());
            }

            //Segunda:
            for (int i = 0; i < response.Headers.Count; ++i)
            {
                string name = response.Headers.GetKey(i);
                string value = response.Headers.Get(i);
                Console.WriteLine(name + ": " + value);
                if (name == "Set-Cookie")
                {
                    IList<string> strCookies = value.Split(new[] { "path=/," }, StringSplitOptions.None);
                    foreach (var strCookie in strCookies)
                    {
                        Console.WriteLine(strCookie);
                    }
                }
            }

            //A veces, no podemos capturar las cookies
            //https://medium.com/swlh/7-keys-to-the-mystery-of-a-missing-cookie-fdf22b012f09

            //Una vez cargado el request,
            //Con todo lo que le hemos metido
            //Y con lo que le ha metido el site,
            //lo guardamos en nuestro objeto:
            objeto.requestGuardado = request;

            //Devolvemos:
            return objeto;
        }
    
        //private ObjetoGordoViewModel ExtraerDatoDelHtmlDevuelto(ObjetoGordoViewModel objeto)
        //{
        //    ScHtmlParserService scHtmlParserService = new ScHtmlParserService();
        //    //DevuelveValorBuscado(string xpath, string url, string html, bool conProxy, string attribute)
        //    objeto.valor01 = scHtmlParserService.DevuelveValorBuscado(objeto.xpathValor01, null, objeto.responseHtml, false, objeto.attributeValor01);
        //    ////Limpiamos el objeto token:
        //    //if (objeto.token != null && objeto.token != "")
        //    //{
        //    //    objeto.token = objeto.token.Split('\"')[5];
        //    //}

        //    return objeto;
        //}
    }
}
