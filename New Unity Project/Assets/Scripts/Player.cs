using UnityEngine;
using System.Collections;
using Pathfinding;
using System.Collections.Generic;
using System;

public enum PlayerState { Standing, Walking, Running, Drinking, Dancing, RunningTreadmill, Sleeping, Sitting };

public class Player : MonoBehaviour {
    //The point to move to
    public Vector3 targetPosition;
    
    private Seeker seeker;
    private CharacterController controller;
 
    //The calculated path
    public Path path;
    
    //The AI's speed per second
    public float speed = 200;
	public float rotationSpeed = 10;
    
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 0.1f;
 
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;
	
	private Animator animator;
	
	public GameObject nightclubDoorIn;
	public GameObject nightclubDoorOut;
	public GameObject cinemaDoorIn;
	public GameObject cinemaDoorOut;
	public GameObject apartmentsDoorIn;
	public GameObject apartmentsDoorOut;
	public GameObject universityDoorIn;
	public GameObject universityDoorOut;
	public GameObject gymDoorIn;
	public GameObject gymDoorOut;
	public GameObject mainCamera;
	
	private CameraBehaviour cameraBehaviour;
	private Mechanics mechanics;
	private CameraFade cameraFade;
	private Vector3 initCameraOffset;
	private Vector3 teleportCameraOffset;
	private Vector3 teleportPos;
	
	private PlayerEventQueue eventQueue;
	private PlayerEvent moveEvent;
	
	public GameObject statusText;
	private TextMesh statusTextMesh;
	private PlayerState playerState;
	public PlayerState GetPlayerState() {return playerState;}
	public void SetPlayerState(PlayerState playerState) {
		blobShadow.SetActive(true);
		this.playerState = playerState;
		if(playerState == PlayerState.Drinking)
			animator.SetBool("Using", true);
		else
			animator.SetBool("Using", false);
		if(playerState == PlayerState.Dancing)
			animator.SetBool("Dancing", true);
		else
			animator.SetBool("Dancing", false);
		if(playerState == PlayerState.RunningTreadmill)
			animator.SetFloat("Speed", 100);
		else
			animator.SetFloat("Speed", 0);
		if(playerState == PlayerState.Sleeping) {
			animator.SetBool("Sleeping", true);
			blobShadow.SetActive(false);
		}
		else
			animator.SetBool("Sleeping", false);
		if(playerState == PlayerState.Sitting) {
			animator.SetBool("Sitting", true);
			blobShadow.SetActive(false);
		}
		else
			animator.SetBool("Sitting", false);
	}
	private String statusTextDots;
	private float addNextDot;
	
	private int layerMask;
	public Camera minimapCamera;
	private float sittingOffset = 0;
	private float maxSittingOffset = 0.6f;
	public GameObject characterModel;
	public GameObject blobShadow;
 
    public void Start () {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
        seeker.pathCallback += OnPathComplete;
		
		animator = transform.Find("AnimatedCharacter").GetComponent<Animator>();
		cameraBehaviour = (CameraBehaviour)FindObjectOfType(typeof(CameraBehaviour));
		initCameraOffset = cameraBehaviour.cameraOffset;
		cameraFade = (CameraFade)FindObjectOfType(typeof(CameraFade));
		mechanics = (Mechanics)FindObjectOfType(typeof(Mechanics));
		statusTextMesh = statusText.GetComponent<TextMesh>();
		statusTextDots = "";
		addNextDot = Time.time+1.0f;
		
		targetPosition = transform.position;
		playerState = PlayerState.Standing;
		
		eventQueue = new PlayerEventQueue();	
		// ignore vehicles and trigger layer
		layerMask = 3 << (13);
		Debug.Log(layerMask);
		layerMask = ~layerMask;
		Debug.Log(layerMask);
    }
    
    public void OnPathComplete (Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }
	
	public void MoveAndActivate(Vector3 target, Action callback) {
		targetPosition = target;
		eventQueue.Clear();
		moveEvent = new PlayerEvent("move");
		eventQueue.Enqueue(moveEvent);
		PlayerEvent callbackEvent = new PlayerEvent("callback");
		callbackEvent.onStart += new PlayerEvent.OnStart(callback);
		eventQueue.Enqueue(callbackEvent);
		seeker.StartPath(transform.position,targetPosition);
		currentWaypoint = 0;
	}
	
	public void FaceTowards(Vector3 forward) {
		transform.LookAt(new Vector3(transform.position.x+forward.x, transform.position.y+forward.y, transform.position.z+forward.z));	
	}
 
