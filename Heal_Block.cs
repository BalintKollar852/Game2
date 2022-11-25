using Godot;
using System;

public class Heal_Block : Node2D
{
    [Export] public PackedScene psHeal;    
    private bool healishereorno;
    private float healspawntime;
    Game gameclass;
    Area2D areacollison;
    public override void _Ready()
    {
        gameclass = GetNode<Game>("/root/Game");
        areacollison = GetNode("Area2D") as Area2D;

        Node2D heal = (Node2D)psHeal.Instance();
        heal.Position = areacollison.Position;
        heal.Connect("CharacterHeal",gameclass,"on_characterheal");
        AddChild(heal);
    }
    public override void _Process(float delta)
    {
        if(!healishereorno && healspawntime >= 30){
            healspawntime = 0;
            Node2D heal = (Node2D)psHeal.Instance();
            heal.Position = areacollison.Position;
            heal.Connect("CharacterHeal",gameclass,"on_characterheal");
            AddChild(heal);
        }
        if(!healishereorno){
            healspawntime += delta;
        }
    }
    public void _on_Area2D_area_entered(Area2D healarea){
        if(healarea.IsInGroup("heal")){
            healishereorno = true;
        }
    }
    public void _on_Area2D_area_exited(Area2D healarea){
        if(healarea.IsInGroup("heal")){
            healishereorno = false;
        }
    }
}
