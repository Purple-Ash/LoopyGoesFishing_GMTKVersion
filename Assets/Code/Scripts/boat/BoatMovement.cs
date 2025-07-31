using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeedForward = 5f; // Speed of the boat
    [SerializeField] private float maxSpeedBackward = 2f; // Speed of the boat when moving backward
    [SerializeField] private float acceleration = 2f; // Acceleration of the boat
    [SerializeField] private float deceleration = 1f; // Deceleration of the boat
    [SerializeField] private float turnSpeed = 100f; // Turn speed of the boat
    [SerializeField] private float topTurnSpeed = 1f; // Maximum angle the boat can turn
    protected Rigidbody2D rb; // Rigidbody component for physics interactions
    protected bool collisioned = false; // Flag to check if the boat has collided with something
    [Header("Net Settings")]
    [SerializeField] protected Material closedNet;
    [SerializeField] protected Texture closeNet;
    [SerializeField] protected float netTime = 2f;
    protected List<float> emissions = new List<float>();


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        foreach (var emission in GetComponentsInChildren<ParticleSystem>())
        {
            emissions.Add(emission.emission.rateOverTime.constant);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // Move the boat forward
            rb.velocity = Rotate(Vector2.up, transform.eulerAngles.z * Mathf.Deg2Rad) * acceleration * Time.deltaTime + rb.velocity;
            // Clamp the speed to MaxSpeed
            if (rb.velocity.magnitude > maxSpeedForward)
            {
                rb.velocity = rb.velocity.normalized * maxSpeedForward;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            // Move the boat backward
            rb.velocity = Rotate(Vector2.down, transform.eulerAngles.z * Mathf.Deg2Rad) * deceleration * Time.deltaTime + rb.velocity;
            // Clamp the speed to MaxSpeedBackward
            if (rb.velocity.magnitude > maxSpeedBackward)
            {
                rb.velocity = rb.velocity.normalized * maxSpeedBackward;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            // Turn the boat left
            transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime * GetTurnCoefficient(rb.velocity.magnitude));
            rb.velocity = Rotate(rb.velocity, turnSpeed * Time.deltaTime * GetTurnCoefficient(rb.velocity.magnitude) * Mathf.Deg2Rad);
        }

        if (Input.GetKey(KeyCode.D))
        {
            // Turn the boat right
            transform.Rotate(Vector3.forward, -turnSpeed * Time.deltaTime * GetTurnCoefficient(rb.velocity.magnitude));
            rb.velocity = Rotate(rb.velocity, -turnSpeed * Time.deltaTime * GetTurnCoefficient(rb.velocity.magnitude) * Mathf.Deg2Rad);
        }

        ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
        int i = 0;
        foreach (ParticleSystem particle in particleSystems)
        {
            var emission = particle.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(emissions[i] * (rb.velocity.magnitude/maxSpeedForward) * (rb.velocity.magnitude / maxSpeedForward));
            var particleMain = particle.main;
            particleMain.startSpeed = new ParticleSystem.MinMaxCurve(rb.velocity.magnitude / maxSpeedForward * 1.5f, rb.velocity.magnitude / maxSpeedForward * 2.5f) ; // Adjust particle speed based on boat speed
            var shape = particle.shape;
            shape.angle = rb.velocity.magnitude / (maxSpeedForward * 0.05f); // Adjust particle shape angle based on boat speed
        }
    }

    internal Vector2 Rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    internal float GetTurnCoefficient(float speed)
    {
        // Calculate the turn coefficient based on the speed of the boat
        if (speed < 0.5 * maxSpeedForward)
        {
            return speed / (0.5f * maxSpeedForward);
        } 
        else if (speed < 0.75 * maxSpeedForward)
        {
            return -(speed - 0.5f * maxSpeedForward) * 2f / maxSpeedForward + 1f;
        }
        else
        {
            return -(speed - 0.75f * maxSpeedForward) * 1f / maxSpeedForward + 0.5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisioned)
        {
            // If the boat has already collided, ignore further collisions
            return;
        }
        // Check if the boat collides with a buoy
        if (collision.gameObject.CompareTag("Buoy"))
        {
            collisioned = true; // Set the collision flag to true
            StartCoroutine(CollisionedBack()); // Start the coroutine to reset the collision flag after a delay
            // Handle buoy collision logic here
            Debug.Log("Boat collided with a buoy: " + collision.gameObject.name);

            GameObject fishCatcher = Instantiate(new GameObject() { name = "FishCatcher" }, Vector3.zero, Quaternion.identity);
            PolygonCollider2D collider2D = fishCatcher.AddComponent<PolygonCollider2D>();
            collider2D.isTrigger = true; // Set the collider as a trigger

            GameObject currentByoy = collision.gameObject;
            List<Vector2> path = new List<Vector2> { new Vector2(currentByoy.transform.position.x, currentByoy.transform.position.y) };
            while (true)
            {
                currentByoy = currentByoy.GetComponent<NetExtension>().followedPoint;
                if (currentByoy == null)
                {
                    Debug.LogWarning("No followed point found for the buoy. Stopping path creation.");
                    break;
                }
                if (currentByoy.CompareTag("Boat"))
                {
                    path.Add(new Vector2(currentByoy.transform.position.x, currentByoy.transform.position.y));
                    break;
                }
                path.Add(new Vector2(currentByoy.transform.position.x, currentByoy.transform.position.y));
            }

            collider2D.SetPath(0, path);
            collider2D.excludeLayers = LayerMask.GetMask("buoy", "boat"); // Exclude the boat and fish catcher layers from the collider
            MeshRenderer meshRenderer = fishCatcher.AddComponent<MeshRenderer>();
            meshRenderer.material = closedNet; // Assign the closed net material to the fish catcher
            meshRenderer.material.mainTexture = closeNet; // Assign the closed net texture to the fish catcher

            // Add a MeshFilter to the fish catcher to visualize the collider
            MeshFilter meshFilter = fishCatcher.AddComponent<MeshFilter>();
            Mesh mesh = collider2D.CreateMesh(false, false);
            meshFilter.mesh = mesh;

            Vector3[] vertex = mesh.vertices;
            Vector2[] uvs = new Vector2[vertex.Length];
            Debug.Log(uvs.Length + " " + vertex.Length);
            for(int i = 0; i < vertex.Length; i++)
                uvs[i] = new Vector2(vertex[i].x, vertex[i].y);
            mesh.uv = uvs;
            mesh.RecalculateBounds();
            

            fishCatcher.AddComponent<FishCatcher>().lifetime = netTime;
        }
    }

    IEnumerator CollisionedBack()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before allowing further collisions
        collisioned = false; // Reset the collision flag
    }
}
