using System;
using GameCatcher.Models;
using IGDB;

namespace GameCatcher.Services;

public class IGDBService
{
    private readonly IGDBClient _igdbClient;
    private readonly Game _game;

    private string GameQueryFields { get; set; } = "id, name, summary, artworks.image_id";
    private string PopularityPrimitiveFields { get; set; } = "id, game_id, popularity_type, value";

    private string GameEndpoint { get; set; } = IGDBClient.Endpoints.Games;
    private string PopularityPrimitiveEntpoint { get; set; } =
        IGDBClient.Endpoints.PopularityPrimitives;

    public IGDBService()
    {
        var clientId = Environment.GetEnvironmentVariable("IGDB_CLIENT_ID");
        var clientSecret = Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET");

        _igdbClient = new IGDBClient(clientId!, clientSecret!);
        _game = new Game();
    }

    public async Task<List<IGDB.Models.PopularityPrimitive>> GetGamesByTypeAsync(
        int limit,
        int popularityType
    )
    {
        var popularityPrimitive = await _igdbClient.QueryAsync<IGDB.Models.PopularityPrimitive>(
            PopularityPrimitiveEntpoint,
            query: $"fields {PopularityPrimitiveFields} limit {limit}; where popularity_type = {popularityType};"
        );

        return popularityPrimitive.ToList();
    }

    public async Task<List<IGDB.Models.Game>> GetGamesByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids?.Where(i => i > 0).Distinct().ToList();

        if (idList == null || !idList.Any())
        {
            return new List<IGDB.Models.Game>();
        }

        var query =
            $"fields {GameQueryFields}; where id ({string.Join(',', idList)}); limit {idList.Count}";
        var games = await QueryWithRetryAsync<IGDB.Models.Game>(GameEndpoint, query);

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
                if (attempt > maxRetries || (ex.Message?.Contains("429") != true))
                {
                    throw;
                }

                var delayMs = (int)(Math.Pow(2, attempt) * 500) + new Random().Next(100, 500);
                await Task.Delay(delayMs);
                continue;
            }
        }
    }
}
