using Godot;
using System;

public class Player : Node2D {
	private Vector2 playerMovement;
    private Line2D line;
    private Vector2 start;
    private Vector2 nextPoint;
    private int nextPointIndex = 1;
    private int startIndex = 53;
    private bool finished = false;

	[Export] public int speed = 0;
    [Export] public int maxSpeed = 2000;
    [Export] public int pointSnap = 30;
    [Export] public int dash = 3;
    [Export] public int drag = 10;
    [Export] public int turboAcceleration = 10;
    [Export] public int defaultAcceleration = 1;
    [Export] public int acceleration = 10;

	public override void _Ready() {
        line = this.GetTree().Root.GetNode("Main").GetNode<Line2D>("Circuit");
        nextPointIndex = startIndex + 1;
        start = line.Position + line.Points[startIndex];
        nextPoint = line.Position + line.Points[nextPointIndex];
        this.Position = start;
        acceleration = defaultAcceleration;
	}

	public override void _Process(float delta) {
        playerMovement = new Vector2(0, 0);
        GD.Print("Speed: " + speed + " || Acceleration: " + acceleration);

        //Circuit
        if ((this.Position - nextPoint).Length() < pointSnap) {
            this.Position = nextPoint;
            nextPointIndex = (nextPointIndex + 1 == line.GetPointCount()) ? 0 : ++nextPointIndex;
            nextPoint = line.Position + line.Points[nextPointIndex];
            if (this.Position == start)  GD.Print("!Finished!");
        }
        
        //Acceleration
        if (Input.IsActionPressed("space")) speed = (speed + 1 > maxSpeed) ? speed : speed + acceleration;
        else speed = (speed - 1 < 0) ? 0 : speed - drag;

        //Dash
        if (Input.IsActionPressed("shift")) acceleration = turboAcceleration;
        else acceleration = defaultAcceleration;

        this.Position += (nextPoint - this.Position).Normalized() * delta * speed;
	}
}