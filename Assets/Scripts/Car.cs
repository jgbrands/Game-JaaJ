using Godot;
using System;

public class Car : Node2D
{
    private Vector2 playerMovement;
    private Line2D line;
    private Vector2 start;
    private Vector2 lastPoint;
    private Vector2 nextPoint1;
    private Vector2 nextPoint2;
    private int nextPointIndex = 1;
    private bool finished = false;
    private Godot.Object stackedSprite;
    private float driftAngle;
    private Vector2 movementDirection;

    public string state = "Normal";
    public int acceleration;
    public int speed;
    private int maxSpeed;

    [Export] public int maxNormalSpeed = 500;
    [Export] public int maxDriftSpeed = 800;
    [Export] public int pointSnap = 5;
    [Export] public int drag = 5;
    [Export] public int driftAcceleration = 5;
    [Export] public int defaultAcceleration = 2;
    [Export] public int startIndex = 53;


    public void Accelerate()
    {
        if (this.state == "Drifting")
        {
            acceleration = driftAcceleration;
            maxSpeed = maxDriftSpeed;
        }
        else
        {
            acceleration = defaultAcceleration;
            maxSpeed = maxNormalSpeed;
        }

        speed = (speed + 1 > maxSpeed) ? speed - acceleration : speed + acceleration;
    }

    public void Deaccelerate()
    {
        speed = (speed - 1 < 0) ? 0 : speed - drag;
    }

    public void SetStateAsDerailed()
    {
        this.state = "Derailed";
    }

    public void SetStateAsDrifting()
    {
        this.state = "Drifting";
    }

    public void SetStateAsNormal()
    {
        this.state = "Normal";
    }

    private void CalculateRotation()
    {
        driftAngle = (Mathf.Pi / 2) * (speed - maxNormalSpeed) / (maxDriftSpeed - maxNormalSpeed);
        if (driftAngle <= 0) driftAngle = 0;
        movementDirection = (lastPoint - nextPoint1);

        if (this.state != "Derailed")
        {
            stackedSprite.Call("spriteRotateTo", (movementDirection).Angle());
            stackedSprite.Call("spriteRotate", driftAngle);
        }
        else
        {
            stackedSprite.Call("spriteRotate", 0.1);
        }

    }

    public override void _Ready()
    {
        line = this.GetTree().Root.GetNode("Main").GetNode<Line2D>("Circuit");
        stackedSprite = (Godot.Object)this.GetNode("StackedSprite");

        nextPointIndex = startIndex + 1;
        start = line.GlobalPosition + line.Points[startIndex];

        nextPoint1 = line.GlobalPosition + line.Points[nextPointIndex];
        lastPoint = start;

        this.Position = start;
        acceleration = defaultAcceleration;
    }

    public override void _Process(float delta)
    {
        playerMovement = new Vector2(0, 0);

        //Circuit
        if ((this.Position - nextPoint1).Length() < pointSnap && this.state != "Derailed")
        {
            lastPoint = new Vector2(nextPoint1);
            //this.Position = nextPoint;
            nextPointIndex = (nextPointIndex + 1 == line.GetPointCount()) ? 0 : ++nextPointIndex;
            nextPoint1 = line.Position + line.Points[nextPointIndex];
            nextPoint2 = line.Position + line.Points[(nextPointIndex + 2 >= line.GetPointCount()) ? nextPointIndex - line.GetPointCount() + 2 : nextPointIndex + 2];
            if (this.Position == start) finished = true;
        }

        CalculateRotation();

        if (this.state != "Derailed") this.Position += (nextPoint1 - this.Position).Normalized() * delta * speed;
        else this.Position += (nextPoint1 - lastPoint).Normalized() * delta * speed;
    }
}