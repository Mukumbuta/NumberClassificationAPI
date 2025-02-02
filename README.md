# NumberClassificationAPI

## Overview
This is an API that classifies numbers based on their mathematical properties, such as prime, perfect, Armstrong, and odd/even. It also provides a fun fact about the number. Built with C# and ASP.NET Core, the API returns structured JSON responses, handles invalid input gracefully.

## Features
- Determines if a number is prime or perfect.
- Identifies special number properties (e.g., Armstrong numbers, odd/even classification).
- Returns the sum of its digits.
- Provides a fun fact about the number.
- Handles invalid input gracefully.

## Endpoint
### GET `/api/classify-number?number={value}`

### Request Parameters
- `number` (string): The number to classify. Must be a valid integer.

### Responses
#### 200 OK
```json
{
    "number": 371,
    "is_prime": false,
    "is_perfect": false,
    "properties": ["armstrong", "odd"],
    "class_sum": 11,
    "fun_fact": "371 is an Armstrong number because 3^3 + 7^3 + 1^3 = 371"
}
```

#### 400 Bad Request
For non-numeric input:
```json
{
    "number": "alphabet",
    "error": true
}
```
For mixed input (alphanumeric, symbols, etc.):
```json
{
    "number": "special character",
    "error": true
}
```

## Status Codes
- **200 OK**: Successfully classified a valid number.
- **400 Bad Request**: Invalid input (non-numeric, alphabet, alphanumeric, or special characters).

## Installation & Running the API
1. Clone the repository:
   ```sh
   git clone git@github.com:Mukumbuta/NumberClassificationAPI.git
   ```
2. Navigate into the project directory:
   ```sh
   cd NumberClassificationAPI
   ```
3. Run the application (for ASP.NET Core):
   ```sh
   dotnet run
   ```

## Deployment
Ensure the API is deployed to a publicly accessible endpoint.

## License
This project is licensed under the MIT License.
