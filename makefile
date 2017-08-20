
all:
	@(echo Makeing library ...)
	@(cd lib ; $(MAKE) $@; cd -)
	@(echo Makeing unit tester ...)
	@(cd tests ; $(MAKE) $@; cd -)
	@(echo Makeing old fashion way tester ...)
	@(cd Program ; $(MAKE) $@; cd -)

clean:
	@(echo Cleaning library ...)
	@(cd lib ; $(MAKE) $@; cd -)
	@(echo Cleaning unit tester ...)
	@(cd tests ; $(MAKE) $@; cd -)
	@(echo Cleaning old fashion way tester ...)
	@(cd Program ; $(MAKE) $@; cd -)
