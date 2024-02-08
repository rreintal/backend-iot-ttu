using System.ComponentModel;
using System.Net;

/*
General
NOT_FOUND
MISSING_TRANSLATION_VALUE -> kasutaja annab 1 contenti koos translationiga
INVALID_TRANSLATION_LANGUAGE_STRING -> kui kasutaja annab vale lang str

Middleware
INVALID_LANGUAGE_STRING @Tee middleware, mis juba enne detectib kas lang str on õige || See on controlleritele!!


TopicArea
TOPIC_AREA_CREATE_PARENT_DOES_NOT_EXIST
TOPIC_AREA_CREATE_NAME_EXISTS
general



Mail - get repo
SERIVCE_UNAVAILABLE (503)

Mail - contact us
MESSAGE_TOO_LONG -> validation 

News
MISSING_AUTHOR 
TOO_BIG_IMAGE -> validation
IMAGE_MISSING 
TOO_MANY_TOPIC_AREAS -> validation
MISISNG_TOPIC_AREA
general 


PROJECT
INVALID_YEAR -> validation (positive number, maxValue < 3000)
general

TODO: Küsi kliendi käest
MISING_PROJECT_MANAGER -> validation
MISSING_PROJECT_VOLUME -> validation











*/
namespace Public.DTO;

public class RestApiResponse
{
    public string Message { get; set; } = default!;
    public HttpStatusCode Status { get; set; }
}