using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotSelection : MonoBehaviour
{
    public struct Mat_key{

        public Mat_key(string obj, string mat){
            obj_name=obj;
            mat_name=mat;
        }
        public string obj_name;
        public string mat_name;
    }
    public Camera PlayerCamera;
    public GameObject Level;

    private float _range = 100f;
    private RaycastHit _raycastHit;     

    private GameObject _avatar_pf;
    private GameObject _avatar;
    private GameObject _waypoint_pf;
    private GameObject _waypoint_cursor;
    private Renderer[] _wpRND;
    private bool _raysStarted=false;
    private Dictionary<Mat_key, Material> _inactive_materials;
    private List<GameObject> _waypointList;
    private int _numWaypoints;

    public delegate void StartNavigation();
    public static event StartNavigation OnEndPointSet;


    
    void Start()
    {
        _waypoint_pf= (GameObject)Resources.Load("Prefabs/waypoint", typeof(GameObject));
        _avatar_pf=(GameObject)Resources.Load("Prefabs/avatar_finale",typeof(GameObject));
        _inactive_materials=new Dictionary<Mat_key,Material>();
        _waypointList=new List<GameObject>();
        _numWaypoints=0;
        /*if (Level!=null){
            MakeWallsTransparent(Level);
        }*/

    }

    // Update is called once per frame
    void Update()
    {   
        
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out _raycastHit,_range))
        {  
            if(!_raysStarted && _raycastHit.transform.name.ToLower().IndexOf("floor")!=-1){
                _waypoint_cursor=Instantiate(_waypoint_pf, _raycastHit.point, Quaternion.identity);
                _raysStarted=true;
            }
            
            Debug.DrawRay(PlayerCamera.transform.position,_raycastHit.point-PlayerCamera.transform.position,Color.red,0.0f,true);
            if (_waypoint_cursor!=null && _raycastHit.transform.name.ToLower().IndexOf("floor")!=-1){
                _waypoint_cursor.transform.position=_raycastHit.point;
            }
            if (Input.GetKeyDown(KeyCode.L) && _waypoint_cursor!=null){
                _waypointList.Add(Instantiate(_waypoint_pf,_raycastHit.point, Quaternion.identity));
                if (_numWaypoints==0){
                    _wpRND=_waypointList[0].GetComponentsInChildren<Renderer>();
                    foreach(Renderer x in _wpRND){
                        x.material.SetColor("_Color",Color.green);
                        x.material.SetColor("_EmissionColor",Color.green);
                    }
                }
                _numWaypoints++;
            }
                
        }
        if (Input.GetKeyDown(KeyCode.Return)){
            if (_numWaypoints<2){
                Debug.Log("Non hai ancora inserito almeno 2 waypoint");
            }
            else{
                _wpRND=_waypointList[_numWaypoints-1].GetComponentsInChildren<Renderer>();
                foreach(Renderer x in _wpRND){
                    x.material.SetColor("_Color",Color.green);
                    x.material.SetColor("_EmissionColor",Color.green);
                }
                Destroy(_waypoint_cursor);
                //ResetWalls(Level);
                _avatar=Instantiate(_avatar_pf, _waypointList[0].transform.position, Quaternion.identity);
                SwitchCamera();
                OnEndPointSet?.Invoke();
                Level.GetComponent<NavMeshCreator>()._waypointList=_waypointList;
                this.enabled=false;
            }
        }
        
    }

    private void SwitchCamera(){
        Camera FirstPerson=_avatar.transform.Find("Camera").gameObject.GetComponent<Camera>();
        _avatar.GetComponent<CharacterMovement>().enabled=true;
        _avatar.GetComponent<FurnitureSelection>().enabled=true;
        _avatar.GetComponent<AccDeviceCreator>().enabled=true;
        _avatar.GetComponent<CharacterMovement>().Target=_waypointList[_waypointList.Count-1];
        PlayerCamera.enabled=false;
        FirstPerson.enabled=true;
    }

    private void MakeWallsTransparent(GameObject go){
        if (go.transform.childCount>0){
            foreach (Transform x in go.transform){
                MakeWallsTransparent(x.gameObject);
            }
        }
        if (go.name.ToLower().IndexOf("wall")!=-1 && go.TryGetComponent(typeof(MeshRenderer),out Component comp)){
            Debug.Log("Rendo trasparente" + go.name);
            MeshRenderer mr = (MeshRenderer) comp;
            Material[] mat_set=mr.materials;
            foreach (Material m in mat_set){
                Debug.Log("material name is " + m.name);
                Material temp = new Material(m.shader);
                temp.CopyPropertiesFromMaterial(m);
                Mat_key k = new Mat_key(go.name,m.name);
                _inactive_materials.Add(k,temp);
                m.SetFloat("_Mode",2);
                Color c=m.GetColor("_Color");
                c.a=0.5f;
                m.SetColor("_Color",c);
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.SetInt("_ZWrite", 0);
                m.DisableKeyword("_ALPHATEST_ON");
                m.EnableKeyword("_ALPHABLEND_ON");
                m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                m.renderQueue = 3000;
            }
        }
    }

   private void ResetWalls(GameObject go){
        if (go.transform.childCount>0){
            foreach (Transform x in go.transform){
                ResetWalls(x.gameObject);
            }
        }
        if (go.name.ToLower().IndexOf("wall")!=-1 && go.TryGetComponent(typeof(MeshRenderer),out Component comp)){
            MeshRenderer mr = (MeshRenderer) comp;
            Material[] mat_set=mr.materials;
            Debug.Log("Resetting materials in " + go.name);
            int i=0;
            foreach (Material m in mat_set){
                Debug.Log("Resetting material " + m.name + " in " + go.name);
                mr.materials[i].CopyPropertiesFromMaterial(_inactive_materials.GetValueOrDefault(new Mat_key(go.name,m.name)));
                i+=1;
                /*m.SetFloat("_Mode",1);
                Color c=m.GetColor("_Color");
                c.a=1f;
                m.SetColor("_Color",c);
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                m.SetInt("_ZWrite", 1);
                m.DisableKeyword("_ALPHATEST_ON");
                m.DisableKeyword("_ALPHABLEND_ON");
                m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                m.renderQueue = -1;*/
            }
        }
    }
}
