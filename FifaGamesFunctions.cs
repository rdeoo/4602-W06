using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FifaGame.Function;

public class FifaGamesFunctions
{
    private readonly ILogger<FifaGamesFunctions> _logger;

    public FifaGamesFunctions(ILogger<FifaGamesFunctions> logger)
    {
        _logger = logger;
    }

    [Function("FifaGamesFunctions")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
