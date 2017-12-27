#!/bin/sh

echo 'Output path = ' $1

TEST_RESULT=$1/test_results.xml

echo 'Delete test result'
rm -rf $TEST_RESULT

mono --debug unit-test-tools/Touch.Server.exe \
	--launchsim ./bin/iPhoneSimulator/Debug/RefractoredXamSettingsNUnitTestiOSUnified.app \
	-autoexit \
	-skipheader \
	-logfile=$TEST_RESULT \
	--verbose \
	--device=":v2:runtime=com.apple.CoreSimulator.SimRuntime.iOS-11-2,devicetype=com.apple.CoreSimulator.SimDeviceType.iPhone-SE"