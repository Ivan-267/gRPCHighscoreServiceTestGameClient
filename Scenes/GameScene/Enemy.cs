using Godot;
using System;

namespace HighscoreServiceClient.Scenes.GameScene
{
    public partial class Enemy : Area2D
    {
        public Player PlayerNode { get; set; }
        public float MovementSpeed { get; set; }
        /// <summary>
        /// Duration of each state in seconds. Used for automatic cycling between states.
        /// </summary>
        public float StateDuration { get; set; }

        private State _currentState;
        private float _stateSwitchTimer;

        Vector2 _movementDirection;
        Random _random;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _random = new Random();
            SetState(State.MovingTowardsRandomPosition);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            HandleMovement(delta);
            HandleStateCycling(delta, StateDuration);
        }

        private void HandleMovement(float delta)
        {
            Move(_movementDirection, delta);
        }
        private void HandleStateCycling(float delta, float stateDuration)
        {
            _stateSwitchTimer += delta;
            if (_stateSwitchTimer >= stateDuration)
            {
                SwitchState();
                _stateSwitchTimer = 0;
            }
        }

        /// <summary>
        /// Moves the enemy in the provided direction.
        /// If the enemy is outside of the screen, it will be wrapped around.
        /// </summary>
        private void Move(Vector2 movementDirection, float delta)
        {
            Position += movementDirection * MovementSpeed * delta;
            WrapAroundScreen();
        }

        private void WrapAroundScreen()
        {
            Position = Position.PosMod(OS.WindowSize);
        }

        private void SwitchState()
        {
            switch (_currentState)
            {
                case State.MovingTowardsPlayer:
                    SetState(State.MovingTowardsRandomPosition);
                    break;
                case State.MovingTowardsRandomPosition:
                    SetState(State.MovingTowardsPlayer);
                    break;
                default:
                    throw new Exception($"Enemy is in invalid state. Current state: {_currentState}");
            }
        }

        private void SetState(State state)
        {
            switch (state)
            {
                case State.MovingTowardsPlayer:
                    _movementDirection = GetDirectionTowardPlayer();
                    break;
                case State.MovingTowardsRandomPosition:
                    _movementDirection = GetRandomDirection();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), $"Invalid state provided: {state}");
            }

            _currentState = state;
        }

        private Vector2 GetDirectionTowardPlayer()
        {
            return GlobalPosition.DirectionTo(PlayerNode.GlobalPosition);
        }

        public Vector2 GetRandomDirection()
        {
            return new Vector2(x: (float)(1 - _random.NextDouble() % 3),
                               y: (float)(1 - _random.NextDouble() % 3)).Normalized();
        }

        public void Bounce(Vector2 normal)
        {
            _movementDirection = _movementDirection.Bounce(normal);
        }
    }

}