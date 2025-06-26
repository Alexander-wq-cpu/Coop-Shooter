using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Header("Weapon Rotation")]
    private Vector3 targetRotation;
    private Vector3 currentRotation;

    [Range(0,2)]
    [SerializeField] private float xRecoil;

    [SerializeField] private float yRecoil;

    [Range(1,15)]
    [SerializeField] private float roationReturnSpeed;

    [Header("WeaponPosition")]
    private Vector3 weaponStartPostion;
    private Vector3 targetPosition;

    [Range(0,0.04f)]
    [SerializeField] private float zPosition;
    [Range(1, 8)]
    [SerializeField] private float positionReturnSpeed;

    private void Start()
    {
        weaponStartPostion = transform.localPosition;
        targetPosition = weaponStartPostion;
    }
    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * roationReturnSpeed);
        transform.localRotation = Quaternion.Euler(targetRotation);

        targetPosition = Vector3.Lerp(targetPosition, weaponStartPostion, Time.deltaTime * positionReturnSpeed);
        transform.localPosition = targetPosition;
    }

    public void doWeaponRecoil()
    {
        targetRotation += new Vector3(-xRecoil, Random.Range(-yRecoil,yRecoil), 0);
        targetPosition += new Vector3(0, 0, -zPosition);
    }
}
