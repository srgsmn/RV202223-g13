using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

public class AccDeviceCreator : MonoBehaviour
{   
    public enum acc_device {None, Button, Ramp, Stairlift};
    public acc_device mode = acc_device.None;
    public Camera PlayerCamera;
    public GameObject Level;
    public enum furniture_type { Generic, Door, Stair }

    private RaycastHit _raycastHit;
    private GameObject _waypoint_pf;
    private GameObject _waypoint;
    private bool _destroy_wp = false;
    private bool _destroy_ramp = false;
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
    private GameObject _ramp = null;
    private GameObject _ramp_pf;
    private bool _placed_ramp = false;

    // Stairlift variables     
    public List<GameObject> Stairs;
    private GameObject _stairlift_pf;
    private GameObject _stairlift;
    private Vector3 _linkStartPosition;
    private Vector3 _linkEndPosition;
    private bool _linkStartSet;
    private NavMeshLink _nvl;
    private Vector3 _cameraOffset;

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
        _button_pf = (GameObject)Resources.Load("Prefabs/bottone", typeof(GameObject));
        _ramp_pf = (GameObject)Resources.Load("Prefabs/Adaptable_Ramp", typeof(GameObject));
        _waypoint_pf= (GameObject)Resources.Load("Prefabs/waypoint", typeof(GameObject));
        _stairlift_pf = (GameObject) Resources.Load("Prefabs/Stairlift_raw", typeof(GameObject));
        _waypoint = Instantiate(_waypoint_pf);
        _waypoint.SetActive(false);
    }

    
    /*void ActivateFLCam(bool f)
    {
        if (f) {
            _cameraOffset = PlayerCamera.transform.localPosition;
            gameObject.GetComponent<CharacterMovement>().enabled = false;
            PlayerCamera.GetComponent<FollowPlayer>().enabled = false;
            PlayerCamera.GetComponent<FreeLookCamera>().enabled = true;
            
        } else
        {
            PlayerCamera.transform.SetLocalPositionAndRotation(_cameraOffset, Quaternion.FromToRotation(PlayerCamera.transform.forward, gameObject.transform.forward));
            gameObject.GetComponent<CharacterMovement>().enabled = true;
            PlayerCamera.GetComponent<FollowPlayer>().enabled = true;
            PlayerCamera.GetComponent<FreeLookCamera>().enabled = false;
            
        }
    }*/

    private void ActivateFLCam(bool f)
    {
        Camera FreeLook = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        Camera FirstPerson = gameObject.GetComponentInChildren<Camera>();

        if (f)
        {
            gameObject.GetComponent<CharacterMovement>().enabled = false;
            FirstPerson.GetComponent<FollowPlayer>().enabled = false;
            FreeLook.GetComponent<FreeLookCamera>().enabled = true;
            FreeLook.transform.SetPositionAndRotation(FirstPerson.transform.position, FirstPerson.transform.rotation);
            FirstPerson.gameObject.GetComponent<AudioListener>().enabled = false;
            FreeLook.gameObject.GetComponent<AudioListener>().enabled = true;
            FirstPerson.enabled = false;
            FreeLook.enabled = true;
            PlayerCamera = FreeLook;
        }
        else
        {
            gameObject.GetComponent<CharacterMovement>().enabled = true;
            FirstPerson.GetComponent<FollowPlayer>().enabled = true;
            FreeLook.GetComponent<FreeLookCamera>().enabled = false;
            FirstPerson.gameObject.GetComponent<AudioListener>().enabled = true;
            FreeLook.gameObject.GetComponent<AudioListener>().enabled = false;
            FreeLook.enabled = false;
            FirstPerson.enabled = true;
            PlayerCamera = FirstPerson;
        }

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
                    case acc_device.None: break;
                    case acc_device.Button:
                        //BUTTON MODE
                        if (isOfType(_raycastHit.transform.gameObject, "wall"))
                        {
                            if (!_raysStarted)
                            {
                                _waypoint.SetActive(true);
                                _waypoint.transform.SetPositionAndRotation(_raycastHit.point, Quaternion.identity);
                                _raysStarted = true;
                            }
                            Debug.DrawRay(PlayerCamera.transform.position, _raycastHit.point - PlayerCamera.transform.position, Color.red, 0.0f, true);
                            if (_waypoint.activeInHierarchy)
                            {
                                _waypoint.transform.SetPositionAndRotation(_raycastHit.point, Quaternion.FromToRotation(Vector3.up, _raycastHit.normal));
                                _doorClosest = _findClosest(Doors, _raycastHit.point);
                                if ((Doors[_doorClosest].transform.position - _raycastHit.point).magnitude <= 2.5f)
                                {
                                    _wpRND = _waypoint.GetComponentsInChildren<Renderer>();
                                    foreach (Renderer x in _wpRND)
                                    {
                                        x.material.SetColor("_Color", Color.green);
                                        x.material.SetColor("_EmissionColor", Color.green);
                                    }
                                    if (Input.GetKeyDown(KeyCode.Space))
                                    {
                                        _button=Instantiate(_button_pf, _raycastHit.point, Quaternion.FromToRotation(Vector3.up, _raycastHit.normal), Level.transform);
                                        _button.GetComponent<ButtonController>().ConnectedDoor=Doors[_doorClosest];
                                        NavMeshModifier nvm;
                                        nvm=Doors[_doorClosest].GetComponent<NavMeshModifier>();
                                        nvm.ignoreFromBuild=true; 
                                        nvm.SetAffectedAgentType(-1);
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
                        }
                        else if (_raysStarted)
                        {
                            //Destroy(_waypoint);
                            _waypoint.SetActive(false);
                            _raysStarted = false;
                        }
                        break;
                    case acc_device.Stairlift://STAIRLIFT MODE
                        if (isOfType(_raycastHit.transform.gameObject, "floor"))
                        {
                            if (!_raysStarted)
                            {
                                _waypoint.SetActive(true);
                                _waypoint.transform.SetPositionAndRotation(_raycastHit.point, Quaternion.identity);
                                _raysStarted = true;
                            }
                            Debug.DrawRay(PlayerCamera.transform.position, _raycastHit.point - PlayerCamera.transform.position, Color.red, 0.0f, true);
                            if (_waypoint.activeInHierarchy)
                            {
                                _waypoint.transform.SetPositionAndRotation(_raycastHit.point, Quaternion.FromToRotation(Vector3.up, _raycastHit.normal));
                                _stairClosest = _findClosest(Stairs, _raycastHit.point);
                                if ((Stairs[_stairClosest].transform.position - _raycastHit.point).magnitude <= 8f)
                                {
                                    _wpRND = _waypoint.GetComponentsInChildren<Renderer>();
                                    foreach (Renderer x in _wpRND)
                                    {
                                        x.material.SetColor("_Color", Color.green);
                                        x.material.SetColor("_EmissionColor", Color.green);
                                    }
                                    if (Input.GetKeyDown(KeyCode.Space))
                                    {
                                        if (!_linkStartSet)
                                        {
                                            // set start position
                                            _linkStartPosition = _raycastHit.point;
                                            _linkStartSet = true;
                                            _stairlift = Instantiate(_stairlift_pf, _raycastHit.point, Quaternion.FromToRotation(Vector3.up, _raycastHit.normal), Level.transform);

                                            // switch camera
                                            ActivateFLCam(true);

                                            
                                        }
                                        else
                                        {
                                            // set end position
                                            _linkEndPosition = _raycastHit.point;
                                            _linkStartSet = false;

                                            // set stairlift variables
                                            _stairlift.GetComponent<StairliftController>().SetStart(_linkStartPosition);
                                            _stairlift.GetComponent<StairliftController>().SetEnd(_linkEndPosition);
                                            _stairlift.GetComponent<StairliftController>().SetPlayer(gameObject);

                                            // switch camera
                                            ActivateFLCam(false);

                                            // resolve selected stair
                                            CreateNavMeshLink(Stairs[_stairClosest]);
                                            Stairs.RemoveAt(_stairClosest);
                                            // trigger event
                                            AccDevicePlaced();
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
                            
                        } else if (_raysStarted)
                        {
                            _waypoint.SetActive(false);
                            _raysStarted = false;
                        }
                        if (_linkStartSet)
                        {
                            if (Input.GetKey(KeyCode.U))
                                _stairlift.transform.Rotate(0, 1, 0);
                            else if (Input.GetKey(KeyCode.O))
                                _stairlift.transform.Rotate(0, -1, 0);
                        }
                        break;
                    case acc_device.Ramp:
                        // RAMP MODE

                        if (isOfType(_raycastHit.transform.gameObject, "floor"))
                        {
                            if (!_waypoint.activeInHierarchy && !_raysStarted)
                            {
                                Debug.Log("Creo Rampa in " + _raycastHit.point);
                                _waypoint.SetActive(true);
                                _waypoint.transform.SetPositionAndRotation(_raycastHit.point, Quaternion.identity);
                                if (!_placed_ramp)
                                {
                                    _ramp = Instantiate(_ramp_pf, _raycastHit.point + _raycastHit.normal * 0.5f, Quaternion.FromToRotation(Vector3.up, _raycastHit.normal), Level.transform);
                                    _placed_ramp = true;
                                }
                                _raysStarted = true;
                            }
                            Debug.DrawRay(PlayerCamera.transform.position, _raycastHit.point - PlayerCamera.transform.position, Color.red, 0.0f, true);
                            if (_waypoint.activeInHierarchy)
                            {
                                Vector3 pos1, pos2;
                                RaycastHit hit1, hit2;

                                Debug.Log("Sposto rampa in " + _raycastHit.point);
                                _waypoint.transform.SetPositionAndRotation(_raycastHit.point, Quaternion.FromToRotation(Vector3.up, _raycastHit.normal));
                                _ramp.transform.position = _raycastHit.point + _raycastHit.normal * 0.5f;
                                if(Input.GetKey(KeyCode.U))
                                {
                                    _ramp.transform.Rotate(0, 1, 0);
                                } else if (Input.GetKey(KeyCode.O))
                                {
                                    _ramp.transform.Rotate(0, -1, 0);
                                }
                                pos1 = _ramp.GetComponent<RampController>().RampGetOn.transform.position; // posizione rampa_salita
                                pos2 = _ramp.GetComponent<RampController>().RampGetOff.transform.position; // posizione rampa_discesa
                                Physics.Raycast(pos1, Vector3.down, out hit1, 1f);
                                Physics.Raycast(pos2, Vector3.down, out hit2, 1f);
                                float f = Mathf.Abs((hit1.point.y - hit2.point.y) / asVector2(pos1 - pos2).magnitude);
                                Debug.Log("Pendenza: " + f);
                                if (f <= 0.10f) // calcolo della pendenza
                                {
                                    Debug.Log("Piazzata rampa");
                                    foreach (Renderer x in _waypoint.GetComponentsInChildren<Renderer>())
                                    {
                                        x.material.SetColor("_Color", Color.green);
                                        x.material.SetColor("_EmissionColor", Color.green);
                                    }
                                    if (Input.GetKeyDown(KeyCode.Space))
                                    {
                                        _ramp.GetComponent<RampController>().Place();
                                        _waypoint.SetActive(false);
                                        _linkStartPosition=hit1.point;
                                        _linkEndPosition=hit2.point;
                                        CreateNavMeshLink(_ramp);
                                        AccDevicePlaced();
                                        _placed_ramp = false;
                                        _ramp = null;
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
                        }
                        else if (_raysStarted)
                        {
                            //Destroy(_waypoint);
                            _waypoint.SetActive(false);
                            _raysStarted = false;
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

        if (_destroy_ramp)
        {
            Destroy(_ramp);
            _destroy_ramp = false;
            _placed_ramp = false;
        }
    }
    Vector2 asVector2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    private void CreateNavMeshLink(GameObject Stair)
    {
        GameObject _linkGo = new GameObject(Stair.name + "_link");
        _linkGo.transform.parent = Stair.transform;
        _linkGo.transform.position = Stair.transform.position;
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
                _destroy_ramp=false;
                _placed_ramp=false;
                break;
            case AccItemType.Stairlift:
                _destroy_ramp=true;
                mode=acc_device.Stairlift;
                break;
            case AccItemType.DoorButton: 
                _destroy_ramp=true;
                mode=acc_device.Button;
                break;
            default:break;
        }
    }

    private void InsertMode(Mode mode_input){
        switch(mode_input){
            case Mode.EPSelector:
                _startInsert=false;
                _destroy_wp = true;
                break;
            case Mode.Nav:
                _destroy_ramp = true;
                _startInsert=false;
                _destroy_wp = true;
                break;
            case Mode.Edit:
                _destroy_ramp = true;
                _startInsert=false;
                _destroy_wp=true;
                break;
            case Mode.Plan: //aggiungi device
                _startInsert=true;
                mode = acc_device.None;
                break;
        }
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
    }

    private void OnDisable(){
        InventoryBtn.OnItemSelect-=Inserisci;
        InputManager.OnChangeMode-=InsertMode;
    }
}
