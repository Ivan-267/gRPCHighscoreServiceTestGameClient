using Godot;

namespace HighscoreServiceClient.Scenes.MainScene
{
    public class MainSceneManager : Control
    {
        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey)
            {
                SceneChanger.ChangeSceneTo(Scene.GameScene, GetTree());
            }
        }
    }
}