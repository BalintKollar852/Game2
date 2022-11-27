using Godot;
using System;
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
    private int bulletnumber = 30;
    public int hp = 100;
    public int gold;
    public int maxammo = 90;
    public int grenadeammo = 2;
    public bool weaponuse = true;
    private Node2D karakter;
    private KinematicBody2D karakterbody;
    Random random = new Random();
    public override void _Ready()
    {
        
        // Amerre néz a karakter egy kis flasflight (picit sötét lenne a map)
        //Animációk beállítása
        // Ne tudj lőni reload közben
        // Az enemy hud ne buggoljon 
        // Gránát mozgása buggos
    }
    public void on_grenadedamage(){
        hp -= 35;
    }
    public void on_enemyattack(){
        hp -= 5;
    }
    public void on_goldpickup(){
        gold++;
    }
    public void on_characterheal(){
        if((hp + 25) <= 100){
            hp += 25;
        }
        else{
            hp = 100;
        }
    }
    public void on_ammoboxpickup(){
        if((maxammo + 60) <= 150){
            maxammo += 60;
        }
        else{
            maxammo = 150;
        }
        grenadeammo = 3;
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
    }
    public override void _Process(float delta)
    {
        karakter = GetNode("Character") as Node2D;
        karakterbody = GetNode("Character/KinematicBody2D") as KinematicBody2D;
        var knife = GetNode("Character/HUD/Knife/Sprite") as Sprite;
        var rifle = GetNode("Character/HUD/Rifle/Sprite") as Sprite;
        var weapon_background = GetNode("Character/HUD/Weapon_BackGround") as Sprite;
        var hpnumber = GetNode("Character/HUD/HPBar/Count/Background/Number") as Label;
        var hptexture = GetNode("Character/HUD/HPBar/TextureProgress") as TextureProgress;
        var goldtexture = GetNode("Character/HUD/Gold") as Label;
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
            enemy.Position = new Vector2(random.Next(0, 200), random.Next(0, 200));
            enemy.Connect("EnemyAttack",this,"on_enemyattack");
            AddChild(enemy);
            enemyspawntime = 0;
        }
        elapsedtime += delta;
        if(weaponuse){
            if(mousedown == true){
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
                    if(maxammo >= 0 && maxammo <= 150){
                        if(30 - bulletnumber <= maxammo){
                            maxammo -= 30 - bulletnumber;
                            bulletnumber = 30;
                            reloadtime = 0;
                            reload_texure.Value = 0;
                        }
                        if(30 - bulletnumber > maxammo){
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

}

