using Godot;
using System;

public class Blood : Node2D
{
    public float removetime;
    public override void _Ready()
    {
        
    }
    public override void _Process(float delta) {
        removetime += delta;
        if(removetime >= 5){
            QueueFree();
        }
    }
}
