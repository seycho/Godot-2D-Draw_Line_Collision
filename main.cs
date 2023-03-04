using Godot;
using System;

public class main : Node2D
{
	private Node LineDefence;
	private StaticBody2D LineDefenceBody;
	private Line2D LineDefenceBodyLine2D;
	private CollisionPolygon2D LineDefenceBodyCollisionPolygon2D;

	private float LineCollisionWidth = 5;
	private float LinePointInterval = 10;

	private Vector2 CalLineCollisionSide(Vector2 _linePointA, Vector2 _linePointC, Vector2 _linePointB)
	{
		Vector2 _normVectorA = (_linePointA - _linePointC).Normalized();
		Vector2 _normVectorB = (_linePointB - _linePointC).Normalized();

		float _cos2a = _normVectorA.Dot(_normVectorB);
		float _cosa = (float)Math.Pow((1+_cos2a)/2, 0.5);
		float _sina = (float)Math.Pow((1-_cos2a)/2, 0.5);
		float _crossPro = _normVectorA.x*_normVectorB.y - _normVectorA.y*_normVectorB.x;

		if (_crossPro < 0)
		{
			_sina *= -1;
			_normVectorB *= -1;
		}
		Vector2 _matRot1 = new Vector2(_cosa, _sina);
		Vector2 _matRot2 = new Vector2(-_sina, _cosa);
		Vector2 _normalVector = new Vector2(_normVectorB.Dot(_matRot1), _normVectorB.Dot(_matRot2));

		return _normalVector;
	}

	public void DrawLine()
	{
		bool _isDraw = false;

		if (Input.IsActionJustPressed("player_mouse_rightclick"))
		{
			GD.Print("start");
			LineDefenceBodyLine2D.ClearPoints();
			LineDefenceBodyLine2D.AddPoint(GetGlobalMousePosition());
		}

		if (Input.IsActionPressed("player_mouse_rightclick"))
		{
			Vector2 linePointLast = LineDefenceBodyLine2D.GetPointPosition(LineDefenceBodyLine2D.GetPointCount()-1);
			if (linePointLast.DistanceTo(GetGlobalMousePosition()) > LinePointInterval)
			{
				_isDraw = true;
			}
		}

		if (Input.IsActionJustReleased("player_mouse_rightclick"))
		{
			_isDraw = true;
		}

		if (_isDraw)
		{
			LineDefenceBodyLine2D.AddPoint(GetGlobalMousePosition());
			var linePoints = LineDefenceBodyLine2D.GetPoints();
			int numPoints = linePoints.Length;
			int numPointsLast = numPoints-1;
			int numLinePointsArray = 2+2*(numPointsLast-1);

			Vector2[] collisionPoints = new Vector2[numLinePointsArray];
			collisionPoints[0] = linePoints[0];
			collisionPoints[numPointsLast] = linePoints[numPointsLast];
			for (int i = 1; i < numPointsLast; i++)
			{
				Vector2 _normVectorSide = CalLineCollisionSide(linePoints[i-1], linePoints[i], linePoints[i+1]);
				collisionPoints[i] = linePoints[i] + _normVectorSide*LineCollisionWidth;
				collisionPoints[numLinePointsArray-i] = linePoints[i] - _normVectorSide*LineCollisionWidth;
				Vector2 _rotMat901 = new Vector2(0, -1);
				Vector2 _rotMat902 = new Vector2(1, 0);
				Vector2 _normVectorFront = new Vector2(_normVectorSide.Dot(_rotMat901), _normVectorSide.Dot(_rotMat902));
			}
			LineDefenceBodyCollisionPolygon2D.SetPolygon(collisionPoints);
		}
	}

	public override void _Ready()
	{
		Engine.SetTargetFps(60);
		LineDefence = GetNode<Node>("LineDefence");
		LineDefenceBody = LineDefence.GetNode<StaticBody2D>("Body");
		LineDefenceBodyLine2D = LineDefence.GetNode<StaticBody2D>("Body").GetNode<Line2D>("Line2D");
		LineDefenceBodyCollisionPolygon2D = LineDefence.GetNode<StaticBody2D>("Body").GetNode<CollisionPolygon2D>("CollisionPolygon2D");
		GetNode<RigidBody2D>("target").LinearVelocity = new Vector2(-100, 0);
	}

	public override void _Process(float delta)
	{
		DrawLine();
	}
}
