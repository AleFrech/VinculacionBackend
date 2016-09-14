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

Scenario:  projects number by a major per current period
	Given I have this majors
		| Id | MajorId | Name          | Faculty |
		| 1  | 1       | Ing. Sistemas | 1       |
		| 2  | 2       | Mecatronica   | 1       |
		| 3  | 3       | Ing. Civil    | 1       |
		| 4  | 4       | Lic. Admon    | 2       |
	And This is the current period
		| Id | Number | Year | FromDate | ToDate   | IsCurrent |
		| 1  | 1      | 2016 | 08/08/16 | 10/10/16 | True      |  
	And I have the majors and it has many projects
		| MajorId | Major         | Total |
		|   1     | Ing. Sistemas | 3     |
		|   1     | Ing. Sistemas | 5     |
		|	2     | Mecatronica   | 4     |
		|	3     | Ing. Civil    | 3     |
		|	3     | Ing. Civil    | 2     |
		|	4     | Lic. Admon    | 3     |
	When I execute the projects by major report
	Then I have the projects
		| Carrera       | Proyectos|
		| Ing. Sistemas | 8        |
		| Mecatronica   | 4        |
		| Ing. Civil    | 5        |
		| Lic. Admon    | 3        |

Scenario: List all the projects that has been made in a class
	Given I have the section
			| Code | Class | Period | User |
			| 1    | 1     | 1      | 1    |
	And The period is the number 1 of year 2016
	And The class 1 has the projects
		| Id | ProjectId | Name               | Description        | IsDeleted | BeneficiarieOrganization |
		| 1  | 1         | Vinculacion Unitec | Vinculacion Unitec | false     | Unitec                   |
		| 2  | 2         | Mhotivo            | Mhotivo            | false     | Mhotivo                  |
		| 3  | 3         | Proyecto CCIC      | Proyecto CCIC      | false     | CCIC                     |
	And The list of professors is
		| Id | AccountId | Name    | Password | Major  | Campus | Email             | CreationDate | ModificationDate | Finiquiteado | Status |
		| 1  | 1         | Andrea  | 1234     |   1    | SPS    | andrea@gmail.com  | 08/08/16     | 10/10/16         | False        | Active |
	When I execute the list projects by class report
	Then The result should be
		| IdProyecto | Nombre             | Beneficiario | Maestro | Periodo | Anio |
		| 1          | Vinculacion Unitec | Unitec       | Andrea  | 1       | 2016 |
		| 2          | Mhotivo            | Mhotivo      | Andrea  | 1       | 2016 |
		| 3          | Proyecto CCIC      | CCIC         | Andrea  | 1       | 2016 |