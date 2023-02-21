using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderAllocator : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform _gt;
    public PhysicMaterial psmt;

    private NavMeshCreator nc;
    private string[] _structElements={"wall","floor", "ceiling","lavandino","roof","stair","scale"};
    void Start()
    {
        _gt = gameObject.transform;
        //NavMeshCreator nc;
        AllocateColliders(_gt,true,false);
        if (TryGetComponent(typeof(NavMeshCreator),out Component x)){
            nc=(NavMeshCreator)x;
            nc.InitializeNavMesh();
        }
        _gt.GetComponentsInChildren<Collider>().ToList().ForEach(x => x.material=psmt);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void AllocateColliders(Transform prn, bool container, bool staticParent){

        //NB: il set di modelli dev'essere all'interno di un empty
        var children = new Transform[prn.childCount];
        var i = 0;
        bool mesh_ex;
        bool isStatic;
        int colliderType=0;
        Rigidbody rb;
        foreach (Transform child in prn){
            children[i]=child;
            i++;
        }
        //Debug.Log("About to detach children of " + prn.name);
        prn.DetachChildren();
        foreach (Transform x in children){
            isStatic=true;
            mesh_ex=x.TryGetComponent(typeof(MeshFilter),out Component mf);
            if (mesh_ex){
                if ((container || !staticParent) && !IsFixed(x.name.ToLower()) && !IsStructural(x.name.ToLower())){
                    rb=x.gameObject.AddComponent<Rigidbody>();
                    rb.isKinematic=true;
                    rb.useGravity=false;
                    isStatic=false;
                }
                colliderType=ChooseColliders(x.gameObject,(MeshFilter) mf);
            }
            if (x.childCount>0){
                AllocateColliders(x,!mesh_ex,isStatic);
            }
            if (mesh_ex){
                //Debug.Log("Adding colliders to " + x.name);
                AddColliders(x.gameObject, colliderType);
            }
            if (!container && mesh_ex && !staticParent){
                x.gameObject.AddComponent<FixedJoint>().connectedBody=prn.gameObject.GetComponent<Rigidbody>();
            }
        }
        foreach (Transform x in children){
            x.SetParent(prn,true);
        }

    }

    private void AddColliders(GameObject x, int colliderType){
        switch(colliderType){
            case 0:
                x.AddComponent<BoxCollider>();
                break;
            case 1:
                x.AddComponent<MeshCollider>().convex=false;
                break;
            case 2:
                x.AddComponent<NonConvexMeshCollider_runtime>().Calculate();
                break;
            default:
                break;
        }
        return;
    }

    private int ChooseColliders(GameObject x, MeshFilter mf){

        Mesh mesh = mf.sharedMesh;
        Vector3 m_SizeM =mesh.bounds.size;
        float boxSize = m_SizeM.x * m_SizeM.y * m_SizeM.z;
        float volume = VolumeOfMesh(mesh);
        if (volume>=0.9*boxSize){
            return 0;
        }
        else if (IsFixed(x.name.ToLower()) || IsStructural(x.name.ToLower())){
            return 1;    
        }
        return 2;
    }

    /*void AllocateColliders(Transform prn, bool container){

        //NB: il set di modelli dev'essere all'interno di un empty
        var children = new Transform[prn.childCount];
        var i = 0;
        bool mesh_ex;
        foreach (Transform child in prn){
            children[i]=child;
            i++;
        }
        Debug.Log("About to detach children of " + prn.name);
        prn.DetachChildren();
        foreach (Transform x in children){
            x.gameObject.AddComponent<Rigidbody>().isKinematic=true;
            if (x.childCount>0){
                AllocateColliders(x,false);
            }
            Debug.Log("Adding colliders to " + x.name);
            mesh_ex=ChooseColliders(x.gameObject);
            if (!container && mesh_ex){
                x.gameObject.AddComponent<FixedJoint>().connectedBody=prn.gameObject.GetComponent<Rigidbody>();
            }
            if (!mesh_ex){
                Destroy(x.gameObject.GetComponent<Rigidbody>());
            }
        }
        foreach (Transform x in children){
            x.SetParent(prn,true);
        }

    }

    private bool ChooseColliders(GameObject x){
        //try box collider
        
        if(x.TryGetComponent(typeof(MeshFilter),out Component mf)){
            MeshFilter meshFilter=(MeshFilter) mf;
            Mesh mesh = meshFilter.sharedMesh;
            Vector3 m_SizeM =mesh.bounds.size;
            float boxSize = m_SizeM.x * m_SizeM.y * m_SizeM.z;
            float volume = VolumeOfMesh(mesh);
            if (volume>=0.9*boxSize){
                x.AddComponent<BoxCollider>();
            }
            else if (IsFixed(x.name.ToLower()) || IsStructural(x.name.ToLower())){
                x.AddComponent<MeshCollider>().convex=false;
                
            }
            else if (volume>=0.9*GetHullVolume(mesh)){
                x.AddComponent<MeshCollider>().convex=true;
            }
            else{
                x.AddComponent<NonConvexMeshCollider_runtime>().Calculate();
            }
            return true;
        }
        return false;
    }*/


    private float VolumeOfMesh(Mesh mesh){
        float volume = 0;
        
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }

    private float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3){
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;

        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    /*private float GetHullVolume(Mesh x){
        var calc = new GK.ConvexHullCalculator();
        var verts = new List<Vector3>();
        var tris = new List<int>();
        var normals = new List<Vector3>();
        var points = new List<Vector3>();
        points=x.vertices.ToList();

        calc.GenerateHull(points, true, ref verts, ref tris, ref normals);
        var mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.SetNormals(normals);
        return VolumeOfMesh(mesh);
    }*/

    private bool IsStructural(string name){
        foreach (string x in _structElements){
            if (name.IndexOf(x)!=-1){
                return true;
            }
        }
        return false;
    }

    private bool IsFixed(string name){
        if (name.IndexOf("fixed")!=-1){
                return true;
            }
        return false;
    }



}
