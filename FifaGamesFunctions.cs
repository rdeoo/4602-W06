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

    [Function("GetFifaGamesById")]
    public HttpResponseData GetFifaGamesById
    (
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "fifagames/{id}")] HttpRequestData req,
        int id
    )
    {
        _logger.LogInformation("C# HTTP GET/posts trigger function processed a request.");
        var game = _context.Games.FindAsync(id).Result;
        if(game == null)
        {
            var response = req.CreateResponse(HttpStatusCode.NotFound);
            response.Headers.Add("Content-Type", "application/json");
            response.WriteStringAsync("Not Found");
            return response;
        }
        var response2 = req.CreateResponse(HttpStatusCode.OK);
        response2.Headers.Add("Content-Type", "application/json");
        response2.WriteStringAsync(JsonConvert.SerializeObject(game));
        return response2;
    }

    [Function("CreateFifaGame")]
    public HttpResponseData CreateFifaGame
    (
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "fifagames")] HttpRequestData req
    )
    {
        _logger.LogInformation("C# HTTP POST/posts trigger function processed a request.");
        var game = JsonConvert.DeserializeObject<Game>(req.ReadAsStringAsync().Result);
        _context.Games.Add(game);
        _context.SaveChanges();
        var response = req.CreateResponse(HttpStatusCode.Created);
        response.Headers.Add("Content-Type", "application/json");
        response.WriteStringAsync(JsonConvert.SerializeObject(game));
        return response;
    }

    [Function("UpdateFifaGame")]
    public HttpResponseData UpdateFifaGame
    (
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "fifagames/{id}")] HttpRequestData req,
        int id
    )
    {
        _logger.LogInformation("C# HTTP PUT/posts trigger function processed a request.");
        var game = _context.Games.FindAsync(id).Result;
        if (game == null)
        {
            var response = req.CreateResponse(HttpStatusCode.NotFound);
            response.Headers.Add("Content-Type", "application/json");
            response.WriteStringAsync("Not Found");
            return response;
        }

        var game2 = JsonConvert.DeserializeObject<Game>(req.ReadAsStringAsync().Result);
        game.GameId = game2.GameId;
        game.Year = game2.Year;
        game.Gender = game2.Gender;
        game.City = game2.City; 
        game.Country = game2.Country; 
        game.Continent = game2.Continent; 
        game.Winner = game2.Winner; 
        game.Created = game2.Created; 

        _context.SaveChanges();
        var response2 = req.CreateResponse(HttpStatusCode.OK);
        response2.Headers.Add("Content-Type", "application/json");
        response2.WriteStringAsync(JsonConvert.SerializeObject(game));
        return response2;
    }

    [Function("DeleteFifaGame")]
    public HttpResponseData DeleteFifaGame
    (
      [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "fifagames/{id}")] HttpRequestData req,
      int id
    )
    {
        _logger.LogInformation("C# HTTP DELETE/posts trigger function processed a request.");
        var game = _context.Games.FindAsync(id).Result;
        if (game == null)
        {
            var response = req.CreateResponse(HttpStatusCode.NotFound);
            response.Headers.Add("Content-Type", "application/json");
            response.WriteStringAsync("Not Found");
            return response;
        }
        _context.Games.Remove(game);
        _context.SaveChanges();
        var response2 = req.CreateResponse(HttpStatusCode.OK);
        response2.Headers.Add("Content-Type", "application/json");
        response2.WriteStringAsync(JsonConvert.SerializeObject(game));
        return response2;
    }
}
