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

    private List<string> _allRecords;
    private float _totalDistance;
    private List<Collision> _allCollisions;
    private Dictionary<string, RotoTranslation> _allTranslations;
    private List<AccDevicePlacement> _allAccDevices;
    
    // Start is called before the first frame update
    void Awake()
    {
        _allRecords = new List<string>();
        _totalDistance = 0f;
        _allCollisions = new List<Collision>();
        _allTranslations = new Dictionary<string, RotoTranslation>();
        _allAccDevices = new List<AccDevicePlacement>();
    }

    void AddDistance(float distance)
    {
        _totalDistance += distance;
        //Debug.Log("Distanza percorsa: " + distance + " m");
        //_allRecords.Add(new string("Distanza percorsa: " + distance + " m"));
    }

    void OnCollisionEnter(Collision collision)
    {
        _allCollisions.Add(collision);
        Debug.Log("Collisione in " + collision.GetContact(0).point + " con " + collision.collider.gameObject.name);
        if (collision.collider.gameObject.TryGetComponent(typeof(Renderer),out Component mf)){
            if (collision.collider.gameObject.name.ToLower().IndexOf("floor")==-1){
                _allRecords.Add(new string("Collisione in " + collision.GetContact(0).point + " con " + collision.collider.gameObject.name));
            }
        }
        else{
            if (collision.collider.gameObject.transform.parent.name.ToLower().IndexOf("floor")==-1){
                _allRecords.Add(new string("Collisione in " + collision.GetContact(0).point + " con " + collision.collider.gameObject.transform.parent.name));
            }
        }
        
    }

    void AddTranslation(string pickedFurniture, Vector3 translation, float rotation)
    {
        if (_allTranslations.ContainsKey(pickedFurniture))
        {
            RotoTranslation a = _allTranslations[pickedFurniture];
            a.translation += translation;
            a.rotation += rotation;
            _allTranslations[pickedFurniture] = a;
        } else {
            RotoTranslation a = new RotoTranslation();
            a.translation += translation;
            a.rotation += rotation;
            _allTranslations.Add(pickedFurniture, a); 
        }
        Debug.Log("Spostato l'oggetto " + pickedFurniture + " di " + translation.magnitude + " m");
        _allRecords.Add(new string("Spostato l'oggetto " + pickedFurniture + " di " + translation.magnitude + " m" + ", e ruotato di " + rotation + "gradi"));
    }

    void AddAccDevice(string accDevice, Vector3 pos)
    {
        AccDevicePlacement adp;
        adp.accDevice = new string(accDevice);
        adp.position = new Vector3(pos.x, pos.y, pos.z);
        _allAccDevices.Add(adp);
        Debug.Log("Posizionato il dispositivo " + accDevice + " in " + pos.ToString());
        _allRecords.Add(new string("Posizionato il dispositivo " + accDevice + " in " + pos.ToString()));
    }

    void WriteReport()
    {
        string path = Application.persistentDataPath + "/report.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        foreach(string a in _allRecords)
        {
            writer.WriteLine(a + ";\n");
        }
        foreach(string a in _allTranslations.Keys)
        {
            writer.WriteLine("Mobile: '" + a + "', spostato di " + _allTranslations[a]);
        }
        writer.WriteLine("Numero di mobili spostati: " + _allTranslations.Count);
        writer.WriteLine("Numero di collisioni: " + _allCollisions.Count + ";\n");
        writer.WriteLine("Distanza totale: " + _totalDistance + ";\n");
        writer.Close();
    }

    private void OnEnable()
    {
        CharacterMovement.OnMovement += AddDistance;
        CharacterMovement.OnTargetReached += WriteReport;
        FurnitureSelection.OnFurnitureTranslation += AddTranslation;
        AccDeviceCreator.OnDeviceCreation += AddAccDevice;
    }

    private void OnDisable()
    {
        CharacterMovement.OnMovement -= AddDistance;
        CharacterMovement.OnTargetReached -= WriteReport;
        FurnitureSelection.OnFurnitureTranslation -= AddTranslation;
        AccDeviceCreator.OnDeviceCreation -= AddAccDevice;
    }
}
