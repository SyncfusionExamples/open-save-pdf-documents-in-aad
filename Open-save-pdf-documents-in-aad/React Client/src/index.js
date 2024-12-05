import * as ReactDOM from 'react-dom';
import * as React from 'react';
import './index.css';
import  { PdfViewerComponent, Toolbar, Magnification, Navigation, LinkAnnotation, BookmarkView,
         ThumbnailView, Print, TextSelection, Annotation, TextSearch, FormFields, FormDesigner, Inject} from '@syncfusion/ej2-react-pdfviewer';     

export function App() {


  var toolItem1 = {
    text: 'LoadFromAAD',
    id: 'loadFromAAD',
    tooltipText: 'Custom toolbar item',
};
var toolItem2  = {
    id: 'saveToAAD',
    text: 'SaveToAAD',
    tooltipText: 'Custom toolbar item',
    align: 'left'
};

  function toolbarClick(args){
    var viewer = document.getElementById('container').ej2_instances[0];
    if (args.item && args.item.id === 'loadFromAAD') {
        // Handle 'Load From AAD' logic
        var xhr = new XMLHttpRequest();
        xhr.open('POST', `https://localhost:44308/pdfviewer/LoadFromAAD?fileName=pdf-succinctly.pdf`, true);
        xhr.onreadystatechange = function () {
          if (xhr.readyState === 4 && xhr.status === 200) {
            var data = xhr.responseText;
            console.log(data); // Handle the response
            viewer.load(data); // Load the document
          }
        };
        xhr.send();
      } else if (args.item && args.item.id === 'saveToAAD') {
        // Handle 'Save To AAD' logic
        viewer.serverActionSettings.download = "SaveToAAD";
        viewer.download(); // Trigger download
      }
    };
return (<div>
    <div className='control-section'>
        <PdfViewerComponent 
            id="container" 
            documentPath=""
            serviceUrl="https://localhost:44308/pdfviewer"
            toolbarSettings={{ showTooltip : true, toolbarItems: [toolItem1, toolItem2, 'OpenOption', 'PageNavigationTool', 'MagnificationTool', 'PanTool', 'SelectionTool', 'SearchOption', 'PrintOption', 'DownloadOption', 'UndoRedoTool', 'AnnotationEditTool', 'FormDesignerEditTool', 'CommentTool', 'SubmitForm']}}
            toolbarClick={toolbarClick}
            style={{ 'height': '640px' }}>
               {/* Inject the required services */}
               <Inject services={[ Toolbar, Magnification, Navigation, Annotation, LinkAnnotation, BookmarkView, ThumbnailView,
                                   Print, TextSelection, TextSearch, FormFields, FormDesigner]} />
        </PdfViewerComponent>
    </div>
</div>);
}
const root = ReactDOM.createRoot(document.getElementById('sample'));
root.render(<App />);