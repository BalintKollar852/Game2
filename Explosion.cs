using Godot;
using System;

public class Explosion : Node2D
{
    AnimatedSprite explosionsprite;
    public override void _Ready()
    {
        explosionsprite = GetNode("Area2D/AnimatedSprite") as AnimatedSprite;
        explosionsprite.Play("default");
    }
    public void _on_AnimatedSprite_animation_finished(){
        QueueFree();
    }
}
