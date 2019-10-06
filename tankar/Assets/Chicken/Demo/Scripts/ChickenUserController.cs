using UnityEngine;
using System.Collections;

public class ChickenUserController : MonoBehaviour {

    public ChickenCharacter chickenCharacter;
    public float upDownInputSpeed = 3f;


    void Start()
    {
        chickenCharacter = GetComponent<ChickenCharacter>();
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

    void FixedUpdate()
    {
        chickenCharacter.forwardAcceleration = Input.GetAxis("Vertical");
        chickenCharacter.yawVelocity = Input.GetAxis("Horizontal");
    }
}
