Feature: FacultiesCostReport
	In order to know the value each faculty has given to the community
	As manager
	I want to be told the costs of each faculty

@reports
Scenario: Both Faculties have data in all periods
	Given I have this faculties
		| Id | Name         |
		| 1  | Ingenieria   |
		| 2  | Licenciatura |
	And the year is 2015
	And the cost for faculty 1 for the period 1 and year 2015 is
		| FacultyName | ProjectCost |
		| Ingenieria  | 30          |
		| Ingenieria  | 10          |
	And the cost for faculty 1 for the period 2 and year 2015 is
		| FacultyName | ProjectCost |
		| Ingenieria  | 30          |
		| Ingenieria  | 10          |
	And the cost for faculty 1 for the period 3 and year 2015 is
		| FacultyName | ProjectCost |
		| Ingenieria  | 30          |
		| Ingenieria  | 10          |
	And the cost for faculty 1 for the period 5 and year 2015 is
		| FacultyName | ProjectCost |
		| Ingenieria  | 30          |
		| Ingenieria  | 10          |
	And the cost for faculty 2 for the period 1 and year 2015 is
		| FacultyName | ProjectCost |
		| Licenciatura| 100         |
	And the cost for faculty 2 for the period 2 and year 2015 is
		| FacultyName | ProjectCost |
		| Licenciatura| 100         |
	And the cost for faculty 2 for the period 3 and year 2015 is
		| FacultyName | ProjectCost |
		| Licenciatura| 100         |
	And the cost for faculty 2 for the period 5 and year 2015 is
		| FacultyName | ProjectCost |
		| Licenciatura| 100         |
	When I execute the faculties cost report
	Then the faculties cost report should be 
		| Facultad     | Periodo 1 | Periodo 2 | Periodo 3 | Periodo 5 |
		| Ingenieria   | 40        | 40        | 40        | 40        |
		| Licenciatura | 100       | 100       | 100       | 100       |


