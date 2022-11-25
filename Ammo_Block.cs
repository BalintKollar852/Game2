using Godot;
using System;

public class Ammo_Block : Node2D
{ 
    [Export] public PackedScene psAmmo;    
    private bool ammoishereorno;
    private float ammospawntime;
    Game gameclass;
    Area2D areacollison;
    public override void _Ready()
    {
        gameclass = GetNode<Game>("/root/Game");
        areacollison = GetNode("Area2D") as Area2D;

        Node2D ammo = (Node2D)psAmmo.Instance();
        ammo.Position = areacollison.Position;
        ammo.Connect("CharacterHeal",gameclass,"on_characterheal");
        AddChild(ammo);
    }
    public override void _Process(float delta)
    {
        if(!ammoishereorno && ammospawntime >= 5){
            ammospawntime = 0;
            Node2D ammo = (Node2D)psAmmo.Instance();
            ammo.Position = areacollison.Position;
            ammo.Connect("AmmoBoxPickUp",gameclass,"on_ammoboxpickup");
            AddChild(ammo);
        }
        if(!ammoishereorno){
            ammospawntime += delta;
        }
    }
    public void _on_Area2D_area_entered(Area2D ammoarea){
        if(ammoarea.IsInGroup("ammo")){
            ammoishereorno = true;
        }
    }
    public void _on_Area2D_area_exited(Area2D ammoarea){
        if(ammoarea.IsInGroup("ammo")){
            ammoishereorno = true;
        }
    }
}
