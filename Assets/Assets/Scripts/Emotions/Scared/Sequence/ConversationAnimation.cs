using UnityEngine;
using System.Collections;
using ScaredScene;

namespace ScaredScene
{
    public class ConversationAnimation : MonoBehaviour
    {
        public Animator anim;
        public Animator other;

        public void StartListening()
        {
            anim.SetTrigger("Listening");
            other.SetTrigger("Talking");
        }
    }
}