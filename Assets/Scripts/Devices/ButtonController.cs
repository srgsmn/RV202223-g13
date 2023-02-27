using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ConnectedDoor;
    private Camera _cam;
    private bool _changeState=false;
    private bool _internalState=false;
    private NavMeshModifier _nvm;


    public delegate void ButtonPressed();
    public static event ButtonPressed OnButtonPressed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        _cam=Camera.main;
        if(_changeState){
            _changeState=false;
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


    private int? GetNavMeshAgentID(string name)
    {
        for (int i = 0; i < UnityEngine.AI.NavMesh.GetSettingsCount(); i++)
        {
            NavMeshBuildSettings settings = UnityEngine.AI.NavMesh.GetSettingsByIndex(index: i);
            if (name == UnityEngine.AI.NavMesh.GetSettingsNameFromID(agentTypeID: settings.agentTypeID))
            {
                return settings.agentTypeID;
            }
        }
        return null;
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
