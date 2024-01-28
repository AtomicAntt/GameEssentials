using Godot;
using System;

public class Main : Node
{
    // ---------- Initialized audio node variables ----------

    // SFX
    public AudioStreamPlayer mouseHoverSound;
    public AudioStreamPlayer clickPlaySound;
    public AudioStreamPlayer quitSound;
    public AudioStreamPlayer clickSettingsSound;

    // Music
    public AudioStreamPlayer mainMenuMusic;
    public AudioStreamPlayer inGameMusic;

    // ---------- Variables for loading/unloading the game levels ----------

    private Node2D _levels; // Initialized child node

    private Node2D _levelInstance; // Current Node2D scene that will be under _levels

    // ----------- Initialized menu references that may need to be shown or hidden ----------

    private Control _mainMenu;
    private Control _settings;

    // ----------- Initialize all child node references ----------
    public override void _Ready()
    {
        mouseHoverSound = GetNode<AudioStreamPlayer>("SFX/MouseHoverSound");
        clickPlaySound = GetNode<AudioStreamPlayer>("SFX/ClickPlaySound");
        quitSound = GetNode<AudioStreamPlayer>("SFX/QuitSound");
        clickSettingsSound = GetNode<AudioStreamPlayer>("SFX/ClickSettingsSound");

        mainMenuMusic = GetNode<AudioStreamPlayer>("Music/MainMenuMusic");
        inGameMusic = GetNode<AudioStreamPlayer>("Music/InGameMusic");

        _levels = GetNode<Node2D>("Levels");

        _mainMenu = GetNode<Control>("MainMenu");
        _settings = GetNode<Control>("Settings");
    }

    // ---------- Methods to handle loading and unloading levels ----------

    public void UnloadLevel()
    {
        if (IsInstanceValid(_levelInstance))
        {
            _levelInstance.QueueFree();
        }
    }

    public void LoadLevel(String levelName)
    {
        // First, remove menus and any current levels that are running
        UnloadLevel();
        _mainMenu.Visible = false;

        // This assumes that the level scene is in a folder called 'Levels' that is under res://
        String levelPath = "res://Levels/" + levelName + ".tscn";
        PackedScene levelResource = GD.Load<PackedScene>(levelPath);

        if (levelResource != null)
        {
            _levelInstance = levelResource.Instance<Node2D>();
            _levels.AddChild(_levelInstance);
        }
    }

    // ---------- BUTTON SIGNAL METHODS ----------

    // Signal used on all menu buttons to indicate that the mouse is hovering over a button by playing a sound.
    public void _on_Button_mouse_entered()
    {
        mouseHoverSound.Play();
    }

    public void _on_StartButton_pressed()
    {
        // Make sure you have 'Levels' folder with a valid Node2D Level1.tscn file in it first.
        // LoadLevel("Level1");
        clickPlaySound.Play();
    }

    public void _on_QuitButton_pressed()
    {
        quitSound.Play();

        GetTree().Quit();
    }

    public void _on_SettingsButton_pressed()
    {
        _mainMenu.Visible = false;
        _settings.Visible = true;
        
        clickSettingsSound.Play();
    }

    public void _on_SettingsBackButton_pressed()
    {
        _mainMenu.Visible = true;
        _settings.Visible = false;

        quitSound.Play();
    }
}
