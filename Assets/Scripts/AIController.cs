using Godot;
using System;

public class AIController : Node2D
{
    private Car parent;
    public bool active = false;
    private Random rand = new Random();
    [Export] public int driftChance = 10;
    [Export] public int driftCooldown = 5;
    [Export] public int exitDriftChance = 90;
    [Export] public int exitDriftCooldown = 3;
    [Export] public int deaccelerateChance = 90;
    [Export] public int deaccelerateCooldown = 5;
    private float driftTime = 0;
    private float exitDriftTime = 0;
    private float deacceleratetTime = 0;
    [Export] public int maxNormalSpeed = 300;
    [Export] public int maxDriftSpeed = 500;

    public override void _Ready()
    {
        parent = (Car)this.GetParent();
        parent.maxNormalSpeed = maxNormalSpeed;
        parent.maxDriftSpeed = maxDriftSpeed;
    }

    public override void _Process(float delta)
    {
        driftTime += delta;
        exitDriftTime += delta;
        deacceleratetTime += delta;
        GD.Print(parent.lapsCompleted);
        if (this.active)
        {
            if (parent.state != "Derailed") parent.Accelerate();
            if (rand.Next(1, 101) <= driftChance && parent.state != "Derailed" && driftTime > driftCooldown)
            {
                parent.SetStateAsDrifting();
                driftTime = 0;
            }
            else if (rand.Next(1, 101) <= exitDriftChance && parent.state != "Derailed" && exitDriftTime > exitDriftCooldown)
            {
                parent.SetStateAsNormal();
                exitDriftTime = 0;
            }
            if (parent.state == "Normal" && rand.Next(1, 101) <= deaccelerateChance && deacceleratetTime > deaccelerateCooldown)
            {
                parent.Deaccelerate();
                deacceleratetTime = 0;
            }
        }
    }
}