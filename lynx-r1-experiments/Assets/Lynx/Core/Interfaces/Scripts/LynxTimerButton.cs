//   ==============================================================================
//   | Lynx Interfaces (2023)                                                     |
//   |======================================                                      |
//   | LynxTimerButton Script                                                     |
//   | Script to set a UI element as Timer Button.                                |
//   ==============================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lynx.UI
{
    public class LynxTimerButton : Button
    {
        #region INSPECTOR VARIABLES

        // Button Parameters
        [SerializeField] private UnityEvent OnPress;
        [SerializeField] private UnityEvent OnUnpress;
        [SerializeField] public UnityEvent OnTimerPress;
        [SerializeField] private bool m_disableSelectState = true;

        // Timer Button Parameters
        [SerializeField] private Image m_timerImage = null;
        [SerializeField] private float m_deltaTime = 2.0f;
        [SerializeField] private ButtonAnimation m_animation = new ButtonAnimation();

        #endregion

        #region PRIVATE VARIABLES

        private bool m_isRunning = false; // Avoid multiple press or unpress making the object in unstable state.
        private bool m_isCurrentlyPressed = false; // Status of the current object.
        private bool m_timerIsRunning = false; // Status of the timer.
        
        private IEnumerator timerCoroutine = null; // Timer coroutine reference.

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

            if(!m_isRunning && !m_isCurrentlyPressed)
            {
                m_isRunning = true;
                StartCoroutine(ButtonAnimationMethods.PressingAnimationCoroutine(m_animation, this.transform, CallbackStopRunning));
                m_isCurrentlyPressed = true;
            }

            if (!m_timerIsRunning)
            {
                m_timerIsRunning = true;
                timerCoroutine = TimerAnimationCoroutine();
                StartCoroutine(timerCoroutine);
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

            m_timerIsRunning = false;
            StopCoroutine(timerCoroutine);

            m_timerImage.fillAmount = 0.0f;
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

        #region ANIMATION COROUTINES

        /// <summary>
        /// Start this coroutine to launch the fill animation.
        /// </summary>
        private IEnumerator TimerAnimationCoroutine()
        {
            float elapsedTime = 0.0f;

            while (elapsedTime < m_deltaTime)
            {
                elapsedTime += Time.deltaTime;
                m_timerImage.fillAmount = elapsedTime / m_deltaTime;
                yield return new WaitForEndOfFrame();
            }

            m_timerImage.fillAmount = 0.0f;


            OnTimerPress.Invoke();

            m_timerIsRunning = false;
        }

        #endregion
    }
}
