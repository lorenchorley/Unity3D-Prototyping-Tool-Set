using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

// https://forum.unity3d.com/threads/2-simple-scripts-to-upload-files-to-a-http-server-and-free-hosting-recommendation.144261/
// TOOD Redo as static class using Rx
public class FileUploadHelper : MonoBehaviour {

    public void StartUpload() {
        StartCoroutine(UploadLevel());
    }

    IEnumerator UploadLevel() { // Works
        //making a dummy xml level file
        XmlDocument map = new XmlDocument();
        map.LoadXml("<level></level>");

        //converting the xml to bytes to be ready for upload
        byte[] levelData = Encoding.UTF8.GetBytes(map.OuterXml);

        //generate a long random file name , to avoid duplicates and overwriting
        string fileName = Path.GetRandomFileName();
        fileName = fileName.Substring(0, 6);
        fileName = fileName.ToUpper();
        fileName = fileName + ".xml";

        //if you save the generated name, you can make people be able to retrieve the uploaded file, without the needs of listings
        //just provide the level code name , and it will retrieve it just like a qrcode or something like that, please read below the method used to validate the upload,
        //that same method is used to retrieve the just uploaded file, and validate it
        //this method is similar to the one used by the popular game bike baron
        //this method saves you from the hassle of making complex server side back ends which enlists available levels
        //this way you could enlist outstanding levels just by posting the levels code on a blog or forum, this way its easier to share, without the need of user accounts or install procedures
        WWWForm form = new WWWForm();

        Debug.Log("form created ");

        form.AddField("action", "level upload");
        form.AddField("file", "file");
        form.AddBinaryData("file", levelData, fileName, "text/xml");

        Debug.Log("binary data added ");

        //change the url to the url of the php file
        WWW w = new WWW("http://cybirddev.lorenchorley.com/testfileuploadxx.php", form);
        Debug.Log("www created");

        yield return w;
        Debug.Log("after yield w");
        if (w.error != null) {
            Debug.Log("error");
            Debug.Log(w.error);
        } else {
            //this part validates the upload, by waiting 5 seconds then trying to retrieve it from the web
            if (w.uploadProgress == 1 && w.isDone) {
                Debug.Log("done");
            }
        }
    }
}

