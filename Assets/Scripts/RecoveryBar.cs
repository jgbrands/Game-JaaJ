using Godot;
using System;

public class RecoveryBar : TextureProgress
{
    public bool complete = false;

    public bool active = false;

    public override void _Ready()
    {

    }


    public override void _Process(float delta)
    {
        this.complete = false;

        if (this.active)
        {
            if (Input.IsActionJustPressed("recover")) this.Value += 10;
            if (this.Value >= this.MaxValue)
            {
                this.Value = this.MaxValue;
                this.complete = true;
                this.active = false;
            }
        }
    }
}
