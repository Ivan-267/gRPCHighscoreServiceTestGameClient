using Godot;

namespace HighscoreServiceClient.Scenes.GameScene
{
    public class CollisionManager : Node
    {
        [Export]
        public NodePath ScoreManagerPath { get; set; }
        private ScoreManager _scoreManager;

        [Export]
        public NodePath PlayerNodePath { get; set; }
        private Player _player;

        [Export]
        public NodePath EnemyManagerNodePath { get; set; }
        private EnemyManager _enemyManager;

        public override void _Ready()
        {
            GetRequiedNodes();
            ConnectCollisionSignals();
        }

        /// <summary>
        /// Gets the instances of required nodes in the current scene and assigns references to them.
        /// </summary>
        private void GetRequiedNodes()
        {
            _scoreManager = GetNode<ScoreManager>(ScoreManagerPath);
            _player = GetNode<Player>(PlayerNodePath);
            _enemyManager = GetNode<EnemyManager>(EnemyManagerNodePath);
        }

        /// <summary>
        /// Connects player and enemy collision signals to handler methods.
        /// </summary>
        private void ConnectCollisionSignals()
        {
            _player.Connect("area_entered", this, nameof(OnPlayerCollision));
            foreach (Enemy enemy in _enemyManager.GetChildren())
            {
                enemy.Connect("area_entered", this, nameof(OnEnemyCollision), new Godot.Collections.Array(enemy));
            }
        }

        /// <summary>
        /// Handles collision signals received from Player.
        /// </summary>
        /// <param name="area">The area that collided with the player.</param>
        private void OnPlayerCollision(Area2D area)
        {
            if (area is Enemy)
            {
                LocalScore.LastGameScore = _scoreManager.Score;
                SwitchToHighscoreScene();
            }
        }

        /// <summary>
        /// Handles collision signals received from Enemy instances.
        /// </summary>
        /// <param name="area">The area that collided with the enemy.</param>
        /// <param name="sender">The enemy that sent the collision signal.</param>
        private void OnEnemyCollision(Area2D area, Enemy sender)
        {
            if (area is Enemy enemy)
            {
                enemy.Bounce(normal: (enemy.GlobalPosition - sender.GlobalPosition).Normalized());
            }
        }

        private void SwitchToHighscoreScene()
        {
            SceneChanger.ChangeSceneTo(Scene.ScoreScene, GetTree());
        }
    }
}