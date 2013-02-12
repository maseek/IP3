#pragma strict
var GuiTexture : Texture2D;
var gamestate : int = 1;
var howtoTexture : Texture;
var menuTexture : Texture;
//public var menuPlane : GameObject;
var startSkin: GUISkin;
var howtoSkin: GUISkin;
var exitSkin: GUISkin;
 
function Start () {

}

function Update () {
 
}

function OnGUI() {

	if(gamestate == 1)
	{
		GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height),menuTexture);
		
    	GUI.skin = startSkin;
    	if (GUI.Button(Rect(Screen.width/10 ,Screen.height/2 ,Screen.width/5,Screen.height/10)," "))
    	{
        	Application.LoadLevel("lighting");
    	}
    	
    	GUI.skin = howtoSkin;
    	if (GUI.Button(Rect(Screen.width/10 ,Screen.height/2 + 75 ,Screen.width/5,Screen.height/10)," "))
    	{
        	gamestate=2;
    	}   
    	
    	GUI.skin = exitSkin;
    	if (GUI.Button(Rect(Screen.width/10 ,Screen.height/2 + 150 ,Screen.width/5,Screen.height/10)," "))
    	{
       		Application.Quit();
    	}
    }
    
    if(gamestate == 2)
    {   
    	GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height),howtoTexture);
    	GUI.skin = exitSkin;
    	if (GUI.Button(Rect(Screen.width/10 ,Screen.height/1.3 ,Screen.width/5,Screen.height/10)," "))
    	{
       		gamestate=1;
    	}
    }
    
   
 }
 
/*
function ResizeGUI(_rect : Rect) : Rect
{
   	var filScreenWidth = _rect.width / 800;
   	var rectWidth = filScreenWidth * Screen.width;
   	var filScreenHeight = _rect.height / 600;
   	var rectHeight = filScreenHeight * Screen.height;
   	var rectX = (_rect.x / 800) * Screen.width;
   	var rectY = (_rect.x / 600) * Screen.height;
}
*/