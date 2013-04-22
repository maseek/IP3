using UnityEngine;
using System.Collections;

public class StressItem : MonoBehaviour {
	
	private GameObject player;
	private Player playerScript;
	private Mechanics mechanics;
	private CameraFade cameraFade;
	
	public GameObject targetPlace;
	public int stressAdj = 0;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		playerScript = (Player)FindObjectOfType(typeof(Player));
		mechanics = (Mechanics)FindObjectOfType(typeof(Mechanics));
		cameraFade = (CameraFade)FindObjectOfType(typeof(CameraFade));		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown() {
		if(mechanics.GetGameState() == GameState.inGame) {
			if(targetPlace.gameObject.name.Equals("CinemaSeatTarget")) {
				playerScript.MoveAndActivate(targetPlace.transform.position, CinemaSeatCallback);
			}
			else if(targetPlace.gameObject.name.Equals("NightclubBarTarget")) {
				playerScript.MoveAndActivate(targetPlace.transform.position, NightclubBarCallback);
			}
			else if(targetPlace.gameObject.name.Equals("NightclubDanceFloorTarget")) {
				playerScript.MoveAndActivate(targetPlace.transform.position, NightclubDanceFloorCallback);
			}
			else if(targetPlace.gameObject.name.Equals("classDoorInPosition")) {
				playerScript.MoveAndActivate(targetPlace.transform.position, ClassCallback);
			}
			else if(targetPlace.gameObject.name.Equals("BedTarget")) {
				playerScript.MoveAndActivate(targetPlace.transform.position, BedCallback);
			}
			else if(targetPlace.gameObject.name.Equals("treadmillTarget")) {
				playerScript.MoveAndActivate(targetPlace.transform.position, TreadmillCallback);
			}
		}
	}
	
	public void CinemaSeatCallback() {
		playerScript.FaceTowards(Vector3.forward);
		playerScript.SetPlayerState(PlayerState.Sitting);
		mechanics.SetGameState(GameState.usingSomething);
		StartCoroutine(CinemaSeatCallbackCoroutine());
	}
	
	private IEnumerator CinemaSeatCallbackCoroutine() {
		yield return new WaitForSeconds(1.5f);
		cameraFade.StartFade(Color.black, 3.0f, true);
		yield return new WaitForSeconds(1.5f);
		mechanics.stressMeter.AdjustValue(stressAdj);
		mechanics.RollForwardInTime(UnityEngine.Random.Range(90.0f, 180.0f));
		mechanics.SetBankBalance(mechanics.GetBankBalance()-10.00f);
		mechanics.SetGameState(GameState.inGame);
		playerScript.SetPlayerState(PlayerState.Standing);
	}
	
	public void NightclubBarCallback() {
		mechanics.SetGameState(GameState.usingSomething);
		playerScript.SetPlayerState(PlayerState.Drinking);
		mechanics.stressMeter.AdjustValue(stressAdj);
		mechanics.SetBankBalance(mechanics.GetBankBalance()-3.0f);
		StartCoroutine(NightclubBarCallbackCoroutine());
	}
	
	private IEnumerator NightclubBarCallbackCoroutine() {
		yield return new WaitForSeconds(5.0f);
		mechanics.SetGameState(GameState.inGame);
		playerScript.SetPlayerState(PlayerState.Standing);
	}
	
	public void NightclubDanceFloorCallback() {
		mechanics.SetGameState(GameState.usingSomething);
		playerScript.SetPlayerState(PlayerState.Dancing);
		mechanics.stressMeter.AdjustValue(stressAdj);
		mechanics.energyMeter.AdjustValue(-10);
		StartCoroutine(NightclubDanceFloorCallbackCoroutine());
	}
	
	private IEnumerator NightclubDanceFloorCallbackCoroutine() {
		yield return new WaitForSeconds(5.0f);
		mechanics.SetGameState(GameState.inGame);
		playerScript.SetPlayerState(PlayerState.Standing);
	}
	
	public void ClassCallback() {
		if(mechanics.IsAnyClassOn()) {
			cameraFade.StartFade(Color.black, 3.0f, true);
			mechanics.SetGameState(GameState.usingSomething);
			StartCoroutine(ClassCallbackCoroutine());
		}
	}
	
	private IEnumerator ClassCallbackCoroutine() {
		yield return new WaitForSeconds(1.5f);
		mechanics.stressMeter.AdjustValue(stressAdj);
		mechanics.RollForwardInTime(mechanics.HowManyMinutesInClass());
		mechanics.SetGameState(GameState.inGame);
	}
	
	public void BedCallback() {
		mechanics.SetGameState(GameState.usingSomething);
		playerScript.FaceTowards(Vector3.forward);
		playerScript.SetPlayerState(PlayerState.Sleeping);
		StartCoroutine(BedCallbackCoroutine());
	}
	
	private IEnumerator BedCallbackCoroutine() {
		yield return new WaitForSeconds(1.5f);
		cameraFade.StartFade(Color.black, 3.0f, true);
		yield return new WaitForSeconds(1.5f);
		mechanics.stressMeter.AdjustValue(stressAdj);
		mechanics.energyMeter.AdjustValue(50);
		mechanics.RollForwardInTime(450);
		mechanics.SetGameState(GameState.inGame);
		playerScript.SetPlayerState(PlayerState.Standing);
	}
	
	public void TreadmillCallback() {
		mechanics.SetGameState(GameState.usingSomething);
		playerScript.SetPlayerState(PlayerState.RunningTreadmill);
		StartCoroutine(TreadmillCallbackCoroutine());
	}
	
	private IEnumerator TreadmillCallbackCoroutine() {
		yield return new WaitForSeconds(5.0f);
		mechanics.stressMeter.AdjustValue(stressAdj);
		mechanics.energyMeter.AdjustValue(-10);
		mechanics.SetGameState(GameState.inGame);
		playerScript.SetPlayerState(PlayerState.Standing);
	}
}
