using Shared;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace HorrorHouse.Controllers
{
    /// <summary>
    ///     Manages the game.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameObject[] destroyOnCurseLifted;
        [SerializeField] private GameObject[] enableOnCurseLifted;
        [SerializeField] private GameObject player;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Volume transitionPost;

        #region METHODS

        #region ENGINE

        protected override void Awake()
        {
            base.Awake();
            RenderSettings.fog = true; //enable fog
        }

        private void OnEnable()
        {
            GameEventBroadcaster.OnGameOver += CurseLifted;
        }

        private void OnDisable()
        {
            GameEventBroadcaster.OnGameOver -= CurseLifted;
        }

        #endregion

        //Destroys items on curse lifted
        private void CurseLifted()
        {
            foreach(var item in destroyOnCurseLifted)
                if(item)
                    Destroy(item);
            
            foreach (var toEnable in enableOnCurseLifted)
                if(toEnable)
                    toEnable.SetActive(true);

            RenderSettings.fog = false; //disable fog

            StartCoroutine(Transition());
        }

        private IEnumerator Transition()
        {
            yield return new WaitForSeconds(3); //wait for 3 seconds before transition

            if(transitionPost)
            {
                LeanTween.value(0f, 1f, 1f)
                    .setOnUpdate(v => transitionPost.weight = v)
                    .setEase(LeanTweenType.linear);
            }

            yield return new WaitForSeconds(2); //wait for transition

            if (player)
            {
                player.transform.position = spawnPoint.position;
                player.transform.localEulerAngles = 180 * Vector3.up;
            }
            
            if(transitionPost)
            {
                LeanTween.value(1f, 0f, 1f)
                    .setOnUpdate(v => transitionPost.weight = v)
                    .setEase(LeanTweenType.linear);
            }
        }

        #endregion
    }
}