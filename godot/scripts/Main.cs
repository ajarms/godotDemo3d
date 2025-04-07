using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }

	private void OnMobTimerTimeout(){
		Mob mob = MobScene.Instantiate<Mob>();

		var mobSpawnLocation = this.GetNode<PathFollow3D>("SpawnPath/SpawnLocation");

		mobSpawnLocation.ProgressRatio = Godot.GD.Randf();

		Vector3 playerPosition = this.GetNode<Player>("Player").Position;
		mob.Initialize(mobSpawnLocation.Position, playerPosition);

		AddChild(mob);
	}
}
