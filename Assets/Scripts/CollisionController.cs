using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionController : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
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
} 