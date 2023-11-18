using UnityEngine;
using System;
using DG.Tweening;
namespace MarwanZaky
{
    public class PlayerMovement : Character
    {
        public enum MoveAir { Moveable, NotMoveable }

        public static Action<int> OnCurrentControllerChange;
        public static Action OnAttack;

        const float GRAVITY = -9.81f;
        const bool DEBUG = true;

        Collider col;
        Transform cam;

        Vector3 velocity = Vector3.zero;

        int currentController = 0;

        bool isGrounded = false;
        bool wasGrounded = false;

        [Header("Properties"), SerializeField] CharacterController controller;
        [SerializeField] Animator animator;
        [SerializeField] float walkSpeed = 5f;
        [SerializeField] float runSpeed = 10f;
        [SerializeField] float gravityScale = 1f;
        [SerializeField] float jumpHeight = 8f;

        [Header("Settings"), SerializeField] CursorLockMode cursorLockMode = CursorLockMode.None;
        [SerializeField] LayerMask groundMask;
        [SerializeField] MoveAir moveAir = MoveAir.Moveable;
        [SerializeField] AnimatorOverrideController[] controllers;

        [Header("Controlls"), SerializeField] KeyCode jumpKeyCode = KeyCode.Space;
        [SerializeField] KeyCode runKeyCode = KeyCode.LeftShift;
        [SerializeField] KeyCode attackKeyCode = KeyCode.Mouse0;
        [SerializeField] KeyCode granadeKeyCode = KeyCode.Mouse1;
        [SerializeField] KeyCode portalGear = KeyCode.KeypadEnter;

        public float Speed => IsRunning ? runSpeed : walkSpeed;
        public bool IsRunning => Input.GetKey(runKeyCode);
        public bool IsMoving { get; set; }

        public GameObject cursor;
        public Vector3 posY;

        public StackManager healthManager;
        public GameObject grenadeObject;
        public float launchSpeed = 10f;
        public int linePoints = 175;
        private void Start()
        {
            Cursor.lockState = cursorLockMode;

            col = controller.GetComponent<Collider>();
            cam = Camera.main.transform;

            UpdateCurrentController(0);     // select default controller


        }

        private void OnEnable()
        {
            OnCurrentControllerChange += UpdateCurrentController;
        }

        private void OnDisable()
        {
            OnCurrentControllerChange -= UpdateCurrentController;
        }

        private void OnGUI()
        {
            var buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 18;

            //if (GUI.Button(new Rect(50, 32, 200, 32), $"Cursor Locked (M)", buttonStyle))
            //    ToggleCursorLockState();

            //if (GUI.Button(new Rect(50, 69, 200, 32), "Movement (1)", buttonStyle))
            //    OnCurrentControllerChange?.Invoke(0);

            //if (GUI.Button(new Rect(50, 106, 200, 32), "Sword (2)", buttonStyle))
            //    OnCurrentControllerChange?.Invoke(1);

            //if (GUI.Button(new Rect(50, 143, 200, 32), "Gun (3)", buttonStyle))
            //{
              
            //    OnCurrentControllerChange?.Invoke(2);
                
            //}
        }

        private void Update()
        {
            IsGrounded();
            Inputs();

            if (IsMoving)
                LookAtCamera();

            Gravity();
            Movement();

            controller.Move(velocity * Time.deltaTime);


            if (Input.GetKeyDown(KeyCode.R))
            {
                if(healthManager.reloadBullet > 0)
                {
                    //Reload
                    float x = 10 - healthManager.currentAmmo;
                    healthManager.reloadBullet -= x;
                    healthManager.currentAmmo += x; 

                }
               
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                //Dash
                SoundManager.Instance.SoundPlay(3);
            }

           
        }

        private void IsGrounded()
        {
            // Is grounded
            isGrounded = IsGroundedSphere(col, controller.radius, groundMask, true);

            if (isGrounded && velocity.y < 0)
                velocity.y = -5f;

            // Play sound fx on land
            if (isGrounded && !wasGrounded)
                AudioManager.Instance.Play("Land");

            wasGrounded = isGrounded;
            animator.SetBool("Float", !isGrounded);
        }

        private void Inputs()
        {
            if (Input.GetKeyDown(jumpKeyCode) && isGrounded)
                Jump();

            if (Input.GetKeyDown(attackKeyCode))
            {
                if(healthManager.currentAmmo > 0 ) 
                {
                    Attack();
                }
            }

            if(Input.GetKeyDown(granadeKeyCode))
            {
                if(healthManager.grenadeCount>0)
                {
                    AttackGranade();
                }
                else
                {
                    return;
                }
            }

            if (Input.GetKeyDown(portalGear))
            {
                if(healthManager.gearCount==6)
                {
                    PutTheGear();
                }
            }

            // Scroll between controllers
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                UseNextController();
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                UsePreviousController();

            // Toggle cursor lock state
            if (Input.GetKeyDown(KeyCode.M))
                ToggleCursorLockState();

            // Switch controllers.
            if (Input.GetKeyDown(KeyCode.Alpha1))
                OnCurrentControllerChange?.Invoke(0);

            else if (Input.GetKeyDown(KeyCode.Alpha2))
                OnCurrentControllerChange?.Invoke(1);

            else if (Input.GetKeyDown(KeyCode.Alpha3))
                OnCurrentControllerChange?.Invoke(2);

        }

