using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMen : MonoBehaviour
{

    public GameObject body;
    public GameObject hammer;

    Rigidbody body_rig;
    Rigidbody hammer_rig;
    Transform hammer_anchor;
    // Start is called before the first frame update
    void Start()
    {
        //body = GetComponentsInChildren<GameObject>()[1];
       // hammer = GetComponentsInChildren<GameObject>()[2];

        body_rig = body.GetComponent<Rigidbody>();
        hammer_rig = hammer.GetComponent<Rigidbody>();
        hammer_anchor = hammer.transform.GetChild(2).GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        HammerControl();
    }

    void HammerControl()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        hammer_rig.velocity =(new Vector3(mousePosition.x,mousePosition.y,hammer.transform.position.z)-hammer_anchor.position)*10;

        Vector3 direction =(new Vector3(mousePosition.x,mousePosition.y,hammer_anchor.position.z)-new Vector3(body.transform.position.x, body.transform.position.y, hammer_anchor.position.z).normalized);

        hammer.transform.RotateAround(hammer_anchor.position, Vector3.Cross(hammer_anchor.up, direction), Vector3.Angle(hammer_anchor.up, direction));

    }
}
