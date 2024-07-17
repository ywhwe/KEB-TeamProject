using System.Collections;
using UnityEngine;

public class Trolley : MonoBehaviour
{
    public GameObject wheel1;
    public GameObject wheel2;
    public GameObject wheel3;
    public GameObject wheel4;
    private WaitForSeconds wait = new (0.1f);
    private float angle = 7f;

    private void Start()
    {
        StartCoroutine(Spin());
    }
    
    // Make wheels spin
    private IEnumerator Spin()
    {
        while (BtnAction.actionInstance.successCount < 10)
        {
            yield return wait;

            if (BtnController.ctrlInstance.IsMatch) angle = 34f;
            
            wheel1.transform.Rotate(Vector3.back, angle, Space.World);
            wheel2.transform.Rotate(Vector3.back, angle, Space.World);
            wheel3.transform.Rotate(Vector3.back, angle, Space.World);
            wheel4.transform.Rotate(Vector3.back, angle, Space.World);

            angle = 7f;
        }
    }
}
