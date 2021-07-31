using Godot;
using System;

public class PlayerController : Control
{
    private Car parent;
    public bool active = false;
    private Control recoveryBarContainer;
    private RecoveryBar recoveryBar;
    [Export] public int maxNormalSpeed = 500;
    [Export] public int maxDriftSpeed = 800;


    private DriftRecovery driftRecovery;
    public override void _Ready()
    {
        recoveryBarContainer = (Control)this.GetNode("CanvasLayer/RecoveryBarContainer");
        recoveryBarContainer.Hide();
        recoveryBar = (RecoveryBar)recoveryBarContainer.GetNode("RecoveryBar");

        driftRecovery = (DriftRecovery)this.GetNode("CanvasLayer/DriftRecovery");
        parent = (Car)this.GetParent();
        
        parent.maxNormalSpeed = maxNormalSpeed;
        parent.maxDriftSpeed = maxDriftSpeed;
    }


    public override void _Process(float delta)
    {

        if (this.active)
        {
            if (Input.IsActionPressed("accelerate") && parent.state != "Derailed") parent.Accelerate();
            if (Input.IsActionPressed("drift") && parent.state != "Derailed") parent.SetStateAsDrifting();
            else if (parent.state != "Derailed") parent.SetStateAsNormal();
            if (parent.state == "Normal" && !Input.IsActionPressed("accelerate")) parent.Deaccelerate();



            if (driftRecovery.lost)
            {
                parent.SetStateAsDerailed();
                recoveryBar.Value = 0;
                recoveryBarContainer.Show();
                recoveryBar.active = true;
                driftRecovery.time = 0;
                driftRecovery.lost = false;
            }
            if (parent.state == "Derailed")
            {
                parent.Deaccelerate();
                if (recoveryBar.complete)
                {
                    parent.Recover();
                    recoveryBar.active = false;
                    recoveryBarContainer.Hide();
                }
            }

            if (parent.speed > parent.maxNormalSpeed * 1.2 && parent.state != "Derailed" && !driftRecovery.active)
            {
                driftRecovery.set();
            }
            if (parent.speed < parent.maxNormalSpeed && parent.state != "Derailed") driftRecovery.shut();
        }
        else driftRecovery.shut();
    }
}
