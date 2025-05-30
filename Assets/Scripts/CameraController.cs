using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Kamera Ayarları")]
    public Transform to;              // Takip edilecek top
    public float smoothSpeed = 0.125f;  // Kamera yumuşaklık değeri
    public Vector3 offset;              // Kameranın topa olan uzaklığı

    void Start()
    {
        // Başlangıçta kameranın topa olan uzaklığını kaydet
        if (to != null)
        {
            offset = transform.position - to.position;
        }
        else
        {
            Debug.LogWarning("to referansı atanmamış! Lütfen Inspector'dan top nesnesini atayın.");
        }
    }

    void LateUpdate()
    {
        if (to != null)
        {
            // Hedef pozisyonu hesapla
            Vector3 desiredPosition = to.position + offset;

            // Yumuşak geçişli kamera hareketi
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Kameranın topa bakmasını sağla
            transform.LookAt(to);
        }
    }
} 