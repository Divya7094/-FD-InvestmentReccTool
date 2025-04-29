// using WisVestAPI.Models.DTOs;
// using WisVestAPI.Services.Interfaces;
// using System.Text.Json;

// namespace WisVestAPI.Services
// {
//     public class UserInputService : IUserInputService
//     {
//         private readonly IAllocationService _allocationService;

//         public UserInputService(IAllocationService allocationService)
//         {
//             _allocationService = allocationService;
//         }

//         public async Task<AllocationResultDTO> HandleUserInput(UserInputDTO input)
//         {
//             // Validate the input
//             if (input == null)
//             {
//                 throw new ArgumentNullException(nameof(input), "User input cannot be null.");
//             }

//             if (string.IsNullOrWhiteSpace(input.RiskTolerance) || 
//                 string.IsNullOrWhiteSpace(input.InvestmentHorizon) || 
//                 string.IsNullOrWhiteSpace(input.Goal))
//             {
//                 throw new ArgumentException("RiskTolerance, InvestmentHorizon, and Goal cannot be null or empty.");
//             }

//             // Calculate the allocation based on user input
//             var allocationDictionary = await _allocationService.CalculateFinalAllocation(input);

//             if (allocationDictionary == null)
//             {
//                 throw new InvalidOperationException("Allocation calculation failed.");
//             }

//             // Map the allocation to a DTO
//             var result = new AllocationResultDTO
//             {
//                 Equity = allocationDictionary.GetValueOrDefault("equity", 0.0),
//                 FixedIncome = allocationDictionary.GetValueOrDefault("fixedIncome", 0.0),
//                 Commodities = allocationDictionary.GetValueOrDefault("commodities", 0.0),
//                 Cash = allocationDictionary.GetValueOrDefault("cash", 0.0),
//                 RealEstate = allocationDictionary.GetValueOrDefault("realEstate", 0.0)
//             };

//             // (Optional) Log the serialized result for debugging
//             var jsonResult = JsonSerializer.Serialize(result);
//             Console.WriteLine($"Serialized Result: {jsonResult}");

//             return result;
//         }
//     }
// }
using WisVestAPI.Models.DTOs;
using WisVestAPI.Services.Interfaces;
using System.Text.Json;

namespace WisVestAPI.Services
{
    public class UserInputService : IUserInputService
    {
        private readonly IAllocationService _allocationService;

        public UserInputService(IAllocationService allocationService)
        {
            _allocationService = allocationService;
        }
        public async Task<AllocationResultDTO> HandleUserInput(UserInputDTO input)
{
    if (input == null)
        throw new ArgumentNullException(nameof(input), "User input cannot be null.");

    Console.WriteLine($"Received input: {JsonSerializer.Serialize(input)}");

    var allocationDictionary = await _allocationService.CalculateFinalAllocation(input);

    if (allocationDictionary == null)
        throw new InvalidOperationException("Allocation calculation failed.");

    var result = new AllocationResultDTO
    {
        Equity = allocationDictionary.GetValueOrDefault("equity", 0.0),
        FixedIncome = allocationDictionary.GetValueOrDefault("fixedIncome", 0.0),
        Commodities = allocationDictionary.GetValueOrDefault("commodities", 0.0),
        Cash = allocationDictionary.GetValueOrDefault("cash", 0.0),
        RealEstate = allocationDictionary.GetValueOrDefault("realEstate", 0.0)
    };

    Console.WriteLine($"Calculated allocation: {JsonSerializer.Serialize(result)}");

    return result;
}
        }
    }
