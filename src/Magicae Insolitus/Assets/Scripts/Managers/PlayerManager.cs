using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        KeyBinder.instance.OnMove += OnMove;
    }

    private void OnMove(float horizontal, float vertical)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }
}
