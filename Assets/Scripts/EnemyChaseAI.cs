using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseAI : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float roamRadius = 5f;
    public float roamInterval = 3f;

    private Transform player;
    private bool playerInRange = false;

    private Vector3 roamTarget;
    private float roamTimer;

    // Use this to rotate the model 180 degrees if it's facing backwards
    public Vector3 modelForwardOffset = new Vector3(0, 180, 0);

    private void Start()
    {
        PickNewRoamTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            PickNewRoamTarget();
            roamTimer = 0f;
        }
    }

    private void Update()
    {
        if (playerInRange && player != null)
        {
            ChasePlayer();
        }
        else
        {
            Roam();
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        MoveAndRotate(direction);
    }

    private void Roam()
    {
        roamTimer += Time.deltaTime;

        Vector3 direction = (roamTarget - transform.position);
        direction.y = 0;

        if (direction.magnitude < 0.2f || roamTimer >= roamInterval)
        {
            PickNewRoamTarget();
            roamTimer = 0f;
        }
        else
        {
            MoveAndRotate(direction.normalized);
        }
    }

    private void MoveAndRotate(Vector3 direction)
    {
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            toRotation *= Quaternion.Euler(modelForwardOffset); // Apply model rotation offset
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
    }

    private void PickNewRoamTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * roamRadius;
        roamTarget = new Vector3(transform.position.x + randomCircle.x, transform.position.y, transform.position.z + randomCircle.y);
    }
}
