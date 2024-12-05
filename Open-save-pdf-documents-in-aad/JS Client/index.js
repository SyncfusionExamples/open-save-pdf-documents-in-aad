// Define custom toolbar items
var toolItem1 = {
    id: 'loadFromAAD',
    text: 'Load From AAD',
    tooltipText: 'Custom toolbar item',
    align: 'left'
  };
  
  var toolItem2 = {
    id: 'saveToAAD',
    text: 'Save To AAD',
    tooltipText: 'Custom toolbar item',
    align: 'left'
  };
  
  
  // Initialize the PDF Viewer with custom toolbar items
  var pdfviewer = new ej.pdfviewer.PdfViewer({
    serviceUrl: 'https://localhost:44308/pdfviewer',
    toolbarSettings: {
      toolbarItems: [
        toolItem1,
        toolItem2,
        'OpenOption',
        'PageNavigationTool',
        'MagnificationTool',
        'PanTool',
        'SelectionTool',
        'SearchOption',
        'PrintOption',
        'DownloadOption',
        'UndoRedoTool',
        'AnnotationEditTool',
        'FormDesignerEditTool',
        'CommentTool',
        'SubmitForm',
      ]
    }
  });
  
  // Inject required modules for PDF Viewer functionality
  ej.pdfviewer.PdfViewer.Inject(
    ej.pdfviewer.TextSelection,
    ej.pdfviewer.TextSearch,
    ej.pdfviewer.Print,
    ej.pdfviewer.Navigation,
    ej.pdfviewer.Toolbar,
    ej.pdfviewer.Magnification,
    ej.pdfviewer.Annotation,
    ej.pdfviewer.FormDesigner,
    ej.pdfviewer.FormFields
  );
  
  // Render the PDF Viewer in the DOM
  pdfviewer.appendTo('#PdfViewer');
  
  // Handle toolbar clicks
  pdfviewer.toolbarClick = function (args) {
    if (args.item && args.item.id === 'loadFromAAD') {
      // Handle 'Load From AAD' logic
      var xhr = new XMLHttpRequest();
      xhr.open('POST', `https://localhost:44308/pdfviewer/LoadFromAAD?fileName=pdf-succinctly.pdf`, true);
      xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
          var data = xhr.responseText;
          console.log(data); // Handle the response
          pdfviewer.load(data); // Load the document
        }
      };
      xhr.send();
    } else if (args.item && args.item.id === 'saveToAAD') {
      // Handle 'Save To AAD' logic
      pdfviewer.serverActionSettings.download = "SaveToAAD";
      pdfviewer.download(); // Trigger download
    }
  };
  
  // Search box icon addition
  function OnCreateSearch() {
    this.addIcon('prepend', 'e-icons e-search');
  }
  