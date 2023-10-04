//   ==============================================================================
//   | Lynx Interfaces (2023)                                                     |
//   |======================================                                      |
//   | LynxSimpleButton Script                                                    |
//   | Script to set a UI element as Simple Button.                               |
//   ==============================================================================

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lynx.UI
{
    public class LynxSimpleButton : Button
    {
        #region INSPECTOR VARIABLES

        // Button Parameters
        [SerializeField] private UnityEvent OnPress;
        [SerializeField] private UnityEvent OnUnpress;
        [SerializeField] private bool m_disableSelectState = true;

        // Simple Button Parameters
        [SerializeField] private ButtonAnimation m_animation = new ButtonAnimation();

        #endregion

        #region PRIVATE VARIABLES

        private bool m_isRunning = false; // Avoid multiple press or unpress making the object in unstable state.
        private bool m_isCurrentlyPressed = false; // Status of the current object.

        #endregion

        #region UNITY API

        // Start is called before the first frame update.
        protected override void Start()
        {
            base.Start();
            ButtonAnimationMethods.MoveRootAutoCompletion(m_animation, this.targetGraphic.transform);
        }

        // OnEnable is called when the object becomes enabled and active.
        protected override void OnEnable()
        {
            base.OnEnable();
            ButtonAnimationMethods.MoveRootAutoCompletion(m_animation, this.targetGraphic.transform);
            ButtonAnimationMethods.SetMoveRoot(m_animation, this.transform);
        }

        // OnSelect is called when the selectable UI object is selected.
        public override void OnSelect(BaseEventData eventData)
        {
            // State Select can affect the expected behaviour of the button.
            // It is natively deactivated on our buttons.
            // But can be reactivated by unchecking disableSelectState
            
            if (m_disableSelectState)
            {
                base.OnDeselect(eventData);
            }
            else
            {
                base.OnSelect(eventData);
            }
        }

        // OnPointerDown is called when the mouse is clicked over this selectable UI object.
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!IsInteractable()) return;

            base.OnPointerDown(eventData);

            if (!m_isRunning && !m_isCurrentlyPressed)
            {
                m_isRunning = true;
                StartCoroutine(ButtonAnimationMethods.PressingAnimationCoroutine(m_animation, this.transform, CallbackStopRunning));
                m_isCurrentlyPressed = true;
            }
        }

        // OnPointerUp is called when the mouse click on this selectable UI object is released.
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!IsInteractable()) return;

            base.OnPointerUp(eventData);

            if (m_isCurrentlyPressed)
            {
                m_isRunning = true;
                StartCoroutine(ButtonAnimationMethods.UnpressingAnimationCoroutine(m_animation, this.transform, CallbackStopRunning));
                m_isCurrentlyPressed = false;
            }
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// CallbackStopRunning is called when a button animation coroutine is complete.
        /// </summary>
        /// <param name="state">True to call OnUnpress, false to call OnPress.</param>
        private void CallbackStopRunning(bool state)
        {
            m_isRunning = false;

            if (state)
            {
                OnUnpress.Invoke();
            }
            else
            {
                OnPress.Invoke();
            }
        }

        #endregion
    }
}