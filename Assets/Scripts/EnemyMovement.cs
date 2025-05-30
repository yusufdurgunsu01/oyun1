using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager için gerekli

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;          // Hareket hızı
    public float distance = 5f;       // Z ekseninde ileri geri ne kadar gideceği

    private Vector3 startPos;         // Başlangıç pozisyonu

    void Start()
    {
        startPos = transform.position;
        Debug.Log("Enemy başlatıldı. Başlangıç pozisyonu: " + startPos);
    }

    void Update()
    {
        // Sinüs fonksiyonu ile ileri geri z hareketi oluştur
        float zMovement = Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(startPos.x, startPos.y, startPos.z + zMovement);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Çarpışma tespit edildi! Çarpan nesne: " + collision.gameObject.name);
        
        // Eğer çarpışan nesne Player ise
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player ile çarpışma tespit edildi! Oyun yeniden başlatılıyor...");
            // Oyunu yeniden başlat
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Alternatif çarpışma kontrolü
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger tespit edildi! Tetikleyen nesne: " + other.gameObject.name);
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player trigger tespit edildi! Oyun yeniden başlatılıyor...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
} 