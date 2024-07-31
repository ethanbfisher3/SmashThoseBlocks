using UnityEngine;
using System;
using System.Collections;

namespace Utils
{
    public class Transitions : MonoBehaviour
    {
        public Animation anim;

        static Transitions instance;

        void Awake() => instance = this;

        public static float[] PlayTransition(CutSceneTransition cst, Func<float> OnHalfway = null)
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

            instance.StartCoroutine(instance.PlayTransitionOverTime(startName, endName, OnHalfway));
            return new float[] 
            { 
                instance.anim.GetClip(startName).length, 
                instance.anim.GetClip(endName).length 
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