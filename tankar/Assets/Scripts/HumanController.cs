using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
  public float m_Speed = 12f;
  public float m_TurnSpeed = 180f;

  private string m_MovementHorizontalAxisName;
  private string m_MovementVerticalAxisName;
  private Rigidbody m_Rigidbody;
  private float m_MovementHorizontalInputValue;
  private float m_MovementVerticalInputValue;



  private void Start()
  {
    print("Start Movement script");
    m_MovementHorizontalAxisName = "HorizontalHuman";
    m_MovementVerticalAxisName = "VerticalHuman";
  }


  private void Update()
  {
    m_MovementHorizontalInputValue = Input.GetAxis(m_MovementHorizontalAxisName);
    m_MovementVerticalInputValue = Input.GetAxis(m_MovementVerticalAxisName);
  }

  private void FixedUpdate()
  {
    Move();
  }

  private void Move()
  {
    Vector3 movement = transform.forward * m_MovementHorizontalInputValue * m_Speed * Time.deltaTime + transform.forward * m_MovementVerticalInputValue * m_Speed * Time.deltaTime;
    m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
  }

}
