using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnim : MonoBehaviour
{
    [SerializeField]
    GameObject _hitParticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Hit()
    {
        Instantiate(_hitParticle, gameObject.transform.parent);
    }
}
