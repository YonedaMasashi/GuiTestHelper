using System;

namespace GuiTestHelper.FW.Message
{
    public class MessageEventArgs
    {
        /// <summary>
        /// 送信するメッセージ
        /// </summary>
        public Message Message { get; private set; }

        /// <summary>
        /// ViewModelのコールバック
        /// </summary>
        public Action<Message> Callback { get; private set; }

        /// <summary>
        /// メッセージとコールバックを指定してイベント引数を作成する
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callback"></param>
        public MessageEventArgs(Message message, System.Action<Message> callback)
        {
            this.Message = message;
            this.Callback = callback;
        }
    }
}