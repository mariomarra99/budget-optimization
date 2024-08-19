using System;

class CampaignBudget
{
    // Function to calculate the total campaign budget given ad spend values
    public static double CalculateTotalBudget(double[] adBudgets, double agencyFeePercentage, double thirdPartyFeePercentage, double fixedAgencyHoursCost)
    {
        double totalAdSpend = 0;

        // Sum the budgets of all ads to get the total ad spend
        for (int i = 0; i < adBudgets.Length; i++)
        {
            totalAdSpend += adBudgets[i];
        }

        // Calculate the third-party fee considering only the first, second, and fourth ads if available
        double thirdPartyFee = thirdPartyFeePercentage * (
            (adBudgets.Length > 0 ? adBudgets[0] : 0) + 
            (adBudgets.Length > 1 ? adBudgets[1] : 0) + 
            (adBudgets.Length > 3 ? adBudgets[3] : 0)
        );

        // Calculate the agency fee as a percentage of the total ad spend
        double agencyFee = agencyFeePercentage * totalAdSpend;

        // Return the total campaign budget, including all costs
        return totalAdSpend + agencyFee + thirdPartyFee + fixedAgencyHoursCost;
    }

    // Goal Seek function to find the maximum budget for a specific ad
    public static double GoalSeekForAdBudget(double approvedBudget, double[] adBudgets, int adIndex, double agencyFeePercentage, double thirdPartyFeePercentage, double fixedAgencyHoursCost, double tolerance = 0.01, int maxIterations = 1000)
    {
        // Define the lower and upper bounds for the binary search
        double low = 0;
        double high = approvedBudget;

        // Save the original budget of the ad to restore it later
        double originalBudget = adBudgets[adIndex];

        int iterations = 0; // Counter for the number of iterations

        // Binary search to find the maximum budget for the specific ad
        while (high - low > tolerance)
        {
            if (iterations >= maxIterations)
            {
                // If the maximum number of iterations is reached, throw an exception or return a special value
                throw new InvalidOperationException("Goal seek did not converge within the allowed number of iterations.");
            }

            adBudgets[adIndex] = (low + high) / 2; // Set the current budget for the ad
            double totalBudget = CalculateTotalBudget(adBudgets, agencyFeePercentage, thirdPartyFeePercentage, fixedAgencyHoursCost);

            if (totalBudget > approvedBudget)
            {
                // If the total budget exceeds the approved budget, reduce the upper bound
                high = adBudgets[adIndex];
            }
            else
            {
                // If the total budget is within the approved budget, increase the lower bound
                low = adBudgets[adIndex];
            }

            iterations++; // Increment the iteration counter
        }

        // Restore the original budget value for the ad
        adBudgets[adIndex] = originalBudget;

        // Return the maximum budget found for the ad
        return (low + high) / 2;
    }

