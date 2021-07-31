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
    private float driftAngle = 0f;
    private float driftSpeed = 0.07f;
    private float targetAngle;
    private float driftSpeedMultiplier;
    private Vector2 movementDirection;
    private Node2D particleNode;

    public string state = "Normal";
    public int acceleration;
    public int speed;
    private int maxSpeed;
    public int lapsCompleted = 0;

    public int maxNormalSpeed = 500;
    public int maxDriftSpeed = 800;
    [Export] public int pointSnap = 5;
    [Export] public int drag = 20;
    [Export] public int driftAcceleration = 5;
    [Export] public int defaultAcceleration = 2;
    [Export] public int startIndex = 0;


    public void Accelerate()
    {
        if (this.state == "Drifting")
        {
            acceleration = driftAcceleration;
            maxSpeed = maxDriftSpeed;

            speed = (speed + 1 > maxSpeed) ? speed - acceleration : speed + acceleration;
        }
        else
        {
            acceleration = defaultAcceleration;
            maxSpeed = maxNormalSpeed;

            speed = (speed + 1 > maxSpeed) ? speed - acceleration : speed + acceleration;
        }
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

    public void Recover()
    {
        this.Position = lastPoint;
        this.SetStateAsNormal();
    }

    private void CalculateRotation()
    {
        targetAngle = ((this.nextPoint1 - this.lastPoint).Angle() - (this.nextPoint2 - this.nextPoint1).Angle());
        if (targetAngle > Mathf.Pi) targetAngle -= Mathf.Pi;
        if (targetAngle < -Mathf.Pi) targetAngle += Mathf.Pi;

        if (this.state == "Drifting") this.driftAngle = -3.5f * this.targetAngle * (float)this.speed / (float)this.maxDriftSpeed;
        if (this.state != "Drifting")
        {
            if (this.driftAngle > 0) this.driftAngle -= this.driftSpeed;
            if (this.driftAngle < 0) this.driftAngle += this.driftSpeed;
        }

        GD.Print(targetAngle);

        if ((lastPoint - nextPoint1).Length() != 0) movementDirection = (lastPoint - nextPoint1);
        else movementDirection = nextPoint1 - nextPoint2;

        if (this.state != "Derailed")
        {
            stackedSprite.Call("spriteRotateTo", (movementDirection).Angle());
            stackedSprite.Call("spriteRotate", driftAngle);
            particleNode.Rotation = movementDirection.Angle() + driftAngle;
        }
        else
        {
            stackedSprite.Call("spriteRotate", this.driftAngle / 2);
        }

    }

    public override void _Ready()
    {
        line = this.GetParent().GetNode<Line2D>("CircuitBuilder/Circuit");
        stackedSprite = (Godot.Object)this.GetNode("StackedSprite");
        particleNode = (Node2D)this.GetNode("ParticleNode");

        nextPointIndex = startIndex + 1;
        line.Points[line.GetPointCount() - 1] = line.Points[0];
        start = line.GlobalPosition + line.Points[startIndex] * line.Scale;

        nextPoint1 = line.GlobalPosition + line.Points[nextPointIndex] * line.Scale;
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
            if (lastPoint == start) lapsCompleted += 1;

            nextPointIndex = (nextPointIndex + 1 == line.GetPointCount()) ? 0 : ++nextPointIndex;
            nextPoint1 = line.Position + line.Points[nextPointIndex] * line.Scale;
            nextPoint2 = line.Position + line.Points[(nextPointIndex + 2 >= line.GetPointCount()) ? nextPointIndex - line.GetPointCount() + 2 : nextPointIndex + 2] * line.Scale;

        }

        foreach (CPUParticles2D emitter in particleNode.GetChildren())
        {
            if (this.speed >= this.maxDriftSpeed * 0.9) emitter.Emitting = true;
            else emitter.Emitting = false;
        }


        CalculateRotation();

        if (this.state != "Derailed") this.Position += (nextPoint1 - this.Position).Normalized() * delta * speed;
        else this.Position += (nextPoint1 - lastPoint).Normalized() * delta * speed;
    }
}