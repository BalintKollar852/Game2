using Godot;
using System;

public class Character : Node2D
{
    int sebesseg;
    Vector2 Move = new Vector2(0, 0);
    Vector2 iranycucc;
    private AnimatedSprite animatedSprite;
    private bool mousedown;
    private bool moveornot;
    private bool reload;
    private float knifeattacktime;
    private bool leftbutton;
    public override void _Ready()
    {
        animatedSprite = GetNode("KinematicBody2D/AnimatedSprite") as AnimatedSprite;

    }
    public override void _Process(float delta)
    {
        var whichweaponused = GetNode("/root/Game").Get("weaponuse"); 
        if(Convert.ToBoolean(whichweaponused)){
            if(moveornot){
                if(reload){
                    animatedSprite.Play("reload");
                }
                else{
                    if(Input.IsActionPressed("left_button")){
                        animatedSprite.Play("shoot");
                    }
                    else{
                        animatedSprite.Play("move");
                    }
                }
            }
            else{
                if(reload){
                    animatedSprite.Play("reload");
                }
                else{
                    if(Input.IsActionPressed("left_button")){
                        animatedSprite.Play("shoot");
                    }
                    else{
                        animatedSprite.Play("move");
                    }
                }
            }
        }
        else{
            if(Input.IsActionJustPressed("left_button")){
                leftbutton = true;
            }
           if(moveornot){
                if(knifeattacktime <= 0.75f && leftbutton){
                    animatedSprite.Play("knife_attack");
                }
                else{
                    animatedSprite.Play("knife_move");
                    leftbutton = false;
                }
            }
            else{
                if(knifeattacktime <= 0.75f && leftbutton){
                    animatedSprite.Play("knife_attack");
                }
                else{
                    animatedSprite.Play("knife_idle");
                    leftbutton = false;
                }
            }
        }
        if(leftbutton){ 
                knifeattacktime += delta;
            }   
            else{
                    knifeattacktime = 0;
        }
        var alak = GetNode("KinematicBody2D") as KinematicBody2D;
        var knifearea = GetNode("KinematicBody2D/KnifeArea") as Area2D;
        alak.LookAt(GetGlobalMousePosition());
        knifearea.LookAt(GetGlobalMousePosition());

        Move.x = 0;
        Move.y = 0;
        if (Input.IsActionPressed("ui_left"))
        {
            Move.x -= 50;
        }
        if (Input.IsActionPressed("ui_right"))
        {
            Move.x += 50;
        }
        if (Input.IsActionPressed("ui_up"))
        {
            Move.y -= 50;
        }
        if (Input.IsActionPressed("ui_down"))
        {
            Move.y += 50;
        }
        if (Input.IsActionPressed("ui_down") || Input.IsActionPressed("ui_left")|| Input.IsActionPressed("ui_up") || Input.IsActionPressed("ui_right"))
        {
            moveornot = true;
        }
        else{
            moveornot = false;
        }
        if (Input.IsActionPressed("r"))
        {
            reload = true;
        }
        else{
            reload = false;
        }
        Move += Move.Normalized() * delta;
        alak.MoveAndSlide(Move);
    }
}
