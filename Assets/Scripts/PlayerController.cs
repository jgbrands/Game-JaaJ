using Godot;
using System;

public class PlayerController : Node2D
{
    private Car parent;
    public bool active = false;
    private Control recoveryBarContainer;
    private RecoveryBar recoveryBar;
    public override void _Ready()
    {
        recoveryBarContainer = (Control)this.GetNode("CanvasLayer/RecoveryBarContainer");
        recoveryBarContainer.Hide();
        recoveryBar = (RecoveryBar)recoveryBarContainer.GetNode("RecoveryBar");
        parent = (Car)this.GetParent().GetNode("Player");
    }


    public override void _Process(float delta)
    {
        if (this.active)
        {
            if (Input.IsActionPressed("accelerate") && parent.state != "Derailed") parent.Accelerate();
            if (Input.IsActionPressed("drift") && parent.state != "Derailed") parent.SetStateAsDrifting();
            else if (parent.state != "Derailed") parent.SetStateAsNormal();
            if (parent.state == "Normal" && !Input.IsActionPressed("accelerate")) parent.Deaccelerate();


            /* SetStateAsDerailed nao eh pra ser decidio pelo jogador quando vai ser usado mas eu coloquei aqui
            pra descarrilhar o carro quando aperta s por motivos de teste */
            if (Input.IsActionJustPressed("brake"))
            {
                parent.SetStateAsDerailed();
                recoveryBar.Value = 0;
                recoveryBarContainer.Show();
                recoveryBar.active = true;
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
        }
    }
}
