using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurretMovement : MonoBehaviour
{

  public float m_x_angle = 0;
  public float m_y_angle = 0;
  public float m_rotate_speed = 120f;

  private float m_x_angle_min = -180;
  private float m_x_angle_max = 180;

  private float m_y_angle_min = 0;
  private float m_y_angle_max = 70;

  private float m_y_offset = 8;

  private string m_RotateXAxisName = "RotateX1";
  private string m_RotateYAxisName = "RotateY1";

  private float m_x_angle_input = 0;
  private float m_y_angle_input = 0;
  private MeshRenderer turretRenderer;
  // Start is called before the first frame update

  private void UpdateTurrentAngle()
  {
    Vector3 new_rotation = new Vector3(
        this.turretRenderer.transform.eulerAngles.x + 180 % 360,
        this.turretRenderer.transform.eulerAngles.y,
        this.turretRenderer.transform.eulerAngles.z);
    new_rotation.x = Mathf.Max(new_rotation.x, 0);
    float new_x = new_rotation.x - m_y_angle_input * Time.deltaTime;
    print("New x: " + new_x % 360);

    if (180 - (new_x % 360) > m_y_angle_min && 180 - (new_x % 360) < m_y_angle_max)
    {
      new_rotation.x = new_x;
    }
    new_rotation.x -= 180;
    new_rotation.y += m_x_angle_input * Time.deltaTime;

    this.turretRenderer.transform.eulerAngles = new_rotation;


    print("x: " + this.turretRenderer.transform.eulerAngles.y + ", y: " + this.turretRenderer.transform.eulerAngles.x);

  }

  void Start()
  {
    print("Start Tank Turret Movement Script");
    MeshRenderer[] objs = this.GetComponentsInChildren<MeshRenderer>();
    foreach (MeshRenderer o in objs)
    {
      if (o.name.Equals("TankTurret"))
      {
        this.turretRenderer = o;
      }
    }
  }

  // Update is called once per frame
  private void Update()
  {
    m_x_angle_input = Input.GetAxis(m_RotateXAxisName) * this.m_rotate_speed;
    m_y_angle_input = Input.GetAxis(m_RotateYAxisName) * this.m_rotate_speed / 4;
  }

  private void FixedUpdate()
  {
    this.UpdateTurrentAngle();
  }

}
