using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(Food_Connecter.ContentPageRenderer))]

namespace Food_Connecter
{
    public class ContentPageRenderer : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var toolbarItems = ((ContentPage)Element).ToolbarItems.OrderBy(w => w.Priority);
            var navigationItem = NavigationController.TopViewController.NavigationItem;
            var rightItems = new List<UIBarButtonItem>();
            var leftItems = new List<UIBarButtonItem>();

            foreach (var item in toolbarItems)
            {
                // Priority がマイナスだったらナビゲーションバーの左側に配置する
                if (item.Priority < 0)
                {
                    leftItems.Add(item.ToUIBarButtonItem());
                }
                else
                {
                    rightItems.Add(item.ToUIBarButtonItem());
                }
            }

            navigationItem.SetRightBarButtonItems(rightItems.ToArray(), animated);
            navigationItem.SetLeftBarButtonItems(leftItems.ToArray(), animated);

            var items = this.NavigationController.TopViewController.NavigationItem;

            foreach (var item in items.LeftBarButtonItems)
            {
                if (item.Image == null) continue;

                item.Image = item.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            }
        }
    }
}