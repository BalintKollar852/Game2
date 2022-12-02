using Godot;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;
public class Menu : Node2D
{
	Camera2D zoomcamera;
	private bool zoomin;
	private bool zoomout;
    private bool fullscreen;
    private bool options;
	private bool fpscounter;
	private string text;
	private bool crosshair;
	private Label fpscountershow;
	private ConfigBody config;
    Panel optionspanel;
	Panel buttonsmenu;
	Resource arrow;
	private float besttime;
	private int bestpoints;
	public override void _Ready()
	{
		arrow = ResourceLoader.Load("res://crosshair.png");
		buttonsmenu = GetNode("Buttons") as Panel;
		fpscountershow = GetNode("Menubackground/Camera2D/CanvasLayer/Fps") as Label;
		zoomcamera = GetNode("Menubackground/Camera2D") as Camera2D;
        optionspanel = GetNode("OptionsPanel") as Panel;
		config = new ConfigBody();
		text = File.ReadAllText(@"options.json");
		var fullscreengomb = GetNode("OptionsPanel/Fullscreen") as CheckButton;
		var crosshairbutton = GetNode("OptionsPanel/Crosshair") as CheckButton;
		var fpscounterbutton = GetNode("OptionsPanel/FpsCounter") as CheckButton;
		var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
		fullscreengomb.Pressed = config.fullscreen;
		fpscounterbutton.Pressed = config.fps;
		crosshairbutton.Pressed = config.crosshair;
		
	}
	public override void _Process(float delta)
	{
		config = new ConfigBody();
		besttime = config.besttime;
		bestpoints = config.bestpoints;
		text = File.ReadAllText(@"options.json");
		var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
		fpscountershow.Text =  "FPS: " + Convert.ToString(Math.Round(1/delta));
		if(config.crosshair){
			Input.SetCustomMouseCursor(arrow);
		}
		else{
			Input.SetCustomMouseCursor(null);
		}
		if(config.fullscreen){
			OS.WindowFullscreen = true;
		}
		else{
			OS.WindowFullscreen = false;
		}
		if(config.fps){
			fpscountershow.Visible = true;
		}
		else{
			fpscountershow.Visible = false;
		}
		if(options){
			buttonsmenu.Visible = false;
		}
		else{
			buttonsmenu.Visible = true;
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
	
	private void _on_Crosshair_toggled(bool button_pressed){
		if(button_pressed){
			crosshair = true;
        }
        else{
           	crosshair = false;
        }
	}
	public void _on_Save_pressed(){
        JObject options = new JObject(
        new JProperty("Fullscreen", fullscreen),
		new JProperty("FPS", fpscounter),
		new JProperty("Crosshair", crosshair),
		new JProperty("BestTime", besttime),
		new JProperty("BestPoints", bestpoints)
		);
		File.WriteAllText(@"options.json", options.ToString());
		using (StreamWriter file = File.CreateText(@"options.json"))
		using (JsonTextWriter writer = new JsonTextWriter(file))
		{
			options.WriteTo(writer);
		}
	}
	public void _on_Play_pressed(){
		GetTree().ChangeScene("res://Game.tscn");
	}
	public void _on_Exit_pressed(){
		GetTree().Quit();
	}

}

