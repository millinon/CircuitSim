OUTPUT:=CircuitSim.dll

MODULES:=Console Digital FloatingPoint Input Integer List Signal String Time
MATH_MODULES:=FloatingPoint Integer
CONV_MODULES:=Digital FloatingPoint Integer String Time
COMP_MODULES:=Digital FloatingPoint Integer String

CHIPS=Chips/Chips.cs $(foreach mod,$(MODULES),Chips/$(mod)/$(mod).cs) $(foreach mod,$(MATH_MODULES),Chips/$(mod)/Arithmetic.cs) $(foreach mod,$(CONV_MODULES),Chips/$(mod)/Conversion.cs) $(foreach mod,$(COMP_MODULES),Chips/$(mod)/Comparison.cs)

CircuitSim.dll: CircuitSim.cs $(CHIPS)
	mcs -t:library CircuitSim.cs $(CHIPS)

clean:
	rm -f $(OUTPUT)
