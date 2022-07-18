using Godot;

namespace HighscoreServiceClient.Scenes.GameScene
{
    public class ScoreManager : Node
    {
        [Export]
        NodePath ScoreLabelPath { get; set; }
        private Label _scoreLabel;

        private float _score;
        /// <summary>
        /// Current score as integer, used for displaying and saving the score.
        /// </summary>
        public int Score => (int)_score;
        public override void _Ready()
        {
            _scoreLabel = GetNode<Label>(ScoreLabelPath);
        }

        public override void _PhysicsProcess(float delta)
        {
            IncreaseScore(delta);
            UpdateScoreLabelText();
        }

        private void IncreaseScore(float delta)
        {
            _score += delta;
        }

        private void UpdateScoreLabelText()
        {
            _scoreLabel.Text = $"Score: {Score}";
        }
    }
}