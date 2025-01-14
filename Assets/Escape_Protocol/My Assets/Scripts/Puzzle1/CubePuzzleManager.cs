using System.Collections.Generic;
using GD.Audio;
using GD.Types;
using UnityEngine;

public class CubePuzzleManager : MonoBehaviour
{
    public List<PuzzleCube> cubes; // Assign all cubes in the Inspector
    public List<Pedestal> pedestals; // Assign all pedestals in the Inspector
    public AudioClip puzzleCompleteSound; // Sound for puzzle completion
    public AudioClip puzzleCompleteSound2;
    public AudioMixerGroupName audioGroup = AudioMixerGroupName.SFX;

    private bool isPuzzleComplete = false;

    private void Update()
    {
        if (isPuzzleComplete) return;

        if (AreAllCubesCorrectlyPlaced())
        {
            isPuzzleComplete = true;
            CompletePuzzle();
        }
    }

    private bool AreAllCubesCorrectlyPlaced()
    {
        foreach (var cube in cubes)
        {
            bool isOnCorrectPedestal = false;

            foreach (var pedestal in pedestals)
            {
                float distance = Vector3.Distance(cube.transform.position, pedestal.transform.position);
                Debug.Log($"Cube {cube.name} is {distance} units away from pedestal {pedestal.name}");

                if (distance < 10f && pedestal.IsCubeOnTop(cube))
                {
                    Debug.Log($"Cube {cube.name} matches pedestal {pedestal.name}");
                    isOnCorrectPedestal = true;
                    break;
                }
            }

            if (!isOnCorrectPedestal)
            {
                Debug.LogWarning($"Cube {cube.name} is NOT on the correct pedestal.");
                return false;
            }
        }

        Debug.Log("All cubes are correctly placed!");
        return true;
    }

    private void CompletePuzzle()
    {
        Debug.Log("Puzzle Completed!");
        PlaySound(puzzleCompleteSound);
        PlaySound(puzzleCompleteSound2);

        // Trigger additional actions here (e.g., unlock a door, play effects, etc.)
    }

    private void PlaySound(AudioClip sound)
    {
        if (sound != null)
        {
            Debug.Log($"Playing puzzle completion sound: {sound.name}");
            AudioManager.Instance.PlaySound(sound, audioGroup, transform.position);
        }
        else
        {
            Debug.LogWarning("Puzzle complete sound is not assigned.");
        }
    }
}
