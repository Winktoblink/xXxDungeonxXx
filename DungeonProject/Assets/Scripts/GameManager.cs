using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager INSTANCE = null;
	private BoardManager mBoardScript;

	// Use this for initialization
	void Awake() {
		if (INSTANCE == null) {
			INSTANCE = this;
		} else if (INSTANCE != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		mBoardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	void InitGame(){
		//TODO: Implement a level system that is managed here and passes through to the board manager.
		mBoardScript.SetupScene (0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
