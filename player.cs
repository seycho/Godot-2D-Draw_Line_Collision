using Godot;
using System;

public class player : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	public int speedStop = 30;
	public int speedMax = 400;
	public int speedNormal = 50;
	public int speedBrake = 18;
	private Vector2 _velocity = new Vector2();
	private Vector2 _playerControl = new Vector2();
	
	public void GetInput(float delta)
	{
		_playerControl = new Vector2();
		if (Input.IsActionPressed("player_control_up"))
			_playerControl.y -= 1;
		if (Input.IsActionPressed("player_control_down"))
			_playerControl.y += 1;
		if (Input.IsActionPressed("player_control_left"))
			_playerControl.x -= 1;
		if (Input.IsActionPressed("player_control_right"))
			_playerControl.x += 1;
		_playerControl = _playerControl.Normalized();

		_velocity += _playerControl * speedNormal;
		Vector2 _velNormal = _velocity.Normalized();
		// Max velocity
		if (_velocity.Dot(_velocity) > Math.Pow(speedMax, 2))
			_velocity = _velNormal * speedMax;
		// Braking
		if (_playerControl.y == 0)
			_velocity.y -= _velNormal.y * speedBrake;
			if (Math.Abs(_velocity.y) < speedStop)
				_velocity.y = 0;
		if (_playerControl.x == 0)
			_velocity.x -= _velNormal.x * speedBrake;
			if (Math.Abs(_velocity.x) < speedStop)
				_velocity.x = 0;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		GetInput(delta);
		MoveAndCollide(_velocity * delta);
	}
}
