using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;
using static UnityEngine.CullingGroup;

/// <summary>
/// Command list:
/// [0] Endpoint selection mode     -> OnChangeMode(Utilities.Mode.EPSelector)
///     [SPACE] Endpoint insertion  -> OnEPConfirm();   TODO
///     
/// [1] Navigation mode             -> OnChangeMode(Utilities.Mode.Nav)
///     [W] Move forward            -> 
///     [S] Move backward
///     [A] Steer left
///     [D] Steer right
///
/// [2] Edit mode                   -> OnChangeMode(Utilities.Mode.Edit)
///     [SPACE] Object selection    
///         [E] Eimination          -> OnEliminate()
///         [R] Rotation
///             [<-] Counterclockwise wrt z axis    -> OnRotate(Utilities.RotDir.CCw)
///             [->] Clockwise wrt z axis           -> OnRotate(Utilities.RotDir.Cw)
///         [T] Traslate
///             [arrows] translation direction      -> OnTranslate(Utilities.TranDir.CCw)
///
/// [3] Plan mode                   -> OnChangeMode(Utilities.Mode.Plan)
///     [arrows] Moving inside the inventory
///     [SPACE] Devide Selection
///
/// NOTA:   Input manager tiene traccia attraverso dei flag di alcune variabili di stato
///         affinché filtri gli input. Ad esempio, il tasto E non avvia l'evento eliminazione
///         a meno che non sia stato precedentemente premuto il tasto spazio per selezionare un
///         oggetto. Eventuali listeners dovranno gestire in modo analogo questi eventi.
///
///         Alcune variabili, per mantenere indipendente questo script, possono essere impostate
///         esternamente con metodi pubblici. Un esempio è la funzione SetHoverFlag() che modifica
///         lo stato di hoverObject. La funzione dev'essere richamata qualora il cursore si posizioni
///         sopra un oggetto interagibile.
///
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("Game state:")]
    [SerializeField][ReadOnlyInspector] SceneType sceneType;
    [SerializeField][ReadOnlyInspector] SceneState sceneState;
    [SerializeField][ReadOnlyInspector] bool isPlaying;
    [SerializeField][ReadOnlyInspector] bool isPaused;
    [SerializeField][ReadOnlyInspector] bool isTutorial;
    [SerializeField][ReadOnlyInspector] Mode mode;
    [SerializeField][ReadOnlyInspector] bool objectSelected;
    [SerializeField][ReadOnlyInspector] bool rotationSelected = false;
    [SerializeField][ReadOnlyInspector] bool translationSelected = false;
    [SerializeField]/*[ReadOnlyInspector]*/ bool hoverObject = false;

    [Header("Event System:")]
    [SerializeField][ReadOnlyInspector] bool evSysDetected = false;
    [SerializeField][ReadOnlyInspector] GameObject evSysPrefab;
    private GameObject evSysInstance;

    private PlayerInputs inputs;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }

        inputs = new PlayerInputs();

        evSysInstance = GameObject.Find("EventSystem");

        if (evSysInstance==null)
        {
            evSysDetected = false;

            evSysPrefab = Resources.Load(PREFABS.EVSYS) as GameObject;

            evSysInstance = Instantiate(evSysPrefab);

            if (evSysInstance != null)
                evSysDetected = true;
        }
        else
        {
            evSysDetected = true;
        }

        EventSubscriber();
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        if (inputs != null) inputs.Disable();
    }

    private void OnDestroy()
    {
        EventSubscriber(false);
    }

    public delegate void PauseEv();
    public static event PauseEv OnPause;
    public delegate void ResumeEv();
    public static event ResumeEv OnResume;
    public delegate void BackEv();
    public static event BackEv OnBack;
    public delegate void ConfirmEv();
    public static event ConfirmEv OnConfirm;
    public delegate void TutorialPageEv(bool forward = true);
    public static event TutorialPageEv OnTutorialPageUpdate;
    public delegate void ChangeModeEv(Mode mode);
    public static event ChangeModeEv OnChangeMode;
    public delegate void SelectionEv();
    public static event SelectionEv OnSelection;
    public delegate void ElimEv();
    public static event ElimEv OnEliminate;
    public delegate void RotateEv(RotDir dir);
    public static event RotateEv OnRotate;
    public delegate void TranslateEv(TranDir dir);
    public static event TranslateEv OnTranslate;
    public delegate void ArrowPressedEv();
    public static event ArrowPressedEv OnArrowPressed;

    public delegate void MovementEv(Vector2 move);
    public static event MovementEv OnMovementInput;
    public delegate void PlayerCameraEv(Vector2 mouse);
    public static event PlayerCameraEv OnPlayerCameraInput;
    public delegate void FLCamMovementEv(Vector2 move);
    public static event FLCamMovementEv OnFLCamMovement;
    public delegate void FLCamLiftEv(float lift);
    public static event FLCamLiftEv OnFLCamLifting;
    public delegate void FLCamRotationEv(Vector2 move);
    public static event FLCamRotationEv OnFLCamRotInput;
    public delegate void FLCamZoomEv(float zoom);
    public static event FLCamZoomEv OnFLCamZoomInput;
    public delegate void FurnitureTraslationEv(Vector2 move);
    public static event FurnitureTraslationEv OnFurnitureTranslation;
    public delegate void FurnitureRotationEv(float rotate);
    public static event FurnitureRotationEv OnFurnitureRotation;

    private void EventSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            //Game state
            GameManager.OnSceneUpdate += OnSceneUpdate;

            //Input
            inputs.UI.Back.started += OnBackPressed;
            inputs.UI.Pause.started += OnPausePressed;
            inputs.UI.Space.started += OnSpacePressed;
            inputs.UI.Enter.started += OnEnterPressed;
            inputs.UI.Mode1.started += OnMode1Pressed;
            inputs.UI.Mode2.started += OnMode2Pressed;
            inputs.UI.Mode3.started += OnMode3Pressed;


            inputs.UI.Eliminate.started += OnEliminatePressed;
            inputs.UI.Rotate.started += OnRotatePressed;
            inputs.UI.Translate.started += OnTranslatePressed;
            inputs.UI.Directions.started += OnDirectionPressed;
            inputs.UI.Directions.performed += OnDirectionPressed;
            inputs.UI.Directions.canceled += OnDirectionPressed;
            inputs.UI.MouseLtClick.started += OnMouseLtPressed;

            inputs.CharacterInput.Move.started += OnMovementInputPressed;
            inputs.CharacterInput.Move.canceled += OnMovementInputPressed;
            inputs.CharacterInput.Move.performed += OnMovementInputPressed;
            inputs.CharacterInput.PlayerCamera.started += OnPlayerCameraMoved;
            inputs.CharacterInput.PlayerCamera.canceled += OnPlayerCameraMoved;
            inputs.CharacterInput.PlayerCamera.performed += OnPlayerCameraMoved;
            inputs.CharacterInput.TranslateFurniture.started += OnFurnitureTranslationPressed;
            inputs.CharacterInput.TranslateFurniture.canceled += OnFurnitureTranslationPressed;
            inputs.CharacterInput.TranslateFurniture.performed += OnFurnitureTranslationPressed;
            inputs.CharacterInput.RotateFurniture.started += OnFurnitureRotationPressed;
            inputs.CharacterInput.RotateFurniture.canceled += OnFurnitureRotationPressed;
            inputs.CharacterInput.RotateFurniture.performed += OnFurnitureRotationPressed;

            inputs.FreeLookCamera.Movement.started += OnFLCamMoved;
            inputs.FreeLookCamera.Movement.canceled += OnFLCamMoved;
            inputs.FreeLookCamera.Movement.performed += OnFLCamMoved;
            inputs.FreeLookCamera.Lift.started += OnFLCamLift;
            inputs.FreeLookCamera.Lift.canceled += OnFLCamLift;
            inputs.FreeLookCamera.Lift.performed += OnFLCamLift;
            inputs.FreeLookCamera.Rotate.started += OnFLCamRotate;
            inputs.FreeLookCamera.Rotate.canceled += OnFLCamRotate;
            inputs.FreeLookCamera.Rotate.performed += OnFLCamRotate;
            inputs.FreeLookCamera.Zoom.started += OnFLCamZoom;
            inputs.FreeLookCamera.Zoom.canceled += OnFLCamZoom;
            inputs.FreeLookCamera.Zoom.performed += OnFLCamZoom;


            HotSpotSelection.OnEndPointSet+=NavModeStart;
            FurnitureSelection.OnHover+=SetHoverFlag;
            FurnitureSelection.OnSelect+=SetObjectSelected;

            OnChangeMode += ResetFlags;
        }
        else
        {
            //Game state
            GameManager.OnSceneUpdate -= OnSceneUpdate;

            //Input
            inputs.UI.Back.started -= OnBackPressed;
            inputs.UI.Pause.started -= OnPausePressed;
            inputs.UI.Space.started -= OnSpacePressed;
            inputs.UI.Enter.started -= OnEnterPressed;
            inputs.UI.Mode1.started -= OnMode1Pressed;
            inputs.UI.Mode2.started -= OnMode2Pressed;
            inputs.UI.Mode3.started -= OnMode3Pressed;


            inputs.UI.Eliminate.started -= OnEliminatePressed;
            inputs.UI.Rotate.started -= OnRotatePressed;
            inputs.UI.Translate.started -= OnTranslatePressed;
            inputs.UI.Directions.started -= OnDirectionPressed;
            inputs.UI.Directions.performed -= OnDirectionPressed;
            inputs.UI.Directions.canceled -= OnDirectionPressed;
            inputs.UI.MouseLtClick.started -= OnMouseLtPressed;

            inputs.CharacterInput.Move.started -= OnMovementInputPressed;
            inputs.CharacterInput.Move.canceled -= OnMovementInputPressed;
            inputs.CharacterInput.Move.performed -= OnMovementInputPressed;
            inputs.CharacterInput.PlayerCamera.started -= OnPlayerCameraMoved;
            inputs.CharacterInput.PlayerCamera.canceled -= OnPlayerCameraMoved;
            inputs.CharacterInput.PlayerCamera.performed -= OnPlayerCameraMoved;

            inputs.FreeLookCamera.Movement.started -= OnFLCamMoved;
            inputs.FreeLookCamera.Movement.canceled -= OnFLCamMoved;
            inputs.FreeLookCamera.Movement.performed -= OnFLCamMoved;

            inputs.FreeLookCamera.Lift.started -= OnFLCamLift;
            inputs.FreeLookCamera.Lift.canceled -= OnFLCamLift;
            inputs.FreeLookCamera.Lift.performed -= OnFLCamLift;

            inputs.FreeLookCamera.Rotate.started -= OnFLCamRotate;
            inputs.FreeLookCamera.Rotate.canceled -= OnFLCamRotate;
            inputs.FreeLookCamera.Rotate.performed -= OnFLCamRotate;

            HotSpotSelection.OnEndPointSet-=NavModeStart;
            FurnitureSelection.OnHover-=SetHoverFlag;
            FurnitureSelection.OnSelect-=SetObjectSelected;

            OnChangeMode -= ResetFlags;
        }
    }

    // GAMESTATE CALLBACKS
    private void OnSceneUpdate(SceneType type, SceneState state)
    {
        sceneType = type;
        sceneState = state;

        if(type != SceneType.MainMenu)
            switch (state)
            {
                case SceneState.Tutorial:

                    isTutorial = true;
                    isPlaying = false;
                    isPaused = false;

                    break;

                case SceneState.Playing:

                    isTutorial = false;
                    isPlaying = true;
                    isPaused = false;
                    
                    break;

                case SceneState.Paused:

                    isTutorial = false;
                    isPlaying = false;
                    isPaused = true;
                    
                    break;

                case SceneState.Endgame:

                    isTutorial = false;
                    isPlaying = false;
                    isPaused = false;
                    
                    break;
            }
    }

    // INPUTS CALLBACKS
    private void OnBackPressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > BACKSPACE Key pressed (context value as button {context.ReadValueAsButton()})");

        if (isPaused)
        {
            OnResume?.Invoke();
        }
        else if (isTutorial)
        {
            Debug.Log($"{GetType().Name}.cs > State is TUTORIAL, moving backward)");

            OnTutorialPageUpdate?.Invoke(false);
        }
        else if (isPlaying)
        {
            if (rotationSelected)
                rotationSelected = false;

            if (translationSelected)
                translationSelected = false;

            if (objectSelected)
                objectSelected = false;
        }
        
            //OnBack?.Invoke();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > ESCAPE Key pressed (context value as button {context.ReadValueAsButton()})");

        if (context.ReadValueAsButton())
        {
            if (isPlaying)
            {
                Debug.Log($"{GetType().Name}.cs > State is PLAYING, pausing the app)");

                OnPause?.Invoke();
            }
            else if (isPaused)
            {
                Debug.Log($"{GetType().Name}.cs > State is PAUSED, resuming the app)");

                OnResume?.Invoke();
            }
        }
    }

    private void OnSpacePressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > SPACEBAR Key pressed (context value as button {context.ReadValueAsButton()})");

        if (context.ReadValueAsButton())
        {
            // Se sono nel tutorial, vado alla pagina successiva
            if (isTutorial)
            {
                Debug.Log($"{GetType().Name}.cs > State is TUTORIAL, moving forward)");

                OnTutorialPageUpdate?.Invoke();
            }
            // Se sto giocando, seleziono cose
            else if (isPlaying)
            {
                if (mode == Mode.EPSelector) {
                    OnSelection?.Invoke();
                }
                else if(mode == Mode.Edit && hoverObject)
                {
                    if (!objectSelected)
                    {
                        OnSelection?.Invoke();
                    }
                    else
                    {
                        Debug.Log("Sto entrando su confirm");
                        objectSelected = false;
                        rotationSelected = false;
                        translationSelected = false;

                        OnConfirm?.Invoke();
                    }

                }
                else if (mode==Mode.Plan || mode==Mode.Nav){
                    OnSelection?.Invoke();  
                }
                else
                {
                    //OnConfirm?.Invoke();
                }
            }
        }
    }

    private void OnMouseLtPressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > LEFT BUTTON pressed (context value as button {context.ReadValueAsButton()})");

        if (context.ReadValueAsButton())
        {
            // Se sto giocando, seleziono cose
            if (isPlaying)
            {
                if (mode == Mode.EPSelector)
                {
                    OnSelection?.Invoke();
                }
            }
        }
    }

    private void OnEnterPressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > ENTER Key pressed (context value as button {context.ReadValueAsButton()})");

         if (isPlaying) 
        { 
            if(mode != Mode.Edit)
            {
                OnConfirm?.Invoke();;
            } 
        } 
    }

    /// <summary>
    /// Sets hoverObject variable to the passed parameter
    /// </summary>
    /// <param name="isHover">If the cursor is hover or not</param>
    public void SetHoverFlag(bool isHover)
    {
        hoverObject = isHover;
    }

    // MODE CHANGE

    private void OnMode0Pressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > 0 Key pressed (context value as button {context.ReadValueAsButton()})");

        if (context.ReadValueAsButton())
        {
            if (isPlaying)
            {
                Debug.Log($"{GetType().Name}.cs > State is PLAYING, entering EndPoint Selection mode");

                mode = Mode.EPSelector;

                OnChangeMode?.Invoke(mode);
            }
        }
    }

    private void OnMode1Pressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > 1 Key pressed (context value as button {context.ReadValueAsButton()})");

        if (context.ReadValueAsButton())
        {
            if (isPlaying && mode!=Mode.EPSelector)
            {
                Debug.Log($"{GetType().Name}.cs > State is PLAYING, entering navigation mode");

                mode = Mode.Nav;

                OnChangeMode?.Invoke(mode);
            }
        }
    }

    private void OnMode2Pressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > 2 Key pressed (context value as button {context.ReadValueAsButton()})");

        if (context.ReadValueAsButton())
        {
            if (isPlaying && mode != Mode.EPSelector)
            {
                Debug.Log($"{GetType().Name}.cs > State is PLAYING, entering edit mode");

                mode = Mode.Edit;

                OnChangeMode?.Invoke(mode);
            }
        }
    }

    private void OnMode3Pressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > 3 Key pressed (context value as button {context.ReadValueAsButton()})");

        if (context.ReadValueAsButton())
        {
            if (isPlaying && mode != Mode.EPSelector)
            {
                Debug.Log($"{GetType().Name}.cs > State is PLAYING, entering plan mode");

                mode = Mode.Plan;

                OnChangeMode?.Invoke(mode);
            }
        }
    }

    // ACTIONS

    private void OnEliminatePressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > E Key pressed (context value as button {context.ReadValueAsButton()})");

        if (context.ReadValueAsButton())
        {
            if (isPlaying && mode == Mode.Edit && objectSelected)
            {
                Debug.Log($"{GetType().Name}.cs > State is PLAYING + mode is EDIT: Eliminating the object...");

                OnEliminate?.Invoke();

                objectSelected = false;

                mode = Mode.Edit;

                OnChangeMode?.Invoke(mode);
            }
        }
    }

    private void OnRotatePressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > R Key pressed (context value as button {context.ReadValueAsButton()})");

        if (context.ReadValueAsButton())
        {
            if (isPlaying && mode == Mode.Edit && objectSelected)
            {
                if (translationSelected)
                    translationSelected = false;

                rotationSelected = true;
            }
        }
    }

    private void OnTranslatePressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > T Key pressed (context value as button {context.ReadValueAsButton()})");

        if (context.ReadValueAsButton())
        {
            if (isPlaying && mode == Mode.Edit && objectSelected)
            {
                if (rotationSelected)
                    rotationSelected = false;

                translationSelected = true;
            }
        }
    }

    private void OnDirectionPressed(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();

        Debug.Log($"{GetType().Name}.cs > Arrow key pressed (context value as button {value})");

        //if (value != Vector2.zero)
        if (isPlaying && mode == Mode.Edit)
        {
            if (rotationSelected)
            {
                if (value.x > 0.5)
                    OnRotate?.Invoke(RotDir.Cw);
                else if (value.x<-0.5)
                    OnRotate?.Invoke(RotDir.CCw);
                else
                    OnRotate?.Invoke(RotDir.None);
            }
            else if (translationSelected)
            {
                if (value.x > 0.5)
                    OnTranslate?.Invoke(TranDir.Rt);
                else if (value.x<-0.5)
                    OnTranslate?.Invoke(TranDir.Lt);
                else
                    OnTranslate?.Invoke(TranDir.NoneX);
                if (value.y > 0.5)
                    OnTranslate?.Invoke(TranDir.Fwd);
                else if(value.y < -0.5)
                    OnTranslate?.Invoke(TranDir.Bwd);
                else
                    OnTranslate?.Invoke(TranDir.NoneY);
            }
        }
        else
        {
            OnArrowPressed?.Invoke();
        }
    }

    private void OnMovementInputPressed(InputAction.CallbackContext context)
    {

        Vector2 movementInput;

        if (isPlaying)
        {
            movementInput = context.ReadValue<Vector2>();

            OnMovementInput?.Invoke(movementInput);
        }
    }

    private void OnFurnitureTranslationPressed(InputAction.CallbackContext context)
    {
        Vector2 movementInput;

        if (isPlaying)
        {
            movementInput = context.ReadValue<Vector2>();

            OnFurnitureTranslation?.Invoke(movementInput);
        }
    }

    private void OnFurnitureRotationPressed(InputAction.CallbackContext context)
    {
        float rotationInput;

        if (isPlaying)
        {
            rotationInput = context.ReadValue<float>();

            OnFurnitureRotation?.Invoke(rotationInput);
        }
    }

    private void OnPlayerCameraMoved(InputAction.CallbackContext context)
    {

        Vector2 movementInput;

        if (isPlaying)
        {
            movementInput = context.ReadValue<Vector2>();

            OnPlayerCameraInput?.Invoke(movementInput);
        }
    }
    private void OnFLCamMoved(InputAction.CallbackContext context)
    {

        Vector2 movementInput;

        if (isPlaying)
        {
            movementInput = context.ReadValue<Vector2>();

            OnFLCamMovement?.Invoke(movementInput);
        }
    }

    private void OnFLCamLift(InputAction.CallbackContext context)
    {
        float liftInput;
        
        if (isPlaying)
        {
            liftInput = context.ReadValue<float>();

            OnFLCamLifting?.Invoke(liftInput);
        }
    }

    private void OnFLCamRotate(InputAction.CallbackContext context)
    {
        Vector2 movementInput;

        if (isPlaying)
        {
            movementInput = context.ReadValue<Vector2>();

            OnFLCamRotInput?.Invoke(movementInput);
        }
    }

    private void OnFLCamZoom(InputAction.CallbackContext context)
    {
        float zoomInput;

        if (isPlaying)
        {
            zoomInput = context.ReadValue<float>();

            OnFLCamZoomInput?.Invoke(zoomInput);
        }
    }

    private void NavModeStart(){
        if (isPlaying)
        {
            Debug.Log($"{GetType().Name}.cs > State is PLAYING, entering navigation mode");

            mode = Mode.Nav;

            OnChangeMode?.Invoke(mode);

        }
    }

    private void ResetFlags(Mode modo)
    {
        objectSelected = false;
        rotationSelected = false;
        translationSelected = false;
        hoverObject = false;
    }

    private void SetObjectSelected(bool sel){
        Debug.Log("Ci entro?" + sel);
        objectSelected=sel;
        Debug.Log("ObjectSelected: "+ objectSelected);  
    }
}
