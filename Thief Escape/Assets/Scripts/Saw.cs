using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, -90 * Time.fixedDeltaTime);
    }
}
