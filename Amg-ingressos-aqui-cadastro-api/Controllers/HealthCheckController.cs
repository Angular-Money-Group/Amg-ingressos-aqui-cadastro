using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class HealthCheckController : ControllerBase
{

    public HealthCheckController()
    {
    }

}
