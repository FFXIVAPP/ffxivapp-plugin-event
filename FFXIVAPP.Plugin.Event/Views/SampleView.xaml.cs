// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleView.xaml.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   SampleView.xaml.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event.Views {
    /// <summary>
    ///     Interaction logic for SampleView.xaml
    /// </summary>
    public partial class SampleView {
        public static SampleView View;

        public SampleView() {
            this.InitializeComponent();
            View = this;
        }
    }
}