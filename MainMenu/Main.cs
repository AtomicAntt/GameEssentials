using Godot;
using System;

public class Main : Node
{
    // Initialized audio node variables ----------
    public AudioStreamPlayer mouseHoverSound;
    public AudioStreamPlayer clickPlaySound;
    public AudioStreamPlayer quitSound;
    public AudioStreamPlayer clickSettingsSound;


    public override void _Ready()
    {
        // Initialize all child node references ----------

        mouseHoverSound = GetNode<AudioStreamPlayer>("SFX/MouseHover");
        clickPlaySound = GetNode<AudioStreamPlayer>("SFX/ClickPlaySound");
        quitSound = GetNode<AudioStreamPlayer>("SFX/QuitSound");
        clickSettingsSound = GetNode<AudioStreamPlayer>("SFX/ClickSettingsSound");
    }

    // SIGNALS ----------

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
