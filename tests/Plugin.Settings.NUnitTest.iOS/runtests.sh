#!/bin/sh

echo 'Output path = ' $1

TEST_RESULT=$1/test_results.xml
TOUCH_SERVER=unit-test-tools/Touch.Server.exe


echo 'Delete test result'
rm -rf $TEST_RESULT

#downloading touch.server
if [ ! -f $TOUCH_SERVER ]; then 
	echo "Touch server doesn't exists"
	curl -L -O "https://github.com/prashantvc/Touch.Server/releases/download/0.1/Touch.Server.exe" > Touch.Server.exe
	mkdir unit-test-tools
	mv Touch.Server.exe $TOUCH_SERVER
fi;

mono --debug $TOUCH_SERVER \
	--launchsim ./bin/iPhoneSimulator/Debug/RefractoredXamSettingsNUnitTestiOSUnified.app \
	-autoexit \
	-skipheader \
	-logfile=$TEST_RESULT \
	--verbose \
	--device=":v2:runtime=com.apple.CoreSimulator.SimRuntime.iOS-11-2,devicetype=com.apple.CoreSimulator.SimDeviceType.iPhone-SE"