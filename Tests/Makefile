TESTS:=HasError
EXES:=$(foreach test,$(TESTS),$(test).exe)

%.exe: %.cs ../CircuitSim/CircuitSim.dll
	mcs -out:$@ $< -r:../CircuitSim/CircuitSim.dll

tests: $(EXES)
	cp ../CircuitSim/CircuitSim.dll .
	mono HasError.exe

clean:
	rm -f $(EXES) CircuitSim.dll
