using System;
using UnityEngine;

public class Bernoli : MonoBehaviour
{
    public float gravity;
    public float mass;
    private float height = 2.9f;
    private double velocity;
    public GameObject brush;
    private void Update()
    {
        transform.Translate(Vector3.down * ((float)velocity * Time.deltaTime));
        var position = gameObject.transform.position;
        if(position.y < 1.6f)
        {
            position.y = 1.567292f;
            Instantiate(brush, position , Quaternion.identity);
            Destroy(gameObject);
        }
    }
    void Start()
    {
        velocity = 0.6 * Math.Sqrt(2*gravity * height)  + gravity * mass;
    }
}
