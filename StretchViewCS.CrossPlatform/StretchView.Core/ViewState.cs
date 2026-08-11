namespace StretchView.Core;

public sealed class ViewState
{
    public const double DefaultZoom = 1.0d;
    public const double DefaultRotationDegrees = 0.0d;

    public double Zoom { get; private set; } = DefaultZoom;
    public double RotationDegrees { get; private set; } = DefaultRotationDegrees;
    public bool IsFlippedHorizontal { get; private set; }
    public bool IsFlippedVertical { get; private set; }
    public bool IsGridVisible { get; private set; }

    public void SetZoom(double zoom)
    {
        if (zoom <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(zoom));
        }

        Zoom = zoom;
    }

    public void SetRotationDegrees(double rotationDegrees)
    {
        RotationDegrees = rotationDegrees;
    }

    public void ToggleHorizontalFlip()
    {
        IsFlippedHorizontal = !IsFlippedHorizontal;
    }

    public void ToggleVerticalFlip()
    {
        IsFlippedVertical = !IsFlippedVertical;
    }

    public void ToggleGrid()
    {
        IsGridVisible = !IsGridVisible;
    }

    public void ResetTransform()
    {
        Zoom = DefaultZoom;
        RotationDegrees = DefaultRotationDegrees;
        IsFlippedHorizontal = false;
        IsFlippedVertical = false;
    }
}
