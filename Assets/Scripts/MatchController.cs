using Godot;
using System.Linq;
using System;

public class MatchController : Control
{

    public Godot.Collections.Array<Car> players;
    public Godot.Collections.Array<PlayerController> playerControllers;

    public AnimationPlayer countdown;
    public Control loss;
    public Control victory;

    private Godot.Collections.Array neighbors;

    public void _on_AnimationPlayer_animation_finished(string animationName)
    {
        GD.Print("Fudeu hehehhe");
        for (int i = 0; i < playerControllers.Count; i++) playerControllers[i].active = true;
    }

    public override void _Ready()
    {
        countdown = (AnimationPlayer)this.GetNode("CanvasLayer/Label/AnimationPlayer");
        loss = (Control)this.GetNode("CanvasLayer/Loss");
        victory = (Control)this.GetNode("CanvasLayer/Victory");

        players = new Godot.Collections.Array<Car>();
        playerControllers = new Godot.Collections.Array<PlayerController>();

        neighbors = this.GetParent().GetChildren();
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i] is Car) players.Add((Car)neighbors[i]);
            if (neighbors[i] is PlayerController) playerControllers.Add((PlayerController)neighbors[i]);
        }

        countdown.Play("CountDown");
        loss.Hide();
        victory.Hide();

    }

    public override void _Process(float delta)
    {
        GD.Print(players[0].speed);
    }
}
