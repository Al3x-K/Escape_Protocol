using UnityEngine;
using System.Collections;

public class ProximityDoor : MonoBehaviour
{
    public Transform door;          
    public float slideDistance; 
    public float slideSpeed;  

    private Vector3 originalPosition; 
    private bool isOpen = false;


    private void Start()
    {
        if (door != null)
        {
            originalPosition = door.position;
        }
        else
        {
            Debug.LogError("Door reference not assigned to ProximityDoor.");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen && other.CompareTag("Player"))
        {
            StartCoroutine(OpenDoor());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isOpen && other.CompareTag("Player"))
        {
            StartCoroutine(CloseDoor());
        }
    }

    private IEnumerator OpenDoor()
    {
        isOpen = true;

        Vector3 targetPosition = originalPosition - new Vector3(0, slideDistance, 0);
        while (Vector3.Distance(door.position, targetPosition) > 0.01f)
        {
            door.position = Vector3.MoveTowards(door.position, targetPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(2f);

        while (Vector3.Distance(door.position, originalPosition) > 0.01f)
        {
            door.position = Vector3.MoveTowards(door.position, originalPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }

        isOpen = false;
    }
}

