using Godot;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
using System.IO;
public class Game : Node2D
{
    [Export] public PackedScene psBullet;
    [Export] public PackedScene psEnemy;
    [Export] public PackedScene psAmmo;
    [Export] public PackedScene psGrenade;
    private bool mousedown;
    private float elapsedtime;
    private float reloadtime;
    private float enemyspawntime;
    private int bulletnumber = 60;
    public int hp = 100;
    public int gold;
    public int maxammo = 60;
    public int grenadeammo = 3;
    public bool weaponuse = true;
    private Node2D karakter;
    private KinematicBody2D karakterbody;
    Random random = new Random();
    Timer timer;
    Label timerlabel;
    CanvasLayer endmenu;
    CanvasLayer pausemenu;
    Label endmenupoints;
    public ConfigBody config;
    private string text;
    Label fps;
    Label completedtime;
    AnimationPlayer daynight;
    Light2D flashlight;
    Label bestpointsandtime;
    private bool fullscreen;
    private bool fpscounter;
    private bool crosshair;
    private float besttime;
    private int bestpoints;
    public override void _Ready()
    {
        flashlight = GetNode("Character/KinematicBody2D/Light2D") as Light2D;
        daynight = GetNode("CanvasModulate/AnimationPlayer") as AnimationPlayer;
        daynight.Play("Day_Night_Cycle");
        daynight.Seek(random.Next(0, 101));
        daynight.GetAnimation("Day_Night_Cycle").Loop = true;
        fps = GetNode("Character/HUD/Fps") as Label;
        text = File.ReadAllText(@"options.json");
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
        if(get_options.fps){
            fps.Visible = true;
        }
        else{
            fps.Visible = false;
        }
        completedtime = GetNode("Character/End_Menu/Time") as Label;
        config = new ConfigBody();
        timerlabel = GetNode("Character/HUD/Time") as Label;
        timer = GetNode("Timer") as Timer;
        endmenu = GetNode("Character/End_Menu") as CanvasLayer;
        pausemenu = GetNode("Character/Pause_Menu") as CanvasLayer;
        endmenupoints = GetNode("Character/End_Menu/Points") as Label;
        bestpointsandtime = GetNode("Character/End_Menu/Besttime") as Label; 
        timer.Start();
        fullscreen = config.fullscreen;
        fpscounter = config.fps;
        crosshair = config.crosshair;
    }
    public void on_grenadedamage(){
        hp -= 15;
    }
    public void on_enemyattack(){
        hp -= 5;
    }
    public void on_goldpickup(){
        gold++;
    }
    public void on_characterheal(){
        if((hp + 50) <= 100){
            hp += 50;
        }
        else{
            hp = 100;
        }
    }
    public void on_ammoboxpickup(){
        if((maxammo + 60) <= 120){
            maxammo += 60;
        }
        else{
            maxammo = 120;
        }
        grenadeammo = 5;
    }
    public override void _Input(InputEvent esemeny)
	{
		if(esemeny is InputEventMouseButton eger){
            if(eger.Pressed && eger.ButtonIndex == 1){
			    mousedown = true;
        	}
            else if(eger.ButtonIndex == 1 && eger.Pressed == false){
			    mousedown = false;
        	}
        }
        if (Input.IsActionJustPressed("1"))
        {
            weaponuse = true;
        }
        if (Input.IsActionJustPressed("2"))
        {
            weaponuse = false;
        }
        if (Input.IsActionJustPressed("g"))
        {
           if(grenadeammo > 0){
                grenadeammo--;
                Node2D grenade = (Node2D)psGrenade.Instance();
                grenade.Position = karakter.Position + karakterbody.Position;
                AddChild(grenade);
           }
        }
        if (Input.IsActionJustPressed("esc"))
        {
            GetTree().Paused = true;
            pausemenu.Visible = true;
        }
    }
    public override void _Process(float delta)
    {
        var get_options = JsonConvert.DeserializeObject<ConfigBody>(text);
        if(daynight.CurrentAnimationPosition > 22 &&  daynight.CurrentAnimationPosition < 85){
            flashlight.Enabled = false;
        }
        else{
            flashlight.Enabled = true;
        }
        fps.Text = "FPS: " + Convert.ToString(Math.Round(1/delta));
        if(hp <= 0){
            if(config.bestpoints <= gold){
                besttime = timer.WaitTime - timer.TimeLeft;
                bestpoints = gold;
                SaveToFile();
                bestpointsandtime.Text = "Best Points/Time: \n" + Convert.ToString(bestpoints) + " / " + Convert.ToString(TimeSpan.FromSeconds(Math.Round(besttime, 0)));
            }
            else{
                bestpointsandtime.Text = "Best Points/Time: \n" + Convert.ToString(config.bestpoints) + " / " + Convert.ToString(TimeSpan.FromSeconds(Math.Round(config.besttime, 0)));
            }
            GetTree().Paused = true;
            endmenu.Visible = true;
        }
        completedtime.Text = "Time: " + Convert.ToString(TimeSpan.FromSeconds(Math.Round(timer.WaitTime - timer.TimeLeft, 0)));
        endmenupoints.Text ="Points: " + Convert.ToString(gold);
        timerlabel.Text = Convert.ToString(TimeSpan.FromSeconds(Math.Round(timer.WaitTime - timer.TimeLeft, 0)));
        karakter = GetNode("Character") as Node2D;
        karakterbody = GetNode("Character/KinematicBody2D") as KinematicBody2D;
        var knife = GetNode("Character/HUD/Knife/Sprite") as Sprite;
        var rifle = GetNode("Character/HUD/Rifle/Sprite") as Sprite;
        var weapon_background = GetNode("Character/HUD/Weapon_BackGround") as Sprite;
        var hpnumber = GetNode("Character/HUD/HPBar/Count/Background/Number") as Label;
        var hptexture = GetNode("Character/HUD/HPBar/TextureProgress") as TextureProgress;
        var goldtexture = GetNode("Character/HUD/GoldPanel/Gold") as Label;
        var rifle_bulletnumber = GetNode("Character/HUD/Rifle/BulletNumber") as Label;
        var reload_texure = GetNode("Character/HUD/Rifle/Reload_ProgressBar") as ProgressBar;
        var grenade_number = GetNode("Character/HUD/Grenade/Number") as Label;
        grenade_number.Text = Convert.ToString(grenadeammo);
        if(weaponuse){
            knife.Modulate = Color.Color8((byte) 128, (byte)127, (byte)108);
            weapon_background.Position = new Vector2(960, 420);
            rifle.Modulate = Color.Color8((byte) 255, (byte)255, (byte)255);
        }
        else{
            rifle.Modulate = Color.Color8((byte) 128, (byte)127, (byte)108);
            weapon_background.Position = new Vector2(960, 475);
            knife.Modulate = Color.Color8((byte) 255, (byte)255, (byte)255);
        }
        hpnumber.Text = Convert.ToString(hp);
        hptexture.Value = hp;
        goldtexture.Text = Convert.ToString(gold);
        enemyspawntime += delta;
        if(enemyspawntime >= 3){
            Node2D enemy = (Node2D)psEnemy.Instance();
            enemy.Position = new Vector2(random.Next(-64, 368), random.Next(-48, 176));
            enemy.Connect("EnemyAttack",this,"on_enemyattack");
            AddChild(enemy);
            enemyspawntime = 0;
        }
        elapsedtime += delta;
        if(weaponuse){
            if(mousedown == true && Input.IsActionPressed("r") == false){
                if(elapsedtime >= 0.3f && bulletnumber > 0){
                    Node2D bullet = (Node2D)psBullet.Instance();
		    	    bullet.Position = karakter.Position + karakterbody.Position;
                    AddChild(bullet); 
                    bulletnumber--;
                    elapsedtime = 0;
                }
            }
            if (Input.IsActionPressed("r"))
            {
                reloadtime += delta;
                reload_texure.Visible = true;
                reload_texure.Value = reloadtime;
                if(reloadtime >= 2){
                    if(maxammo >= 0 && maxammo <= 120){
                        if(60 - bulletnumber <= maxammo){
                            maxammo -= 60 - bulletnumber;
                            bulletnumber = 60;
                            reloadtime = 0;
                            reload_texure.Value = 0;
                        }
                        if(60 - bulletnumber > maxammo){
                            maxammo = 0;
                            bulletnumber = bulletnumber + maxammo;
                            reloadtime = 0;
                            reload_texure.Value = 0;
                        }
                    }
                }
            }
            else{
            reloadtime = 0;
            reload_texure.Visible = false;
            }
        }
        rifle_bulletnumber.Text = Convert.ToString(bulletnumber) + " / " + Convert.ToString(maxammo);
    }
    public void _on_Restart_pressed(){
        GetTree().Paused = false;
        GetTree().ChangeScene("res://Game.tscn");
    }
     public void _on_Menu_pressed(){
        GetTree().Paused = false;
        GetTree().ChangeScene("res://Menu.tscn");
    }
    public void _on_Exit_pressed(){
        GetTree().Quit();
    }
     public void _on_Resume_pressed(){
        GetTree().Paused = false;
        pausemenu.Visible = false;
    }

    public void SaveToFile(){
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

}

