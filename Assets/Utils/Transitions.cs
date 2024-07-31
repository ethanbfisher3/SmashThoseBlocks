using UnityEngine;
using System;
using System.Collections;

namespace Utils
{
    public class CutSceneManager : MonoBehaviour
    {
        public Animation anim;

        public static CutSceneManager Instance { get; private set; }

        void Awake() => Instance = this;

        public float[] PlayTransition(CutSceneTransition cst, Func<float> OnHalfway = null)
        {
            string startName, endName;

            switch (cst)
            {
                case CutSceneTransition.Fade:
                    startName = "FadeOut";
                    endName = "FadeIn";
                    break;
                default:
                    startName = endName = "";
                    break;
            }

            StartCoroutine(PlayTransitionOverTime(startName, endName, OnHalfway));
            return new float[] 
            { 
                anim.GetClip(startName).length, 
                anim.GetClip(endName).length 
            };
        }

        IEnumerator PlayTransitionOverTime(string startName, string endName, Func<float> OnHalfway = null)
        {
            anim.Play(startName);
            yield return new WaitForSeconds(anim.GetClip(startName).length);

            if (OnHalfway != null)
                yield return new WaitForSeconds(OnHalfway());

            anim.Play(endName);
            yield return new WaitForSeconds(anim.GetClip(endName).length);
        }
    }
}