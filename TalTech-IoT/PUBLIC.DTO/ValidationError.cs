namespace Public.DTO;

public class ValidationError
{
    public string Parameter { get; set; } = default!;
    public string? ErrorMessage { get; set; }
    public int? Max { get; set; }
    public int? Min { get; set; }
}

/*
[
 {
 "parameter": "MessageText", 
  "errorMessage": "TOO_LONG",
  "max" : 1000,
  "min: : 5
 },
 {
 "parameter": "MessageText", 
  "errorMessage": "FORBIDDEN_CHARACTERS",
  "max" : null,
  "min: : null
 }
]
*/