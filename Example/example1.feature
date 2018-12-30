#comment1a
#comment1b
""" multiline
comments
here"""

@tag1 @tag2 @tag 3
@tag   4
Feature: Setting starting points and destinations

some description
extra line

Background: some info
    Given a global administrator named "Greg"
    And a blog named "Greg's anti-tax rants"
    And a customer named "Dr. Bill"
    And a blog named "Expensive Therapy" owned by "Dr. Bill"

    Examples:
    | start | eat | left |
    |    12 |   5 |    7 |
    |    20 |   5 |   15 |

@myoutline
Scenario Outline: Add two numbers <num1> & <num2>
    Given I have a calculator
    When I add <num1> and <num2>
    Then the result should be <total>

  #comment 2
  @location
  Scenario: Starting point should be set to current location

    a wonderful story
    about everythings

    Given a commuter that enabled location tracking
      And has a nice phone
      But it's not an android
     When the commuter wants to plan a journey
     Then the starting point should be set to current location

  Scenario: Commuters should be able to choose bus stops and locations
     Given a bus stop at Edison Street
       And a Edison Business Center building at Main Street
      When the commuter chooses a destination
    #comment 3
      Then the commuter should be able to choose Edison Street
       But the commuter should be also able to choose Edison Business Center

    #comment 4