namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class MessageReturn
    {

        public MessageReturn() {
            Message = string.Empty;
            Data = null;
        }
        public MessageReturn(string message) {
            Message = message;
            Data = null;
        }
        public MessageReturn(object data) {
            Message = string.Empty;
            Data = data;
        }

        /// <summary>
        /// Mensagem de retorno
        /// </summary>
        public string? Message;

        /// <summary>
        /// Objeto de dados retornado
        /// </summary>
        public object? Data;

        // METHODS
        public bool hasRunnedSuccessfully() {
            return string.IsNullOrEmpty(Message) && (Data is not null);
        }
    }
}