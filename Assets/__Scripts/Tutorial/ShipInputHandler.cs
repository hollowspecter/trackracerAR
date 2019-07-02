using UnityEngine;
using System.Collections;

public class ShipInputHandler : MonoBehaviour
{
    [SerializeField]
    float _speed = 2;

    public void Update ()
    {
        if ( Input.GetKey ( KeyCode.UpArrow ) )
        {
            this.transform.position += Vector3.forward * _speed * Time.deltaTime;
        }

        if ( Input.GetKey ( KeyCode.DownArrow ) )
        {
            this.transform.position -= Vector3.forward * _speed * Time.deltaTime;
        }
    }
}