using System;
using System.Threading.Tasks;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace WSAlmacen
{
    public class WSAlmacen : IWSAlmacen
    {
        static private IFirebaseConfig config = new FirebaseConfig { 
            AuthSecret = "GlBIK78ZVCv03j1B49pp2zbvgIjbN07JXgOU9NNT",
            BasePath = "https://sw-tienda-online-default-rtdb.firebaseio.com/"
        };
        static private IFirebaseClient client = new FireSharp.FirebaseClient(config);

        private async Task<string> getMensajeDeRespuesta(int codigo=999)
        {
            FirebaseResponse response = await client.GetTaskAsync("/respuestas/" + codigo.ToString());
            return response.Body;
        }

        public async Task<RespuestaSetProd> setProd(string user, string pass, string categoria, string producto)
        {
            RespuestaSetProd respuesta = new RespuestaSetProd { code = 999, status = "Error", data = "" };
            Detalle detalleProducto;

            if (!await Validaciones.ExisteUsuario(client, user))
                respuesta.code = 500;
            else if (!await Validaciones.CoincideContraseña(client, user, pass))
                respuesta.code = 501;
            else if (!await Validaciones.ExisteCategoria(client, categoria))
                respuesta.code = 300;
            else if ((detalleProducto=Validaciones.JsonToDetalle(producto)) == null)
                respuesta.code = 303;
            else if (await Validaciones.ExisteISBN(client,detalleProducto.ISBN))
                respuesta.code = 302;
            else
            {
                await client.SetTaskAsync("detalles/" + detalleProducto.ISBN, detalleProducto);
                client.Set("productos/" + categoria + "/" + detalleProducto.ISBN, detalleProducto.Nombre);
                respuesta.status = "Success";
                respuesta.code = 202;
                respuesta.data = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            }

            respuesta.message = await getMensajeDeRespuesta(respuesta.code);
            return respuesta;
        }

        public async Task<RespuestaUpdateProd> updateProd(string user, string pass, string isbn, string detalles)
        {
            RespuestaUpdateProd respuesta = new RespuestaUpdateProd { code = 999, status = "Error", data = "" };
            Detalle detalleProducto;

            if (!await Validaciones.ExisteUsuario(client, user))
                respuesta.code = 500;
            else if (!await Validaciones.CoincideContraseña(client, user, pass))
                respuesta.code = 501;
            else if (!await Validaciones.ExisteISBN(client, isbn))
                respuesta.code = 304;
            else if ((detalleProducto = Validaciones.JsonToDetalle(detalles)) == null)
                respuesta.code = 303;
            else
            {
                String categoria = Validaciones.GetCategoria(isbn);
                await client.UpdateTaskAsync("detalles/" + detalleProducto.ISBN, detalleProducto);
                client.Set("productos/" + categoria + "/" + detalleProducto.ISBN, detalleProducto.Nombre);
                respuesta.status = "Success";
                respuesta.code = 203;
                respuesta.data = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            }

            respuesta.message = await getMensajeDeRespuesta(respuesta.code);
            return respuesta;
        }

        public async Task<RespuestaDeleteProd> deleteProd(string user, string pass, string isbn)
        {

            RespuestaDeleteProd respuesta = new RespuestaDeleteProd { code = 999, status = "Error", data = "" };

            if (!await Validaciones.ExisteUsuario(client, user))
                respuesta.code = 500;
            else if (!await Validaciones.CoincideContraseña(client, user, pass))
                respuesta.code = 501;
            else if (!await Validaciones.ExisteISBN(client, isbn))
                respuesta.code = 304;
            else
            {
                string categoria = Validaciones.GetCategoria(isbn);
                await client.DeleteTaskAsync("detalles/" + isbn);
                client.Delete("productos/" + categoria + "/" + isbn);
                respuesta.status = "Success";
                respuesta.code = 204;
                respuesta.data = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            }

            respuesta.message = await getMensajeDeRespuesta(respuesta.code);
            return respuesta;
        }
    }
}
