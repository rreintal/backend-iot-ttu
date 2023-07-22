using App.Domain;

namespace Public.DTO.V1;

public class CreateProjectDto
{
    public int Year { get; set; } = default!;

    public string ProjectManager { get; set; } = default!;
    // In Euros
    public double ProjectVolume { get; set; } = default!;
    
    public List<ContentDto> Title { get; set; } = default!;

    public List<ContentDto> Body { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = default!;
    
    // one project can be inside many topic areas
    // for example - programming, Java, microservices ...
    public List<TopicArea>? TopicAreas { get; set; }
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
