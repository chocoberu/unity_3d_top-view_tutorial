using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerController))] // Player 스크립트를 오브젝트에 붙일 때 PlayerController 또한 같이 붙이도록 강요함
public class Player : MonoBehaviour {

    public float moveSpeed = 5;
    PlayerController controller;
	// Use this for initialization
	void Start () {
        controller = GetComponent<PlayerController>(); // PlayerController와 Player 스크립트가 같은 오브젝트에 붙어 있다고 가정
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

	}
}
