using UnityEngine;

public class UIAnimation : MonoBehaviour
{

    [SerializeField] private GameObject textAnim;



    private void Update()
    {
        if (TimeManager.Instance.ElapsedTime >= 5f)
        {
            CloseAnim();
        }
        if (TimeManager.Instance.ElapsedTime >= 5.5f)
        {
            textAnim.SetActive(false);
            this.enabled = false;
        }
    }

    public void CloseAnim()
    {
        textAnim.GetComponent<Animator>().SetTrigger("Close");
    }


}
