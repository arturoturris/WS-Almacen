using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSAlmacen
{
    public class Detalle
    {
        public string Autor { get; set; }
        public bool Descuento { get; set; }
        public string Editorial { get; set; }
        public int Fecha { get; set; }
        public string ISBN { get; set; }
        public string Nombre { get; set; }
        public float Precio { get; set; }

        public Detalle() { }

        public Detalle(JObject detalle) {
            this.Autor = detalle.GetValue("Autor").ToString();
            this.Descuento = detalle.GetValue("Descuento").ToObject<bool>();
            this.Editorial = detalle.GetValue("Editorial").ToString();
            this.Fecha = detalle.GetValue("Fecha").ToObject<int>();
            this.ISBN = detalle.GetValue("ISBN").ToString();
            this.Nombre = detalle.GetValue("Nombre").ToString();
            this.Precio = detalle.GetValue("Precio").ToObject<float>();
        }
    }
}