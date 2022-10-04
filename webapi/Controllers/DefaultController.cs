using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers;

[ApiController]
[Route("/")]
public class DefaultController : ControllerBase
{
    private readonly ILogger<DefaultController> _logger;

    public DefaultController(ILogger<DefaultController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        _logger.LogInformation("Default controller action hit.");
        return "Hello World.";
    }
}
