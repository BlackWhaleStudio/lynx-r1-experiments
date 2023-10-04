//   ==============================================================================
//   | Lynx Interfaces (2023)                                                     |
//   |======================================                                      |
//   | LynxSwitchButton Script                                                    |
//   | Script to set a UI element as Switch Button.                               |
//   ==============================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lynx.UI
{
    public class LynxSwitchButton : Button
    {
        #region INSPECTOR VARIABLES

        // Button Parameters
        [SerializeField] private UnityEvent OnPress;
        [SerializeField] private UnityEvent OnUnpress;
        [SerializeField] public UnityEvent OnToggle;
        [SerializeField] public UnityEvent OnUntoggle;
        [SerializeField] private bool m_disableSelectState = true;

        // Switch Button Parameters
        [SerializeField] private Image m_backgroundTarget = null;
        [SerializeField] private Slider m_slider = null;
        [SerializeField] private float m_lerpSpeed = 15f;
        [SerializeField] private ButtonAnimation m_animation = new ButtonAnimation();

        #endregion

        #region PRIVATE VARIABLES

        private bool m_isRunning = false; // Avoid multiple press or unpress making the object in unstable state.
        private bool m_animationIsRunning = false; // Avoid multiple animations to start.
        private bool m_isCurrentlyPressed = false; // Status of the current object.
        private bool m_isToggle = false; // Status of the button.

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
            m_animationIsRunning = false;

            base.OnEnable();
            ButtonAnimationMethods.MoveRootAutoCompletion(m_animation, this.targetGraphic.transform);

            if (m_isToggle)
            {
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                base.OnPointerDown(eventData);
                base.OnDeselect(eventData);
            }

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

            if (!m_animationIsRunning)
            {
                m_animationIsRunning = true;
                if (m_isToggle)
                {
                    base.OnPointerUp(eventData);

                    m_isToggle = false;
                    StartCoroutine(ToggleAnimationCoroutine());

                    OnUntoggle.Invoke();
                }
                else
                {
                    base.OnPointerDown(eventData);

                    m_isToggle = true;
                    StartCoroutine(ToggleAnimationCoroutine());

                    OnToggle.Invoke();
                }
            }

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

        /// <summary>
        /// Call this function to return a boolean value as float.
        /// </summary>
        /// <param name="isOn">Boolean to convert.</param>
        private float ToFloat(bool isOn)
        {
            return (isOn) ? 1 : 0;
        }

        #endregion

        #region ANIMATION COROUTINES
        /// <summary>
        /// Start this coroutine to activate the switch toggle animation.
        /// </summary>
        private IEnumerator ToggleAnimationCoroutine()
        {
            float duration = 0.5f;
            float elapsedTime = 0.0f;
            //Color baseColor = backgroundImageActive.color;
            while (elapsedTime < duration)
            {
                m_slider.value = Mathf.Lerp(m_slider.value, ToFloat(m_isToggle), Time.deltaTime * m_lerpSpeed);
                Color baseColor = m_backgroundTarget.color;
                baseColor.a = m_slider.value;
                m_backgroundTarget.color = baseColor;
                yield return new WaitForEndOfFrame();
                elapsedTime += Time.deltaTime;
            }
            m_slider.value = ToFloat(m_isToggle);
            Color baseColorEnd = m_backgroundTarget.color;
            baseColorEnd.a = m_slider.value;
            m_backgroundTarget.color = baseColorEnd;
            m_animationIsRunning = false;
        }

        #endregion
    }
}