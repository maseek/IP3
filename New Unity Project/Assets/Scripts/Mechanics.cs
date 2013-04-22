using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;

public enum GamePlace { University, Cinema, Nightclub, Street, Apartments, Gym, Hospital };
public enum GameState { inGame, schedule, scheduleDayView, usingSomething, questionnaire, phone, studentApplication1, studentApplication2, phoneMessage, phoneMessages};

public class Mechanics : MonoBehaviour {
	
	private GameState gameState;
	public GameState GetGameState() {return gameState;}
	public void SetGameState(GameState gameState) {this.gameState = gameState;}
	private GamePlace gamePlacePosition;
	public GamePlace GetGamePlacePosition() {return gamePlacePosition;}
	public void SetGamePlacePosition(GamePlace gamePlacePosition) {this.gamePlacePosition = gamePlacePosition;}
	private Player playerScript;
	private CameraFade cameraFade;
	
	private DateTime currentDate;
	public Light dayLight;
	public Light nightLight;
	public Light cinemaLight;
	public Light universityLight;
	public Light apartmentsLight;
	public Light gymLight;
	private GameObject[] nightclubLights;
	private GameObject[] lamps;
	public float timeScale;
	
	private Schedule schedule;
	private string[] modules;
	private string[] books;
	private int[] modulesGenerated = {0, 0, 0};
	private List<Assignment> assignments = new List<Assignment>();
	private List<PhoneMessage> phoneMessages = new List<PhoneMessage>();
	private PhoneMessage currentMessage;
	
	public GUIStyle backStyle;
	public GUIStyle frontStyle;
	public GUISkin scheduleSkin;
	private DateTime selectedDay;
	private string selectedMonth;
	private string tooltipText;
	private ScheduleEvent reminderedEvent;
	private int lastReminderUpdateHour;
	
	private float dayLightIntensity;
	private float nightLightIntensity;
	
	public StressAndEnergyMeter stressMeter;
	public Texture2D stressMeterTexture;
	public Texture2D stressMeterTextureBg;
	//private Texture2D croppedStressMeterTexture;
	//private int lastStress;
	private float displayedStress;
	private float displayedStressAdj;
	public Texture2D energyBarTexture;
	public Texture2D energyBarTextureBg;
	public StressAndEnergyMeter energyMeter;
	private float displayedEnergy;
	private float displayedEnergyBg;
	private float displayedEnergyAdj;
	private float displayedEnergyBgAdj;
	
	private float phoneRotation = 0;
	private float phoneRotationSpeed = 0;
	public Texture2D phoneTexture;
	public Texture2D phoneSmallTexture;
	private Vector2 scheduleScrollViewVector = Vector2.zero;
	private Vector2 scheduleDayViewScrollViewVector = Vector2.zero;
	private Vector2 studentApplicationScrollViewVector = Vector2.zero;
	private Vector2 questionnaireScrollViewVector = Vector2.zero;
	private Vector2 phoneMessageScrollViewVector = Vector2.zero;
	private Vector2 phoneMessagesScrollViewVector = Vector2.zero;
	private float question1 = 0.0f;
	private float question2 = 0.0f;
	private float question3 = 0.0f;
	private float question4 = 0.0f;
	private float question5 = 0.0f;
	private float question6 = 1.0f;
	private string question7 = "";
	private string question8 = "";
	private string question9 = "";
	public Texture2D questionAnswer1;
	public Texture2D questionAnswer2;
	public Texture2D questionAnswer3;
	public Texture2D questionAnswer4;
	public Texture2D questionAnswer5;
	private bool studentApplication1 = false;
	private bool studentApplication2 = false;
	private bool studentApplication3 = false;
	private bool studentApplication4 = false;
	private bool studentApplication5 = false;
	private bool studentApplication6 = false;
	
	private float bankBalance = 500;
	private float monthlyGain = 0;
	public float GetBankBalance() { return bankBalance; }
	public void SetBankBalance(float bankBalance) { this.bankBalance = bankBalance; }
	private int lastBankBalanceUpdateMonth;
	
	private bool mouseOverGUI = false;
	public bool GetMouseOverGUI() { return mouseOverGUI; }
	private int layerMask;

