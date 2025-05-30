using UnityEngine;
using UnityEngine.SceneManagement;

public class mavienegel : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the ball (player)
        if (collision.gameObject.CompareTag("Player"))
        {
            // Restart the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
} 