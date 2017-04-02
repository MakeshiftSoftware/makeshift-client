using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    private const string RIGHT  = "right";
    private const string LEFT   = "left";
    private const string UP     = "up";
    private const string DOWN   = "down";
    private const float scrollSpeed     = 50.0f;
    private const float damping         = 7.0f;
    private const float leftBoundary    = 0.0f;
    private const float rightBoundary   = 100.0f;
    private const float topBoundary     = 80.0f;
    private const float bottomBoundary  = -8.0f;
    private Transform _transform;
    private Transform follow;
    private Vector3 upDir       = scrollSpeed * Vector3.forward;
    private Vector3 downDir     = scrollSpeed * Vector3.back;
    private Vector3 rightDir    = scrollSpeed * Vector3.right;
    private Vector3 leftDir     = scrollSpeed * Vector3.left;

    void Awake()
    {
        this._transform = transform;
        this.follow = new GameObject("Follow").transform;
        this.follow.position = transform.position;
    }

    void  Update ()
    {
        this._transform.position = Vector3.Lerp(
            transform.position, this.follow.position, damping * Time.deltaTime);

        if (Input.GetKey(RIGHT) || Input.GetKey(KeyCode.D))
        {
            if (this.follow.position.x < rightBoundary)
            {
                this.follow.Translate(this.rightDir * Time.deltaTime, Space.World);
            }
        }

        if (Input.GetKey(LEFT) || Input.GetKey(KeyCode.A))
        {
            if (this.follow.position.x > leftBoundary)
            {
                this.follow.Translate(this.leftDir * Time.deltaTime, Space.World);
            }
        }

        if (Input.GetKey(UP) || Input.GetKey(KeyCode.W))
        {
            if (this.follow.position.z < topBoundary)
            {
                this.follow.Translate(this.upDir * Time.deltaTime, Space.World);
            }
        }

        if (Input.GetKey(DOWN) || Input.GetKey(KeyCode.S))
        {
            if (this.follow.position.z > bottomBoundary)
            {
                this.follow.Translate(this.downDir * Time.deltaTime, Space.World);
            }
        }
    }
}
