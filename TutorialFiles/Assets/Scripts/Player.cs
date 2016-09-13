using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class Player : MovingObject {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    public Text foodText;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    private Animator mAnimator;
    private int mFood;
    private Vector2 mTouchOrigin = -Vector2.one;

	// Use this for initialization
	protected override void Start () {
        mAnimator = GetComponent<Animator>();
        mFood = GameManager.INSTANCE.playerFoodPoints;

        foodText.text = "Food: " + mFood;

        base.Start();
	}

    private void OnDisable() {
        GameManager.INSTANCE.playerFoodPoints = mFood;
    }


    private void Restart() {
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       //Application.LoadLevel(Application.loadedLevel);
    }

    // Update is called once per frame
    void Update () {

        if(!GameManager.INSTANCE.playerTurn) {
            return;
        }

        //Used to store the direction we are moving.
        int horizontal = 0;
        int vertical = 0;

    #if UNITY_STANDALONE || UNITY_WEBPLAYER

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        //Prevents diagonal movement
        if(horizontal != 0) {
            vertical = 0;
        }
    #else 
        if(Input.touchCount > 0) {

            Touch myTouch = Input.touches[0];

            if(myTouch.phase == TouchPhase.Began) {
                mTouchOrigin = myTouch.position;
            } else if(myTouch.phase == TouchPhase.Ended && mTouchOrigin.x >= 0) {
                Vector2 touchEnd = myTouch.position;
                float x = touchEnd.x - mTouchOrigin.x;
                float y = touchEnd.y - mTouchOrigin.y;
                mTouchOrigin.x = -1;

                if(Mathf.Abs(x) > Mathf.Abs(y)) {
                    horizontal = x > 0 ? 1 : -1;
                } else {
                    vertical = y > 0 ? 1 : -1;
                }
            }
        }

    #endif

        //If you are moving in a direction, attempt to move there by passing the direction values.
        if(horizontal != 0 || vertical != 0) {
            attemptMove<Wall>(horizontal, vertical);
        }
	}

    protected override void attemptMove<T>(int xDir, int yDir) {
        mFood--;
        foodText.text = "Food: " + mFood;

        base.attemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        if(Move(xDir, yDir, out hit)) {
            SoundManager.INSTANCE.randomizeSfx(moveSound1, moveSound2);
        }

        checkIfGameOver();
        GameManager.INSTANCE.playerTurn = false;
    }

    //when you can't move to a square, handle it based on what it is
    protected override void onCantMove<T>(T component) {
        Wall hitWall = component as Wall;
        hitWall.damageWall(wallDamage);
        mAnimator.SetTrigger("playerChop");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Exit") {   //Start the next level
            Invoke("Restart", restartLevelDelay);
            enabled = false;

        } else if(other.tag == "Food") {    //Consume food when the player collides with it
            mFood += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + mFood;
            SoundManager.INSTANCE.randomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);

        } else if(other.tag == "Soda") {    //Consume soda when the player collides with it
            mFood += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + mFood;
            SoundManager.INSTANCE.randomizeSfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);

        }
    }

    //Trigger hit animation, subtract food points, and check game status
    public void loseFood(int loss) {
        mAnimator.SetTrigger("playerHit");
        mFood -= loss;
        foodText.text = "+" + loss + " Food: " + mFood;
        checkIfGameOver();
    }

    //If you have no food points, the game ends
    private void checkIfGameOver() {
        if(mFood <= 0) {
            SoundManager.INSTANCE.playSingle(gameOverSound);
            SoundManager.INSTANCE.musicSource.Stop();
            GameManager.INSTANCE.gameOver();
        }
    }
}
