using Godot;
using System;

public partial class Options : Control
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InitializeUIEvents();
        SetOptionsUIToPlayerSettings();
    }

    public void InitializeUIEvents()
    {
        GetNode<Button>("OptionsExitButton").Connect(Button.SignalName.Pressed, Callable.From(OnOptionsExitButton));
        GetNode<HSlider>("VolumeSlider").ValueChanged += VolumeChanged;
        GetNode<CheckBox>("MusicBG/MusicCheckBox").Toggled += MusicToggled;
        GetNode<CheckBox>("SFXBG/SFXCheckBox").Toggled += SFXToggled;
        GetNode<CheckBox>("SkipTutorialsBG/SkipTutorialsCheckBox").Toggled += SkipTutorialsToggled;
    }

    private void SetOptionsUIToPlayerSettings()
    {
        GetNode<HSlider>("VolumeSlider").Value = UIHelper.volume;
        GetNode<CheckBox>("MusicBG/MusicCheckBox").ButtonPressed = UIHelper.music;
        GetNode<CheckBox>("SFXBG/SFXCheckBox").ButtonPressed = UIHelper.sfx;
        GetNode<CheckBox>("SkipTutorialsBG/SkipTutorialsCheckBox").ButtonPressed = UIHelper.skip_tutorials;
    }

    private void SkipTutorialsToggled(bool toggledOn)
    {
        UIHelper.skip_tutorials = toggledOn;
        MainMenu.UpdateSettings(5, toggledOn);
    }
    private void SFXToggled(bool toggledOn)
    {
        UIHelper.sfx = toggledOn;
        MainMenu.UpdateSettings(4, toggledOn);
    }

    private void MusicToggled(bool toggledOn)
    {
        UIHelper.music = toggledOn;
        MainMenu.UpdateSettings(3, toggledOn);
    }

    private void VolumeChanged(double value)
    {
        GetNode<Label>("VolumeLabel").Text = $"Volume: {value}";
        MainMenu.UpdateVolumeSlider((int)value);
        UIHelper.volume = (int)value;
    }

    public void OnOptionsExitButton()
    {
        if (GameLogic.inGame)
        {
            GetNode<Game>("/root/Game").ShowGame();
        }
        else
        {
            UIHelper.SwitchSceneTo(this, UIHelper.previous_scene);
        }
    }
}