    public void FixedUpdate () {
		eventQueue.Update();
		
		minimapCamera.transform.position = new Vector3(transform.position.x, minimapCamera.transform.position.y, transform.position.z);
		
		
		/// STATUS TEXT
		if(playerState == PlayerState.Drinking) {
			statusTextMesh.text = "Drinking"+statusTextDots;
		}
		else if(playerState == PlayerState.Dancing) {
			statusTextMesh.text = "Dancing"+statusTextDots;
		}
		else if(playerState == PlayerState.RunningTreadmill) {
			statusTextMesh.text = "Exercising"+statusTextDots;
		}
		else {
			statusTextMesh.text = "";
		}
		if(!statusTextMesh.text.Equals("")) {
			if(Time.time > addNextDot) {
				statusTextDots += ".";
				if(statusTextDots.Equals("...."))
					statusTextDots = "";
				addNextDot = Time.time+0.2f;
			}
			statusTextMesh.characterSize = cameraBehaviour.cameraOffset.y/42.0f;
			statusText.transform.LookAt(new Vector3(mainCamera.transform.position.x, statusText.transform.position.y, mainCamera.transform.position.z));
		}
		
		/// NAVIGATION
		if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0 && mechanics.GetGameState() == GameState.inGame && !mechanics.GetMouseOverGUI()) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
				if(hit.collider.gameObject.layer == 8) //ground layer
	            {
					targetPosition = ray.GetPoint(hit.distance);
					seeker.StartPath(transform.position,targetPosition);
					currentWaypoint = 0;
					eventQueue.Clear();
				}
				else if(hit.collider.gameObject.layer == 9)
				{
					bool move = false;
					if(hit.collider.gameObject.tag.Equals("Apartments")) {
						targetPosition = apartmentsDoorIn.transform.position;
						move = true;
						eventQueue.Clear();
						moveEvent = new PlayerEvent("move");
						eventQueue.Enqueue(moveEvent);
						PlayerEvent doorsEvent = new PlayerEvent("door");
						doorsEvent.onStart += new PlayerEvent.OnStart(ApartmentsDoorInActivated);
						eventQueue.Enqueue(doorsEvent);
					}
					else if(hit.collider.gameObject.tag.Equals("Cinema")) {
						targetPosition = cinemaDoorIn.transform.position;
						move = true;
						eventQueue.Clear();
						moveEvent = new PlayerEvent("move");
						eventQueue.Enqueue(moveEvent);
						PlayerEvent doorsEvent = new PlayerEvent("door");
						doorsEvent.onStart += new PlayerEvent.OnStart(CinemaDoorInActivated);
						eventQueue.Enqueue(doorsEvent);
					}
					else if(hit.collider.gameObject.tag.Equals("Gym")) {
						targetPosition = gymDoorIn.transform.position;
						move = true;
						eventQueue.Clear();
						moveEvent = new PlayerEvent("move");
						eventQueue.Enqueue(moveEvent);
						PlayerEvent doorsEvent = new PlayerEvent("door");
						doorsEvent.onStart += new PlayerEvent.OnStart(GymDoorInActivated);
						eventQueue.Enqueue(doorsEvent);
					}
					else if(hit.collider.gameObject.tag.Equals("Hospital")) {
						//targetPosition = new Vector3(-67.01894f, 0, -52.66562f);
					}
					else if(hit.collider.gameObject.tag.Equals("Nightclub")) {
						targetPosition = nightclubDoorIn.transform.position;
						move = true;
						eventQueue.Clear();
						moveEvent = new PlayerEvent("move");
						eventQueue.Enqueue(moveEvent);
						PlayerEvent doorsEvent = new PlayerEvent("door");
						doorsEvent.onStart += new PlayerEvent.OnStart(NightclubDoorInActivated);
						eventQueue.Enqueue(doorsEvent);
					}
					else if(hit.collider.gameObject.tag.Equals("University")) {
						targetPosition = universityDoorIn.transform.position;
						move = true;
						eventQueue.Clear();
						moveEvent = new PlayerEvent("move");
						eventQueue.Enqueue(moveEvent);
						PlayerEvent doorsEvent = new PlayerEvent("door");
						doorsEvent.onStart += new PlayerEvent.OnStart(UniversityDoorInActivated);
						eventQueue.Enqueue(doorsEvent);
					}
					else if(hit.collider.gameObject.name.Equals("cinemaOut")) {
						targetPosition = cinemaDoorOut.transform.position;
						move = true;
						eventQueue.Clear();
						moveEvent = new PlayerEvent("move");
						eventQueue.Enqueue(moveEvent);
						PlayerEvent doorsEvent = new PlayerEvent("door");
						doorsEvent.onStart += new PlayerEvent.OnStart(CinemaDoorOutActivated);
						eventQueue.Enqueue(doorsEvent);
					}
					else if(hit.collider.gameObject.name.Equals("nightclubOut")) {
						targetPosition = nightclubDoorOut.transform.position;
						move = true;
						eventQueue.Clear();
						moveEvent = new PlayerEvent("move");
						eventQueue.Enqueue(moveEvent);
						PlayerEvent doorsEvent = new PlayerEvent("door");
						doorsEvent.onStart += new PlayerEvent.OnStart(NightclubDoorOutActivated);
						eventQueue.Enqueue(doorsEvent);
					}
					else if(hit.collider.gameObject.name.Equals("apartmentsOut")) {
						targetPosition = apartmentsDoorOut.transform.position;
						move = true;
						eventQueue.Clear();
						moveEvent = new PlayerEvent("move");
						eventQueue.Enqueue(moveEvent);
						PlayerEvent doorsEvent = new PlayerEvent("door");
						doorsEvent.onStart += new PlayerEvent.OnStart(ApartmentsDoorOutActivated);
						eventQueue.Enqueue(doorsEvent);	
					}
					else if(hit.collider.gameObject.name.Equals("uniOut")) {
						targetPosition = universityDoorOut.transform.position;
						move = true;
						eventQueue.Clear();
						moveEvent = new PlayerEvent("move");
						eventQueue.Enqueue(moveEvent);
						PlayerEvent doorsEvent = new PlayerEvent("door");
						doorsEvent.onStart += new PlayerEvent.OnStart(UniversityDoorOutActivated);
						eventQueue.Enqueue(doorsEvent);	
					}
					else if(hit.collider.gameObject.name.Equals("gymOut")) {
						targetPosition = gymDoorOut.transform.position;
						move = true;
						eventQueue.Clear();
						moveEvent = new PlayerEvent("move");
						eventQueue.Enqueue(moveEvent);
						PlayerEvent doorsEvent = new PlayerEvent("door");
						doorsEvent.onStart += new PlayerEvent.OnStart(GymDoorOutActivated);
						eventQueue.Enqueue(doorsEvent);	
					}
					if(move) {
						seeker.StartPath(transform.position,targetPosition);
						currentWaypoint = 0;
					}
				}				
			}
		}		
		
        if (path != null) {
			MovePlayer();
        }
		
		if(playerState == PlayerState.Sitting) {
			if(sittingOffset < maxSittingOffset) {
				sittingOffset += Time.fixedTime*0.01f;
				if(sittingOffset > maxSittingOffset)
					sittingOffset = maxSittingOffset;
				//characterModel.transform.position = new Vector3(characterModel.transform.position.x, characterModel.transform.position.y, sittingOffset);
				characterModel.transform.Translate(0, 0, -Time.fixedTime*0.01f, Space.Self);
			}
		}
		else {
			if(sittingOffset > 0) {
				sittingOffset -= Time.fixedTime*0.01f;
				if(sittingOffset < 0)
					sittingOffset = 0;	
				//characterModel.transform.position = new Vector3(characterModel.transform.position.x, characterModel.transform.position.y, sittingOffset);
				characterModel.transform.Translate(0, 0, Time.fixedTime*0.01f, Space.Self);
			}
		}
    }
	
	public void UpdateMinimapCamera() {
		if(mechanics.GetGameState() == GameState.inGame || mechanics.GetGameState() == GameState.usingSomething) {
			minimapCamera.enabled = true;
		}
		else {
			minimapCamera.enabled = false;
		}	
	}
	
	public void TeleportPlayer(Vector3 pos, Vector3 cameraOffset, GamePlace gamePlace) {
		this.teleportPos = pos;
		this.teleportCameraOffset = cameraOffset;
		cameraFade.StartFade(Color.black, 1.0f, true);
		StartCoroutine(TeleportPlayerCoroutine(gamePlace));
	}
	
	public IEnumerator TeleportPlayerCoroutine(GamePlace gamePlace) {
		yield return new WaitForSeconds(0.5f);
		controller.enabled = false;
		transform.position = teleportPos;
		cameraBehaviour.cameraOffset = teleportCameraOffset;
		cameraBehaviour.UpdatePosition();
		controller.enabled = true;
		mechanics.SetGamePlacePosition(gamePlace);
	}
	
	public void NightclubDoorInActivated() {
		TeleportPlayer(nightclubDoorOut.transform.position, new Vector3(0, 10.5f, -5), GamePlace.Nightclub);
	}
	public void NightclubDoorOutActivated() {
		TeleportPlayer(nightclubDoorIn.transform.position, initCameraOffset, GamePlace.Street);
	}
	public void ApartmentsDoorInActivated() {
		TeleportPlayer(apartmentsDoorOut.transform.position, new Vector3(0, 7.37f, -3.75f), GamePlace.Apartments);
	}
	public void ApartmentsDoorOutActivated() {
		TeleportPlayer(apartmentsDoorIn.transform.position, initCameraOffset, GamePlace.Street);
	}
	public void CinemaDoorInActivated() {
		TeleportPlayer(cinemaDoorOut.transform.position, new Vector3(0, 10.5f, -5), GamePlace.Cinema);
	}
	public void CinemaDoorOutActivated() {
		TeleportPlayer(cinemaDoorIn.transform.position, initCameraOffset, GamePlace.Street);
	}
	public void UniversityDoorInActivated() {
		TeleportPlayer(universityDoorOut.transform.position, new Vector3(0, 15.75f, -7.5f), GamePlace.University);
	}
	public void UniversityDoorOutActivated() {
		TeleportPlayer(universityDoorIn.transform.position, initCameraOffset, GamePlace.Street);
	}
	public void GymDoorInActivated() {
		TeleportPlayer(gymDoorOut.transform.position, new Vector3(0, 15.75f, -7.5f), GamePlace.Gym);
	}
	public void GymDoorOutActivated() {
		TeleportPlayer(gymDoorIn.transform.position, initCameraOffset, GamePlace.Street);
	}
	
	public void MovePlayer () {
		if (currentWaypoint >= path.vectorPath.Count && currentWaypoint != 0) {
			if (Vector3.Distance(transform.position, targetPosition) > 0.1) {
				Vector3 dir = (targetPosition-transform.position);
		        dir *= speed * Time.fixedDeltaTime;
		        controller.SimpleMove(dir);
				Quaternion origRotation = transform.rotation;
				transform.LookAt(new Vector3(transform.position.x+dir.x, transform.position.y, transform.position.z+dir.z));
				transform.rotation = Quaternion.Slerp(origRotation, transform.rotation, rotationSpeed * Time.fixedDeltaTime);
				animator.SetFloat("Speed", dir.sqrMagnitude);
			}
			else {
				animator.SetFloat("Speed", 0f);
				if(moveEvent != null)
					moveEvent.finished = true;
				path = null;
            	return;
			}
        }
		else {
	        //Direction to the next waypoint
	        Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
	        dir *= speed * Time.fixedDeltaTime;
	        controller.SimpleMove(dir);
			Quaternion origRotation = transform.rotation;
			transform.LookAt(new Vector3(transform.position.x+dir.x, transform.position.y, transform.position.z+dir.z));
			transform.rotation = Quaternion.Slerp(origRotation, transform.rotation, rotationSpeed * Time.fixedDeltaTime);
			animator.SetFloat("Speed", dir.sqrMagnitude);
	        
	        //Check if we are close enough to the next waypoint
	        //If we are, proceed to follow the next waypoint
	        if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
	            currentWaypoint++;
	            return;
	        }
		}
	}
	
	public void OnDisable () {
	    seeker.pathCallback -= OnPathComplete;
	} 
}

