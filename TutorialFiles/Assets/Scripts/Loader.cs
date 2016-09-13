using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject gameManager;

	//Only instantiate a game manager if one hasn't been loaded already.
	void Awake () {
	    if(GameManager.INSTANCE == null) {
            Instantiate(gameManager);
        }
	}
}
