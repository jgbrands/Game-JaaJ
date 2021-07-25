using Godot;
using System;

public class PlayerController : Node2D
{
    private Car parent;
    public override void _Ready()
    {
        parent = (Car)this.GetParent();
    }


    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("drift") && parent.state != "Derailed") parent.SetStateAsDrifting();
        else parent.SetStateAsNormal();

        if (Input.IsActionPressed("accelerate") && parent.state != "Derailed") parent.Accelerate();
        else parent.Deaccelerate();

        /* SetStateAsDerailed nao eh pra ser decidio pelo jogador quando vai ser usado mas eu coloquei aqui
        pra descarrilhar o carro quando aperta s por motivos de teste */
        if (Input.IsActionPressed("brake")) parent.SetStateAsDerailed();
    }
}
