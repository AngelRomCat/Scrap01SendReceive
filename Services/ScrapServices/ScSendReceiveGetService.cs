namespace Services.ScrapServices
{
    internal class ScSendReceiveGetService
    {
        public string EnviarYRecibirGet(string xpath,string url)
        {
            string valorBuscado = "";
            ScHtmlParserService scHtmlParserService = new ScHtmlParserService();
            //Sencillo
            valorBuscado = scHtmlParserService.DevuelveValorBuscado(xpath, url);
            ////Complejo
            //valorBuscado = scHtmlParserService.DevuelveValorBuscado(xpath, url, null, false);
            ////Complejo Con Proxy
            //valorBuscado = scHtmlParserService.DevuelveValorBuscado(xpath, url, null, true);

            return valorBuscado;
        }
    }
}
