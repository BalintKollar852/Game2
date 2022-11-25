using Godot;
using System;

public class Ammo : Node2D
{
    // Ez is úgyan úgy működne mint a heal (tf2-höz hasonlóan)
    [Signal] delegate void AmmoBoxPickUp();
    private int maxammo;
    public override void _Process(float delta)
    {
        var game_maxammo = GetNode("/root/Game").Get("maxammo"); 
        maxammo = Convert.ToInt32(game_maxammo);
    }
    public void _on_Area2D_body_entered(KinematicBody2D karakter){
        if(karakter.Owner.Name == "Character"){
            if(maxammo < 150){
                EmitSignal("AmmoBoxPickUp");
                QueueFree();
            }
        }
    }
}
