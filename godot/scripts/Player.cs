using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Export]
    public int Speed { get; set; } = 14;

    [Export]
    public int FallAcceleration { get; set; } = 75;

    private Godot.Vector3 _targetVelocity = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        var direction = Vector3.Zero;

        if (Godot.Input.IsActionPressed("move_right")){
            direction.X += 1.0f;
        }
        if (Godot.Input.IsActionPressed("move_left")){
            direction.X -= 1.0f;
        }
        if (Godot.Input.IsActionPressed("move_back")){
            direction.Z += 1.0f;
        }
        if (Godot.Input.IsActionPressed("move_forward")){
            direction.Z -= 1.0f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            // set the Basis, which is the basis of the node's transform, of the Pivot node to adjust the entire player tree
            this.GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
        }

        _targetVelocity.X = direction.X * this.Speed;
        _targetVelocity.Z = direction.Z * this.Speed;

        if (!this.IsOnFloor()){
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }

        Velocity = _targetVelocity;
        MoveAndSlide();
    }

}
