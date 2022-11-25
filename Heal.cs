using Godot;
using System;

public class Heal : Node2D
{
    [Signal] delegate void CharacterHeal();
    private int hpfromcharacter;
    public override void _Process(float delta)
    {
        var game_hp = GetNode("/root/Game").Get("hp"); 
        hpfromcharacter = Convert.ToInt32(game_hp);
    }
    public void _on_Area2D_body_entered(KinematicBody2D karakter){
        if(karakter.Owner.Name == "Character"){
            if(hpfromcharacter < 100){
                EmitSignal("CharacterHeal");
                QueueFree();
            }
        }
    }
}
