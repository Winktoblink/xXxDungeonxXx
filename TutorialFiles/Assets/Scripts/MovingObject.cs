using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D mBoxCollider;
    private Rigidbody2D mRigidbody2d;
    private float mInverseMoveTime;

	// Use this for initialization
	protected virtual void Start () {
        mBoxCollider = GetComponent<BoxCollider2D>();
        mRigidbody2d = GetComponent<Rigidbody2D>();
        mInverseMoveTime = 1f / moveTime;
    }

    protected IEnumerator smoothMovement(Vector3 end) {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while(sqrRemainingDistance > float.Epsilon) {
            Vector3 newPosition = Vector3.MoveTowards(mRigidbody2d.position, end, mInverseMoveTime * Time.deltaTime);
            mRigidbody2d.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit) {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        mBoxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        mBoxCollider.enabled = true;

        if(hit.transform == null) {
            StartCoroutine(smoothMovement(end));
            return true;
        }
        return false;
    }

    protected virtual void attemptMove<T>(int xDir, int yDir)
        where T : Component{

        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if(hit.transform == null) {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();
        if(!canMove && hitComponent != null) {
            onCantMove(hitComponent);
        }
    }

    protected abstract void onCantMove<T>(T component)
        where T : Component;
}
