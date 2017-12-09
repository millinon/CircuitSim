OUTPUT:=CircuitSim.dll

MODULES:=Console Conversion Digital FloatingPoint Input Integer List Signal String

CHIPS=Chips/Chips.cs $(foreach mod,$(MODULES),Chips/$(mod)/$(mod).cs)

CircuitSim.dll: CircuitSim.cs $(CHIPS)
	mcs -t:library CircuitSim.cs $(CHIPS)

clean:
	rm -f $(OUTPUT)
