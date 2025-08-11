using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] protected float lifetime = 2f; // Lifetime of the notification in seconds
    protected float timer = 0f; // Duration of the fade effect in seconds
    [SerializeField]  protected float verticalVelocity = 0.5f;
    [SerializeField] private float notificationScale = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize * notificationScale, GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize * notificationScale, 1); // Set the scale of the notification
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - (timer / lifetime) * (timer / lifetime));
        timer += Time.deltaTime;

        transform.position += new Vector3(0, verticalVelocity * Time.deltaTime, 0); // Move the notification upwards

        if (timer >= lifetime)
        {
            Destroy(gameObject); // Destroy the notification after its lifetime
        }
    }
}
