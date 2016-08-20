Feature: FacultiesReports
	In order to know the value each faculty has given to the community
	As manager
	I want to be told the costs of each faculty

Scenario: Costs report by faculties
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

Scenario: Hours report by faculties
Given I have this faculties
		| Id | Name         |
		| 1  | Ingenieria   |
		| 2  | Licenciatura |
	And the year is 2015
	And the hours for faculty 1 for the year 2015 is
		| FacultyName | ProjectHours |
		| Ingenieria  | 70           |
		| Ingenieria  | 80           |
		| Ingenieria  | 50           |
		| Ingenieria  | 60           |
		| Ingenieria  | 95           |
		| Ingenieria  | 74           |
		| Ingenieria  | 76           |
		| Ingenieria  | 65           |
	And the hours for faculty 2 for the year 2015 is
		| FacultyName | ProjectHours |
		| Licenciatura| 80           |
		| Licenciatura| 95           |
		| Licenciatura| 85           |
		| Licenciatura| 84           |		
	When I execute the faculties hours report
	Then the faculties hour report should be 
		| Facultad     | Horas	   |
		| Ingenieria   | 570       |
		| Licenciatura | 344       |

