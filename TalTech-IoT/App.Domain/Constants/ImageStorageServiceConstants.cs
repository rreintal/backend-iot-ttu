namespace App.Domain.Constants;

public static class ImageStorageServiceConstants
{
    // TODO: tee kõik ENV muutujatega
    
    // "http://172.16.0.87:5000/images/upload"; // TTU server
    // "http://172.16.0.87/images/"; TTU SERVER img loc
    
    /* Contabo server
     http://66.94.110.127:5000/home/dockeradmin/imagestorage/images/
     http://66.94.110.127:5000/images/upload
     */
    public static string UPLOAD_IMAGE { get; set; } = "http://185.170.213.135:5052/images/upload"; //"http://172.16.0.87:5000/images/upload"; // "http://localhost:5052/images/upload"; 

    public static string DELETE_IMAGE { get; } = "http://185.170.213.135:5052/images/delete";//"http://172.16.0.87:5000/images/delete";
    public static string IMAGE_PUBLIC_LOCATION { get; set; } = "http://185.170.213.135:5052/images/"; //"http://172.16.0.87/images/";
}