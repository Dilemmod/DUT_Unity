using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerPlayer : MonoBehaviour
{
    [SerializeField] private Collider2D ememy;
    public bool trigered=false;
    private void Update()
    {
        if (ememy != null)
            if (ememy.enabled == false)
                trigered = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        trigered = true;
        Destroy(this);
    }
}
