using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private List<Transform> players;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.25f;

    private Vector3 velocity;

    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, GetBoundsCenter() + offset, ref velocity, smoothTime);
    }

    Vector3 GetBoundsCenter()
    {
        Bounds bound = new Bounds(players[0].position, Vector3.zero);

        for (int i = 0; i < players.Count; ++i)
        {
            bound.Encapsulate(players[i].position);
        }
        return bound.center;
    }
}
