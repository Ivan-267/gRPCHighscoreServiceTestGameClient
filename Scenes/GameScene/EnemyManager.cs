using Godot;

namespace HighscoreServiceClient.Scenes.GameScene
{
    public class EnemyManager : Node
    {
        [Export]
        public NodePath PlayerNodePath { get; set; }
        private Player _player;

        [Export]
        public float MovementSpeed { get; set; }

        [Export]
        public float StateDuration { get; set; }

        public override void _Ready()
        {
            _player = GetNode<Player>(PlayerNodePath);
            ConfigureEnemies(_player);
        }

        /// <summary>
        /// Configures enemy instances.
        /// </summary>
        /// <param name="player">Reference to the player node in the current scene.</param>
        private void ConfigureEnemies(Player player)
        {
            foreach (Enemy enemy in GetChildren())
            {
                enemy.PlayerNode = player;
                enemy.MovementSpeed = MovementSpeed;
                enemy.StateDuration = StateDuration;
            }
        }
    }
}