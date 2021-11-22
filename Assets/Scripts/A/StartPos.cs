using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPos : MonoBehaviour
{
    // Start is called before the first frame update
    public BoxCollider boxCollider;
    Grid gird;
    public Main main;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        main = GetComponent<Main>();
    }

    // Update is called once per frame
    void Update()
    {
        StartPos_();
    }


    public void StartPos_()
    {
        Node node = Ray();
        if (node != null)
        {
            main.start = node;
        }
    }
    public Node Ray()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, -transform.transform.up, out hit))
        {
            GameObject obj = hit.collider.gameObject;
            return gird.NodePoint(obj.transform.position);
        }
        return null;
    }
}
