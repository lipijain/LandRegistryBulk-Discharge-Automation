Feature: LoginPage
As a user I want to be able to navigate to land registry landing page and login

Background: Navigate to land registry url landing page 
	

 #Logon into e-DS1 page
 Scenario: E2E for application submission 
  Given I am on land registry sign-in page
  When I enter Username in the user name field
  And I enter Password in the password field
  Then I click the login button
  And data successfully sumitted
  And I click request official copies go to service button
  Then I click lender services

  #Discharge numeber e-DS1 page
  When I am on e-DS1 discharge page
  #I Read address on excel datasheet and select and enter the title number and enter Charge details 
  Then I perform the end to end application submission for all the data
  #Submission page and I copy the address & application reference to the excel data sheet
  Then I click logout button


#Scenario: Login to discharge page 
#  Given I am on land registry sign-in page
#  When I enter Username in the user name field
#  And I enter Password in the password field
#  Then I click the login button
#  And data successfully sumitted
#  And I click request official copies go to service button
#  Then I click lender services

# #Discharge numeber e-DS1 page
# Given I am on e-DS1 discharge page
# #Read address on excel datasheet and select and enter the title number
# When I have identified address on excel datasheet and select the title number
# And I click next button

# #Charge details page
# And I enter date of charge 
# And I select yes for  borrowers redemption button 
# And I select yes for messages option
# And I click next button
# And I click next button

# #Application Submission page
# And I enter customer reference
# And I click submit button
# And I copy the address to the excel data sheet
# And I copy the application reference to the excel data sheet
# Then I click logout button