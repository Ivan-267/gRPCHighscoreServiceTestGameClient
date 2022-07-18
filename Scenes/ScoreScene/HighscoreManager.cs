using Godot;
using System.Threading.Tasks;

namespace HighscoreServiceClient.Scenes.ScoreScene
{
    public class HighscoreManager : Control
    {
        [Export]
        public NodePath TabContainerPath;
        private TabContainer _tabContainer;

        [Export]
        public NodePath ScoreLabelPath;
        private Label _scoreLabel;

        [Export]
        public NodePath AllScoresListPath;
        private ScoreList _allScores;

        [Export]
        public NodePath Top10ScoresListPath;
        private ScoreList _top10Scores;

        [Export]
        public NodePath SubmitScoreButtonPath;
        private Button _submitScore;

        [Export]
        public NodePath PlayAgainButtonPath;
        private Button _playAgain;

        [Export]
        public NodePath NicknameLineEditPath;
        public LineEdit _nickname;

        [Export]
        public NodePath ErrorDisplayManagerPath;
        public ErrorDisplayManager _errorDisplayManager;

        private Gateway _gateway;

        private int _submittedScoreId;

        public override void _Ready()
        {
            _scoreLabel = GetNode<Label>(ScoreLabelPath);
            _scoreLabel.Text = $"Your score is: {LocalScore.LastGameScore}";

            _tabContainer = GetNode<TabContainer>(TabContainerPath);
            _tabContainer.Connect("tab_changed", this, nameof(OnTabChanged));

            _allScores = GetNode<ScoreList>(AllScoresListPath);
            _top10Scores = GetNode<ScoreList>(Top10ScoresListPath);

            _submitScore = GetNode<Button>(SubmitScoreButtonPath);
            _submitScore.Connect("pressed", this, nameof(OnSubmitScoreButtonPressed));

            _playAgain = GetNode<Button>(PlayAgainButtonPath);
            _playAgain.Connect("pressed", this, nameof(OnPlayAgainButtonPressed));

            _nickname = GetNode<LineEdit>(NicknameLineEditPath);

            _errorDisplayManager = GetNode<ErrorDisplayManager>(ErrorDisplayManagerPath);

            _gateway = new Gateway();
            Connect("tree_exiting", this, nameof(OnTreeExiting));
        }

        private void OnPlayAgainButtonPressed()
        {
            SceneChanger.ChangeSceneTo(Scene.GameScene, GetTree());
        }

        private async void OnSubmitScoreButtonPressed()
        {
            if (IsNicknameValid() == false)
            {
                _errorDisplayManager.DisplayErrorPopup(ErrorPopup.InvalidNickname);
                _nickname.GrabFocus();
                return;
            }

            _submitScore.Disabled = true;

            try
            {
                _submittedScoreId = await _gateway.CreateScoreAsync(_nickname.Text, LocalScore.LastGameScore);
            }
            catch
            {
                _errorDisplayManager.DisplayErrorPopup(ErrorPopup.ServerError);
            }
        }

        private bool IsNicknameValid()
        {
            return _nickname.Text.Length > 0;
        }

        private void OnTreeExiting()
        {
            _gateway?.Dispose();
        }

        public async void OnTabChanged(int tabId)
        {
            string tabTitle = _tabContainer.GetTabTitle(tabId);

            if (tabTitle == _allScores.Name)
            {
                _allScores.RemoveAllScores();
                await PopulateListWithAllScores(_allScores);
            }
            else if (tabTitle == _top10Scores.Name)
            {
                _top10Scores.RemoveAllScores();
                await PopulateListWithScores(_top10Scores, 10);
            }

        }
        private async Task PopulateListWithScores(ScoreList scoreList, int count)
        {
            try
            {
                var scores = await _gateway.GetTopScoresAsync(count);
                scoreList.AddScores(scores, _submittedScoreId);
            }
            catch
            {
                _errorDisplayManager.DisplayErrorPopup(ErrorPopup.ServerError);
            }
        }
        private async Task PopulateListWithAllScores(ScoreList scoreList)
        {
            try
            {
                var scores = await _gateway.GetAllScoresAsync();
                scoreList.AddScores(scores, _submittedScoreId);
            }
            catch
            {
                _errorDisplayManager.DisplayErrorPopup(ErrorPopup.ServerError);
            }
        }

    }
}