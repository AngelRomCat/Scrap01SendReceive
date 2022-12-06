using Services.Helpers;

namespace Services.ScrapServices
{
    public class ScProxyService
    {
        public string GetProxy(int cuentaErrores)
        {
            string proxyUrl = "";
            if (cuentaErrores < 4)
            {
                proxyUrl = LogHelper.LeerProxyActual();
            }
            else
            {
                proxyUrl = "";
                LogHelper.AgregarProxyActual("");
                LogHelper.AgregarListaProxysBaneados(proxyUrl);
            }
            while (proxyUrl == "" || proxyUrl == "\r\n")
            {
                //TRET D'AQUESTA PÀGINA: https://i.imgur.com/6sX9yXG.png DE LA PÀGINA DE PROXYSHARP
                proxyUrl = ProxySharp.GetSingleRandomProxy();// ProxySharp.GetSingleRandomProxy();
                string listaProxysBaneados = LogHelper.LeerListaProxysBaneados();
                if (listaProxysBaneados.Contains(proxyUrl))
                {
                    proxyUrl = "";
                }
            }
            return proxyUrl;    
        }
    }
}
