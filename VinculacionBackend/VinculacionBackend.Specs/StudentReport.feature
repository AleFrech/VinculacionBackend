Feature: StudentCountReport
	In order to know the amount of students that did hours
	As manager
	I want to be told the amount of students

@reports
Scenario: All classes have students
	Given the amount of students for Class 'Inglés' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the amount of students for Class 'Ofimática' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the amount of students for Class 'Sociología' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the amount of students for Class 'Filosofía' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the amount of students for Class 'Ecología' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the amount of students for Faculty 1 and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the amount of students for Faculty 2 and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the current year is 2015
	When I execute the student count report
	Then the student count report should be 
		|StudentNumber| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| Inglés	  | 10		    | 30           | 20			  | 40		    |
		| Ofimática	  | 10		    | 30           | 20			  | 40		    |
		| Sociología  | 10		    | 30           | 20			  | 40		    |
		| Filosofía   | 10		    | 30           | 20			  | 40		    |
		| Ecología    | 10		    | 30           | 20			  | 40		    |
		| FIA		  | 10		    | 30           | 20			  | 40		    |
		| FCAS		  | 10		    | 30           | 20			  | 40		    |
		| Total		  | 70		    | 210          | 140		  | 280		    |

Scenario: All classes have no students
	Given the amount of students for Class 'Inglés' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 0 		  | 0            | 0			| 0 		  |
	And the amount of students for Class 'Ofimática' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 0 		  | 0            | 0			| 0 		  |
	And the amount of students for Class 'Sociología' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 0 		  | 0            | 0			| 0 		  |
	And the amount of students for Class 'Filosofía' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 0 		  | 0            | 0			| 0 		  |
	And the amount of students for Class 'Ecología' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 0 		  | 0            | 0			| 0 		  |
	And the amount of students for Faculty 1 and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 0 		  | 0            | 0			| 0 		  |
	And the amount of students for Faculty 2 and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 0 		  | 0            | 0			| 0 		  |
	And the current year is 2015
	When I execute the student count report
	Then the student count report should be 
		|StudentNumber| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| Inglés	  | 0 		    | 0            | 0			  | 0 		    |
		| Ofimática	  | 0 		    | 0            | 0			  | 0 		    |
		| Sociología  | 0 		    | 0            | 0			  | 0 		    |
		| Filosofía   | 0 		    | 0            | 0			  | 0 		    |
		| Ecología    | 0 		    | 0            | 0			  | 0 		    |
		| FIA		  | 0 		    | 0            | 0			  | 0 		    |
		| FCAS		  | 0 		    | 0            | 0			  | 0 		    |
		| Total		  | 0 		    | 0            | 0			  | 0 		    |