    static void Main(string[] args)
    {
        try
        {
            // Check for command-line arguments
            if (args.Length > 0)
            {
                if (args[0] ==  "help" || args[0] == "--h")
                {
                    // Display help information
                    Console.WriteLine("=== Campaign Budget Optimization Help ===");
                    Console.WriteLine("Usage: dotnet run [--test] [-- help | -- h]");
                    Console.WriteLine();
                    Console.WriteLine("dotnet run -- help | -- h: Displays this help message.");
                    Console.WriteLine("dotnet run -- test: Runs the program in test mode with predefined values.");
                    Console.WriteLine("To run the program in regular mode, simply use: dotnet run");
                    Console.WriteLine();
                    Console.WriteLine("In test mode, the program uses predefined values for campaign budget optimization.");
                    Console.WriteLine("In regular mode, the program prompts the user for input.");
                    Console.WriteLine();
                    return;
                }
                else if (args[0] == "test")
                {
                    // Test cases
                    var testCases = new[]
                    {
                        new { AdBudgets = new double[] { 500, 1000, 1500, 2000 }, AgencyFee = 0.10, ThirdPartyFee = 0.05, FixedCost = 300, ApprovedBudget = 5000, AdIndex = 2, ExpectedMaxBudget = 1340.34 },
                        new { AdBudgets = new double[] { 800, 1200, 1800 }, AgencyFee = 0.12, ThirdPartyFee = 0.06, FixedCost = 400, ApprovedBudget = 6000, AdIndex = 1, ExpectedMaxBudget = 1751.46 },
                        new { AdBudgets = new double[] { 1000, 2000, 3000, 4000 }, AgencyFee = 0.08, ThirdPartyFee = 0.07, FixedCost = 250, ApprovedBudget = 8000, AdIndex = 0, ExpectedMaxBudget = 640.23 },
                        new { AdBudgets = new double[] { 600, 900, 1200 }, AgencyFee = 0.15, ThirdPartyFee = 0.05, FixedCost = 500, ApprovedBudget = 5000, AdIndex = 2, ExpectedMaxBudget = 1704.28 },
                        new { AdBudgets = new double[] { 400, 800, 1200, 1600 }, AgencyFee = 0.10, ThirdPartyFee = 0.04, FixedCost = 350, ApprovedBudget = 5500, AdIndex = 3, ExpectedMaxBudget = 1314.78 }
                    };

                    Console.WriteLine("=== Campaign Budget Optimization - Test Mode ===");

                    foreach (var testCase in testCases)
                    {   
                        Console.WriteLine();
                        Console.WriteLine($"Ad Budgets: {string.Join(", ", testCase.AdBudgets)}");
                        Console.WriteLine($"Agency Fee Percentage: {testCase.AgencyFee:P}");
                        Console.WriteLine($"Third-Party Fee Percentage: {testCase.ThirdPartyFee:P}");
                        Console.WriteLine($"Fixed Agency Hours Cost: {testCase.FixedCost}");
                        Console.WriteLine($"Approved Budget: {testCase.ApprovedBudget}");
                        Console.WriteLine($"Ad Index to Optimize: {testCase.AdIndex + 1}");

                        try
                        {
                            // Perform the goal seek algorithm to find the maximum budget for the selected ad
                            double maxAdBudget = GoalSeekForAdBudget(
                                testCase.ApprovedBudget,
                                testCase.AdBudgets,
                                testCase.AdIndex,
                                testCase.AgencyFee,
                                testCase.ThirdPartyFee,
                                testCase.FixedCost
                            );
                            
                            //Display the target value:
                            Console.WriteLine($"ExpectedMaxBudget (Target value): {testCase.ExpectedMaxBudget:F2}");

                            // Display the result
                            Console.WriteLine($"The maximum budget for ad X{testCase.AdIndex + 1} is: {maxAdBudget:F2}");

                        }
                        catch (InvalidOperationException ex)
                        {
                            // Handle cases where the goal seek does not converge
                            Console.WriteLine($"Goal seek failed: {ex.Message}");
                        }
                    }

                    Console.WriteLine("=======================================");
                }
                else
                {
                    // Handle invalid argument
                    Console.WriteLine("Invalid argument. Use -- help or --h for usage information.");
                }
            }
            else
            {
                // Regular mode - get inputs from the user
                Console.WriteLine("=== Campaign Budget Optimization ===");

                // Get the number of ads from the user
                int numAds = GetPositiveIntegerInput("Enter the number of ads: ");

                // Initialize an array to store the budgets for each ad
                double[] adBudgets = new double[numAds];

                // Get the budget for each ad from the user
                for (int i = 0; i < numAds; i++)
                {
                    adBudgets[i] = GetNonNegativeDoubleInput($"Enter the budget for ad X{i + 1}: ");
                }

                // Get the agency fee percentage from the user
                double agencyFeePercentage = GetPercentageInput("Enter the agency fee percentage (as a decimal, e.g., 0,10 for 10%): ");

                // Get the third-party tool fee percentage from the user
                double thirdPartyFeePercentage = GetPercentageInput("Enter the third-party tool fee percentage (as a decimal, e.g., 0,05 for 5%): ");

                // Get the fixed cost for agency hours from the user
                double fixedAgencyHoursCost = GetNonNegativeDoubleInput("Enter the fixed cost for agency hours: ");

                // Get the approved total campaign budget from the user
                double approvedBudget = GetNonNegativeDoubleInput("Enter the approved total campaign budget: ");

                // Get the index of the ad to optimize from the user
                int adIndex = GetAdIndexInput(numAds);

                // Perform the goal seek algorithm to find the maximum budget for the selected ad
                double maxAdBudget = GoalSeekForAdBudget(approvedBudget, adBudgets, adIndex, agencyFeePercentage, thirdPartyFeePercentage, fixedAgencyHoursCost);

                // Display the result to the user
                Console.WriteLine();
                Console.WriteLine($"The maximum budget for ad X{adIndex + 1} is: {maxAdBudget:F2}");
                Console.WriteLine("=======================================");
            }
        }
        catch (InvalidOperationException ex)
        {
            // Handle cases where the goal seek does not converge
            Console.WriteLine($"Goal seek failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors that occur during the program execution
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    // Helper function to get a positive integer input from the user
    static int GetPositiveIntegerInput(string prompt)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out value) && value > 0)
            {
                return value;
            }
            Console.WriteLine("Invalid input. Please enter a positive integer.");
        }
    }

    // Helper function to get a non-negative double input from the user
    static double GetNonNegativeDoubleInput(string prompt)
    {
        double value;
        while (true)
        {
            Console.Write(prompt);
            if (double.TryParse(Console.ReadLine(), out value) && value >= 0)
            {
                return value;
            }
            Console.WriteLine("Invalid input. Please enter a non-negative number.");
        }
    }

    // Helper function to get a percentage input (a double between 0 and 1) from the user
    static double GetPercentageInput(string prompt)
    {
        double value;
        while (true)
        {
            Console.Write(prompt);
            if (double.TryParse(Console.ReadLine(), out value) && value >= 0 && value <= 1)
            {
                return value;
            }
            Console.WriteLine("Invalid input. Please enter a decimal number between 0 and 1.");
        }
    }

    // Helper function to get the index of the ad to optimize from the user
    static int GetAdIndexInput(int numAds)
    {
        int value;
        while (true)
        {
            Console.Write($"Enter the ad number (1 to {numAds}) you want to optimize: ");
            if (int.TryParse(Console.ReadLine(), out value) && value > 0 && value <= numAds)
            {
                return value - 1; // Convert to zero-based index
            }
            Console.WriteLine($"Invalid input. Please enter a number between 1 and {numAds}.");
        }
    }
}
