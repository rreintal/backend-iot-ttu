using System.ComponentModel.DataAnnotations;
using App.Domain.Constants;
using Public.DTO.ValidationAttributes;

namespace Public.DTO.V1;

public class PostProjectDto
{
    [Required(ErrorMessage = RestApiErrorMessages.MissingProjectYear)]
    [MinLength(0)]
    [MaxLength(5)] // igaksjuhuks 5
    public int Year { get; set; } = default!;

    [Required(ErrorMessage = RestApiErrorMessages.MissingProjectManager)]
    [MinLength(2)]
    [MaxLength(64)]
    public string ProjectManager { get; set; } = default!;
    
    // In Euros
    [Required(ErrorMessage = RestApiErrorMessages.MissingProjectVolume)]
    [MinLength(2)]
    [MaxLength(64)]
    public double ProjectVolume { get; set; } = default!;

    public string? Image { get; set; }
    
    //[ValidCultures]
    //[IncludesAllCultures]
    public List<ContentDto> Title { get; set; } = default!;
    
    //[ValidCultures]
    //[IncludesAllCultures]
    public List<ContentDto> Body { get; set; } = default!;
    

    // TODO - seda pole ju siin vaja, sest date pannakse alles siis kui db salvestan
    //public DateTime CreatedAt { get; set; } = default!;
    
    // one project can be inside many topic areas
    // for example - programming, Java, microservices ...
    [MinLength(1, ErrorMessage = RestApiErrorMessages.GeneralMissingTopicArea)]
    public List<TopicArea> TopicAreas { get; set; } = default!;
}

/*

{
 "Year" : "2023",
 "ProjectManager" : "Mart Kivi",
 "ProjectVolume" : "1 500 000",
 "Title" : [
    {
    "Value" : "Title for project",
    "Culture" "en" 
    },
    {
    "Value" : "Projekti pealkiri",
    "Culture" "et" 
    }
 ],
 "Body": 
 [
    {
    "Value" : "Body for project",
    "Culture" "en" 
    },
    {
    "Value" : "Projekti sisu",
    "Culture" "et" 
    }
  ],
  "CreatedAt" : "23.01.2023",
  "TopicAreas" : 
  [
    {
     "Id" : "abcde-ffff-hhhh-gggg"
    },
    {
     "Id" : "gggg-ffff-aaaa-hhhh"
    },
  ]
}

*/
