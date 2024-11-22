import { PdfViewer, Toolbar, TextSelection, TextSearch, Print, Navigation, Magnification, Annotation, FormDesigner, FormFields, CustomToolbarItemModel } from '@syncfusion/ej2-pdfviewer';
import { ComboBox } from "@syncfusion/ej2-dropdowns";

// Inject required modules for PDF Viewer functionality
PdfViewer.Inject(
    TextSelection,
    TextSearch,
    Print,
    Navigation,
    Toolbar,
    Magnification,
    Annotation,
    FormDesigner,
    FormFields
);

// Define custom toolbar items
let toolItem1: CustomToolbarItemModel = {
    id: 'loadFromAAD',
    text: 'Load From AAD',
    tooltipText: 'Custom toolbar item',
    align: 'left'
};

let toolItem2: CustomToolbarItemModel = {
    id: 'saveToAAD',
    text: 'Save To AAD',
    tooltipText: 'Custom toolbar item',
    align: 'left'
};

// Initialize the PDF Viewer with custom toolbar items
let pdfviewer: PdfViewer = new PdfViewer({
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

// Render the PDF Viewer in the DOM
pdfviewer.appendTo('#PdfViewer');

// Handle toolbar clicks
pdfviewer.toolbarClick = (args: { item?: { id: string } }) => {
    if (args.item) {
        if (args.item.id === 'loadFromAAD') {
            // Handle 'Load From AAD' logic
            const xhr = new XMLHttpRequest();
            xhr.open('POST', `https://localhost:44308/pdfviewer/LoadFromAAD?fileName=pdf-succinctly.pdf`, true);
            xhr.onreadystatechange = () => {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    const data = xhr.responseText;
                    console.log(data); // Handle the response
                    pdfviewer.load(data,''); // Load the document
                }
            };
            xhr.send();
        } else if (args.item.id === 'saveToAAD') {
            // Handle 'Save To AAD' logic
            pdfviewer.serverActionSettings.download = "SaveToAAD";
            pdfviewer.download(); // Trigger download
        }
    }
};
