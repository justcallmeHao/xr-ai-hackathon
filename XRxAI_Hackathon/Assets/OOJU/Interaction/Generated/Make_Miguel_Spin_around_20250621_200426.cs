using UnityEngine;

public class Make_Miguel_Spin_around_20250621_200426 : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up, 100f * Time.deltaTime); // Spin Miguel around his y-axis
    }
}