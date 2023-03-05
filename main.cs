using Godot;
using System;

public class main : Node2D
{
	private LineCollision DefenceLine;

	private void UpdateLineWidth()
	{
		if (Input.IsActionJustReleased("player_mouse_wheelup"))
			DefenceLine.lineWidth++;
		if (Input.IsActionJustReleased("player_mouse_wheeldown"))
			DefenceLine.lineWidth--;
	}

	public override void _Ready()
	{
		Engine.SetTargetFps(60);
		DefenceLine = GetNode<LineCollision>("DefenceLine");
		GetNode<RigidBody2D>("target").LinearVelocity = new Vector2(-100, 0);
	}

	public override void _Process(float delta)
	{
		UpdateLineWidth();
	}
}
