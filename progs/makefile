
# --------------------------------------------------------------------
# config
# --------------------------------------------------------------------

OBJDIR   = obj

MK       = mkdir

CPP      = g++
CPPFLAGS = -c -Wall -ggdb
INCLUDES = -I.

SOURCES  = main.cc \
           logger.cc \
           inputstring.cc
APPL     = $(addprefix $(OBJDIR)/, $(notdir $(SOURCES:.cc=.o)))

BIN      = vigenere

# --------------------------------------------------------------------
# rules
# --------------------------------------------------------------------

all: $(BIN)

clean: create
	@(echo Cleaning $(BIN) ...)

create:
	@(echo Creating $(OBJDIR) ...)
	@($(MK) $(OBJDIR))

$(BIN): $(APPL)
	@(echo Linking $(BIN) ...)
	$(CPP) $(APPL) -o $(BIN)

define compile
$(OBJDIR)/$(notdir $(1:.cc=.o)) : $1
	@echo Compile "$$(*F)" ...
	$(CPP) $(CPPFLAGS) $(INCLUDES) $$< -MD -o $$@
endef

$(foreach src, $(SOURCES), $(eval $(call compile, $(src))))

-include $(APPL:.o=.d)

