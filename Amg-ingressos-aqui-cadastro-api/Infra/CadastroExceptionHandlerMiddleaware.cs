using System.Net;
using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Amg_ingressos_aqui_cadastro_api.Model;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_cadastro_api.Infra
{
    public class CadastroExceptionHandlerMiddleaware: AbstractExceptionHandlerMiddleware
    {
        public CadastroExceptionHandlerMiddleaware(RequestDelegate next) : base(next)
        {

        }

        public override (HttpStatusCode code, string message) GetResponse(Exception exception)
        {
            HttpStatusCode code;
            switch (exception)
            {
                case GetException:
                    code = HttpStatusCode.NotFound;
                    break;
                case IdMongoException:
                    code = HttpStatusCode.Conflict;
                    break;
                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case RuleException
                    or DeleteException
                    or EditException
                    or IdMongoException
                    or ArgumentException
                    or InvalidOperationException:
                    code = HttpStatusCode.BadRequest;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }
            return (code, JsonConvert.SerializeObject(new MessageReturn() { Message = exception.Message }));
        }
    }
}