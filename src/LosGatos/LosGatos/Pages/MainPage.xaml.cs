﻿using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace LosGatos.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage() => InitializeComponent();

        void ImageButton_Clicked(System.Object sender, System.EventArgs e)
        {
            App.Current.UserAppTheme = (App.Current.UserAppTheme == OSAppTheme.Dark)
                ? OSAppTheme.Light
                : OSAppTheme.Dark;
        }

        private async void TabViewItem_OnTabTapped(object sender, TabTappedEventArgs e)
        {
            await Navigation.PushAsync(new ShoppingCartPage(), true);
        }
    }

    public class TabViewItemAnimation : ITabViewItemAnimation
    {
        protected uint SelectedAnimationLength { get; } = 500;
        protected uint DeSelectedAnimationLength { get; } = 1000;

        public Task OnDeSelected(View tabViewItem)
        {
            var tcs = new TaskCompletionSource<bool>();

            if (!(tabViewItem.FindByName("TabIcon") is Image icon))
                return Task.FromResult(false);

            var deSelectedAnimation = new Animation();

            deSelectedAnimation.WithConcurrent((f) => icon.Opacity = f, 1, 0.75, null, 0, 0.01);
            deSelectedAnimation.WithConcurrent((f) => icon.Scale = f, 1.2, 1, null, 0, 0.5);

            deSelectedAnimation.Commit(tabViewItem, nameof(OnDeSelected), length: DeSelectedAnimationLength,
                finished: (v, t) => tcs.SetResult(true));

            return tcs.Task;
        }

        public Task OnSelected(View tabViewItem)
        {
            var tcs = new TaskCompletionSource<bool>();

            var icon = tabViewItem.FindByName<Image>("TabIcon");

            var selectedAnimation = new Animation();

            selectedAnimation.WithConcurrent((f) => icon.RotationY = f, 75, 0, Easing.CubicOut);
            selectedAnimation.WithConcurrent((f) => icon.Opacity = f, 0.75, 1, null, 0, 0.01);
            selectedAnimation.WithConcurrent((f) => icon.Scale = f, 1, 1.2, null, 0, 0.5);

            selectedAnimation.Commit(icon, nameof(OnSelected), length: SelectedAnimationLength,
               finished: (v, t) => tcs.SetResult(true));

            return tcs.Task;
        }
    }
}