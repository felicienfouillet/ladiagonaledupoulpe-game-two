using Godot;
using System;

public class MainMenu : CanvasLayer
{
    private Button startButton;
    private Button joinButton;
    private Button quitButton;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        startButton = ((Button) this.GetNode("StartButton"));
        joinButton = ((Button) this.GetNode("JoinButton"));
        quitButton = ((Button) this.GetNode("QuitButton"));
        
        startButton.Connect("pressed", this, nameof(StartGame));
        joinButton.Connect("pressed", this, nameof(JoinGame));
        quitButton.Connect("pressed", this, nameof(Quit));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
    }

    public void StartGame()
    {
        GetTree().ChangeScene("res://Sources/App/Game-Scenes/000-test/test.tscn");
    }

    public void JoinGame()
    {
        // GetTree().ChangeScene("res://Sources/App/Game-Scenes/000-test/test.tscn");
    }

    public void Quit()
    {
        GetTree().Quit();
    }
}
