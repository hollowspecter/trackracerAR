using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoPhysicsController : MonoBehaviour
{
    public float forwardSpeed = 0.5f;
    public float steeringPower = 0.2f;
    public float spinoutThreshhold = 0.1f;
    public float spinoutAngle = 45f;

    private Transform child;

    private Vector3 moveVector;
    private Vector3 gVector;
    // Use this for initialization
    void Start()
    {
        child = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 oldforward = transform.forward;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        moveVector = transform.forward * forwardSpeed * v + steeringPower * transform.right * h;

      
        if(moveVector.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(moveVector);
        }
        transform.position += moveVector;

        //orthogonal projection is:
        Vector3 a1, a2;
        a1 = oldforward * Vector3.Dot(moveVector, oldforward);
        a2 = moveVector - a1;

        gVector += a2;
        gVector = gVector * 0.95f;
        Debug.Log(gVector.sqrMagnitude);
        if(gVector.sqrMagnitude > spinoutThreshhold)
        {
            Vector3 localg = transform.worldToLocalMatrix* gVector;
            child.localEulerAngles = new Vector3(0, localg.x > 0 ? spinoutAngle : -spinoutAngle, 0);
        }
        else
        {
            child.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, moveVector * 10);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, gVector * 10);
    }
}
