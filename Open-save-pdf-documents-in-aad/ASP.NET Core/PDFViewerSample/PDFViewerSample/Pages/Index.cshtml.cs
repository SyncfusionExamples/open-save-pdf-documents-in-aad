using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Syncfusion.EJ2.PdfViewer;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using System.Net;
using Azure.Identity;
using Azure.Storage.Blobs;

namespace PDFViewerSample.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private IMemoryCache _cache;
        
        string tenantId = "Provide the tenant id here";
        string clientId = "Provide the clientid here";
        string clientSecret = "Provide the client secret here";
        string blobServiceEndpoint = "https://your-storage-account.blob.core.windows.net";
        string containerName = "Provide the container name here";

        public IndexModel(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IMemoryCache cache)
        {
            _hostingEnvironment = hostingEnvironment;
            _cache = cache;
        }

        public async Task<IActionResult> OnGetLoadFromAAD(string fileName)
        {
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var blobServiceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), clientSecretCredential);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            // Download the PDF file to a local stream
            using MemoryStream pdfStream = new MemoryStream();
            await blobClient.DownloadToAsync(pdfStream);
            var base64 = Convert.ToBase64String(pdfStream.ToArray());
            return Content("data:application/pdf;base64," + base64);
        }

        public async Task<IActionResult> OnPostSaveToAAD([FromBody]jsonObjects responseData)
        {
            var jsonObject = JsonConverterstring(responseData);
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var fileName = jsonObject.ContainsKey("documentId") ? jsonObject["documentId"] : "Test.pdf";
            string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
            string convertedBase = documentBase.Substring(documentBase.LastIndexOf(',') + 1);
            // Decode the Base64 string to a byte array
            byte[] byteArray = Convert.FromBase64String(convertedBase);
            // Create a MemoryStream from the byte array
            MemoryStream stream = new MemoryStream(byteArray);
            // Create a new BlobServiceClient using the DefaultAzureCredential
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            var blobServiceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), clientSecretCredential);
            // Get a reference to the container
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            // Get a reference to the blob
            var blobClient = containerClient.GetBlobClient(fileName);
            //FileStream uploadFileStream = new FileStream();
            await blobClient.UploadAsync(stream, true);
            stream.Close();
            return Content(string.Empty);
        }

        public IActionResult OnPostLoad([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            MemoryStream stream = new MemoryStream();
            var jsonObject = JsonConverterstring(responseData);
            object jsonResult = new object();
            if (jsonObject != null && jsonObject.ContainsKey("document"))
            {
                if (bool.Parse(jsonObject["isFileName"]))
                {
                    string documentPath = GetDocumentPath(jsonObject["document"]);
                    if (!string.IsNullOrEmpty(documentPath))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(documentPath);
                        stream = new MemoryStream(bytes);
                    }
                    else
                    {
                        string fileName = jsonObject["document"].Split(new string[] { "://" }, StringSplitOptions.None)[0];
                        if (fileName == "http" || fileName == "https")
                        {
                            WebClient WebClient = new WebClient();
                            byte[] pdfDoc = WebClient.DownloadData(jsonObject["document"]);
                            stream = new MemoryStream(pdfDoc);
                        }
                        else
                            return this.Content(jsonObject["document"] + " is not found");
                    }
                }
                else
                {
                    byte[] bytes = Convert.FromBase64String(jsonObject["document"]);
                    stream = new MemoryStream(bytes);
                }
            }
            jsonResult = pdfviewer.Load(stream, jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        public Dictionary<string, string> JsonConverterstring(jsonObjects results)
        {
            Dictionary<string, object> resultObjects = new Dictionary<string, object>();
            resultObjects = results.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(results, null));
            var emptyObjects = (from kv in resultObjects
                                where kv.Value != null
                                select kv).ToDictionary(kv => kv.Key, kv => kv.Value);
            Dictionary<string, string> jsonResult = emptyObjects.ToDictionary(k => k.Key, k => k.Value.ToString());
            return jsonResult;
        }

        //Post action for processing the PDF documents.
        public IActionResult OnPostRenderPdfPages([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            object jsonResult = pdfviewer.GetPage(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        //Post action for unloading and disposing the PDF document resources
        public IActionResult OnPostUnload([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            pdfviewer.ClearCache(jsonObject);
            return this.Content("Document cache is cleared");
        }

        //Post action for rendering the ThumbnailImages
        public IActionResult OnPostRenderThumbnailImages([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            object result = pdfviewer.GetThumbnailImages(jsonObject);
            return Content(JsonConvert.SerializeObject(result));
        }

        //Post action for processing the bookmarks from the PDF documents
        public IActionResult OnPostBookmarks([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            object jsonResult = pdfviewer.GetBookmarks(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        //Post action for rendering the annotation comments
        public IActionResult OnPostRenderAnnotationComments([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            object jsonResult = pdfviewer.GetAnnotationComments(jsonObject);
            return Content(JsonConvert.SerializeObject(jsonResult));
        }

        //Post action for exporting the annotations
        public IActionResult OnPostExportAnnotations([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            string jsonResult = pdfviewer.ExportAnnotation(jsonObject);
            return Content(jsonResult);
        }

        //Post action for importing the annotations
        public IActionResult OnPostImportAnnotations([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            string jsonResult = string.Empty;
            object JsonResult;
            if (jsonObject != null && jsonObject.ContainsKey("fileName"))
            {
                string documentPath = GetDocumentPath(jsonObject["fileName"]);
                if (!string.IsNullOrEmpty(documentPath))
                {
                    jsonResult = System.IO.File.ReadAllText(documentPath);
                }
                else
                {
                    return this.Content(jsonObject["document"] + " is not found");
                }
            }
            else
            {
                string extension = Path.GetExtension(jsonObject["importedData"]);
                if (extension != ".xfdf")
                {
                    JsonResult = pdfviewer.ImportAnnotation(jsonObject);
                    return Content(JsonConvert.SerializeObject(JsonResult));
                }
                else
                {
                    string documentPath = GetDocumentPath(jsonObject["importedData"]);
                    if (!string.IsNullOrEmpty(documentPath))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(documentPath);
                        jsonObject["importedData"] = Convert.ToBase64String(bytes);
                        JsonResult = pdfviewer.ImportAnnotation(jsonObject);
                        return Content(JsonConvert.SerializeObject(JsonResult));
                    }
                    else
                    {
                        return this.Content(jsonObject["document"] + " is not found");
                    }
                }
            }
            return Content(jsonResult);
        }

        //Post action for downloading the PDF documents
        public IActionResult OnPostDownload([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
            return Content(documentBase);
        }

        //Post action for printing the PDF documents
        public IActionResult OnPostPrintImages([FromBody] jsonObjects responseData)
        {
            PdfRenderer pdfviewer = new PdfRenderer(_cache);
            var jsonObject = JsonConverterstring(responseData);
            object pageImage = pdfviewer.GetPrintImage(jsonObject);
            return Content(JsonConvert.SerializeObject(pageImage));
        }

        //Gets the path of the PDF document
        private string GetDocumentPath(string document)
        {
            string documentPath = string.Empty;
            if (!System.IO.File.Exists(document))
            {
                string basePath = _hostingEnvironment.WebRootPath;
                string dataPath = string.Empty;
                dataPath = basePath + "/";
                if (System.IO.File.Exists(dataPath + (document)))
                    documentPath = dataPath + document;
            }
            else
            {
                documentPath = document;
            }
            return documentPath;
        }
    }

    public class jsonObjects
    {
        public string document { get; set; }

        public string password { get; set; }

        public bool isClientsideLoading { get; set; }

        public string organizePages { get; set; }

        public string zoomFactor { get; set; }

        public string isFileName { get; set; }

        public string xCoordinate { get; set; }

        public string yCoordinate { get; set; }

        public string pageNumber { get; set; }

        public string documentId { get; set; }

        public string hashId { get; set; }

        public string sizeX { get; set; }

        public string sizeY { get; set; }

        public string startPage { get; set; }

        public string endPage { get; set; }

        public string stampAnnotations { get; set; }

        public string textMarkupAnnotations { get; set; }

        public string stickyNotesAnnotation { get; set; }

        public string shapeAnnotations { get; set; }

        public string measureShapeAnnotations { get; set; }

        public string action { get; set; }

        public string pageStartIndex { get; set; }

        public string pageEndIndex { get; set; }

        public string fileName { get; set; }

        public string elementId { get; set; }

        public string pdfAnnotation { get; set; }

        public string importPageList { get; set; }

        public string uniqueId { get; set; }

        public string data { get; set; }

        public string viewPortWidth { get; set; }

        public string viewportHeight { get; set; }

        public string tilecount { get; set; }

        public bool isCompletePageSizeNotReceived { get; set; }

        public string freeTextAnnotation { get; set; }

        public string signatureData { get; set; }

        public string fieldsData { get; set; }

        public string formDesigner { get; set; }

        public bool isSignatureEdited { get; set; }

        public string inkSignatureData { get; set; }

        public bool hideEmptyDigitalSignatureFields { get; set; }

        public bool showDigitalSignatureAppearance { get; set; }

        public bool digitalSignaturePresent { get; set; }

        public string tileXCount { get; set; }

        public string tileYCount { get; set; }

        public string digitalSignaturePageList { get; set; }

        public string annotationCollection { get; set; }

        public string annotationsPageList { get; set; }

        public string formFieldsPageList { get; set; }

        public bool isAnnotationsExist { get; set; }

        public bool isFormFieldAnnotationsExist { get; set; }

        public string documentLiveCount { get; set; }

        public string annotationDataFormat { get; set; }

    }
}