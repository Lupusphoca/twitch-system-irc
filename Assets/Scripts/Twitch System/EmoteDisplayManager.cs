namespace PierreARNAUDET.TwitchUtilitary
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.Networking;

    using PierreARNAUDET.Core.Attributes;
    using static PierreARNAUDET.TwitchUtilitary.TwitchGlobalEmotesData.GlobalEmotes;

    public class EmoteDisplayManager : MonoBehaviour
    {
        [Data]
        [SerializeField] GameObject emotePrefab;

        [Settings]
        [SerializeField] Vector3 desiredSize = new Vector3(50, 50, 50);
        [SerializeField] float growDuration = 0.5f;
        [SerializeField] float shrinkDuration = 0.5f;
        [SerializeField] int impulsePowerX;
        [SerializeField] int impulsePowerY;

        private Vector3 originalScale;
        private Vector3 targetScale;
        private float timer;

        public void Process(GlobalEmote globalEmote)
        {
            StartCoroutine(LoadSprite(globalEmote.Images.Url4x));
        }

        private IEnumerator LoadSprite(string url)
        {
            var uwr = new UnityWebRequest();
            uwr = UnityWebRequestTexture.GetTexture(url);
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                var texture = ((DownloadHandlerTexture)uwr.downloadHandler).texture;
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                var emote = Instantiate(emotePrefab, this.transform);
                ControlSize(emote);
                ChangeSprite(sprite, emote);
                PropulseObject(emote);
            }
        }

        private void ChangeSprite(Sprite sprite, GameObject emote)
        {
            emote.GetComponent<SpriteRenderer>().sprite = sprite;
        }

        private void PropulseObject(GameObject emote)
        {
            emote.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-impulsePowerX, impulsePowerX), impulsePowerY), ForceMode2D.Impulse);
        }

        #region Size control over lifetime
        private void ControlSize(GameObject emote)
        {
            emote.transform.localScale = Vector3.zero;
            StartCoroutine(GrowAndShrinkCoroutine(emote));
        }

        private IEnumerator GrowAndShrinkCoroutine(GameObject emote)
        {
            var startTime = Time.time;
            var originalScale = emote.transform.localScale;

            while (Time.time - startTime < growDuration)
            {
                var t = (Time.time - startTime) / growDuration;
                emote.transform.localScale = Vector3.Lerp(originalScale, desiredSize, t);
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            startTime = Time.time;
            originalScale = emote.transform.localScale;

            while (Time.time - startTime < shrinkDuration)
            {
                var t = (Time.time - startTime) / shrinkDuration;
                emote.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
                yield return null;
            }

            Destroy(emote);
        }

        // private IEnumerator GrowAndShrink(GameObject emote)
        // {
        //     while (true)
        //     {
        //         timer += Time.deltaTime;
        //         var t = Mathf.Clamp01(timer / growDuration);
        //         transform.localScale = Vector3.Lerp(originalScale, targetScale, t);

        //         if (t >= 1)
        //         {
        //             timer = 0f;
        //             originalScale = targetScale;
        //             targetScale = Vector3.zero;
        //             growDuration = shrinkDuration;
        //         }

        //         if (transform.localScale == Vector3.zero)
        //         {
        //             break;
        //         }

        //         yield return null;
        //     }

        // var elapsedTime = 0.0f;
        // var timeToReduce = 1.0f;
        // var originalScale = transform.localScale;
        // var targetScale = Vector3.zero;

        // while (elapsedTime < timeToReduce)
        // {
        //     transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / timeToReduce);
        //     elapsedTime += Time.deltaTime;
        //     yield return new WaitForEndOfFrame();
        // }

        // transform.localScale = targetScale;
        // }
        #endregion

        // Emote display manager receive GlobalEmote data at each emote call on chat
        // Then he download the needed sprite
        // Then he instantiate the prefab
        // Then he change his sprite
        // Then he get his rb and launched it in a specified way
        // Then he start an async or enumerator to grow the prefab on a certains size, wait a moment, then shrink it and destroy it.
        // In fact, there is no need to have any script on the prefab
        // We can remove spriteload script and filterglobalemote script
    }
}