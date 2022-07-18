using Godot;
using System;
using Path = System.IO.Path;

public static class SceneChanger
{
    private const string _sceneBasePath = "res://Scenes";
    private static readonly string _mainScenePath = Path.Combine(_sceneBasePath, "MainScene", "MainScene.tscn");
    private static readonly string _gameScenePath = Path.Combine(_sceneBasePath, "GameScene", "GameScene.tscn");
    private static readonly string _scoreScenePath = Path.Combine(_sceneBasePath, "ScoreScene", "ScoreScene.tscn");

    /// <summary>
    /// Changes the active scene.
    /// </summary>
    /// <param name="scene">Scene to change to.</param>
    /// <param name="sceneTree">Reference to the current scene tree.</param>
    public static void ChangeSceneTo(Scene scene, SceneTree sceneTree)
    {
        switch (scene)
        {
            case Scene.MainScene:
                sceneTree.ChangeScene(_mainScenePath);
                break;
            case Scene.GameScene:
                sceneTree.ChangeScene(_gameScenePath);
                break;
            case Scene.ScoreScene:
                sceneTree.ChangeScene(_scoreScenePath);
                break;
            default:
                throw new ArgumentException($"Invalid scene provided: {scene}", nameof(scene));
        }
    }
}
