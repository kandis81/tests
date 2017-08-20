
all: build

include ../makeinit

# *************************************************
# tests

testfile: build
	mkdir -p ./logs
	cat tester.txt |  dotnet run --log-type file --log-level 3 --log-path ./logs

testconsole: build
	cat tester.txt |  dotnet run --log-level 3

# *************************************************

