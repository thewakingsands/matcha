// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Utils
{
    using System.ComponentModel;

    public class BindingTarget : INotifyPropertyChanged
    {
        protected void EmitPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class StaticBindingTarget<T> : BindingTarget where T : new()
    {
        public static T Instance { get; } = new T();
    }
}
