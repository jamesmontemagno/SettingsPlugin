using System;
using NUnit.Framework;
using Plugin.Settings.Tests.Portable.Helpers;


namespace Plugin.Settings.NUnitTest
{
    [TestFixture]
    public class TestsFileNameSample
    {

        [SetUp]
        public void Setup()
        {
            TestSettings.FileName = "Test";
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
        public void Clear()
        {
            Int64 test = 10;

            TestSettings.Int64Setting = test;
            Assert.True(TestSettings.Int64Setting == test, "Int64 not saved");

            var contains = TestSettings.AppSettings.Contains("int64_setting", TestSettings.FileName);

            TestSettings.Clear();


            contains = TestSettings.AppSettings.Contains("int64_setting", TestSettings.FileName);

            Assert.IsFalse(contains, "Setting was not removed");
        }

        [Test]
        public void ContainsKey()
        {
            Int64 test = 10;

            Assert.IsFalse(TestSettings.AppSettings.Contains("int64_setting", TestSettings.FileName), "Default value was not false");

            TestSettings.Int64Setting = test;

            Assert.IsTrue(TestSettings.AppSettings.Contains("int64_setting", TestSettings.FileName), "Default value was not false");

        }


        [Test]
        public void Int32()
        {
            Int32 test = 10;

            TestSettings.Int32Setting = test;
            Assert.True(TestSettings.Int32Setting == test, "Int32 not saved");
        }


        [Test]
        public void Int()
        {
            int test = 10;

            TestSettings.IntSetting = test;
            Assert.True(TestSettings.IntSetting == test, "Int not saved");
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
        public void Decimal()
        {
            decimal test = 0.099M;

            TestSettings.DecimalSetting = test;
            Assert.True(TestSettings.DecimalSetting == test, "Decimal not saved");
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

            var contains = TestSettings.AppSettings.Contains("settings_key", TestSettings.FileName);

            Assert.IsTrue(contains, "String wasn't added" + TestSettings.StringSetting);


            TestSettings.Remove("settings_key");

            contains = TestSettings.AppSettings.Contains("settings_key", TestSettings.FileName);

            Assert.IsFalse(contains, "String wasn't removed" + TestSettings.StringSetting);
        }

    }
}