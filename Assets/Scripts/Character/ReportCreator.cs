using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReportCreator : MonoBehaviour
{    
    
    public struct AccDevicePlacement
    {
        public string accDevice;
        public Vector3 position;
    }

    public struct RotoTranslation
    {
        public Vector3 translation;
        public float rotation;
    }

    public List<string> AllRecords;
    public float TotalDistance;
    public List<Collision> AllCollisions;
    public List<AccDevicePlacement> AllAccDevices;
    public List<string> AllRemoved;
    public Dictionary<string, RotoTranslation> AllTranslations;
    
    // Start is called before the first frame update
    void Awake()
    {
        AllRecords = new List<string>();

        TotalDistance = 0f;
        AllCollisions = new List<Collision>();
        AllAccDevices = new List<AccDevicePlacement>();
        AllRemoved = new List<string>();
        AllTranslations = new Dictionary<string, RotoTranslation>();

        FurnitureSelection.OnFurnitureRemoval += RemoveFurniture;
    }

    private void OnDestroy()
    {
        FurnitureSelection.OnFurnitureRemoval -= RemoveFurniture;
    }

    void AddDistance(float distance)
    {
        TotalDistance += distance;
        //Debug.Log("Distanza percorsa: " + distance + " m");
        //_allRecords.Add(new string("Distanza percorsa: " + distance + " m"));
    }

    void PlayerCollision(Collision collision)
    {
        AllCollisions.Add(collision);
        Debug.Log("Collisione in " + collision.GetContact(0).point + " con " + collision.collider.gameObject.name);
        if (collision.collider.gameObject.TryGetComponent(typeof(MeshFilter),out Component mf)){
            if (collision.collider.gameObject.name.ToLower().IndexOf("floor")==-1){
                AllRecords.Add(new string("Collision in " + collision.GetContact(0).point + " with " + collision.collider.gameObject.name));
            }
        }
        else{
            if (collision.collider.gameObject.transform.parent.name.ToLower().IndexOf("floor")==-1){
                AllRecords.Add(new string("Collision in " + collision.GetContact(0).point + " with '" + collision.collider.gameObject.transform.parent.name + "';"));
            }
        }
        
    }

    void AddTranslation(string pickedFurniture, Vector3 translation, float rotation)
    {
        if (AllTranslations.ContainsKey(pickedFurniture))
        {
            RotoTranslation a = AllTranslations[pickedFurniture];
            a.translation += translation;
            a.rotation += rotation;
            AllTranslations[pickedFurniture] = a;
        } else {
            RotoTranslation a = new RotoTranslation();
            a.translation = translation;
            a.rotation = rotation;
            AllTranslations.Add(pickedFurniture, a); 
        }
        //Debug.Log("Spostato l'oggetto " + pickedFurniture + " di " + translation.magnitude + " m");
        //AllRecords.Add(new string("Spostato l'oggetto " + pickedFurniture + " di " + translation.magnitude + " m" + ", e ruotato di " + rotation + "gradi"));
    }

    void AddAccDevice(string accDevice, Vector3 pos)
    {
        AccDevicePlacement adp;
        adp.accDevice = new string(accDevice);
        adp.position = new Vector3(pos.x, pos.y, pos.z);
        AllAccDevices.Add(adp);
        Debug.Log("Posizionato il dispositivo " + accDevice + " in " + pos.ToString());
        AllRecords.Add(new string("Placed device: '" + accDevice + "', in " + pos.ToString()));
    }

    void RemoveFurniture(string furni, Vector3 pos)
    {
        AllRemoved.Add(furni);
        Debug.Log("Rimosso l'oggetto '" + furni + "', dalla posizione " + pos);
        AllRecords.Add(new string("Removed piece of furniture: '" + furni + "', from position " + pos));
    }

    void WriteReport()
    {
        string path = Application.dataPath + "/report.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);

        writer.WriteLine("Total Distance: " + TotalDistance + ";");
        writer.WriteLine("Number of collisions: " + AllCollisions.Count + ";");
        writer.WriteLine("Number of moved pieces of furniture: " + AllTranslations.Count + ";");
        writer.WriteLine("Number of removed pieces of furniture: " + AllRemoved.Count + ";");
        writer.WriteLine("Number of devices: " + AllAccDevices.Count + ";");
        foreach(string a in AllRecords)
        {
            writer.WriteLine(a + ";\n");
        }
        foreach(string a in AllTranslations.Keys)
        {
            string record = "Furniture: '" + a + "', translated by " + AllTranslations[a].translation + ", and rotated around y axis by " + AllTranslations[a].rotation + "�;";
            writer.WriteLine(record);
            AllRecords.Add(record);
        }
        foreach (string a in AllRemoved)
        {
            string record = "Furniture: '" + a + "', removed;";
            writer.WriteLine(record);
            AllRecords.Add(record);
        }
        writer.Close();
    }

    private void OnEnable()
    {
        CharacterMovement.OnMovement += AddDistance;
        CharacterMovement.OnTargetReached += WriteReport;
        CharacterMovement.OnPlayerCollision += PlayerCollision;
        FurnitureSelection.OnFurnitureTranslation += AddTranslation;
        FurnitureSelection.OnFurnitureRemoval += RemoveFurniture;
        AccDeviceCreator.OnDeviceCreation += AddAccDevice;
    }

    private void OnDisable()
    {
        CharacterMovement.OnMovement -= AddDistance;
        CharacterMovement.OnTargetReached -= WriteReport;
        CharacterMovement.OnPlayerCollision -= PlayerCollision;
        FurnitureSelection.OnFurnitureTranslation -= AddTranslation;
        FurnitureSelection.OnFurnitureRemoval -= RemoveFurniture;
        AccDeviceCreator.OnDeviceCreation -= AddAccDevice;
    }
}
