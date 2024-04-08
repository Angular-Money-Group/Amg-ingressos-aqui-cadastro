using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amg_ingressos_aqui_cadastro_api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize(Policy = "PublicSecure")]
public class HealthCheckController : ControllerBase
{

    public HealthCheckController()
    {
    }

}
