using ArcadeGame.Data;
using Shared.Editor;
using Shared.Helpers;
using UnityEngine;

namespace ArcadeGame.Controllers
{
    /// <summary>
    ///     Controls the behaviour of the plushies for the claw machine.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Plushie : Shared.Behaviour
    {
        #region VARIABLE DECLARATIONS

        [SerializeField] private ClawMachinePrizes plushieType;
        
        private Rigidbody body;
        private Collider collider;
        private bool insideClaw;
        private bool spawned;

        #endregion

        #region SETUP

        private async void Awake()
        {
            body = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            body.useGravity = true;

            //wait for the plushie to spawn and fall
            await Timer.WaitForSeconds(2f);
            if (this == null) return;
            spawned = true;
        }

        #endregion

        #region METHODS

        private void Update()
        {
            if (insideClaw)
                transform.localPosition = Vector3.zero;
        }


        private void OnTriggerEnter(Collider collision)
        {
            if (!spawned) return;

            if (collision.CompareTag(Constants.TAG_CLAW_MACHINE_BIN))
            {
                GameEventBroadcaster.BroadcastClawMachinePrize(plushieType, name);
                
                //award prize and destroy
                Destroy();
            }
            else if (collision.CompareTag(Constants.TAG_CLAW_MACHINE_CLAW))
            {
                if (collision.transform.childCount == 0)
                {
                    //parent the plushie to the transform of the claw only if another
                    //plushie has not been mounted to the claw already

                    body.useGravity = false;
                    collider.enabled = false;
                    transform.SetParent(collision.transform);
                    insideClaw = true;
                }
            }
        }
        
        /// <summary>
        ///     Releases the plushie from the control of the claw.
        /// </summary>
        public void DropPlushie()
        {
            Log("Dropping" + name);
            insideClaw = false;
            //transform.SetParent(null);
            collider.enabled = true;
            body.useGravity = true;
        }

        #endregion

        [Debugging]
        [SerializeField, InspectorButton("DropPlushie")] bool m_DropPlushie;
    }
}