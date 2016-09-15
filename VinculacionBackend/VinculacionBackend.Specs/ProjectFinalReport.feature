Feature: ProjectFinalReport
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Project Final Report Is Valid
	Given I have a ProjectId 1
	And I have a SectionProjectId 2
	And FieldHours 10
	And Calification 90
	And BeneficiariesQuantity 2042
	And BeneficiarieGroups UNITEC
	When I execute GenerateFinalReportModel
	Then the result should be 120 on the screen
