using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaScript : MonoBehaviour
{
    bool _isScaling = false;
    Transform _transform;

    float _damage = 4;

    float _lifeTime = 7;
    float _start;
    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        StartCoroutine(scaleOverTime(_transform, new Vector3(2.5f,2.5f,0), 2));
        _start = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _start >= _lifeTime)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator scaleOverTime(Transform objectToScale, Vector3 toScale, float duration)
    {
        //Make sure there is only one instance of this function running
        if (_isScaling)
        {
            yield break; ///exit if this is still running
        }
        _isScaling = true;

        float counter = 0;

        //Get the current scale of the object to be moved
        Vector3 startScaleSize = objectToScale.localScale;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectToScale.localScale = Vector3.Lerp(startScaleSize, toScale, counter / duration);
            yield return null;
        }

        _isScaling = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerController>() != null)
            {
                collision.GetComponent<PlayerController>().Damage(_damage);
            }
        }
    }
}
