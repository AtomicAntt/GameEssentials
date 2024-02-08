using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    public int speed = 200;
    public Vector2 velocity = Vector2.Zero;

    public void GetInput()
    {
        velocity = Vector2.Zero;
        if (Input.IsActionPressed("right"))
        {
            velocity.x += 1;
        }
        if (Input.IsActionPressed("left"))
        {
            velocity.x -= 1;
        }
        if (Input.IsActionPressed("down"))
        {
            velocity.y += 1;
        }
        if (Input.IsActionPressed("up"))
        {
            velocity.y -= 1;
        }

        velocity = velocity.Normalized() * speed;
    }

    public override void _PhysicsProcess(float delta)
    {
        GetInput();
        velocity = MoveAndSlide(velocity);
    }

}
