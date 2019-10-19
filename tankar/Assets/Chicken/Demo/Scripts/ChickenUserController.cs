using UnityEngine;
using System.Collections;

public class ChickenUserController : MonoBehaviour {

    public ChickenCharacter chickenCharacter;
    public float upDownInputSpeed = 3f;
    private Vector3 last_position;

    void Start()
    {
        chickenCharacter = GetComponent<ChickenCharacter>();
        last_position = chickenCharacter.transform.position;
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

    float mod(float x, float m) {
        return (x%m + m)%m;
    }

    void FixedUpdate()
    {
        // the original controls
        // chickenCharacter.forwardAcceleration = Input.GetAxis("Vertical");
        // chickenCharacter.yawVelocity = Input.GetAxis("Horizontal");

        float acceleration_scalar = 100.0f;
        float yaw_scalar = 10.0f;

        // chicken position
        float chicken_position_x = chickenCharacter.transform.position.x;
        float chicken_position_z = chickenCharacter.transform.position.z;

        // human position
        // TODO: this should be Human Cube or AR Camera
        GameObject human = GameObject.Find("AR Camera");
        float human_position_x = human.transform.position.x;
        float human_position_z = human.transform.position.z;

        float difference_x = chicken_position_x - human_position_x;
        float difference_z = chicken_position_z - human_position_z;
        float distance = Mathf.Sqrt(difference_x * difference_x + difference_z * difference_z);

        // maybe use the velocity to affect the acceleration of the chicken
        float human_velocity_x = (human_position_x - last_position.x) / Time.deltaTime;
        float human_velocity_z = (human_position_z - last_position.z) / Time.deltaTime;
        float human_velocity_magnitude = Mathf.Sqrt(human_velocity_x * human_velocity_x + human_velocity_z * human_velocity_z);

        float forward_acceleration = 1.0f - (distance / 7.0f);
        chickenCharacter.forwardAcceleration = acceleration_scalar * forward_acceleration;

        // the target angle [0, 360.0)
        float angle = mod((90.0f - Mathf.Atan2(difference_z, difference_x) * Mathf.Rad2Deg) + 270, 360.0f);

        // the current heading angle of chicken
        float chicken_heading_angle = Quaternion.LookRotation(chickenCharacter.transform.forward).eulerAngles.y;

        // calculate the angle command
        float angle_offset = mod(angle - chicken_heading_angle, 360.0f);
        // Debug.Log(angle_offset);
        if (angle_offset > 180.0f) {
            // map to [0, -180.0]
            angle_offset = -(360.0f - angle_offset);
        }
        chickenCharacter.yawVelocity = yaw_scalar * (angle_offset / 180.0f);

        // set the last position
        last_position = human.transform.position;
    }
}
