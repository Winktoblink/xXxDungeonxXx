using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager INSTANCE = null;
    public BoardManager boardScript;
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playerTurn = true;

    private Text mLevelText;
    private GameObject mLevelImage;
    private int mLevel = 1;
    private List<Enemy> mEnemies;
    private bool mEnemiesMoving;
    private bool mDoingSetup;

	// References between scripts and initialization
	void Awake () {
        Debug.Log("I awake...");
        //Make this object a singleton & make sure no other instances exist.
        if(INSTANCE == null) {
            INSTANCE = this;
        } else if(INSTANCE != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        mEnemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        initGame();
	}

    //Used to load the next level
    private void OnLevelWasLoaded(int index) {
        mLevel++;
        initGame();
        Debug.Log("LOAD BITCHES");
    }

    void initGame() {
        mDoingSetup = true;
        mLevelImage = GameObject.Find("LevelImage");
        mLevelText = GameObject.Find("LevelText").GetComponent<Text>();
        mLevelText.text = "Day " + mLevel;

        mLevelImage.SetActive(true);
        Invoke("hideLevelImage", levelStartDelay);

        mEnemies.Clear();
        boardScript.setupScene(mLevel);
    }

    private void hideLevelImage() {
        Debug.Log("WTF");
        mLevelImage.SetActive(false);
        mDoingSetup = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if(playerTurn || mEnemiesMoving || mDoingSetup) {
            return;
        }
        StartCoroutine(moveEnemies());
	}

    public void addEnemyToList(Enemy script) {
        mEnemies.Add(script);
    }

    public void gameOver() {
        mLevelText.text = "After " + mLevel + " days, you starved.";
        mLevelImage.SetActive(true);
        enabled = false;
    }

    //Delay for .1 seconds on changing turns
    IEnumerator moveEnemies() {
        mEnemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if(mEnemies.Count == 0) {
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i = 0; i < mEnemies.Count; i++) {
            mEnemies[i].moveEnemy();
            yield return new WaitForSeconds(mEnemies[i].moveTime);
        }

        playerTurn = true;
        mEnemiesMoving = false;
    }
}
