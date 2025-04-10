using Godot;
using System;

public partial class Mob : CharacterBody3D
{
    [Export]
    public int MinSpeed { get; set; } = 10;

    [Export]
    public int MaxSpeed { get; set; } = 18;

    [Signal]
    public delegate void SquashedEventHandler();

    public override void _PhysicsProcess(double delta){
        MoveAndSlide();
    }

    public void Initialize(Vector3 startPosition, Vector3 playerPosition){
        // start direction: face player and randomize +- 45deg
        LookAtFromPosition(startPosition, playerPosition, Vector3.Up);
        RotateY((float)Godot.GD.RandRange(-Mathf.Pi / 4.0, Mathf.Pi / 4.0));

        // get random speed in range & orient direction
        int randomSpeed = GD.RandRange(MinSpeed, MaxSpeed);
        Velocity = Vector3.Forward * randomSpeed;
        Velocity = Velocity.Rotated(Vector3.Up, Rotation.Y);

        // set animation speed based on random mob speed
        GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = randomSpeed / MaxSpeed;
    }

    public void Squash(){
        // signal for scoring etc, then the mob 'dies'
        this.EmitSignal(SignalName.Squashed);
        QueueFree();
    }

    private void OnVisibilityNotifierScreenExited(){
        QueueFree();
    }
}
