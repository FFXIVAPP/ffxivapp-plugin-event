﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellView.xaml.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ShellView.xaml.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event {
    using System.Windows;

    /// <summary>
    ///     Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView {
        public static ShellView View;

        public ShellView() {
            this.InitializeComponent();
            View = this;
        }

        public bool IsRendered { get; set; }

        private void ShellView_OnLoaded(object sender, RoutedEventArgs e) {
            if (this.IsRendered) {
                return;
            }

            this.IsRendered = true;
        }
    }
}