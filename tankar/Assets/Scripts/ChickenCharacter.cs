using UnityEngine;
using System.Collections;

public class ChickenCharacter : MonoBehaviour
{
  public Animator chickenAnimator;
  public float chickenSpeed = 1f;
  Rigidbody chickenRigid;
  public bool isFlying = false;
  public float upDown = 0f;
  public float forwardAcceleration = 0f;

  public float forwardAccelerationMultiplier = 1f;
  public float yawVelocity = 0f;
  public float groundCheckDistance = 5f;
  public bool isGrounded = true;
  public float forwardSpeed = 0f;
  public float maxForwardSpeed = 3f;
  public float meanForwardSpeed = 1.5f;
  public float speedDumpingTime = .1f;
  public float groundCheckOffset = 0.1f;
  float soaringTime = 0f;
  public bool isLived = true;
  float walkMode = 1f;

  void Start()
  {
    chickenAnimator = GetComponent<Animator>();
    chickenAnimator.speed = chickenSpeed;
    chickenRigid = GetComponent<Rigidbody>();
  }

  void Update()
  {
    Move();
    soaringTime = soaringTime + Time.deltaTime;
    GroundedCheck();
  }

  void GroundedCheck()
  {
    RaycastHit hit;
    if (Physics.Raycast(transform.position + Vector3.up * groundCheckOffset, Vector3.down, out hit, groundCheckDistance))
    {
      if (!isFlying || (isFlying && soaringTime > 1f))
      {
        Landing();
        isGrounded = true;
      }
    }
    else
    {
      isGrounded = false;
      chickenAnimator.SetBool("IsGrounded", false);
    }
  }

  public void SpeedSet(float animSpeed)
  {
    chickenAnimator.speed = animSpeed;
  }

  public void Landing()
  {
    chickenAnimator.SetBool("IsGrounded", true);
    chickenAnimator.SetBool("IsFlying", false);

    chickenAnimator.applyRootMotion = true;
    chickenRigid.useGravity = true;
    isFlying = false;
  }

  public void Soar()
  {
    if (isGrounded && isLived)
    {
      soaringTime = 0f;
      chickenAnimator.SetBool("IsGrounded", false);
      chickenAnimator.SetBool("IsFlying", true);
      chickenAnimator.SetTrigger("Soar");
      chickenRigid.useGravity = false;
      isGrounded = false;
      forwardAcceleration = 0f;
      forwardSpeed = 0f;
      upDown = 0f;
      chickenAnimator.applyRootMotion = false;
      isFlying = true;
    }
  }

  public void Attack()
  {
    chickenAnimator.SetTrigger("Attack");
  }

  public void Hit()
  {
    chickenAnimator.SetTrigger("Hit");
  }


  public void Death()
  {
    chickenAnimator.SetBool("IsLived", false);
    isLived = false;
  }

  public void Rebirth()
  {
    chickenAnimator.SetBool("IsLived", true);
    isLived = true;
  }

  public void Eat()
  {
    chickenAnimator.SetTrigger("Eat");
  }

  public void Grooming()
  {
    chickenAnimator.SetTrigger("Grooming");
  }

  public void Crowing()
  {
    chickenAnimator.SetTrigger("Crowing");
  }

  public void Walk()
  {
    walkMode = 1f;
  }

  public void Gallop()
  {
    walkMode = 2f;
  }

  public void Move()
  {
    chickenAnimator.SetFloat("Forward", forwardAcceleration);
  }
}
