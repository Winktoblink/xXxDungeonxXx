using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

    public Sprite dmgSprite;
    public int hp = 4;

    public AudioClip chopSound1;
    public AudioClip chopSound2;

    private SpriteRenderer mSpriteRenderer;

	void Awake () {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	public void damageWall(int loss) {
        SoundManager.INSTANCE.randomizeSfx(chopSound1, chopSound2);
        mSpriteRenderer.sprite = dmgSprite;
        hp -= loss;

        if(hp <= 0) {
            gameObject.SetActive(false);
        }
    }
}
