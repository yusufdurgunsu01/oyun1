using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Transform ball;
    private Vector3 startMousePos, startBallPos;
    private bool moveTheBall;
    private bool isDragging;

    [Range(0f, 1f)] public float maxSpeed = 0.3f;
    [Range(0f, 1f)] public float camSpeed = 0.5f;
    [Range(0f, 50f)] public float pathSpeed = 9.4f;
    [Range(0f, 20f)] public float jumpForce = 8f;

    private float velocity;
    private float camVelocityX;
    private float camVelocityZ;

    private Camera mainCam;
    public Transform path;
    private Rigidbody rb;
    private Collider _collider;

    private Vector3 previousMousePos;
    private bool canJump;

    void Start()
    {
        ball = transform;
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        canJump = true;
        isDragging = false;
    }

    void Update()
    {
        // Fare tıklama başlangıcı
        if (Input.GetMouseButtonDown(0) && MenuManager.MenuManagerInstance.GameState)
        {
            isDragging = true;
            moveTheBall = true;
            previousMousePos = Input.mousePosition;

            Plane newPlane = new Plane(Vector3.up, 0f);
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (newPlane.Raycast(ray, out float distance))
            {
                startMousePos = ray.GetPoint(distance);
                startBallPos = ball.position;
            }
        }
        // Fare bırakıldığında
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            moveTheBall = false;

            // Eğer oyun aktifse ve zıplayabilir durumdaysa zıpla
            if (MenuManager.MenuManagerInstance.GameState && canJump)
            {
                Jump();
            }
        }

        // Topu hareket ettirme
        if (moveTheBall && isDragging)
        {
            Plane newPlane = new Plane(Vector3.up, 0f);
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (newPlane.Raycast(ray, out float distance))
            {
                Vector3 mouseNewPos = ray.GetPoint(distance);
                Vector3 delta = mouseNewPos - startMousePos;
                Vector3 desiredBallPos = startBallPos + delta;

                desiredBallPos.x = Mathf.Clamp(desiredBallPos.x, -1.5f, 1.5f);
                float smoothX = Mathf.SmoothDamp(ball.position.x, desiredBallPos.x, ref velocity, maxSpeed);
                ball.position = new Vector3(smoothX, ball.position.y, ball.position.z);
            }
        }

        // Yolu hareket ettirme
        if (MenuManager.MenuManagerInstance.GameState)
        {
            Vector3 pathNewPos = path.position;
            path.position = new Vector3(
                pathNewPos.x,
                pathNewPos.y,
                Mathf.MoveTowards(pathNewPos.z, -1000f, pathSpeed * Time.deltaTime)
            );
        }
    }

    void Jump()
    {
        rb.isKinematic = false;
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        canJump = false;
    }

    void LateUpdate()
    {
        Vector3 camPos = mainCam.transform.position;
        float hedefX = ball.position.x;
        float hedefZ = ball.position.z - 7f;

        float smoothCamX = Mathf.SmoothDamp(camPos.x, hedefX, ref camVelocityX, camSpeed);
        float smoothCamZ = Mathf.SmoothDamp(camPos.z, hedefZ, ref camVelocityZ, camSpeed);

        mainCam.transform.position = new Vector3(smoothCamX, camPos.y, smoothCamZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstacle"))
        {
            gameObject.SetActive(false);
            MenuManager.MenuManagerInstance.GameState = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("paths"))
        {
            rb.isKinematic = false;
            _collider.isTrigger = false;
            rb.velocity = new Vector3(0f, jumpForce, 0f);
            pathSpeed *= 2;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("paths"))
        {
            rb.isKinematic = true;
            _collider.isTrigger = true;
            rb.velocity = Vector3.zero;
            pathSpeed = 30;
            canJump = true;
        }
    }
}
