using Amg_ingressos_aqui_cadastro_api.Exceptions;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class MessageReturn
    {

        public MessageReturn()
        {
            Message = string.Empty;
            Data = string.Empty;
        }

        /// <summary>
        /// Mensagem de retorno
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Objeto de dados retornado
        /// </summary>
        public object Data { get; set; }

        // METHODS
        public bool HasRunnedSuccessfully()
        {
            return string.IsNullOrEmpty(Message) && (Data is not null);
        }
        public T ToObject<T>()
        {
            return (T)this.Data;
        }
        public List<T> ToListObject<T>()
        {
            return (List<T>)this.Data;
        }
        public T JsonToModel<T>()
        {
            return JsonConvert.DeserializeObject<T>(this.Data.ToString() ?? string.Empty) ?? throw new ConvertException("Não foi possivel converter.");
        }
    }
}