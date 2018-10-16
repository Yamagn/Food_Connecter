using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Food_Connecter
{
    public class KeyboardResizingPageRenderer : PageRenderer
    {
        NSObject _onKeyBoardShow;
        NSObject _onKeyBoardHide;
        nfloat _barHeight;
        double _initialHeight;
        bool _isInitial;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _isInitial = true;

            if (NavigationController != null && NavigationController.TabBarController != null && NavigationController.TabBarController.TabBar != null)
            {
                _barHeight = NavigationController.TabBarController.TabBar.Frame.Height;
            }

            _onKeyBoardShow = UIKeyboard.Notifications.ObserveWillShow(OnKeyBoardShow);
            _onKeyBoardHide = UIKeyboard.Notifications.ObserveWillHide(OnKeyBoardHide);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (_onKeyBoardShow != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_onKeyBoardShow);
            }

            if (_onKeyBoardHide != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(_onKeyBoardHide);
            }
        }

        void OnKeyBoardShow(object sender, UIKeyboardEventArgs args)
        {
            var page = Element as ContentPage;

            if (_isInitial)
            {
                _initialHeight = page.Bounds.Height;
                _isInitial = false;
            }

            if (page != null)
            {
                var keyboardHeight = args.FrameEnd.Height - _barHeight;

                var pageFrame = Element.Bounds;

                // Padding変更じゃなしにページをLayoutTo
                Element.LayoutTo(new Rectangle(pageFrame.X, pageFrame.Y,
                                               pageFrame.Width, _initialHeight - keyboardHeight));
            }
        }

        void OnKeyBoardHide(object sender, UIKeyboardEventArgs args)
        {

            var page = Element as ContentPage;
            if (page != null)
            {
                var pageFrame = Element.Bounds;
                Element.LayoutTo(new Rectangle(pageFrame.X, pageFrame.Y,
                                               pageFrame.Width, _initialHeight));
            }
        }
    }
}

