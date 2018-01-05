TOUCH_SERVER=unit-test-tools/Touch.Server.exe
TEST_RESULT=./Test_Results.xml
MTOUCH=/Developer/MonoTouch/usr/bin/mtouch
THIS_FILE := $(lastword $(MAKEFILE_LIST))

all: build-simulator build-device

run-all-tests: run-simulator run-device

build-simulator:
	msbuild Plugin.Settings.NUnitTest.iOS.csproj /t:Rebuild

build-device:
	msbuild Plugin.Settings.NUnitTest.iOS.csproj /t:ReBuild /p:Platform=iPhone

run-simulator-tests: build-simulator
	mono --debug $(TOUCH_SERVER) \
	--launchsim ./bin/iPhoneSimulator/Debug/RefractoredXamSettingsNUnitTestiOSUnified.app \
	-autoexit -skipheader \
	-logfile=$(TEST_RESULT) \
	--verbose \
	--device=":v2:runtime=com.apple.CoreSimulator.SimRuntime.iOS-11-2,devicetype=com.apple.CoreSimulator.SimDeviceType.iPhone-SE"
	@$(MAKE) -f $(THIS_FILE) show-results

run-device-tests: build-device
	$(MTOUCH) --installdev ./bin/iPhone/Debug/RefractoredXamSettingsNUnitTestiOSUnified.app
	mono --debug $(TOUCH_SERVER) --launchdev com.plugin.settingstest -autoexit -skipheader -logfile=$(TEST_RESULT)
	@$(MAKE) -f $(THIS_FILE) show-results

show-results:
	mono ../../tools/nunit-summary/nunit-summary.exe $(TEST_RESULT)
	rm -rf $(TEST_RESULT)