#pragma strict
//var GuiTexture : Texture2D;
var gamestate : int = 1;
var stressvalue: int =0;
var stress : int = 1;
var stressMetre : Texture;
var assignment1 : GUISkin;
var assignment2 : GUISkin;
var assignment3 : GUISkin;
var inventory : GUISkin;
 
function Start () {

}

function Update () {

	if(stressvalue == 0)
	{
		stress = 1;
	}
	if(stressvalue == 25)
	{
		stress = 2;
	}
	if(stressvalue == 50)
	{
		stress = 3;
	}
	if(stressvalue == 75)
	{
		stress = 4;
	}
	if(stressvalue == 100)
	{
		stress = 5;
	}
	if(stressvalue > 100)
	{
		stress = 6;
	}
}

function OnGUI() {

	if(gamestate == 1)
	{
		if(stress == 1){
		GUI.DrawTexture(Rect(Screen.width/10,Screen.height/1.1, Screen.width/15, Screen.height/Screen.height),stressMetre);
		}
		
		if(stress == 2){
		GUI.DrawTexture(Rect(Screen.width/10,Screen.height/1.2, Screen.width/15, Screen.height/10),stressMetre);
		}
		
		if(stress == 3){
		GUI.DrawTexture(Rect(Screen.width/10,Screen.height/1.5, Screen.width/15, Screen.height/4),stressMetre);
		}
		
		if(stress == 4){
		GUI.DrawTexture(Rect(Screen.width/10,Screen.height/2.4, Screen.width/15, Screen.height/2),stressMetre);
		}
		
		if(stress == 5){
		GUI.DrawTexture(Rect(Screen.width/10,Screen.height/2.8, Screen.width/15, Screen.height/1.8),stressMetre);
		}
		
		if(stress == 6){
			Application.LoadLevel("gameover");
		}
		
		GUI.skin = assignment1;
		if(GUI.Button(Rect(Screen.width/3,Screen.height/1.2, Screen.width/20, Screen.height/10)," "))
		{
			gamestate=3;
		}
		
		GUI.skin = assignment2;
		if(GUI.Button(Rect(Screen.width/2.4,Screen.height/1.2, Screen.width/20, Screen.height/10)," "))
		{
			gamestate=3;
		}
		
		GUI.skin = assignment3;
		if(GUI.Button(Rect(Screen.width/2,Screen.height/1.2, Screen.width/20, Screen.height/10)," "))
		{
			gamestate=3;
		}
		
		GUI.skin = inventory;
		if(GUI.Button(Rect(Screen.width/1.5,Screen.height/1.2, Screen.width/20, Screen.height/10)," "))
		{
			gamestate=4;
		}
	}
	
	if(gamestate == 3)
	{
	
	} 
	
	if(gamestate == 4)
	{
	
	}  	
}