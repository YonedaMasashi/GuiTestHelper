﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GuiTestHelper.FW
{
    public abstract class ViewModelBase : INotifyPropertyChanged 
    {
        protected ViewModelBase() { }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise PropertyChanged Event.
        /// </summary>
        /// <param name="names"></param>
        protected virtual void OnPropertyChanged(params string[] names)
        {
            var h = PropertyChanged;
            if (h == null) return;

            CheckPropertyName(names);

            foreach (var name in names)
            {
                h(this, new PropertyChangedEventArgs(name));
            }
        }

        [Conditional("DEBUG")]
        private void CheckPropertyName(params string[] names)
        {
            var props = GetType().GetProperties();
            foreach (var name in names)
            {
                var prop = props.Where(p => p.Name == name).SingleOrDefault();
                if (prop == null) throw new ArgumentException(name);
            }
        }

        protected void OnPropertyChanged<T>(params Expression<Func<T>>[] propertyExpression)
        {
            OnPropertyChanged(
                propertyExpression.Select(ex => ((MemberExpression)ex.Body).Member.Name).ToArray());
        }

        public virtual void Cleanup()
        {
            Message.Messenger.Default.UnRegister(this);
        }
    }
}
