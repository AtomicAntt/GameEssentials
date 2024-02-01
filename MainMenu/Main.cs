using Godot;
using System;

public class Main : Control
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

    private Panel _settingsPanel;
    private ColorRect _settingsBackground;

    // ---------- Initialized Autoloads ----------

    private Saves _saves;

    // ---------- Methods to call on ready, which also loads saved parts of the game as the game is getting started ----------

    public void InitializeVolume()
    {
        
    }

    // ----------- Initialize all child node references ----------

    public override void _Ready()
    {
        // Initialize all child node references
        mouseHoverSound = GetNode<AudioStreamPlayer>("SFX/MouseHoverSound");
        clickPlaySound = GetNode<AudioStreamPlayer>("SFX/ClickPlaySound");
        quitSound = GetNode<AudioStreamPlayer>("SFX/QuitSound");
        clickSettingsSound = GetNode<AudioStreamPlayer>("SFX/ClickSettingsSound");

        mainMenuMusic = GetNode<AudioStreamPlayer>("Music/MainMenuMusic");
        inGameMusic = GetNode<AudioStreamPlayer>("Music/InGameMusic");

        _levels = GetNode<Node2D>("Levels");

        _mainMenu = GetNode<Control>("MainMenu");
        _settings = GetNode<Control>("Settings");

        _settingsPanel = GetNode<Panel>("Settings/SettingsPanel");
        _settingsBackground = GetNode<ColorRect>("Settings/SettingsBackground");

        // Initialize needed autoloads
        _saves = GetNode<Saves>("/root/Saves");

        // Load values like settings that were set.
        _saves.LoadGame();

        // Play music after loading to allow it to play at the volume setting the user saved.
        mainMenuMusic.Play();
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

	// ---------- Methods to handle tween animations ----------

	public void ShowUI(Control ui, bool showUI)
	{
		SceneTreeTween tween = GetTree().CreateTween();

		if (showUI)
		{
			tween.TweenProperty(ui, "rect_scale", new Vector2(1.0f, 1.0f), 0.2f);
		}
		else
		{
			tween.TweenProperty(ui, "rect_scale", new Vector2(0.0f, 0.0f), 0.2f);
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
        LoadLevel("Level1");
        clickPlaySound.Play();
    }

    public void _on_QuitButton_pressed()
    {
        quitSound.Play();

        GetTree().Quit();
    }

    public void _on_SettingsButton_pressed()
    {
        _settings.Visible = true;
		ShowUI(_settingsPanel, true);
        
        clickSettingsSound.Play();
    }

    public void _on_SettingsBackButton_pressed()
    {
        _settings.Visible = false;

        quitSound.Play();
    }

    // ---------- VOLUME SLIDER SIGNAL METHODS ----------

    public void _on_VolumeSlider_value_changed(float value, String audioBusName)
    {
        int busIndex = AudioServer.GetBusIndex(audioBusName);
        AudioServer.SetBusVolumeDb(busIndex, GD.Linear2Db(value));

        // Save it so next time you open the game you get the same volume
        _saves.SaveGame();
    }

    // ---------- INPUT SIGNAL METHOD ----------

    // If you click and settings is up, if it was outside of the settings window, it will close it
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton && _settings.Visible)
        {
            Vector2 mousePos = GetGlobalMousePosition();

            Rect2 uiBoundingBox = _settingsPanel.GetRect();

            if (!uiBoundingBox.HasPoint(mousePos))
            {
                _settings.Visible = false;

                quitSound.Play();
            }
        }
    }
}
