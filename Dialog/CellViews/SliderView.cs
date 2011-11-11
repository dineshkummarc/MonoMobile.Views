// 
//  SliderView.cs
// 
//  Author:
//    Robert Kozak (rkozak@gmail.com / Twitter:@robertkozak)
// 
//  Copyright 2011, Nowcom Corporation.
// 
//  Code licensed under the MIT X11 license
// 
//  Permission is hereby granted, free of charge, to any person obtaining
//  a copy of this software and associated documentation files (the
//  "Software"), to deal in the Software without restriction, including
//  without limitation the rights to use, copy, modify, merge, publish,
//  distribute, sublicense, and/or sell copies of the Software, and to
//  permit persons to whom the Software is furnished to do so, subject to
//  the following conditions:
// 
//  The above copyright notice and this permission notice shall be
//  included in all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//  NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//  LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//  OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//  WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
namespace MonoMobile.Views
{
	using System.Drawing;
	using MonoMobile.Views;
	using MonoTouch.UIKit;
	using MonoTouch.Foundation;
	
	[Preserve(AllMembers = true)]
	public class SliderView : CellView, ICellContent
	{
		private UILabel _TextLabel;
 
		public UISlider Slider { get; set; }
		
		public UITableViewCell Cell { get; set; }

		public override UITableViewCellStyle CellStyle { get { return UITableViewCellStyle.Default; } }

		public SliderView(RectangleF frame) : base(frame) 
		{
			Slider = new UISlider(new RectangleF(100, 0, frame.Width - 110, frame.Height)) 
			{ 
				AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth,
				BackgroundColor = UIColor.Clear, 
				Continuous = true,
				
				Tag = 1 
			};


			Slider.ValueChanged += delegate 
			{
				DataContext.Value = Slider.Value;
			};

			Add(Slider);
		}

		public override void UpdateCell(UITableViewCell cell, MonoTouch.Foundation.NSIndexPath indexPath)
		{	
			cell.TextLabel.Text = string.Empty;

			var rangeAttribute = DataContext.Member.GetCustomAttribute<RangeAttribute>();
			if (rangeAttribute != null)
			{
				Slider.MaxValue = rangeAttribute.High;
				Slider.MinValue = rangeAttribute.Low;
				ShowCaption = rangeAttribute.ShowCaption;
			}
			
			if (ShowCaption)
				cell.TextLabel.Text = Caption;

			Slider.Value = (float)DataContext.Value;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && Slider != null)
			{
				Slider.Dispose();
				Slider = null;
			}
			
			base.Dispose(disposing);
		}
	}
}

