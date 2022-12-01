using Godot;
using System;

public class Enemy : Node2D
{
    [Export] public PackedScene psGold;
    [Signal] delegate void EnemyAttack();
    public float speed;
    public Vector2 irany;
    public Vector2 target; 
    public Vector2 move; 
    private bool followorno = false; 
    private bool idleorno = true; 
    private bool attackorno = false; 
    private float attacktime; 
    private float fps; 
    public int hp = 100; 
    private int db; 
    private bool knifeattackarea; 
    private ProgressBar hpbar;
    public bool weapon;
    private float mousedown;
    private bool leftbutton;
    Character charactergame = new Character();
    public override void _Ready()
    {
        speed = 25;
        hpbar = GetNode("ProgressBar") as ProgressBar;
    }
    
    public override void _Process(float delta)
    {
        var enemy = GetNode("EnemyBody") as KinematicBody2D;
        var enemynode = enemy.Owner as Node2D;
        hpbar.SetPosition(enemy.Position + new Vector2(-5, -10));
        var gamecucc = GetNode("/root/Game").Get("weaponuse"); 
        if(knifeattackarea && Convert.ToBoolean(gamecucc) == false){
            if(Input.IsActionJustPressed("left_button")){
                leftbutton = true;
            }
            if(leftbutton){
                mousedown += delta;
            }
            if(mousedown >= 0.5f){
                hp -= 35;
                mousedown = 0;
                leftbutton = false;
            }
        }
        hpbar.Value = hp;
        hpbar.RectRotation = 0;
        if(hp < 100){
            hpbar.Visible = true;
        }
        else{
            hpbar.Visible = false;
        }
        if(hp <= 0){
            var game = GetNode<Game>("/root/Game");
            var gamenode = GetTree().Root.GetNode("Game") as Node2D;
            Node2D gold = (Node2D)psGold.Instance();
            gold.Position = enemynode.Position + enemy.Position;
            gold.Connect("GoldPickUp",game,"on_goldpickup");
		    gamenode.AddChild(gold);
            QueueFree();
        }
        fps = 1 / delta;
        var player = GetNode<Character>("../Character");
        var playerbody = GetNode<KinematicBody2D>("../Character/KinematicBody2D");
        var enemy_animatedsprite = GetNode("EnemyBody/AnimatedSprite") as AnimatedSprite;
        
        target = Position.DirectionTo((player.Position + playerbody.Position) - enemy.Position);
        if(attackorno == false && followorno){
            enemy_animatedsprite.Play("move");
            enemy.LookAt((player.Position + playerbody.Position));
            enemy.MoveAndSlide(target * speed);
        }
        if(attackorno){ 
            attacktime += delta;
            enemy_animatedsprite.Play("attack");
            enemy.LookAt((player.Position + playerbody.Position));
            enemy.MoveAndSlide(target * speed);
            if(attacktime >= 1){
                EmitSignal("EnemyAttack");
                attacktime = 0;
            }
        }
        if(idleorno){
            enemy_animatedsprite.Play("idle"); 
        }
    }
    public void _on_Enemy_tree_exiting(){
    }
    public void _on_Follow_body_entered(KinematicBody2D karakter){
        var enemy_animatedsprite = GetNode("EnemyBody/AnimatedSprite") as AnimatedSprite;
        if(karakter.Owner.Name == "Character"){ 
            enemy_animatedsprite.Stop();
            followorno = true;
            idleorno = false;
        }
    }
    public void _on_Follow_body_exited(KinematicBody2D karakter){
        var enemy_animatedsprite = GetNode("EnemyBody/AnimatedSprite") as AnimatedSprite;
        if(karakter.Owner.Name == "Character"){ 
            enemy_animatedsprite.Stop();
            idleorno = true;
            followorno = false;
        }
    }
    public void _on_Attack_body_entered(KinematicBody2D karakter){
        var enemy_animatedsprite = GetNode("EnemyBody/AnimatedSprite") as AnimatedSprite;
        if(karakter.Owner.Name == "Character"){ 
            enemy_animatedsprite.Stop();
            attackorno = true;

        }
    }
    public void _on_Attack_body_exited(KinematicBody2D karakter){
        var enemy_animatedsprite = GetNode("EnemyBody/AnimatedSprite") as AnimatedSprite;
        if(karakter.Owner.Name == "Character"){ 
            enemy_animatedsprite.Stop();
            attackorno = false;
        }
    }
    public void _on_AreaShape_area_entered(Area2D areashape){
        // Ha több darab lövedék jön akkor valamiért szar
        // Ahány db lövedék van egyszerre benne anyiszor kene levonni
        var areanodecucc = areashape.Owner as Node2D;
        if(areanodecucc.IsInGroup("bullet")){
            hp -= 50;
            areashape.Owner.QueueFree();
        }
        if(areashape.Name == "KnifeArea"){
            knifeattackarea = true;
        }
        if(areashape.IsInGroup("explosion")){
            hp -= 75;
        }
    }
    public void _on_AreaShape_area_exited(Area2D areashape){
        if(areashape.Name == "KnifeArea"){
            knifeattackarea = false;
        }
    }
    public void _on_AreaShape_body_entered(Node2D grenade){
        if(grenade.IsInGroup("grenade")){
            grenade.QueueFree();
        }
    }
    
}
