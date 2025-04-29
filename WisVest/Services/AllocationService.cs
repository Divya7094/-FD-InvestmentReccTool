using System.Text.Json;
using WisVestAPI.Models.DTOs;
using WisVestAPI.Models.Matrix;
using WisVestAPI.Repositories.Matrix;
using WisVestAPI.Services.Interfaces;

namespace WisVestAPI.Services
{
    public class AllocationService : IAllocationService
    {
        private readonly MatrixRepository _matrixRepository;

        public AllocationService(MatrixRepository matrixRepository)
        {
            _matrixRepository = matrixRepository;
        }

        public async Task<Dictionary<string, double>> CalculateFinalAllocation(UserInputDTO input)
        {
            Console.WriteLine("Starting allocation calculation...");

            // Load the allocation matrix
            var allocationMatrix = await _matrixRepository.LoadMatrixDataAsync();
            if (allocationMatrix == null)
            {
                Console.WriteLine("Error: Allocation matrix is null.");
                throw new InvalidOperationException("Allocation matrix data is null.");
            }
            Console.WriteLine("Allocation matrix loaded successfully.");

            // Step 1: Map input values to match JSON keys
            var riskToleranceMap = new Dictionary<string, string>
            {
                { "Low", "Low" },
                { "Medium", "Mid" },
                { "High", "High" }
            };

            var investmentHorizonMap = new Dictionary<string, string>
            {
                { "Short", "Short" },
                { "Moderate", "Mod" },
                { "Long", "Long" }
            };

            var riskToleranceKey = riskToleranceMap[input.RiskTolerance ?? throw new ArgumentException("RiskTolerance is required")];
            var investmentHorizonKey = investmentHorizonMap[input.InvestmentHorizon ?? throw new ArgumentException("InvestmentHorizon is required")];
            var riskHorizonKey = $"{riskToleranceKey}+{investmentHorizonKey}";

            Console.WriteLine($"Looking up base allocation for key: {riskHorizonKey}");

            // Step 2: Determine base allocation
            if (!allocationMatrix.Risk_Horizon_Allocation.TryGetValue(riskHorizonKey, out var baseAllocation))
            {
                Console.WriteLine($"Error: No base allocation found for key: {riskHorizonKey}");
                throw new ArgumentException($"Invalid combination of RiskTolerance and InvestmentHorizon: {riskHorizonKey}");
            }
            Console.WriteLine($"Base allocation found: {JsonSerializer.Serialize(baseAllocation)}");

            // Clone the base allocation to avoid modifying the original matrix
            var finalAllocation = new Dictionary<string, double>(baseAllocation);

            // Step 3: Apply age adjustment rules
            var ageRuleKey = GetAgeGroup(input.Age);
            Console.WriteLine($"Looking up age adjustment rules for key: {ageRuleKey}");

            if (allocationMatrix.Age_Adjustment_Rules.TryGetValue(ageRuleKey, out var ageAdjustments))
            {
                Console.WriteLine($"Age adjustments found: {JsonSerializer.Serialize(ageAdjustments)}");
                foreach (var adjustment in ageAdjustments)
                {
                    if (finalAllocation.ContainsKey(adjustment.Key))
                    {
                        finalAllocation[adjustment.Key] += adjustment.Value;
                    }
                }
            }
            else
            {
                Console.WriteLine($"No age adjustments found for key: {ageRuleKey}");
            }

            // Step 4: Apply goal tuning
            Console.WriteLine($"Looking up goal tuning for goal: {input.Goal}");
            if (allocationMatrix.Goal_Tuning.TryGetValue(input.Goal, out var goalTuning))
            {
                Console.WriteLine($"Goal tuning found: {JsonSerializer.Serialize(goalTuning)}");

                if (goalTuning.TryGetValue("equity_boost", out var equityBoostObj))
                {
                    var equityBoost = GetDoubleFromObject(equityBoostObj);
                    finalAllocation["equity"] += equityBoost;

                    // Redistribute the remaining percentages
                    var remainingAdjustment = -equityBoost;
                    var categoriesToAdjust = new[] { "fixedIncome", "commodities", "cash", "realEstate" };

                    foreach (var category in categoriesToAdjust)
                    {
                        if (finalAllocation.ContainsKey(category))
                        {
                            finalAllocation[category] += remainingAdjustment / categoriesToAdjust.Length;
                        }
                    }
                }

                if (goalTuning.TryGetValue("equity_threshold", out var equityThresholdObj) &&
                    finalAllocation["equity"] > GetDoubleFromObject(equityThresholdObj))
                {
                    finalAllocation["equity"] = GetDoubleFromObject(equityThresholdObj);
                }
            }
            else
            {
                Console.WriteLine($"No goal tuning found for goal: {input.Goal}");
            }

            Console.WriteLine($"Final allocation before normalization: {JsonSerializer.Serialize(finalAllocation)}");

            // Step 5: Normalize allocation to ensure it adds up to 100%
            var total = finalAllocation.Values.Sum();
            if (Math.Abs(total - 100) > 0.01)
            {
                var keyToAdjust = finalAllocation.OrderByDescending(kv => kv.Value).First().Key;
                finalAllocation[keyToAdjust] += 100 - total;
            }

            Console.WriteLine($"Final allocation after normalization: {JsonSerializer.Serialize(finalAllocation)}");
            return finalAllocation;
        }

        private string GetAgeGroup(int age)
        {
            if (age < 30) return "<30";
            if (age <= 45) return "30-45";
            if (age <= 60) return "45-60";
            return "60+";
        }

        private double GetDoubleFromObject(object obj)
        {
            if (obj is JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == JsonValueKind.Number)
                {
                    return jsonElement.GetDouble();
                }
                throw new InvalidCastException($"JsonElement is not a number: {jsonElement}");
            }

            if (obj is IConvertible convertible)
            {
                return convertible.ToDouble(null);
            }

            throw new InvalidCastException($"Unable to convert object of type {obj.GetType()} to double.");
        }
    }
}