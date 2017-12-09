OUTPUT=CircuitSim.dll

CHIPS=Chips/Chips.cs

CircuitSim.dll: CircuitSim.cs $(CHIPS)
	mcs -t:library CircuitSim.cs $(CHIPS)

clean:
	rm -f $(OUTPUT)
