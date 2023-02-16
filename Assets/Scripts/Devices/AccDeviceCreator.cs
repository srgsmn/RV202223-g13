using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
public class AccDeviceCreator : MonoBehaviour
{   
    public enum acc_device {Button, Ramp, Stairlift};
    public Camera PlayerCamera;
    public GameObject Door;
    public GameObject Stair;
    public acc_device device_type = acc_device.Button;
    private float _range = 100f;
    private GameObject _selected;
    private RaycastHit _raycastHit;
    private GameObject _waypoint_pf;
    private GameObject _waypoint;
    private GameObject _button_pf;
    private GameObject _button;
    private Renderer[] _wpRND;
    private bool _raysStarted=false;
    private bool _startInsert=false;
    // Start is called before the first frame update
    void Start()
    {
        _waypoint_pf= (GameObject)Resources.Load("Prefabs/waypoint", typeof(GameObject));
        _button_pf = (GameObject)Resources.Load("Prefabs/Button", typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {
       
        if (_startInsert){
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out _raycastHit,_range))
            {   
                switch(device_type){
                    case acc_device.Button:
                    //BUTTON MODE
                        if (_raycastHit.transform.name.ToLower().IndexOf("wall")!=-1){
                            if(!_raysStarted){
                                _waypoint=Instantiate(_waypoint_pf, _raycastHit.point, Quaternion.identity);
                                _raysStarted=true;
                            }
                            Debug.DrawRay(PlayerCamera.transform.position,_raycastHit.point-PlayerCamera.transform.position,Color.red,0.0f,true);
                            if (_waypoint!=null){
                                _waypoint.transform.position=_raycastHit.point;
                                _waypoint.transform.rotation=Quaternion.FromToRotation(Vector3.up, _raycastHit.normal);
                                if (Door!=null){
                                    if ((Door.transform.position-_raycastHit.point).magnitude<=2.5f){
                                        _wpRND=_waypoint.GetComponentsInChildren<Renderer>();
                                        foreach(Renderer x in _wpRND){
                                            x.material.SetColor("_Color",Color.green);
                                            x.material.SetColor("_EmissionColor",Color.green);
                                        }
                                        if (Input.GetKeyDown(KeyCode.T)){
                                            _button=Instantiate(_button_pf,_raycastHit.point,Quaternion.FromToRotation(Vector3.up,_raycastHit.normal));
                                        }
                                    }
                                    else{
                                        _wpRND=_waypoint.GetComponentsInChildren<Renderer>();
                                        foreach(Renderer x in _wpRND){
                                            x.material.SetColor("_Color",Color.red);
                                            x.material.SetColor("_EmissionColor",Color.red);
                                        }
                                    }
                                }
                            }
                            else{
                                if (_raysStarted){
                                    Destroy(_waypoint);
                                    _raysStarted=false;
                                }
                            }
                        }
                        break;
                    case acc_device.Stairlift:
                    /*STAIRLIFT MODE
                        if (_raycastHit.transform.name.ToLower().IndexOf("floor")!=-1){
                            if(!_raysStarted){
                                _waypoint=Instantiate(_waypoint_pf, _raycastHit.point, Quaternion.identity);
                                _raysStarted=true;
                            }
                            Debug.DrawRay(PlayerCamera.transform.position,_raycastHit.point-PlayerCamera.transform.position,Color.red,0.0f,true);
                            if (_waypoint!=null){
                                _waypoint.transform.position=_raycastHit.point;
                                _waypoint.transform.rotation=Quaternion.FromToRotation(Vector3.up, _raycastHit.normal);
                                if (Stair!=null){
                                    if ((Stair.transform.position-_raycastHit.point).magnitude<=4.0f){
                                        _wpRND=_waypoint.GetComponentsInChildren<Renderer>();
                                        foreach(Renderer x in _wpRND){
                                            x.material.SetColor("_Color",Color.green);
                                            x.material.SetColor("_EmissionColor",Color.green);
                                        }
                                        if (Input.GetKeyDown(KeyCode.T)){
                                            _button=Instantiate(_button_pf,_raycastHit.point,Quaternion.FromToRotation(Vector3.up,_raycastHit.normal));
                                        }
                                    }
                                    else{
                                        _wpRND=_waypoint.GetComponentsInChildren<Renderer>();
                                        foreach(Renderer x in _wpRND){
                                            x.material.SetColor("_Color",Color.red);
                                            x.material.SetColor("_EmissionColor",Color.red);
                                        }
                                    }
                                }
                            }
                            else{
                                if (_raysStarted){
                                    Destroy(_waypoint);
                                    _raysStarted=false;
                                }
                            }
                        }*/
                        break;
                    default: break;
                }
            }
        }
    }

    private void Inserisci(AccItemType type){
        switch(type){
            case AccItemType.Ramp:
                device_type=acc_device.Button;
                break;
            case AccItemType.Stairlift:
                device_type=acc_device.Button;
                break;
            case AccItemType.DoorButton: 
                device_type=acc_device.Button;
                break;
            default:break;
        }
    }

    private void InsertMode(Mode mode_input){
        Debug.Log("Sto cercando di entrare in questa modalità");
        switch(mode_input){
            case Mode.Nav:
                _startInsert=false;
                break;
            case Mode.Edit:
                _startInsert=false;
                break;
            case Mode.Plan: //aggiungi device
                _startInsert=true;
                break;
        }
    }
    private void Awake(){
        InventoryBtn.OnItemSelect+=Inserisci;
        InputManager.OnChangeMode+=InsertMode;
    }

    private void OnDestroy(){
        InventoryBtn.OnItemSelect-=Inserisci;
        InputManager.OnChangeMode-=InsertMode;
    }
}
