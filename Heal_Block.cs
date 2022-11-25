using Godot;
using System;

public class Heal_Block : Node2D
{
    [Export] public PackedScene psHeal;    
    private bool healishereorno = true;
    private float healspawntime;
    public override void _Ready()
    {
        //var gameclass = GetNode<Game>("/root/Game");
        Node2D heal = (Node2D)psHeal.Instance();
        heal.Position = Position;
        //heal.Connect("CharacterHeal",gameclass,"on_characterheal");
        AddChild(heal);
    }
    public override void _Process(float delta)
    {
        //var gameclass = GetNode<Game>("/root/Game");
        if(healishereorno && healspawntime >= 5){
            healspawntime = 0;
            Node2D heal = (Node2D)psHeal.Instance();
            heal.Position = Position;
           // heal.Connect("CharacterHeal",gameclass,"on_characterheal");
            AddChild(heal);
        }
        else{
            healspawntime += delta;
        }
    }
    public void _on_Area2D_area_etered(Area2D healarea){
        if(healarea.IsInGroup("heal")){
            healishereorno = true;
        }
        if(healarea.IsInGroup("bullet") && healarea.IsInGroup("heal") == false){
            healishereorno = false;
        }
    }
}
