Imports MockingFrameworksCompare.BrainSample
Imports NUnit.Framework
Imports TypeMock.ArrangeActAssert

<TestFixture()> _
Public Class BrainTests
    ''' <summary>
    ''' Verify that if a hot iron is passed, mouth.Yell gets called. 
    ''' </summary>
    ''' <remarks>
    ''' In this test, Isolator is used in a Dependency Injection scenario 
    ''' (hand and mouth are injected into the brain instance). All other
    ''' mocking frameworks could do that as well.
    ''' </remarks>
    <Test()> _
    <Isolated()> _
    Public Sub TouchHotIron_Yell()
        Dim hand As Hand = Isolate.Fake.Instance(Of Hand)()
        Dim mouth As Mouth = Isolate.Fake.Instance(Of Mouth)()
        Dim iron As New Iron() With {.IsHot = True}

        'Dim callMe As Func(Of Hand) = Function() hand.TouchIron(iron)
        Dim callMe2 = New Action(Of Iron)(AddressOf hand.TouchIron)

        'Isolate.WhenCalled(callMe2).WillThrow(New BurnException)

        Dim brain As New Brain(hand, mouth)
        brain.TouchIron(iron)

        'Isolate.Verify.WasCalledWithAnyArguments(Of Mouth)(AddressOf mouth.Yell)
    End Sub
End Class
