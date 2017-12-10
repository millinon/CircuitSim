TESTS:=HasError Repeat
EXES:=$(foreach test,$(TESTS),$(test).exe)

test: $(EXES)
	cp ../CircuitSim/CircuitSim.dll .
	mono HasError.exe
	mono Repeat.exe

%.exe: %.cs ../CircuitSim/CircuitSim.dll
	mcs -out:$@ $< -r:../CircuitSim/CircuitSim.dll

clean:
	rm -f $(EXES) CircuitSim.dll
