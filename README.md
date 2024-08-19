# Campaign Budget Optimization

This is a simple console application written in C# that calculates the total campaign budget given ad spend values using a Goal Seek function.

## Prerequisites

- [.NET SDK 6.0 or higher](https://dotnet.microsoft.com/download/dotnet)

## Getting Started

Clone this repository to your local machine:

```bash
git clone https://github.com/mariomarra99/campaign-budget-optimization.git
```
Navigate to the project directory:

```bash
cd campaign-budget-optimization 
```
## How to Run

### 1. Run in Regular Mode

To run the application in regular mode, where you will be prompted for input, use the following command:

` dotnet run` 

The application will prompt you for the following inputs:

-   Number of ads
-   Budget for each ad
-   Agency fee percentage
-   Third-party tool fee percentage
-   Fixed cost for agency hours
-   Approved total campaign budget
-   The index of the ad you want to optimize

### 2. Run in Test Mode (TEST CASES REQUIRED)

To run the application in test mode with predefined values, use the following command:

`dotnet run --test` 

In test mode, the application will execute a series of predefined test cases and display the results for each with a help of an Expected value to compare the ideal of cases compared to reality according to our calculations.


### 3. Display Help Information

To display help information on how to use the application, run:

`dotnet run --help` 

This will show usage instructions and information about the available modes.

## Example Usage

### Regular Mode

`dotnet run` 

Example Input:
```yaml
Enter the number of ads: 4
Enter the budget for ad X1: 500
Enter the budget for ad X2: 1000
Enter the budget for ad X3: 1500
Enter the budget for ad X4: 2000
Enter the agency fee percentage (as a decimal, e.g., 0.10 for 10%): 0.10
Enter the third-party tool fee percentage (as a decimal, e.g., 0.05 for 5%): 0.05
Enter the fixed cost for agency hours: 300
Enter the approved total campaign budget: 5000
Enter the ad number (1 to 4) you want to optimize: 3
```

Example Output:
```yaml
The maximum budget for ad X3 is: 1340.34
```

### Test Mode

`dotnet run --test` 

Example Output:

```yaml
`=== Campaign Budget Optimization - Test Mode ===

Ad Budgets: 500, 1000, 1500, 2000
Agency Fee Percentage: 10.00%
Third-Party Fee Percentage: 5.00%
Fixed Agency Hours Cost: 300
Approved Budget: 5000
Ad Index to Optimize: 3
ExpectedMaxBudget (Target value): 1340,34
The maximum budget for ad X3 is: 613,64
...

=======================================
```

## Error Handling

-   The application includes error handling for invalid inputs (e.g., negative numbers, invalid percentages).
-   If the goal-seek function fails to converge within the allowed number of iterations, an error message will be displayed.
