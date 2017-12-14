NeuralNet.exe: NeuralNet.cs ../../CircuitSim/CircuitSim.dll
	mcs -debug -out:NeuralNet.exe NeuralNet.cs -r:../../CircuitSim/CircuitSim.dll
	cp ../../CircuitSim/CircuitSim.dll .

clean:
	rm -f NeuralNet.exe CircuitSim.dll
