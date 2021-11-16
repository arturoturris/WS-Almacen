using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace WSAlmacen
{
    [ServiceContract]
    public interface IWSAlmacen
    {
        [OperationContract]
        Task<RespuestaSetProd> setProd(string user, string pass, string categoria, string producto);
        [OperationContract]
        Task<RespuestaUpdateProd> updateProd(string user, string pass, string isbn, string detalles);
        [OperationContract]
        Task<RespuestaDeleteProd> deleteProd(string user, string pass, string isbn);
    }

    [DataContract]
    public class RespuestaBase
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public string data { get; set; }
        [DataMember]
        public string status { get; set; }
    }

    [DataContract]
    public class RespuestaSetProd : RespuestaBase
    {

    }

    [DataContract]
    public class RespuestaUpdateProd : RespuestaBase
    {

    }

    [DataContract]
    public class RespuestaDeleteProd : RespuestaBase
    {

    }
}
