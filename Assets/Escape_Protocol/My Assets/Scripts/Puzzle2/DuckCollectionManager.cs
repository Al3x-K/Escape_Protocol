using UnityEngine;

public class DuckCollectionManager : MonoBehaviour
{
    public static DuckCollectionManager Instance;

    public int totalDucks = 10; // Total number of ducks to collect
    private int collectedDucks = 0;

    public AudioClip puzzleCompleteSound; // Sound played when the puzzle is completed

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectDuck(RubberDuck duck)
    {
        collectedDucks++;
        Debug.Log($"Collected {collectedDucks}/{totalDucks} ducks!");

        // Check if all ducks are collected
        if (collectedDucks >= totalDucks)
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        Debug.Log("All ducks collected! Puzzle complete!");

        // Play completion sound
        if (puzzleCompleteSound != null)
        {
            AudioSource.PlayClipAtPoint(puzzleCompleteSound, Camera.main.transform.position);
        }

    }
}
