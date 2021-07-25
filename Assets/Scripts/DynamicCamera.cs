using Godot;
using System;

public class DynamicCamera : Camera2D
{
    [Export] public float minZoom = 0.2f;
    [Export] public float maxZoom = 0.5f;
    private int parentSpeed;
    private int parentMaxSpeed;
    private float currentZoom;

    public override void _Ready()
    {
        parentMaxSpeed = ((Player)this.GetParent()).maxSpeed;
    }

    public override void _Process(float delta)
    {
        parentSpeed = ((Player)this.GetParent()).speed;

        currentZoom = minZoom * (parentMaxSpeed - parentSpeed) / parentMaxSpeed
                    + maxZoom * (parentSpeed) / parentMaxSpeed;

        this.Zoom = new Vector2(currentZoom, currentZoom);

    }
}
