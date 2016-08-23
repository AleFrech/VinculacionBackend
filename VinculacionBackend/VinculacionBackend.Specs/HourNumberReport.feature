Feature: HourNumberReport
	In order to know the amount of hours done in a year
	As manager
	I want to be told the amount of hours in each period

@reports
Scenario: All classes have hours done
	Given the amount of hours for Class 'Inglés' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the amount of hours for Class 'Ofimática' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the amount of hours for Class 'Sociología' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the amount of hours for Class 'Filosofía' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the amount of hours for Class 'Ecología' and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the amount of hours for Faculty 1 and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the amount of hours for Faculty 2 and year 2015 is
		| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| 10		  | 30           | 20			| 40		  |
	And the hours year is 2015
	When I execute the hour number report
	Then the hour number report should be 
		|StudentNumber| FirstPeriod | SecondPeriod | FourthPeriod | FifthPeriod |
		| Inglés	  | 10		    | 30           | 20			  | 40		    |
		| Ofimática	  | 10		    | 30           | 20			  | 40		    |
		| Sociología  | 10		    | 30           | 20			  | 40		    |
		| Filosofía   | 10		    | 30           | 20			  | 40		    |
		| Ecología    | 10		    | 30           | 20			  | 40		    |
		| FIA		  | 10		    | 30           | 20			  | 40		    |
		| FCAS		  | 10		    | 30           | 20			  | 40		    |
		| Total		  | 70		    | 210          | 140		  | 280		    |

Scenario: All classes have no hours done
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

