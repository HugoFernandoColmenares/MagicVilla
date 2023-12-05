using System.Net;

namespace MagicVilla_API.Modelos
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorsMessages = new List<string>();
        }

        public HttpStatusCode StatusCode {  get; set; }
        public bool IsExitoso { get; set; } = true;
        public List<string> ErrorsMessages { get; set; }
        public object Resultado { get; set; }
        public int TotalPaginas { get; set; }
    }
}
