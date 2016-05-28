using System;
using Xamarin.Forms;

namespace Test
{
    class View: ContentPage
    {
        Label label = new Label();

        public View() {
            this.Content = label;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            label.Text = "hehe";
        }
    }
}
