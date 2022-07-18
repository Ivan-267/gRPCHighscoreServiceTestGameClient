using Godot;
using System;
using System.Collections.Generic;
using Score = HighscoreServiceClient.Models.Score;

namespace HighscoreServiceClient.Scenes.ScoreScene
{
    public partial class ScoreList : ItemList
    {
        /// <summary>
        /// Number of scores in the list.
        /// </summary>
        public int ScoreCount => _scoreCount;
        private int _scoreCount;

        public override void _Ready()
        {
            Connect("resized", this, nameof(OnResized));
            AddColumnTitles();
        }

        public void OnResized()
        {
            // Padding used to ensure that all columns are aligned in the same row. 
            const int paddingWidth = 20;
            FixedColumnWidth = (int)RectSize.x / MaxColumns - paddingWidth;
        }

        private void AddColumnTitles()
        {
            foreach (string colummName in Enum.GetNames(typeof(Column)))
            {
                AddItem($"{colummName}: ");
            }
        }
        public void RemoveAllScores()
        {
            _scoreCount = 0;
            Clear();
            AddColumnTitles();
        }
        public void AddScore(Score score, int highlightScoreId)
        {
            _scoreCount++;

            foreach (Column column in Enum.GetValues(typeof(Column)))
            {
                switch (column)
                {
                    case Column.Position:
                        AddItem($"{_scoreCount}");
                        break;
                    case Column.Player:
                        AddItem($"{score.PlayerName}");
                        break;
                    case Column.Score:
                        AddItem($"{score.Points}");
                        break;
                    case Column.Date:
                        AddItem($"{score.TimeCreated}");
                        break;
                    default:
                        break;
                }
            }

            if (score.Id == highlightScoreId)
            {
                FocusOnScore(_scoreCount, Column.Player);
            }
        }

        /// <summary>
        /// Focuses on and highlights a score in the list.
        /// </summary>
        /// <param name="scoreId">ID of the score to focus on.</param>
        /// <param name="column">Which column to highlight.</param>
        private void FocusOnScore(int scoreId, Column column)
        {
            Select(scoreId * (MaxColumns) + (int)column);
            EnsureCurrentIsVisible();
        }

        /// <summary>
        /// Adds scores to the list and optionally highlights a score.
        /// </summary>
        /// <param name="scores"></param>
        /// <param name="highlightScoreId">(Optional) Highlights the score in the list. 
        /// Score Ids range from 1 being the top score to the score count being the last score in the list.</param>
        public void AddScores(IEnumerable<Score> scores, int highlightScoreId = 0)
        {
            foreach (var score in scores)
            {
                AddScore(score, highlightScoreId);
            }
        }
    }
}