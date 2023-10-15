using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] float timeAcive = 3f;
    bool isShieldActive;
    private void Start()
    {
        isShieldActive = false;
        gameObject.SetActive(isShieldActive);
    }
    public void ActiveShield()
    {
        isShieldActive = !isShieldActive;
        OnActiveShield();
        gameObject.SetActive(isShieldActive);
    }
    private void OnActiveShield()
    {

        TankController tankController = GetComponentInParent<TankController>();
        if (tankController != null)
        {
            tankController.isShieldActive = isShieldActive;
        }
    }
    private void OnEnable()
    {
        StartCoroutine(DeActiveShield());
    }
    IEnumerator DeActiveShield()
    {
        yield return new WaitForSeconds(timeAcive);
        ActiveShield();
    }
}
