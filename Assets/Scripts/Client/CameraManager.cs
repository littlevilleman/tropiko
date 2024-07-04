using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Volume postProcess;

    private ChromaticAberration aberration;

    private void Start()
    {
        postProcess.sharedProfile.TryGet(out aberration);
    }
    public void Shake()
    {
        cam.DOShakePosition(.25f, .5f, 20);
        float abInt = 0f;// aberration.intensity.value;
        //aberration.intensity = new ClampedFloatParameter();


        DOTween.To(() => abInt, x => abInt = x, .25f, .125f).SetLoops(2, LoopType.Yoyo).OnUpdate(OnUpdate).Play();

        void OnUpdate()
        {
            aberration.intensity.Override(abInt);
        }
    }
}
