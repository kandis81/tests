
# *************************************************
# all : compile & run

all:
	$(MAKE) update
	$(MAKE) build

# *************************************************

# *************************************************
# run source

run:
	dotnet run

# *************************************************

# *************************************************
# create basic console project
# basicly it is removed, due to not used anymore
# in this directory

#create:
#	dotnet new -l C# -t Console

# *************************************************

# *************************************************
# Restore : download missing dependencies
# Use it when you added any new usages (Libs) to it

update:
	dotnet restore

# *************************************************

# *************************************************
# Compile source

build:
	dotnet build

# *************************************************

