using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointPicker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D info)
    {
        info.GetComponent<PlayerController>().SetCheckPoint(gameObject.transform);
        gameObject.SetActive(false);
    }
}
