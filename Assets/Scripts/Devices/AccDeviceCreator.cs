using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
public class AccDeviceCreator : MonoBehaviour
{   
    public enum acc_device {Button, Ramp, Stairlift};
    public Camera PlayerCamera;
    public List<GameObject> Doors;
    public GameObject Level;
    public acc_device device_type = acc_device.Button;
    private float _range = 100f;
    private Dictionary<string, Color[]> _originalMats = null;
    private GameObject _selected;
    private RaycastHit _raycastHit;
    private GameObject _waypoint_pf;
    private GameObject _waypoint;
    private GameObject _button_pf;
    private GameObject _ramp_pf;
    private GameObject _button;
    private Renderer[] _wpRND;
    private bool _raysStarted=false;
    private bool _startInsert=false;
    private GameObject _doorClosest;
    private Vector2 _localTranslation; // accessibility device
    private float _localRotation; // accessibility device

    #region GESTIONE_REPORT

    public delegate void CreateAccDevice(string accDevice, Vector3 position);
    public static event CreateAccDevice OnDeviceCreation;

    void AccDevicePlaced()
    {
        OnDeviceCreation?.Invoke(device_type.ToString(), _raycastHit.point);
    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        FindDoors(Level);
        Debug.Log("Ho trovato x porte con x = " + Doors.Count);
        _waypoint_pf= (GameObject)Resources.Load("Prefabs/waypoint", typeof(GameObject));
        _button_pf = (GameObject)Resources.Load("Prefabs/Button", typeof(GameObject));
        _ramp_pf = (GameObject)Resources.Load("Prefabs/Adaptable_Ramp", typeof(GameObject));
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
                                _doorClosest=FindClosestDoor(_waypoint.transform);
                                if (_doorClosest!=null){
                                    Debug.Log(_doorClosest.name);
                                    if ((_doorClosest.transform.position-_raycastHit.point).magnitude<=2.5f){
                                        _wpRND=_waypoint.GetComponentsInChildren<Renderer>();
                                        foreach(Renderer x in _wpRND){
                                            x.material.SetColor("_Color",Color.green);
                                            x.material.SetColor("_EmissionColor",Color.green);
                                        }
                                        if (Input.GetKeyDown(KeyCode.Space)){
                                            _button=Instantiate(_button_pf,_raycastHit.point,Quaternion.FromToRotation(Vector3.up,_raycastHit.normal));
                                            Destroy(_doorClosest);
                                            Destroy(_waypoint);
                                            Doors.Clear();
                                            FindDoors(Level);
                                            AccDevicePlaced();
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
                    case acc_device.Ramp:
                        // RAMP MODE
                        if (_raycastHit.transform.name.ToLower().IndexOf("floor") != -1)
                        {
                            if (!_raysStarted)
                            {
                                _ramp_pf = Instantiate(_ramp_pf, _raycastHit.point + _raycastHit.normal * 0.8f, Quaternion.FromToRotation(Vector3.forward, PlayerCamera.transform.forward));
                                _ramp_pf.GetComponent<Rigidbody>().isKinematic = true;
                                foreach (Rigidbody rb in _ramp_pf.GetComponentsInChildren<Rigidbody>())
                                {
                                    rb.isKinematic = true;
                                }
                                if (_originalMats == null)
                                {
                                    _originalMats = new Dictionary<string, Color[]>();
                                }
                                else
                                {
                                    _originalMats.Clear();
                                }
                                foreach (Renderer r in _ramp_pf.GetComponentsInChildren<Renderer>())
                                {
                                    _originalMats.Add(r.gameObject.name, getColors(r.materials));
                                }
                                _raysStarted = true;
                            }
                        
                            Debug.DrawRay(PlayerCamera.transform.position, _ramp_pf.transform.position, Color.red, 0.0f, true);
                            if (_ramp_pf != null)
                            {
                                Vector3 pos1, pos2;
                                RaycastHit hit1, hit2;
                                float slope;

                                pos1 = _ramp_pf.transform.GetChild(1).position; // Rampa_salita, più lontana
                                pos2 = _ramp_pf.transform.GetChild(2).position; // Rampa_discesa, più vicina
                                if(Physics.Raycast(pos1, -Vector3.up, out hit1, 1f) &&
                                    Physics.Raycast(pos2, -Vector3.up, out hit2, 1f))
                                {
                                    slope = Mathf.Abs(hit1.point.y - hit2.point.y) / (hit1.point.z - hit2.point.z);
                                } else
                                {
                                    slope = 100f;
                                }

                                if (slope < 0.10)
                                {
                                    Debug.Log(_doorClosest.name);

                                    _wpRND = _ramp_pf.GetComponentsInChildren<Renderer>();
                                    foreach (Renderer x in _wpRND)
                                    {
                                        for (int i = 0; i < x.materials.Length; i++)
                                        {
                                            x.materials[i].SetColor("_Color", Color.green);
                                            x.materials[i].SetColor("_EmissionColor", Color.green);
                                        }
                                    }
                                    if (Input.GetButtonDown("Space"))
                                    {
                                        foreach (Renderer x in _wpRND)
                                        {
                                            for(int i = 0; i < x.materials.Length; i++)
                                            {
                                                Material m = x.materials[i];
                                                m.SetColor("_Color", _originalMats[x.gameObject.name][i]);
                                                m.SetColor("_EmissionColor", Color.clear);
                                            }
                                        }
                                        _ramp_pf.GetComponent<Rigidbody>().isKinematic = false;
                                        foreach (Rigidbody rb in _ramp_pf.GetComponentsInChildren<Rigidbody>())
                                        {
                                            rb.isKinematic = false;
                                        }

                                        AccDevicePlaced();
                                    }

                                }
                                else
                                {
                                    _wpRND = _ramp_pf.GetComponentsInChildren<Renderer>();
                                    foreach (Renderer x in _wpRND)
                                    {
                                        foreach (Material m in x.materials)
                                        {
                                            m.SetColor("_Color", Color.red);
                                            m.SetColor("_EmissionColor", Color.red);
                                        }
                                    }
                                }

                            }
                            else
                            {
                                if (_raysStarted)
                                {
                                    Destroy(_waypoint);
                                    _raysStarted = false;
                                }
                            }
                        }

                        break;
                    default: break;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        switch (device_type)
        {
            case acc_device.Ramp:
                if(_ramp_pf != null)
                {
                    if(_localTranslation.y > 0.5)
                    {
                        _ramp_pf.transform.Translate(0f, 0f, 0.1f);
                    }
                    if (_localTranslation.y < 0.5)
                    {
                        _ramp_pf.transform.Translate(0f, 0f, -0.1f);
                    }
                    if (_localTranslation.x > 0.5)
                    {
                        _ramp_pf.transform.Translate(0.1f, 0f, 0f);
                    }
                    if (_localTranslation.x < 0.5)
                    {
                        _ramp_pf.transform.Translate(-0.1f, 0f, 0f);
                    }
                    if(_localRotation > 0.5)
                    {
                        _ramp_pf.transform.Rotate(0f, 1f, 0f);
                    }
                    if (_localRotation < 0.5)
                    {
                        _ramp_pf.transform.Rotate(0f, -1f, 0f);
                    }
                }
                break;
            case acc_device.Button: case acc_device.Stairlift: default: break;
        }
    }

    private Color[] getColors(Material[] mats)
    {
        Color[] colors = new Color[mats.Length];

        for(int i = 0; i < mats.Length; i++)
        {
            colors[i] = mats[i].GetColor("_Color");
        }

        return colors;
    }

    private void FindDoors(GameObject go){
        if (go.name.ToLower().IndexOf("door")!=-1){
            Doors.Add(go);
        }
        if (go.transform.childCount>0){
            foreach (Transform child in go.transform){
                FindDoors(child.gameObject);
            }
        }
    }

    private GameObject FindClosestDoor(Transform wp){
        float max=10000.0f;
        GameObject closest=new GameObject();//
        foreach(GameObject x in Doors){
            float dist=(wp.position-x.transform.position).magnitude;
            if (dist<max){
                closest=x;
                max=dist;
            }
        }
        return closest;
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

    private void ApplyFurniTransl(Vector2 delta)
    {
        _localTranslation = delta;
    }
    private void ApplyFurniRot(float delta)
    {
        _localRotation = delta;
    }

    private void Awake(){
        InventoryBtn.OnItemSelect+=Inserisci;
        InputManager.OnChangeMode+=InsertMode;
        InputManager.OnFurnitureTranslation += ApplyFurniTransl;
        InputManager.OnFurnitureRotation += ApplyFurniRot;
    }

    private void OnDestroy(){
        InventoryBtn.OnItemSelect-=Inserisci;
        InputManager.OnChangeMode-=InsertMode;
    }
}
