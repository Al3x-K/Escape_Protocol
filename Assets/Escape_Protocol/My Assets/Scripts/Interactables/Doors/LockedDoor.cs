using UnityEngine;
using System.Collections;

public class LockedDoor : MonoBehaviour
{
    public string requiredKeyID;

    private bool isUnlocked = false;
    private Vector3 originalPosition; 
    public float slideDistance; 
    public float slideSpeed;

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void UnlockDoor()
    {
        if (isUnlocked) return; 
        isUnlocked = true;
        StartCoroutine(SlideDoor());
    }

    private IEnumerator SlideDoor()
    {
        Vector3 targetPosition = originalPosition - new Vector3(0, slideDistance, 0);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }

        isUnlocked = false; 
    }
}