	// Use this for initialization
	void Start () {
		gameState = GameState.inGame;
		Time.timeScale = 1;
		playerScript = (Player)FindObjectOfType(typeof(Player));
		cameraFade = (CameraFade)FindObjectOfType(typeof(CameraFade));		
		gamePlacePosition = GamePlace.Street;
		currentDate = new DateTime(2013, 9, 1, 12, 0, 0);
		lastBankBalanceUpdateMonth = currentDate.Month;
		dayLightIntensity = dayLight.intensity;
		nightLightIntensity = nightLight.intensity;
		selectedDay = currentDate;
		schedule = new Schedule();
		modules = new string[3]{"Intro To Programming", "Discrete Maths", "Intro to Web Development" };
		books = new string[]{"Cimple C", "", "Web Development: Creating sites that people don’t hate" };
		
		assignments.Add(new Assignment(modules[0], "Assignment 1", "Complete introductory exercises in Cimple C on a computer.", 
			60, 60, new DateTime(2013, 9, 9, 12, 0, 0), new DateTime(2013, 9, 13, 12, 0, 0)));
		assignments.Add(new Assignment(modules[0], "Assignment 2", "Create a system to manage appointments in a doctor’s surgery.", 
			90, 120, new DateTime(2013, 9, 30, 12, 0, 0), new DateTime(2013, 10, 4, 12, 0, 0)));
		
		assignments.Add(new Assignment(modules[2], "Assignment 1", "Look at 4 websites specified in textbook and evaluate them based on the guidelines given.", 
			10, 20, new DateTime(2013, 9, 16, 12, 0, 0), new DateTime(2013, 9, 20, 12, 0, 0)));
		assignments.Add(new Assignment(modules[2], "Assignment 2", "Create a web page that can display text, video and images.", 
			180, 180, new DateTime(2013, 10, 7, 12, 0, 0), new DateTime(2013, 10, 11, 12, 0, 0)));
		
		assignments.Add(new Assignment(modules[0], "Assignment 3", "Create a system for booking a holiday online.", 
			90, 120, new DateTime(2013, 10, 21, 12, 0, 0), new DateTime(2013, 10, 25, 12, 0, 0)));
		assignments.Add(new Assignment(modules[0], "Assignment 4", "Create a system to record attendance for a school.", 
			90, 120, new DateTime(2013, 11, 11, 12, 0, 0), new DateTime(2013, 11, 15, 12, 0, 0)));
		
		assignments.Add(new Assignment(modules[2], "Assignment 3", "Create a website that allows user to register for the site and log-in.", 
			120, 120, new DateTime(2013, 10, 28, 12, 0, 0), new DateTime(2013, 11, 1, 12, 0, 0)));
		assignments.Add(new Assignment(modules[2], "Assignment 4", "Create a website based on the needs of the uni shop.", 
			240, 360, new DateTime(2013, 11, 18, 12, 0, 0), new DateTime(2013, 11, 22, 12, 0, 0)));
		
		phoneMessages.Add(new PhoneMessage("NHS", "Questionnaire", "Please take a minute to fill in a "));	
		
		//currentMessage = new PhoneMessage("Intro To Programming", "Assignment 1", "Complete introductory exercises in Cimple C on a computer.\n\nDue Date: "+new DateTime(2013, 9, 13, 12, 0, 0).ToString("f"));
		currentMessage = null;
		
		GenerateTimetable(schedule);
		lamps = GameObject.FindGameObjectsWithTag("Lamp");
		nightclubLights = GameObject.FindGameObjectsWithTag("Nightclub Lights");
		stressMeter = new StressAndEnergyMeter();
		stressMeter.AdjustValue(50);
		//lastStress = stressMeter.stress-1;
		displayedStress = stressMeter.value;
		displayedStressAdj = 0.0f;
		energyMeter = new StressAndEnergyMeter();
		energyMeter.AdjustValue(100);
		displayedEnergy = energyMeter.value;
		displayedEnergyBg = energyMeter.value;
		displayedEnergyAdj = 0;
		displayedEnergyBgAdj = 0;
		
		// ignore vehicles and trigger layer
		layerMask = 3 << (13);
		Debug.Log(layerMask);
		layerMask = ~layerMask;
		Debug.Log(layerMask);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// check if game over
		if(displayedStress >= 100) {
			cameraFade.StartFade(Color.black, 3.0f, true);
			StartCoroutine(OnGameOver());
			//Application.LoadLevel("GameOver");	
		}
		
		// update stress bar size
		if(displayedStress > stressMeter.value) {
			if(displayedStressAdj != 0.0f) {
				displayedStress -= Time.fixedDeltaTime*20*Mathf.Sin((displayedStress-stressMeter.value)/displayedStressAdj*Mathf.PI/2+(Mathf.PI/3));
			}
			else {
				displayedStressAdj = (displayedStress-stressMeter.value)*1.5f;
			}
			if(displayedStress <= stressMeter.value) {
				displayedStress = stressMeter.value;
				displayedStressAdj = 0.0f;
			}
		}
		else if(displayedStress < stressMeter.value) {
			if(displayedStressAdj != 0.0f) {
				displayedStress += Time.fixedDeltaTime*20*Mathf.Sin((stressMeter.value-displayedStress)/displayedStressAdj*Mathf.PI/2+(Mathf.PI/3));
			}
			else {
				displayedStressAdj = (stressMeter.value-displayedStress)*1.5f;
			}
			if(displayedStress >= stressMeter.value) {
				displayedStress = stressMeter.value;
				displayedStressAdj = 0.0f;
			}
		}
		
		// update energy bar size
		if(displayedEnergy > Mathf.Floor(energyMeter.value)) {
			if(displayedEnergyAdj != 0.0f) {
				displayedEnergy -= Time.fixedDeltaTime*20*Mathf.Sin((displayedEnergy-energyMeter.value)/displayedEnergyAdj*Mathf.PI/2+(Mathf.PI/3));
			}
			else {
				displayedEnergyAdj = (displayedEnergy-energyMeter.value)*1.5f;
			}
			if(displayedEnergy <= Mathf.Floor(energyMeter.value)) {
				displayedEnergy = Mathf.Floor(energyMeter.value);
				displayedEnergyAdj = 0.0f;
			}
		}
		else if(displayedEnergy < Mathf.Floor(energyMeter.value)) {
			if(displayedEnergyAdj != 0.0f) {
				displayedEnergy += Time.fixedDeltaTime*20*Mathf.Sin((energyMeter.value-displayedEnergy)/displayedEnergyAdj*Mathf.PI/2+(Mathf.PI/3));
			}
			else {
				displayedEnergyAdj = (energyMeter.value-displayedEnergy)*1.5f;
			}
			if(displayedEnergy >= Mathf.Floor(energyMeter.value)) {
				displayedEnergy = Mathf.Floor(energyMeter.value);
				displayedEnergyAdj = 0.0f;
			}
		}
		if(displayedEnergyAdj == 0) {
			if(displayedEnergyBg > Mathf.Floor(energyMeter.value)) {
				if(displayedEnergyBgAdj != 0.0f) {
					displayedEnergyBg -= Time.fixedDeltaTime*20*Mathf.Sin((displayedEnergyBg-energyMeter.value)/displayedEnergyBgAdj*Mathf.PI/2+(Mathf.PI/3));
				}
				else {
					displayedEnergyBgAdj = (displayedEnergyBg-energyMeter.value)*1.5f;
				}
				if(displayedEnergyBg <= Mathf.Floor(energyMeter.value)) {
					displayedEnergyBg = Mathf.Floor(energyMeter.value);
					displayedEnergyBgAdj = 0.0f;
				}
			}
		}
		else if(displayedEnergyBg < Mathf.Floor(energyMeter.value)) {
			if(displayedEnergyBgAdj != 0.0f) {
				displayedEnergyBg += Time.fixedDeltaTime*20*Mathf.Sin((energyMeter.value-displayedEnergyBg)/displayedEnergyBgAdj*Mathf.PI/2+(Mathf.PI/3));
			}
			else {
				displayedEnergyBgAdj = (energyMeter.value-displayedEnergyBg)*1.5f;
			}
			if(displayedEnergyBg >= Mathf.Floor(energyMeter.value)) {
				displayedEnergyBg = Mathf.Floor(energyMeter.value);
				displayedEnergyBgAdj = 0.0f;
			}
		}
		
		if(Input.GetKeyDown(KeyCode.S)) {
			selectedDay = currentDate;
			gameState = GameState.schedule;
			Time.timeScale = 0;
		}
		if(Input.GetKeyDown(KeyCode.Q)) {
			gameState = GameState.questionnaire;
			Time.timeScale = 0;
		}
		if(Input.GetKeyDown(KeyCode.P)) {
			gameState = GameState.phone;
			Time.timeScale = 0;
		}
		if(Input.GetKeyDown(KeyCode.M)) {
			gameState = GameState.phoneMessages;
			Time.timeScale = 0;
		}
		
		// update reminder
		if(currentDate.Hour != lastReminderUpdateHour && currentDate.Minute >= 50) {
			UpdateReminderedEvent();	
		}
		
		// update bank balance
		if(currentDate.Month != lastBankBalanceUpdateMonth) {
			bankBalance += monthlyGain;
			lastBankBalanceUpdateMonth = currentDate.Month;
		}
		
		currentDate = currentDate.AddMinutes(Time.deltaTime*timeScale);
		energyMeter.AdjustValue(Time.deltaTime/10.0f);
		
		//Check for a building tooltip
		if(gameState == GameState.inGame) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
				if(hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 12)
				{
					if(hit.collider.gameObject.tag.Equals("Apartments") || hit.collider.gameObject.tag.Equals("Cinema") ||
						hit.collider.gameObject.tag.Equals("Gym") || hit.collider.gameObject.tag.Equals("Hospital") || 
						hit.collider.gameObject.tag.Equals("Nightclub") || hit.collider.gameObject.tag.Equals("University"))
					{
							tooltipText = hit.collider.gameObject.tag;
					}
					else if(hit.collider.gameObject.name.Equals("cinemaOut") || hit.collider.gameObject.name.Equals("nightclubOut") || hit.collider.gameObject.name.Equals("uniOut") || 
						hit.collider.gameObject.name.Equals("apartmentsOut") || hit.collider.gameObject.name.Equals("gymOut") ) {
						tooltipText = "To the street";	
					}
					else if(hit.collider.gameObject.name.Equals("classDoorIn")) {
						if(IsAnyClassOn())
							tooltipText = "Attend class - "+reminderedEvent.name;
						else
							tooltipText = "No class is on";
					}
					else if(hit.collider.gameObject.tag.Equals("Cinema Seat")) {
						tooltipText = "Watch a Film - £10";
					}
					else if(hit.collider.gameObject.tag.Equals("Nightclub Bar")) {
						tooltipText = "Have a drink - £3";
					}
					else if(hit.collider.gameObject.tag.Equals("Nightclub DanceFloor")) {
						tooltipText = "Dance!";
					}
					else if(hit.collider.gameObject.name.Equals("Bed")) {
						tooltipText = "Sleep";
					}
					else if(hit.collider.gameObject.name.Equals("treadmill")) {
						tooltipText = "Exercise";
					}
				}
				else tooltipText = "";
			}
		}
		else tooltipText = "";
		
		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			stressMeter.AdjustValue(10);	
			displayedStressAdj = 0.0f;
			energyMeter.AdjustValue(10);
			displayedEnergyAdj = 0.0f;
			displayedEnergyBgAdj = 0.0f;
		}
		if(Input.GetKeyDown(KeyCode.DownArrow)) {
			stressMeter.AdjustValue(-10);		
			displayedStressAdj = 0.0f;
			energyMeter.AdjustValue(-10);
			displayedEnergyAdj = 0.0f;
			displayedEnergyBgAdj = 0.0f;
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			timeScale -= 10;
			if(timeScale < 1)
				timeScale = 1;
		}
		if(Input.GetKeyDown(KeyCode.RightArrow)) {
			timeScale += 10;	
		}
		if(Input.GetKeyDown(KeyCode.F9)) {
			Application.LoadLevel("GameScene");	
		}
		
		dayLight.enabled = false;
		nightLight.enabled = false;
		cinemaLight.enabled = false;
		universityLight.enabled = false;
		apartmentsLight.enabled = false;
		gymLight.enabled = false;
		UpdateDayNight(gamePlacePosition == GamePlace.Street);
		if(gamePlacePosition == GamePlace.Nightclub) {
			/// Nightclub Light
			foreach(GameObject nightclubLight in nightclubLights) {
				Color nightclubLightColor = nightclubLight.light.color;
				nightclubLightColor.r += UnityEngine.Random.Range(-3.0f, 3.0f)*Time.fixedDeltaTime;	
				nightclubLightColor.g += UnityEngine.Random.Range(-3.0f, 3.0f)*Time.fixedDeltaTime;	
				nightclubLightColor.b += UnityEngine.Random.Range(-3.0f, 3.0f)*Time.fixedDeltaTime;
				nightclubLight.light.color = nightclubLightColor;
				nightclubLight.transform.Rotate(0, 
					UnityEngine.Random.Range(-1, 1)*180*Time.fixedDeltaTime, 
					UnityEngine.Random.Range(-1, 1)*180*Time.fixedDeltaTime);
			}
		}
		else if(gamePlacePosition == GamePlace.Cinema) {
			cinemaLight.enabled = true;
		}
		else if(gamePlacePosition == GamePlace.University) {
			universityLight.enabled = true;
		}
		else if(gamePlacePosition == GamePlace.Apartments) {
			apartmentsLight.enabled = true;
		}
		else if(gamePlacePosition == GamePlace.Gym) {
			gymLight.enabled = true;
		}
		
		// update phone rotation
		phoneRotation = (Mathf.Sin(Time.fixedTime*30.0f)*10.0f-5.0f)*phoneRotationSpeed;
		phoneRotationSpeed = Mathf.Sin(Time.fixedTime*2.0f)/2.0f+0.5f;
		
		playerScript.UpdateMinimapCamera();
	}
	
	void OnGUI() {
		mouseOverGUI= false;
		if(gameState == GameState.inGame || gameState == GameState.schedule || gameState == GameState.scheduleDayView || gameState == GameState.usingSomething || gameState == GameState.phone) {
			/// CLOCK
			string timeString = currentDate.ToShortTimeString()+"\n"+currentDate.ToString("D");
			backStyle.alignment = TextAnchor.UpperLeft;
			backStyle.fontSize = 24;
			frontStyle.alignment = TextAnchor.UpperLeft;
			frontStyle.fontSize = 24;
			GUI.Label(new Rect (20+16,20,300,60), timeString, backStyle);
		    GUI.Label(new Rect (20+14,20,300,60), timeString, backStyle);
		    GUI.Label(new Rect (20+15,20+1,300,60), timeString, backStyle);
		    GUI.Label(new Rect (20+15,20-1,300,60), timeString, backStyle);
		    GUI.Label(new Rect (20+15,20,300,60), timeString, frontStyle);
			
			/// STRESS METER
			GUI.DrawTexture(new Rect(35,Screen.height-335, 80, 300),stressMeterTextureBg);  
			if(displayedStress > 0.0f) {
				Rect stressRect = new Rect(35,Screen.height-35-displayedStress*3, 100, displayedStress*3);
				GUI.BeginGroup (stressRect);
				GUI.DrawTexture(new Rect(0,-300+displayedStress*3,80,300),stressMeterTexture);   
    			GUI.EndGroup ();
			}
			string stressString = ((int)displayedStress).ToString()+"/100";
			backStyle.alignment = TextAnchor.UpperCenter;
			frontStyle.alignment = TextAnchor.UpperCenter;
			GUI.Label(new Rect (23+26,Screen.height-80,50,60), stressString, backStyle);
		    GUI.Label(new Rect (23+24,Screen.height-80,50,60), stressString, backStyle);
		    GUI.Label(new Rect (23+25,Screen.height-80+1,50,60), stressString, backStyle);
		    GUI.Label(new Rect (23+25,Screen.height-80-1,50,60), stressString, backStyle);
		    GUI.Label(new Rect (23+25,Screen.height-80,50,60), stressString, frontStyle);
			
			/// ENERGY METER
			//GUI.DrawTexture(new Rect(35+100,Screen.height-335, 80, 300),energyBarTextureBg);  
			if(displayedEnergy > 0.0f) {
				Rect energyRect1 = new Rect(35+100,Screen.height-35-displayedEnergyBg*3, 100, displayedEnergyBg*3);
				GUI.BeginGroup (energyRect1);
				GUI.DrawTexture(new Rect(0,-300+displayedEnergyBg*3,80,300),energyBarTextureBg);   
    			GUI.EndGroup ();
				Rect energyRect2 = new Rect(35+100,Screen.height-35-displayedEnergy*3, 100, displayedEnergy*3);
				GUI.BeginGroup (energyRect2);
				GUI.DrawTexture(new Rect(0,-300+displayedEnergy*3,80,300),energyBarTexture);   
    			GUI.EndGroup ();
			}
			string energyString = ((int)displayedEnergy).ToString()+"/100";
			backStyle.alignment = TextAnchor.UpperCenter;
			frontStyle.alignment = TextAnchor.UpperCenter;
			GUI.Label(new Rect (23+26+100,Screen.height-80,50,60), energyString, backStyle);
		    GUI.Label(new Rect (23+24+100,Screen.height-80,50,60), energyString, backStyle);
		    GUI.Label(new Rect (23+25+100,Screen.height-80+1,50,60), energyString, backStyle);
		    GUI.Label(new Rect (23+25+100,Screen.height-80-1,50,60), energyString, backStyle);
		    GUI.Label(new Rect (23+25+100,Screen.height-80,50,60), energyString, frontStyle);
		}
		if(gameState == GameState.inGame || gameState == GameState.usingSomething) {
			/// REMINDER
			string reminder;
			if(reminderedEvent != null)
				reminder = reminderedEvent.name+"\n"+reminderedEvent.starts.ToString("f");
			else reminder = "";
			backStyle.alignment = TextAnchor.UpperRight;
			backStyle.fontSize = 24;
			frontStyle.alignment = TextAnchor.UpperRight;
			frontStyle.fontSize = 24;
			GUI.Label(new Rect (Screen.width-450+16,20,400,60), reminder, backStyle);
		    GUI.Label(new Rect (Screen.width-450+14,20,400,60), reminder, backStyle);
		    GUI.Label(new Rect (Screen.width-450+15,20+1,400,60), reminder, backStyle);
		    GUI.Label(new Rect (Screen.width-450+15,20-1,400,60), reminder, backStyle);
		    GUI.Label(new Rect (Screen.width-450+15,20,400,60), reminder, frontStyle);	
			// PHONE BUTTON
			//check for unread messages
			bool anyUnread = false;
			foreach(PhoneMessage pm in phoneMessages) {
				if(!pm.isRead)
					anyUnread = true;
			}
			
		    GUIStyle testStyle = new GUIStyle();
			
			Vector2 pivotPoint = new Vector2(Screen.width-78, Screen.height-78);
			if(anyUnread) {
       			GUIUtility.RotateAroundPivot(phoneRotation, pivotPoint);
			}
			
			if(GUI.Button(new Rect(Screen.width-115,Screen.height-195,75,155), phoneSmallTexture, testStyle)) {
				if(anyUnread)
					gameState = GameState.phoneMessages;
				else 
					gameState = GameState.phone;	
				Time.timeScale = 0;
			}
			mouseOverGUI = new Rect(Screen.width-115,Screen.height-195,75,155).Contains(new Vector2(Input.mousePosition.x, Screen.height-Input.mousePosition.y));
			
			if(anyUnread) {
				string newMessageText = "New\nMessage";
				backStyle.alignment = TextAnchor.UpperCenter;
				backStyle.fontSize = 15;
				frontStyle.alignment = TextAnchor.UpperCenter;
				frontStyle.fontSize = 15;
				GUI.Label(new Rect (Screen.width-131+16,Screen.height-145,75,60), newMessageText, backStyle);
			    GUI.Label(new Rect (Screen.width-131+14,Screen.height-145,75,60), newMessageText, backStyle);
			    GUI.Label(new Rect (Screen.width-131+15,Screen.height-145+1,75,60), newMessageText, backStyle);
			    GUI.Label(new Rect (Screen.width-131+15,Screen.height-145-1,75,60), newMessageText, backStyle);
			    GUI.Label(new Rect (Screen.width-131+15,Screen.height-145,75,60), newMessageText, frontStyle);
			}
			
			if(anyUnread) {
       			GUIUtility.RotateAroundPivot(-phoneRotation, pivotPoint);
			}
		}
		if(gameState == GameState.phone) {
			// PHONE
			GUI.DrawTexture(new Rect(Screen.width/2-512,Screen.height/2-247, 1024, 495),phoneTexture);  	
			if(GUI.Button(new Rect(Screen.width/2+405,Screen.height/2-22,44,44), "")) {
				gameState = GameState.inGame;	
				Time.timeScale = 1;
			}
			frontStyle.fontSize = 38;
			frontStyle.alignment = TextAnchor.UpperLeft;
			GUI.Label(new Rect (Screen.width/2-390,Screen.height/2-187, 500, 70), "Money Balance: "+bankBalance.ToString("c", new CultureInfo("en-GB")), frontStyle);
			GUI.skin.button.fontStyle = FontStyle.Normal;
			GUI.skin.button.fontSize = 30;
			GUI.skin.button.normal.textColor = new Color(1f, 1f, 1f, 1);
			GUI.skin.button.hover.textColor = new Color(0.1f, 1f, 1f, 1);
			if(GUI.Button(new Rect(Screen.width/2-390,Screen.height/2+137,155,40), "Schedule")) {
				gameState = GameState.schedule;
			}
			if(GUI.Button(new Rect(Screen.width/2-225,Screen.height/2+137,155,40), "Messages")) {
				gameState = GameState.phoneMessages;
			}
		}
		if(gameState == GameState.schedule) {
			/// SCHEDULE
			GUI.DrawTexture(new Rect(Screen.width/2-512,Screen.height/2-247, 1024, 495),phoneTexture);  	
			if(GUI.Button(new Rect(Screen.width/2+405,Screen.height/2-22,44,44), "")) {
				gameState = GameState.inGame;	
				Time.timeScale = 1;
			}
			scheduleScrollViewVector = GUI.BeginScrollView(new Rect(Screen.width/2-424,Screen.height/2-215,784,431), scheduleScrollViewVector, new Rect(0,0,764,431), false, false);
			
			selectedMonth = selectedDay.ToString("y");
			GUI.skin = scheduleSkin;
			GUI.skin.box.fontStyle = FontStyle.Bold;
			GUI.skin.box.fontSize = 24;
			GUI.skin.box.alignment = TextAnchor.UpperCenter;
			GUI.Box(new Rect(/*Screen.width/2-400*/0,/*Screen.height/2-300*/ 0,800,431), "Schedule");
			if(GUI.Button(new Rect(/*Screen.width/2+360*/740,/*Screen.height/2-295*/5,35,25), "X")) {
				gameState = GameState.phone;
			}
			GUI.skin.box.fontStyle = FontStyle.Normal;
			GUI.skin.box.fontSize = 22;
			GUI.Box(new Rect(/*Screen.width/2-390*/5,/*Screen.height/2-260*/40,780,550), selectedMonth);
			if(GUI.Button(new Rect(/*Screen.width/2-385*/10,/*Screen.height/2-255*/45,70,25), "<<")) {
				selectedDay = selectedDay.AddMonths(-1);
			}
			if(GUI.Button(new Rect(/*Screen.width/2+315*/710,/*Screen.height/2-255*/45,70,25), ">>")) {
				selectedDay = selectedDay.AddMonths(1);
			}
			GUI.skin.box.fontSize = 18;
			GUI.Box(new Rect(/*Screen.width/2-385*/10,/*Screen.height/2-225*/75,110,510), "Monday");
			GUI.Box(new Rect(/*Screen.width/2-275*/120,/*Screen.height/2-225*/75,110,510), "Tuesday");
			GUI.Box(new Rect(/*Screen.width/2-165*/230,/*Screen.height/2-225*/75,110,510), "Wednesday");
			GUI.Box(new Rect(/*Screen.width/2-55*/340,/*Screen.height/2-225*/75,110,510), "Thursday");
			GUI.Box(new Rect(/*Screen.width/2+55*/450,/*Screen.height/2-225*/75,110,510), "Friday");
			GUI.Box(new Rect(/*Screen.width/2+165*/560,/*Screen.height/2-225*/75,110,510), "Saturday");
			GUI.Box(new Rect(/*Screen.width/2+275*/670,/*Screen.height/2-225*/75,110,510), "Sunday");
			int row = 0;
			int column = 0;
			DateTime calendarDay = new DateTime(selectedDay.Year, selectedDay.Month, 1, 0, 0, 0);
			switch(calendarDay.DayOfWeek) {
			case DayOfWeek.Tuesday:
				column = 1;
				break;
			case DayOfWeek.Wednesday:
				column = 2;
				break;
			case DayOfWeek.Thursday:
				column = 3;
				break;
			case DayOfWeek.Friday:
				column = 4;
				break;
			case DayOfWeek.Saturday:
				column = 5;
				break;
			case DayOfWeek.Sunday:
				column = 6;
				break;
			}
			int days = DateTime.DaysInMonth(calendarDay.Year, calendarDay.Month);
			for(int i = 0; i < days; i++) {
				if(calendarDay.Day == currentDate.Day && calendarDay.Month == currentDate.Month && calendarDay.Year == currentDate.Year) {
					GUI.skin.button.fontStyle = FontStyle.Bold;
					GUI.skin.button.fontSize = 36;
					GUI.skin.button.normal.textColor = new Color(0.1f, 1f, 1f, 1);
					GUI.skin.button.hover.textColor = new Color(0.1f, 1f, 1f, 1);
				}
				else {
					GUI.skin.button.fontStyle = FontStyle.Normal;
					GUI.skin.button.fontSize = 30;
					GUI.skin.button.normal.textColor = new Color(1f, 1f, 1f, 1);
					GUI.skin.button.hover.textColor = new Color(0.1f, 1f, 1f, 1);
				}
				if(GUI.Button(new Rect(/*Screen.width/2-385*/10+column*110,/*Screen.height/2-195*/105+row*52,110,52),calendarDay.Day.ToString())) {
					selectedDay = calendarDay;
					gameState = GameState.scheduleDayView;
				}
				column += 1;
				calendarDay = calendarDay.AddDays(1);
				if(calendarDay.DayOfWeek == DayOfWeek.Monday) {
					row += 1;
					column = 0;
				}
			}
			
			GUI.EndScrollView();
		}
		if(gameState == GameState.scheduleDayView) {
			/// SCHEDULE DAY VIEW
			GUI.DrawTexture(new Rect(Screen.width/2-512,Screen.height/2-247, 1024, 495),phoneTexture);  	
			if(GUI.Button(new Rect(Screen.width/2+405,Screen.height/2-22,44,44), "")) {
				gameState = GameState.inGame;	
				Time.timeScale = 1;
			}
			scheduleDayViewScrollViewVector = GUI.BeginScrollView(new Rect(Screen.width/2-424,Screen.height/2-215,784,431), scheduleDayViewScrollViewVector, new Rect(0,0,764,600), false, true);
			
			GUI.skin = scheduleSkin;
			GUI.skin.box.fontStyle = FontStyle.Bold;
			GUI.skin.box.fontSize = 24;
			GUI.skin.box.alignment = TextAnchor.UpperCenter;
			GUI.Box(new Rect(/*Screen.width/2-400*/0,/*Screen.height/2-300*/0,800,600), "Schedule");
			GUI.skin.button.fontSize = 18;
			if(GUI.Button(new Rect(/*Screen.width/2-395*/10,/*Screen.height/2-295*/5,55,25), "Back")) {
				gameState = GameState.schedule;	
			}
			GUI.skin.button.fontSize = 30;
			if(GUI.Button(new Rect(/*Screen.width/2+360*/730,/*Screen.height/2-295*/5,35,25), "X")) {
				gameState = GameState.phone;	
			}
			GUI.skin.box.fontStyle = FontStyle.Normal;
			GUI.skin.box.fontSize = 22;
			GUI.Box(new Rect(/*Screen.width/2-390*/5,/*Screen.height/2-260*/40,780,550), selectedDay.ToString("D"));
			if(GUI.Button(new Rect(/*Screen.width/2-385*/10,/*Screen.height/2-255*/45,70,25), "<<")) {
				selectedDay = selectedDay.AddDays(-1);
			}
			if(GUI.Button(new Rect(/*Screen.width/2+315*/700,/*Screen.height/2-255*/45,70,25), ">>")) {
				selectedDay = selectedDay.AddDays(1);
			}
			TimeSpan ts = new TimeSpan(9, 0, 0);
			selectedDay = selectedDay.Date + ts;
			GUI.skin.box.alignment = TextAnchor.UpperLeft;
			for(int i = 0; i < 14; i++) {
				ScheduleEvent anEvent = schedule.GetEvent(selectedDay);
				string activity;
				if(anEvent != null)
					activity = selectedDay.ToString("t")+": "+anEvent.PlaceString()+" - "+anEvent.name;
				else
					activity = selectedDay.ToString("t");
				GUI.Box(new Rect(/*Screen.width/2-385*/10,/*Screen.height/2-220*/80+i*36,770,34), activity);
				selectedDay = selectedDay.AddHours(1d);
			}		
			
			GUI.EndScrollView();
		}
		if(gameState == GameState.questionnaire) {
			/// QUESTIONNAIRE
			GUI.DrawTexture(new Rect(Screen.width/2-512,Screen.height/2-247, 1024, 495),phoneTexture);  	
			if(GUI.Button(new Rect(Screen.width/2+405,Screen.height/2-22,44,44), "")) {
				gameState = GameState.inGame;	
				Time.timeScale = 1;
			}
			questionnaireScrollViewVector = GUI.BeginScrollView(new Rect(Screen.width/2-424,Screen.height/2-215,784,431), questionnaireScrollViewVector, new Rect(0,0,764,1330), false, true);
			
			GUI.skin = scheduleSkin;
			GUI.skin.box.fontStyle = FontStyle.Bold;
			GUI.skin.box.fontSize = 24;
			GUI.skin.box.alignment = TextAnchor.UpperLeft;
			GUI.Box(new Rect(15,15,735,115), "How do you feel today?");
			if(question1 == 0.0f) {
				GUI.Label(new Rect(577, 15, 75, 75), questionAnswer1);
			}
			else if(question1 == 1.0f) {
				GUI.Label(new Rect(577, 15, 75, 75), questionAnswer2);
			}
			else if(question1 == 2.0f) {
				GUI.Label(new Rect(577, 15, 75, 75), questionAnswer3);
			}
			else if(question1 == 3.0f) {
				GUI.Label(new Rect(577, 15, 75, 75), questionAnswer4);
			}
			else if(question1 == 4.0f) {
				GUI.Label(new Rect(577, 15, 75, 75), questionAnswer5);
			}
			question1 = Mathf.Round(GUI.HorizontalSlider(new Rect(490, 100, 250, 30), question1, 0.0f, 4.0f));
			
			int i = 1;
			GUI.Box(new Rect(15,15+120*i,735,115), "How do you feel about your university\nwork?");
			if(question2 == 0.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer1);
			}
			else if(question2 == 1.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer2);
			}
			else if(question2 == 2.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer3);
			}
			else if(question2 == 3.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer4);
			}
			else if(question2 == 4.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer5);
			}
			question2 = Mathf.Round(GUI.HorizontalSlider(new Rect(490, 100+120*i, 250, 30), question2, 0.0f, 4.0f));
			++i;
			
			GUI.Box(new Rect(15,15+120*i,735,115), "How do you feel about your classes?");
			if(question3 == 0.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer1);
			}
			else if(question3 == 1.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer2);
			}
			else if(question3 == 2.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer3);
			}
			else if(question3 == 3.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer4);
			}
			else if(question3 == 4.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer5);
			}
			question3 = Mathf.Round(GUI.HorizontalSlider(new Rect(490, 100+120*i, 250, 30), question3, 0.0f, 4.0f));
			++i;
			
			GUI.Box(new Rect(15,15+120*i,735,115), "How do you feel about money?");
			if(question4 == 0.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer1);
			}
			else if(question4 == 1.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer2);
			}
			else if(question4 == 2.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer3);
			}
			else if(question4 == 3.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer4);
			}
			else if(question4 == 4.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer5);
			}
			question4 = Mathf.Round(GUI.HorizontalSlider(new Rect(490, 100+120*i, 250, 30), question4, 0.0f, 4.0f));
			++i;
			
			GUI.Box(new Rect(15,15+120*i,735,115), "How do you feel about your social life?\nAre you satisfied?");
			if(question5 == 0.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer1);
			}
			else if(question5 == 1.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer2);
			}
			else if(question5 == 2.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer3);
			}
			else if(question5 == 3.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer4);
			}
			else if(question5 == 4.0f) {
				GUI.Label(new Rect(577, 15+120*i, 75, 75), questionAnswer5);
			}
			question5 = Mathf.Round(GUI.HorizontalSlider(new Rect(490, 100+120*i, 250, 30), question5, 0.0f, 4.0f));
			++i;
			
			GUI.Box(new Rect(15,15+120*i,735,125), "Rate how stressed you feel. Imagine\nthat 1 is completely relaxed\nand carefree and 10 is the most\nstressed you can be.");
			GUI.skin.label.fontSize = 30;
			GUI.skin.label.fontStyle = FontStyle.Bold;
			GUI.skin.label.alignment = TextAnchor.UpperCenter;
			GUI.Label(new Rect(577, 55+120*i, 75, 75), question6.ToString());
			question6 = Mathf.Round(GUI.HorizontalSlider(new Rect(490, 100+120*i, 250, 30), question6, 1.0f, 10.0f));
			++i;
			
			GUI.skin.textArea.fontSize = 24;
			GUI.Box(new Rect(15,25+120*i,735,165), "Which events have caused\nyou the most stress in the\nlast week?");
			question7 = GUI.TextArea(new Rect(350, 30+120*i, 395, 155), question7);
			++i;
			GUI.skin.textArea.fontSize = 24;
			GUI.Box(new Rect(15,75+120*i,735,165), "How did you react?");
			question8 = GUI.TextArea(new Rect(350, 80+120*i, 395, 155), question8);
			++i;
			GUI.skin.textArea.fontSize = 24;
			GUI.Box(new Rect(15,125+120*i,735,165), "If you could go back and\nchange how you dealt with\nthe situation, what would\nyou do differently?");
			question9 = GUI.TextArea(new Rect(350, 130+120*i, 395, 155), question9);
			++i;
			
			if(GUI.Button(new Rect(640,1260,105,55), "Save")) {
				gameState = GameState.inGame;	
				Time.timeScale = 1;
			}
			
			GUI.EndScrollView();
		}
		if(gameState == GameState.studentApplication1) {
			// STUDENT APPLICATION RENT
			GUI.DrawTexture(new Rect(Screen.width/2-512,Screen.height/2-247, 1024, 495),phoneTexture);  	
			if(GUI.Button(new Rect(Screen.width/2+405,Screen.height/2-22,44,44), "")) {
				gameState = GameState.inGame;	
				Time.timeScale = 1;
			}
			studentApplicationScrollViewVector = GUI.BeginScrollView(new Rect(Screen.width/2-424,Screen.height/2-215,784,431), studentApplicationScrollViewVector, new Rect(0,0,764,431), false, false);
			
			GUI.skin = scheduleSkin;
			GUI.skin.box.fontStyle = FontStyle.Bold;
			GUI.skin.box.fontSize = 24;
			GUI.skin.box.alignment = TextAnchor.UpperCenter;
			GUI.Box(new Rect(/*Screen.width/2-400*/0,/*Screen.height/2-300*/0,800,600), "Student Application - Rent");
			GUI.skin.button.fontSize = 18;
			
			GUI.skin.toggle.fontSize = 24;
			GUI.skin.toggle.fontStyle = FontStyle.Bold;
			GUI.skin.box.alignment = TextAnchor.UpperLeft;
			studentApplication1 = GUI.Toggle(new Rect(30, 50, 500, 50), !studentApplication2 && !studentApplication3, " Living with parent(s)");
			studentApplication2 = GUI.Toggle(new Rect(30, 100, 500, 50), !studentApplication1 && !studentApplication3, " Living alone");
			studentApplication3 = GUI.Toggle(new Rect(30, 150, 500, 50), !studentApplication1 && !studentApplication2, " Living in student halls");
			string cost = "";
			if(studentApplication1)
				cost = "Free";
			else if(studentApplication2)
				cost = "£400 / month";
			else if(studentApplication3)
				cost = "£300 / month";
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.fontStyle = FontStyle.Normal;
			GUI.skin.label.fontSize = 24;
			GUI.Label(new Rect(30, 200, 750, 75), "Accommodation Cost: "+cost);
			
			if(GUI.Button(new Rect(640,360,105,55), "Next")) {
				gameState = GameState.studentApplication2;
			}
			
			GUI.EndScrollView();
		}
		if(gameState == GameState.studentApplication2) {
			// STUDENT APPLICATION FINANCE
			GUI.DrawTexture(new Rect(Screen.width/2-512,Screen.height/2-247, 1024, 495),phoneTexture);  	
			if(GUI.Button(new Rect(Screen.width/2+405,Screen.height/2-22,44,44), "")) {
				gameState = GameState.inGame;	
				Time.timeScale = 1;
			}
			studentApplicationScrollViewVector = GUI.BeginScrollView(new Rect(Screen.width/2-424,Screen.height/2-215,784,431), studentApplicationScrollViewVector, new Rect(0,0,764,431), false, false);
			
			GUI.skin = scheduleSkin;
			GUI.skin.box.fontStyle = FontStyle.Bold;
			GUI.skin.box.fontSize = 24;
			GUI.skin.box.alignment = TextAnchor.UpperCenter;
			GUI.Box(new Rect(/*Screen.width/2-400*/0,/*Screen.height/2-300*/0,800,600), "Student Application - Finance");
			GUI.skin.button.fontSize = 18;
			
			GUI.skin.toggle.fontSize = 24;
			GUI.skin.toggle.fontStyle = FontStyle.Bold;
			GUI.skin.box.alignment = TextAnchor.UpperLeft;
			studentApplication4 = GUI.Toggle(new Rect(30, 50, 500, 50), !studentApplication5 && !studentApplication6, " Student loan");
			studentApplication5 = GUI.Toggle(new Rect(30, 100, 500, 50), !studentApplication4 && !studentApplication6, " Parental support");
			studentApplication6 = GUI.Toggle(new Rect(30, 150, 500, 50), !studentApplication4 && !studentApplication5, " No financial support");
			
			string cost = "";
			if(studentApplication1)
				cost = "Free";
			else if(studentApplication2)
				cost = "£400 / month";
			else if(studentApplication3)
				cost = "£300 / month";
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.fontStyle = FontStyle.Normal;
			GUI.skin.label.fontSize = 24;
			GUI.Label(new Rect(30, 200, 750, 75), "Accommodation Cost: "+cost);
			
			string gain = "";
			if(studentApplication4) {
				if(studentApplication1)
					gain = "£250 / month";
				else
					gain = "£500 / month";
			}
			else if(studentApplication5) {
				gain = "£200 / month";
			}
			else
				gain = "£0";
			GUI.Label(new Rect(30, 240, 750, 75), "Money support: "+gain);
			
			if(studentApplication1) {
				if(studentApplication4)
					monthlyGain = 250;
				else if(studentApplication5)
					monthlyGain = 200;
				else
					monthlyGain = 0;
			}
			else if(studentApplication2) {
				if(studentApplication4)
					monthlyGain = 100;
				else if(studentApplication5)
					monthlyGain = -200;
				else
					monthlyGain = -400;
			}
			else if(studentApplication3) {
				if(studentApplication4)
					monthlyGain = 200;
				else if(studentApplication5)
					monthlyGain = -100;
				else
					monthlyGain = -300;
			}
			GUI.skin.label.fontStyle = FontStyle.Bold;
			GUI.Label(new Rect(30, 280, 750, 75), "Total: "+monthlyGain.ToString("c", new CultureInfo("en-GB"))+" / month");
			
			if(GUI.Button(new Rect(30,360,105,55), "Previous")) {
				gameState = GameState.studentApplication1;
			}
			if(GUI.Button(new Rect(640,360,105,55), "Apply")) {
				gameState = GameState.inGame;
				Time.timeScale = 1;
			}
			
			GUI.EndScrollView();
		}
		if(gameState == GameState.phoneMessages) {
			// PHONE MESSAGES
			GUI.DrawTexture(new Rect(Screen.width/2-512,Screen.height/2-247, 1024, 495),phoneTexture);  	
			if(GUI.Button(new Rect(Screen.width/2+405,Screen.height/2-22,44,44), "")) {
				gameState = GameState.inGame;	
				Time.timeScale = 1;
			}
			phoneMessagesScrollViewVector = GUI.BeginScrollView(new Rect(Screen.width/2-424,Screen.height/2-215,784,431), phoneMessagesScrollViewVector, new Rect(0,0,764+(phoneMessages.Count>22?phoneMessages.Count*32:0),431), false, false);
			
			GUI.skin.button.fontSize = 30;
			if(GUI.Button(new Rect(/*Screen.width/2+360*/730,/*Screen.height/2-295*/5,35,35), "X")) {
				gameState = GameState.phone;	
			}
			
			if(phoneMessages.Count > 0) {
				phoneMessages.Reverse();
				GUI.skin.button.fontSize = 20;
				GUI.skin.button.alignment = TextAnchor.UpperLeft;
				int i = 0;
				foreach(PhoneMessage pm in phoneMessages) {
					string messagePrev = pm.fromName+" - "+pm.subject;
					if(pm.isRead)
						GUI.skin.button.fontStyle = FontStyle.Normal;
					else
						GUI.skin.button.fontStyle = FontStyle.Bold;
					if(GUI.Button(new Rect(10, 40+i*32, 754, 30), messagePrev)) {
						currentMessage = pm;
						gameState = GameState.phoneMessage;
					}
					++i;
				}
				phoneMessages.Reverse();	
			}
			else {
				GUI.skin.label.fontSize = 20;
				GUI.skin.label.alignment = TextAnchor.UpperLeft;
				GUI.Label(new Rect(10, 40, 754, 30), "No messages");
			}
			
			GUI.EndScrollView();
		}
		if(gameState == GameState.phoneMessage) {
			// PHONE MESSAGE
			
			if(!currentMessage.isRead) {
				if(currentMessage.subject == "Questionnaire") {
					phoneMessages.Add(new PhoneMessage("Student Support Agency", "Student Support Application", "Please take a minute to fill in your "));		
				}
				currentMessage.isRead = true;
			}
			
			GUI.DrawTexture(new Rect(Screen.width/2-512,Screen.height/2-247, 1024, 495),phoneTexture);  	
			if(GUI.Button(new Rect(Screen.width/2+405,Screen.height/2-22,44,44), "")) {
				gameState = GameState.inGame;	
				Time.timeScale = 1;
			}
			phoneMessageScrollViewVector = GUI.BeginScrollView(new Rect(Screen.width/2-424,Screen.height/2-215,784,431), phoneMessageScrollViewVector, new Rect(0,0,764,431), false, false);
			
			GUI.skin.button.fontSize = 18;
			if(GUI.Button(new Rect(/*Screen.width/2-395*/10,/*Screen.height/2-295*/5,55,25), "Back")) {
				gameState = GameState.phoneMessages;	
			}
			GUI.skin.button.fontSize = 30;
			if(GUI.Button(new Rect(/*Screen.width/2+360*/730,/*Screen.height/2-295*/5,35,35), "X")) {
				gameState = GameState.phone;	
			}
			
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.fontStyle = FontStyle.Bold;
			GUI.skin.label.fontSize = 20;
			GUI.Label(new Rect(30, 40, 750, 30), "From: "+currentMessage.fromName);
			GUI.Label(new Rect(30, 70, 750, 30), "Subject: "+currentMessage.subject);
			GUI.skin.label.fontStyle = FontStyle.Normal;
			GUI.Label(new Rect(30, 120, 750, 330), currentMessage.text);
			if(currentMessage.subject == "Questionnaire") {
				GUI.skin.button.fontSize = 18;
				if(GUI.Button(new Rect(310, 120, 135, 30), "Questionnaire")) {
					gameState = GameState.questionnaire;	
				}
			}
			if(currentMessage.subject == "Student Support Application") {
				GUI.skin.button.fontSize = 18;
				if(GUI.Button(new Rect(345, 120, 263, 30), "Application for Student Support")) {
					gameState = GameState.studentApplication1;	
				}
			}
			
			GUI.EndScrollView();
		}
		if (tooltipText != "" && (gameState == GameState.inGame || gameState == GameState.usingSomething)) {
			/// BUILDING TOOLTIP
			backStyle.alignment = TextAnchor.UpperLeft;
			frontStyle.alignment = TextAnchor.UpperLeft;
			backStyle.fontSize = 24;
			frontStyle.fontSize = 24;
	        var x = Event.current.mousePosition.x;
	        var y = Event.current.mousePosition.y;
	        GUI.Label(new Rect (x+16,y,300,60), tooltipText, backStyle);
	        GUI.Label(new Rect (x+14,y,300,60), tooltipText, backStyle);
	        GUI.Label(new Rect (x+15,y+1,300,60), tooltipText, backStyle);
	        GUI.Label(new Rect (x+15,y-1,300,60), tooltipText, backStyle);
	        GUI.Label(new Rect (x+15,y,300,60), tooltipText, frontStyle);
	    }
		
		playerScript.UpdateMinimapCamera();
	}
	
	private IEnumerator OnGameOver() {
		yield return new WaitForSeconds(1.5f);
		Application.LoadLevel("GameOver");
	}
	
	void UpdateDayNight(bool isOnTheStreet) {
		float hour = currentDate.Hour;
		float minute = currentDate.Minute;
		if(isOnTheStreet) {
			if(hour <= 18 || hour >= 6)
			{
				dayLight.enabled = true;
				nightLight.enabled = false;
			}
			else
			{
				dayLight.enabled = false;
				nightLight.enabled = true;
			}
			// lamps light up at 16 and turn off at 8 o'clock
			if(hour >= 16 || hour <= 8)
			{
				foreach(GameObject lamp in lamps)
					lamp.light.enabled = true;	
			}
			else
			{
				foreach(GameObject lamp in lamps)
					lamp.light.enabled = false;	
			}
		}
		dayLight.intensity = dayLightIntensity*Mathf.Sin((hour-6+minute/60)*Mathf.PI/12);
		nightLight.intensity = nightLightIntensity*Mathf.Sin((hour-18+minute/60)*Mathf.PI/12);
		dayLight.transform.RotateAround(Vector3.zero, new Vector3(1, 0, 1), Time.deltaTime*0.25f*timeScale);
		nightLight.transform.RotateAround(Vector3.zero, new Vector3(1, 0, 1), Time.deltaTime*0.25f*timeScale);
	}
	
	public void RollForwardInTime(float minutes) {
		currentDate = currentDate.AddMinutes(minutes);
		dayLight.transform.RotateAround(Vector3.zero, new Vector3(1, 0, 1), minutes/60.0f);
		nightLight.transform.RotateAround(Vector3.zero, new Vector3(1, 0, 1), minutes/60.0f);
	}
	
	public void GenerateTimetable(Schedule schedule) {
		int[] moduleDay = new int[3];
		moduleDay[0] = UnityEngine.Random.Range(0, 4);
		do {
			moduleDay[1] = UnityEngine.Random.Range(0, 4);
		} while(moduleDay[0] == moduleDay[1]);
		do {
			moduleDay[2] = UnityEngine.Random.Range(0, 4);
		} while(moduleDay[0] == moduleDay[2] || moduleDay[1] == moduleDay[2]);
		// starts from 9 to 11
		int[] moduleHour = new int[3];
		moduleHour[0] = UnityEngine.Random.Range(9, 12);
		moduleHour[1] = UnityEngine.Random.Range(9, 12);
		moduleHour[2] = UnityEngine.Random.Range(9, 12);
		// break starts from 11 to 13
		int[] moduleBreak = new int[3];
		moduleBreak[0] = UnityEngine.Random.Range(11, 14);
		moduleBreak[1] = UnityEngine.Random.Range(11, 14);
		moduleBreak[2] = UnityEngine.Random.Range(11, 14);
		int[] moduleBreakLen = new int[3];
		// break lasts 1 to 3 hours
		moduleBreakLen[0] = UnityEngine.Random.Range(1, 4);
		moduleBreakLen[1] = UnityEngine.Random.Range(1, 4);
		moduleBreakLen[2] = UnityEngine.Random.Range(1, 4);
		// module for 5 to 7 hours
		int[] moduleLen = new int[3];
		moduleLen[0] = UnityEngine.Random.Range(5, 8);
		moduleLen[1] = UnityEngine.Random.Range(5, 8);
		moduleLen[2] = UnityEngine.Random.Range(5, 8);
		for(int i = 0; i < 3; ++i) {
			for(int hours = 0; hours < moduleLen[i]; ++hours) {
				DateTime startDate = new DateTime(2013, 9, 2+moduleDay[i], moduleHour[i]+hours+((moduleHour[i]+hours+1 > moduleBreak[i]) ? (moduleBreakLen[i]) : 0), 0, 0);
				int moduleNo = UnityEngine.Random.Range(0, 3);
				while(modulesGenerated[moduleNo] >= moduleLen[moduleNo])
					moduleNo = UnityEngine.Random.Range(0, 3);
				modulesGenerated[moduleNo]++;
				
				for(int week = 0; week < 12; ++week) {
					ScheduleEvent moduleEvent = new ScheduleEvent(modules[moduleNo], startDate, GamePlace.University);
					schedule.AddEvent(moduleEvent);
					startDate = startDate.AddDays(7);
				}
				//Debug.Log (modules[i]+" "+startDate+"H:"+moduleHour[i]+"B"+moduleBreak[i]+"L"+moduleBreakLen[i]+"M"+moduleLen[i]+"HH"+hours);
				
			}
		}
		UpdateReminderedEvent();
	}
	
	public void UpdateReminderedEvent() {
		reminderedEvent = schedule.FindNextEvent(currentDate);
		lastReminderUpdateHour = currentDate.Hour;
		CheckForNewAssignment(currentDate);
	}
	
	public bool IsAnyClassOn() {
		if(reminderedEvent.starts.Month == currentDate.Month && reminderedEvent.starts.Day == currentDate.Day && 
			((reminderedEvent.starts.Hour == currentDate.Hour && currentDate.Minute < 50) || 
			(reminderedEvent.starts.Hour == currentDate.Hour + 1 && currentDate.Minute >= 50)))
		return true;
		return false;
	}
	
	public int HowManyMinutesInClass() {
		if(currentDate.Minute < 50)
			return 50 - currentDate.Minute;
		else
			return 110 - currentDate.Minute;
	}
	
	public void CheckForNewAssignment(DateTime date) {
		TimeSpan ts = new TimeSpan(date.Hour, 0, 0);
		DateTime searchDate = date.Date + ts;
		if(date.Minute >= 50)
			searchDate = searchDate.AddHours(1);
		
		int i = 0;
		foreach(Assignment ass in assignments) {
			if(ass.starts == searchDate) {
			//if(i++ < 4) {
				CreateAssignmentNotification(ass);
			}
		}
	}
	
	public void CreateAssignmentNotification(Assignment ass) {
		phoneMessages.Add(new PhoneMessage(ass.module, ass.name, ass.description+"\n\n"+"Due Date: "+ass.finishes.ToString("f")));	
	}
}

