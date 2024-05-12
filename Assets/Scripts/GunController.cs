using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    LineRenderer laserLine;

    [SerializeField]
    float laserDuration = 0.05f;

    [SerializeField]
    float laserRange = 600f;

    [SerializeField]
    WorldBehaviors worldBehaviors;

   public bool shootGun()
    {
        if (Physics.Raycast(spawnPoint.position, transform.forward, out RaycastHit hit, laserRange))
        {
            laserLine.enabled = true;
            laserLine.SetPosition(0, spawnPoint.position);
            laserLine.SetPosition(1, hit.point);

            if (hit.transform.gameObject.tag == "Zombie")
            {
                StartCoroutine(ShootLaser());
                worldBehaviors.spawnedZombies.Remove(hit.transform.parent.GetComponent<ZombieController>());
                Destroy(hit.transform.parent.gameObject);
                return true;
            } else if (hit.collider.gameObject.tag == "Wall")
            {
                StartCoroutine(ShootLaser());
                return false;
            }
        }
        StartCoroutine(ShootLaser());
        return false;
    }

    private IEnumerator ShootLaser()
    {
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}
