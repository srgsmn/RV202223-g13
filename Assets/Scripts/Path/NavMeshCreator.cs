using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshCreator : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshSurface _nvs;

    private string[] _walkableElements={"floor","pavimento","scala","scale","stairs","stair"};
    private string[] _passthroughElements={"porta","door","porte"};

    public Transform _startSpot_transform;
    public Transform _target_transform;

    private Vector3 _startSpot_pos;
    private Vector3 _target_pos;

    private GameObject _myPrefab;
    private GameObject _pathGuy;

    private ParticleSystem _pathParticle;
    private ParticleSystem.EmissionModule _emission;

    private bool _agentMoving=false;
    private bool _pathExists=false;

    private bool _navMeshExists=false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (_agentMoving){
            if (_pathGuy.transform.position.x==_target_pos.x && _pathGuy.transform.position.z==_target_pos.z){
                Debug.Log("Arrived");
                _emission.enabled=false;
                _agentMoving=false;
            }
        }
        if (Input.GetKeyDown("k")){
            if (_navMeshExists){
                setAreas(this.transform);
                _nvs.UpdateNavMesh(_nvs.navMeshData);
                //UpdateNavMesh reagisce agli oggetti solo se aggiunti come figli del container
                Debug.Log("Updated");
            }
        }
        if (Input.GetKeyDown("n")){
            if (_navMeshExists){
                Debug.Log("NavMesh already exists");
            }
            else{
                CreateNavMesh();
            }     
        }
        if (Input.GetKeyDown("p")){
            CreatePathGuy(_startSpot_transform.position,_target_transform.position);
        }
    }

    public void CreateNavMesh(){
        _navMeshExists=true;
        _nvs=this.gameObject.AddComponent(typeof(NavMeshSurface)) as NavMeshSurface;
        _nvs.collectObjects=CollectObjects.Children;
        _nvs.defaultArea=1;
        _nvs.overrideVoxelSize=true;
        _nvs.voxelSize=0.1f;

        //Set walkable areas
        setAreas(this.transform);
        

        _nvs.BuildNavMesh();
    }

    public void setAreas(Transform prn){
        foreach (Transform child in prn){
            if (child.childCount>0){
                setAreas(child);
            }
            if (IsWalkable(child.name.ToLower())){
                NavMeshModifier nvm;
                if (!TryGetComponent(typeof(NavMeshModifier), out Component x)){
                    nvm = child.gameObject.AddComponent(typeof(NavMeshModifier)) as NavMeshModifier;
                }
                else{
                    nvm=(NavMeshModifier)x;
                }
                nvm.overrideArea=true;
                nvm.area=0;
            }
            if (IsPassthrough(child.name.ToLower())){
                NavMeshModifier nvm;
                if (!TryGetComponent(typeof(NavMeshModifier), out Component x)){
                    nvm = child.gameObject.AddComponent(typeof(NavMeshModifier)) as NavMeshModifier;
                }
                else{
                    nvm=(NavMeshModifier)x;
                }
                nvm.ignoreFromBuild=true;
            }
        }

    }

    public void CreatePathGuy(Vector3 s, Vector3 t){
        if (_pathExists){
            _pathParticle.Clear();
            Destroy(_pathGuy);
        }
        _target_pos=t;
        _startSpot_pos=s;

        _myPrefab = (GameObject)Resources.Load("Prefabs/path_guy", typeof(GameObject));
        _pathGuy=Instantiate(_myPrefab, _startSpot_pos, Quaternion.identity);

        _pathParticle = _pathGuy.GetComponentInChildren<ParticleSystem>();
        _emission = _pathParticle.emission;
        _emission.enabled = true;

        NavMeshAgent agent=_pathGuy.GetComponent<NavMeshAgent>();
        agent.destination=_target_pos;
        _agentMoving=true;
        _pathExists=true;
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
}
