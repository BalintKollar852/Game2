using Godot;
using System;

public class Menu : Node2D
{
	Camera2D zoomcamera;
	private bool zoomin;
	private bool zoomout;
    public bool fullscreen = true;
    private bool options;
	private bool fpscounter;
    Panel optionspanel;
	public override void _Ready()
	{
		zoomcamera = GetNode("Menubackground/Camera2D") as Camera2D;
        optionspanel = GetNode("OptionsPanel") as Panel;
	}
	public override void _Process(float delta)
	{
		if(fullscreen){
			OS.WindowFullscreen = true;
		}
		else{
			OS.WindowFullscreen = false;
		}
        if(options && zoomin == false){
            optionspanel.Visible = true;
        }
        else{
            optionspanel.Visible = false;
        }
		if(zoomin){
			if(zoomcamera.Zoom.x >= 0.4f){ 
				zoomcamera.Position += new Vector2(0, -11);
				zoomcamera.Zoom -= new Vector2(0.025f, 0.025f);
			}
			else{
				zoomin = false;
			}
		}
		if(zoomout){
			if(zoomcamera.Zoom.x <= 0.99f){ 
				zoomcamera.Position += new Vector2(0, 11);
				zoomcamera.Zoom += new Vector2(0.025f, 0.025f);
			}
			else{
				zoomout = false;
			}
		}
	}
	public void _on_Options_pressed(){
		zoomin = true;   
        options = true;
	}
	public void _on_BackToMenu_pressed(){
		zoomout = true;
        options = false;
	}
    private void _on_FpsCounter_toggled(bool button_pressed)
    {
        if(button_pressed){
            fpscounter = true;
        }
        else{
            fpscounter = false;
        }
    }
    private void _on_Fullscreen_toggled(bool button_pressed)
    {
        if(button_pressed){
            fullscreen = true;
        }
        else{
            fullscreen = false;
        }
    }
}

