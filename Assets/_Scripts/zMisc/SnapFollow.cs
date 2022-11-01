using System;
using UnityEngine;

/// <summary>
/// Ensures grid floor plane continues on forever
/// Taken from: https://www.youtube.com/watch?v=v39NEg3EfSE&t=175s
/// Via: https://www.reddit.com/r/Unity3D/comments/dtrm7i/how_to_make_an_infinite_grid_in_unity/
/// </summary>
public class SnapFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float snap;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var position = target.position;
        Vector3 pos = new Vector3(Mathf.Round(position.x / snap) * snap, _startPosition.y, Mathf.Round(position.z / snap) * snap);
        transform.position = pos;
    }
}
