using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 5f;        // Normal hareket hızı
    public float jumpForce = 7f;        // Zıplama kuvveti
    public float stopJumpForce = 5f;    // Durduğunda zıplama kuvveti
    public float spaceJumpForce = 10f;  // Boşluk tuşu ile zıplama kuvveti

    [Header("Ses Ayarları")]
    public AudioClip collisionSound;    // Çarpışma sesi
    public AudioClip backgroundMusic;   // Arka plan müziği
    [Range(0f, 1f)]
    public float collisionVolume = 0.5f;  // Çarpışma sesi seviyesi
    [Range(0f, 1f)]
    public float musicVolume = 0.3f;      // Müzik sesi seviyesi

    private Rigidbody rb;
    private bool isGrounded;
    private float lastJumpTime;
    private float jumpCooldown = 0.1f;  // Zıplamalar arası bekleme süresi
    private Vector3 lastVelocity;       // Son hız
    private AudioSource collisionAudio; // Çarpışma sesi için AudioSource
    private AudioSource musicAudio;     // Müzik için AudioSource

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastJumpTime = -jumpCooldown;
        lastVelocity = Vector3.zero;

        // Çarpışma sesi için AudioSource ekle
        collisionAudio = gameObject.AddComponent<AudioSource>();
        collisionAudio.clip = collisionSound;
        collisionAudio.volume = collisionVolume;
        collisionAudio.playOnAwake = false;

        // Müzik için AudioSource ekle
        musicAudio = gameObject.AddComponent<AudioSource>();
        musicAudio.clip = backgroundMusic;
        musicAudio.volume = musicVolume;
        musicAudio.loop = true;  // Müziği sürekli tekrarla
        musicAudio.playOnAwake = true;
        musicAudio.Play();  // Müziği başlat

        // Topun ismini kontrol et
        if (gameObject.name != "to")
        {
            Debug.LogWarning("Bu script 'to' isimli top için tasarlanmıştır. Lütfen topun ismini 'to' olarak değiştirin.");
        }
    }

    void Update()
    {
        // Yere temas kontrolü
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // Boşluk tuşuna basıldığında zıplama (öncelikli)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump(spaceJumpForce);  // Boşluk tuşu için daha yüksek zıplama kuvveti
            lastJumpTime = Time.time;
            return; // Diğer hareketleri kontrol etme
        }

        // Hareket kontrolü
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.forward;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.back;
        if (Input.GetKey(KeyCode.Z)) direction += Vector3.right;
        if (Input.GetKey(KeyCode.W)) direction += Vector3.left;

        // Hareketi uygula
        if (direction != Vector3.zero)
        {
            direction = direction.normalized;
            rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);

            // Tuşa basıldığında zıplama
            if (isGrounded && Time.time >= lastJumpTime + jumpCooldown)
            {
                Jump(jumpForce);
                lastJumpTime = Time.time;
            }
        }
        else
        {
            // Top durduğunda zıplama
            if (isGrounded &&
                Mathf.Abs(rb.velocity.x) < 0.1f &&
                Mathf.Abs(rb.velocity.z) < 0.1f &&
                Time.time >= lastJumpTime + jumpCooldown)
            {
                Jump(stopJumpForce);
                lastJumpTime = Time.time;
            }

            // Yatay hızı sıfırla ama dikey hızı koru
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        lastVelocity = rb.velocity;
    }

    void Jump(float force)
    {
        // Mevcut dikey hızı sıfırla ve yeni zıplama kuvvetini uygula
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Çarpışma sesini çal
        if (collisionSound != null)
        {
            collisionAudio.Play();
        }

        // Eğer çarpışan nesne "Engel" tag'ine sahipse
        if (collision.gameObject.CompareTag("Engel"))
        {
            Debug.Log("Top engele çarptı! Oyun yeniden başlatılıyor...");
            // Mevcut sahneyi yeniden yükle
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Eğer tetikleyen nesne "Engel" tag'ine sahipse
        if (other.CompareTag("Engel"))
        {
            Debug.Log("Top engele çarptı! Oyun yeniden başlatılıyor...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Gizmos ile yere temas kontrolünü görselleştir
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1.1f);
    }
} 