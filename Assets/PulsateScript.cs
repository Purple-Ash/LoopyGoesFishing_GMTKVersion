using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsateScript : MonoBehaviour
{
    [SerializeField] float intensity;
    [SerializeField] float frequency;
    void Update()
    {
        transform.localScale = Vector3.one * (1 + Mathf.Sin(Time.time* frequency) * intensity);
    }
}
