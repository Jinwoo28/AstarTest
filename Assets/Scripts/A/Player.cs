using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public BoxCollider boxCollider;
    Grid gird;
    public Main main;

    public void StartPos()
    {
        Node node = Ray();
        if(node != null)
        {
            main.start = node;
        }
    }
    public Node Ray()
    {
        RaycastHit hit;
        if(Physics.Raycast(this.transform.position,-transform.transform.up,out hit))
        {
            GameObject obj = hit.collider.gameObject;
            return gird.NodePoint(obj.transform.position);
        }
        return null;
    }
}

