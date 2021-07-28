using Godot;
using System.Linq;
using System;

public class MatchController : Control
{

    public Godot.Collections.Array<Car> players;

    public AnimationPlayer countdown;
    public Control loss;
    public Control victory;
    public Label lapCounter;
    public bool matchEnded = false;

    [Export] public int laps = 3;

    private Godot.Collections.Array neighbors;

    public void _on_AnimationPlayer_animation_finished(string animationName)
    {
        foreach (Node player in players) foreach (Node node in player.GetChildren()) if (node is PlayerController) ((PlayerController)node).active = true;
    }

    public override void _Ready()
    {
        countdown = (AnimationPlayer)this.GetNode("CanvasLayer/Countdown/AnimationPlayer");
        loss = (Control)this.GetNode("CanvasLayer/Loss");
        victory = (Control)this.GetNode("CanvasLayer/Victory");
        lapCounter = (Label)this.GetNode("CanvasLayer/LapCounter");

        players = new Godot.Collections.Array<Car>();

        neighbors = this.GetParent().GetChildren();
        for (int i = 0; i < neighbors.Count; i++) if (neighbors[i] is Car) players.Add((Car)neighbors[i]);

        countdown.Play("CountDown");
        loss.Hide();
        victory.Hide();

    }

    public override void _Process(float delta)
    {
        if (!this.matchEnded)
        {
            lapCounter.Text = $"{players[0].lapsCompleted}/{this.laps}";

            foreach (Car player in players)
            {
                if (player.lapsCompleted == this.laps)
                {
                    this.matchEnded = true;

                    foreach (Node node in player.GetChildren()) if (node is PlayerController) victory.Show();
                }
            }
        }

    }
}
