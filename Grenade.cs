using Godot;
using System;

public class Grenade : Node2D
{
    [Export] public PackedScene psExplosion;
    private float explosiontime;
    private Vector2 irany;
    private RigidBody2D grenadebody;
    private Vector2 target;
    public override void _Ready()
    {
        grenadebody = GetNode("RigidBody2D") as RigidBody2D;
        target = GetGlobalMousePosition();
        irany = GlobalPosition.DirectionTo(target);
        // Buggos a dobás
        //grenadebody.ApplyImpulse(Vector2.Zero ,irany);
    }
    public override void _Process(float delta)
    {
        explosiontime += delta;
        if(explosiontime >= 3){
            QueueFree();
        }
        
    }
    public void _on_RigidBody2D_tree_exiting(){
        var gamenode = GetTree().Root.GetNode("Game") as Node2D;
        Node2D explosion = (Node2D)psExplosion.Instance();
        explosion.Position = grenadebody.Position + Position;
		gamenode.AddChild(explosion);
    }
}

