using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }

    public override void _Ready(){
        GetNode<Control>("UserInterface/Retry").Hide();
    }

    public override void _UnhandledInput(InputEvent @event){
        if (@event.IsActionPressed("ui_accept") && GetNode<Control>("UserInterface/Retry").Visible){
			GetTree().ReloadCurrentScene();
		}
    }

	private void OnMobTimerTimeout(){
		Mob mob = MobScene.Instantiate<Mob>();

		var mobSpawnLocation = this.GetNode<PathFollow3D>("SpawnPath/SpawnLocation");

		mobSpawnLocation.ProgressRatio = Godot.GD.Randf();

		Vector3 playerPosition = this.GetNode<Player>("Player").Position;
		mob.Initialize(mobSpawnLocation.Position, playerPosition);

		mob.Squashed += GetNode<ScoreLabel>("UserInterface/ScoreLabel").OnMobSquashed;

		AddChild(mob);
	}

	private void OnPlayerHit(){
		this.GetNode<Timer>("MobTimer").Stop();
        GetNode<Control>("UserInterface/Retry").Show();
	}
}
