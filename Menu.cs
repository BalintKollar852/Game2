using Godot;
using System;

public class Menu : Node2D
{
    Camera2D zoomcamera;
    private bool zoomin;
    public override void _Ready()
    {
        zoomcamera = GetNode("Menubackground/Camera2D") as Camera2D;
    }
    public override void _Process(float delta)
    {
        GD.Print(zoomcamera.Zoom);
        if(zoomin){
            if(zoomcamera.Zoom.x >= 0.5f){ 
                zoomcamera.Zoom -= new Vector2(0.1f, 0.1f);
            }
            else{
                zoomin = false;
            }
        }
    }
    public void _on_Options_pressed(){
        zoomin = true;   
    }
}
