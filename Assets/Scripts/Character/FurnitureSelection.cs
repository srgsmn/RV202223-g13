using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class FurnitureSelection : MonoBehaviour
{
    enum  e_mode { mode_navigation, mode_selection, mode_move };
    private string[] _structElements={"wall","floor", "ceiling","lavandino","roof","stair","scale"};

    //private static Color my_transparency = new Color(0, 0, 0, 0);
    
    public Camera PlayerCamera;
    public Material SelectedMaterial;
    public Color SelectedColor;
    public float Range = 100f;
    public float MoveSpeed = 0.5f;

    private GameObject _selected;
    private Rigidbody _active_rb;
    private Dictionary<int, Material> _inactive_materials;
    private RaycastHit _raycastHit;
    private e_mode _currentMode;
    private bool isEmpty;
    private bool _selectionToNav=false;
    private bool _moveToNav=false;
    private Vector2 _localTranslation; // furniture
    private float _localRotation; // furniture

    private Vector3 _originalPosition;

    #region GESTIONE_REPORT
    // posizione in cui trovi l'oggetto da spostare:

    public delegate void TranslateFurniture(string pickedFurniture, Vector3 translation);
    public static event TranslateFurniture OnFurnitureTranslation;


    // da inserire quando posi l'oggetto
    void LeaveFurniture()
    {
        OnFurnitureTranslation?.Invoke(_selected.name, _selected.transform.position - _originalPosition);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _selected = null;
        _currentMode = e_mode.mode_navigation;
        _inactive_materials = new Dictionary<int, Material>();
        SelectedMaterial.SetColor("_Color", SelectedColor);
    }

    // Update is called once per frame
    void Update()
    {
        bool mesh_ex;
        //if(_selected != null) _selected.GetComponent<Renderer>().material.SetColor("_Color", SelectedColor);
        switch (_currentMode)
        {
            case e_mode.mode_navigation:
                break;
            case e_mode.mode_selection:
                if (_selectionToNav)
                {
                    if(_selected != null)
                    {
                        //Per qualche motivo non toglie il materiale
                        DeHighlight(_selected);
                        _selected = null;
                    }
                    _selectionToNav=false;
                    _currentMode = e_mode.mode_navigation;
                }
                if (Input.GetButtonDown("Space") /*GetKeyDown(KeyCode.Space)*/ )
                {
                    if (_selected != null)
                    {
                        mesh_ex=_selected.TryGetComponent(typeof(Renderer),out Component mf);
                        /*
                        if (mesh_ex){
                            //_active_rb = _selected.GetComponent<Rigidbody>();
                        }
                        else {
                            //_active_rb = _selected.transform.parent.gameObject.GetComponent<Rigidbody>();
                        }
                        */
                        //_active_rb.isKinematic=false;
                        if (!IsStructural(_selected.name.ToLower()) && !IsFixed(_selected.name.ToLower())){
                            _originalPosition=_selected.transform.position;
                            _currentMode = e_mode.mode_move;
                        }
                    }
                }
                if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out _raycastHit, Range))
                {   
                    if (_selected == null || _selected.GetInstanceID() != _raycastHit.collider.gameObject.GetInstanceID())
                    {
                        if (_selected != null)
                        {
                            DeHighlight(_selected);
                        }
                        _selected = _raycastHit.transform.gameObject;
                        Highlight(_selected);
                    }
                }
                break;
            case e_mode.mode_move:
                if (Input.GetButtonDown("Space"))
                {   

                    if (_selected != null)
                    {
                        DeHighlight(_selected);
                        LeaveFurniture();
                        //_active_rb.isKinematic=true;
                        _selected = null;
                    }  
                    _currentMode = e_mode.mode_selection;
                }
                if (_moveToNav)
                {
                    if (_selected != null)
                    {
                        DeHighlight(_selected);
                        _selected.transform.position=_originalPosition;
                        //_active_rb.isKinematic=true;
                        _selected = null;
                    }
                    _moveToNav=false;
                    _currentMode = e_mode.mode_navigation;
                }

                break;
            default: break;
        }
    }

    private void FixedUpdate()
    {
        switch (_currentMode) {
            case e_mode.mode_move:
                Debug.Log("active object is  =" + _selected.name);
                
                if (_localTranslation.y > 0.5)
                {
                    _selected.transform.Translate(0.1f, 0, 0, Space.World);
                    //_active_rb.velocity = MoveSpeed * Vector3.forward;
                }
                else if (_localTranslation.y < 0.5)
                {
                    _selected.transform.Translate(-0.1f, 0, 0, Space.World);
                    //_active_rb.velocity = -MoveSpeed * Vector3.forward;
                }
                else if (_localTranslation.x < 0.5)
                {
                    _selected.transform.Translate(0, 0, -0.1f, Space.World);
                    //_active_rb.velocity = MoveSpeed * Vector3.left;
                }
                else if (_localTranslation.x > 0.5)
                {   
                    _selected.transform.Translate(0, 0, 0.1f, Space.World);
                    //_active_rb.velocity = -MoveSpeed * Vector3.left;
                }
                else if(_localRotation > 0.5)
                {
                    //_active_rb.velocity = Vector3.zero;
                    _selected.transform.Rotate(0f, 1f, 0f);
                } else if(_localRotation < 0.5)
                {
                    _selected.transform.Rotate(0f, -1f, 0f);
                }
                break;
            case e_mode.mode_navigation: case e_mode.mode_selection: default: break;
        }
    }

    private void Highlight(GameObject gobj)
    {
        SelectMaterial_r(gobj, true);
        /*Outline ol;
        if (gobj.GetComponent<Outline>() == null)
        {
            ol = gobj.AddComponent<Outline>();

        } else
        {
            ol = gobj.GetComponent<Outline>();
        }
        ol.OutlineMode = Outline.Mode.OutlineAll;
        ol.OutlineColor = SelectedColor;
        ol.OutlineWidth = 10f;*/
    }

    private void SelectMaterial_r(GameObject gobj, bool first)
    {   
        Renderer r;
        bool mesh_ex;
        mesh_ex=gobj.TryGetComponent(typeof(Renderer),out Component mf);
        if (mesh_ex){
            r = gobj.GetComponent<Renderer>();
            _inactive_materials.Add(gobj.transform.gameObject.GetInstanceID(), r.material);
            r.material = SelectedMaterial;
        }
        else if (first){
            SelectMaterial_r(gobj.transform.parent.gameObject,false);
            return;
        }
        
        if (gobj.transform.childCount <= 0) return;
        for (int i = 0; i < gobj.transform.childCount;
            SelectMaterial_r(gobj.transform.GetChild(i++).gameObject,false)) ;
    }

    private void DeHighlight(GameObject gobj)
    {
        ResetMaterial_r(gobj, true);
        _inactive_materials.Clear();

        //Destroy(gobj.GetComponent("Outline"));
        /*
        Outline ol = gobj.GetComponent<Outline>();
        if (ol == null) return;

        ol.OutlineMode = (Outline.Mode) 5;
        ol.OutlineColor = my_transparency;
        ol.OutlineWidth = 0f;*/
    }

    private void ResetMaterial_r(GameObject gobj, bool first)
    {
        Renderer r;
        bool mesh_ex;
        Material old_mat;
        mesh_ex=gobj.TryGetComponent(typeof(Renderer),out Component mf);
         if (mesh_ex){
            Debug.Log("Ci arrivo per il " + gobj.name); 
            r = gobj.GetComponent<Renderer>();
            old_mat = _inactive_materials.GetValueOrDefault(gobj.transform.gameObject.GetInstanceID());
            r.material = (old_mat != default) ? old_mat : null;
        }
        else if (first){
            ResetMaterial_r(gobj.transform.parent.gameObject,false);
            return;
        }
        if (gobj.transform.childCount <= 0) return;
        for (int i = 0; i < gobj.transform.childCount;
            ResetMaterial_r(gobj.transform.GetChild(i++).gameObject,false));
    }


    private bool CheckFixed(GameObject go){
        return (IsFixed(go.name.ToLower()) || IsStructural(go.name.ToLower()));
    }
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

    private void ChangeMode(Mode mode_input){
        switch(mode_input){
            case Mode.Nav:
                if (_currentMode==e_mode.mode_selection){
                    _selectionToNav=true;
                }
                else if (_currentMode==e_mode.mode_move){
                    _moveToNav=true;
                }
                else{
                    _currentMode=e_mode.mode_navigation;
                }
                break;
            case Mode.Edit:
                _currentMode=e_mode.mode_selection;
                break;
            case Mode.Plan: //aggiungi device
                _currentMode=e_mode.mode_navigation;
                break;
        }
    }

    private void ApplyTranslation(Vector2 delta) 
    {
        _localTranslation = delta;
    }
    private void ApplyRotation(float delta)
    {
        _localRotation = delta;
    }

    private void Awake(){
        InputManager.OnChangeMode += ChangeMode;
        InputManager.OnFurnitureTranslation += ApplyTranslation;
        InputManager.OnFurnitureRotation += ApplyRotation;
        //InputManager.OnBack+=
    }
    private void OnDestroy(){
        InputManager.OnChangeMode -= ChangeMode;
        InputManager.OnFurnitureTranslation -= ApplyTranslation;
        InputManager.OnFurnitureRotation -= ApplyRotation;
        //InputManager.OnBack-=
    }
}