public class PlayerEvent {
	public string name;
	public bool finished;
	
	public delegate void OnStart();
	public event OnStart onStart;
	public delegate void OnProcess();
	public event OnProcess onProcess;
	public delegate void OnFinish();
	public event OnFinish onFinish;
	
	public PlayerEvent(string name) {
		this.name = name;
		finished = false;
	}
	
	public void Start() {
		if(onStart != null)
			onStart();
	}	
	
	public void Process() {
		if(onProcess != null)
			onProcess();
	}	
	
	public void Finish() {
		if(onProcess != null)
			onProcess();	
	}
}

public class PlayerEventQueue {
	private Queue<PlayerEvent> queue;
	private PlayerEvent currentEvent;
	
	public PlayerEventQueue() {
		currentEvent = null;
		queue = new Queue<PlayerEvent>();	
	}
	
	public void Enqueue(PlayerEvent anEvent)
	{
		queue.Enqueue(anEvent);	
	}
	
	public void Update() {
		if(currentEvent == null)
		{
			if(queue.Count > 0)
			{
				currentEvent = queue.Dequeue();	
				currentEvent.Start();
			}
		}
		else
		{
			if(currentEvent.finished)
			{
				currentEvent.Finish();
				currentEvent = null;
			}
			else
			{	
				currentEvent.Process();	
			}
		}
	}
	
	public void Clear()
	{
		currentEvent = null;
		queue.Clear();	
	}
}