public class Schedule
{
	public Dictionary<DateTime, ScheduleEvent> events;
	public Schedule() {
		events = new Dictionary<DateTime, ScheduleEvent>();
	}
	
	public bool AddEvent(ScheduleEvent anEvent) {
		try {
			events.Add(anEvent.starts, anEvent);
			return true;
		}
        catch (ArgumentException) {
            return false;
        }
		//events.Sort(new ScheduleEventComparer());
	}
	
	public ScheduleEvent GetEvent(DateTime key) {
		try {
			return events[key];
		}
		catch (KeyNotFoundException) {
			return null;	
		}
	}
	
	public ScheduleEvent FindNextEvent(DateTime date) {
		TimeSpan ts = new TimeSpan(date.Hour, 0, 0);
		DateTime searchDate = date.Date + ts;
		if(date.Minute >= 50)
			searchDate = searchDate.AddHours(1);
		ScheduleEvent anEvent = null;
		for(int i = 0; i < 96; ++i) {
			anEvent = GetEvent(searchDate);
			if(anEvent == null)
				searchDate = searchDate.AddHours(1);
			else
				break;
		}
		return anEvent;
	}
}

public class ScheduleEvent
{
	public string name;
	public DateTime starts;
	public GamePlace place;
	public ScheduleEvent(string name, DateTime starts, GamePlace place) {
		this.name = name;
		this.starts = starts;
		this.place = place;
	}
	
	public string PlaceString() {
		return place.ToString();	
	}
}

public class StressAndEnergyMeter
{
	public float value;
	public StressAndEnergyMeter() {
		value = 0;	
	}	
	public void AdjustValue(float amount) {
		value += amount;
		if(value > 100)
			value = 100;
		else if(value < 0)
			value = 0;
	}
}

public class Assignment
{
	public string module;
	public string name;
	public string description;
	public float timeRequiredWithBook;
	public float timeRequiredWithoutBook;
	public DateTime starts;
	public DateTime finishes;
	
	public Assignment(string module, string name, string description, float timeRequiredWithBook, float timeRequiredWithoutBook, DateTime starts, DateTime finishes) {
		this.module = module;
		this.name = name;
		this.description = description;
		this.timeRequiredWithBook = timeRequiredWithBook;
		this.timeRequiredWithoutBook = timeRequiredWithoutBook;
		this.starts = starts;
		this.finishes = finishes;
	}
}

public class PhoneMessage
{
	public string fromName;
	public string subject;
	public string text;
	public bool isRead = false;
	
	public PhoneMessage(string fromName, string subject, string text) {
		this.fromName = fromName;
		this.subject = subject;
		this.text = text;
	}
}