using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

public class AccDeviceCreator : MonoBehaviour
{   
    public enum acc_device {Button, Ramp, Stairlift};
    public acc_device mode = acc_device.Button;
    public Camera PlayerCamera;

    public enum furniture_type { Generic, Door, Stair }

    private RaycastHit _raycastHit;
    private GameObject _waypoint_pf;
    private GameObject _waypoint;
    private bool _destroy_wp = false;
    private Renderer[] _wpRND;
    private float _range = 1000f;
    private bool _raysStarted=false;
    private bool _startInsert=false;
    private bool _spacePressed = false;

    // Button variables
    public List<GameObject> Doors;
    private GameObject _button_pf;
    private GameObject _button;

    // Ramp variables
    private GameObject Ramp = null;
    private GameObject _ramp_pf;

    // Stairlift variables     
    public List<GameObject> Stairs;
    private GameObject _stairlift_pf;
    private GameObject _stairlift;
    private Vector3 _linkStartPosition;
    private Vector3 _linkEndPosition;
    private bool _linkStartSet;
    private NavMeshLink _nvl;

    private Vector2 _localTranslation; // accessibility device
    private float _localRotation; // accessibility device

    #region GESTIONE_REPORT

    public delegate void CreateAccDevice(string accDevice, Vector3 position);
    public static event CreateAccDevice OnDeviceCreation;

