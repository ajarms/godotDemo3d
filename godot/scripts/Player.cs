using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Export]
    public int Speed { get; set; } = 14;

    [Export]
    public int FallAcceleration { get; set; } = 75;

    [Export]
    public int JumpImpulse { get; set; } = 20;

    [Export]
    public int BounceImpulse { get; set; } = 16;

    [Signal]
    public delegate void HitEventHandler();

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
            // set the Basis--the basis of the node's transform--of the Pivot node to adjust the entire player tree
            this.GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
        }

        _targetVelocity.X = direction.X * this.Speed;
        _targetVelocity.Z = direction.Z * this.Speed;

        if (!this.IsOnFloor()){
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        } else if (Input.IsActionPressed("jump")){
            _targetVelocity.Y = JumpImpulse;
        }

        // check any collisions with enemy mobs
        for (int i = 0; i < this.GetSlideCollisionCount(); i++){
            KinematicCollision3D collision = this.GetSlideCollision(i);
            if (collision.GetCollider() is Mob m){
                // use dot to determine if colliding from above
                if (Vector3.Up.Dot(collision.GetNormal()) > 0.2f){
                    m.Squash();
                    _targetVelocity.Y = BounceImpulse;
                    // prevent killing multiple mobs per jump
                    break;
                }
            }
        }

        Velocity = _targetVelocity;
        MoveAndSlide();
    }

    private void Die(){
        EmitSignal(SignalName.Hit);
        QueueFree();
    }

    private void OnMobDetectorBodyEntered(Node3D body){
        // only detects enemy mask so no need to check the body
        Die();
    }
}
