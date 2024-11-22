using System.Collections;
using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    MeshRenderer meshRenderer;
    SphereCollider sphereCollider;
    Transform meshTransform;


    private void Awake()
    {
        meshTransform = transform.GetChild(0);
        meshRenderer = meshTransform.GetComponent<MeshRenderer>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        EnableItem(false);
        StartCoroutine(CooldownToRespawn());
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * 250 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnableItem(false);
            StartCoroutine(CooldownToRespawn());
            GameManager.Instance.AddPlayerHealth();
        }
    }

    void EnableItem(bool enable)
    {
        meshRenderer.enabled = enable;
        sphereCollider.enabled = enable;
    }

    IEnumerator CooldownToRespawn()
    {
        yield return new WaitForSeconds(120);
        EnableItem(true);
    }

}
