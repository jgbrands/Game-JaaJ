using Godot;
using System;

public class DriftRecovery : Control
{
    private TextureRect gauge;
    private TextureRect bar;
    private TextureRect playerDot;

    private float gaugeTop;
    private float gaugeBottom;
    private Vector2 barVelocity;
    private Vector2 playerDotVelocity;

    private Random random;
    [Export] public float slipness = 0.015f;


    public float time = 0;
    public bool lost = false;
    public bool active = false;

    [Export] public int playerAccel = 20;
    [Export] public int playerAccelGravity = 10;
    [Export] public float timeToLose = 1.5f;

    private TextureProgress gaugeMeter;

    public void set()
    {
        this.time = 0;
        gaugeTop = gauge.RectPosition.y;
        gaugeBottom = gauge.RectPosition.y + gauge.RectSize.y * gauge.RectScale.y;

        bar.RectPosition = new Vector2(bar.RectPosition.x, gaugeBottom - bar.RectSize.y * bar.RectScale.y);
        playerDot.RectPosition = new Vector2(playerDot.RectPosition.x, gaugeBottom - (playerDot.RectSize.y * playerDot.RectScale.y) - 5);

        barVelocity = new Vector2(0, -100);
        playerDotVelocity = new Vector2(0, 0);

        this.lost = false;
        this.active = true;
        this.Show();
    }

    public void shut()
    {
        this.active = false;
        this.time = 0;
        this.Hide();
    }

    public override void _Ready()
    {
        gauge = this.GetNode<Godot.TextureRect>("Gauge");
        bar = this.GetNode<Godot.TextureRect>("Bar");
        playerDot = this.GetNode<Godot.TextureRect>("PlayerDot");
        gaugeMeter = (TextureProgress)this.GetNode("GaugeMeter");
        random = new Random();

        this.set();
        this.Hide();
    }

    private void moveBar(float delta)
    {
        bar.RectPosition += barVelocity * delta;
        if (bar.RectPosition.y < gaugeTop)
        {
            barVelocity.y = Mathf.Abs(barVelocity.y);
            bar.RectPosition = new Vector2(bar.RectPosition.x, gaugeTop);
        }
        if (bar.RectPosition.y > gaugeBottom - bar.RectSize.y * bar.RectScale.y)
        {
            barVelocity.y = -Mathf.Abs(barVelocity.y);
            bar.RectPosition = new Vector2(bar.RectPosition.x, gaugeBottom - bar.RectSize.y * bar.RectScale.y);
        }

    }

    private void getInput(float delta)
    {
        if (Input.IsActionPressed("brake")) playerDotVelocity.y -= playerAccel;
    }

    public override void _Process(float delta)
    {
        if (this.active)
        {
            playerDotVelocity.y += playerAccelGravity;
            gaugeMeter.Value = (1 - this.time / this.timeToLose) * 100;

            if (playerDot.RectPosition.y < gaugeTop + 5)
            {
                playerDot.RectPosition = new Vector2(playerDot.RectPosition.x, gaugeTop + 5);
                playerDotVelocity.y = 0;
            }

            if (playerDot.RectPosition.y > gaugeBottom - (playerDot.RectSize.y * playerDot.RectScale.y) - 5)
            {
                if (Mathf.Abs(playerDotVelocity.y) > playerAccelGravity * 2.5f) playerDotVelocity.y = -playerDotVelocity.y * 0.5f;
                else playerDotVelocity.y = 0;
                playerDot.RectPosition = new Vector2(playerDot.RectPosition.x, gaugeBottom - (playerDot.RectSize.y * playerDot.RectScale.y) - 5);
            }

            getInput(delta);
            moveBar(delta);

            playerDot.RectPosition += playerDotVelocity * delta;

            if (playerDot.RectPosition.y < bar.RectPosition.y || playerDot.RectPosition.y > bar.RectPosition.y + bar.RectSize.y * bar.RectScale.y) time += delta;
            else time -= delta;

            if (random.NextDouble() <= slipness) barVelocity *= -1;

            if (time < 0) time = 0;

            if (time > timeToLose)
            {
                this.Hide();
                this.lost = true;
                this.active = false;
            }
        }



    }
}
