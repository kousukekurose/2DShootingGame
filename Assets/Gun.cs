using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float _shotSpeed = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShotSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,_shotSpeed* Time.deltaTime,0) ;
    }

    public void ShotSpeed()
    {
        StartCoroutine(OnShotSpeed());
    }

    private IEnumerator OnShotSpeed()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
