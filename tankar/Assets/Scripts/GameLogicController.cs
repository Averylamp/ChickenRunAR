using UnityEngine;
using UnityEngine.UI;
using System;

public class GameLogicController : MonoBehaviour
{

  public double CATCH_DISTANCE = 2.5;
  private const float CHICKEN_RESPAWN_RANGE = 3.3f;

  private float chickenDistance = 100;
  private GameObject chicken;

  private void Start()
  {
    chicken = GameObject.Find("Chicken");
  }

  // Update is called once per frame
  public void Update()
  {
    UpdateChickenDistance();
  }

  public void RemoveAndReplaceChicken()
  {
    GameObject terrain = GameObject.Find("Terrain");
    GameObject confettiObject = GameObject.Find("ConfettiCelebration");
    confettiObject = Instantiate(confettiObject, chicken.transform.position, Quaternion.identity);
    confettiObject.transform.parent = terrain.transform;
    confettiObject.GetComponent<ParticleSystem>().Play();

    // GameObject confettiObject = Instantiate()
    //     ParticleSystem confetti = chicken.GetComponent<ParticleSystem>();
    //     confetti.Play();
    ChickenCharacter chickenController = chicken.GetComponent<ChickenCharacter>();
    chickenController.Death();
    Destroy(confettiObject, 3.0f);
    Destroy(chicken, 3.0f);
    Vector3 humanPosition = Vector3.zero;

    if (Application.platform == RuntimePlatform.IPhonePlayer)
    {
      humanPosition = GameObject.Find("AR Camera").transform.position;
    }
    else
    {
      humanPosition = GameObject.Find("Human Cube").transform.position;
    }
    Vector3 newChickenPosition = Vector3.zero;
    do
    {
      newChickenPosition = terrain.transform.position
      + (UnityEngine.Random.Range(-CHICKEN_RESPAWN_RANGE, CHICKEN_RESPAWN_RANGE)) * terrain.transform.right
      + (UnityEngine.Random.Range(-CHICKEN_RESPAWN_RANGE, CHICKEN_RESPAWN_RANGE)) * terrain.transform.forward;
    } while (Vector3.Distance(newChickenPosition, humanPosition) < CHICKEN_RESPAWN_RANGE);


    chicken = Instantiate(chicken, newChickenPosition, Quaternion.identity);
    chicken.transform.parent = terrain.transform;
    (chicken.GetComponent("ChickenCharacter") as ChickenCharacter).chickenSpeed += 0.3f;
  }

  public float GetChickenDistance()
  {
    return chickenDistance;
  }
  private void UpdateChickenDistance()
  {
    chickenDistance = 100; // Default is not close enough
    if (chicken != null)
    {
      if (Application.platform == RuntimePlatform.IPhonePlayer)
      {
        GameObject mainCamera = GameObject.Find("AR Camera");
        chickenDistance = Vector3.Distance(chicken.transform.position, mainCamera.transform.position);
      }
      else
      {
        GameObject humanCube = GameObject.Find("Human Cube");
        chickenDistance = Vector3.Distance(chicken.transform.position, humanCube.transform.position);
      }
    }
    else
    {
      Debug.Log("Can't find chicken");
    }


    try
    {
      Image catchButton = GameObject.Find("CatchChickenButton").GetComponent<Image>();
      if (chickenDistance < CATCH_DISTANCE)
      {
        catchButton.color = Color.green;
      }
      else
      {
        catchButton.color = Color.red;
      }

    }
    catch
    {
      // no-op
    }
  }

}
