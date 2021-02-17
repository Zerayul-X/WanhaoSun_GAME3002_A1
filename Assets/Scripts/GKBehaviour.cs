using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GKBehaviour : MonoBehaviour
{

    private Rigidbody m_rb;
    Vector3 newPosition;
    bool touchRight = false;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        newPosition = m_rb.position;
        if(!touchRight && newPosition.x <= 2.8f)
        {
            newPosition.x += 0.01f;
            if(newPosition.x >= 2.8f)
            {
                touchRight = true;
            }
        }
        else if (newPosition.x >= -2.8f)
        {
            newPosition.x -= 0.01f;
        }
        else
        {
            touchRight = false;
        }
        m_rb.MovePosition(newPosition);
    }
}
