
# --------------------------------------------------------------------
# config
# --------------------------------------------------------------------

OBJDIR   = obj

MK       = mkdir -p
RM       = rm -rf

CPP      = g++
CPPFLAGS = -c -Wall -ggdb
LDFLAGS  = -ggdb
INCLUDES = -I.

SOURCES  = main.cc        \
           logger.cc      \
           inputstring.cc \
           file.cc        \
           vigenereapp.cc
APPL     = $(addprefix $(OBJDIR)/, $(notdir $(SOURCES:.cc=.o)))

BIN      = vigenere

# --------------------------------------------------------------------
# rules
# --------------------------------------------------------------------

all: $(BIN)

clean: create
	@(echo Cleaning $(BIN) ...)
	@($(RM) $(OBJDIR)/*.*)

create: $(OBJDIR)

$(OBJDIR):
	@(echo Creating $(OBJDIR) ...)
	@($(MK) $(OBJDIR))

$(BIN): $(APPL)
	@(echo Linking $(BIN) ...)
	$(CPP) $(LDFLAGS) $(APPL) -o $(BIN)

define compile
$(OBJDIR)/$(notdir $(1:.cc=.o)) : $1
	@echo Compile "$$(*F)" ...
	$(CPP) $(CPPFLAGS) $(INCLUDES) $$< -MD -o $$@
endef

$(foreach src, $(SOURCES), $(eval $(call compile, $(src))))

-include $(APPL:.o=.d)

