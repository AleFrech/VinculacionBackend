Feature: ProjectFinalReport
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Project Final Report Is Valid
	Given I have a ProjectId 1
	And I have a SectionId 23
	And The Project with Id 1 is
		| Id | ProjectId | Name    | Description        | IsDeleted | BeneficiarieOrganization |
		| 1  | id-100    | Mohtivo | DescripcionProyect | False     | Escuela                  |
	And The Section belonging to Project with id 1 is
		| Id | Code | ProfessorName |
		| 23 | s1   | Xavier        |
	And The students in Section 23 Are
		| Id | AccountId | Name | Major           |
		| 1  | 21324124  | Jose | Ing. Sistemas   |
		| 2  | 21324166  | Juan | Ing. Mecatronica|
	And The Students Majors is "Ing. Sistema / Ing. Mecatronica"
	And The SectionProject with id 5 is
		| Id | Costo |
		| 5  | 1000  |
	And The students hours for Project 1 are
		| Id | AccountId | Name | Hours |
		| 1  | 21324124  | Jose | 40    |
		| 2  | 21324166  | Juan | 20    |
	And I have a SectionProjectId 5
	And FieldHours 10
	And Calification 90
	And BeneficiariesQuantity 2042
	And BeneficiarieGroups "UNITEC"
	When I execute GenerateFinalReportModel
	Then The Final Report Model Project Should Be
	| FieldHours | Calification | BeneficiariesQuantity |
	| 10         | 90           | 2042                  |
