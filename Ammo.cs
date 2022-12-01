using Godot;
using System;

public class Ammo : Node2D
{
    [Signal] delegate void AmmoBoxPickUp();
    private int maxammo;
     private int grenadeammo;
    public override void _Process(float delta)
    {
        var game_maxammo = GetNode("/root/Game").Get("maxammo");
        var game_grenadeammo = GetNode("/root/Game").Get("grenadeammo");  
        maxammo = Convert.ToInt32(game_maxammo);
        grenadeammo= Convert.ToInt32(game_grenadeammo);
    }
    public void _on_Area2D_body_entered(KinematicBody2D karakter){
        if(karakter.Owner.Name == "Character"){
            if(maxammo < 120 || grenadeammo < 5){
                EmitSignal("AmmoBoxPickUp");
                QueueFree();
            }
        }
    }
}
