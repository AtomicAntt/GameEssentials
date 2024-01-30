using Godot;
using System;
using System.Runtime.Serialization.Json;

public class Saves : Node
{
    private String _saveFilePath = "user://gamedata.save";

    // https://docs.godotengine.org/en/3.5/tutorials/io/saving_games.html
    public void SaveGame()
    {
        var saveGame = new File();
        saveGame.Open(_saveFilePath, File.ModeFlags.Write);

        // Manually extract save data here :(

        // ---------- Volume Sliders ----------
        HSlider masterVolumeSlider = GetTree().GetNodesInGroup("Master")[0] as HSlider;
        HSlider bgmVolumeSlider = GetTree().GetNodesInGroup("BGM")[0] as HSlider;
        HSlider sfxVolumeSlider = GetTree().GetNodesInGroup("SFX")[0] as HSlider;

        var saveData = new Godot.Collections.Dictionary<string, object>()
        {
            {"MasterVolume", masterVolumeSlider.Value},
            {"BackgroundMusicVolume", bgmVolumeSlider.Value},
            {"SoundEffectVolume", sfxVolumeSlider}
        };

        saveGame.StoreLine(JSON.Print(saveData));

        saveGame.Close();
    }

    public void LoadGame()
    {
        var saveGame = new File();

        if (!saveGame.FileExists(_saveFilePath))
        {
            return;
        }

        saveGame.Open(_saveFilePath, File.ModeFlags.Read);

        // Manually load data here :(

        


    }
}
