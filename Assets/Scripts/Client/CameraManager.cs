using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Volume postProcess;

    private ChromaticAberration aberration;
    private Vector3 initialPosition;

    private void Start()
    {
        postProcess.sharedProfile.TryGet(out aberration);
        initialPosition = transform.position;
    }
    public void Shake(int strenght)
    {
        cam.DOShakePosition(.25f, Mathf.Lerp(.5f, 2f, strenght), 20).OnComplete(() => transform.position = initialPosition);
        float abInt = 0f;


        DOTween.To(() => abInt, x => abInt = x, .25f, .125f).SetLoops(2, LoopType.Yoyo).OnUpdate(OnUpdate).Play();
        
        void OnUpdate()
        {
            aberration.intensity.Override(abInt);
        }
    }
}
