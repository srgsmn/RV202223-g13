//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Scripts/Input/PlayerInputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputs : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""UI"",
            ""id"": ""90702192-24e2-476a-82dc-b16d6ea06ef6"",
            ""actions"": [
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""8c525ee0-524a-4836-9437-49111582c36a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""a357fdef-1b7e-4341-8b5a-18c1486ee4a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""d7f2e1bf-f429-4306-a30d-53d92f9885aa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Space"",
                    ""type"": ""Button"",
                    ""id"": ""43699282-2b03-4eee-867b-df6aeb7de518"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mode1"",
                    ""type"": ""Button"",
                    ""id"": ""4378fd93-1530-44ee-be82-06aa364eee75"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mode2"",
                    ""type"": ""Button"",
                    ""id"": ""793e783d-8aaf-410e-8f43-c25faf0e47e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mode3"",
                    ""type"": ""Button"",
                    ""id"": ""bb9a8c1a-8935-439a-ad03-0fc6dcddd317"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""90fe69e9-293a-4331-8fc0-351aa1702242"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8e2c534-2c06-4104-9f71-85e9aca59876"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5229d61-b9aa-4094-9aaa-97243394367b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ca01cbf-bbc7-4131-a2ab-3cb19e566c96"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38988370-0bbc-463f-9120-a150ca4949d3"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Space"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c800ab42-ce72-4517-9d06-a498823e2a3f"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mode1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e48088c4-0ac4-46d0-8580-cb4d33764394"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mode1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1880b1e4-f6a5-457f-a3e6-9fc59c5dda0e"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mode2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba347bab-fbfe-44b7-8f28-daae6509fcc9"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mode2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38a8cfdd-6382-4046-88f9-28d77d7dcb66"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mode3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eee05bd4-8fbf-47ea-b9af-3fefa89b64a0"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mode3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CharacterInput"",
            ""id"": ""34bd102c-f8dc-4301-b19c-9bf4fef89838"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""671fd4d7-a1e7-49fb-9b9e-1aa511922d33"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PlayerCamera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""600f0d11-b9ef-4fe6-a58d-09253842e1ff"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""eac0f607-49e1-44fa-96ef-0e9131bbf9ba"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fd79008d-5a1f-4e7f-a9f0-2bceef0391ed"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""aee2b7b3-e58d-4230-a6c1-39dfcbf05e05"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b02e9af4-1d40-4533-9283-f83e693cb027"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b6e36ea1-95bd-48ba-a48d-e652f877d4b7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5908741d-7d00-466c-aed3-125c255d7292"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""FreeLookCamera"",
            ""id"": ""6aa7f921-3988-4d31-b205-2c62a79ebef7"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""256aef62-d1f6-4781-ac0c-c4cacbecfc41"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Lift"",
                    ""type"": ""Button"",
                    ""id"": ""0da5e585-614e-44fe-84dc-53ec31ab76b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""672a6dd1-d53d-43a4-9737-2017b785c30b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""625df0b1-0726-4d3c-a59d-c4dc8685c4cb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2a6684d5-a890-4d0e-8dba-7064108d3a6e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8114dacd-5b79-436c-9624-4bb35f7eb7fb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fa305e9b-306c-498c-8f80-0266723f5a15"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b3ad9a0c-93ef-4087-8553-4a8c79cd5ca3"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""d9a273b8-b585-45a1-8ec5-fc47bbd0bfd3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a5e439c0-c1db-46c3-8cde-219c7d802f90"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4142e21a-1f49-4941-9405-6dffadaaeba6"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ab58251d-6225-4f4f-944a-89b52a0b3013"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2e984881-6885-4bc4-8f59-e9a442a72faa"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""QE"",
                    ""id"": ""0f83b038-90df-4c73-830a-6fd5e396bfb1"",
                    ""path"": ""1DAxis(whichSideWins=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lift"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""2d08992f-96d8-4be9-99d5-0f2a9b1f8db8"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""decb28ec-5953-4962-a4c3-7f1c32083bd2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f03f06a1-e5b8-4c69-bcfd-910a66ecee0d"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Back = m_UI.FindAction("Back", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
        m_UI_Confirm = m_UI.FindAction("Confirm", throwIfNotFound: true);
        m_UI_Space = m_UI.FindAction("Space", throwIfNotFound: true);
        m_UI_Mode1 = m_UI.FindAction("Mode1", throwIfNotFound: true);
        m_UI_Mode2 = m_UI.FindAction("Mode2", throwIfNotFound: true);
        m_UI_Mode3 = m_UI.FindAction("Mode3", throwIfNotFound: true);
        // CharacterInput
        m_CharacterInput = asset.FindActionMap("CharacterInput", throwIfNotFound: true);
        m_CharacterInput_Move = m_CharacterInput.FindAction("Move", throwIfNotFound: true);
        m_CharacterInput_PlayerCamera = m_CharacterInput.FindAction("PlayerCamera", throwIfNotFound: true);
        // FreeLookCamera
        m_FreeLookCamera = asset.FindActionMap("FreeLookCamera", throwIfNotFound: true);
        m_FreeLookCamera_Movement = m_FreeLookCamera.FindAction("Movement", throwIfNotFound: true);
        m_FreeLookCamera_Lift = m_FreeLookCamera.FindAction("Lift", throwIfNotFound: true);
        m_FreeLookCamera_Rotate = m_FreeLookCamera.FindAction("Rotate", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Back;
    private readonly InputAction m_UI_Pause;
    private readonly InputAction m_UI_Confirm;
    private readonly InputAction m_UI_Space;
    private readonly InputAction m_UI_Mode1;
    private readonly InputAction m_UI_Mode2;
    private readonly InputAction m_UI_Mode3;
    public struct UIActions
    {
        private @PlayerInputs m_Wrapper;
        public UIActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Back => m_Wrapper.m_UI_Back;
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputAction @Confirm => m_Wrapper.m_UI_Confirm;
        public InputAction @Space => m_Wrapper.m_UI_Space;
        public InputAction @Mode1 => m_Wrapper.m_UI_Mode1;
        public InputAction @Mode2 => m_Wrapper.m_UI_Mode2;
        public InputAction @Mode3 => m_Wrapper.m_UI_Mode3;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Back.started -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @Pause.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Confirm.started -= m_Wrapper.m_UIActionsCallbackInterface.OnConfirm;
                @Confirm.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnConfirm;
                @Confirm.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnConfirm;
                @Space.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSpace;
                @Space.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSpace;
                @Space.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSpace;
                @Mode1.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMode1;
                @Mode1.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMode1;
                @Mode1.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMode1;
                @Mode2.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMode2;
                @Mode2.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMode2;
                @Mode2.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMode2;
                @Mode3.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMode3;
                @Mode3.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMode3;
                @Mode3.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMode3;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Confirm.started += instance.OnConfirm;
                @Confirm.performed += instance.OnConfirm;
                @Confirm.canceled += instance.OnConfirm;
                @Space.started += instance.OnSpace;
                @Space.performed += instance.OnSpace;
                @Space.canceled += instance.OnSpace;
                @Mode1.started += instance.OnMode1;
                @Mode1.performed += instance.OnMode1;
                @Mode1.canceled += instance.OnMode1;
                @Mode2.started += instance.OnMode2;
                @Mode2.performed += instance.OnMode2;
                @Mode2.canceled += instance.OnMode2;
                @Mode3.started += instance.OnMode3;
                @Mode3.performed += instance.OnMode3;
                @Mode3.canceled += instance.OnMode3;
            }
        }
    }
    public UIActions @UI => new UIActions(this);

    // CharacterInput
    private readonly InputActionMap m_CharacterInput;
    private ICharacterInputActions m_CharacterInputActionsCallbackInterface;
    private readonly InputAction m_CharacterInput_Move;
    private readonly InputAction m_CharacterInput_PlayerCamera;
    public struct CharacterInputActions
    {
        private @PlayerInputs m_Wrapper;
        public CharacterInputActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_CharacterInput_Move;
        public InputAction @PlayerCamera => m_Wrapper.m_CharacterInput_PlayerCamera;
        public InputActionMap Get() { return m_Wrapper.m_CharacterInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterInputActions set) { return set.Get(); }
        public void SetCallbacks(ICharacterInputActions instance)
        {
            if (m_Wrapper.m_CharacterInputActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CharacterInputActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CharacterInputActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CharacterInputActionsCallbackInterface.OnMove;
                @PlayerCamera.started -= m_Wrapper.m_CharacterInputActionsCallbackInterface.OnPlayerCamera;
                @PlayerCamera.performed -= m_Wrapper.m_CharacterInputActionsCallbackInterface.OnPlayerCamera;
                @PlayerCamera.canceled -= m_Wrapper.m_CharacterInputActionsCallbackInterface.OnPlayerCamera;
            }
            m_Wrapper.m_CharacterInputActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @PlayerCamera.started += instance.OnPlayerCamera;
                @PlayerCamera.performed += instance.OnPlayerCamera;
                @PlayerCamera.canceled += instance.OnPlayerCamera;
            }
        }
    }
    public CharacterInputActions @CharacterInput => new CharacterInputActions(this);

    // FreeLookCamera
    private readonly InputActionMap m_FreeLookCamera;
    private IFreeLookCameraActions m_FreeLookCameraActionsCallbackInterface;
    private readonly InputAction m_FreeLookCamera_Movement;
    private readonly InputAction m_FreeLookCamera_Lift;
    private readonly InputAction m_FreeLookCamera_Rotate;
    public struct FreeLookCameraActions
    {
        private @PlayerInputs m_Wrapper;
        public FreeLookCameraActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_FreeLookCamera_Movement;
        public InputAction @Lift => m_Wrapper.m_FreeLookCamera_Lift;
        public InputAction @Rotate => m_Wrapper.m_FreeLookCamera_Rotate;
        public InputActionMap Get() { return m_Wrapper.m_FreeLookCamera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FreeLookCameraActions set) { return set.Get(); }
        public void SetCallbacks(IFreeLookCameraActions instance)
        {
            if (m_Wrapper.m_FreeLookCameraActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_FreeLookCameraActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_FreeLookCameraActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_FreeLookCameraActionsCallbackInterface.OnMovement;
                @Lift.started -= m_Wrapper.m_FreeLookCameraActionsCallbackInterface.OnLift;
                @Lift.performed -= m_Wrapper.m_FreeLookCameraActionsCallbackInterface.OnLift;
                @Lift.canceled -= m_Wrapper.m_FreeLookCameraActionsCallbackInterface.OnLift;
                @Rotate.started -= m_Wrapper.m_FreeLookCameraActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_FreeLookCameraActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_FreeLookCameraActionsCallbackInterface.OnRotate;
            }
            m_Wrapper.m_FreeLookCameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Lift.started += instance.OnLift;
                @Lift.performed += instance.OnLift;
                @Lift.canceled += instance.OnLift;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
            }
        }
    }
    public FreeLookCameraActions @FreeLookCamera => new FreeLookCameraActions(this);
    public interface IUIActions
    {
        void OnBack(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnConfirm(InputAction.CallbackContext context);
        void OnSpace(InputAction.CallbackContext context);
        void OnMode1(InputAction.CallbackContext context);
        void OnMode2(InputAction.CallbackContext context);
        void OnMode3(InputAction.CallbackContext context);
    }
    public interface ICharacterInputActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnPlayerCamera(InputAction.CallbackContext context);
    }
    public interface IFreeLookCameraActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLift(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
    }
}
