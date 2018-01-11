using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Android.NUnitLite;
using System.Reflection;

namespace Refractored.Xam.Settings.NUnitTest.Android
{
	[Instrumentation(Name = "app.tests.TestInstrumentation")]
	public class TestInstrumentation : TestSuiteInstrumentation
	{
		protected TestInstrumentation(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
		{
		}

		protected override void AddTests()
		{
			AddTest(Assembly.GetExecutingAssembly());
		}
	}
}