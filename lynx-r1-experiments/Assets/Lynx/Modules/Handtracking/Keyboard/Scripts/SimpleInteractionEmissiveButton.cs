/******************************************************************************
 * Copyright (C) Ultraleap, Inc. 2011-2020.                                   *
 * Ultraleap proprietary and confidential.                                    *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Ultraleap and you, your company or other organization.             *
 ******************************************************************************/

using Leap.Unity;
using Leap.Unity.Interaction;
using UnityEngine;

/// <summary>
/// This simple script changes the color of an InteractionBehaviour as
/// a function of its distance to the palm of the closest hand that is
/// hovering nearby.
/// </summary>
[AddComponentMenu("")]
[RequireComponent(typeof(InteractionBehaviour))]
public class SimpleInteractionEmissiveButton : MonoBehaviour
{

    [Tooltip("If enabled, the object will lerp to its hoverColor when a hand is nearby.")]
    public bool useHover = true;

    [Tooltip("If enabled, the object will use its primaryHoverColor when the primary hover of an InteractionHand.")]
    public bool usePrimaryHover = false;

    [Header("InteractionBehaviour Colors")]
    public Color defaultColor = Color.black;
    public Color suspendedColor = new Color(84.0f / 255.0f, 84.0f / 255.0f, 84.0f / 255.0f);
    public Color hoverColor = new Color(149.0f / 255.0f, 149.0f / 255.0f, 149.0f / 255.0f);
    public Color primaryHoverColor = new Color(233.0f / 255.0f, 233.0f / 255.0f, 233.0f / 255.0f);

    [Header("InteractionButton Colors")]
    [Tooltip("This color only applies if the object is an InteractionButton or InteractionSlider.")]
    public Color pressedColor = new Color(0, 145.0f / 255.0f, 1.0f);

    private Material _material;
    private int _emissionColorId;

    private InteractionBehaviour _intObj;

    void Start()
    {
        _intObj = GetComponent<InteractionBehaviour>();

        Renderer renderer = GetComponent<Renderer>();

        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
        }

        if (renderer != null)
        {
            _material = renderer.material;

            if (_material.HasProperty("_EmissionColor"))
            {
                _emissionColorId = Shader.PropertyToID("_EmissionColor");
            }
            else
            {
                _emissionColorId = -1;
            }
        }
    }


    void Update()
    {
        if (_material != null)
        {

            // The target color for the Interaction object will be determined by various simple state checks.
            Color targetColor = defaultColor;

            // "Primary hover" is a special kind of hover state that an InteractionBehaviour can
            // only have if an InteractionHand's thumb, index, or middle finger is closer to it
            // than any other interaction object.
            if (_intObj.isPrimaryHovered && usePrimaryHover)
            {
                targetColor = primaryHoverColor;
            }
            else
            {
                // Of course, any number of objects can be hovered by any number of InteractionHands.
                // InteractionBehaviour provides an API for accessing various interaction-related
                // state information such as the closest hand that is hovering nearby, if the object
                // is hovered at all.
                if (_intObj.isHovered && useHover)
                {
                    float glow = _intObj.closestHoveringControllerDistance.Map(0F, 0.2F, 1F, 0.0F);
                    targetColor = Color.Lerp(defaultColor, hoverColor, glow);
                }
            }

            if (_intObj.isSuspended)
            {
                // If the object is held by only one hand and that holding hand stops tracking, the
                // object is "suspended." InteractionBehaviour provides suspension callbacks if you'd
                // like the object to, for example, disappear, when the object is suspended.
                // Alternatively you can check "isSuspended" at any time.
                targetColor = suspendedColor;
            }

            // We can also check the depressed-or-not-depressed state of InteractionButton objects
            // and assign them a unique color in that case.
            if (_intObj is InteractionButton && (_intObj as InteractionButton).isPressed)
            {
                targetColor = pressedColor;
            }

            // Lerp actual material color to the target color.
            if (_emissionColorId != -1)
            {
                _material.SetColor(_emissionColorId, Color.Lerp(_material.GetColor(_emissionColorId), targetColor, 20F * Time.deltaTime));
            }
            else
            {
                _material.color = Color.Lerp(_material.color, targetColor, 20F * Time.deltaTime);
            }

        }
    }

}
