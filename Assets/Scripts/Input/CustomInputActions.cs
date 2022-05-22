//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Input/CustomInputActions.inputactions
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

public partial class @CustomInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @CustomInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CustomInputActions"",
    ""maps"": [
        {
            ""name"": ""User"",
            ""id"": ""28404352-985f-4d04-ac45-35dc5f3a4e35"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e2d452ee-e959-48cf-9e92-19573ebb5769"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Mouse"",
                    ""type"": ""PassThrough"",
                    ""id"": ""71c4276a-c697-4390-81fc-6c98ebfdb8bb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""eb78aae2-dcb7-4873-8f39-cf097508b496"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""fa130567-463a-424a-a1db-bb4f8d380a16"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a8f34348-55b4-4954-8267-d02aecf64485"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseLeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""6d9fe429-14db-487c-8e7f-26f3804eb320"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseRightClick"",
                    ""type"": ""Button"",
                    ""id"": ""be71a9ab-6002-4527-b983-346a4ad231d2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseMiddleClick"",
                    ""type"": ""Button"",
                    ""id"": ""47cb44ab-0524-4d21-b90d-a814a3048ec9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""c7f03a8e-cd1f-4736-aaf3-a4486bd4eb1e"",
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
                    ""id"": ""9223aaf7-0735-4571-b732-fbae36ba2382"",
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
                    ""id"": ""4631ac69-7cba-4661-89b2-55813e01cb88"",
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
                    ""id"": ""161f3763-c4da-4131-928f-c02660d6f465"",
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
                    ""id"": ""36d19dba-c0ec-40c0-b3a5-c9cfc5eb2498"",
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
                    ""id"": ""31685130-6d98-45b1-a8ad-b4807b0ac9f1"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f47f0dc1-d6f0-4b00-a46d-7b274088d538"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d031ebdc-c0b3-4e6e-9adf-eadf910f8975"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae50de55-73c5-4a10-a81b-40a038466522"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6706170-141a-4b1c-b4a5-7cc55cc82ccc"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b29fda2a-374b-4df2-a8d3-39bf71247cdf"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""efa903f6-2a5c-4125-a962-f23430644e46"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseLeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5657a153-35d7-4db3-a81f-06738732c780"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseRightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9b77b3c-f750-41fd-949e-524cd6102c37"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // User
        m_User = asset.FindActionMap("User", throwIfNotFound: true);
        m_User_Move = m_User.FindAction("Move", throwIfNotFound: true);
        m_User_Mouse = m_User.FindAction("Mouse", throwIfNotFound: true);
        m_User_Escape = m_User.FindAction("Escape", throwIfNotFound: true);
        m_User_Sprint = m_User.FindAction("Sprint", throwIfNotFound: true);
        m_User_Jump = m_User.FindAction("Jump", throwIfNotFound: true);
        m_User_MouseLeftClick = m_User.FindAction("MouseLeftClick", throwIfNotFound: true);
        m_User_MouseRightClick = m_User.FindAction("MouseRightClick", throwIfNotFound: true);
        m_User_MouseMiddleClick = m_User.FindAction("MouseMiddleClick", throwIfNotFound: true);
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

    // User
    private readonly InputActionMap m_User;
    private IUserActions m_UserActionsCallbackInterface;
    private readonly InputAction m_User_Move;
    private readonly InputAction m_User_Mouse;
    private readonly InputAction m_User_Escape;
    private readonly InputAction m_User_Sprint;
    private readonly InputAction m_User_Jump;
    private readonly InputAction m_User_MouseLeftClick;
    private readonly InputAction m_User_MouseRightClick;
    private readonly InputAction m_User_MouseMiddleClick;
    public struct UserActions
    {
        private @CustomInputActions m_Wrapper;
        public UserActions(@CustomInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_User_Move;
        public InputAction @Mouse => m_Wrapper.m_User_Mouse;
        public InputAction @Escape => m_Wrapper.m_User_Escape;
        public InputAction @Sprint => m_Wrapper.m_User_Sprint;
        public InputAction @Jump => m_Wrapper.m_User_Jump;
        public InputAction @MouseLeftClick => m_Wrapper.m_User_MouseLeftClick;
        public InputAction @MouseRightClick => m_Wrapper.m_User_MouseRightClick;
        public InputAction @MouseMiddleClick => m_Wrapper.m_User_MouseMiddleClick;
        public InputActionMap Get() { return m_Wrapper.m_User; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UserActions set) { return set.Get(); }
        public void SetCallbacks(IUserActions instance)
        {
            if (m_Wrapper.m_UserActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_UserActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_UserActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_UserActionsCallbackInterface.OnMove;
                @Mouse.started -= m_Wrapper.m_UserActionsCallbackInterface.OnMouse;
                @Mouse.performed -= m_Wrapper.m_UserActionsCallbackInterface.OnMouse;
                @Mouse.canceled -= m_Wrapper.m_UserActionsCallbackInterface.OnMouse;
                @Escape.started -= m_Wrapper.m_UserActionsCallbackInterface.OnEscape;
                @Escape.performed -= m_Wrapper.m_UserActionsCallbackInterface.OnEscape;
                @Escape.canceled -= m_Wrapper.m_UserActionsCallbackInterface.OnEscape;
                @Sprint.started -= m_Wrapper.m_UserActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_UserActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_UserActionsCallbackInterface.OnSprint;
                @Jump.started -= m_Wrapper.m_UserActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_UserActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_UserActionsCallbackInterface.OnJump;
                @MouseLeftClick.started -= m_Wrapper.m_UserActionsCallbackInterface.OnMouseLeftClick;
                @MouseLeftClick.performed -= m_Wrapper.m_UserActionsCallbackInterface.OnMouseLeftClick;
                @MouseLeftClick.canceled -= m_Wrapper.m_UserActionsCallbackInterface.OnMouseLeftClick;
                @MouseRightClick.started -= m_Wrapper.m_UserActionsCallbackInterface.OnMouseRightClick;
                @MouseRightClick.performed -= m_Wrapper.m_UserActionsCallbackInterface.OnMouseRightClick;
                @MouseRightClick.canceled -= m_Wrapper.m_UserActionsCallbackInterface.OnMouseRightClick;
                @MouseMiddleClick.started -= m_Wrapper.m_UserActionsCallbackInterface.OnMouseMiddleClick;
                @MouseMiddleClick.performed -= m_Wrapper.m_UserActionsCallbackInterface.OnMouseMiddleClick;
                @MouseMiddleClick.canceled -= m_Wrapper.m_UserActionsCallbackInterface.OnMouseMiddleClick;
            }
            m_Wrapper.m_UserActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Mouse.started += instance.OnMouse;
                @Mouse.performed += instance.OnMouse;
                @Mouse.canceled += instance.OnMouse;
                @Escape.started += instance.OnEscape;
                @Escape.performed += instance.OnEscape;
                @Escape.canceled += instance.OnEscape;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @MouseLeftClick.started += instance.OnMouseLeftClick;
                @MouseLeftClick.performed += instance.OnMouseLeftClick;
                @MouseLeftClick.canceled += instance.OnMouseLeftClick;
                @MouseRightClick.started += instance.OnMouseRightClick;
                @MouseRightClick.performed += instance.OnMouseRightClick;
                @MouseRightClick.canceled += instance.OnMouseRightClick;
                @MouseMiddleClick.started += instance.OnMouseMiddleClick;
                @MouseMiddleClick.performed += instance.OnMouseMiddleClick;
                @MouseMiddleClick.canceled += instance.OnMouseMiddleClick;
            }
        }
    }
    public UserActions @User => new UserActions(this);
    public interface IUserActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnMouse(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnMouseLeftClick(InputAction.CallbackContext context);
        void OnMouseRightClick(InputAction.CallbackContext context);
        void OnMouseMiddleClick(InputAction.CallbackContext context);
    }
}
