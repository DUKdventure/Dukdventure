using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitchAnimator : MonoBehaviour
{
    public Toggle toggle;
    public Animator animator;

    readonly int hashIsOn = Animator.StringToHash("IsOn");

    void Awake()
    {
        if (toggle == null)
            toggle = GetComponent<Toggle>();

        if (animator == null)
            animator = GetComponent<Animator>();

        //시작 상태 동기화
        animator.SetBool(hashIsOn, toggle.isOn);

        //값이 바뀔 때마다 애니메이션 파라미터 갱신
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        animator.SetBool(hashIsOn, isOn);
    }

}
