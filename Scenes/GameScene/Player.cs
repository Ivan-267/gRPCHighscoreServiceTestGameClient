using Godot;

namespace HighscoreServiceClient.Scenes.GameScene
{
    public class Player : Area2D
    {
        [Export]
        public float MovementSpeed { get; set; }

        public override void _PhysicsProcess(float delta)
        {
            Move(delta);
        }

        private void Move(float delta)
        {
            Vector2 movementDirection = GetMovementDirectionFromInput();
            Position += movementDirection * MovementSpeed * delta;
            WrapAroundScreen();
        }

        private static Vector2 GetMovementDirectionFromInput()
        {
            Vector2 movementDirection = default;

            if (Input.IsActionPressed("player_left"))
            {
                movementDirection += Vector2.Left;
            }

            if (Input.IsActionPressed("player_right"))
            {
                movementDirection += Vector2.Right;
            }

            if (Input.IsActionPressed("player_up"))
            {
                movementDirection += Vector2.Up;
            }

            if (Input.IsActionPressed("player_down"))
            {
                movementDirection += Vector2.Down;
            }

            return movementDirection;
        }

        private void WrapAroundScreen()
        {
            Position = Position.PosMod(OS.WindowSize);
        }
    }
}