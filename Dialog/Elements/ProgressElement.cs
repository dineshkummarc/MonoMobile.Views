//
// ProgressElement.cs
//
// Author:
//   Robert Kozak (rkozak@gmail.com / Twitter:@robertkozak
//
// Copyright 2011, Nowcom Corporation
//
// Code licensed under the MIT X11 license
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
namespace MonoMobile.Views
{
	using System;
	using System.Drawing;
	using System.Linq;
	using System.Threading;
	using MonoTouch.Foundation;
	using MonoTouch.ObjCRuntime;
	using MonoTouch.UIKit;
	using MonoMobile.Views;

	[Preserve(AllMembers = true)]
	public class ProgressElement : Element, ISelectable, ITappable
	{
		private const int pad = 10;
		private const int indicatorSize = 20;

		public string Title { get; set; }
		public string DetailsText { get; set; }
		public ICommand Command { get; set; }
		public object CommandParameter { get; set; }
		public bool ShowHud { get; set;}

		private ProgressHud _ProgressHud;

		public ProgressElement(string caption, string title, string detailsText, ICommand command) : base(caption)
		{
			Title = title;
			DetailsText = detailsText;
			Command = command;
			
			Theme.TextFont = UIFont.BoldSystemFontOfSize(17f);
			Theme.TextAlignment = UITextAlignment.Center;

			ShowHud = true;
		}

		public override void InitializeCell(UITableView tableView)
		{
			base.InitializeCell(tableView);
			Theme.TextAlignment = UITextAlignment.Center;
		}

		public void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{			
			if (ShowHud)
			{
				_ProgressHud = new ProgressHud() { TitleText = Title, DetailText = DetailsText, Mode = HudProgressMode.Indeterminate };
				
				if (Command != null && Command.CanExecute(CommandParameter))
				{	
					_ProgressHud.ShowWhileExecuting(()=>Command.Execute(CommandParameter), true);
				}
			}
			else
			{
				ShowIndicator(()=>Command.Execute(CommandParameter));
			}
		}

		private void ShowIndicator(Action execute)
		{		
			var accessoryView = Cell.AccessoryView;
			var accessory = Cell.Accessory;
			var indicatorView = new UIActivityIndicatorView()
			{
				ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray,
				Frame = new RectangleF(indicatorSize * 2, pad, indicatorSize, indicatorSize)
			};
			
			indicatorView.StopAnimating();
			var caption = Caption;
			Caption = Title;

			var thread = new System.Threading.Thread(() =>
			{
				using (NSAutoreleasePool pool = new NSAutoreleasePool())
				{
					try
					{	
						Cell.Accessory = UITableViewCellAccessory.None;
						Cell.AccessoryView = indicatorView;
						indicatorView.StartAnimating();

						execute();
					}
					finally
					{
						Caption = caption;
						indicatorView.StopAnimating();
						indicatorView.Dispose();

						Cell.Accessory = accessory;
						Cell.AccessoryView = accessoryView;
					}
				}
			});
				
			thread.Start();
		}
	}
}

