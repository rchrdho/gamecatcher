using System;
using GameCatcher.Models;
using IGDB;
using IGDB.Models;
using Microsoft.Extensions.Configuration;

namespace GameCatcher.Services;

public class IgdbService
{
    private readonly IGDBClient _igdbClient;
    private readonly GameDetails _gameDetails;
    private readonly PeakGames _peakGames;

    public IgdbService()
    {
        var clientId = Environment.GetEnvironmentVariable("IGDB_CLIENT_ID");
        var clientSecret = Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET");

        _igdbClient = new IGDBClient(clientId!, clientSecret!);
        _gameDetails = new GameDetails();
        _peakGames = new PeakGames();
    }

    public async Task<List<PopularityPrimitive>> GetGamesByTypeAsync(int limit, int popularityType)
    {
        var popularityPrimitives = await _igdbClient.QueryAsync<PopularityPrimitive>(
            IGDBClient.Endpoints.PopularityPrimitives,
            query: $"fields id, game_id, popularity_type, value; limit {limit}; where popularity_type = {popularityType};"
        );
        return popularityPrimitives.ToList();
    }

    public async Task<List<Game>> GetGamesByIdsAsync(IEnumerable<int> ids, string fields)
    {
        var idList = ids?.Where(i => i > 0).Distinct().ToList();
        if (idList == null || !idList.Any())
            return new List<Game>();

        var q = $"fields {fields}; where id = ({string.Join(',', idList)}); limit {idList.Count};";
        var games = await QueryWithRetryAsync<Game>(IGDBClient.Endpoints.Games, q);
        return games;
    }

    private async Task<List<T>> QueryWithRetryAsync<T>(
        string endpoint,
        string query,
        int maxRetries = 4
    )
    {
        int attempt = 0;
        while (true)
        {
            try
            {
                var result = await _igdbClient.QueryAsync<T>(endpoint, query);
                return result.ToList();
            }
            catch (RestEase.ApiException ex)
            {
                attempt++;
                // simple detection for 429 (rate limit). If other transient errors you can extend this.
                if (attempt > maxRetries || (ex.Message?.Contains("429") != true))
                    throw;

                // exponential backoff with jitter
                var delayMs = (int)(Math.Pow(2, attempt) * 500) + new Random().Next(100, 500);
                await Task.Delay(delayMs);
                continue;
            }
        }
    }
}
