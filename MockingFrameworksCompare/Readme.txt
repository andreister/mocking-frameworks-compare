
	MockingFrameworksCompare.

The purpose of the project is to allow developers easily compare mocking frameworks available 
for .NET, and make an informed decision when picking one up. 

It provides a unified set of tests to showcase Moq, NMock2, Rhino Mocks and Typemock Isolator.
Each test is written against every framework so you can easily compare syntax of the frameworks 
and implications of using any of them. For educational purposes, source code of the frameworks
if provided (apart from the Isolator which is a paid one - but you can download a 21 day trial).

The project contains the following test scenarios:

	1. BrainSample. 
	   This is a fictitious example of a system that needs to be tested. Contains only one simple
	   test scenario, written against every framework. 
	   
	2. ShoppingCartSample.
	   This is a more "real world" system with a few basic test cases:  
			* Test1_MockedMethod - simply mock a method
			* Test2_MockedEvent - mock an event invocation
			* Test3_MockedProperty - simply mock a property
			* Test4_MockedArgument - mock a method for a specific argument only
			* Test5_PartialMocks - mock only some methods/properties, and call original ones if they're not mocked
			* Test6_RecursiveMocks - mock chains like "foo.Boo.Zoo.Goo()" without having to mock the intermediate types.


The following versions of the mocking frameworks are used:
* Moq			3.0.204.1  (checked out 18.02.2009, revision #482)
* NMock2		1.2.3262   (checked out 18.02.2009, revision #101)
* Rhino Mocks		3.5.0.2041 (checked out 18.02.2009, revision #2041)
* Isolator		5.1.2.0 




Thanks for your interest, for more details refer to http://code.google.com/p/mocking-frameworks-compare/

For downloading latest source code, 
Moq:			svn co http://moq.googlecode.com/svn/trunk/ moq-read-only
NMock2:			svn co https://nmock2.svn.sourceforge.net/svnroot/nmock2 nmock2
Rhino Mocks:	svn co https://svn.sourceforge.net/svnroot/rhino-tools/trunk/ rhino-tools 



