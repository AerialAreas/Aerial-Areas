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
    }
    private void SFXToggled(bool toggledOn)
    {
        UIHelper.sfx = toggledOn;
    }

    private void MusicToggled(bool toggledOn)
    {
        UIHelper.music = toggledOn;
    }

    private void VolumeChanged(double value)
    {
        GetNode<Label>("VolumeLabel").Text = $"Volume: {value}";
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
