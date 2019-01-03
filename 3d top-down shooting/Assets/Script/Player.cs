using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerController))] // Player 스크립트를 오브젝트에 붙일 때 PlayerController 또한 같이 붙이도록 강요함
public class Player : MonoBehaviour {

    public float moveSpeed = 5;
    PlayerController controller;
    Camera viewCamera;
	// Use this for initialization
	void Start () {
        controller = GetComponent<PlayerController>(); // PlayerController와 Player 스크립트가 같은 오브젝트에 붙어 있다고 가정
        viewCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            controller.LookAt(point);
        }
	}
}
