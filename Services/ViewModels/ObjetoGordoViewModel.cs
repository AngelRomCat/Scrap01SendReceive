using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class ObjetoGordoViewModel
    {
        //Parámetros necesarios:
        //        Parámetros enviados por POST:
        //1.	name: 
        //ANGEL
        //2.	email: 
        //angel @rom.cat
        //Es tan importante el nombre del parámetro como el valor

        public string url01 { get; set; }
        public string url02 { get; set; }
        public string nomParam01 { get; set; }
        public string valueParam01 { get; set; }
        public string nomParam02 { get; set; }
        public string valueParam02 { get; set; }
        public IList<string> nomParams { get; set; }
        public IList<string> valueParams { get; set; }
        public string parametros { get; set; }
        public string request_Method { get; set; }
        public bool request_AllowAutoRedirect { get; set; }
        public string request_UserAgent { get; set; }
        public string request_ContentType { get; set; }
        public string request_Accept { get; set; }        
        public HttpWebRequest requestGuardado { get; set; }
        public string responseHtml01 { get; set; }
        public string responseHtml02 { get; set; }
        public string xpathToken { get; set; }
        public string attributeToken { get; set; }
        public string nomParamToken { get; set; }
        public string token { get; set; }
        public string xpathValor01 { get; set; }
        public string attributeValor01 { get; set; }
        public string valor01 { get; set; }
        public string xpathValor02 { get; set; }
        public string attributeValor02 { get; set; }
        public string valor02 { get; set; }
        public IList<string> xpathValors { get; set; }
        public IList<string> attributeValors { get; set; }
        public IList<string> valorValors { get; set; }
    }
}
