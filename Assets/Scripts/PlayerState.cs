using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // Player Health
    public float currentHealth;
    public float maxHealth;

    // Player Calories
    public float currentCalories;
    public float maxCalories;

    // Player Hydration
    public float currentHydration;
    public float maxHydration;
    public bool isHydrationDecreasing;


    // calorie depletion from walking
    float distanceTraveled = 0;
    Vector3 lastPosition;
    public GameObject playerBody;


    // always need this in singletons
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydration = maxHydration;

        StartCoroutine(decreaseHydration());

    }

    IEnumerator decreaseHydration()
    {
        while (true)
        {
            currentHydration -= 1;
            yield return new WaitForSeconds(2);
        }
    }

    // Update is called once per frame
    void Update()
    {

        // calorie depletion from walking
        distanceTraveled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;
        // -1 calorie for every 5 units of distance
        if (distanceTraveled >= 5f)
        {
            distanceTraveled = 0;
            currentCalories -= 1;
        }

        // testing the health bar
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }

    public void setHealth(float adjustedHealth)
    {
        currentHealth = adjustedHealth;
    }

    public void setCalories(float adjustedCalories)
    {
        currentCalories = adjustedCalories;
    }

    public void setHydration(float adjustedHydration)
    {
        currentHydration = adjustedHydration;
    }

}
