using System.ComponentModel.DataAnnotations;
using App.Domain;

namespace Public.DTO.V1;

public class PostProjectDto
{
    [Required(ErrorMessage = "News year is required")]
    public int Year { get; set; } = default!;

    [Required(ErrorMessage = "ProjectManager field is required!")]
    public string ProjectManager { get; set; } = default!;
    
    // In Euros
    [Required(ErrorMessage = nameof(ProjectVolume) + " is required")]
    public double ProjectVolume { get; set; } = default!;

    public string? Image { get; set; }
    
    [Required]
    public List<ContentDto> Title { get; set; } = default!;
    
    [Required]
    public List<ContentDto> Body { get; set; } = default!;
    

    // TODO - seda pole ju siin vaja, sest date pannakse alles siis kui db salvestan
    //public DateTime CreatedAt { get; set; } = default!;
    
    // one project can be inside many topic areas
    // for example - programming, Java, microservices ...
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
