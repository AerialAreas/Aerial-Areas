using Godot;
using System;

public partial class Tutorial : Node
{
	public override void _Ready()
	{
		InitializeUIEvents();
	}
	public void InitializeUIEvents()
	{
		GetNode<Button>("BackButton").Connect(Button.SignalName.Pressed, Callable.From(OnBackButtonClicked));
		GetNode<Button>("ReadyButton").Connect(Button.SignalName.Pressed, Callable.From(OnReadyButtonClicked));
	}
	public void OnBackButtonClicked()
	{
		UIHelper.SwitchSceneTo(this, "Main Menu");
	}
	public void OnReadyButtonClicked()
	{
		UIHelper.SwitchSceneTo(this, "Game");
	}
}
