using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireDeath2 : MonoBehaviour
{
    public Transform target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == target.name)
        {
            SceneManager.LoadScene(1);
        }
    }
}
