using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Grpc.Services;
using HighscoreServiceClient.gRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Empty = Google.Protobuf.WellKnownTypes.Empty;
using Score = HighscoreServiceClient.Models.Score;
using ScoreResource = Grpc.Services.Score;

namespace HighscoreServiceClient
{
    /// <summary>
    /// Handles communication with the highscore service.
    /// </summary>
    internal class Gateway : IDisposable
    {
        private readonly GrpcChannel _grpcChannel;
        private readonly HighscoreService.HighscoreServiceClient _highscoreServiceClient;
        private bool disposedValue;
        public Gateway()
        {
            _grpcChannel = GrpcChannel.ForAddress(Config.gRPCServerAddress, new GrpcChannelOptions
            {
                HttpHandler = new GrpcWebHandler(new HttpClientHandler()),
                DisposeHttpClient = true
            });
            _highscoreServiceClient = new HighscoreService.HighscoreServiceClient(_grpcChannel);
        }

        internal async Task<int> CreateScoreAsync(string playerName, int points)
        {
            var response = await _highscoreServiceClient.CreateScoreAsync(new CreateScoreRequest()
            {
                PlayerName = playerName,
                Points = points,
            });

            return response.Id;
        }

        internal async Task<IEnumerable<Score>> GetAllScoresAsync()
        {
            var scores = (await _highscoreServiceClient.GetAllScoresAsync(new Empty())).Scores;
            return scores.Select(scoreResource => ConvertToScore(scoreResource)).ToList();
        }

        internal async Task<Score> GetScoreByIdAsync(int scoreId)
        {
            return ConvertToScore((await _highscoreServiceClient.GetScoreByIdAsync(new GetScoreByIdRequest()
            {
                Id = scoreId
            })).Score);
        }

        internal async Task<IEnumerable<Score>> GetScoresByPlayerAsync(string playerName)
        {
            var scores = (await _highscoreServiceClient.GetScoresByPlayerAsync(new GetScoresByPlayerRequest()
            {
                PlayerName = playerName
            })).Scores;

            return scores.Select(scoreResource => ConvertToScore(scoreResource)).ToList();
        }

        internal async Task<IEnumerable<Score>> GetTopScoresAsync(int numberOfScores)
        {
            var scores = (await _highscoreServiceClient.GetTopScoresAsync(new GetTopScoresRequest()
            {
                NumberOfScores = numberOfScores
            })).Scores;

            return scores.Select(scoreResource => ConvertToScore(scoreResource)).ToList();
        }

        private Score ConvertToScore(ScoreResource scoreResource)
        {
            return new Score
            (
                id: scoreResource.Id,
                points: scoreResource.Points,
                playerName: scoreResource.PlayerName,
                timeCreated: scoreResource.CreateTime.ToDateTime()
            );
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _grpcChannel.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}
