using Godot;
using System;

public class Main : Node
{
    // Initialized audio node variables ----------

    // SFX
    public AudioStreamPlayer mouseHoverSound;
    public AudioStreamPlayer clickPlaySound;
    public AudioStreamPlayer quitSound;
    public AudioStreamPlayer clickSettingsSound;

    // Music
    public AudioStreamPlayer mainMenuMusic;
    public AudioStreamPlayer inGameMusic;

    // Variables for loading/unloading the game levels ----------

    // Levels is an initialized child node
    private Node2D _levels;

    // Level instance is the current Node2D scene that will be under the levels node right above, which should contain the level
    private Node2D _levelInstance;

    // Initialize all child node references ----------
    public override void _Ready()
    {
        mouseHoverSound = GetNode<AudioStreamPlayer>("SFX/MouseHoverSound");
        clickPlaySound = GetNode<AudioStreamPlayer>("SFX/ClickPlaySound");
        quitSound = GetNode<AudioStreamPlayer>("SFX/QuitSound");
        clickSettingsSound = GetNode<AudioStreamPlayer>("SFX/ClickSettingsSound");

        mainMenuMusic = GetNode<AudioStreamPlayer>("Music/MainMenuMusic");
        inGameMusic = GetNode<AudioStreamPlayer>("Music/InGameMusic");

        _levels = GetNode<Node2D>("Levels");
    }

    // Methods to handle loading and unloading levels ----------

    public void UnloadLevel()
    {
        if (IsInstanceValid(_levelInstance))
        {
            _levelInstance.QueueFree();
        }
    }

    public void LoadLevel(String levelName)
    {
        UnloadLevel();

        // This assumes that the level scene is in a folder called 'Levels' that is under res://
        String levelPath = "res://Levels/" + levelName + ".tscn";
        PackedScene levelResource = GD.Load<PackedScene>(levelPath);

        if (levelResource != null)
        {
            _levelInstance = levelResource.Instance<Node2D>();
            _levels.AddChild(_levelInstance);
        }
    }

    // SIGNAL METHODS ----------

    // BUTTON SIGNALS =========

    // Signal used on all menu buttons to indicate that the mouse is hovering over a button by playing a sound.
    public void _on_Button_mouse_entered()
    {
        mouseHoverSound.Play();
    }

    public void _on_StartButton_pressed()
    {
        clickPlaySound.Play();
    }

    public void _on_QuitButton_pressed()
    {
        quitSound.Play();

        GetTree().Quit();
    }

    public void _on_SettingsButton_pressed()
    {
        clickSettingsSound.Play();
    }
}
