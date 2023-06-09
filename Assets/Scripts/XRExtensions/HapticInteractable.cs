using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticInteractable : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float _hapticImpulseIntensity;
    [SerializeField] private float _hapticImpulseDuration;

    private void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.activated.AddListener(TriggerHapctic);
    }

    private void TriggerHapctic(BaseInteractionEventArgs  eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            TriggerHapctic(controllerInteractor.xrController);
        }
    }

    private void TriggerHapctic(XRBaseController controller)
    {
        if (_hapticImpulseIntensity > 0)
        {
            controller.SendHapticImpulse(_hapticImpulseIntensity, _hapticImpulseDuration);
        }
            
    }
}
