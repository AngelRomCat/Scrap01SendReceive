using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class LogHelper
    {
        public static void EscribirLog(string incidencia)
        {
            incidencia = "\r\n"
                + "\r\n"
                + "\r\n"
                + DateTime.Now.ToString()
                + "\r\n"
                + "API_JOEL: "
                + "\r\n"
                + incidencia
                + "\r\n"
                + "\r\n"
                + "\r\n";
            //Trace.TraceInformation(incidencia);
            ////Conservamos también el otro Log
            string ruta = @"C:\CursoTardes\Scrap01SendReceive\files\logsPath\InfoAxesor.log";
            try
            {
                using (StreamWriter sw = File.AppendText(ruta))
                {
                    sw.WriteLine(incidencia);
                    sw.Close();
                }
            }
            catch
            {

            }
        }


        //Aquests dos mètodes ens copien la pàgina web tal i com es veuren a Infocif i Axesor:
        public static void EscribirWebAxesorNif(string html, string nif)
        {
            //string ruta = @"C:\CursoTardes\Scrap01SendReceive\files\logsPath\PaginasInfoAxesor\Axesor" + nif + ".html";

            //File.WriteAllText(ruta, html);
        }


        public static void EscribirWebInfocifNif(string html, string nif)
        {
            //string ruta = @"C:\CursoTardes\Scrap01SendReceive\files\logsPath\PaginasInfoAxesor\Infocif" + nif + ".html";

            //File.WriteAllText(ruta, html);
        }


        public static void AgregarProxyActual(string proxyServer)
        {
            string ruta = @"C:\CursoTardes\Scrap01SendReceive\files\logsPath\ProxyActual.log";
            File.WriteAllText(ruta, proxyServer);
        }


        public static string LeerProxyActual()
        {
            string proxyServer = "";
            string ruta = @"C:\CursoTardes\Scrap01SendReceive\files\logsPath\ProxyActual.log";
            FilesHelper.CreaArchivoDeTextoSiNoExiste(ruta, "");
            using (StreamReader sr = new StreamReader(ruta))
            {
                proxyServer = sr.ReadToEnd();
            }
            return proxyServer;
            //return string.Empty;
        }


        public static void AgregarNumeroErroresSeguidos(int numeroErroresSeguidos)
        {
            string ruta = @"C:\CursoTardes\Scrap01SendReceive\files\logsPath\NumeroDeErroresSeguidos.log";
            File.WriteAllText(ruta, numeroErroresSeguidos.ToString());
        }


        public static int LeerNumeroErroresSeguidos()
        {
            int numeroErroresSeguidos = 0;
            string ruta = @"C:\CursoTardes\Scrap01SendReceive\files\logsPath\NumeroDeErroresSeguidos.log";
            int aux = 0;
            string numeroErroresSeguidosString = "";
            using (StreamReader sr = new StreamReader(ruta))
            {
                numeroErroresSeguidosString = sr.ReadToEnd();
            }
            //numeroErroresSeguidos = (int.TryParse(numeroErroresSeguidosString, out aux) == true) ? aux : aux;
            if (int.TryParse(numeroErroresSeguidosString, out aux) == true)
            {
                numeroErroresSeguidos = aux;
            }
            return numeroErroresSeguidos;
            //return 0;
        }


        public static void AgregarListaProxysBaneados(string proxyServer)
        {
            string ruta = @"C:\CursoTardes\Scrap01SendReceive\files\logsPath\ListaProxysBaneados.log";
            using (StreamWriter sw = File.AppendText(ruta))
            {
                sw.WriteLine(proxyServer);
                sw.Close();
            }
        }


        public static void HabemusProxy(string proxyServer)
        {
            string ruta = @"C:\CursoTardes\Scrap01SendReceive\files\logsPath\HabemusProxy.log";
            using (StreamWriter sw = File.AppendText(ruta))
            {
                sw.WriteLine(proxyServer);
                sw.Close();
            }
        }


        public static string LeerListaProxysBaneados()
        {
            string proxysBaneados = "";
            string ruta = @"C:\CursoTardes\Scrap01SendReceive\files\logsPath\ListaProxysBaneados.log";
            using (StreamReader sr = new StreamReader(ruta))
            {
                proxysBaneados = sr.ReadToEnd();
            }
            return proxysBaneados;
        }


        public static int EscribirChivato(string donde, int numeroErrores)
        {
            try
            {
                string ruta = @"C:\CursoTardes\Scrap01SendReceive\files\logsPath\Chivatos\" + DateTime.Today.ToString("yyyy'/'MM'/'dd").Replace(":", "_").Replace("/", "_").Replace("-", "_") + "_" + donde + "_.log";
                numeroErrores = numeroErrores + LeerChivato(ruta);

                File.WriteAllText(ruta, numeroErrores.ToString());
            }
            catch
            {
            }
            return numeroErrores;
        }


        public static int LeerChivato(string ruta)
        {
            int numeroErrores = 0;
            try
            {
                if (File.Exists(ruta))
                {
                    int aux = 0;
                    string numeroErroresString = "";
                    using (StreamReader sr = new StreamReader(ruta))
                    {
                        numeroErroresString = sr.ReadToEnd();
                    }
                    //numeroErroresSeguidos = (int.TryParse(numeroErroresSeguidosString, out aux) == true) ? aux : aux;
                    if (int.TryParse(numeroErroresString, out aux) == true)
                    {
                        numeroErrores = aux;
                    }
                }
            }
            catch
            {
            }
            return numeroErrores;
        }


        public static void EscribirEstanNoEstanLog(string nif, bool axesor, bool esta)
        {
            string buscador = "_Axesor";
            if (axesor == false)
            {
                buscador = "_Infocif";
            }
            string estaONo = "_EstanEn";
            if (esta == false)
            {
                estaONo = "_NoEstanEn";
            }

            try
            {
                string ruta = @"C:\CursoTardes\Scrap01SendReceive\files\logsPath\NifsQue" + estaONo + buscador + "_.log";

                using (StreamWriter sw = File.AppendText(ruta))
                {
                    sw.WriteLine("'" + nif + "', ");
                    sw.Close();
                }
            }
            catch
            {
            }
        }
    }
}