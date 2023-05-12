namespace Amg_ingressos_aqui_cadastro_api.Infra
{
    public class TransactionDatabaseSettings
    {
        /// <summary>
        /// Connection string base de dados Mongo
        /// </summary>
        public string ConnectionString { get; set; } = null!;
        /// <summary>
        /// Nome base de dados Mongo
        /// </summary>
        public string DatabaseName { get; set; } = null!;
        /// <summary>
        /// Nome collection Mongo
        /// </summary>
        public string TransactionCollectionName { get; set; } = null!;
    }
}