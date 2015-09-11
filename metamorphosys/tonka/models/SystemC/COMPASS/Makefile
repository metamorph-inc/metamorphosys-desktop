

# Install directory
INSTALLDIR = /usr/local/bin


CC_SYS=g++ -std=c++11 
CFLAGS = -Wall -g 

SYSTEMCINCS= -I$(SYSTEMC_HOME)/include
SYSTEMC_LIB= -L. -L$(SYSTEMC_HOME)/lib-linux64 -Wl,-rpath=$(SYSTEMC_HOME)/lib-linux64
LFLAGS = 
LIBS = -Bstatic -lsystemc -Bdynamic -pthread -lm -lrt


SYSTEMC_SRCS= test_compass.cpp compass.cpp test_main.cpp
SYSTEMC_OBJS=$(SYSTEMC_SRCS:.cpp=.o)


MAIN = module

.PHONY: depend clean

all:    $(MAIN)

$(MAIN): $(SYSTEMC_OBJS)
	$(CC_SYS) $(CFLAGS) $(SYSTEMCINCS) $(SYSTEMC_LIB)  -o $(MAIN) $(SYSTEMC_OBJS) $(LFLAGS) $(LIBS) 


.cpp.o:
	$(CC_SYS) $(CFLAGS) $(SYSTEMCINCS) $(INCS) -c $< -o $@

clean:
	$(RM) *.o *~ $(MAIN)

depend: $(SYSTEMC_SRCS)
	makedepend $(SYSTEMCINCS) $(CFLAGS) $(SYSTEMC_SRCS)

install: $(MAIN)
	install $(MAIN) $(INSTALLDIR)



