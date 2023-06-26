namespace Amg_ingressos_aqui_cadastro_api.Model
{
    public class MessageReturn
    {

        public MessageReturn() {
            this.Message = string.Empty;
            this.Data = null;
        }
        public MessageReturn(string message) {
            this.Message = message;
            this.Data = null;
        }
        public MessageReturn(object data) {
            this.Message = string.Empty;
            this.Data = data;
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
            return string.IsNullOrEmpty(this.Message) && (this.Data is not null);
        }
    }
}