using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnсeHorizontalPlatfoemMover : MonoBehaviour
{
    [SerializeField] private float speed=1;
    [SerializeField] private float range;
    [SerializeField] private Direction directionEnum;
    [SerializeField] private TrigerPlayer trigerPlayer;
    [SerializeField] private AudioSource audioMove;
    private Vector2 startPoint;
    private int direction = 1;

    void Start()
    {
        if (directionEnum == Direction.Down)
            direction = -1;
        else if(directionEnum == Direction.Up)
            direction = 1;
        startPoint = transform.position;
    }
    private void Update()
    {
        if (trigerPlayer.trigered)
        {
            audioMove.Play();
            if (transform.position.y - startPoint.y > range && direction > 0)
                trigerPlayer.trigered = false;
            else if (startPoint.y - transform.position.y > range && direction < 0)
                trigerPlayer.trigered = false;
            transform.Translate(0, speed * direction * Time.deltaTime, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(0.5f, range * 2, 0));
    }
    private enum Direction
    {
        Up,
        Down,
    }
}
