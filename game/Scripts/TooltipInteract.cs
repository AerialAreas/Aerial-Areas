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

    private void OnMouseEntered()
    {
        tooltip.Toggle(true);
    }

    private void OnMouseExisted()
    {
        tooltip.Toggle(false);
    }
}
