using System;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;

using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;

namespace WSAlmacen
{
    public static class Validaciones
    {
        public static async Task<bool> ExisteUsuario(IFirebaseClient client, string user)
        {
            FirebaseResponse response = await client.GetTaskAsync("usuarios/" + user);
            return response.Body != "null";
        }

        private static string ConvertStringtoMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public static async Task<bool> CoincideContraseña(IFirebaseClient client, string user, string pass)
        {
            FirebaseResponse response = await client.GetTaskAsync("usuarios/" + user);
            string rigthPassword = response.Body.Substring(1,32);
            return rigthPassword == ConvertStringtoMD5(pass);
        }

        public static async Task<bool> ExisteCategoria(IFirebaseClient client, string category)
        {
            FirebaseResponse response = await client.GetTaskAsync("productos/" + category);
            return response.Body != "null";
        }

        public static async Task<bool> ExisteISBN(IFirebaseClient client, string isbn)
        {
            FirebaseResponse response = await client.GetTaskAsync("detalles/" + isbn);
            return response.Body != "null";
        }

        public static Detalle JsonToDetalle(string json)
        {
            try
            {
                JObject p = JObject.Parse(json);
                if (p.Property("ISBN") != null &&
                    p.Property("Autor") != null &&
                    p.Property("Descuento") != null &&
                    p.Property("Editorial") != null &&
                    p.Property("Fecha") != null &&
                    p.Property("Nombre") != null &&
                    p.Property("Precio") != null)
                    return new Detalle(p);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetCategoria(string isbn)
        {
            string categoria;
            isbn = isbn.Length > 3 ? isbn.Substring(0, 3) : "NA";

            switch (isbn)
            {
                case "ART":
                    categoria = "articulos";
                    break;
                case "COM":
                    categoria = "comics";
                    break;
                case "LIB":
                    categoria = "libros";
                    break;
                case "MAN":
                    categoria = "mangas";
                    break;
                default:
                    categoria = "articulos";
                    break;
            }

            return categoria;
        }
    }
}