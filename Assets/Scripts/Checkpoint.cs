using Blocks;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [HideInInspector] public bool reached = false;
    public UnityEvent OnCheckpointReached;

    void OnTriggerEnter2D(Collider2D collision)
    {
        reached = collision.TryGetComponent(out Block _);
        if (reached)
            OnCheckpointReached?.Invoke();
    }
}