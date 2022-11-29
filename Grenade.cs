using Godot;
using System;

public class Grenade : Node2D
{
    [Export] public PackedScene psExplosion;
    private float explosiontime;
    private Vector2 irany;
    private KinematicBody2D grenadebody;
    private Vector2 target;
    private float speed;
    public override void _Ready()
    {
        speed = 50;
        grenadebody = GetNode("KinematicBody2D") as KinematicBody2D;
        target = GetGlobalMousePosition();
        irany = Position.DirectionTo(target) * speed; 
    }
    public override void _Process(float delta)
    {
        // Error itt
        grenadebody.MoveAndSlide(irany);
        explosiontime += delta;
        if(explosiontime >= 3){
            QueueFree();
        }
        
    }
    public void _on_KinematicBody2D_tree_exiting(){
        var gamenode = GetTree().Root.GetNode("Game") as Node2D;
        Node2D explosion = (Node2D)psExplosion.Instance();
        explosion.Position = grenadebody.Position + Position;
		gamenode.AddChild(explosion);
    }
}

