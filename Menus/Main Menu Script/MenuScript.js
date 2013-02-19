#pragma strict
var gamestate : int = 1;
var moreinfoTexture : Texture;
var highscoreTexture : Texture;
var menuTexture : Texture;
var startSkin: GUISkin;
var moreinfoSkin: GUISkin;
var exitSkin: GUISkin;
var highscoreSkin: GUISkin;
 
function Start () {

}

function Update () {
 
}

function OnGUI() {

	if(gamestate == 1)
	{
		GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height),menuTexture);
		
    	GUI.skin = startSkin;
    	if (GUI.Button(Rect(Screen.width/10 ,Screen.height/4.5 ,Screen.width/5,Screen.height/10)," "))
    	{
        	Application.LoadLevel("lighting");
    	}
    	
    	GUI.skin = moreinfoSkin;
    	if (GUI.Button(Rect(Screen.width/10 ,Screen.height/3.5+50 ,Screen.width/5,Screen.height/10)," "))
    	{
        	gamestate=2;
    	}   
    	
    	GUI.skin = highscoreSkin;
    	if (GUI.Button(Rect(Screen.width/10 ,Screen.height/2.5+50 ,Screen.width/5,Screen.height/10)," "))
    	{
    		gamestate=3;
    	}
    	
    	GUI.skin = exitSkin;
    	if (GUI.Button(Rect(Screen.width/10 ,Screen.height/1.5-30,Screen.width/5,Screen.height/10)," "))
    	{
       		Application.Quit();
    	}
    }
    
    if(gamestate == 2)
    {   
    	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height),moreinfoTexture);
    	GUI.skin = exitSkin;
    	if (GUI.Button(Rect(Screen.width/10 ,Screen.height/1.3 ,Screen.width/5,Screen.height/10)," "))
    	{
       		gamestate=1;
    	}
    }
   
     if(gamestate == 3)
    {   
    	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height),highscoreTexture);
    	GUI.skin = exitSkin;
    	if (GUI.Button(Rect(Screen.width/10 ,Screen.height/1.3 ,Screen.width/5,Screen.height/10)," "))
    	{
       		gamestate=1;
    	}
    } 
   
 }
