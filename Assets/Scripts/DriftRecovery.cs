using Godot;
using System;

public class DriftRecovery : Control{
    private TextureRect gauge;
    private TextureRect bar;
    private TextureRect playerDot;

    private float gaugeTop = 205;
    private float gaugeBottom = 409;
    private Vector2 barVelocity;
    private Vector2 playerDotVelocity;
    private float time = 0;

    public override void _Ready() {
        gauge = this.GetNode<Godot.TextureRect>("Gauge");
        bar = this.GetNode<Godot.TextureRect>("Bar");
        playerDot = this.GetNode<Godot.TextureRect>("PlayerDot");

        gaugeTop = gauge.RectPosition.y;
        gaugeBottom = gauge.RectPosition.y + gauge.RectSize.y * gauge.RectScale.y;

        bar.RectPosition = new Vector2(bar.RectPosition.x, gaugeBottom - bar.RectSize.y * bar.RectScale.y);
        playerDot.RectPosition = new Vector2(playerDot.RectPosition.x, gaugeBottom - (playerDot.RectSize.y * playerDot.RectScale.y) - 5);

        barVelocity = new Vector2(0, -100);
        playerDotVelocity = new Vector2(0, 0);
    }

    private void moveBar (float delta) {
        bar.RectPosition += barVelocity * delta;
        if (bar.RectPosition.y < gaugeTop ||
            bar.RectPosition.y > gaugeBottom - bar.RectSize.y * bar.RectScale.y) barVelocity.y = -barVelocity.y;
    }

    private void getInput (float delta) {
        if (Input.IsActionPressed("brake") && playerDotVelocity.y > -1) playerDotVelocity.y = -200;
    }

    public override void _Process(float delta) {
        time += delta;
        playerDotVelocity.y += 10;
        if (playerDot.RectPosition.y > gaugeBottom - (playerDot.RectSize.y * playerDot.RectScale.y) - 5) playerDotVelocity.y = 0;
        
        moveBar(delta);
        getInput(delta);
        playerDot.RectPosition += playerDotVelocity * delta;
        if (playerDot.RectPosition.y < gaugeTop + 5) playerDot.RectPosition = new Vector2(playerDot.RectPosition.x, gaugeTop);
        
        if (time > 5) {
            barVelocity.y = 0;
            GD.Print("Lost");
        }
    }
}
