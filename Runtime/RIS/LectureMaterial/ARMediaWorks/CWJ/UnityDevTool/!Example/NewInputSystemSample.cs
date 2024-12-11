//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.Interactions;
//using UnityEngine.AI;

//using CWJ;
//using CWJ.VR;

//[RequireComponent(typeof(NavMeshAgent))]
//public class PlayerNavMeshMoveSystem : MonoBehaviour
//{
//    public Transform moveTrf;
//    public Transform lookTrf;

//    [GetComponent] public NavMeshAgent navMeshAgent;
//    //public float moveSmoothing = 10;
//    public float rotateSpeed = 60;
//    public float burstSpeed = 10;
//    public GameObject projectile;
//    [Readonly] public bool isCharging;

//    private MocaInputSetting inputSetting;

//    protected void Awake()
//    {
//        VR_Manager.Instance.AddEvent_Enabled(OnVrInit);

//        inputSetting = new MocaInputSetting();

//        //navMeshAgent.updatePosition = false;

//        rotationDir = lookTrf.localEulerAngles;
//    }

//    private void OnVrInit(bool isEnabled)
//    {
//        if (isEnabled)
//        {
//            VR_Manager.Instance.AddEvent_HmdOnHead((isOnHead) =>
//            {
//                this.enabled = !isOnHead;
//            });
//        }
//        else
//        {
//            this.enabled = true;
//        }
//        VR_Manager.Instance.RemoveEvent_Enabled(OnVrInit);
//    }

//    protected void OnEnable()
//    {
//        inputSetting.PlayerController.Interaction.started += Interaction_Started;
//        inputSetting.PlayerController.Interaction.performed += Interaction_Performed;
//        inputSetting.PlayerController.Interaction.canceled += Interaction_Canceled;
//        inputSetting.Enable();
//    }

//    protected void OnDisable()
//    {
//        if (inputSetting == null) return;

//        inputSetting.PlayerController.Interaction.started -= Interaction_Started;
//        inputSetting.PlayerController.Interaction.performed -= Interaction_Performed;
//        inputSetting.PlayerController.Interaction.canceled -= Interaction_Canceled;
//        inputSetting.Disable();
//    }

//    protected void Update()
//    {
//        OnLook(inputSetting.PlayerController.Look.ReadValue<Vector2>());
//        OnMove(inputSetting.PlayerController.Move.ReadValue<Vector2>());
//    }

//    private void OnMove(Vector2 direction)
//    {
//        if (direction.sqrMagnitude < 0.01f) return;

//        Vector3 moveDir = Quaternion.Euler(0, lookTrf.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
//        navMeshAgent.Move(moveDir * navMeshAgent.speed * Time.deltaTime);
//        //moveTrf.position = Vector3.Lerp(moveTrf.position, navMeshAgent.nextPosition, moveSmoothing);
//    }

//    private Vector2 rotationDir;

//    private void OnLook(Vector2 rotate)
//    {
//        if (rotate.sqrMagnitude < 0.01f) return;

//        rotate *= rotateSpeed * Time.deltaTime;
//        rotationDir.y += rotate.x;
//        rotationDir.x = Mathf.Clamp(rotationDir.x - rotate.y, -89, 89);

//        lookTrf.localEulerAngles = new Vector3(rotationDir.x, rotationDir.y, lookTrf.localEulerAngles.z);
//    }

//    private void Interaction_Performed(InputAction.CallbackContext callbackContext)
//    {
//        if (callbackContext.interaction is SlowTapInteraction)
//        {
//            StartCoroutine(BurstFire((int)(callbackContext.duration * burstSpeed)));
//        }
//        else
//        {
//            FireProcess();
//        }
//        isCharging = false;
//    }

//    private void Interaction_Started(InputAction.CallbackContext callbackContext)
//    {
//        if (callbackContext.interaction is SlowTapInteraction) isCharging = true;
//    }

//    private void Interaction_Canceled(InputAction.CallbackContext callbackContext)
//    {
//        isCharging = false;
//    }

//    private IEnumerator BurstFire(int burstCount)
//    {
//        for (var i = 0; i < burstCount; ++i)
//        {
//            FireProcess();
//            yield return new WaitForSeconds(0.1f);
//        }
//    }

//    const int size = 1;
//    private void FireProcess()
//    {
//        GameObject newProjectile = Instantiate(projectile);
//        newProjectile.transform.position = lookTrf.position + lookTrf.forward * 0.6f;
//        newProjectile.transform.rotation = lookTrf.rotation;

//        newProjectile.transform.localScale *= size;
//        newProjectile.GetComponent<Rigidbody>().mass = Mathf.Pow(size, 3);
//        newProjectile.GetComponent<Rigidbody>().AddForce(lookTrf.forward * 20f, ForceMode.Impulse);
//        newProjectile.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value, 1.0f);
//    }
//}