using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureSelection : MonoBehaviour
{
    enum  e_mode { mode_navigation, mode_selection, mode_move };

    //private static Color my_transparency = new Color(0, 0, 0, 0);
    
    public Camera PlayerCamera;
    public Material SelectedMaterial;
    public Color SelectedColor;
    public float Range = 100f;
    public float MoveSpeed = 0.5f;

    private GameObject _selected;
    private Rigidbody _active_rb;
    private Dictionary<string, Material> _inactive_materials;
    private RaycastHit _raycastHit;
    private e_mode _currentMode;

    // Start is called before the first frame update
    void Start()
    {
        _selected = null;
        _currentMode = e_mode.mode_navigation;
        _inactive_materials = new Dictionary<string, Material>();
        SelectedMaterial.SetColor("_Color", SelectedColor);
    }

    // Update is called once per frame
    void Update()
    {
        if(_selected != null) _selected.GetComponent<Renderer>().material.SetColor("_Color", SelectedColor);
        switch (_currentMode)
        {
            case e_mode.mode_navigation:
                if (Input.GetKeyDown(KeyCode.Space)) 
                    _currentMode = e_mode.mode_selection;
                break;
            case e_mode.mode_selection:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if(_selected != null)
                    {
                        DeHighlight(_selected);
                        _selected = null;
                    }
                    _currentMode = e_mode.mode_navigation;
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    if (_selected != null)
                    {
                        _active_rb = _selected.GetComponent<Rigidbody>();
                        _currentMode = e_mode.mode_move;
                    }
                }
                if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out _raycastHit, Range))
                {
                    if (_selected == null || _selected != _raycastHit.collider.gameObject)
                    {
                        Debug.Log(_raycastHit.transform.name);
                        if (_selected != null)
                        {
                            DeHighlight(_selected);
                        }
                        _selected = _raycastHit.collider.gameObject;
                        Highlight(_selected);
                    }
                }
                break;
            case e_mode.mode_move:
                if (Input.GetKeyDown(KeyCode.B))
                {
                    _currentMode = e_mode.mode_selection;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (_selected != null)
                    {
                        DeHighlight(_selected);
                        _selected = null;
                    }
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
                //Debug.Log("_selected =" + _selected.name);
                if (Input.GetKey(KeyCode.I))
                {
                    //_selected.transform.Translate(0.5f, 0, 0, Space.World);
                    _active_rb.velocity = MoveSpeed * Vector3.forward;
                }
                else if (Input.GetKey(KeyCode.K))
                {
                    //_selected.transform.Translate(-0.5f, 0, 0, Space.World);
                    _active_rb.velocity = -MoveSpeed * Vector3.forward;
                }
                else if (Input.GetKey(KeyCode.J))
                {
                    _active_rb.velocity = MoveSpeed * Vector3.left;
                }
                else if (Input.GetKey(KeyCode.L))
                {
                    _active_rb.velocity = -MoveSpeed * Vector3.left;
                }
                else
                {
                    _active_rb.velocity = Vector3.zero;
                }
                break;
            case e_mode.mode_navigation: case e_mode.mode_selection: default: break;
        }
    }

    private void Highlight(GameObject gobj)
    {
        SelectMaterial_r(gobj);
        /*
        Outline ol;
        if (gobj.GetComponent<Outline>() == null)
        {
            ol = gobj.AddComponent<Outline>();

        } else
        {
            ol = gobj.GetComponent<Outline>();
        }
        ol.OutlineMode = Outline.Mode.OutlineAll;
        ol.OutlineColor = SelectedColor;
        ol.OutlineWidth = 10f;
        */
    }

    private void SelectMaterial_r(GameObject gobj)
    {
        Renderer r = gobj.GetComponent<Renderer>();
        if (r == null) return;
        _inactive_materials.Add(gobj.transform.name, r.material);
        r.material = SelectedMaterial;
        if (gobj.transform.childCount <= 0) return;
        for (int i = 0; i < gobj.transform.childCount;
            SelectMaterial_r(gobj.transform.GetChild(i++).gameObject)) ;
    }

    private void DeHighlight(GameObject gobj)
    {
        ResetMaterial_r(gobj);
        _inactive_materials.Clear();

        //Destroy(gobj.GetComponent("Outline"));
        /*
        Outline ol = gobj.GetComponent<Outline>();
        if (ol == null) return;

        ol.OutlineMode = (Outline.Mode) 5;
        ol.OutlineColor = my_transparency;
        ol.OutlineWidth = 0f;*/
    }

    private void ResetMaterial_r(GameObject gobj)
    {
        Renderer r = gobj.GetComponent<Renderer>();
        if (r == null) return;
        Material old_mat = _inactive_materials.GetValueOrDefault(gobj.transform.name);
        r.material = (old_mat != default) ? old_mat : null;
        if (gobj.transform.childCount <= 0) return;
        for (int i = 0; i < gobj.transform.childCount;
            ResetMaterial_r(gobj.transform.GetChild(i++).gameObject)) ;
    }

    public int GetCurrentMode()
    {
        return (int)_currentMode;
    }

}
