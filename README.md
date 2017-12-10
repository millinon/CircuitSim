# CircuitSim
A library for simulating digital circuits

---

CircuitSim is a C# library that enables a different model of computation. Functions are represented as chips, which can be composed in different ways.

There are a few fundamental units of CircuitSim:

* Component
* Input
* Output

### Component

A [component](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/CircuitSim.cs#L7) is a set of inputs and outputs, and a transformation of input values to output values. Either the inputs or outputs may be empty, but not both. Behavior is implemented in the form of [chips](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/Chips/Chips.cs). For example, we can imagine making an [Sub](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/Chips/Integer/Arithmetic.cs#L49) chip that computes the difference of two integers.

I started noticing some patterns that made more sense to generalize into a set of common behaviors. These classes can be found in [Chips.cs](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/Chips/Chips.cs):

* [NToOne](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/Chips/Chips.cs#L9) - these chips take N inputs and produce one output
* [Predicate](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/Chips/Chips.cs#L79) - take one input and produce one boolean output
* [BinaryComparator](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/Chips/Chips.cs#L120) - take two inputs and produce one boolean output
* [UnaryFunctor](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/Chips/Chips.cs#L161) - take one input and produce one output
* [BinaryFunctor](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/Chips/Chips.cs#L204) - take two inputs and produce one output

Every component has a few important methods:

* Tick()
* Compute()
* Set()
* Detach()

#### Tick()

Tick() is simply a call to Compute() followed by a call to Set(). It is important to note that a call to Tick() will recursively call Tick() on components connected to outputs. This is fundamentally how CircuitSim works. Generally, we do not want c.Tick() to recursively call C.Tick() again, so there is a check that can be disabled.

#### Compute()

Compute() is the method that updates the internal state of the component. For example, for our Sub chip, we might have an internal int variable (perhaps \_diff), which is computed from the input values when Compute() is called.

#### Set()

Set() is the method that updates the external state of the component. This generally always means writing the internal value(s) to the output(s). For our Sub chip, this would mean writing the value of \_diff to the component's output. As covered below, writing to an output calls Tick() on all of the components connected to the output.

#### Detach()

Detach() is the method that disconnects all of a component's inputs and outputs. This may be important for enabling components / inputs / outputs to be garbage collected.

There are a few fields that can be accessed:

* HasError: a component will set HasError to true if it is in an error state. For example, the integer division chip, upon dividing by zero, will throw an exception, which will be caught, and will then set its HasError to true.
* AllowRecursion: this is a hack to enable some special chips (like [Map](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/Chips/List/List.cs#L5) to work recursively. You generally should not set AllowRecursion to true, unless you find yourself in a situation where that would be the only solution.

### Input

An [input](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/CircuitSim.cs#L55) is how a component gets the values that it computes its outputs from. As the Input class is a generic, an input can be of any type. I frequently use Input<T> where T is one of the following:

* bool: digital input
* int: integer input
* double: floating point input
* string: string input
* List: list input

An input has a few fields:

* Component: the component to which the input belongs
* Source: this is the Output<T> from which the input should get its value
* Value: this is the T, which returns the value from the connected output. If no output is connected, an exception is thrown.

An input must be connected to exactly one output. To connect an Input to an Output, you can assign the Input's Source field to the Output. C#'s type safety will only allow you to connect an Input<T> to an Output<T>, which is the desired behavior.

The classes [Inputs](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/CircuitSim.cs#L110) and [InputSet](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/CircuitSim.cs#L595) are collections of Inputs. The difference is that the Inputs class has its inputs named (A, B, C, ...), while the InputSet acts like an array of Inputs.

The only special behavior of inputs that might be surprising is that when you reassign an Input, it will first attempt to detach itself from any outputs it is connected to. This is important because every Output has a list of Inputs to which it is connected.

### Output

An [output](https://github.com/millinon/CircuitSim/blob/master/CircuitSim/CircuitSim.cs#L637) is how a component emits computed values. Outputs are also generic.

An output has a few fields:

* Component: the component to which the output belongs
* AutoTick: whether or not the output should call Tick() on components that are connected to it. Generally this should be true. The exception is if a group of compomnents are cyclical, in which case a recursive Tick() would cause a stack overflow.
* AlwaysUpdate: whether updating the output's value should always call Tick() on connected components. I thought it made more sense to have this false by default, but it turns out that things work better if this is true. If false, then Tick() is only called when the value changes.
* Value: this is a buffer for the value that gets passed between components.

---

### Circuit Composition

To use CircuitSim to accomplish more complex behavior with a new component, there are two approaches:

* Write the component's Compute() and Set() methods to use C# to manage the internal state of the component
* Use CircuitSim chips to manage the internal state of the component

The first approach is probably easier and less error prone, but the second approach is definitely more fun. For example, consider the floating point inverse chip. Here is an implementation using the first approach:

```csharp
public class Inv : Component 
{
    private static uint count = 0;

    public Inputs<double> Inputs;

    public Outputs<double> Outputs;
    private double _out;

    public Inv() : base($"FltInv{count++}")
    {
        Inputs = new Inputs<double>(this);

        Outputs = new Outputs<double>(this);

    }

    public override void Compute()
    {
        _out = 1 / Inputs.A.Value;
    }

    public override void Set()
    {
        Outputs.Out.Value = _out;
    }

    public override void Detach()
    {
        Inputs.Detach();

        Outputs.Detach();
    }
}
```

This works as expected, but where's the fun in that? Consider an implementation that uses CircuitSim chips to perform the computation. For this, we need one floating point division chip, and one constant chip to hold the '1' output value:


```csharp
public class Inv : Component 
{
    private static uint count = 0;

    public Inputs<double> Inputs;

    public Outputs<double> Outputs;

    private Div div = new Div();
    private Constant<double> one = new Constant<double>(new double[] { 1.0 });
    
    public Inv() : base($"FltInv{count++}")
    {
        Inputs = new Inputs<double>(this);

        Outputs = new Outputs<double>(this);

        div.Inputs.A.Source = one.Outputs[0];
    }

    public override void Compute()
    {
        div.Inputs.B = Inputs.A;
        div.Tick();
    }

    public override void Set()
    {
        Outputs.Out.Value = div.Outputs.Out.Value;
    }

    public override void Detach()
    {
        Inputs.Detach();

        Outputs.Detach();
    }
}
```

This approach produces the same result, and we didn't have to worry about the internal \_out state. There are a few important things to note:

* The division chip's B input has to be reassigned every time that Compute() is called since the main chip's Inputs.A may have been reassigned
* We have to call div.Tick() since it is not going to Tick() automatically when the main chip's input changes

