using Godot;
using System;

public class LineCollision : Node2D
{
	[Export]
	public float lineWidth = 10;
	[Export]
	public float lineInterval = 20;

	private float ANGLE_90 = 1.570976f;
	private float lineWidthPrevious;
	private Vector2 mousePointPrevious;

	private Line2D Line;
	private StaticBody2D LineBody;

	public float GetLineWidth()
	{
		return lineWidth;
	}

	private void SetLineWidth(float _widthAfter)
	{
		lineWidth = _widthAfter;
		Line.Width = _widthAfter;
		foreach (CollisionShape2D _node in LineBody.GetChildren())
			((CapsuleShape2D)_node.Shape).SetRadius(_widthAfter/2);
	}

	private void CheckLineWidthChange()
	{
		if (lineWidthPrevious != lineWidth)
		{
			SetLineWidth(lineWidth);
			lineWidthPrevious = lineWidth;
		}
	}

	private void DrawLine()
	{
		if (Input.IsActionPressed("player_mouse_rightclick"))
		{
			// Initializing
			if (Input.IsActionJustPressed("player_mouse_rightclick"))
			{
				mousePointPrevious = GetGlobalMousePosition();
				Line.ClearPoints();
				Line.AddPoint(mousePointPrevious);
				foreach (CollisionShape2D _node in LineBody.GetChildren())
					_node.QueueFree();
			}
			// Line drawing
			Vector2 mousePointCurrent = GetGlobalMousePosition();
			float _distance = mousePointCurrent.DistanceTo(mousePointPrevious);
			if (_distance >= lineInterval)
			{
				// Vector setting
				Vector2 _direction = (mousePointCurrent - mousePointPrevious).Normalized();
				Vector2 _mousePointMiddle = (mousePointCurrent + mousePointPrevious) / 2;
				mousePointPrevious = mousePointCurrent;
				Line.AddPoint(mousePointCurrent);
				// Make collision region
				CollisionShape2D _collision = new CollisionShape2D();
				CapsuleShape2D _shape = new CapsuleShape2D();
				_collision.SetPosition(_mousePointMiddle);
				_collision.SetRotation(ANGLE_90+_direction.Angle());
				_shape.SetHeight(_distance);
				_shape.SetRadius(lineWidth/2);
				_collision.SetShape(_shape);
				LineBody.AddChild(_collision);
			}
		}
	}

	public override void _Ready()
	{
		mousePointPrevious = GetGlobalMousePosition();
		lineWidthPrevious = lineWidth;
		Line = GetNode<Line2D>("Line");
		LineBody = Line.GetNode<StaticBody2D>("Body");
	}

	public override void _Process(float delta)
	{
		CheckLineWidthChange();
		DrawLine();
	}
}
