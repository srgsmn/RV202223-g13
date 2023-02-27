using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshCreator : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshSurface _nvsHumanoid;
    private NavMeshSurface _nvsWheelchair;
    private string[] _walkableElements={"floor","pavimento","scala","scale","stairs","stair","wall"};
    private string[] _passthroughElements={"porta","door","porte"};
    public List<GameObject> _waypointList;

    private GameObject _myPrefab;
    private GameObject _pathGuyHumanoid;
    private GameObject _pathGuyWheelchair;

    private ParticleSystem _pathParticleHumanoid;
    private ParticleSystem _pathParticleWheelchair;
    private ParticleSystem.EmissionModule _emissionHumanoid;
    private ParticleSystem.EmissionModule _emissionWheelchair;

    private bool _humanoidMoving=false;
    private bool _wheelchairMoving=false;
    private bool _humanoidPathExists=false;
    private bool _wheelchairPathExists=false;
    private bool _HumanoidNavMeshExists=false;
    private bool _WheelchairNavMeshExists=false;
    private NavMeshAgent _humanoidAgent;
    private NavMeshAgent _wheelchairAgent;
    private NavMeshPath _humanoidPath;
    private NavMeshPath _wheelchairPath;
    private int _humanoidPoint=0;
    private int _wheelchairPoint=0;

    private bool _needToStartPath=false;
    private bool _needToUpdateNavMesh=false;
    private bool _needToUpdateNavMeshNoAreas=false;
    private bool _updated1=false;
    private bool _updated2=false;
    void Start()
    {
        _humanoidPath = new NavMeshPath();
        _wheelchairPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {   
        if (_humanoidMoving){
            if (_humanoidPoint!=_waypointList.Count-1){
                if (Vector3.Distance(_pathGuyHumanoid.transform.position,_waypointList[_humanoidPoint].transform.position)<=2.0f){
                    _humanoidPoint++;
                    _humanoidAgent.ResetPath();
                    if(_humanoidAgent.CalculatePath(_waypointList[_humanoidPoint].transform.position,_humanoidPath)){
                        Debug.Log(_humanoidAgent.SetPath(_humanoidPath));
                    }
                    else{
                        _humanoidMoving=false;
                    }
                }
            }
            else if (Vector3.Distance(_pathGuyHumanoid.transform.position,_waypointList[_waypointList.Count-1].transform.position)<=2.0f){
                Debug.Log("Human Arrived");
                _humanoidMoving=false;
            }
        }
        if (_wheelchairMoving){
            if (_wheelchairPoint!=_waypointList.Count-1){
                if (Vector3.Distance(_pathGuyWheelchair.transform.position,_waypointList[_wheelchairPoint].transform.position)<=2.0f){
                    _wheelchairPoint++;
                    _wheelchairAgent.ResetPath();
                    if (_wheelchairAgent.CalculatePath(_waypointList[_wheelchairPoint].transform.position,_wheelchairPath)){
                        Debug.Log(_wheelchairAgent.SetPath(_wheelchairPath));
                    }
                    else{
                        _wheelchairMoving=false;
                    }
                }
            }
            else if (Vector3.Distance(_pathGuyWheelchair.transform.position,_waypointList[_waypointList.Count-1].transform.position)<=2.0f){
                Debug.Log("Wheelchair Arrived");
                _wheelchairMoving=false;
            }
        }
        if (_needToUpdateNavMeshNoAreas){
            if (_HumanoidNavMeshExists){
                //setAreas(this.transform,GetNavMeshAgentID("Humanoid").Value);
                AsyncOperation operationHumanoid=_nvsHumanoid.UpdateNavMesh(_nvsHumanoid.navMeshData);
                operationHumanoid.completed+=HumanoidUpdated;
            }
            if (_WheelchairNavMeshExists){
                //setAreas(this.transform,GetNavMeshAgentID("Wheelchair").Value);
                AsyncOperation operationWheelchair=_nvsWheelchair.UpdateNavMesh(_nvsWheelchair.navMeshData);
                operationWheelchair.completed+=WheelchairUpdated;
            }
            _needToStartPath=true;
            _needToUpdateNavMeshNoAreas=false;
        }
        if (_needToUpdateNavMesh){
            if (_HumanoidNavMeshExists){
                setAreas(this.transform,GetNavMeshAgentID("Humanoid").Value);
                _nvsHumanoid.UpdateNavMesh(_nvsHumanoid.navMeshData);
            }
            if (_WheelchairNavMeshExists){
                setAreas(this.transform,GetNavMeshAgentID("Wheelchair").Value);
                _nvsWheelchair.UpdateNavMesh(_nvsWheelchair.navMeshData);
            }
        }
        if (_updated1 && _updated2){
            _needToStartPath=true;
            _needToUpdateNavMesh=false;
            _updated1=false;
            _updated2=false;
        }
        /*if (Input.GetKeyDown("n")){
            if (_navMeshExists){
                Debug.Log("NavMesh already exists");
            }
            else{
                CreateNavMesh();
            }     
        }*/
        if (_needToStartPath){
            CreateHumanoidGuy();
            _humanoidAgent.radius=0.00001f;
            _humanoidAgent.SetPath(_humanoidPath);
            CreateWheelchairGuy();
            _wheelchairAgent.radius=0.00001f;
            _wheelchairAgent.SetPath(_wheelchairPath);
            _needToStartPath=false;
        }
    }

    public void InitializeNavMesh(){
        if (_HumanoidNavMeshExists){
                Debug.Log("NavMesh already exists");
            }   
            else{
                CreateNavMesh(0);
                _HumanoidNavMeshExists=true;
            }  
            if (_WheelchairNavMeshExists){
                Debug.Log("NavMesh already exists");
            }   
            else{
                CreateNavMesh(1);
                _WheelchairNavMeshExists=true;
            }     
    }

    private void CreateNavMesh(int agentType){
        NavMeshSurface nvs;
        int? typeID=null;
        if (agentType==0){
            _HumanoidNavMeshExists=true;
            typeID=GetNavMeshAgentID("Humanoid");
        }
        else if (agentType==1){
            _WheelchairNavMeshExists=true;
            typeID=GetNavMeshAgentID("Wheelchair");
        }
        nvs=this.gameObject.AddComponent(typeof(NavMeshSurface)) as NavMeshSurface;
        nvs.collectObjects=CollectObjects.Children;
        nvs.defaultArea=1;
        if (typeID!=null){
            nvs.agentTypeID=typeID.Value;
        }
        
        if (typeID!=null){
            setAreas(this.transform, typeID.Value);
        }   
        nvs.overrideVoxelSize=true;
        nvs.voxelSize=0.08f;
        nvs.minRegionArea=0.1f;
        nvs.BuildNavMesh();
        if (agentType==0){
            _nvsHumanoid=nvs;
        }
        else if (agentType==1){
            _nvsWheelchair=nvs;
        }
    }

    private void setAreas(Transform prn,int agentTypeID){
        foreach (Transform child in prn){
            if (child.childCount>0){
                setAreas(child,agentTypeID);
            }
            if (IsWalkable(child.name.ToLower())){
                NavMeshModifier nvm;
                if (!child.gameObject.TryGetComponent(typeof(NavMeshModifier), out Component x)){
                    nvm = child.gameObject.AddComponent(typeof(NavMeshModifier)) as NavMeshModifier;
                }
                else{
                    nvm=(NavMeshModifier)x;
                }
                nvm.overrideArea=true;
                nvm.area=0;
            }
            if (IsPassthrough(child.name.ToLower())){
                if (agentTypeID==GetNavMeshAgentID("Humanoid").Value){
                    //BOH
                    NavMeshModifier nvm;
                    if (!child.gameObject.TryGetComponent(typeof(NavMeshModifier), out Component x)){
                        nvm = child.gameObject.AddComponent(typeof(NavMeshModifier)) as NavMeshModifier;
                    }
                    else{
                        nvm=(NavMeshModifier)x;
                    }
                    nvm.ignoreFromBuild=true; 
                    nvm.SetAffectedAgentType(agentTypeID);
                }
            }
        }

    }
    private void CreateHumanoidGuy(){
        if (_humanoidPathExists){
            _pathParticleHumanoid.Clear();
            Destroy(_pathGuyHumanoid);

            _humanoidPathExists=false;
        }
        _myPrefab = (GameObject)Resources.Load("Prefabs/path_guy_humanoid", typeof(GameObject));
        _pathGuyHumanoid=Instantiate(_myPrefab, _waypointList[0].transform.position, Quaternion.identity);
       

        _pathParticleHumanoid = _pathGuyHumanoid.GetComponentInChildren<ParticleSystem>();
        _emissionHumanoid = _pathParticleHumanoid.emission;
        _emissionHumanoid.enabled = true;

        _humanoidAgent=_pathGuyHumanoid.GetComponent<NavMeshAgent>();
        _humanoidAgent.CalculatePath(_waypointList[1].transform.position,_humanoidPath);
        _humanoidPoint=1;
        _humanoidMoving=true;
        _humanoidPathExists=true;
    }

    private void CreateWheelchairGuy(){
        if (_wheelchairPathExists){
            _pathParticleWheelchair.Clear();
            Destroy(_pathGuyWheelchair);

            _wheelchairPathExists=false;
        }
        

        _myPrefab = (GameObject)Resources.Load("Prefabs/path_guy_wheelchair", typeof(GameObject));
        _pathGuyWheelchair=Instantiate(_myPrefab, _waypointList[0].transform.position, Quaternion.identity);

        _pathParticleWheelchair = _pathGuyWheelchair.GetComponentInChildren<ParticleSystem>();
        _emissionWheelchair = _pathParticleWheelchair.emission;
        _emissionWheelchair.enabled = true;

        _wheelchairAgent=_pathGuyWheelchair.GetComponent<NavMeshAgent>();
        _wheelchairAgent.CalculatePath(_waypointList[1].transform.position,_wheelchairPath);
        _wheelchairPoint=1;
        _wheelchairMoving=true;
        _wheelchairPathExists=true;
    }

    /*private NavMeshPath PathFinder(NavMeshAgent nva){
        NavMeshPath pt_finale = new NavMeshPath();
        NavMeshPath pt = new NavMeshPath();
        NavMeshQueryFilter qf = new NavMeshQueryFilter();
        qf.agentTypeID=nva.agentTypeID;
        bool pathFound;

        pathFound = NavMesh.CalculatePath(_waypointList[0].transform.position,_waypointList[1].transform.position,qf,pt);
        for (int i=1;i<_waypointList.Count-1 && pathFound && pt.status==NavMeshPathStatus.PathComplete;i++){
            pathFound = NavMesh.CalculatePath(_waypointList[i].transform.position,_waypointList[i+1].transform.position,qf,pt);
            if (pathFound){
                pt_finale.
            }
        }
    }*/

    private void HumanoidUpdated(AsyncOperation op){
        Debug.Log("Humanoid has been updated");
        _updated1=true;
    }
    private void WheelchairUpdated(AsyncOperation op){
        Debug.Log("Wheelchair has been updated");  
        _updated2=true;
    }


    private bool IsWalkable(string name){
        foreach (string x in _walkableElements){
            if (name.IndexOf(x)!=-1){
                return true;
            }
        }
        return false;
    }
    private bool IsPassthrough(string name){
        foreach (string x in _passthroughElements){
            if (name.IndexOf(x)!=-1){
                return true;
            }
        }
        return false;
    }

    private int? GetNavMeshAgentID(string name)
    {
        for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
        {
            NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(index: i);
            if (name == NavMesh.GetSettingsNameFromID(agentTypeID: settings.agentTypeID))
            {
                return settings.agentTypeID;
            }
        }
        return null;
    }

    private void StartGuy(){
        _needToStartPath=true;
    }

    private void ToUpdateNavMesh(string pickedFurniture, Vector3 translation, float rotation){
        _needToUpdateNavMeshNoAreas=true;
    }
    private void ToUpdateNavMeshAfterRemoval(string pickedFurniture, Vector3 position){
        _needToUpdateNavMeshNoAreas=true;
    }

    private void AddedDevice(string x,Vector3 k){
        _needToUpdateNavMeshNoAreas=true;
        Debug.Log("Updated");
    }

    private void OnEnable(){
        HotSpotSelection.OnEndPointSet+=StartGuy;
        FurnitureSelection.OnFurnitureTranslation+=ToUpdateNavMesh;
        FurnitureSelection.OnFurnitureRemoval+=ToUpdateNavMeshAfterRemoval;
        AccDeviceCreator.OnDeviceCreation+=AddedDevice;
    }
    private void OnDisable(){
        HotSpotSelection.OnEndPointSet-=StartGuy;
        FurnitureSelection.OnFurnitureTranslation-=ToUpdateNavMesh;
        FurnitureSelection.OnFurnitureRemoval-=ToUpdateNavMeshAfterRemoval;
        AccDeviceCreator.OnDeviceCreation-=AddedDevice;
    }
}
