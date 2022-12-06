using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{

    public class FilesHelper
    {
        public static string _serverMapPath = "";
        public static string _pathCapturador = "";
        public static string _pathFinalGrande = "";
        public static string _pathFinalPequenyo = "";

        //public static bool GenerarReportExcel(IList<LineaExcelViewModel> lineas, string path_destino)
        //{
        //    string contenidoExcel = "";
        //    contenidoExcel = contenidoExcel
        //            + "SITE;"
        //            + "PRODUCTE;"
        //            + "ITEM;"
        //            + "URL DEL PRODUCTE;"
        //            + "VALOR MÍNIM;"
        //            + "VALOR MÀXIM;"
        //            + "VARIACIÓ MÀXIMA;"
        //            + "% VARIACIÓ MÀXIMA;"
        //            + "HISTORIAL;"
        //            + "ANOTACIONS;"
        //            //+ "\n\r";
        //            + "\n";

        //    foreach (var linea in lineas)
        //    {
        //        contenidoExcel = contenidoExcel
        //                            + linea.nameSite + ";"
        //                            + linea.nameProduct + ";"
        //                            + linea.nameItem + ";"
        //                            + "= HIPERVINCULO(\""
        //                            + linea.linkProduct
        //                            + "\")"
        //                            + ";"
        //                            + linea.precioProductRegisterMinimo + ";"
        //                            + linea.precioProductRegisterMaximo + ";"
        //                            + linea.variacionMaxima + ";"
        //                            + decimal.Round(linea.variacionMaximaPorcentaje, 2) + ";"
        //                            //+ decimal.Round(100 * linea.variacionMaxima / linea.precioProductRegisterMinimo, 2) + ";"
        //                            + "= HIPERVINCULO(\""
        //                            + linea.linkHistorial
        //                            + "\")"
        //                            + ";"
        //                            + " ;"
        //                            //+ "\n\r"
        //                            + "\n"
        //                            ;
        //    }
        //    AfegirPrimeraLiniaArchivo(contenidoExcel, path_destino);

        //    return true;
        //}

        //public static bool GenerarInformeExcel(IList<LineaExcelJosepViewModel> lineasJosep, string pathInformeHoy, string pathInformeHoyEsp, string path_capturas)
        //{
        //    string contenidoExcelEsp = "";
        //    contenidoExcelEsp = contenidoExcelEsp
        //            + "WEBSITE;"
        //            + "URL DEL PRODUCTE;"
        //            + "DATA;"
        //            + "NUMERO DEL PRODUCTO;"
        //            + "ITEM;"
        //            + "ID REGISTRE;"
        //            + "PREU;"
        //            + "HISTORIAL;"
        //            + " ;"
        //            + "CANVI"
        //            + " ;"
        //            + "FOTO?"
        //            + "\n";
        //    string linkProduct = "";
        //    foreach (var linea in lineasJosep)
        //    {
        //        if (linkProduct != linea.linkProduct)
        //        {
        //            contenidoExcelEsp = contenidoExcelEsp
        //                                    + "\n"
        //                                    ;
        //            linkProduct = linea.linkProduct;
        //        }
        //        string haCambiado = "";
        //        if (linea.haCambiado == true)
        //        {
        //            haCambiado = "Canvi controlat";
        //        }
        //        if (linea.haCambiado == false)
        //        {
        //            haCambiado = "Canvi";
        //        }
        //        int faltaFoto = 0;
        //        string rutaFoto = path_capturas + linea.idProductRegister + ".jpg";
        //        if (Existe(rutaFoto) != true && linea.haCambiado != null)
        //        {
        //            faltaFoto = linea.idProductRegister;
        //        }
        //        else
        //        {
        //            faltaFoto = 0;
        //        }
        //        if (faltaFoto > 0)
        //        {

        //        }
        //        contenidoExcelEsp = contenidoExcelEsp
        //                            + linea.urlSite + ";"
        //                            + "= HIPERVINCULO(\""
        //                            + linea.linkProduct
        //                            + "\")"
        //                            + ";"
        //                            + linea.dateRegister.ToString("dd/MM/yyyy") + ";"
        //                            + linea.idProduct + ";"
        //                            + linea.nameItem + ";"
        //                            + linea.idProductRegister + ";"
        //                            + decimal.Round(linea.precioProductRegister, 2) + ";"
        //                            + "= HIPERVINCULO(\""
        //                            + linea.linkHistorial
        //                            + "\")"
        //                            + ";"
        //                            + " ;"
        //                            + haCambiado + ";"
        //                            + " ;"
        //                            + faltaFoto + ";"
        //                                + "\n"
        //                                ;
        //    }
        //    AfegirPrimeraLiniaArchivo(contenidoExcelEsp, pathInformeHoyEsp);

        //    string contenidoExcel = contenidoExcelEsp.Replace("HIPERVINCULO", "ENLLAÇ");
        //    AfegirPrimeraLiniaArchivo(contenidoExcel, pathInformeHoy);

        //    return true;
        //}

        public static DirectoryInfo CheckDirectory(String path)
        {
            return Directory.CreateDirectory(path);
        }

        public static bool Existe(string path)
        {
            bool ok = false;

            var file = new FileInfo(path);
            if (file.Exists && file.Length > 0)
            {
                ok = true;
            }

            return ok;
        }

        public static bool CreaArchivoDeTextoSiNoExiste(string path, string texto)
        {
            bool ok = false;
            ok = Existe(path);
            //Si no existe: ok = false;
            if (ok == false)
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(texto);
                }
            }
            ok = Existe(path);

            return ok;
        }


        public static bool CreaArchivoDeTextoYMachacarAnterior(string path, string texto)
        {
            bool ok = false;
            ok = EliminarUnArchivo(path);
            ok = Existe(path);
            //Si no existe: ok = false;
            if (ok == false)
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(texto);
                }
            }
            ok = Existe(path);

            return ok;
        }

        public static bool Copia(string pathOrigen, string pathDestino)
        {
            try
            {
                if (Existe(pathOrigen))
                {
                    File.Copy(pathOrigen, pathDestino);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void WriteFile(string path, string text)
        {
            File.WriteAllText(path, text.ToString());
        }

        public static IList<string> LeerLineasTxt(string ruta)
        {
            IList<string> lineas = new List<string>();
            try
            {
                if (File.Exists(ruta))
                {
                    string linea;

                    using (StreamReader sr = new StreamReader(ruta))
                    {
                        while ((linea = sr.ReadLine()) != null)
                        {
                            linea = linea
                                .Replace(";", "");
                            ////https://www.phonehouse.es/altavoces/trust/tytan-2-1-2-1channels-60w-negro-conjunto-de.html
                            //string lineaTuneada = linea.ToLower()
                            //                .Replace(";", "")
                            //                .Replace("https://", "")
                            //                .Replace("http://", "")
                            //                .Replace("www.", "");
                            ////Ahora:
                            ////phonehouse.es/altavoces/trust/tytan-2-1-2-1channels-60w-negro-conjunto-de.html

                            //Comprobamos que ninguno de sus líneas esté repetido:
                            //Si es la primera linea, la añadimos
                            if (lineas.Count == 0)
                            {
                                lineas.Add(linea);
                            }
                            else
                            {//Si no, Como están por orden alfabético, solamente hay que comprobar que el líneas[n-1] != líneas[n]
                                if (lineas[lineas.Count() - 1] != linea)
                                {
                                    lineas.Add(linea);
                                }
                            }
                        }
                    }
                }
                return lineas;
            }
            catch
            {
                return lineas;
            }
        }

        public static string DevolverTextoDelArchivo(string path)
        {
            string text = "";
            bool ok = false;
            ok = Existe(path);
            //Si existe: ok = true;
            if (ok == true)
            {
                StreamReader sr = new StreamReader(path);
                {
                    text = sr.ReadToEnd();
                    sr.Close();
                }
            }

            return text;
        }

        public static string LlegirPaginaProductoAmazon(string path)
        {
            string html = "";
            StreamReader sr = new StreamReader(path);
            {
                html = sr.ReadToEnd();
                sr.Close();
            }

            return html;
        }

        //public static string GenerarPrimeraLineaArchivoCsv()
        //{
        //    string linea = "";

        //    IList<string> nombresCampos = StringHelper.NombresCampos();

        //    foreach (var nombreCampo in nombresCampos)
        //    {
        //        linea = linea + nombreCampo;// + ",";
        //    }

        //    return linea;
        //}

        public static bool AfegirPrimeraLiniaArchivo(string linea, string rutaArchivo)
        {
            StreamWriter sw = new StreamWriter(rutaArchivo, false, new UTF8Encoding(true));
            {
                sw.WriteLine(linea);
                sw.Close();
            }

            return true;
        }

        public static bool AfegirLiniaArchivo(string linea, string rutaArchivo)
        {
            using (StreamWriter sw = File.AppendText(rutaArchivo))
            {
                sw.WriteLine(linea);
                sw.Close();
            }

            return true;
        }

        public static bool AfegirLiniaDownloaderBatch(int id, string url, string rutaBatch)
        {
            bool ok = false;
            string linea = "\n\r" + "start chrome " + url
                + "\n\r" + "timeout 5 > NUL"
                + "\n\r" + "start chrome " + url;

            if (id % 10 == 0)
            {
                linea = linea
                    + "\n\r" + "timeout 5 > NUL"
                    + "\n\r" + "taskkill /F /IM chrome.exe";
            }
            ok = AfegirLiniaArchivo(linea, rutaBatch);

            return ok;
        }

        public static bool EliminarTodosLosArchivos(string path)
        {
            IList<string> strDirectories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories).ToList();
            IList<string> strFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories).ToList();

            foreach (string archivo in strFiles)
            {
                File.Delete(archivo);
            }
            bool ok = NoExisten(path);
            return ok;
        }

        public static bool EliminarUnArchivo(string path)
        {
            bool ok = Existe(path);
            if (ok == true)
            {
                File.Delete(path);
            }
            ok = Existe(path);

            return !ok;
        }

        public static bool NoExisten(string path)
        {
            IList<string> strFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories).ToList();
            bool ok = false;
            if (strFiles == null || strFiles.Count == 0)
            {
                ok = true;
            }
            return ok;
        }

        public static bool GuardaImagen(string path, byte[] bitMap)
        {
            File.WriteAllBytes(path, bitMap);
            bool ok = Existe(path);
            return ok;
        }

    }
}
