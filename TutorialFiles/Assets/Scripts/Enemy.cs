using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObject {

    public int playerDamage;

    private Animator mAnimator;
    private Transform mTarget;
    private bool mSkipMove;

    public AudioClip attackSound1;
    public AudioClip attackSound2;

	// Use this for initialization
	protected override void Start () {
        GameManager.INSTANCE.addEnemyToList(this);
        mAnimator = GetComponent<Animator>();
        mTarget = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}

    protected override void attemptMove<T>(int xDir, int yDir) {
        if(mSkipMove) {
            mSkipMove = false;
            return;
        }

        base.attemptMove<T>(xDir, yDir);

        mSkipMove = true;
    }

    //Logic for enemy movement.
    public void moveEnemy() {
        int xDir = 0;
        int yDir = 0;

        if(Mathf.Abs(mTarget.position.x - transform.position.x) < float.Epsilon) {
            yDir = mTarget.position.y > transform.position.y ? 1 : -1;
        } else {
            xDir = mTarget.position.x > transform.position.x ? 1 : -1;
        }

        attemptMove<Player>(xDir, yDir);
    }

    //Trigger an attack when the enemy tries to move into the player
    protected override void onCantMove<T>(T component) {
        Player hitPlayer = component as Player;
        hitPlayer.loseFood(playerDamage);

        mAnimator.SetTrigger("enemyAttack");
        SoundManager.INSTANCE.randomizeSfx(attackSound1, attackSound2);
    }
}
