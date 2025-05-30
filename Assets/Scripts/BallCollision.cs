using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli

public class basadun : MonoBehaviour
{
    private GameObject to; // Top referansı

    void Start()
    {
        // "to" isimli topu bul
        to = GameObject.Find("to");
        
        if (to == null)
        {
            Debug.LogWarning("'to' isimli top bulunamadı! Lütfen topun isminin 'to' olduğundan emin olun.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Eğer çarpışan nesne "to" ise ve diğer nesne "Engel" tag'ine sahipse
        if (collision.gameObject == to && collision.other.CompareTag("Engel"))
        {
            Debug.Log("Top engele çarptı! Oyun yeniden başlatılıyor...");
            // Mevcut sahneyi yeniden yükle
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Alternatif olarak trigger kullanımı için
    void OnTriggerEnter(Collider other)
    {
        // Eğer tetikleyen nesne "to" ise ve diğer nesne "Engel" tag'ine sahipse
        if (other.gameObject == to && other.CompareTag("Engel"))
        {
            Debug.Log("Top engele çarptı! Oyun yeniden başlatılıyor...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
} 