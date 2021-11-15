using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAndArrow : MonoBehaviour
{

    private Rigidbody myBody;

    public float speed = 30f;

    public float deactivate_Timer = 3f;
    
    public float damage = 15f;


    // Start is called before the first frame update
    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Start()
    {
        Invoke("DeactivateGameObject", deactivate_Timer);
    }

    public void Launch(Camera mainCamera)
    {
        myBody.velocity = mainCamera.transform.forward * speed;

        transform.LookAt(transform.position + myBody.velocity);

    }
    void DeactivateGameObject()
    {
        if(gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }


}
