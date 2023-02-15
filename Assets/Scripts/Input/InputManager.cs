using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;
using static UnityEngine.CullingGroup;

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
    public delegate void TutorialPageEv(bool forward = true);
    public static event TutorialPageEv OnTutorialPageUpdate;
    public delegate void ChangeModeEv(Mode mode);
    public static event ChangeModeEv OnChangeMode;
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
            inputs.UI.Mode1.started += OnMode1Pressed;
            inputs.UI.Mode2.started += OnMode2Pressed;
            inputs.UI.Mode3.started += OnMode3Pressed;

            inputs.CharacterInput.Move.started += OnMovementInputPressed;
            inputs.CharacterInput.Move.canceled += OnMovementInputPressed;
            inputs.CharacterInput.Move.performed += OnMovementInputPressed;
            inputs.CharacterInput.PlayerCamera.started += OnPlayerCameraMoved;
            inputs.CharacterInput.PlayerCamera.canceled += OnPlayerCameraMoved;
            inputs.CharacterInput.PlayerCamera.performed += OnPlayerCameraMoved;

            inputs.FreeLookCamera.Movement.started += OnFLCamMoved;
            inputs.FreeLookCamera.Movement.canceled += OnFLCamMoved;
            inputs.FreeLookCamera.Movement.performed += OnFLCamMoved;

            inputs.FreeLookCamera.Lift.started += OnFLCamLift;
            inputs.FreeLookCamera.Lift.canceled += OnFLCamLift;
            inputs.FreeLookCamera.Lift.performed += OnFLCamLift;

            inputs.FreeLookCamera.Rotate.started += OnFLCamRotate;
            inputs.FreeLookCamera.Rotate.canceled += OnFLCamRotate;
            inputs.FreeLookCamera.Rotate.performed += OnFLCamRotate;
        }
        else
        {
            //Game state
            GameManager.OnSceneUpdate -= OnSceneUpdate;

            //Input
            inputs.UI.Back.started -= OnBackPressed;
            inputs.UI.Pause.started -= OnPausePressed;
            inputs.UI.Space.started -= OnSpacePressed;
            inputs.UI.Mode1.started -= OnMode1Pressed;
            inputs.UI.Mode2.started -= OnMode2Pressed;
            inputs.UI.Mode3.started -= OnMode3Pressed;

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
        else
            OnBack?.Invoke();
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
            if (isTutorial)
            {
                Debug.Log($"{GetType().Name}.cs > State is TUTORIAL, moving forward)");

                OnTutorialPageUpdate?.Invoke();
            }
        }
    }

    private void OnMode1Pressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > 1 Key pressed (context value as button {context.ReadValueAsButton()})");

        if (context.ReadValueAsButton())
        {
            if (isPlaying)
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
            if (isPlaying)
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
            if (isPlaying)
            {
                Debug.Log($"{GetType().Name}.cs > State is PLAYING, entering plan mode");

                mode = Mode.Plan;

                OnChangeMode?.Invoke(mode);
            }
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
}
