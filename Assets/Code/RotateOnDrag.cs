using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnDrag : MonoBehaviour {
    [SerializeField] private float _rotationSpeed;

    void Update() {
        if (Input.GetMouseButton(0)) {
            transform.Rotate((Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime),
                (Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime),
                0, Space.World);
        }
    }
}