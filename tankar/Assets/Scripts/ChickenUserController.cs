using UnityEngine;
using System.Collections;

public class ChickenUserController : MonoBehaviour
{

  public ChickenCharacter chickenCharacter;
  public float upDownInputSpeed = 3f;
  float target_angle;
  float next_turn = 0.0f;

  void Start()
  {
    chickenCharacter = GetComponent<ChickenCharacter>();
    target_angle = 0.0f;
  }

  void Update()
  {
    if (Input.GetButtonDown("Jump"))
    {
      chickenCharacter.Soar();
    }

    if (Input.GetButtonDown("Fire1"))
    {
      chickenCharacter.Attack();
    }

    if (Input.GetKeyDown(KeyCode.H))
    {
      chickenCharacter.Hit();
    }

    if (Input.GetKey(KeyCode.K))
    {
      chickenCharacter.Death();
    }

    if (Input.GetKey(KeyCode.L))
    {
      chickenCharacter.Rebirth();
    }

    if (Input.GetKeyDown(KeyCode.E))
    {
      chickenCharacter.Eat();
    }

    if (Input.GetKeyDown(KeyCode.C))
    {
      chickenCharacter.Crowing();
    }

    if (Input.GetKeyDown(KeyCode.G))
    {
      chickenCharacter.Grooming();
    }

    if (Input.GetKey(KeyCode.X))
    {
      chickenCharacter.Walk();
    }

    if (Input.GetKey(KeyCode.R))
    {
      chickenCharacter.Gallop();
    }

    if (Input.GetKey(KeyCode.N))
    {
      chickenCharacter.upDown = Mathf.Clamp(chickenCharacter.upDown - Time.deltaTime * upDownInputSpeed, -1f, 1f);
    }
    if (Input.GetKey(KeyCode.U))
    {
      chickenCharacter.upDown = Mathf.Clamp(chickenCharacter.upDown + Time.deltaTime * upDownInputSpeed, -1f, 1f);
    }
  }

  float mod(float x, float m)
  {
    return (x % m + m) % m;
  }

  void FixedUpdate()
  {
    // the original controls
    // chickenCharacter.forwardAcceleration = Input.GetAxis("Vertical");
    // chickenCharacter.yawVelocity = Input.GetAxis("Horizontal");

    float yaw_scalar = 10.0f;

    // chicken position
    float chicken_position_x = chickenCharacter.transform.position.x;
    float chicken_position_z = chickenCharacter.transform.position.z;


    GameLogicController gameLogicController = GameObject.Find("Canvas").GetComponent<GameLogicController>();

    // human position
    Vector3 human_position = gameLogicController.GetPlayerPosition();
    float chickenDistance = gameLogicController.GetChickenDistance();

    float variableAcceleration = Mathf.Max(-2f * Mathf.Log10(chickenDistance) + 1.75f, 0f);
    float forward_acceleration = 0.5f + variableAcceleration;
    chickenCharacter.forwardAcceleration = forward_acceleration; //acceleration_scalar * forward_acceleration;
                                                                 // chickenCharacter.GetComponent<Rigidbody>().velocity = new Vector3(10.0f, 0.0f, 0.0f);

    // the target angle [0, 360.0)
    // float angle = mod((90.0f - Mathf.Atan2(difference_z, difference_x) * Mathf.Rad2Deg) + 270, 360.0f);
    // set the target angle
    float angle = target_angle;
    float current_time = Time.time;
    if (current_time > next_turn)
    {
      angle = mod(target_angle + Random.Range(90.0f, 270.0f), 360.0f);
      target_angle = angle;
      next_turn = current_time + Random.Range(0.5f, 3.0f);
    }

    // the current heading angle of chicken
    float chicken_heading_angle = Quaternion.LookRotation(chickenCharacter.transform.forward).eulerAngles.y;

    // calculate the angle command
    float angle_offset = mod(angle - chicken_heading_angle, 360.0f);
    // Debug.Log(angle_offset);
    if (angle_offset > 180.0f)
    {
      // map to [0, -180.0]
      angle_offset = -(360.0f - angle_offset);
    }


    chickenCharacter.yawVelocity = yaw_scalar * (angle_offset / 180.0f);

  }
}
