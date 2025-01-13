using UnityEngine;
using System.Collections;
using GD.Audio;
using GD.Types;

public class ProximityDoor : MonoBehaviour
{
    public Transform door;          
    public float slideDistance; 
    public float slideSpeed;  

    private Vector3 originalPosition; 
    private bool isOpen = false;

    public AudioClip openDoorSound;
    public AudioMixerGroupName audioGroup = AudioMixerGroupName.SFX;


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
        PlayOpenSound();
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

    private void PlayOpenSound()
    {
        if (openDoorSound != null)
        {
            AudioManager.Instance.PlaySound(openDoorSound, audioGroup, this.transform.position);
        }
        else
        {
            Debug.LogWarning("Open door sound is not assigned.");
        }
    }
}

