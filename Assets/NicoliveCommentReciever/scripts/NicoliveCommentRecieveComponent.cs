using UnityEngine;

namespace NicoliveCommentReciever
{
    public class NicoliveCommentRecieveComponent : MonoBehaviour
    {
        public string Host = "127.0.0.1";
        public int Port = 17305;
        public bool ConnectOnAwake;

        /// <summary>
        /// メッセージ受信時に発行されるイベント
        /// </summary>
        public event MessageRecievedHandler MessageRecievedEvent;
        public delegate void MessageRecievedHandler(object sender, CommentRecievedEventArgs e);

        private CommentClient client;

        void Awake()
        {
            client = new CommentClient();
            if (ConnectOnAwake)
            {
                Connect();
            }
        }

        void Update()
        {
            if (!client.IsConnected) return;
            if (!client.IsCommentRecived()) return;

            var comments = client.TakeRecievedComments();
            for (var i = 0; i < comments.Length; i++)
            {
                MessageRecievedEvent(this, new CommentRecievedEventArgs(comments[i]));
            }
        }

        void OnDestroy()
        {
            client.Disconnect();
        }

        public void Connect()
        {
            client.Connect(Host, Port);
        }

        public void Disconnect()
        {
            client.Disconnect();
        }
    }
}
