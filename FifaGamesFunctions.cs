using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudentFunctions.Models.FifaGames;

namespace FifaGame.Function;

public class FifaGamesFunctions
{
    private readonly ILogger<FifaGamesFunctions> _logger;

    private readonly FifaGamesContext _context; 

    public FifaGamesFunctions(ILogger<FifaGamesFunctions> logger , FifaGamesContext context)
    {
        _logger = logger;
        _context = context; 
    }

    [Function("FifaGamesFunctions")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }

    [Function("GetFifaGames")]
    // Route suggests that our endpoint is /api/fifagames
    public HttpResponseData GetFifaGames(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fifagames")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP GET/posts trigger function processed a request in GetFifaGames().");

        var games = _context.Games.ToArray();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json");

        response.WriteStringAsync(JsonConvert.SerializeObject(games));

        return response;
    }
}
