ReadKeys.exe: ReadKeys.cs ../../CircuitSim/CircuitSim.dll
	mcs -out:ReadKeys.exe ReadKeys.cs -r:../../CircuitSim/CircuitSim.dll
	cp ../../CircuitSim/CircuitSim.dll .

clean:
	rm -f ReadKeys.exe CircuitSim.dll
