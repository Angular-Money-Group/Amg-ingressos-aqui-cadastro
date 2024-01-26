using Amg_ingressos_aqui_cadastro_api.Services;
using NUnit.Framework;
using Amg_ingressos_aqui_cadastro_api.Controllers;


namespace Prime.UnitTests.Controllers
{
    public class ReceiptAccountControllerTest
    {
        private ReceiptAccountController _receiptAccountController;
        private ReceiptAccountService _receiptAccountService;


        [SetUp]
        public void SetUp()
        {
            this._receiptAccountController = new ReceiptAccountController(this._receiptAccountService);
        }
    }
}