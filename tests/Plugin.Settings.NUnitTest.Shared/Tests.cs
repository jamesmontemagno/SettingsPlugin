using System;
using NUnit.Framework;
using Plugin.Settings.Tests.Portable.Helpers;
using System.Globalization;
using System.Threading;

namespace Plugin.Settings.NUnitTest
{
    [TestFixture]
    public class TestsSample
    {

        [SetUp]
        public void Setup()
        {
            TestSettings.FileName = null;
            TestSettings.Clear();
        }

        [TearDown]
        public void Tear() { }

        [Test]
        public void Int64()
        {
            Int64 test = 10;

            TestSettings.Int64Setting = test;
            Assert.True(TestSettings.Int64Setting == test, "Int64 not saved");
        }

        [Test]
        public void When_Clear_Is_Called_Then_All_KeyValue_Pairs_Should_Be_Cleared_From_Settings()
        {
            Int64 test = 10;

            TestSettings.Int64Setting = test;
            Assert.True(TestSettings.Int64Setting == test, "Int64 not saved");

            TestSettings.Clear();

            Assert.IsFalse(TestSettings.AppSettings.Contains("int64_setting"), "Setting was not removed");
        }

        [Test]
        public void When_Value_Is_Saved_A_Preference_Then_Settings_Should_Contain_The_Key_For_Value()
        {
            Int64 test = 10;

            Assert.IsFalse(TestSettings.AppSettings.Contains("int64_setting"), "Key should not exist, but does");
            TestSettings.Int64Setting = test;

            Assert.IsTrue(TestSettings.AppSettings.Contains("int64_setting"), "Key should exist, but doesn't");

        }


        [Test]
        public void Int32()
        {
            Int32 test = 10;

            TestSettings.Int32Setting = test;
            Assert.True(TestSettings.Int32Setting == test, "Int32 not saved");
        }

        [Test]
        public void Bool()
        {
            var test = true;

            TestSettings.BoolSetting = test;
            Assert.True(TestSettings.BoolSetting == test, "Bool not saved");
        }

        [Test]
        public void Double()
        {
            double test = 10.001;

            TestSettings.DoubleSetting = test;
            Assert.True(TestSettings.DoubleSetting == test, "Double not saved");
        }

        [Test]
        [SetCulture("es-ES")]
        [SetUICulture("es-ES")]
        public void Double15Comma()
        {
            double test = 1.5;

            TestSettings.DoubleSetting = test;

            var output = TestSettings.DoubleSetting;
            Assert.True(TestSettings.DoubleSetting == test, "Double not saved");
        }

        [Test]
        public void Double_Zero()
        {
            double test = 0.0D;

            TestSettings.DoubleSetting = test;
            Assert.True(TestSettings.DoubleSetting == test, "Double not saved");
        }

        [Test]
        public void Double_Max()
        {
            double test = double.MaxValue;

            TestSettings.DoubleSetting = test;
            Assert.True(TestSettings.DoubleSetting == test, "Double not saved");
        }

        [Test]
        public void Double_Min()
        {
            double test = double.MinValue;

            TestSettings.DoubleSetting = test;
            Assert.True(TestSettings.DoubleSetting == test, "Double not saved");
        }


        [Test]
        public void Decimal_Max()
        {
            decimal test = decimal.MaxValue;

            TestSettings.DecimalSetting = test;
            Assert.True(TestSettings.DecimalSetting == test, "DecimalSetting not saved");
        }

        [Test]
        public void Decimal_Min()
        {
            decimal test = decimal.MinValue;

            TestSettings.DecimalSetting = test;
            Assert.True(TestSettings.DecimalSetting == test, "DecimalSetting not saved");
        }

		[Test]
        public void Issue_101_Given_A_Five_Decimal_Double_Value_When_Saved_Then_It_Should_Saved_To_Preferences()
		{
            //https://github.com/jamesmontemagno/SettingsPlugin/issues/101

			double i = 0.001;
			var inStuff = i / 100;
			TestSettings.txt_MinRate = inStuff;

			var val = TestSettings.txt_MinRate;
			Assert.True(val == inStuff, "Double not saved");
		}

		[Test]
        public void Decimal()
        {
            decimal test = 0.099M;

            TestSettings.DecimalSetting = test;
            Assert.True(TestSettings.DecimalSetting == test, "Decimal not saved");
        }


        [Test]
        public void Decimal_Max_2()
        {
            decimal test = decimal.MaxValue;

            TestSettings.DecimalSetting = test;
            Assert.True(TestSettings.DecimalSetting == test, "DecimalSetting not saved");
        }

        [Test]
        public void Decimal_Min_2()
        {
            decimal test = decimal.MinValue;

            TestSettings.DecimalSetting = test;
            Assert.True(TestSettings.DecimalSetting == test, "DecimalSetting not saved");
        }

        [Test]
        public void DateTime()
        {

            DateTime test = new DateTime(1986, 6, 25, 4, 0, 0).ToUniversalTime();

            TestSettings.DateTimeSetting = test;
            Assert.True(TestSettings.DateTimeSetting.Ticks == test.Ticks, "DateTime not saved");
        }

        [Test]
        public void Guid()
        {
            Guid test = new Guid("EFFB4B96-92F3-4551-9732-36B11DC8B051");

            TestSettings.GuidSetting = test;
            Assert.True(TestSettings.GuidSetting.ToString() == test.ToString(), "Guid not saved");
        }


        [Test]
        public void AddRemove()
        {
            TestSettings.StringSetting = "Hello World";

            Assert.AreEqual(TestSettings.StringSetting, "Hello World", "Date wasn't set to null, it is: " + TestSettings.StringSetting);

            var contains = TestSettings.AppSettings.Contains("settings_key");

            Assert.IsTrue(contains, "String wasn't added" + TestSettings.StringSetting);


            TestSettings.Remove("settings_key");

            contains = TestSettings.AppSettings.Contains("settings_key");

            Assert.IsFalse(contains, "String wasn't removed" + TestSettings.StringSetting);
        }


    }
}