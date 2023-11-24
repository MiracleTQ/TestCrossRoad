using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{


    private void Update()
    {
        //FIXME
        CheckPostion();
    }
    private void CheckPostion()
    {
        if (Camera.main.transform.position.y - transform.position.y > 25)
        {
            Destroy(this.gameObject);
        }
    }
}
