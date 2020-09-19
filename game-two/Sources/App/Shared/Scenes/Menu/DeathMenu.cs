using Godot;
using System;

public class DeathMenu : CanvasLayer
{
    private Button restartButton;
    private Button mainMenuButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        restartButton = ((Button) this.GetNode("RestartButton"));
        mainMenuButton = ((Button) this.GetNode("MainMenuButton"));

        restartButton.Connect("pressed", this, nameof(RestartGame));
        mainMenuButton.Connect("pressed", this, nameof(GotoMainMenu));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
    }

    public void RestartGame()
    {
        GetTree().ChangeScene("res://Sources/App/Game-Scenes/000-test/test.tscn");
    }

    public void GotoMainMenu()
    {
        GetTree().ChangeScene("res://Sources/App/Shared/Scenes/Menu/MainMenu.tscn");
    }
}
