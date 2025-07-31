using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCatcher : MonoBehaviour
{
    internal float lifetime;
    internal float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < lifetime)
        {
            timer += Time.deltaTime;
            Color c = GetComponent<MeshRenderer>().material.color;
            GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, (lifetime - timer) / lifetime); // Fade out the net
            //TODO ustawiæ wygaszanie sieci
        }
        else
        {
            Destroy(gameObject);
        }

        if (timer > 0.1f)
        {
            GetComponent<PolygonCollider2D>().enabled = false; // Disable the collider after 0.1 seconds
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            FishScript fish = collision.gameObject.GetComponent<FishScript>();
            if (fish != null)
            {
                fish.Catch();
                Destroy(collision.gameObject); // Destroy the fish object
            }
        }
    }
}
