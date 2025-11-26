using Godot;
using System;

public partial class Options : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InitializeUIEvents();
    }

    public void InitializeUIEvents()
    {
        GetNode<Button>("OptionsExitButton").Connect(Button.SignalName.Pressed, Callable.From(OnOptionsExitButton));
        GetNode<HSlider>("VolumeSlider").ValueChanged += VolumeChanged;
        GetNode<CheckBox>("MusicCheckBox").Toggled += MusicToggled;
        GetNode<CheckBox>("SFXCheckBox").Toggled += SFXToggled;
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
        UIHelper.SwitchSceneTo(this, UIHelper.previous_scene);
    }
}