        private void Gravity()
        {
            velocity.y = velocity.y + GRAVITY * gravityScale * Time.deltaTime;
        }

        private void Movement()
        {
            const float IS_MOVING_MIN_MAG = .02f;

            if (moveAir == MoveAir.NotMoveable && !isGrounded)
                return;

            var moveX = Input.GetAxis("Horizontal");
            var moveY = Input.GetAxis("Vertical");
            var move = (transform.right * moveX + transform.forward * moveY).normalized;

            animator.SetFloat("MoveX", GetAnimMoveVal(moveX, animator.GetFloat("MoveX")));
            animator.SetFloat("MoveY", GetAnimMoveVal(moveY, animator.GetFloat("MoveY")));

            controller.Move(move * Speed * Time.deltaTime);
            IsMoving = move.magnitude >= IS_MOVING_MIN_MAG;
        }

        private void Jump()
        {
            AudioManager.Instance.Play("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY * gravityScale);
            SoundManager.Instance.SoundPlay(2);
        }

        private void LookAtCamera()
        {
            const float SMOOTH_TIME = 5f;
            var camAngles = Packtool.Vector3X.IgnoreXZ(cam.eulerAngles);
            var targetRot = Quaternion.Euler(camAngles);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, SMOOTH_TIME * Time.deltaTime);
        }

        private void Attack()
        {
            animator.SetTrigger("Attack");
            OnAttack?.Invoke();
            
            if(currentController == 2)
            {
                healthManager.currentAmmo--;
                SoundManager.Instance.SoundPlay(0);
            }

            else if(currentController == 1)
            {
                SoundManager.Instance.SoundPlay(10);
            }

        }

        private void AttackGranade()
        {
            healthManager.grenadeCount--;
            GameObject grenade = Instantiate(grenadeObject, healthManager.ItemHolderTransform[1].transform.position, Quaternion.identity);
            grenade.GetComponent<Rigidbody>().velocity = launchSpeed * healthManager.ItemHolderTransform[1].up;
            grenade.GetComponent<Rigidbody>().AddForce(Vector3.forward * launchSpeed * linePoints);
            SoundManager.Instance.SoundPlay(0);
        }
        private void PutTheGear()
        {
            healthManager.startPortal.SetActive(true);

            SoundManager.Instance.SoundPlay(12);
            GameObject newParticle = Instantiate(ParticleManager.Instance.bulletParticle[9], healthManager.gearYeri.transform.position, Quaternion.identity);
            newParticle.transform.parent = gameObject.transform;
            Destroy(newParticle, 0.60f);

            //gear koyma ve ses gelecek
            Transform[] children = healthManager.ItemHolderTransform[0].GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                child.transform.parent = null;
                child.transform.SetParent(healthManager.gearYeri.transform, true);

                child.transform.DOJump(healthManager.gearYeri.transform.position + new Vector3(0, 0.025f , 0), 0.5f, 1, 0.25f).OnComplete(
             () =>
             {

                 child.transform.localPosition = new Vector3(0,0,0);
                 child.transform.localRotation = Quaternion.identity;
                 child.transform.SetParent(healthManager.gearYeri.transform, true);
                 child.transform.localScale = new Vector3(0, 0, 0);
             }
            );
            }
            Destroy(healthManager.portalaGit);
            
        }

        private void ToggleCursorLockState()
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
        }

        #region Controller

        private void UseNextController()
        {
            currentController = (currentController + 1) % controllers.Length;
            OnCurrentControllerChange?.Invoke(currentController);
        }

        private void UsePreviousController()
        {
            if (currentController > 0)
                currentController--;
            else currentController = controllers.Length - 1;

            OnCurrentControllerChange?.Invoke(currentController);
        }

        private void UpdateCurrentController(int currentController)
        {
            animator.runtimeAnimatorController = controllers[currentController];
        }

        #endregion

        float GetAnimMoveVal(float move, float animCurVal)
        {
            const float SMOOTH_TIME = 10f;
            const float WALK_VAL = 1f;
            const float RUN_VAL = 2f;
            var newVal = move * (IsRunning ? RUN_VAL : WALK_VAL);
            var res = Mathf.Lerp(animCurVal, newVal, SMOOTH_TIME * Time.deltaTime);
            return newVal;
        }
    }
}
