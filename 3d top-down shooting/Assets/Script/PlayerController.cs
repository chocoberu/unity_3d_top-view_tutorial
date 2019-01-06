using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{

    Rigidbody myRigidbody; // 충돌에 영향을 받도록
    Vector3 velocity;

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody>();
	}

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    private void FixedUpdate() // 정기적이고 짧게 반복적으로 실행되어야함
                              // 프레임이 저하되어도 프레임에 시간의 가중치를 곱해 실행되어 이동 속도를 유지
    {
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime); // fixedDeltaTime은 두 fixedUpdate 메소드가 호출된 시간 간격
    }
    public void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }
}
