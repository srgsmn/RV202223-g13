using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class FurnitureSelection : MonoBehaviour
{
    enum  e_mode { mode_navigation, mode_selection, mode_move };
    public struct Mat_key{
        public Mat_key(string obj, string mat){
            obj_name=obj;
            mat_name=mat;
        }
        public string obj_name;
        public string mat_name;
    }

    public struct Mat_emission{
        public Mat_emission(bool isem, Color emc){
            isEmitting=isem;
            emissionColor=emc;
        }
        public bool isEmitting;
        public Color emissionColor;
    }
    private string[] _structElements={"wall","floor", "ceiling","lavandino","roof","stair","scale"};

    //private static Color my_transparency = new Color(0, 0, 0, 0);
    
    public Camera PlayerCamera;
    public Material SelectedMaterial;
    public Color SelectedColor;
    public float Range = 100f;
    public float MoveSpeed = 0.5f;

    private GameObject _selected;
    private GameObject _toBeSelected;
    private Rigidbody _active_rb;
    private Dictionary<Mat_key, Mat_emission> _inactive_materials;
    private RaycastHit _raycastHit;
    private e_mode _currentMode;
    private bool _selectionToNav=false;
    private bool _moveToNav=false;
    private bool _moveToSel=false;
    private Vector2 _localTranslation; // furniture
    private float _localRotation; // furniture

    private Vector3 _originalPosition;
    private float _originalRotation;

    private bool _translateMode=false;
    private bool _rotateMode=false;

    #region GESTIONE_INPUT
        public delegate void Hover(bool isHovering);
        public static event Hover OnHover;

        public delegate void SelectEvent(bool sel);
        public static event SelectEvent OnSelect;
        private bool _spacePressed=false;
        private bool _eliminatePressed=false;
        private bool _applyChange=false;
        private bool _toRotate=false;

    #endregion


    #region GESTIONE_REPORT
    // posizione in cui trovi l'oggetto da spostare:
public delegate void TranslateFurniture(string pickedFurniture, Vector3 translation, float rotation);
    public static event TranslateFurniture OnFurnitureTranslation;
    public delegate void RemoveFurniture(string pickedFurniture, Vector3 translation);
    public static event RemoveFurniture OnFurnitureRemoval;


    // da inserire quando posi l'oggetto
    void LeaveFurniture()
    {
        OnFurnitureTranslation?.Invoke(_selected.name, _selected.transform.position - _originalPosition, _originalRotation);
    }

    void TakeFurniture()
    {
        OnFurnitureRemoval?.Invoke(_selected.name, _selected.transform.position);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _selected = null;
        _currentMode = e_mode.mode_navigation;
        _inactive_materials=new Dictionary<Mat_key,Mat_emission>();
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
                if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out _raycastHit, Range))
                {   
                    if(!_raycastHit.collider.gameObject.TryGetComponent(typeof(MeshFilter),out Component mf)){
                        _toBeSelected=_raycastHit.collider.gameObject.transform.parent.gameObject;
                    }
                    else{
                        _toBeSelected=_raycastHit.collider.gameObject;
                    }
                    if ((_selected == null || _selected.GetInstanceID() != _toBeSelected.GetInstanceID()))
                    {
                        if (_selected != null)
                        {
                            DeHighlight(_selected);
                            OnHover?.Invoke(false);
                            _selected=null;
                        }
                        if (!CheckFixed(_toBeSelected)){
                            _selected = _toBeSelected;
                            Highlight(_selected);
                            OnHover?.Invoke(true);
                        }
                    }
                }
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
                if (_spacePressed)
                {
                    _spacePressed=false;
                    if (_selected != null)
                    {
                        /*
                        if (mesh_ex){
                            //_active_rb = _selected.GetComponent<Rigidbody>();
                        }
                        else {
                            //_active_rb = _selected.transform.parent.gameObject.GetComponent<Rigidbody>();
                        }
                        */
                        //_active_rb.isKinematic=false;
                        //if (!IsStructural(_selected.name.ToLower()) && !IsFixed(_selected.name.ToLower())){
                        _originalPosition = _selected.transform.position;
                        _originalRotation = _selected.transform.rotation.y;
                        OnSelect?.Invoke(true);
                        _currentMode = e_mode.mode_move;
                    }
                }
                break;
            case e_mode.mode_move:
                Debug.Log("oggetto selezionato");
                if (_eliminatePressed){
                    Debug.Log("Entro in eliminatePressed");
                    _eliminatePressed=false;
                    if (_selected!=null){
                        Destroy(_selected);
                        _moveToNav=true;
                    }
                }
                if (_applyChange)
                {   
                    Debug.Log("Entro in applychange");
                    _applyChange=false;
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
                    Debug.Log("Entro in movetonav");
                    if (_selected != null)
                    {
                        DeHighlight(_selected);
                        _selected.transform.position=_originalPosition;
                        _selected.transform.Rotate(0.0f,_originalRotation-_selected.transform.rotation.y,0.0f,Space.Self);
                        //_active_rb.isKinematic=true;
                        _selected = null;
                    }
                    _moveToNav=false;
                    _currentMode = e_mode.mode_navigation;
                }
                if (_moveToSel)
                {
                    Debug.Log("Entro in movetosel");
                    if (_selected != null)
                    {
                        DeHighlight(_selected);
                        _selected.transform.position=_originalPosition;
                        _selected.transform.Rotate(0.0f,_originalRotation-_selected.transform.rotation.y,0.0f,Space.Self);
                        //_active_rb.isKinematic=true;
                        _selected = null;
                    }
                    _moveToSel=false;
                    _currentMode = e_mode.mode_selection;
                }
                if (_localTranslation.y > 0.5)
                {
                    _selected.transform.Translate(0.1f,0,0,PlayerCamera.transform);
                    //_selected.transform.Translate(0.1f, 0, 0, Space.World);
                    //_active_rb.velocity = MoveSpeed * Vector3.forward;
                }
                else if (_localTranslation.y < -0.5)
                {
                    _selected.transform.Translate(-0.1f, 0, 0, PlayerCamera.transform);
                    //_selected.transform.Translate(-0.1f, 0, 0, Space.World);
                    //_active_rb.velocity = -MoveSpeed * Vector3.forward;
                }
                if (_localTranslation.x < -0.5)
                {
                    Vector3 direction=_selected.transform.position-PlayerCamera.transform.position;
                    Vector3 horizontalPlane=new Vector3(0f,1.0f,0f);
                    _selected.transform.Translate((Vector3.Normalize(Vector3.ProjectOnPlane(direction,horizontalPlane))*(-0.1f)),Space.World);
                    //_selected.transform.Translate(0, 0, -0.1f, PlayerCamera.transform);
                    //_selected.transform.Translate(0, 0, -0.1f, Space.World);
                    //_active_rb.velocity = MoveSpeed * Vector3.left;
                }
                else if (_localTranslation.x > 0.5)
                {   
                    //_selected.transform.Translate(0, 0, 0.1f,PlayerCamera.transform);
                    Vector3 direction=_selected.transform.position-PlayerCamera.transform.position;
                    Vector3 horizontalPlane=new Vector3(0f,1.0f,0f);
                    _selected.transform.Translate((Vector3.Normalize(Vector3.ProjectOnPlane(direction,horizontalPlane))*(0.1f)),Space.World);
                    //_selected.transform.Translate(0, 0, 0.1f, Space.World);
                    //_active_rb.velocity = -MoveSpeed * Vector3.left;
                }
                if(_localRotation > 0.5)
                {
                    //_active_rb.velocity = Vector3.zero;
                    _selected.transform.Rotate(0f, 1f, 0f,Space.World);
                } else if(_localRotation < -0.5)
                {
                    _selected.transform.Rotate(0f, -1f, 0f,Space.World);
                }
                
                
                break;
            default: break;
        }
    }

    private void FixedUpdate()
    {
        /*switch (_currentMode) {
            case e_mode.mode_move:
                if (_localTranslation.y > 0.5)
                {
                    _selected.transform.Translate(0.1f, 0, 0, Space.World);
                    //_active_rb.velocity = MoveSpeed * Vector3.forward;
                }
                else if (_localTranslation.y < -0.5)
                {
                    _selected.transform.Translate(-0.1f, 0, 0, Space.World);
                    //_active_rb.velocity = -MoveSpeed * Vector3.forward;
                }
                if (_localTranslation.x < -0.5)
                {
                    _selected.transform.Translate(0, 0, -0.1f, Space.World);
                    //_active_rb.velocity = MoveSpeed * Vector3.left;
                }
                else if (_localTranslation.x > 0.5)
                {   
                    _selected.transform.Translate(0, 0, 0.1f, Space.World);
                    //_active_rb.velocity = -MoveSpeed * Vector3.left;
                }
                if (_toRotate){
                    if(_localRotation > 0.5)
                    {
                        //_active_rb.velocity = Vector3.zero;
                        _selected.transform.Rotate(0f, 1f, 0f,Space.World);
                    } else if(_localRotation < -0.5)
                    {
                        _selected.transform.Rotate(0f, -1f, 0f,Space.World);
                    }
                    _toRotate=false;
                }
                
                break;
            case e_mode.mode_navigation: case e_mode.mode_selection: default: break;
        }*/
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
            r = (Renderer) mf;
            Material[] mat_set=r.materials;
            foreach (Material m in mat_set){
                Mat_key k = new Mat_key(gobj.name,m.name);
                Mat_emission em = new Mat_emission(m.IsKeywordEnabled("_EMISSION"),m.GetColor("_EmissionColor"));
                _inactive_materials.Add(k,em);
                m.EnableKeyword("EMISSION");
                m.SetColor("_EmissionColor",SelectedColor);
            }
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
        mesh_ex=gobj.TryGetComponent(typeof(Renderer),out Component mf);
         if (mesh_ex){
            r = (Renderer) mf;
            Material[] mat_set=r.materials;
            foreach (Material m in mat_set){
                Mat_emission old_col = _inactive_materials.GetValueOrDefault(new Mat_key(gobj.name,m.name));
                if (!old_col.isEmitting){ 
                    m.EnableKeyword("EMISSION");
                }
                m.SetColor("_EmissionColor",old_col.emissionColor);
            }
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
                if (_currentMode==e_mode.mode_move){
                    _moveToSel=true;
                }
                else{
                    _currentMode=e_mode.mode_selection;
                }
                break;
            case Mode.Plan: //aggiungi device
                _currentMode=e_mode.mode_navigation;
                break;
        }
    }

    private void ApplyTranslation(Utilities.TranDir dir) 
    {
        if (dir==TranDir.Fwd){
            _localTranslation.x=1;
        }
        else if (dir==TranDir.Bwd){
            _localTranslation.x=-1;
        }
        else if (dir==TranDir.Rt){
            _localTranslation.y=1;
        }
        else if (dir==TranDir.Lt){
            _localTranslation.y=-1;
        }
        else if (dir==TranDir.NoneY){
            _localTranslation.x=0;
        }
        else if (dir==TranDir.NoneX){
            _localTranslation.y=0;
        }
    }
    private void ApplyRotation(Utilities.RotDir dir)
    {
        if (dir==RotDir.Cw){
            _localRotation=1;
        }
        else if (dir==RotDir.CCw){
            _localRotation=-1;
        }
        else if (dir==RotDir.None){
             _localRotation=0;
        }
    }

    private void SelectObject(){
        _spacePressed=true;
    }
    private void EliminateObject(){
        _eliminatePressed=true;
    }
    private void ConfirmEdit(){
        _applyChange=true;
    }

    private void Awake(){
        InputManager.OnChangeMode += ChangeMode;
        InputManager.OnRotate+=ApplyRotation;
        InputManager.OnTranslate+=ApplyTranslation;
        InputManager.OnConfirm+=ConfirmEdit;
        //InputManager.OnFurnitureTranslation += ApplyTranslation;
        //InputManager.OnFurnitureRotation += ApplyRotation;
        InputManager.OnSelection+=SelectObject;
        InputManager.OnEliminate+=EliminateObject;
        //InputManager.OnBack+=
    }
    private void OnDestroy(){
        InputManager.OnChangeMode -= ChangeMode;
        InputManager.OnRotate-=ApplyRotation;
        InputManager.OnTranslate-=ApplyTranslation;
        InputManager.OnConfirm-=ConfirmEdit;
        //InputManager.OnFurnitureTranslation -= ApplyTranslation;
        //InputManager.OnFurnitureRotation -= ApplyRotation;
        InputManager.OnSelection-=SelectObject;
        InputManager.OnEliminate-=EliminateObject;
        //InputManager.OnBack-=
    }
}
