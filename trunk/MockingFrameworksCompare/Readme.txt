
	MockingFrameworksCompare.

---------------------------------------
----------------- WHY -----------------
---------------------------------------
The purpose of the project is to allow developers easily compare mocking frameworks 
available for .NET, and make an informed decision when picking one up. 

---------------------------------------
----------------- HOW -----------------
---------------------------------------
It provides a unified set of tests to showcase Moq, NMock2, Rhino Mocks, Typemock Isolator 
and a new star of mocking - Stubs framework from Microsoft Research.

Each test is written against every framework so you can easily compare syntax and implications
of using any of them. For educational purposes, source code of the frameworks if provided, 
apart from:
	* the Isolator (which is a paid one - but you can download a 21 day trial)
	* and Stubs (which is free but not open source).

---------------------------------------
----------------- WHAT-----------------
---------------------------------------
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
			* Test5_PartialMocks - mock only some methods/properties, and call original ones if they're not mocked.
			* Test6_RecursiveMocks - mock chains like "user.ContactDetails.Address.GetZipCode()" without having to mock the intermediate types.
			
	3. Performance.
	   This project runs all tests from ShoppingCartSample in a sophisticated loop, and displays 
	   an average speed for some standard scenarios in each mocking framework. Performance is measured
	   based on CodeTimers, amazing approach from Vance Morrison (http://blogs.msdn.com/vancem/archive/2006/09/21/765648.aspx)
		
	4. FailingSample.
	   This one reveals the differences in error messages that come from the mockings frameworks.


---------------------------------------
------------ OTHER DETAILS ------------
---------------------------------------
The following versions of the mocking frameworks are used:
* Moq:			3.5.0.0 (checked out 01.08.2009, revision #604)
* NMock2:		1.2.3262 (checked out 31.05.2009, revision #153)
* Rhino Mocks:		3.5.0.2041 (checked out 31.05.2009, revision #2217)
* Isolator:		5.3.1.0 
* Stubs:		0.14.40610.2




Thanks for your interest, for more details refer to http://code.google.com/p/mocking-frameworks-compare/

For downloading latest source code, 
Moq:		svn co http://moq.googlecode.com/svn/trunk/ moq-read-only
NMock2:		svn co https://nmock2.svn.sourceforge.net/svnroot/nmock2 nmock2
Rhino Mocks:	svn co https://svn.sourceforge.net/svnroot/rhino-tools/trunk/ rhino-tools 

A free SVN client is available at http://www.sliksvn.com/en/download


