using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtr : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
          //float rotateX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float rotateY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

           float distance = Vector3.Distance(target.transform.position, transform.position);

            transform.position +=  transform.right * -rotateY;//transform.right * -rotateX;

            transform.LookAt(target.transform);

            transform.position = target.transform.position + -transform.forward * distance;
        }
    }
}
