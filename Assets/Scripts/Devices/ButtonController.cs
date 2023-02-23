using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ConnectedDoor;
    private Camera _cam;
    private bool _changeState=false;
    private bool _internalState=false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        _cam=Camera.main;
        if(_changeState){
            _changeState=false;
            Debug.Log("AbbastanzaVicino");
            if ((this.gameObject.transform.position - _cam.transform.position).magnitude<=4.0f){
                if (_internalState){
                    ConnectedDoor.GetComponent<Renderer>().enabled=true;
                    if (ConnectedDoor.TryGetComponent(typeof(Collider),out Component mf)){
                        Collider c=(Collider) mf;
                        c.enabled=true;
                    }
                    else{
                        Collider[] cols = ConnectedDoor.GetComponentsInChildren<Collider>();
                        foreach(Collider c in cols){
                            c.enabled=true;
                        }
                    }
                    
                }
                else{
                    ConnectedDoor.GetComponent<Renderer>().enabled=false;
                    if (ConnectedDoor.TryGetComponent(typeof(Collider),out Component mf)){
                        Collider c=(Collider) mf;
                        c.enabled=false;
                    }
                    else{
                        Collider[] cols = ConnectedDoor.GetComponentsInChildren<Collider>();
                        foreach(Collider c in cols){
                            c.enabled=false;
                        }
                    }
                }
                _internalState=!_internalState;
            }
        }
    }

    public void ChangeDoorState(){
        _changeState=true;
    }

    private void Awake(){
        InputManager.OnSelection+=ChangeDoorState;
    }

    private void OnDisable(){
        InputManager.OnSelection-=ChangeDoorState;
    }
}
