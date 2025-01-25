using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private List<Transform> players;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float zoomLimit;

    private Vector3 velocity;
    private Camera mCamera;

    private void Start()
    {
        mCamera = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        Move();
        Zoom();
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

    private void Move()
    {
        transform.position = Vector3.SmoothDamp(transform.position, GetBoundsCenter() + offset, ref velocity, smoothTime);
    }

    private void Zoom()
    {
        float currentSize = mCamera.orthographicSize;
        float newFOV = Mathf.Clamp(GetGreatestDistance() * zoomLimit, minZoom, maxZoom);
        if ((currentSize > newFOV && currentSize - newFOV < 0.01f) || (currentSize < newFOV && currentSize + 0.01f > newFOV))
        {
            mCamera.orthographicSize = newFOV;
            return;
        }
        mCamera.orthographicSize = Mathf.Lerp(mCamera.orthographicSize, newFOV, Time.deltaTime);
    }

    private float GetGreatestDistance()
    {
        Bounds bound = new Bounds(players[0].position, Vector3.zero);
        for (int i = 0; i < players.Count; i++)
        {
            bound.Encapsulate(players[i].position);
        }

        return Mathf.Max(bound.size.x, bound.size.y);
    }
}
