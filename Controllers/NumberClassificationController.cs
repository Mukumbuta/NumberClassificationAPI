using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("api/classify-number")]
public class NumberClassificationController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ClassifyNumber([FromQuery] string number)
    {
        // Validate input: Check if it's a valid integer
        if (!int.TryParse(number, out int num))
        {
            string inputType = GetInputType(number);
            var errorResponse = new { number = inputType, error = true };

            var badRequestResult = new ObjectResult(errorResponse) { StatusCode = 400 };

            return badRequestResult;
        }

        var properties = new List<string>();
        bool isPrime = IsPrime(num);
        bool isPerfect = IsPerfect(num);
        bool isArmstrong = IsArmstrong(num);
        int digitSum = num.ToString().Sum(c => c - '0');
        string funFact = await GetFunFactAsync(num);

        if (isPrime)
            properties.Add("prime");
        if (isPerfect)
            properties.Add("perfect");
        if (isArmstrong)
            properties.Add("armstrong");
        if (num % 2 == 0)
            properties.Add("even");
        else
            properties.Add("odd");

        var successResponse = new
        {
            number = num,
            is_prime = isPrime,
            is_perfect = isPerfect,
            properties,
            digit_sum = digitSum,
            fun_fact = funFact,
        };

        var okResult = new ObjectResult(successResponse) { StatusCode = 200 };

        return okResult;
    }

    private static string GetInputType(string input)
    {
        if (input.All(char.IsLetter))
            return "alphabet";
        if (input.All(char.IsSymbol) || input.All(char.IsPunctuation))
            return "special character";
        if (input.Any(char.IsLetter) && input.Any(char.IsDigit))
            return "alphanumeric";
        return "unknown";
    }

    private static bool IsPrime(int num)
    {
        if (num < 2)
            return false;
        for (int i = 2; i * i <= num; i++)
            if (num % i == 0)
                return false;
        return true;
    }

    private static bool IsPerfect(int num)
    {
        int sum = 1;
        for (int i = 2; i * i <= num; i++)
        {
            if (num % i == 0)
            {
                sum += i;
                if (i != num / i)
                    sum += num / i;
            }
        }
        return sum == num && num != 1;
    }

    private static bool IsArmstrong(int num)
    {
        int original = num,
            sum = 0,
            digits = num.ToString().Length;
        while (num > 0)
        {
            sum += (int)Math.Pow(num % 10, digits);
            num /= 10;
        }
        return sum == original;
    }

    // private static string GetFunFact(int num, bool isArmstrong)
    // {
    //     http://numbersapi.com/#5/math
    //     if (isArmstrong)
    //         return $"{num} is an Armstrong number because its digits raised to the power sum up to itself.";
    //     if (num % 2 == 0)
    //         return $"{num} is an even number.";
    //     return $"{num} is a number with very unique properties.";
    // }

    private static readonly HttpClient client = new();

    public async Task<string> GetFunFactAsync(int number)
    {
        string url = $"http://numbersapi.com/{number}/math?json=true";

        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            // Parse the response to get the 'text' field
            var json = JsonConvert.DeserializeObject<dynamic>(responseBody);

            // Check for null after deserialization
            return json?.text ?? "No fun fact available";
        }
        catch (Exception ex)
        {
            return $"Error fetching fun fact: {ex.Message}";
        }
    }
}
