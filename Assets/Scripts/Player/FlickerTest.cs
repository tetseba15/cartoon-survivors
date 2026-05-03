using UnityEngine;

public class FlickerTest : MonoBehaviour
{
    private SpriteRenderer[] childs;

    private void Start()
    {
        childs = GetComponentsInChildren<SpriteRenderer>();
    }

    public void Ouch()
    {
        for(int i = 0; i < childs.Length; i++)
        {
            childs[i].enabled = false;
        }
    }

}