    void AccDevicePlaced()
    {
        OnDeviceCreation?.Invoke(mode.ToString(), _raycastHit.point);
    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //FindDoors(Level);
        Doors = _findObjectsOfType("door");
        Stairs = _findObjectsOfType("stair");
        Debug.Log("Ho trovato " + Doors.Count + " porte e " + Stairs.Count + " scale");
        _button_pf = (GameObject)Resources.Load("Prefabs/Button", typeof(GameObject));
        _ramp_pf = (GameObject)Resources.Load("Prefabs/Adaptable_Ramp", typeof(GameObject));
        _waypoint_pf= (GameObject)Resources.Load("Prefabs/waypoint", typeof(GameObject));
        _stairlift_pf = (GameObject) Resources.Load("Prefabs/Stairlift_raw", typeof(GameObject));
        _waypoint = Instantiate(_waypoint_pf);
        _waypoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int _doorClosest = -1;
        int _stairClosest = -1;

        if (_startInsert){
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out _raycastHit, _range))
            {
                switch (mode)
                {
                    case acc_device.Button:
                        //BUTTON MODE
                        if (isOfType(_raycastHit.transform.gameObject, "wall"))
                        {
                            if (!_raysStarted)
                            {
                                //_waypoint=Instantiate(_waypoint_pf, _raycastHit.point, Quaternion.identity);
                                _waypoint.SetActive(true);
                                _waypoint.transform.SetPositionAndRotation(_raycastHit.point, Quaternion.identity);
                                _raysStarted = true;
                            }
                            Debug.DrawRay(PlayerCamera.transform.position, _raycastHit.point - PlayerCamera.transform.position, Color.red, 0.0f, true);
                            //if (_waypoint!=null){
                            if (_waypoint.activeInHierarchy)
                            {
                                //_waypoint.transform.position=_raycastHit.point;
                                //_waypoint.transform.rotation=Quaternion.FromToRotation(Vector3.up, _raycastHit.normal);
                                _waypoint.transform.SetPositionAndRotation(_raycastHit.point, Quaternion.FromToRotation(Vector3.up, _raycastHit.normal));
                                _doorClosest = _findClosest(Doors, _raycastHit.point);
                                Debug.Log(Doors[_doorClosest].name);
                                if ((Doors[_doorClosest].transform.position - _raycastHit.point).magnitude <= 2.5f)
                                {
                                    _wpRND = _waypoint.GetComponentsInChildren<Renderer>();
                                    foreach (Renderer x in _wpRND)
                                    {
                                        x.material.SetColor("_Color", Color.green);
                                        x.material.SetColor("_EmissionColor", Color.green);
                                    }
                                    if (_spacePressed)
                                    {
                                        _spacePressed=false;
                                        _button=Instantiate(_button_pf, _raycastHit.point, Quaternion.FromToRotation(Vector3.up, _raycastHit.normal));
                                        _button.GetComponent<ButtonController>().ConnectedDoor=Doors[_doorClosest];
                                        //Destroy(_waypoint);
                                        //Doors.Clear();
                                        //FindDoors(Level);
                                        //Doors.RemoveAt(_doorClosest);
                                        _waypoint.SetActive(false);

                                        AccDevicePlaced();
                                    }
                                }
                                else
                                {
                                    _wpRND = _waypoint.GetComponentsInChildren<Renderer>();
                                    foreach (Renderer x in _wpRND)
                                    {
                                        x.material.SetColor("_Color", Color.red);
                                        x.material.SetColor("_EmissionColor", Color.red);
                                    }
                                }
                            }
                            else
                            {
                                if (_raysStarted)
                                {
                                    //Destroy(_waypoint);
                                    _waypoint.SetActive(false);
                                    _raysStarted = false;
                                }
                            }
                        }
                        break;
                    case acc_device.Stairlift://STAIRLIFT MODE
                        if (isOfType(_raycastHit.transform.gameObject, "floor"))
                        {
                            if (!_raysStarted)
                            {
                                //_waypoint=Instantiate(_waypoint_pf, _raycastHit.point, Quaternion.identity);
                                _waypoint.SetActive(true);
                                _waypoint.transform.SetPositionAndRotation(_raycastHit.point, Quaternion.identity);
                                _raysStarted = true;
                            }
                            Debug.DrawRay(PlayerCamera.transform.position, _raycastHit.point - PlayerCamera.transform.position, Color.red, 0.0f, true);
                            if (_waypoint.activeInHierarchy)
                            {
                                //_waypoint.transform.position = _raycastHit.point;
                                //_waypoint.transform.rotation = Quaternion.FromToRotation(Vector3.up, _raycastHit.normal);
                                _waypoint.transform.SetPositionAndRotation(_raycastHit.point, Quaternion.FromToRotation(Vector3.up, _raycastHit.normal));
                                _stairClosest = _findClosest(Stairs, _raycastHit.point);
                                if ((Stairs[_stairClosest].transform.position - _raycastHit.point).magnitude <= 5f)
                                {
                                    _wpRND = _waypoint.GetComponentsInChildren<Renderer>();
                                    foreach (Renderer x in _wpRND)
                                    {
                                        x.material.SetColor("_Color", Color.green);
                                        x.material.SetColor("_EmissionColor", Color.green);
                                    }
                                    if (_spacePressed)
                                    {
                                        _spacePressed=false;
                                        if (!_linkStartSet)
                                        {
                                            // set start position
                                            _linkStartPosition = _raycastHit.point;
                                            _linkStartSet = true;
                                            _stairlift = Instantiate(_stairlift_pf, _raycastHit.point, Quaternion.FromToRotation(Vector3.up, _raycastHit.normal));

                                            // switch camera
                                            //Camera.main.transform.SetPositionAndRotation(PlayerCamera.transform.position, 
                                                //Quaternion.FromToRotation(Camera.main.transform.forward, PlayerCamera.transform.forward));
                                            //PlayerCamera.enabled = false;
                                            //Camera.main.enabled = true;

                                            // trigger event
                                            AccDevicePlaced();
                                        }
                                        else
                                        {
                                            // set end position
                                            _linkEndPosition = _raycastHit.point;
                                            _linkStartSet = false;

                                            // set stairlift variables
                                            _stairlift.GetComponent<StairliftController>().SetStart(_linkStartPosition);
                                            _stairlift.GetComponent<StairliftController>().SetEnd(_linkEndPosition);
                                            _stairlift.GetComponent<StairliftController>().SetPlayer(this.gameObject);

                                            // switch camera
                                            //Camera.main.enabled = false;
                                            //PlayerCamera.enabled = true;

                                            // resolve selected stair
                                            CreateNavMeshLink(Stairs[_stairClosest]);
                                            Stairs.RemoveAt(_stairClosest);
                                            //Destroy(_waypoint);
                                        }
                                    }
                                }
                                else
                                {
                                    _wpRND = _waypoint.GetComponentsInChildren<Renderer>();
                                    foreach (Renderer x in _wpRND)
                                    {
                                        x.material.SetColor("_Color", Color.red);
                                        x.material.SetColor("_EmissionColor", Color.red);
                                    }
                                }
                            }
                            else
                            {
                                if (_raysStarted)
                                {
                                    //Destroy(_waypoint);
                                    _waypoint.SetActive(false);
                                    _raysStarted = false;
                                }
                            }
                        }
                        break;
                    case acc_device.Ramp:
                        // RAMP MODE
                        if (isOfType(_raycastHit.transform.gameObject, "floor"))
                        {
                            if (!_raysStarted)
                            {
                                //_waypoint = Instantiate(_waypoint_pf, _raycastHit.point, Quaternion.identity);
                                _waypoint.SetActive(true);
                                _waypoint.transform.SetPositionAndRotation(_raycastHit.point, Quaternion.identity);
                                _raysStarted = true;
                            }
                            Debug.DrawRay(PlayerCamera.transform.position, _raycastHit.point - PlayerCamera.transform.position, Color.red, 0.0f, true);
                            if (_waypoint.activeInHierarchy)
                            {
                                //_waypoint.transform.position = _raycastHit.point;
                                //_waypoint.transform.rotation = Quaternion.FromToRotation(Vector3.up, _raycastHit.normal);
                                Vector3 pos1, pos2;
                                RaycastHit hit1, hit2;

                                _waypoint.transform.SetPositionAndRotation(_raycastHit.point, Quaternion.FromToRotation(Vector3.up, _raycastHit.normal));
                                pos1 = _raycastHit.point + Vector3.up * 0.5f + PlayerCamera.transform.forward * 3.1404f; // posizione rampa_salita
                                pos2 = _raycastHit.point + Vector3.up * 0.5f - PlayerCamera.transform.forward * 3.1404f; // posizione rampa_discesa
                                Physics.Raycast(pos1, Vector3.down, out hit1, 10f);
                                Physics.Raycast(pos2, Vector3.down, out hit2, 10f);
                                if (Mathf.Abs(hit1.point.z - hit2.point.z) / 6.2808f <= 0.10f) // calcolo della pendenza
                                {
                                    _wpRND = _waypoint.GetComponentsInChildren<Renderer>();
                                    foreach (Renderer x in _wpRND)
                                    {
                                        x.material.SetColor("_Color", Color.green);
                                        x.material.SetColor("_EmissionColor", Color.green);
                                    }
                                    if (Input.GetKeyDown(KeyCode.Space))
                                    {
                                        Ramp = Instantiate(_ramp_pf, _raycastHit.point + Vector3.up * 0.5f, Quaternion.FromToRotation(Vector3.up, _raycastHit.normal));
                                        Ramp.GetComponent<Rigidbody>().isKinematic = true;
                                        foreach (Rigidbody rb in Ramp.GetComponentsInChildren<Rigidbody>())
                                        {
                                            rb.isKinematic = true;
                                        }
                                        _spacePressed = false;
                                        //Destroy(_waypoint);
                                        _waypoint.SetActive(false);
                                        AccDevicePlaced();
                                    }
                                }
                                else
                                {
                                    _wpRND = _waypoint.GetComponentsInChildren<Renderer>();
                                    foreach (Renderer x in _wpRND)
                                    {
                                        x.material.SetColor("_Color", Color.red);
                                        x.material.SetColor("_EmissionColor", Color.red);
                                    }
                                }
                            }
                            else
                            {
                                if (_raysStarted)
                                {
                                    //Destroy(_waypoint);
                                    _waypoint.SetActive(false);
                                    _raysStarted = false;
                                }
                            }
                        }
                        break;
                    default: break;
                }
            }
        }

        if(_destroy_wp && _waypoint.activeInHierarchy)
        {
            _waypoint.SetActive(false);
            _destroy_wp = false;
        }
    }

    private void FixedUpdate()
    {
        switch (mode)
        {
            case acc_device.Ramp:
                if(Ramp != null)
                {
                    if(_localTranslation.y > 0.5)
                    {
                        Ramp.transform.Translate(0f, 0f, 0.1f);
                    }
                    if (_localTranslation.y < 0.5)
                    {
                        Ramp.transform.Translate(0f, 0f, -0.1f);
                    }
                    if (_localTranslation.x > 0.5)
                    {
                        Ramp.transform.Translate(0.1f, 0f, 0f);
                    }
                    if (_localTranslation.x < 0.5)
                    {
                        Ramp.transform.Translate(-0.1f, 0f, 0f);
                    }
                    if(_localRotation > 0.5)
                    {
                        Ramp.transform.Rotate(0f, 1f, 0f);
                    }
                    if (_localRotation < 0.5)
                    {
                        Ramp.transform.Rotate(0f, -1f, 0f);
                    }
                    if (_spacePressed)
                    {
                        Ramp.GetComponent<Rigidbody>().isKinematic = false;
                        foreach(Rigidbody rb in Ramp.GetComponentsInChildren<Rigidbody>())
                        {
                            rb.isKinematic = false;
                        }
                        Ramp = null;
                        _spacePressed = false;
                    }
                }
                break;
            case acc_device.Button: case acc_device.Stairlift: default: break;
        }
    }

    private void CreateNavMeshLink(GameObject Stair)
    {
        GameObject _linkGo = new GameObject(Stair.name + "_link");
        _linkGo.transform.parent = Stair.transform;
        _linkGo.transform.position = Stair.GetComponent<Renderer>().bounds.center;
        _nvl = _linkGo.AddComponent(typeof(NavMeshLink)) as NavMeshLink;
        _nvl.agentTypeID = GetNavMeshAgentID("Wheelchair").Value;
        _nvl.autoUpdate = false;
        _nvl.width = GetNavMeshAgentWidth(_nvl.agentTypeID) + 1.5f;
        _nvl.startPoint = _linkStartPosition - _linkGo.transform.position;
        _nvl.endPoint = _linkEndPosition - _linkGo.transform.position;
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

    private float GetNavMeshAgentWidth(int agentTypeID)
    {
        NavMeshBuildSettings settings = NavMesh.GetSettingsByID(agentTypeID);
        return settings.agentRadius;
    }

    private bool isOfType(GameObject go, string type)
    {
        return (go.name.ToLower().IndexOf(type) != -1);
    }

    private List<GameObject> _findObjectsOfType(string type)
    {
        List<GameObject> go_list = new List<GameObject>();

        foreach(GameObject g in gameObject.scene.GetRootGameObjects())
        {
            find_r(g, go_list, type);
        }

        return go_list;
    }

    private void find_r(GameObject go, List<GameObject> list, string type)
    {
        if (isOfType(go, type))
        {
            list.Add(go);
        }
        if (go.transform.childCount > 0)
        {
            foreach (Transform child in go.transform)
            {
                find_r(child.gameObject, list, type);
            }
        }
    }

    private int _findClosest(List<GameObject> list, Vector3 position)
    {
        float max = Mathf.Infinity;
        int closest = -1;
        for (int i = 0; i < list.Count; i++)
        {
            float dist = (position - list[i].transform.position).magnitude;
            if (dist < max)
            {
                closest = i;
                max = dist;
            }
        }

        return closest;
    }

    private void Inserisci(AccItemType type){
        switch(type){
            case AccItemType.Ramp:
                mode=acc_device.Ramp;
                break;
            case AccItemType.Stairlift:
                mode=acc_device.Stairlift;
                break;
            case AccItemType.DoorButton: 
                mode=acc_device.Button;
                break;
            default:break;
        }
    }

    private void InsertMode(Mode mode_input){
        switch(mode_input){
            case Mode.EPSelector:
                _startInsert=false;
                break;
            case Mode.Nav:
                _startInsert=false;
                break;
            case Mode.Edit:
                _startInsert=false;
                break;
            case Mode.Plan: //aggiungi device
                _startInsert=true;
                _destroy_wp=true;
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
    private void PressSpace()
    {
        if (_startInsert){
            _spacePressed = true;
        }
        
    }

    private void OnEnable(){
        InventoryBtn.OnItemSelect+=Inserisci;
        InputManager.OnChangeMode+=InsertMode;
        InputManager.OnFurnitureTranslation += ApplyFurniTransl;
        InputManager.OnFurnitureRotation += ApplyFurniRot;
        InputManager.OnSelection += PressSpace;
    }

    private void OnDisable(){
        InventoryBtn.OnItemSelect-=Inserisci;
        InputManager.OnChangeMode-=InsertMode;
        InputManager.OnFurnitureTranslation -= ApplyFurniTransl;
        InputManager.OnFurnitureRotation -= ApplyFurniRot;
        InputManager.OnSelection -= PressSpace;
    }
}
