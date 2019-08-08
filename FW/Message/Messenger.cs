using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuiTestHelper.FW.Message
{
    /// <summary>
    /// VM から View へのメッセージ送信を行うクラス
    /// </summary>
    public class Messenger
    {
        private static Messenger _instance = new Messenger();

        public static Messenger Default {
            get { return _instance; }
        }

        private List<ActionInfo> list = new List<ActionInfo>();

        public void Register<TMessage>(FrameworkElement recipient, Action<TMessage> action)
        {
            list.Add(new ActionInfo
            {
                Type = typeof(TMessage),
                sender = recipient.DataContext as INotifyPropertyChanged,
                action = action,
            });
        }

        public void Send<TMessage>(INotifyPropertyChanged sender, TMessage message)
        {
            var query = list.Where(o => o.sender == sender && o.Type == message.GetType())
                .Select(o => o.action as Action<TMessage>);
            foreach (var action in query)
            {
                action(message);
            }
        }

        /// <summary>
        /// 登録したメッセージ定義の内、引数として渡したVMの定義を削除する
        /// </summary>
        /// <param name="sender"></param>
        public void UnRegister(INotifyPropertyChanged sender)
        {
            if (sender == null) return;

            var query = list.Where(o => o.sender == sender as INotifyPropertyChanged).ToList();
            foreach(var actionInfo in query)
            {
                list.Remove(actionInfo);
            }
        }

        /// <summary>
        /// メッセージが送信されたことを通知するイベント
        /// </summary>
        public event EventHandler<MessageEventArgs> Raised;

        /// <summary>
        /// 指定したメッセージとコールバックでメッセージを送信する
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="callback">コールバック</param>
        public void Raise(Message message, Action<Message> callback)
        {
            var h = this.Raised;
            if (h != null)
            {
                h(this, new MessageEventArgs(message, callback));
            }
        }


        private class ActionInfo
        {
            public Type Type { get; set; }
            public INotifyPropertyChanged sender { get; set; }
            public Delegate action { get; set; }
        }
    }
}
