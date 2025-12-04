using Godot;
using System;

public partial class Tooltip : PanelContainer
{
    Tween opacityTween = null;
    private float _padding = 10.0f;
    public override void _Ready() { Hide(); }
    public override void _Input(InputEvent @event)
    {
        if (Visible && @event is InputEventMouseMotion)
        {
            //GlobalPosition = GetGlobalMousePosition();
            UpdateTooltipPosition();
        }
    }

    public void UpdateTooltipPosition()
    {
        Vector2 pos = GetGlobalMousePosition() + new Vector2(_padding, _padding);
        Rect2 viewportRect = GetViewport().GetVisibleRect();
        Vector2 tooltipSize = Size;

        if (pos.X + tooltipSize.X > viewportRect.End.X)
        {
            pos.X = GetGlobalMousePosition().X - tooltipSize.X - _padding;
        }

        if (pos.Y + tooltipSize.Y > viewportRect.End.Y)
        {
            pos.Y = GetGlobalMousePosition().Y - tooltipSize.Y - _padding;
        }

        if(pos.X < viewportRect.Position.X) pos.X = viewportRect.Position.X + _padding;
        if(pos.Y < viewportRect.Position.Y) pos.Y = viewportRect.Position.Y + _padding;

        GlobalPosition = pos;
    }

    public async void Toggle(bool on)
    {
        if (on)
        {
            Show(); Modulate = new Color(1, 1, 1, 0);
            Callable.From(UpdateTooltipPosition).CallDeferred();
            TweenOpacity(new Color(1, 1, 1, 1));
        }
        else
        {
            Modulate = new Color(1, 1, 1, 1);
            await ToSignal(TweenOpacity(new Color(1, 1, 1, 0)),
                Tween.SignalName.Finished);
            Hide();
        }
    }

    public Tween TweenOpacity(Color to)
    {
        opacityTween?.Kill();
        opacityTween = GetTree().CreateTween();
        opacityTween.TweenProperty(this, "modulate", to, 0.3);
        return opacityTween;
    }
}
