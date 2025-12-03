using Godot;
using System;

public partial class Options : Node2D
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
        GetNode<CheckBox>("MusicCheckBox").Toggled += MusicToggled;
        GetNode<CheckBox>("SFXCheckBox").Toggled += SFXToggled;
        GetNode<CheckBox>("SkipTutorialsCheckBox").Toggled += SkipTutorialsToggled;
    }

    private void SetOptionsUIToPlayerSettings()
    {
        GetNode<HSlider>("VolumeSlider").Value = UIHelper.volume;
        GetNode<CheckBox>("MusicCheckBox").ButtonPressed = UIHelper.music;
        GetNode<CheckBox>("SFXCheckBox").ButtonPressed = UIHelper.sfx;
        GetNode<CheckBox>("SkipTutorialsCheckBox").ButtonPressed = UIHelper.skip_tutorials;
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
        UIHelper.SwitchSceneTo(this, UIHelper.previous_scene);
    }
}
