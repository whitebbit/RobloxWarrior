using System;
using UnityEngine;
using UnityEngine.UI;
#if TMP_YG2
using TMPro;
#endif

namespace YG
{
    public class LBPlayerDataYG : MonoBehaviour
    {
        public ImageLoadYG imageLoad;

        [Serializable]
        public struct TextLegasy
        {
            public Text rank, name, score;
        }

        public TextLegasy textLegasy;

#if TMP_YG2
        [Serializable]
        public struct TextMP
        {
            public TextMeshProUGUI rank, name, score;
        }

        public TextMP textMP;
#endif
        [Space(10)] public MonoBehaviour[] topPlayerActivityComponents = new MonoBehaviour[0];
        public MonoBehaviour[] currentPlayerActivityComponents = new MonoBehaviour[0];

        public Image[] topPlayerImageComponents = new Image[3];

        public class Data
        {
            public string rank;
            public string name;
            public string score;
            public string photoUrl;
            public bool inTop;
            public bool currentPlayer;
            public Sprite photoSprite;
        }

        [HideInInspector] public Data data = new Data();

        public void UpdateEntries()
        {
            if (textLegasy.rank && data.rank != null) textLegasy.rank.text = data.rank.ToString();
            if (textLegasy.name && data.name != null) textLegasy.name.text = data.name;
            if (textLegasy.score && data.score != null) textLegasy.score.text = data.score.ToString();

#if TMP_YG2
            if (textMP.rank && data.rank != null) textMP.rank.text = data.rank.ToString();
            if (textMP.name && data.name != null) textMP.name.text = data.name;
            if (textMP.score && data.score != null) textMP.score.text = data.score.ToString();
#endif

            if (imageLoad)
            {
                if (data.photoSprite)
                {
                    imageLoad.SetTexture(data.photoSprite.texture);
                }
                else if (data.photoUrl == null)
                {
                    imageLoad.ClearTexture();
                }
                else
                {
                    imageLoad.Load(data.photoUrl);
                }
            }


            if (topPlayerImageComponents.Length > 0)
            {
                if (data.inTop)
                {
                    ActivityImageObject(data.rank);
                    if (textLegasy.rank && data.rank != null) textLegasy.rank.gameObject.SetActive(false);
#if TMP_YG2
                    if (textMP.rank && data.rank != null) textMP.rank.gameObject.SetActive(false);
#endif
                }
                else
                {
                    ActivityImageObject("-1");
                }
            }


            if (topPlayerActivityComponents.Length > 0)
            {
                if (data.inTop)
                {
                    ActivityMomoObjects(topPlayerActivityComponents, true);
                }
                else
                {
                    ActivityMomoObjects(topPlayerActivityComponents, false);
                }
            }

            if (currentPlayerActivityComponents.Length > 0)
            {
                if (data.currentPlayer)
                {
                    foreach (var activity in currentPlayerActivityComponents)
                    {
                        activity.gameObject.SetActive(true);
                    }
                }
                else
                {
                    foreach (var activity in currentPlayerActivityComponents)
                    {
                        activity.gameObject.SetActive(false);
                    }
                }
            }

            void ActivityMomoObjects(MonoBehaviour[] objects, bool activity)
            {
                for (int i = 0; i < objects.Length; i++)
                {
                    objects[i].enabled = activity;
                }
            }

            void ActivityImageObject(string rank)
            {
                if (rank == "-1")
                {
                    foreach (var item in topPlayerImageComponents)
                    {
                        item.gameObject.SetActive(false);
                    }
                }


                var r = int.Parse(rank);
                for (int i = 0; i < topPlayerImageComponents.Length; i++)
                {
                    topPlayerImageComponents[i].gameObject.SetActive(r - 1 == i);
                }
            }
        }
    }
}