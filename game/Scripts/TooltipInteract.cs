using Godot;
using System;

public partial class TooltipInteract : Button
{
    Tooltip tooltip;

    public override void _Ready()
    {
        tooltip = GetNode<Tooltip>("Tooltip");
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExisted;
    }

    public void OnMouseEntered()
    {
        tooltip.Toggle(true);
    }

    public void OnMouseExisted()
    {
        tooltip.Toggle(false);
    }
}
