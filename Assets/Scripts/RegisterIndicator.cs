using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterIndicator : MonoBehaviour
{
    [Range(5,30)]
    [SerializeField] float destroyTimer = 20f;
    public void Start()
    {
        Invoke("Register", Random.Range(0f, 8f));

    }
    void Register()
    {
        if (!D1_System.CheckIfObjectInSight(this.transform))
        {
            D1_System.CreateIndicator(this.transform);
        }
    }
}
