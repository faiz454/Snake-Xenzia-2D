using UnityEngine;

public class DeactivePicking : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("Deactivate", Random.Range(3f, 6f)); // Deactivate the game object after a random time between 3 and 6 seconds
    }

    // Update is called once per frame
    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
