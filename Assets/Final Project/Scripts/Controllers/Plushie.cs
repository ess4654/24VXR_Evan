using ArcadeGame.Data;
using ArcadeGame.Helpers.Audio;
using Shared.Editor;
using Shared.Helpers;
using UnityEngine;

namespace ArcadeGame.Controllers
{
    /// <summary>
    ///     Controls the behaviour of the plushies for the claw machine.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Plushie : RandomAudioClip
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

        private async void OnTriggerEnter(Collider collision)
        {
            if (!spawned) return;

            if (collision.CompareTag(Constants.TAG_CLAW_MACHINE_BIN))
            {
                PlayRandomAudio(); //play plushie hit sound
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

                    collision.transform.parent.GetComponent<Collider>().enabled = false;

                    body.useGravity = false;
                    collider.enabled = false;
                    transform.SetParent(collision.transform);

                    await Timer.WaitForSeconds(.25f);
                    if (this == null) return;

                    insideClaw = true;
                }
            }
        }
        
        /// <summary>
        ///     Releases the plushie from the control of the claw.
        /// </summary>
        public void DropPlushie()
        {
            //Log("Dropping" + name);
            insideClaw = false;
            //transform.SetParent(null, true);
            collider.enabled = true;
            body.useGravity = true;
        }

        #endregion

        [Debugging]
        [SerializeField, InspectorButton("DropPlushie")] bool m_DropPlushie;
    }
}