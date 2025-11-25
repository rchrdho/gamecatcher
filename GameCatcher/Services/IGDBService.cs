using System;
using GameCatcher.Models;
using IGDB;

namespace GameCatcher.Services;

public class IGDBService
{
    private readonly IGDBClient _igdbClient;

    // Query Fields
    public string GameQueryFields { get; set; } =
        "id, name, summary, artworks.image_id, genres, first_release_date, total_rating";
    public string PopularityPrimitiveQueryFields { get; set; } =
        "id, game_id, popularity_type, value";

    // Endpoints
    public string IGDBGameEndpoint { get; set; } = IGDBClient.Endpoints.Games;
    public string IGDBPopularityPrimitivesEndpoint { get; set; } =
        IGDBClient.Endpoints.PopularityPrimitives;

    public IGDBService()
    {
        var clientId = Environment.GetEnvironmentVariable("IGDB_CLIENT_ID");
        var clientSecret = Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET");

        _igdbClient = new IGDBClient(clientId!, clientSecret!);
    }

    public async Task<List<IGDB.Models.PopularityPrimitive>> GetGamesByTypeAsync(
        int limit,
        int popularityType
    )
    {
        var popularityPrimitives = await _igdbClient.QueryAsync<IGDB.Models.PopularityPrimitive>(
            IGDBPopularityPrimitivesEndpoint,
            query: $"fields {PopularityPrimitiveQueryFields}; limit {limit}; where popularity_type = {popularityType};"
        );
        return popularityPrimitives.ToList();
    }

    public async Task<IGDB.Models.Game> GetGameBySingleIdAsync(long gameId)
    {
        // var game = await _igdbClient.QueryAsync<IGDB.Models.Game>(
        //     IGDBGameEndpoint,
        //     query: $"fields {GameQueryFields}; where id = {gameId}"
        // );

        var query = $"fields {GameQueryFields}; where id = {gameId};";
        var game = await QueryWithRetryAsync<IGDB.Models.Game>(IGDBGameEndpoint, query);

        return game.FirstOrDefault()!;
    }

    public async Task<List<IGDB.Models.Game>> GetGamesByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids?.Where(i => i > 0).Distinct().ToList();

        if (idList == null || !idList.Any())
            return new List<IGDB.Models.Game>();

        var query =
            $"fields {GameQueryFields}; where id = ({string.Join(',', idList)}); limit {idList.Count};";
        var games = await QueryWithRetryAsync<IGDB.Models.Game>(IGDBGameEndpoint, query);
        return games;
    }

    public async Task<List<IGDB.Models.Game>> SearchGamesAsync(string searchTerm, int limit = 20)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return new List<IGDB.Models.Game>();

        // Escape special characters and prepare search term
        var escapedTerm = searchTerm.Replace("\\", "\\\\").Replace("\"", "\\\"");
        var query = $"fields {GameQueryFields}; search \"{escapedTerm}\"; limit {limit};";
        
        var games = await QueryWithRetryAsync<IGDB.Models.Game>(IGDBGameEndpoint, query);
        return games;
    }

    private async Task<List<T>> QueryWithRetryAsync<T>(
        string endpoint,
        string query,
        int maxRetries = 5
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
                    throw;

                var delayMs = (int)(Math.Pow(2, attempt) * 500) + new Random().Next(500, 1000);
                await Task.Delay(delayMs);
                continue;
            }
        }
    }
}
