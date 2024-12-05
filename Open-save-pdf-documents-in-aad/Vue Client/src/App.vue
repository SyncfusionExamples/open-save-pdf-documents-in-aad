<template>
  <div id="app">
    <!-- PDF Viewer Component -->
    <ejs-pdfviewer
      id="pdfViewer"
      ref="pdfviewer"
      :documentPath="documentPath"
      :serviceUrl="serviceUrl"
      :toolbar-settings="toolbarSettings"
      @toolbar-click="toolbarClick">
    </ejs-pdfviewer>
  </div>
</template>

<script>
import { PdfViewerComponent, Toolbar, Magnification, Navigation, 
         LinkAnnotation, BookmarkView, Annotation, ThumbnailView, 
         Print, TextSelection, TextSearch, FormFields, FormDesigner, 
         PageOrganizer } from '@syncfusion/ej2-vue-pdfviewer';

export default {
  name: 'App',
  components: {
    "ejs-pdfviewer": PdfViewerComponent
  },
  data () {
    // Toolbar items configuration
    const toolItem1 = {
      text: 'Load From AAD',
      id: 'LoadFromAAD',
      tooltipText: 'Custom toolbar item',
      align: 'left'
    };
    const toolItem2 = {
      id: 'SaveToAAD',
      text: 'Save To AAD',
      tooltipText: 'Custom toolbar item',
      align: 'left'
    };

    return {
      // Path to the document
      documentPath: "",  // You can set this to an initial document URL or keep it empty
      // URL for the service (used by the Syncfusion PDF Viewer)
      serviceUrl: "https://localhost:44308/pdfviewer",
      // Toolbar settings
      toolbarSettings: {
        toolbarItems: [
          toolItem1, toolItem2, 
          'OpenOption', 'PageNavigationTool', 'MagnificationTool', 
          'PanTool', 'SelectionTool', 'SearchOption', 'PrintOption', 
          'DownloadOption', 'UndoRedoTool', 'AnnotationEditTool', 
          'FormDesignerEditTool', 'CommentTool', 'SubmitForm'
        ]
      }
    };
  },

  provide: {
    PdfViewer: [Toolbar, Magnification, Navigation, LinkAnnotation, BookmarkView, ThumbnailView, 
                Print, TextSelection, TextSearch, Annotation, FormDesigner, FormFields, PageOrganizer]
  },

  methods: {
    // Handle toolbar button clicks
    toolbarClick(args) {
      const viewer = this.$refs.pdfviewer.ej2Instances;  // Reference to the PdfViewer instance

      if (args.item) {
        if (args.item.id === 'LoadFromAAD') {
          // Handle 'Load From AAD' logic
          this.loadFromAAD(viewer);
        } else if (args.item.id === 'SaveToAAD') {
          // Handle 'Save To AAD' logic
          this.saveToAAD(viewer);
        }
      }
    },

    // Load PDF from AAD (simulated logic)
    loadFromAAD(viewer) {
      console.log('Loading PDF from AAD...');
      const xhr = new XMLHttpRequest();
      //modify the url based on the file name
      xhr.open('POST', `https://localhost:44308/pdfviewer/LoadFromAAD?fileName=pdf-succinctly.pdf`, true);
      xhr.onreadystatechange = () => {
        if (xhr.readyState === 4 && xhr.status === 200) {
          const data = xhr.responseText;
          console.log('Loaded PDF:', data);
          viewer.load(data);  // Load the document into the PDF Viewer
        } else if (xhr.readyState === 4) {
          console.error('Error loading PDF from AAD');
        }
      };
      xhr.send();
    },

    // Save PDF to AAD (simulated logic)
    saveToAAD(viewer) {
      console.log('Saving PDF to AAD...');
      viewer.serverActionSettings.download = "SaveToAAD"; // Custom download action
      viewer.download();  // Trigger the download action for saving
    }
  }
};
</script>

<style scoped>
  /* Syncfusion styles */
  @import "../node_modules/@syncfusion/ej2-base/styles/material.css";
  @import "../node_modules/@syncfusion/ej2-buttons/styles/material.css";
  @import "../node_modules/@syncfusion/ej2-dropdowns/styles/material.css";
  @import "../node_modules/@syncfusion/ej2-inputs/styles/material.css";
  @import "../node_modules/@syncfusion/ej2-navigations/styles/material.css";
  @import "../node_modules/@syncfusion/ej2-popups/styles/material.css";
  @import "../node_modules/@syncfusion/ej2-splitbuttons/styles/material.css";
  @import "../node_modules/@syncfusion/ej2-vue-pdfviewer/styles/material.css";
</style>
