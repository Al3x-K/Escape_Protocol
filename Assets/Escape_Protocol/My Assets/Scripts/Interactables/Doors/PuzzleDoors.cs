using UnityEngine;
using System.Collections;
using GD.Audio;
using GD.Types;
using GD.Items;

public class PuzzleDoor : MonoBehaviour
{
    public GameObject door;
    public ItemData requiredKeyItem;
    public float slideDistance;
    public float slideSpeed;

    public AudioClip openDoorSound;
    public AudioMixerGroupName audioGroup = AudioMixerGroupName.SFX;
    public AudioClip denySound;
    public AudioClip grantAccessSound;
    public AudioMixerGroupName audioGroup1 = AudioMixerGroupName.Voiceover;

    private Vector3 originalPosition;
    private bool isOpen = false;

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void UnlockDoor()
    {
        if (isOpen) return;

        Debug.Log("Door is unlocked!");
        PlayOpenSound();
        this.ToggleActiveState();
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

    public void PlayDenySound()
    {
        AudioManager.Instance.PlaySound(denySound, audioGroup1, this.transform.position);
    }

    public void PlayGrantAccessSound()
    {
        AudioManager.Instance.PlaySound(grantAccessSound, audioGroup1, this.transform.position);
    }

    public void ToggleActiveState()
    {
        if (door != null)
        {
            // Toggle the active state
            door.SetActive(!door.activeSelf);
            Debug.Log($"{door.name} is now {(door.activeSelf ? "active" : "inactive")}");
        }
        else
        {
            Debug.LogWarning("No door is assigned!");
        }
    }

}
