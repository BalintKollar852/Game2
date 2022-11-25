using Godot;
using System;

public class Explosion : Node2D
{
    [Signal] delegate void GrenadeDamage();
    AnimatedSprite explosionsprite;
    public override void _Ready()
    {
        var game = GetNode<Game>("/root/Game");
        this.Connect("GrenadeDamage", game, "on_grenadedamage");
        explosionsprite = GetNode("Area2D/AnimatedSprite") as AnimatedSprite;
        explosionsprite.Play("default");
    }
    public void _on_AnimatedSprite_animation_finished(){
        QueueFree();
    }
    public void _on_Area2D_body_entered(KinematicBody2D character){
        if(character.IsInGroup("character")){
            EmitSignal("GrenadeDamage");    
        }
    }
}
